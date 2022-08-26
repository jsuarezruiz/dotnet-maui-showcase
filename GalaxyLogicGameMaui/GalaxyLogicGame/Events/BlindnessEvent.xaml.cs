using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Events
{

    public partial class BlindnessEvent : AbsoluteLayout, IEvent, IEventObject, IEventInfo
    {
        public const string NAME = "BlindnessEvent";

        private int duration = 4;
        private GameWithEvents game;
        private Position center;
        public BlindnessEvent()
        {
            InitializeComponent();
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && (game.Gamemode == Gamemode.Clasic || game.Gamemode == Gamemode.GameJam);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Do you have a good memory? You will certainly need it!"
        };
        public bool Prerequisites(GameWithEvents game)
        {
            return IsActivated(game);
        }
        public async Task Appear(GameWithEvents game)
        {
            this.game = game;
            game.EventObject = this;
            Random random = new Random();
            game.BG.ShowEvent(mainLayout);
            game.Blindness = this;

            game.EventCounter = -5;

            // something more
            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);
            game.BG.HideAllEvents();
            game.BlindnessLayout.Children.Add(mainLayout);
            

            Position p = CirclePosition.CalculatePosition(random.Next(360), 180);

            center = p;

            blindness.TranslationX = p.X;
            blindness.TranslationY = p.Y;
            blindPlanets.TranslationX = -p.X;
            blindPlanets.TranslationY = -p.Y;

            await blindness.FadeTo(1, 500);
            await Update(game);
        }

        public async Task Update(GameWithEvents game)
        {
            await blindPlanets.FadeTo(0, 500);
            blindPlanets.Children.Clear();
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(game.Atoms.Count, game.Offset);

            for (int i = 0; i < positions.Count; i++)
            {

                Position p = (Position)positions[i];

                Image planet = new Image
                {
                    Source = "empty.png",
                    TranslationX = p.X,
                    TranslationY = p.Y,
                };

                if (p.Distance(center) > 180) planet.Opacity = 0;
                blindPlanets.Children.Add(planet);
                AbsoluteLayout.SetLayoutBounds(planet, new Rect(0.5, 0.5, 60, 60));
                AbsoluteLayout.SetLayoutFlags(planet, AbsoluteLayoutFlags.PositionProportional);

            }

            await blindPlanets.FadeTo(1, 500);

        }


        public async Task Disappear()
        {
            await blindness.FadeTo(0, 500);
            game.BlindnessLayout.Children.Clear();
            game.Blindness = null;
            game.EventObject = null;
        }
        public async Task Move()
        {
            duration--;
            if (duration == 0)
            {
                await Disappear();
            }
        }

        public Label EventTitle => eventTitle;
        public bool EventHappening { get => false; }

    }
}