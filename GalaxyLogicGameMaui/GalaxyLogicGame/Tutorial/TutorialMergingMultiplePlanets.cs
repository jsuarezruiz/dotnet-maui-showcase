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
    class TutorialMergingMultiplePlanets : TutorialGameBase
    {
        public override async Task SetupTutorial()
        {
            ArrayList tempArray = new ArrayList();
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = 0, Text = "6" });
            tempArray.Add(new Planet { Type = 0, Text = "3" });
            tempArray.Add(new Planet { Type = 0, Text = "3" });
            tempArray.Add(new Planet { Type = 0, Text = "6" });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });

            TutorialText("Now let's try it again with more planets", 2);

            await InitializeThisLayout(tempArray, 0);


            AddNewPlanet(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });
            //MiddleParticle.Play();
        }
        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);

                Clicked = true;

                await MergeAtoms();

                

                await SecondTutorialText("I hope you are getting into the bigger picture now.", new TutorialEvents());
            }
        }
        public override async Task MergeAtoms()
        {
            double tempOffset = 0;
            int tempI = 0;
            int heighest = 0;
            int delay = 250;

            bool messageDone = true;

            bool repeatTwice = true;

            int atomValue = 0;
            int atomBonus = 0;
            PlanetBase[] selectedAtom = new PlanetBase[1];
            while (repeatTwice)
            {
                repeatTwice = false;
                for (int i = 0; i < Atoms.Count; i++)
                {
                    if (((PlanetBase)Atoms[i]).Type == 1)
                    {
                        int low, high;


                        if (i == 0) low = Atoms.Count - 1;
                        else low = i - 1;
                        if (i == Atoms.Count - 1) high = 0;
                        else high = i + 1;


                        if (((PlanetBase)Atoms[low]).Type == 0 && ((PlanetBase)Atoms[high]).Type == 0 && ((PlanetBase)Atoms[low]).Text.Equals(((PlanetBase)Atoms[high]).Text) && low != high)
                        {
                            if (atomBonus == 0)
                            {
                                tempOffset = Offset + i * Degre;
                            }

                            //Score += Functions.GetAtomValue((Planet)Atoms[low]) * 2 + atomBonus * atomBonus * 5;

                            repeatTwice = true;
                            double x = ((PlanetBase)Atoms[i]).TranslationX;
                            double y = ((PlanetBase)Atoms[i]).TranslationY;
                            //debuggingLabel.Text = x + ""; // for debugging

                            if (atomValue < GetAtomValue(high))
                            {
                                atomValue = GetAtomValue(high);
                            }
                            atomBonus++;
                            if (atomValue + atomBonus > heighest) heighest = atomValue + atomBonus;
                            //((Button)Atoms[i]).Text = atomValue + "";

                            ////////////////////////////////////////////////////////////////////////////////////////////////// change later
                            // /*

                            OutOfCircleParticles particles = new OutOfCircleParticles();
                            AtomsLayout.Children.Add(particles);
                            particles.TranslationX = x;
                            particles.TranslationY = y;
                            particles.Play(AtomsLayout, (int)(delay / 1.25), delay);

                            // Raise
                            AtomsLayout.Children.Remove(((PlanetBase)Atoms[i]));
                            AtomsLayout.Children.Add(((PlanetBase)Atoms[i]));


                            await Task.WhenAll(
                                ((PlanetBase)Atoms[low]).TranslateTo(x, y, (uint)delay, Easing.SinIn),
                                ((PlanetBase)Atoms[high]).TranslateTo(x, y, (uint)delay, Easing.SinIn),
                                Task.Run(async () => {
                                    await Task.Delay((int)(delay / 1.25));
                                    await ((PlanetBase)Atoms[i]).ScaleTo(1.2 * Math.Pow(1.1, atomBonus), (uint)(delay / (4 * Math.Pow(1.25, atomBonus))), Easing.SinIn);
                                    particles.ScaleTo(1.2 * Math.Pow(1.1, atomBonus), (uint)(delay / (4 * Math.Pow(1.25, atomBonus))), Easing.SinIn);
                                    await ((PlanetBase)Atoms[i]).ScaleTo(1, (uint)delay / 2, Easing.SinOut);
                                }));
                            // */
                            //await Task.Delay(delay); /// -----------

                            selectedAtom[0] = (PlanetBase)Atoms[i];

                            AtomsLayout.Children.Remove((PlanetBase)Atoms[low]);
                            Atoms.RemoveAt(low);

                            if (low < i) tempI = low;
                            else tempI = i;

                            if (low < high)
                            {
                                if (!(high - 1 < 0))
                                {
                                    AtomsLayout.Children.Remove((PlanetBase)Atoms[high - 1]);
                                    Atoms.RemoveAt(high - 1);
                                }
                                else
                                {
                                    AtomsLayout.Children.Remove((PlanetBase)Atoms[Atoms.Count - 1]);
                                    Atoms.RemoveAt(Atoms.Count - 1);
                                }
                            }
                            else //if (Atoms.Count > 1) // maybe a useless check
                            {
                                AtomsLayout.Children.Remove((PlanetBase)Atoms[high]);
                                Atoms.RemoveAt(high);

                                if (low < i) tempI--;
                            }

                            if (messageDone)
                            {
                                BG.TutorialLayout.IsVisible = true;
                                Label label = new Label
                                {
                                    Text = "Even when the planets merge together, the \"+\" continues to work as intended, allowing you to merge multiple pairs of planets at once",
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    TextColor = Color.FromArgb("fff"),

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
                                            BG.TutorialLayout.IsVisible = false;
                                            messageDone = false;
                                            Clicked = true;
                                        }
                                    })
                                });

                                await Task.Delay(500);
                                await tempLayout.FadeTo(1, 250);
                            }
                            while (messageDone)
                            {
                                await Task.Delay(1000);
                            }

                            break;
                        }
                    }
                }
            }
            if (atomValue != 0)
            {
                selectedAtom[0].Type = 0;
                selectedAtom[0].Text = atomValue + atomBonus + "";


                CalculateDeletedOffset(tempOffset, tempI);

                await MoveAtoms();
                await MergeAtoms();
            }
        }
    }
}
