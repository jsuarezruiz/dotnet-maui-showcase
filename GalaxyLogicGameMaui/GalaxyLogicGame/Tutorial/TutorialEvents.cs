using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Tutorial
{
    public class TutorialEvents : TutorialGameBase
    {
        public override async Task SetupTutorial()
        {
            ArrayList tempArray = new ArrayList();
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = 0, Text = "4" });
            tempArray.Add(new Planet { Type = 0, Text = "1" });
            tempArray.Add(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });
            tempArray.Add(new Planet { Type = 0, Text = "4" });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });

            CustomTutorialText("One of the main feature of this game are events. So, let's try out one of them.");

            await InitializeThisLayout(tempArray, 0);

        }


        public async Task CustomTutorialText(string text)
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
                Command = new Command(async () => {
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        ShootingStarTutorial shootingStarsTutorial = new ShootingStarTutorial();

                        tempLayout.IsVisible = false;
                        tempLayout.Children.Clear();

                        await shootingStarsTutorial.Appear(this);
                        //adding clickable areas
                        ClickableAreaWithParticles area = new ClickableAreaWithParticles
                        {
                            Index = 1,
                        };


                        BG.TutorialLayout.Children.Add(area);
                        AbsoluteLayout.SetLayoutBounds(area, new Rect(0.5, 0.5, 80, 80));
                        AbsoluteLayout.SetLayoutFlags(area, AbsoluteLayoutFlags.PositionProportional);
                        ClickableAreas.Add(area);
                        area.Play();

                        area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await shootingStarsTutorial.OnClicked(this, area.Index, area)) });

                        Clicked = true;
                    }
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);
        }
        public void RemoveClickableArea(ClickableAreaWithParticles area)
        {
            ClickableAreas.Remove(area);
            BG.TutorialLayout.Children.Remove(area);
        }
        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);

                await MergeAtoms();

                await SecondTutorialText("There are more events awaiting you. It will be up to you to figure out how to take advantage of them.", new TutorialLastExplanations());

                Clicked = true;
            }
        }
    }
    
}
