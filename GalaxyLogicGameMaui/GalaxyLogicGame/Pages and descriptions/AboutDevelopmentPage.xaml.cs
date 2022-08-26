using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class AboutDevelopmentPage : ContentPage
    {
        public AboutDevelopmentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            topLayout.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });


            //if (Device.RuntimePlatform != Device.Tizen) ScaleToScreen();
        }

        public bool InvertedColors { set { if (value) { title.TextColor = Color.FromArgb("000"); back.TextColor = Color.FromArgb("000"); } } }
        public string TitleText { set { title.Text = value; title.IsVisible = value != null; } }
        public string Text { set { text1.Text = value; text1.IsVisible = value != null; } }
        public string SecondText { set { text2.Text = value; text2.IsVisible = value != null; } }
        public string ThirdText { set { text3.Text = value; text3.IsVisible = value != null; } }

        public string TitleImage { set { titleImage.Source = value; titleImage.IsVisible = value != null; } }
        public string FirstImage { set { firstImage.Source = value; firstImage.IsVisible = value != null; } }

        public string SecondImage { set { secondImage.Source = value; secondImage.IsVisible = value != null; } }
        public string ThirdImage { set { thirdImage.Source = value; thirdImage.IsVisible = value != null; } }
        public string FourthImage { set { fourthImage.Source = value; fourthImage.IsVisible = value != null; } }
        public Layout FirstLayout { set { if (value != null) { firstAbsoluteLayout.IsVisible = true; 
                    firstAbsoluteLayout.Children.Add((StackLayout)value);
                    AbsoluteLayout.SetLayoutBounds((StackLayout)value, new Rect(0.5, 0, 320, AbsoluteLayout.AutoSize));
                    AbsoluteLayout.SetLayoutFlags((StackLayout)value, AbsoluteLayoutFlags.XProportional);
                } } }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            if (((ScrollView)sender).ScrollY < 200)
            {
                AbsoluteLayout.SetLayoutBounds(titleImage, new Rect(0, ((ScrollView)sender).ScrollY / 2, 360, 240));
                topLayout.Opacity = 1 - (((ScrollView)sender).ScrollY / 200);
                //mainImage.Scale = 1 + (((ScrollView)sender).ScrollY / 1000);
            }
            else
            {
            }
        }
        public void ScaleToScreen()
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            double height = displayInfo.Height;
            double density = displayInfo.Density;

            double screenWidth = width / density;
            //double screenHeight = height / density;


            //
            //double layoutAspectRatio = 1111111;
            //double screenAspectRatio = height / width;


            // put in if block
            //mainLayout.MinimumWidthRequest = 360;
            //mainLayout.WidthRequest = 360;


            double scale = screenWidth / 360;

            double stackHeight = mainLayout.Height ;
            
            if (scale > 1)
            {
                BoxView top = new BoxView { HeightRequest = (stackHeight * scale - stackHeight ) / 2 }; // change this

                //mainStackLayout.Children.Add(top);

                // Lower
                //mainStackLayout.Children.Remove(top);
                mainStackLayout.Children.Insert(0, top);

                mainStackLayout.Children.Add(new BoxView { HeightRequest = (stackHeight * scale - stackHeight) / 2 });
            }

            //title.Text = scale + "   ha" + stackHeight;

            //AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0, 0 , 320 * scale, stackHeight * scale));
            //AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(0, 0, 320 * scale, stackHeight * scale));

            mainLayout.Scale = scale;
        }
    }
}