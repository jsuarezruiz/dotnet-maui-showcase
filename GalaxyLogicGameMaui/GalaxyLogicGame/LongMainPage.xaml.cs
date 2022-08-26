using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Tutorial;
using GalaxyLogicGame.Types;
//using MarcTron.Plugin;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Mobile
{
    public partial class LongMainPage : ContentPage, IMainPage
    {
        public const bool IS_ULTRA = true;

        public const string COMPLETED_TUTORIAL = "org.tizen.myApp.completedTutorial";
        public const string FIRST = "org.tizen.myApp.first";
        public const string HIGHSCORE = "org.tizen.myApp.highscore";

        private StarsParticlesLayout stars;


        public IGameBG gameBG;
        private bool clicked = true;
        //private TutorialGameplay tutorial;
        private View[] buttonsArray = new View[7];
        public LongMainPage(StarsParticlesLayout stars)
        {


            //tutorial = new TutorialGameplay();
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);

            /*
            buttonsArray[0] = playButton;
            buttonsArray[1] = playCustom;
            buttonsArray[2] = challengesButton;
            buttonsArray[3] = aboutEventsButton;
            buttonsArray[4] = objectsDescriptionButton;
            buttonsArray[5] = aboutDevelopmentButton;
            buttonsArray[6] = tutorialButton;
            buttonsArray[7] = creditsButton;
            */

            buttonsArray[0] = playButton;
            buttonsArray[1] = playGameJam;
            buttonsArray[2] = challengesButton;
            buttonsArray[3] = aboutEventsButton;
            buttonsArray[4] = objectsDescriptionButton;
            buttonsArray[5] = tutorialButton;
            buttonsArray[6] = creditsButton;

            // Set animation




            SetUpButtons();

            this.stars = stars;
            starsLayout.Children.Add(stars);

            SizeChanged += OnDisplaySizeChanged;
            //buttonsLayout.TranslateTo(0, 0, 500, Easing.SinOut);

            //if (!Preferences.Get("experimental", false)) highscoreLabel.Text = "Highscore: " + Preferences.Get(HIGHSCORE, 0);
            //else highscoreLabel.Text = "Experimental mode";


            //scroll.ScrollToAsync(-126, -24, true);
        }

        
        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }

        private void SetUpButtons()
        {
            double ratio = Functions.GetScreenRatio();

            for (int i = 0; i < buttonsArray.Length; i++)
            {
                buttonsArray[i].TranslationY = 360 * ratio * mainLayout.Scale;
            }
        }
        async void OnPlayClicked(object sender, EventArgs args)
        {
            await NavigateToGame(new GameBG(), new CasualGame());
        }
        async void OnPlayEventfulClicked(object sender, EventArgs args)
        {
            await NavigateToGame(new GameBG(), new GameWithEvents());
        }
        async void OnAboutEventfulClicked(object sender, EventArgs args)
        {
            if (clicked)
            {
                clicked = false;
                await TransitionOut(new AboutEventfulGame());
                clicked = true;
            }
        }
        async void OnChallengesClicked(object sender, EventArgs args)
        {
            if (clicked)
            {
                clicked = false;
                await TransitionOut(new ChallengesPage());
                clicked = true;
            }
        }
        async void OnObjectsInfoClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new TutorialPage(true));
            if (clicked)
            {
                clicked = false;
                await TransitionOut(new TutorialPage(false));
                clicked = true;
            }
        }
        async void OnAboutDevelopmentClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new TutorialPage(true));
            /*
            if (clicked)
            {
                clicked = false;
                try
                {
                    AppControl appcontrol = new AppControl();

                    appcontrol.Operation = AppControlOperations.Default;
                    appcontrol.ApplicationId = "org.tizen.example.AboutDevelopment69";


                    //Preferences.Set("launch", "aboutDevelopment");
                    //Preferences.Set("launch", "aboutDevelopment");

                    //appcontrol.ExtraData.Add("launchPart", "aboutDevelopment");

                    AppControl.SendLaunchRequest(appcontrol);
                } catch (Exception ex)
                {
                    await Navigation.PushAsync(new AboutDevelopmentMissing());
                }
                clicked = true;
            }
            */
        }
        async void OnTutorialClicked(object sender, EventArgs args)
        {
            //CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-1496107080258172/2983887804");
            await NavigateToGame(new GameBG(Color.FromHex("222")), new TutorialPlacingPlanets());
        }
        
        async void OnCustomModeClicked(object sender, EventArgs args)
        {

            /*
            if (clicked)
            {
                clicked = false;
                await Navigation.PushAsync(new CustomSelectionPage(gameBG, this));
                clicked = true;
            }
            */
        }


        private async Task NavigateToGame(GameBG gameBG, CasualGame game)
        {
            if (clicked)
            {
                clicked = false;


                //TransitionIn transition = new TransitionIn();
                //await transition.Play(transitionLayout, 250);


                this.gameBG?.StopLoop();
                this.gameBG = gameBG;
                gameBG.StartLoop();

                game.BG = gameBG;
                game.MainMenuPage = this;

                await Navigation.PushAsync((ContentPage)gameBG, false);
                await gameBG.Setup(game);
                clicked = true;


                //transition.Stop();
            }
        }

        void OnScrolled(object sender, EventArgs args)
        {
            AbsoluteLayout.SetLayoutBounds(transitionLayout, new Rect(0, scroll.ScrollY, 360, 360));
            //AbsoluteLayout.SetLayoutBounds(wallpaper, new Rect(0, scroll.ScrollY - 360, 360, 720));
        }
        public int Highscore { get { return Preferences.Get(HIGHSCORE, 0); } set { Preferences.Set(HIGHSCORE, value); /*highscoreLabel.Text = "Highscore: " + value;*/ } }
        public string HighscoreLabel { get { return Preferences.Get(HIGHSCORE, 0).ToString(); } set { /*highscoreLabel.Text = value;*/ } }

        public bool IsUltra => true;



        private async Task TransitionOut(IStarsPage page)
        {
            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            double ratio = display.Height / display.Width > 1 ? display.Height / display.Width : 1;

            await Task.WhenAll(
                this.stars.TransitionUpIn(),

                mainLayout.TranslateTo(0, -360 * ratio, 500, Easing.SinIn),

                //wallpaper.TranslateTo(0, -180, 500, Easing.SinIn),
                wallpaper.FadeTo(0, 500, Easing.SinIn));

            starsLayout.Children.Remove(this.stars);
            page.AssingStars(stars);
            await Navigation.PushAsync((Page)page, false);
            await page.TransitionIn();

            mainLayout.TranslationX = 0;
            mainLayout.TranslationY = 0;
            wallpaper.Opacity = 1;

            this.stars = new StarsParticlesLayout();
            starsLayout.Children.Add(this.stars);
        }

        public async Task TransitionIn()
        {
            Functions.ScaleToScrollView(this, scroll, buttonsLayout);

            await Task.WhenAll(
                stars.TransitionUpOut(),
                wallpaper.TranslateTo(0, 0, 500, Easing.SinOut),
                wallpaper.FadeTo(1, 500, Easing.SinOut));
            for (int i = 0; i < buttonsArray.Length; i++)
            {
                buttonsArray[i].TranslateTo(0, 0, 500, Easing.SpringOut);
                buttonsArray[i].FadeTo(1, 500, Easing.SinIn);
                await Task.Delay(150);
            }
            await Task.Delay(350);


            starsLayout.Children.Remove(this.stars);
            this.stars = new StarsParticlesLayout();
            starsLayout.Children.Add(this.stars);
        }

        private async void OnCreditsClicked(object sender, EventArgs e)
        {
            if (clicked)
            {
                clicked = false;
                await TransitionOut(new CreditsPage());
                clicked = true;
            }
        }

        private async void OnPlayGameJamClicked(object sender, EventArgs e)
        {
            await NavigateToGame(new GameBG(), new GameWithEvents(Gamemode.GameJam));
        }
    }
}