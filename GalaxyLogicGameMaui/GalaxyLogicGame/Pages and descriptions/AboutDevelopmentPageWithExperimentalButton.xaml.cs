using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class AboutDevelopmentPageWithExperimentalButton : ContentPage
    {
        
        public AboutDevelopmentPageWithExperimentalButton()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            topLayout.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });

            if (Preferences.Get("isUltra", true))
            {
                if (Preferences.Get("experimental", false))
                {
                    //experimentalButton.BackgroundColor = Color.Green;
                }
                else
                {
                    //experimentalButton.BackgroundColor = Color.DarkRed;
                }
            }
            else
            {
                experimentalButton.Text = "Only in ultra";
                //experimentalButton.BackgroundColor = Color.Yellow;
                experimentalButton.TextColor = Color.FromArgb("000");
                experimentalButton.FontFamily = "BigNoodleTitling";
                experimentalButton.FontAttributes = FontAttributes.Bold;
            }
        }
        public string TitleText { set { title.Text = value; } }
        public string Text { set { text1.Text = value; } }
        public string SecondText { set { text2.Text = value; } }
        public string MainImage { set { mainImage.Source = value; } }
        public string SecondaryImage { set { secondaryImage.Source = value; } }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            if (((ScrollView)sender).ScrollY < 200)
            {
                AbsoluteLayout.SetLayoutBounds(mainImage, new Rect(0, ((ScrollView)sender).ScrollY / 2, 360, 240));
                topLayout.Opacity = 1 - (((ScrollView)sender).ScrollY / 200);
                //mainImage.Scale = 1 + (((ScrollView)sender).ScrollY / 1000);
            }
            else
            {
            }
        }

        private void OnExperimentalButtonClicked(object sender, EventArgs e)
        {
            if (Preferences.Get("isUltra", true))
            {
                Preferences.Set("experimental", !Preferences.Get("experimental", false));
                if (Preferences.Get("experimental", false))
                {
                    //experimentalButton.BackgroundColor = Color.Green;

                }
                else
                {
                    //experimentalButton.BackgroundColor = Color.DarkRed;
                }
            }
        }
    }
}