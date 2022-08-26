using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{
    public class AdFunction
    {
        public static async Task ShowAd(AbsoluteLayout layout)
        {
            AbsoluteLayout mainLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            var image = new Image
            {
                Source = "qr code300x300.png",
                Aspect = Aspect.AspectFit,
                GestureRecognizers = {
                    new TapGestureRecognizer { Command = new Command(async() => {
                        await mainLayout.FadeTo(0, 250);
                        layout.Children.Remove(mainLayout);
                        layout.IsVisible = false;
                    }), }
                }
            };
            mainLayout.Children.Add(image);
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(image, new Rect(0.5, 0.5, 260, 260));
            
            layout.IsVisible = true;
            layout.Children.Add(mainLayout);
            AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(mainLayout, AbsoluteLayoutFlags.All);
            await mainLayout.FadeTo(1, 250);
        }
    }
}
