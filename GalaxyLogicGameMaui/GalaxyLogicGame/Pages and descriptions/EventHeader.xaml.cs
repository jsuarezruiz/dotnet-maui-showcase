using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class EventHeader : AbsoluteLayout
    {
        public EventHeader()
        {
            InitializeComponent();
            this.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });

        }
        public string TitleText { set { title.Text = value; } }
        public Color TitleColor { set { title.TextColor = value; } }
        public string Icon { set { icon.Source = value; } }
    }
}