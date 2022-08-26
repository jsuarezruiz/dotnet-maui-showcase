using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ExportFont("SamsungOne700.ttf", Alias = "SamsungOne")]

namespace GalaxyLogicGame
{

    public partial class ChallengesPage : ContentPage, IStarsPage
    {
        private StarsParticlesLayout stars;
        public ChallengesPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();


            /*
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "6 next to each other", Description = "Have 6 planets with the same value next to each other", Completed = Preferences.Get("org.tizen.myApp.challenge1", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Star disappearance", Description = "Make a star dissapear", Completed = Preferences.Get("org.tizen.myApp.challenge2", false) });
            stackLayout.Children.Add(new ChallengeDescriptionWithPowerup { DifficultyLevel = 1, Title = "The beginnings", Description = "Reach score of 500", Completed = Preferences.Get("org.tizen.myApp.challenge3", false),
                PowerupTitle = "Telescope", Color = Color.Blue, Icon = "telescopeFrame.png", TitleImage = "telescopeThumbnail.png",
                PowerupDescription = "Look into the distance and see next 5 planets you are going to play. It is awesome for pulling off big combos! This power-up has a one huge side-effect, it postpones the upcoming event by 5 moves. But you can take this to your advantage, if you figure out how."});
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Scoring high", Description = "Reach score of 1000", Completed = Preferences.Get("org.tizen.myApp.challenge4", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "To the moon!", Description = "Reach score of 2000", Completed = Preferences.Get("org.tizen.myApp.challenge5", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Bigger than Jupiter", Description = "Get a planet with value 15", Completed = Preferences.Get("org.tizen.myApp.challenge6", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Bigger than the Sun", Description = "Get a planet with value 20", Completed = Preferences.Get("org.tizen.myApp.challenge7", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "Bigger than a blackhole", Description = "Get a planet with value 25", Completed = Preferences.Get("org.tizen.myApp.challenge8", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Huge galaxy", Description = "All planets in your galaxy must have two-digit values", Completed = Preferences.Get("org.tizen.myApp.challenge9", false) });
            stackLayout.Children.Add(new ChallengeDescriptionWithPowerup { DifficultyLevel = 2, Title = "The end?", Description = "Make a galaxy empty somehow", Completed = Preferences.Get("org.tizen.myApp.challenge10", false),
                PowerupTitle = "Kindle", Color = Color.Orange, Icon = "kindleFrame.png", TitleImage = "kindleThumbnail.png",
                PowerupDescription = "Use this to get rid of a blindness effect. There is nothing else to it, really."});
            //stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "This is trash", Description = "Have 3 debris in your galaxy", Completed = Preferences.Get("org.tizen.myApp.challenge11", false) });
            stackLayout.Children.Add(new ChallengeDescriptionWithPowerup { DifficultyLevel = 3, Title = "This is trash", Description = "Have 2 debris in your galaxy", Completed = Preferences.Get("org.tizen.myApp.challenge11", false),
                PowerupTitle = "Atomic bomb", Color = Color.LightGreen, Icon = "atomicBombFrame.png", TitleImage = "atomicBombThumbnail.png",
                PowerupDescription = "Activates Atomic bomb event. It is especially useful when you want to get rid of a debris.",
                PowerupDescriptionLayout = new EventDescription { Icon = "atomicBomb.png", Title = "AtomicBomb", TitleColor = Color.LightGreen, Description = "Tap on a planet to detonate it. Can be activated only by a power-up."}
            });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Wait for it to disappear", Description = "Wait a few rounds till the shrinking giant reaches 1 and can not shrink further.", Completed = Preferences.Get("org.tizen.myApp.challenge12", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Combo", Description = "Combine 4 planet pairs in one move.", Completed = Preferences.Get("org.tizen.myApp.challenge13", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Jumbo combo", Description = "Combine 6 planet pairs in one move. Show us how to play the game!", Completed = Preferences.Get("org.tizen.myApp.challenge14", false) });
            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Dustman", Description = "Use atomic bomb power-up to get rid of a debris", Completed = Preferences.Get("org.tizen.myApp.challenge15", false) });
            */


            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "6 next to each other", Description = "Have 6 planets with the same value next to each other", Completed = Preferences.Get("org.tizen.myApp.challenge1", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Star disappearance", Description = "Make a star dissapear", Completed = Preferences.Get("org.tizen.myApp.challenge2", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });
            
            stackLayout.Children.Add(new ChallengeDescription
            {
                DifficultyLevel = 1, Title = "The beginnings", Description = "Reach score of 500", Completed = Preferences.Get("org.tizen.myApp.challenge3", false),
            });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Scoring high", Description = "Reach score of 1000", Completed = Preferences.Get("org.tizen.myApp.challenge4", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "To the moon!", Description = "Reach score of 2000", Completed = Preferences.Get("org.tizen.myApp.challenge5", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Bigger than Jupiter", Description = "Get a planet with value 15", Completed = Preferences.Get("org.tizen.myApp.challenge6", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Bigger than the Sun", Description = "Get a planet with value 20", Completed = Preferences.Get("org.tizen.myApp.challenge7", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "Bigger than a blackhole", Description = "Get a planet with value 25", Completed = Preferences.Get("org.tizen.myApp.challenge8", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Huge galaxy", Description = "All planets in your galaxy must have two-digit values", Completed = Preferences.Get("org.tizen.myApp.challenge9", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription
            {
                DifficultyLevel = 2, Title = "The end?", Description = "Make a galaxy empty somehow", Completed = Preferences.Get("org.tizen.myApp.challenge10", false)
            });
            //stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 3, Title = "This is trash", Description = "Have 3 debris in your galaxy", Completed = Preferences.Get("org.tizen.myApp.challenge11", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription
            {
                DifficultyLevel = 3, Title = "This is trash", Description = "Have 2 debris in your galaxy", Completed = Preferences.Get("org.tizen.myApp.challenge11", false)
            });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Wait for it to disappear", Description = "Wait a few rounds till the shrinking giant reaches 1 and can not shrink further.", Completed = Preferences.Get("org.tizen.myApp.challenge12", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Combo", Description = "Combine 4 planet pairs in one move.", Completed = Preferences.Get("org.tizen.myApp.challenge13", false) });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 2, Title = "Jumbo combo", Description = "Combine 6 planet pairs in one move. Show us how to play the game!", Completed = Preferences.Get("org.tizen.myApp.challenge14", false) });

            //stackLayout.Children.Add(new ChallengeDescription { DifficultyLevel = 1, Title = "Dustman", Description = "Use atomic bomb power-up to get rid of a debris", Completed = Preferences.Get("org.tizen.myApp.challenge15", false) });


            stackLayout.Children.Add(new BoxView { HeightRequest = 120 }); 


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
    }
}