using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Events.EventChallengesBoards;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class BinaryEvent : AbsoluteLayout, IEvent, IEventObject, IEventInfo
    {
        public const string NAME = "BinaryEvent";

        private int duration = 3;
        private string[] formation = new string[4];
        private const int offset = 72;
        private GameWithEvents game;
        private BinaryBoard board = new BinaryBoard();
        private bool eventHappening = true;
        public BinaryEvent()
        {
            InitializeComponent();
        }
        public Label EventTitle => eventTitle;
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return game.CustomMode && Preferences.Get(NAME, true);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "This is a legacy event available only in custom gamemode."
        };
        public bool Prerequisites(GameWithEvents game)
        {
            return game.Atoms.Count > 4 && IsActivated(game);
        }

        public async Task Appear(GameWithEvents game)
        {
            game.EventHappening = true;
            Random random = new Random();
            this.game = game;
            game.EventObject = this;
            game.Binary = this;
            game.BG.ShowEvent(this);


            // something more - IDK what actually
            await Task.WhenAll(
                Functions.EventTitleAnimation(eventTitle, eventIcon)
                /*,choiceLayout.FadeTo(0.5, 500)*/
                , darken.FadeTo(1, 500));

            await choiceLayout.FadeTo(1, 500);

            // turning Binary on
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                ((IPlanet)game.Atoms[i]).Binary.IsVisible = true;

                // Do a custom animation
            }

            // generationg a new formation
            for(int i = 0; i < formation.Length; i++)
            {
                formation[i] = random.Next(0, 2).ToString();
            }

            // showing the screen
            for (int i = 0; i < formation.Length; i++)
            {
                BinaryIndicator binary = new BinaryIndicator
                {
                    TranslationX = -180 + offset * (i + 1),
                    TranslationY = 0,
                    Scale = 0, // 1
                    Opacity = 1, // 0
                    IsVisible = true,
                    BinaryString = formation[i]
                };

                choiceLayout.Children.Add(binary);

                await binary.ScaleTo(1, 250, Easing.SpringOut); // 500
                await Task.Delay(75); //200
            }
        }

        private async Task HideBinary()
        {
            /*
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                ((IPlanet)game.Atoms[i]).Binary.FadeTo(0, 500);
            }
            if (game.NewPlanet != null) game.NewPlanet.Binary.FadeTo(0, 500);
            await Task.Delay(600);
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                ((IPlanet)game.Atoms[i]).Binary.IsVisible = false;
                ((IPlanet)game.Atoms[i]).Binary.Opacity = 1;
            }
            if (game.NewPlanet != null)
            {
                game.NewPlanet.Binary.IsVisible = false;
                game.NewPlanet.Binary.Opacity = 1;
            }
            */

            for (int i = 0; i < game.Atoms.Count; i++)
            {
                ((IPlanet)game.Atoms[i]).Binary.IsVisible = false;
            }
            if (game.NewPlanet != null)
            {
                game.NewPlanet.Binary.IsVisible = false;
            }
        }

        public async Task<bool> CheckFormation()
        {
            if (game.Atoms.Count < 4) return false;
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                if (((IPlanet)game.Atoms[i]).Binary.BinaryString.Equals(formation[0]))
                {
                    for (int j = 1; j < formation.Length; j++)
                    {
                        // successful
                        if (((IPlanet)game.Atoms[(i + j) % game.Atoms.Count]).Binary.BinaryString.Equals(formation[j])) if (j == 3) { await ChallengeCompleted(); return true; } else { }
                        // unsuccessful
                        else break;
                    }
                }
            }
            
            for (int i = game.Atoms.Count - 1; i >= 0; i--)
            {
                if (((IPlanet)game.Atoms[i]).Binary.BinaryString.Equals(formation[0]))
                {
                    for (int j = 1; j < formation.Length; j++)
                    {
                        // successful
                        int temp = i - j;
                        if (temp < 0) temp += game.Atoms.Count;
                        if (((IPlanet)game.Atoms[temp]).Binary.BinaryString.Equals(formation[j])) if (j == 3) { await ChallengeCompleted(); return true; } else { }
                        // unsuccessful
                        else break;
                    }
                }
            }
            return false;
        }

        public async Task Move()
        {
            duration--;
            board.MovesLeft = duration;
            if (duration == 0)
            {
                board.Disappear(game.BG);

                await Unsuccessful();
            }
        }

        public async Task ChallengeCompleted()
        {
            // Special popup that you completed the challenge
            game.Binary = null;
            game.EventObject = null;

            await Task.WhenAll(
                FullscreenTitlePopup.Appear(game.BG.MainLayout, "Event succeeded", Color.FromArgb("ff0"), 0.5),
                HideBinary(),
                board.Disappear(game.BG));
        }
        public async Task Unsuccessful()
        {
            game.Binary = null;
            game.EventObject = null;

            await Task.WhenAll(
                FullscreenTitlePopup.Appear(game.BG.MainLayout, "Event failed", Color.FromArgb("f00"), 0.5),
                HideBinary(),
                board.Disappear(game.BG));

            game.NewPlanet = new Debris();
            game.Debris.Add(game.NewPlanet);
            if (game.Debris.Count >= 2 && game.MainMenuPage.IsUltra) await Challenges.Challenge11(game.BG.MainLayout);
            game.AtomsLayout.Children.Add(game.NewPlanet);
            game.GenerateNewPlanet = false;
            game.AddClickableAreasToLayout();
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            await Task.WhenAll(
                choiceLayout.FadeTo(0, 500),
                darken.FadeTo(0, 500));
            game.BG.HideAllEvents();
            game.BlindnessLayout.Children.Add(this);
            

            game.EventHappening = false;
            eventHappening = false;
            eventTitle.FadeTo(0, 250);
            if (await CheckFormation()) return;


            await board.Appear(game.BG, formation);
        }

        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BinaryInfoPage());
        }
        public bool EventHappening { get => eventHappening; }

    }
}