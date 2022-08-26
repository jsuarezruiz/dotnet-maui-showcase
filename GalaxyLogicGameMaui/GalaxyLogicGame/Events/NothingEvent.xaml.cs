using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class NothingEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "Nothing";

        public NothingEvent()
        {
            InitializeComponent();
        }
        
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return true; //!game.CustomMode || Preferences.Get(NAME, true);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Nothing happens"
        };
        public bool Prerequisites(GameWithEvents game)
        {
            return true; //IsActivated(game);
        }
        public async Task Appear(GameWithEvents game)
        {
            //game.BG.ShowEvent(mainLayout);
            //await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);

            //game.BG.HideAllEvents();
        }
        public bool EventHappening => false; 

    }
}