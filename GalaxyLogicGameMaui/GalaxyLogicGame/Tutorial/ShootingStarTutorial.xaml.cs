using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Tutorial
{

    public partial class ShootingStarTutorial : AbsoluteLayout
    {

        private bool eventHappening = true;
        private PlanetBase[] planetsChoice = new PlanetBase[3];
        public ShootingStarTutorial()
        {
            InitializeComponent();
        }

        public async Task Appear(CasualGame game)
        {
            

            int offsetBetweenPlanets = 90;
            Random random = new Random();

            game.BG.ShowEvent(mainLayout);


            // something more
            await Task.WhenAll(
                Functions.EventTitleAnimation(eventTitle, eventIcon)
                /*,choiceLayout.FadeTo(0.5, 500)*/
                , darken.FadeTo(1, 500));

            await choiceLayout.FadeTo(1, 500);

            // 3 random planets
            for (int i = 0; i < 3; i++)
            {
                PlanetBase planet;


                if (i == 0)
                {
                    planet = new Planet
                    {
                        Type = 1,
                        Text = "+",
                        BGColor = Color.FromArgb("f00"),
                        TextColor = Color.FromArgb("fff"),
                    };
                }
                else if (i == 1)
                {
                    planet = new Planet
                    {
                        Type = 0,
                        Text = "1",
                    };

                }
                else
                {
                    planet = new Planet
                    {
                        Type = 0,
                        Text = "3",
                    };

                }



                planet.TranslationX = 360;

                planetsChoice[i] = planet;
                choiceLayout.Children.Add(planet);

                planet.TranslateTo((i - 1) * offsetBetweenPlanets, 0, 500, Easing.SpringOut);
                await Task.Delay(200);                
            }
            await Task.Delay(350);
            protectiveLayout.IsVisible = false;
            game.Clicked = true; // escaping the barrier
        }
        public async Task OnClicked(TutorialEvents game, int index, ClickableAreaWithParticles area)
        {
            if (game.Clicked)
            {

                game.Clicked = false;
                game.NewPlanet = planetsChoice[index];
                choiceLayout.Children.Remove(planetsChoice[index]);
                game.AtomsLayout.Children.Add(planetsChoice[index]);
                await mainLayout.FadeTo(0, 500);
                await planetsChoice[index].TranslateTo(0, 0, 250);

                game.BG.HideAllEvents();

                game.PlusWaiting++;
                game.StarCountdown = true;
                if (planetsChoice[index].Type == 0) game.MiddleParticle.Play();
                //else if (planetsChoice[index].Type == 1) ;
                else if (planetsChoice[index].Type == 2) { game.StarCountdown = false; game.Stars.Add(planetsChoice[index]); }

                //await game.UpdateShrinkingGiants();
                game.RemoveClickableArea(area);

                await game.TutorialText("Now, thanks to choosing the right planet, you are able to make an epic combo and clear the board.", 3);
                eventHappening = false;
                game.Clicked = true;
            }
        }
        public bool EventHappening { get => eventHappening; }

    }
}