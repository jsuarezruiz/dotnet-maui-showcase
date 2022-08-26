using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class AstronautEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "AstrounautEvent";
        public AstronautEvent()
        {
            InitializeComponent();
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Why is that astronaut flying over your galaxy right now? You will have to discover it by your self."
        };
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && (game.Gamemode == Gamemode.Clasic || game.Gamemode == Gamemode.GameJam);
        }
        public bool Prerequisites(GameWithEvents game)
        {
            return IsActivated(game);
        }
        public async Task Appear(GameWithEvents game)
        {
            Random random = new Random();

            game.BG.ShowEvent(mainLayout);

            Astronaut astronaut = new Astronaut();
            astronaut.Appear(game.BG);

            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);
            await Task.Delay(500);
            await this.FadeTo(0, 500);
            game.BG.HideAllEvents();
        }
        public bool EventHappening { get => false; }

    }
}