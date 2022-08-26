using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.ButtonBackgroundThings;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

[assembly: ExportFont("SamsungOne700.ttf", Alias = "SamsungOne")]

namespace GalaxyLogicGame
{

    partial class ObjectDescription : AbsoluteLayout
    {
        private IButtonBG bg;
        public ObjectDescription()
        {
            InitializeComponent();

            if (true)//(Device.RuntimePlatform == Device.Tizen)
            {
                bg = new ButtonBG { CornerRadius = 30, BackgroundColor = Color.FromHex("2f2f2f"), };
            }
            else
            {
                bg = new BoxViewBG { CornerRadius = 30, BackgroundColor = Color.FromHex("2f2f2f"), };
            }
            mainLayout.Children.Insert(0, (View)bg);
            AbsoluteLayout.SetLayoutBounds((View)bg, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags((View)bg, AbsoluteLayoutFlags.SizeProportional);

        }
        public string BlackholeSource { set { blackholeThumbnail.Source = value; } }
        public string ImageSource { set { thumbnail.Source = value; } }
        public string Title { set { title.Text = value; } get { return title.Text; } }
        public string Description { set { description.Text = value; } get { return description.Text; } }
        public AbsoluteLayout Planet => planet;
    }
}