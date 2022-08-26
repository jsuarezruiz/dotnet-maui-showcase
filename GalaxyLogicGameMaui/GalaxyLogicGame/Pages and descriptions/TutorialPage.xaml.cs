using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

[assembly: ExportFont("SamsungOne700.ttf", Alias = "SamsungOne")]

namespace GalaxyLogicGame
{

    public partial class TutorialPage : ContentPage, IStarsPage
    {
        private StarsParticlesLayout stars;

        public TutorialPage(bool showTutorialGameplay)
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();


            Planet planet = new Planet
            {
                Type = 0,
                Text = "5",
            };
            ObjectDescription planetDescription = new ObjectDescription
            {
                Title = "Planet",
                Description = "The most common thing in this game. By merging them, you earn score and move forward."
            };
            planetDescription.Planet.Children.Add(planet);
            AbsoluteLayout.SetLayoutBounds(planet, new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(planet, AbsoluteLayoutFlags.PositionProportional);


            Planet plus = new Planet
            {
                Type = 1,
                Text = "+",
                BGColor = Color.FromArgb("f00"),
                TextColor = Color.FromArgb("fff"),
            };
            PlanetBase starPlanet = new Supernova
            {
                Type = 2,
                Text = "3",
            };
            ObjectDescription plusDescription = new ObjectDescription
            {
                //ImageSource = "",
                Title = "Merging planets",
                Description = "If it is between two planets with the same value, they will merge and create a bigger one."
            };
            plusDescription.Planet.Children.Add(plus);
            AbsoluteLayout.SetLayoutBounds(plus, new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(plus, AbsoluteLayoutFlags.PositionProportional);

            ObjectDescription blackhole = new ObjectDescription
            {
                BlackholeSource = "blackholeThumbnail.png",
                Title = "Blackhole",
                Description = "Cast a blackhole onto a planet and it will teleport back to you, so that you can change it's position."
            };

            ObjectDescription star = new ObjectDescription
            {    
                Title = "Supernova",
                Description = "You have got only 3 moves before the supernova destroys everything around it. Don't worry, there are ways to stop it."
            };
            star.Planet.Children.Add(starPlanet);
            AbsoluteLayout.SetLayoutBounds(starPlanet, new Rect(0.5, 0.5, 70, 70));
            AbsoluteLayout.SetLayoutFlags(starPlanet, AbsoluteLayoutFlags.PositionProportional);



            Planet shrinkingGiantPlanet = new Planet
            {
                Type = 3,
                Text = "18",
                TextColor = Color.FromArgb("be66ed"),
                BGColor = Color.FromArgb("000"),
            };
            ObjectDescription shrinkingGiant = new ObjectDescription
            {
                Title = "Shrinking giant",
                Description = "A super rare planet. It can only appear when star (supernova) disappears."
            };
            shrinkingGiant.Planet.Children.Add(shrinkingGiantPlanet);
            AbsoluteLayout.SetLayoutBounds(shrinkingGiantPlanet, new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(shrinkingGiantPlanet, AbsoluteLayoutFlags.PositionProportional);
            //Debris debrisObject = new Debris { };
            ObjectDescription debris = new ObjectDescription
            {
                BlackholeSource = "debrisThumbnail.png",
                Title = "Debris",
                Description = "A total trash. It does nothing, it only interferes with other planets."
            };

            Blueberry blueberryPlanet = new Blueberry();
            ObjectDescription blueberry = new ObjectDescription
            {
                Title = "Blueberry",
                Description = "You may at first compare this to debris, however, in a few turns, it can surprise you by doing something usefull."
            };
            blueberry.Planet.Children.Add(blueberryPlanet);
            AbsoluteLayout.SetLayoutBounds(blueberryPlanet, new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(blueberryPlanet, AbsoluteLayoutFlags.PositionProportional);
            //debris.Planet.Children.Add(debrisObject, new Rect(.5, .5, 60, 60), AbsoluteLayoutFlags.PositionProportional);

            stackLayout.Children.Add(planetDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(plusDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(blackhole);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(star);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(shrinkingGiant);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(debris);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(blueberry);




            //stackLayout.Children.Add(new Label { Text = " " });
            //stackLayout.Children.Add(new Label { Text = "+1 more object coming", TextColor = Color.White, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center});
            stackLayout.Children.Add(new BoxView { HeightRequest = 120 });

            /*
            if (showTutorialGameplay)
            {
                Button tutorialGameplayButton = new Button
                {
                    Text = "Show tutorial"
                };
                tutorialGameplayButton.Clicked += OnTutorialGameplayClicked;
                stackLayout.Children.Add(new Label { Text = " " });
                stackLayout.Children.Add(tutorialGameplayButton);
            }
            */

            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
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

        async void OnTutorialGameplayClicked(object sender, EventArgs args)
        {
            // ..
        }
    }
}