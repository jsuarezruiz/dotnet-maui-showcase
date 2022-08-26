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

    public partial class TransitionIn : AbsoluteLayout
    {
        private Color[] c = { Color.FromHex("00006B"), Color.FromHex("00006B"), /*Color.Purple*/ };
        private View[] layers = new View[4];
        AbsoluteLayout layout;
        public TransitionIn()
        {
            InitializeComponent();
        }

        public async Task Play(AbsoluteLayout layout, int delay)
        {
            layout.IsVisible = true;
            this.layout = layout;
            layout.Children.Add(mainLayout);

            DisplayInfo info = DeviceDisplay.MainDisplayInfo;
            double screenWidth = info.Width / info.Density;

            double screenHeight = info.Height / info.Density;

            double diagonal = Math.Sqrt(Math.Pow(screenWidth, 2) + Math.Pow(screenHeight, 2));

            double offset = screenWidth > 360 ? screenWidth * 1.5 : 360 * 1.5;



            
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                BoxView layer = new BoxView
                {
                    BackgroundColor = c[i],
                    Opacity = 0.25,
                    Rotation = 30,
                    TranslationX = -offset,
                    TranslationY = -offset,
                    Scale = diagonal / screenWidth
                };
                layers[i] = layer;
                mainLayout.Children.Add(layer);
                AbsoluteLayout.SetLayoutBounds(layer, new Rect(0.5, 0.5, 1, 1));
                AbsoluteLayout.SetLayoutFlags(layer, AbsoluteLayoutFlags.All);
            }
            Image logo = new Image
            {
                Source = "galaxyTransition.png",
                Rotation = -30,
                TranslationX = offset,
                TranslationY = 0, 
                Scale = screenWidth / diagonal
            };
            AbsoluteLayout.SetLayoutBounds(logo, new Rect(0.5, 0.5, 0.5, 0.5));
            AbsoluteLayout.SetLayoutFlags(logo, AbsoluteLayoutFlags.All);
            AbsoluteLayout finalLayer = new AbsoluteLayout
            {
                BackgroundColor = Color.FromArgb("000"),
                //BackgroundColor = Color.FromHex("2f2f2f"),
                Rotation = 30,
                TranslationX = -offset,
                TranslationY = -offset,
                Scale = diagonal / screenWidth
            };
            finalLayer.Children.Add(new BoxView());
            finalLayer.Children.Add(logo);
            layers[layers.Count() - 1] = finalLayer;
            mainLayout.Children.Add(finalLayer);
            AbsoluteLayout.SetLayoutBounds(finalLayer, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(finalLayer, AbsoluteLayoutFlags.All);
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                layers[i].TranslateTo(0, 0, (uint)delay);
                await Task.Delay(delay / 3);
            }
            layers[layers.Count() - 1].TranslateTo(0, 0, (uint)delay);
            logo.TranslateTo(0, 0, (uint)delay);
            await Task.Delay(delay + 50);
            
            
        } 
        public void Stop()
        {
            layout.IsVisible = false;
            layout.Children.Remove(mainLayout);
        }
        public void StopContinuous()
        {
            layout.Children.Remove(mainLayout);
        }
    }
}