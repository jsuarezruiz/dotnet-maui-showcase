using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Particles
{

    public partial class TransitionOut : AbsoluteLayout
    {
        Color[] c = { Color.FromHex("00006B"), Color.FromHex("00006B"), Color.FromArgb("7800b0") };
        private View[] layers = new View[4];
        private Image logo;
        AbsoluteLayout layout;
        public TransitionOut()
        {
            InitializeComponent();
        }

        public void Set(AbsoluteLayout layout)
        {
            layout.IsVisible = true;
            this.layout = layout;
            layout.Children.Add(mainLayout);
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                View layer = new BoxView
                {
                    BackgroundColor = c[i],
                    Opacity = 0.25,
                    Rotation = 45,
                };
                layers[i] = layer;
                mainLayout.Children.Add(layer);
                AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(0, 0, 360, 360));
            }
            logo = new Image
            {
                Source = "galaxy transition.png",
                Rotation = -45,
                TranslationX = 0,
                TranslationY = 0,
            };
            AbsoluteLayout.SetLayoutBounds(logo, new Rect(0.5, 0.5, 180, 180));
            AbsoluteLayout.SetLayoutFlags(logo, AbsoluteLayoutFlags.PositionProportional);
            View finalLayer = new AbsoluteLayout
            {
                BackgroundColor = Color.FromArgb("000"),
                //BackgroundColor = Color.FromHex("2f2f2f"),
                Rotation = 45,
                TranslationX = 0,
                TranslationY = 0,
            };
            ((AbsoluteLayout)finalLayer).Children.Add(new BoxView());
            ((AbsoluteLayout)finalLayer).Children.Add(logo);

            layers[layers.Count() - 1] = finalLayer;
            mainLayout.Children.Add(finalLayer);
            AbsoluteLayout.SetLayoutBounds(finalLayer, new Rect(0, 0, 360, 360));
        }

        public async Task Play(int delay)
        {
            layers[layers.Count() - 1].TranslateTo(360, 360, (uint)delay);
            logo.TranslateTo(-360, 0, (uint)delay);
            await Task.Delay(delay / 3);

            for (int i = layers.Count() - 2; i >= 0; i--)
            {
                layers[i].TranslateTo(360, 360, (uint)delay);
                if (i > 0) await Task.Delay(delay / 3);
            }
            
            await Task.Delay(delay);
        }
        public void Stop()
        {
            layout.IsVisible = false;
            layout.Children.Remove(mainLayout);
        }
    }
}