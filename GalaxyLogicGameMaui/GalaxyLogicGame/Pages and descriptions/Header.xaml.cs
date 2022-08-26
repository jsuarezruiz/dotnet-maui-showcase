using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class Header : AbsoluteLayout
    {
        public Header()
        {
            InitializeComponent();
            this.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });
        }
        public bool InvertColors { set
            {
                if (value)
                {
                    backButton.TextColor = Color.FromArgb("000");
                    title.TextColor = Color.FromArgb("000");
                    smallTitle.TextColor = Color.FromArgb("000");
                }
            } }
        public string TitleText { set { title.Text = value; } }
        public string SmallTitleText { set { smallTitle.Text = value; smallTitle.IsVisible = true; } }
        public Color TitleColor { set { title.TextColor = value; } }
        //public Color TitleColor { set { title.TextColor = value; } }
    }
}