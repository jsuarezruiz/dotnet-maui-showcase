using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Tutorial
{
    public abstract class TutorialGameBase : CasualGame
    {
        public TutorialGameBase()
        {
            // nothing here
        }
        
        public override async Task Setup()
        {
            await SetupTutorial();
            Clicked = true;

        }
        public override void SetupBase()
        {
            Clicked = false;
            base.SetupBase();
            
            BG.LowerUILayout.IsVisible = false;
            BG.PowerupsAllowed = false;
            if (Device.RuntimePlatform == Device.Tizen) { BG.BackgroundImage.Source = "starsSky.jpg"; AbsoluteLayout.SetLayoutBounds(BG.BackgroundImage, new Rect(0.5, 0.5, 360, 360)); }
            else
            {
                AbsoluteLayout.SetLayoutBounds(BG.BackgroundImage, new Rect(0.5, 0.5, 480, 480));
                BG.BackgroundImage.Source = "starssky.png";
                BG.TransitionLayout.BackgroundColor = Color.FromArgb("f00");
            }
            BG.TutorialLayout.IsVisible = true;
        }

        public abstract Task SetupTutorial();

        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);

                AddTutorialClickableArea(index);

                Clicked = true;
            }
        }
        public override async Task AddingPlanet(int index)
        {
            for (int i = 0; i < ClickableAreas.Count; i++)
            {
                if (ClickableAreas.Count > i) try { ((ClickableAreaWithParticles)ClickableAreas[i]).FadeTo(0, 250); } catch { }
            }

            await base.AddingPlanet(index);

            ClickableAreaLayout.Children.Clear();
            ClickableAreas.Clear();
        }

        public async Task TutorialText(string text, int index)
        {

            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,
            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.IsVisible = true;
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);

            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        AddTutorialClickableArea(index);

                        BG.TutorialLayout.Children.Remove(tempLayout);
                        Clicked = true;
                    }
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }


        public async Task TutorialText(string text, int index, Command onClickCommand)
        {

            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,
            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);

            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = onClickCommand
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }


        public async Task SecondTutorialText(string text, TutorialGameBase nextTutorial)
        {
            BG.TutorialLayout.IsVisible = true;
            Label label = new Label
            {
                Text = text,
                TextColor = Color.FromArgb("fff"),

                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 24,

            };
            AbsoluteLayout tempLayout = new AbsoluteLayout
            {
                BackgroundColor = Color.FromHex("88000000"),
                Opacity = 0,
            };
            tempLayout.Children.Add(label);
            AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 320, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
            BG.TutorialLayout.Children.Add(tempLayout);
            AbsoluteLayout.SetLayoutBounds(tempLayout, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(tempLayout, AbsoluteLayoutFlags.All);



            tempLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        BG.TutorialLayout.Children.Remove(tempLayout);

                        await BG.NavigateToOtherTutorial(nextTutorial);
                    }
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }

        public void AddTutorialClickableArea(int index)
        {
            BG.TutorialLayout.IsVisible = false;

            ClickableAreaLayout.Children.Clear();
            ClickableAreas.Clear();

            double degre = CirclePosition.CalculateDegre(Atoms.Count);
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(Atoms.Count, Offset + degre / 2);

            ClickableAreaWithParticles area = new ClickableAreaWithParticles
            {
                Index = index,
                Opacity = 1,
            };
            ClickableAreas.Add(area);
            area.Play();
            area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AreaClicked(area.Index)) });
            //try { area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { try { await areaClicked(area.Index); } catch (Exception ex) { } }) }); } catch { }
            //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
            Position p = (Position)positions[index];

            if (Atoms.Count < 5)
            {
                int size = 500 / Atoms.Count;
                AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 180 - size / 2, p.Y + 180 - size / 2, size, size));
            }
            else AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
            //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

            ClickableAreaLayout.Children.Add(area);
        }

        public override void LoopTick()
        {
            if (MiddleParticle != null && MiddleParticle.IsPlaying)
            {
                MiddleParticle.Move();
            }
            
            if (ClickableAreas.Count > 0)
            {
                for (int i = 0; i < ClickableAreas.Count; i++)
                {
                    if (ClickableAreas.Count > i) try { ((ClickableAreaWithParticles)ClickableAreas[i]).Move(); } catch { }
                }
            }
       
        }
        public void AddNewPlanet(int value)
        {
            NewPlanet = new Planet { Type = 0, Text = value + "" };
            AtomsLayout.Children.Add(NewPlanet);
        }
        public void AddNewPlanet(PlanetBase planet)
        {
            NewPlanet = planet;
            AtomsLayout.Children.Add(NewPlanet);
        }

        
    }
}
