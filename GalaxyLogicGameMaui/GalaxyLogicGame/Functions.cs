using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{
    public class Functions
    {
        public static int GetAtomValue(PlanetBase p)
        {
            return int.Parse(p.Text);
        }


        public static Color[] GetColors()
        {
            Color[] colors = { Color.FromArgb("FF87CEEB"), Color.FromArgb("FF66CDAA"), Color.FromArgb("FAFAD2"), Color.FromArgb("F4A460"), Color.FromArgb("6495ED"), Color.FromArgb("FF2E8B57"), Color.FromArgb("FF4500"), Color.FromArgb("008080"),
                Color.FromArgb("FF9370DB"), Color.FromArgb("FFEE82EE"), Color.FromArgb("ffa500"), Color.FromArgb("FF32CD32"),
                Color.FromArgb("FF483D8B"), Color.FromArgb("FFADD8E6"), Color.FromArgb("ffff00"), Color.FromArgb("FFFF8C00"), Color.FromArgb("FFF08080"), Color.FromArgb("FFFF69B4"), 
            
                //lateColors
                Color.FromArgb("000080"), Color.FromArgb("FF8B0000"), Color.FromArgb("FF8B008B"), Color.FromArgb("FFE6E6FA"), Color.FromArgb("FFD2691E"),
                Color.FromArgb("FF006400"), Color.FromArgb("FF1E90FF"), Color.FromArgb("FF2F4F4F"), Color.FromArgb("FF98FB98"), Color.FromArgb("FFD2B48C"), Color.FromArgb("ff66ff"),
                Color.FromArgb("FFDC143C"), Color.FromArgb("FF7FFFD4"), Color.FromArgb("00ff00"), Color.FromArgb("800080") };
            return colors;
        }
        public static Color GetColor(int index)
        {
            Color[] colors = { Color.FromArgb("FF87CEEB"), Color.FromArgb("FF66CDAA"), Color.FromArgb("FAFAD2"), Color.FromArgb("F4A460"), Color.FromArgb("6495ED"), Color.FromArgb("FF2E8B57"), Color.FromArgb("FF4500"), Color.FromArgb("008080"),
                Color.FromArgb("FF9370DB"), Color.FromArgb("FFEE82EE"), Color.FromArgb("ffa500"), Color.FromArgb("FF32CD32"),
                Color.FromArgb("FF483D8B"), Color.FromArgb("FFADD8E6"), Color.FromArgb("ffff00"), Color.FromArgb("FFFF8C00"), Color.FromArgb("FFF08080"), Color.FromArgb("FFFF69B4"), 
            
                //lateColors
                Color.FromArgb("000080"), Color.FromArgb("FF8B0000"), Color.FromArgb("FF8B008B"), Color.FromArgb("FFE6E6FA"), Color.FromArgb("FFD2691E"),
                Color.FromArgb("FF006400"), Color.FromArgb("FF1E90FF"), Color.FromArgb("FF2F4F4F"), Color.FromArgb("FF98FB98"), Color.FromArgb("FFD2B48C"), Color.FromArgb("ff66ff"),
                Color.FromArgb("FFDC143C"), Color.FromArgb("FF7FFFD4"), Color.FromArgb("00ff00"), Color.FromArgb("800080") };
            if (index < colors.Length) return colors[index];
            else return Color.FromArgb("fff");
        }
        public static Color[] TransitionColors { get { Color[] c = { Color.FromHex("00006B"), Color.FromHex("00006B"), Color.FromArgb("7800b0") }; return c; } }


        public static Color DetermineWhiteOrBlack(Color bgcolor)
        {
            if ((bgcolor.Red + bgcolor.Green + bgcolor.Blue) < 1.5)
            {
                return Color.FromArgb("fff");
            }
            else
            {
                return Color.FromArgb("000");
            }
        }
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Abs(x1 + x2) * Math.Abs(x1 + x2) + Math.Abs(y1 + y2) * Math.Abs(y1 + y2));
        }

        //public static double CalculateRation() { }

        public static async Task EventTitleAnimation(Label title, Image icon, AbsoluteLayout darken)
        {
            await Task.WhenAll(
                darken.FadeTo(1, 500),
                title.FadeTo(1, 500),
                icon.FadeTo(1, 500));
            await Task.Delay(500);
            icon.FadeTo(0, 500);
            await Task.Delay(250);
            if (Device.RuntimePlatform == Device.Tizen || IsSquareScreen())
            {
                await Task.WhenAll(
                    title.TranslateTo(0, -150, 250, Easing.SinIn),
                    title.ScaleTo(0.5, 250),
                    darken.FadeTo(0, 250));
            }
            else
            {
                await Task.WhenAll(
                    title.TranslateTo(0, 205, 500, Easing.SpringOut),
                    title.ScaleTo(1.2, 500),
                    darken.FadeTo(0, 250));
            }


        }
        public static void ScaleToScrollView(Page page, ScrollView scroll, Layout mainLayout)
        {
            Size size = scroll.ContentSize;

            if (!IsSquareScreen() && size.Height >= mainLayout.Height)
            {
                AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                AbsoluteLayout.SetLayoutFlags(mainLayout, AbsoluteLayoutFlags.PositionProportional);
            }

            /*
            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            double screenHeight = display.Height / display.Density;
            double screenWidth = display.Width / display.Density;

            Size size = scroll.ContentSize;

            double scale = size.Width / 360.0;
            double edge = 0;
            if (screenHeight / screenWidth - scroll.Height / scroll.Width < 0.15 && !IsSquareScreen()) edge = (screenHeight - scroll.Height) / scale;

            mainLayout.Scale = scale;
            if (size.Height > mainLayout.Height || IsSquareScreen()) AbsoluteLayout.SetLayoutBounds(mainLayout,
                new Rect((size.Width - 360) / 2.0, (size.Height - mainLayout.Height) / 2.0, 360, AbsoluteLayout.AutoSize));
            else AbsoluteLayout.SetLayoutBounds(mainLayout,
                new Rect((size.Width - 360) / 2.0, (mainLayout.Height * scale - mainLayout.Height) / 2.0 + edge, 360, AbsoluteLayout.AutoSize));

            // Scaling for phones

            if (!IsSquareScreen() && size.Height <= mainLayout.Height)
            {
                mainLayout.Scale = 1;
                Rect bounds = AbsoluteLayout.GetLayoutBounds(mainLayout);
                AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(bounds.X, 0, bounds.Width, bounds.Height));
            }
            */
        }


        public static void ScaleToScreen(Page page, Layout mainLayout, int heightParameter)
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            double height = displayInfo.Height;
            double density = displayInfo.Density;

            double screenWidth = width / density;
            double screenHeight = height / density;

            mainLayout.Scale = screenWidth / 360;



            if (!IsSquareScreen()) ScaleToHeight(page, mainLayout, heightParameter);

            FillScreenWidth(mainLayout, heightParameter);

            if (IsSquareScreen() || screenWidth < 360)
            {
                page.ContainerArea = new Rect(-(360 - screenWidth) / 2, -(360 - screenHeight) / 2, 360, 360);
            }
            else if ((DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.UWP) && screenHeight < 720)
            {
                page.ContainerArea = new Rect(-(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Height * heightParameter - screenWidth) / 2, -(heightParameter - screenHeight) / 2, DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Height * heightParameter, heightParameter);
            }
            else
            {
                page.ContainerArea = new Rect(0, 0, screenWidth, screenHeight);
            }

        }

        private static void FillScreenWidth(Layout mainLayout, int heightParameter)
        {
            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            {
                Rect bounds = AbsoluteLayout.GetLayoutBounds(mainLayout);
                bounds.Width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Height * heightParameter * mainLayout.Scale;

                AbsoluteLayout.SetLayoutBounds(mainLayout, bounds);
            }
        }

        public static void ScaleToScreen(Page page, Layout mainLayout)
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            double height = displayInfo.Height;
            if (displayInfo.Orientation == DisplayOrientation.Landscape)
            {
                width = displayInfo.Height;
                height = displayInfo.Height;
            }
            double density = displayInfo.Density;

            double screenWidth = width / density;
            double screenHeight = height / density;

            if (IsSquareScreen() || screenWidth < 360)
            {
                page.ContainerArea = new Rect(-(360 - screenWidth) / 2, -(360 - screenHeight) / 2, 360, 360);
            }

            mainLayout.Scale = screenWidth / 360;

            /*if (screenAspectRatio > layoutAspectRatio)
            {
                mainLayout.Scale = screenWidth / 360;
            }
            else mainLayout.Scale = screenHeight / 720;*/
            // put in if block


        }

        public static void FillHeight(Layout mainLayout)
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            double height = displayInfo.Height;
            if (displayInfo.Orientation == DisplayOrientation.Landscape)
            {
                width = displayInfo.Height;
                height = displayInfo.Height;
            }

            double screenAspectRatio = height / width;
            Rect bounds = AbsoluteLayout.GetLayoutBounds(mainLayout);
            bounds.Height = 360 * screenAspectRatio;
            AbsoluteLayout.SetLayoutBounds(mainLayout, bounds);
        }

        public static void ScaleToHeight(Page page, Layout mainLayout, int height)
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            //double height;
            double displayHeight = displayInfo.Height;
            double density = displayInfo.Density;

            double screenWidth = width / density;
            double screenHeight = displayHeight / density;
            double screenAspectRatio = height / width;

            if (screenHeight / 720 < screenWidth / 360) mainLayout.Scale = screenHeight / 720;
        }

        public static bool IsSquareScreen()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            return mainDisplayInfo.Width == mainDisplayInfo.Height;
        }
        public static double GetScreenRatio()
        {
            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            return display.Height / display.Width;
        }

        public static async Task EventTitleAnimation(Label title, Image icon)
        {
            await Task.WhenAll(
                title.FadeTo(1, 500),
                icon.FadeTo(1, 500));
            await Task.Delay(500);
            icon.FadeTo(0, 500);
            await Task.Delay(250);
            if (Device.RuntimePlatform == Device.Tizen || IsSquareScreen())
            {
                await Task.WhenAll(
                    title.TranslateTo(0, -150, 250, Easing.SinIn),
                    title.ScaleTo(0.5, 250));
            }
            else
            {
                await Task.WhenAll(
                    title.TranslateTo(0, 205, 500, Easing.SpringOut),
                    title.ScaleTo(1.2, 500));
            }
        }
    }
}
