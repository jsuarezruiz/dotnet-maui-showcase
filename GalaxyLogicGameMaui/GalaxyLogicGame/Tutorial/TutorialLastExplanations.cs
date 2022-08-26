using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Tutorial
{
    class TutorialLastExplanations : TutorialGameBase
    {
        public override async Task SetupTutorial()
        {
            ArrayList tempArray = new ArrayList();
            tempArray.Add(new Planet { Type = 0, Text = "6" });
            tempArray.Add(new Planet { Type = 0, Text = "4" });
            tempArray.Add(new Planet { Type = 0, Text = "4" });
            tempArray.Add(new Planet { Type = 0, Text = "8" });
            tempArray.Add(new Planet { Type = 0, Text = "7" });
            tempArray.Add(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });
            tempArray.Add(new Planet { Type = 0, Text = "1" });
            tempArray.Add(new Planet { Type = 0, Text = "2" });
            tempArray.Add(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });
            tempArray.Add(new Planet { Type = 0, Text = "3" }); 
            tempArray.Add(new Planet { Type = 0, Text = "3" });
            tempArray.Add(new Planet { Type = 0, Text = "11" });
            tempArray.Add(new Planet { Type = 0, Text = "10" });
            tempArray.Add(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });

            TutorialText("Your goal is to get the highest score possible. The game ends when the playing area is full.", "There are other objects for you to use and you will have to figure out how to take advantage of them.", 4);

            await InitializeThisLayout(tempArray, 0);


            AddNewPlanet(5);
            MiddleParticle.Play();
        }
        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);



                BG.LostScreenText.Text = "Tutorial completed";
                BG.GameOver();
                await BG.LostScreenAnimation();

                Clicked = true;
            }
        }
        public async Task TutorialText(string text, string text2, int index)
        {
            Label label = new Label
            {
                Text = text2,
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
                    // Added Clicked
                    if (Clicked)
                    {
                        Clicked = false;
                        await tempLayout.FadeTo(0, 500);
                        tempLayout.IsVisible = false;
                        BG.TutorialLayout.Children.Remove(tempLayout);
                        
                        await TutorialText(text, index);
                        Clicked = true;
                    }
                    
                })
            });

            await Task.Delay(500);
            await tempLayout.FadeTo(1, 250);

        }
    }
}
