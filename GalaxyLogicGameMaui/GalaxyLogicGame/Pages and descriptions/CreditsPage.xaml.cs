using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Particles;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class CreditsPage : ContentPage, IStarsPage
    {
        private StarsParticlesLayout stars;

        public CreditsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);


            DateTime now = DateTime.Today;
            int bonus = -1;
            if (now.Month > 3 || (now.Month == 3 && now.Day >= 15)) bonus = 0;

            ageLabel.Text = "- " + (now.Year - 2004 + bonus).ToString() + " year old student at Gymnázium Jaroslava Heyrovského";
            SizeChanged += OnDisplaySizeChanged;

        }
        public void AssingStars(StarsParticlesLayout stars)
        {
            this.stars = stars;
            starsLayout.Children.Add(stars);
        }

        public async Task TransitionIn()
        {
            await Task.WhenAll(
                wallpaper.TranslateTo(0, 0, 500, Easing.SinOut),
                wallpaper.FadeTo(1, 500, Easing.SinOut),
                stars.TransitionUpOut(),
                mainLayout.TranslateTo(0, 0, 500, Easing.SinOut),
                mainLayout.FadeTo(1, 500));

            starsLayout.Children.Clear();
        }

        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }
    }
}