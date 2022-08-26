using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Events
{

    public partial class ShootingStarEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "ShootingStarEvent";

        private bool eventHappening = true;
        private PlanetBase[] planetsChoice = new PlanetBase[3];
        public ShootingStarEvent()
        {
            InitializeComponent();
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && (game.Gamemode == Gamemode.Clasic || game.Gamemode == Gamemode.GameJam);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Make a wish and hope for the best choices."
        };
        public bool Prerequisites(GameWithEvents game)
        {
            return IsActivated(game);
        }
        public async Task Appear(GameWithEvents game)
        {
            game.GenerateNewPlanet = false;
            game.EventHappening = true;

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
                    int type = random.Next(1, 3);
                   

                    if(type == 2 && game.Stars.Count > 0) type--;
                    if (type == 1)
                    {
                        planet = new Planet
                        {
                            Type = 1,
                            Text = "+",
                            BGColor = Color.FromArgb("f00"),
                            TextColor = Color.FromArgb("fff"),
                        };
                    }
                    else if (type == 2)
                    {
                        planet = new Supernova
                        {
                            Text = random.Next(3, 9).ToString()
                        };
                    }
                    else
                    {
                        planet = new Planet();
                        // Error
                        choiceLayout.BackgroundColor = Color.FromArgb("f00");
                    }
                }
                else
                {
                    int v = random.Next(game.Lowest, game.Heighest);
                    planet = new Planet
                    {
                        Type = 0,
                        Text = v.ToString(),
                    };

                }

                planet.TranslationX = 360;

                planetsChoice[i] = planet;
                choiceLayout.Children.Add(planet);

                planet.TranslateTo((i - 1) * offsetBetweenPlanets, 0, 500, Easing.SpringOut);
                await Task.Delay(200);



                //adding clickable areas
                BoxViewWithIndex area = new BoxViewWithIndex
                {
                    Index = i,
                    Opacity = 0,
                    TranslationX = (i - 1) * offsetBetweenPlanets,
                };

                choiceLayout.Children.Add(area);
                AbsoluteLayout.SetLayoutBounds(area, new Rect(0.5, 0.5, 80, 80));
                AbsoluteLayout.SetLayoutFlags(area, AbsoluteLayoutFlags.PositionProportional);
                area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await OnClicked(game, area.Index)) });
            }
            await Task.Delay(350);
            protectiveLayout.IsVisible = false;
            game.EventHappening = game.Clicked = true; // escaping the barrier
        }
        private async Task OnClicked(GameWithEvents game, int index)
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

                game.AddClickableAreasToLayout();
                game.EventHappening = false;
                eventHappening = false;
            }
        }
        public bool EventHappening { get => eventHappening; }

    }
}