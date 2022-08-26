using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{
    public class FullscreenTitlePopup
    {
        public static async Task Appear(AbsoluteLayout mainLayout, string titleText, Color titleColor, double yPosition)
        {
            AbsoluteLayout layout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            Label title = new Label
            {
                Text = titleText,
                TextColor = titleColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "SamsungOne"
            };
            layout.Children.Add(title);
            AbsoluteLayout.SetLayoutBounds(title, new Rect(0.5, yPosition, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(title, AbsoluteLayoutFlags.PositionProportional);

            mainLayout.Children.Add(layout);
            AbsoluteLayout.SetLayoutBounds(layout, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);

            await layout.FadeTo(1, 500);
            await Task.Delay(1500);
            await layout.FadeTo(0, 500);

            mainLayout.Children.Remove(layout);
        }
    }
}
