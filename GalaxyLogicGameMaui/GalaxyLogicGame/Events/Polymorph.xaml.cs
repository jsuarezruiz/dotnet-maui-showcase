using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Events
{

    public partial class Polymorph : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "PolymorphEvent";

        private bool eventHappening = true;

        private int chosenIndex;
        private Random random = new Random();
        public Polymorph()
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
            Description = "One of your planets will get chosen at random. You have to change it to a different one from your galaxy."
        };
        public bool Prerequisites(GameWithEvents game)
        {
            if (game.Stars.Count == 0 && game.Debris.Count == 0 && game.Atoms.Count > 2)
            {
                return true && IsActivated(game);
            }
            else return false;
        }
        public async Task Appear(GameWithEvents game)
        {
            game.GenerateNewPlanet = false;
            game.EventHappening = true;
            
            game.BG.ShowEvent(mainLayout);
            

            await Task.WhenAll(
                Functions.EventTitleAnimation(eventTitle, eventIcon)
                /*,choiceLayout.FadeTo(0.5, 500)*/
                , darken.FadeTo(1, 500));

            game.BG.HideAllEvents();
            game.BG.LowerEventLayout.Children.Add(mainLayout);

            await choiceLayout.FadeTo(1, 500);

            chosenIndex = random.Next(game.Atoms.Count);

            await ((PlanetBase)game.Atoms[chosenIndex]).TranslateTo(0, 0, 250);

            //((Planet)game.Atoms[chosenIndex]).BGColor = Color.Gray;
            //((Planet)game.Atoms[chosenIndex]).Text = " ";


            PolymorphParticle particle = new PolymorphParticle();
            game.AtomsLayout.Children.Add(particle);
            game.PolyParticle = particle;
            particle.PulsingPlanet = (PlanetBase)game.Atoms[chosenIndex];
            particle.Play();
            

            game.ClickableAreaLayout.Children.Clear();
            game.ClickableAreas.Clear();
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(game.Atoms.Count, game.Offset);
            for (int i = 0; i < positions.Count; i++)
            {
                if (i != chosenIndex)
                {
                    BoxViewWithIndex area = new BoxViewWithIndex
                    {
                        BackgroundColor = Color.FromArgb("0f00"),
                        Opacity = 0, // change this to see the touch areas
                        Index = i,
                    };

                    area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnClicked(game, area.Index); }) });
                    game.ClickableAreas.Add(area);

                    //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
                    Position p = (Position)positions[i];

                    //AbsoluteLayout.SetLayoutBounds(area, );
                    //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

                    game.ClickableAreaLayout.Children.Add(area);
                    AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
                }
            }
            game.EventHappening = game.Clicked = true; // escaping the barrier
        }

        private async Task OnClicked(GameWithEvents game, int index)
        {
            if (game.Clicked)
            {
                game.Clicked = false;
                Button[] particles = new Button[3];



                await Task.WhenAll(
                    ((PlanetBase)game.Atoms[index]).ScaleTo(1.2, 50, Easing.SinIn),
                    game.PolyParticle.Stop(50));

                game.AtomsLayout.Children.Remove(game.PolyParticle);
                game.PolyParticle = null;
                ((PlanetBase)game.Atoms[chosenIndex]).ScaleTo(1, 250, Easing.SinOut);
                ((PlanetBase)game.Atoms[index]).ScaleTo(1, 125, Easing.SinOut);



                // animation
                int delay = 25;
                for (int i = 0; i < particles.Length; i++)
                {
                    Button particle = new Button
                    {
                        TranslationX = ((PlanetBase)game.Atoms[index]).TranslationX,
                        TranslationY = ((PlanetBase)game.Atoms[index]).TranslationY,
                        BackgroundColor = ((Planet)game.Atoms[index]).BGColor,
                        Rotation = random.Next(-360, 360),
                        Opacity = 0.7,
                        CornerRadius = 20,
                    };
                    int size = random.Next(20, 40);
                    mainLayout.Children.Add(particle);
                    AbsoluteLayout.SetLayoutBounds(particle, new Rect(0.5, 0.5, size, size));
                    AbsoluteLayout.SetLayoutFlags(particle, AbsoluteLayoutFlags.PositionProportional);
                    particles[i] = particle;
                    particle.TranslateTo(0, 0, 250);
                    //particle.RotateTo(random.Next(-360, 360), 250);
                    await Task.Delay(30);
                }

                await Task.Delay(200);

                if (((PlanetBase)game.Atoms[chosenIndex]).IsTypeThree) ((PlanetBase)game.Atoms[index]).Type = 3;
                else ((PlanetBase)game.Atoms[chosenIndex]).Type = ((PlanetBase)game.Atoms[index]).Type;
                
                ((Planet)game.Atoms[chosenIndex]).Text = ((Planet)game.Atoms[index]).Text;
                ((Planet)game.Atoms[chosenIndex]).TextColor = ((Planet)game.Atoms[index]).TextColor;
                ((Planet)game.Atoms[chosenIndex]).BGColor = ((Planet)game.Atoms[index]).BGColor;

                game.UpdateShrinkingGiantsArray();

                await ((Planet)game.Atoms[chosenIndex]).ScaleTo(1.2, 50, Easing.SpringOut);
                await ((Planet)game.Atoms[chosenIndex]).ScaleTo(1.35, 50, Easing.SpringOut);
                await ((Planet)game.Atoms[chosenIndex]).ScaleTo(1.45, 50, Easing.SpringOut);
                await ((Planet)game.Atoms[chosenIndex]).ScaleTo(1, 150, Easing.SinIn);


                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Opacity = 0;
                }

                await Task.WhenAll(
                    mainLayout.FadeTo(0, 500),
                    game.MoveAtoms());


                game.BG.LowerEventLayout.Children.Clear();

                await game.MergeAtoms();
                await game.CheckChallenges();

                game.GenerateNewAtom();
                game.EventHappening = false;
                eventHappening = false;
                
            }
        }
        public bool EventHappening { get => eventHappening; }

    }
}