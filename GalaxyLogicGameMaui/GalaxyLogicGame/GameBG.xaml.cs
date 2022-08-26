using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Events.EventChallengesBoards;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Powerups;
using GalaxyLogicGame.Tutorial;
//using MarcTron.Plugin;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ExportFont("SamsungOne700.ttf", Alias = "SamsungOne")]
[assembly: ExportFont("bignoodletitling.ttf", Alias = "BigNoodleTitling")]

namespace GalaxyLogicGame.Mobile
{
    public partial class GameBG : GameBGBase, IGameBG
    {
        private List<PowerupBase> powerups = new List<PowerupBase>();
        //private Accelerometer _accelerometer;
        /*private CasualGame game;
        private IBoard board;
        private Astronaut astronaut;

        private int time = 0;
        private const int TIME_LIMIT = 15000; // => 5 minutes (15000 ticks)*/

        private SensorSpeed accelerometerSpeed = SensorSpeed.Game;
        private Queue<(float x, float y)> _positions = new Queue<(float, float)>();

        private bool looping = false;
        
        private CasualGame game;
        private IBoard board;
        private Astronaut astronaut;
        private bool gameOver = false;

        private int time = 0;
        private const int TIME_LIMIT = 15000; // => 5 minutes (15000 ticks)

        private bool isTutorial = false;

        private bool powerupsAllowed = true;
        public GameBG()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            //CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-1496107080258172/5205854320");

            SizeChanged += OnDisplaySizeChanged;
            Accelerometer.ReadingChanged += Loop;

            Functions.ScaleToScreen(this, mainLayout, 720);
            ScaleUIToWatch();
        }
        public GameBG(Color bg)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SizeChanged += OnDisplaySizeChanged;

            isTutorial = true;
            this.BackgroundColor = bg;
            powerupsLayout.IsVisible = false;

