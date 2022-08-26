using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class ThreeInRowEvent : AbsoluteLayout, IEvent, IEventObject, IEventInfo
    {
        public const string NAME = "ThreeInRowEvent";

        private GameWithEvents game;
        private int duration = 3;
        private int value;
        
        public ThreeInRowEvent()
        {
            InitializeComponent();
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public Label EventTitle => eventTitle;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && (game.Gamemode == Gamemode.Clasic || game.Gamemode == Gamemode.GameJam);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Get three identical planets in a row. At least you know what is coming - game over maybe? You will soon understand. ^^"
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

            game.TIR = this;
            value = random.Next(game.Lowest, game.Heighest);
            game.EventCounter = -2;

            game.TIRNewPlanet();

            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);
            game.BG.HideAllEvents();
            game.BlindnessLayout.Children.Add(mainLayout);
            
        }
        public async Task Disappear()
        {

            await mainLayout.FadeTo(0, 250);
            game.BlindnessLayout.Children.Remove(mainLayout);
            game.TIR = null;
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
        public int Value => value;
        public bool EventHappening { get => false; }

    }
}