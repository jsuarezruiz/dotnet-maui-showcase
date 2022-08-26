using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Pages_and_descriptions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class AtomicBombEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "AtomicBombEvent";

        public AtomicBombEvent()
        {
            InitializeComponent();
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Tap on a planet to detonate it. Can be activated only by a power-up."
        };
        private bool IsActivated(GameWithEvents game)
        {
            return game.CustomMode && Preferences.Get(NAME, true);
        }
        public bool Prerequisites(GameWithEvents game)
        {
            if (game.Atoms.Count >= 2)
            {
                return IsActivated(game);
            }
            else return false;
        }
        public async Task Appear(GameWithEvents game)
        {
            await Appear((CasualGame)game);
        }
        public async Task Appear(CasualGame game)
        {
            game.ClickableAreaLayout.Children.Clear();
            game.ClickableAreas.Clear();
            //game.BG.ShowEvent(mainLayout);

            game.BG.LowerEventLayout.Children.Add(this);

            if (game.Blackholes.Count == 0 && game.NewPlanet != null)
            {
                if (game.NewPlanet.Type == 0) game.MiddleParticle.Stop(250);
                await game.NewPlanet.FadeTo(0, 500);
                game.AtomsLayout.Children.Remove(game.NewPlanet);
            }
            else
            {
                await ((Blackhole)game.Blackholes[0]).FadeTo(0, 500);
                game.BlackholeLayout.Children.Remove((Blackhole)game.Blackholes[0]);
            }
            if (game is GameWithEvents)
            {
                if (((GameWithEvents)game).TIR != null) ((GameWithEvents)game).TIR.FadeTo(0, 500);
                else if (((GameWithEvents)game).EventObject != null) ((GameWithEvents)game).EventObject.EventTitle.FadeTo(0, 500);
            }
            await Task.WhenAll(
                Functions.EventTitleAnimation(eventTitle, eventIcon)
                /*,choiceLayout.FadeTo(0.5, 500)*/
                , darken.FadeTo(1, 500),
                game.BG.EventLayout.FadeTo(0, 500));

            //game.BG.HideAllEvents();

            await choiceLayout.FadeTo(1, 500);


            game.ClickableAreaLayout.Children.Clear();
            game.ClickableAreas.Clear();
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(game.Atoms.Count, game.Offset);
            for (int i = 0; i < positions.Count; i++)
            {
                BoxViewWithIndex area = new BoxViewWithIndex
                {
                    Opacity = 0, // change this to see the touch areas
                    Index = i,
                };

                area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnClicked(game, area.Index); }) });
                game.ClickableAreas.Add(area);

                //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
                Position p = (Position)positions[i];

                //AbsoluteLayout.SetLayoutBounds(area, );
                //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

                game.ClickableAreaLayout.Children.Add(area);
                AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
            }
            game.Clicked = true; // escaping the barrier
        }

        private async Task OnClicked(CasualGame game, int index)
        {
            if (game.Clicked)
            {
                game.Clicked = false;

                // calculating offset
                game.DegreClicked = game.Offset + index * game.Degre;
                game.Degre = CirclePosition.CalculateDegre(game.Atoms.Count - 1);
                game.TempIndex = (game.Atoms.Count - index - 1) % game.Atoms.Count;


                // A special animation
                PlanetDetonationParticle particle = new PlanetDetonationParticle();
                particle.TranslationX = ((PlanetBase)game.Atoms[index]).TranslationX;
                particle.TranslationY = ((PlanetBase)game.Atoms[index]).TranslationY;
                particle.Play(((PlanetBase)game.Atoms[index]).BGColor, game.AtomsLayout);
                game.AtomsLayout.Children.Remove((PlanetBase)game.Atoms[index]);
                if (((PlanetBase)game.Atoms[index]).Type == 4 && game.MainMenuPage.IsUltra) await Challenges.Challenge15(game.BG.MainLayout);
                game.Atoms.RemoveAt(index);

                await Task.Delay(500);

                // Calculating offset
                game.Offset = (game.Degre * game.TempIndex + game.DegreClicked + game.Degre / 2) % 360;

                await game.MoveAtoms();
                await game.MergeAtoms();
                game.UpdateShrinkingGiantsArray();

                // change this, if there are more "castable" objects
                if (game.Blackholes.Count == 0 && game.NewPlanet != null)
                {
                    if (game.NewPlanet.Type == 0) game.MiddleParticle.Play();
                    game.AtomsLayout.Children.Add(game.NewPlanet);
                    game.NewPlanet.FadeTo(1, 500);
                    game.AddClickableAreasToLayout();
                }
                else
                {
                    game.BlackholeLayout.Children.Add((Blackhole)game.Blackholes[0]);
                    ((Blackhole)game.Blackholes[0]).FadeTo(1, 500);
                    game.AddBlackholeAreas();
                }
                if (game is GameWithEvents)
                {
                    if (((GameWithEvents)game).TIR != null) ((GameWithEvents)game).TIR.FadeTo(1, 500);
                    else if (((GameWithEvents)game).EventObject != null) ((GameWithEvents)game).EventObject.EventTitle.FadeTo(1, 500);
                }

                // add a radioactive thing

                await Task.WhenAll(
                    darken.FadeTo(0, 500),
                    choiceLayout.FadeTo(0, 500),
                    eventTitle.FadeTo(0, 500),
                    game.BG.EventLayout.FadeTo(1, 500));
                game.BG.LowerEventLayout.Children.Remove(this);

                game.Clicked = true;
            }
        }
        public bool EventHappening { get => false; }

        //public IGameBG BG { set { bg = value; } }
    }
}