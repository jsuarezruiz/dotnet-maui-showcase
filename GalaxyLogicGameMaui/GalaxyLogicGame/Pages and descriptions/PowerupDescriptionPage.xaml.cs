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

    public partial class PowerupDescriptionPage : ContentPage
    {
        public PowerupDescriptionPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            topLayout.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });

            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
            SizeChanged += OnDisplaySizeChanged;

        }
        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }
        public string TitleImage { set { titleImage.Source = value; titleImage.IsVisible = value != null; } }
        public bool InvertedColors { set { if (value) { title.TextColor = Color.FromArgb("000"); back.TextColor = Color.FromArgb("000"); } } }
        public string TitleText { set { title.Text = value; title.IsVisible = value != null; } }
        public string Text { set { text1.Text = value; text1.IsVisible = value != null; } }
        public string SecondText { set { text2.Text = value; text2.IsVisible = value != null; } }
        public string ThirdText { set { text3.Text = value; text3.IsVisible = value != null; } }

        //public string TitleImage { set { titleImage.Source = value; titleImage.IsVisible = value != null; } }
        //public string VideoSource { set { video.Source = value; } }
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
        /*
        void Loop(object sender, EventArgs args)
        {
            video.Seek(1);
            video.Start();
        }
        */
        
        //public string TitleText { set { title.Text = value; } }
        public Color Color { set { /*title.TextColor = value; iconBackground.BackgroundColor = value;*/ } }
        public string Description { set { text1.Text = value; text1.IsVisible = true; } }
        public string Icon { set { /*icon.Source = value;*/ } }
        public void AddLayout(AbsoluteLayout layout)
        {
            stackLayout.Children.Add(layout);

            // Raise
            stackLayout.Children.Remove(footer);
            stackLayout.Children.Add(footer);

        }
    }
}