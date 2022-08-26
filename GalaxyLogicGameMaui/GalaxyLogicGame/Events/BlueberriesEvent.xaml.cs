using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Types;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Events
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlueberriesEvent : AbsoluteLayout, IEvent, IEventObject, IEventInfo
    {
        public const string NAME = "BlueberriesEvent";
        private bool eventHappening = true;
        private int duration = new Random().Next(2, 5);
        private GameWithEvents game;

        public BlueberriesEvent ()
		{
			InitializeComponent ();
		}
        public Label EventTitle => eventTitle;
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && game.Gamemode == Gamemode.GameJam;
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "Two planets transform into blueberries. In a few turns, they will explode, resulting in raising other planets' value."
        };
        public GameJamEventDescription GetGameJamEventDescription(TapGestureRecognizer openGameJamPage)
        {
            return new GameJamEventDescription
            {
                Title = eventTitle.Text,
                TitleColor = eventTitle.TextColor,
                IconImageSource = eventIcon.Source,
                Description = "Two planets transform into blueberries. In a few turns, they will explode, resulting in raising other planets' value.",
                OpenGameJamPage = openGameJamPage,
            };
        }
        public bool Prerequisites(GameWithEvents game)
        {
            return ContainsTwoPlanets(game) && IsActivated(game);
        }

        private bool ContainsTwoPlanets(GameWithEvents game)
        {
            int counter = 0;
            foreach (PlanetBase planet in game.Atoms)
            {
                if (planet.Type == 0)
                {
                    counter++;
                    if (counter == 2) return true;
                }
            }
            return false;
        }
        public async Task Appear(GameWithEvents game)
        {
            this.game = game;
            game.EventObject = this;

            game.EventCounter = - duration + 1; // maybe change later

            game.BG.ShowEvent(mainLayout);
            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);

            for (int i = 0; i < 2; i++)
            {
                int highest = 0;
                int highestIndex = 0;
                for (int planetIndex = 0; planetIndex < game.Atoms.Count; planetIndex++)
                {
                    if (((PlanetBase)game.Atoms[planetIndex]).Type == 0 && Functions.GetAtomValue((PlanetBase)game.Atoms[planetIndex]) > highest)
                    {
                        highest = Functions.GetAtomValue((PlanetBase)game.Atoms[planetIndex]);
                        highestIndex = planetIndex;
                    }
                }

                PlanetBase tempPlanet = (PlanetBase)game.Atoms[highestIndex];

                double x = tempPlanet.TranslationX;
                double y = tempPlanet.TranslationY;


                Blueberry blueberry = new Blueberry();
                game.AtomsLayout.Children.Add(blueberry);
                await Task.WhenAll(
                    blueberry.TranslateTo(x, y, 250, Easing.SpringOut), 
                    Task.Run(async () => {
                        await Task.Delay(200);
                        await blueberry.ScaleTo(1.2, 75, Easing.SinIn);
                        await blueberry.ScaleTo(1, 50, Easing.SinOut);

                        // animation
                    }
                ));

                game.AtomsLayout.Children.Remove(tempPlanet);

                game.Atoms[highestIndex] = blueberry;
            }

            eventHappening = false;

            game.BG.HideAllEvents();
            game.BlindnessLayout.Children.Add(mainLayout);
        }
        public async Task Disappear()
        {
            game.BlindnessLayout.Children.Remove(this);
            game.BG.LowerEventLayout.Children.Add(this);

            ArrayList particles = new ArrayList();

            for (int i = game.Atoms.Count - 1; i >= 0; i--)
            {
                if (((PlanetBase)game.Atoms[i]).Type == 5)
                {
                    // animation + special effects

                    game.AtomsLayout.Children.Remove((PlanetBase)game.Atoms[i]);
                    BlueberryExplosion blueberryExplosion = new BlueberryExplosion();
                    blueberryExplosion.TranslationX = ((PlanetBase)game.Atoms[i]).TranslationX;
                    blueberryExplosion.TranslationY = ((PlanetBase)game.Atoms[i]).TranslationY;
                    blueberryExplosion.Play(game.AtomsLayout);



                    ArrayList planets = new ArrayList();
                    for (int j = 0; j < game.Atoms.Count; j++)
                    {
                        if (((PlanetBase)game.Atoms[j]).Type == 0) planets.Add(game.Atoms[j]);
                    }

                    int counter = 3 < planets.Count ? 3 : (planets.Count - 1 > 0 ? planets.Count - 1 : planets.Count);

                    Random random = new Random();

                    while (counter > 0)
                    {
                        counter--;
                        


                        Planet tempPlanet = (Planet)planets[random.Next(0, planets.Count)];
                        planets.Remove(tempPlanet);

                        double endX = tempPlanet.TranslationX;
                        double endY = tempPlanet.TranslationY;


                        int bonus = random.Next(1, 4);

                        for (int p = 0; p < bonus; p++)
                        {
                            double startX = ((PlanetBase)game.Atoms[i]).TranslationX;
                            double startY = ((PlanetBase)game.Atoms[i]).TranslationY; // + positions[p].Y;

                            Button particle = new Button
                            {
                                TranslationX = startX,
                                TranslationY = startY,
                                BackgroundColor = random.Next(0, 4) == 0 ? Color.FromHex("1f1f99") : Color.FromHex("#a62149"),
                                Rotation = random.Next(-360, 360),
                                Opacity = 0.7,
                                CornerRadius = 20,
                            };
                            int size = random.Next(20, 40);
                            mainLayout.Children.Add(particle);
                            AbsoluteLayout.SetLayoutBounds(particle, new Rect(0.5, 0.5, size, size));
                            AbsoluteLayout.SetLayoutFlags(particle, AbsoluteLayoutFlags.PositionProportional);
                            particles.Add(particle);
                            particle.TranslateTo(endX, endY, 250);
                            //particle.RotateTo(random.Next(-360, 360), 250);
                            await Task.Delay(30);
                        }
                        Task.Run(async () => {
                            await Task.Delay(200);
                            await tempPlanet.ScaleTo(1.2, 50, Easing.SpringOut);
                            if (bonus > 1) await tempPlanet.ScaleTo(1.35, 50, Easing.SpringOut);
                            if (bonus > 2) await tempPlanet.ScaleTo(1.45, 50, Easing.SpringOut);
                            await tempPlanet.ScaleTo(1, 150, Easing.SinIn);
                        });

                        tempPlanet.Text = (Functions.GetAtomValue(tempPlanet) + bonus).ToString();

                        await Task.Delay(100);
                    }


                    game.Atoms.RemoveAt(i);


                    await Task.Delay(300);
                }


            }


            await mainLayout.FadeTo(0, 250);
            game.BG.LowerEventLayout.Children.Remove(this);

            game.EventObject = null;

            await game.MoveAtoms();
            await game.MergeAtoms();

        }
        public async Task Move()
        {
            duration--;
            if (duration == 0)
            {
                await Disappear();
            }
        }
        public bool EventHappening { get => eventHappening; }

    }
}