            Accelerometer.ReadingChanged += Loop;
            Functions.ScaleToScreen(this, mainLayout, 720);
        }
        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, mainLayout, 720);
        }

        private void ScaleUIToWatch()
        {
            if (!Functions.IsSquareScreen()) return;

            AbsoluteLayout.SetLayoutBounds(lowerUILayout, new Rect(0.5, 0.5, 360, 360));

            {
                Rect bounds = AbsoluteLayout.GetLayoutBounds(scoreLabel);
                bounds.Y = 0.75;
                AbsoluteLayout.SetLayoutBounds(scoreLabel, bounds);
            }

            Image[] images = { empty1, empty2, empty3, filled1, filled2, filled3 };
            foreach (Image image in images){
                Rect bounds = AbsoluteLayout.GetLayoutBounds(image);
                bounds.Y += 55;
                AbsoluteLayout.SetLayoutBounds(image, bounds);
            }
        }

        public void GameOver()
        {
            if (game.Atoms.Count > game.Limit) // game over
            {
                //if (!isTutorial) CrossMTAdmob.Current.ShowInterstitial();
                gameOver = true;
                /*
                await LostScreenAnimation();


                if (game.Score > game.MainMenuPage?.Highscore && !Preferences.Get("experimental", false))
                {
                    game.MainMenuPage.Highscore = game.Score;
                }
                else if (Preferences.Get("experimental", false))
                {
                    game.MainMenuPage.HighscoreLabel = "Experimental mode";
                }
                */
                //                                                                      <-- add something more later
            }
            else if (game.Atoms.Count > game.Limit - 3)
            {
                gameOver = false;

                game.PlusWaiting++;
                empty1.IsVisible = true;
                empty2.IsVisible = true;
                empty3.IsVisible = true;
                if (game.Atoms.Count == game.Limit - 2)
                {
                    filled1.IsVisible = true;
                    filled2.IsVisible = false;
                    filled3.IsVisible = false;
                }
                else if (game.Atoms.Count == game.Limit - 1)
                {
                    filled1.IsVisible = true;
                    filled2.IsVisible = true;
                    filled3.IsVisible = false;
                }
                else if (game.Atoms.Count == game.Limit)
                {
                    filled1.IsVisible = true;
                    filled2.IsVisible = true;
                    filled3.IsVisible = true;
                }
            }
            else
            {
                gameOver = false;

                empty1.IsVisible = false;
                empty2.IsVisible = false;
                empty3.IsVisible = false;

                filled1.IsVisible = false;
                filled2.IsVisible = false;
                filled3.IsVisible = false;
            }
        }

        public async Task LostScreenAnimation()
        {
            if (gameOver)
            {
                lostScreen.Opacity = 0;
                lostScreenText.Opacity = 0;
                lostScreenButton.Opacity = 0;

                lostScreen.IsVisible = true;
                lostScreenText.IsVisible = true;
                lostScreenButton.IsVisible = true;

                await Task.WhenAll(
                    lostScreen.FadeTo(1, 500),
                    lostScreenText.FadeTo(1, 500),
                    lostScreenButton.FadeTo(1, 500));


                // saving a Highscore
                if (game.Score > game.MainMenuPage?.Highscore && !Preferences.Get("experimental", false))
                {
                    game.MainMenuPage.Highscore = game.Score;
                }
                else if (Preferences.Get("experimental", false))
                {
                    game.MainMenuPage.HighscoreLabel = "Experimental mode";
                }
            }
        }
        

        public async Task Setup(CasualGame game)
        {
            // Transition
            TransitionOut transition = new TransitionOut();
            transition.Set(transitionLayout);

            // Setting references
            this.game = game;
            gameLayout.Children.Add(game);

            SetupPowerupsLayout();

            // setting layouts
            lostScreen.IsVisible = false;
            lostScreenText.IsVisible = false;
            lostScreenButton.IsVisible = false;

            game.SetupBase();

            await transition.Play(250);
            transition.Stop();

            await game.Setup();
        }
        private void SetupPowerupsLayout()
        {
            if (Functions.IsSquareScreen())
            {
                powerupsLayout.IsVisible = false;
                
            }
            else
            {
                if (AtomicBomb.Equiped)
                {
                    AtomicBomb atomicBomb = new AtomicBomb
                    {
                        BG = this,
                    };
                    powerupsLayout.Children.Add(atomicBomb);
                    powerups.Add(atomicBomb);
                }
                    
            }
        }
        public void StartLoop()
        {
            //if (_accelerometer == null || !_accelerometer.IsSensing) InitializeAccelerometer();
            try
            {
                if (!looping)
                {
                    Accelerometer.Start(accelerometerSpeed);
                    looping = true;
                }
            } catch (FeatureNotSupportedException fnsEx)
            {
                NoAccelerometerLoop();
                // Feature not supported on device
            } catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        private async Task NoAccelerometerLoop()
        {
            looping = true;
            while (looping)
            {
                await Task.Delay(16);

                if (astronaut != null) astronaut.Move();

                if (game != null) game.LoopTick();

            }

        }
        private void Loop(object sender, AccelerometerChangedEventArgs e)
        {
            if (astronaut != null) astronaut.Move();
            // time
            //UpdateTime(); // implement later

            // Background Image
            var data = e.Reading;
            // making the movement smoother - taken from: https://tizenschool.org/tutorial/145/contents/7
            _positions.Enqueue((data.Acceleration.X, data.Acceleration.Y));
            if (_positions.Count > 10)
                _positions.Dequeue();

            float xAverage = _positions.Average(item => item.x);
            float yAverage = _positions.Average(item => item.y);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rect(0.5 - xAverage * 0.2, 0.5 + yAverage * 0.2, 480, 480));

            //if (Preferences.Get("experimental", false)) UpdatePowerupArrow(); // implement later

            // other animations
            if (game != null) game.LoopTick();
        }
        public void StopLoop()
        {
            try
            {
                Accelerometer.Stop();
            } catch
            {

            }
            looping = false;
        }


        /*
        
        public void OnAccelerometerDataUpdated(object sender, AccelerometerDataUpdatedEventArgs e)
        {

            float x = (e.X + 10) / 20;
            float y = (e.Y + 10) / 20;
            // making the movement smoother - taken from: https://tizenschool.org/tutorial/145/contents/7
            _positions.Enqueue((x, y));
            if (_positions.Count > 10)
                _positions.Dequeue();

            float xAverage = _positions.Average(item => item.x);
            float yAverage = _positions.Average(item => item.y);

            var position = new Rect(xAverage, 1 - yAverage, 480, 480);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, position);

            if (astronaut != null) astronaut.Move();
            // time
            UpdateTime();


            if (Preferences.Get("experimental", false)) UpdatePowerupArrow();

            // other animations
            if (game != null) game.LoopTick();

        }
        */



        private void UpdateTime()
        {
            time++;

            if (board != null && time % 250 == 0) board.ChangeAppearance();

            if (time >= TIME_LIMIT)
            {
                /*
                PauseScreen pauseScreen = new PauseScreen();
                pauseScreen.Appear(this);
                */
            }
        }

        private void UpdatePowerupArrow()
        {
            /*
            if (!powerupArrows.HasAppeared && time > 250)
            {
                ChangeArrowVisibility(true);
                powerupArrows.Play();
            }
            */
        }
        private async Task ChangeArrowVisibility(bool visibility)
        {
            /*
            powerupArrows.HasAppeared = visibility;

            // animation
            if (visibility) await Task.WhenAll(
               powerupArrows.FadeTo(1, 500),
               scoreLabel.FadeTo(0, 500));
            else await Task.WhenAll(
                powerupArrows.FadeTo(0, 500),
               scoreLabel.FadeTo(1, 500));
            */

        }
        private async void PowerupArrowsClicked(object sender, EventArgs args)
        {
            /*
            if (!Preferences.Get("experimental", false)) return;
            PowerupView powerups = new PowerupView();

            await powerups.Appear(this);
            */
        }

        public void ResetTime()
        {
            time = 0;
            ChangeArrowVisibility(false);
        }
        public async Task ShowTelescopeView()
        {
            /*
            TelescopeView view = new TelescopeView();
            await view.Appear(this);
            */
        }




        public void UpdateScore()
        {
            scoreLabel.Text = game.Score.ToString();
        }
        public async Task NavigateToOtherTutorial(TutorialGameBase game)
        {
            //TransitionIn transition = new TransitionIn();

            //await transition.Play(transitionLayout, 250);

            this.game = game;
            game.BG = this;

            gameLayout.Children.Clear();
            gameLayout.Children.Add(game);
            game.SetupBase();



            await game.Setup();

            //TransitionOut transitionOut = new TransitionOut();
            //transitionOut.Set(transitionLayout);
            //transition.StopContinuous();

            //await transitionOut.Play(250);
            //transition.Stop();
        }
        private async void ToMainMenu(object sender, EventArgs args)
        {
            if (isTutorial)
            {
                Preferences.Set("tutorialCompleted", true);
                //CrossMTAdmob.Current.ShowInterstitial();
            }

            await Navigation.PopAsync();
        }

        public void ShowEvent(AbsoluteLayout layout)
        {
            eventLayout.IsVisible = true;
            eventLayout.Children.Add(layout);
        }
        public void HideAllEvents()
        {
            eventLayout.IsVisible = false;
            eventLayout.Children.Clear();
        }
        public AbsoluteLayout LowerEventLayout { get { return lowerEventLayout; } }

        public AbsoluteLayout LowerUILayout { get { return lowerUILayout; } set { lowerUILayout = value; } }
        public AbsoluteLayout MainLayout { get { return mainLayout; } set { mainLayout = value; } }

        public AbsoluteLayout BackgroundLayout { get => backgroundLayout; set { backgroundLayout = value; } }
        public AbsoluteLayout EventLayout => eventLayout;
        public Image BackgroundImage { get => backgroundImage; }
        public AbsoluteLayout TransitionLayout { get => transitionLayout; set { transitionLayout = value; } }
        public AbsoluteLayout LostScreen { get => lostScreen; }
        public Label LostScreenText { get => lostScreenText; }
        public Label LostScreenButton { get => lostScreenButton; }
        public AbsoluteLayout TutorialLayout { get => tutorialLayout; }
        public AbsoluteLayout GameLayout { get => gameLayout; }
        public CasualGame Game => game;
        public IBoard Board { set { board = value; } }
        public Astronaut Astronaut { set { astronaut = value; } }
        public bool PowerupsAllowed { set { /* .. */ } }
        public List<PowerupBase> Powerups { get => powerups; }
        public AbsoluteLayout PowerUpAnimationLayout => throw new NotImplementedException();
    }

}
