using GalaxyLogicGame.Events;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using GalaxyLogicGame.Powerups;

[assembly: ExportFont("bignoodletitling.ttf", Alias = "BigNoodleTitling")]

// itadaki seieki <- some hentai to watch

namespace GalaxyLogicGame
{

    public partial class CasualGame : Game
    {
        private int totalAtoms = 5;
        private Color[] colors;
        private ArrayList atoms = new ArrayList();
        private ArrayList blackholes = new ArrayList();
        private ArrayList stars = new ArrayList();
        private ArrayList shrinkingGiants = new ArrayList();
        private ArrayList clickableAreas = new ArrayList();
        private ArrayList debris = new ArrayList();
        private int heighest;
        private int lowest;
        private int plusWaiting = 0;
        private int atomSize = 60;
        private int limit = 14;
        //int sizeOffset = 30;
        private int fontSize = 40;
        private int delay = 200;
        private bool starCountdown = false;
        private int score = 0;

        private double degre;
        private double offset;
        private double tempIndex;
        private double degreClicked;
        private double indexClicked;

        //private Timer timer = new Timer();
        private int timerCounter;
        private int frames = 25;
        private ArrayList atomPositions = new ArrayList();
        private ArrayList atomCurrentPositions = new ArrayList();

        private PulsingParticle middleParticle;

        private DreamsEvent dreams;

        private PlanetBase newPlanet;

        private int turn;




        public CasualGame()
        {
            //lowest = GetAtomValue(0);//sets the lowest value (makes sure that you can not be stucked at any time)

            //Logic logic = new Logic(); // may be useless, delete later

            InitializeComponent();



            //Setup();
        }
        public virtual void SetupBase()
        {
            atomsLayout.Children.Clear();
            atoms.Clear();
            if (blackholeLayout.Children.Count > 0)
            {
                blackholes.Clear();
                blackholeLayout.Children.Clear();
            }

            middleParticle = new PulsingParticle();
            particleLayout.Children.Clear();
            particleLayout.Children.Add(middleParticle);


            colors = Functions.GetColors();
            heighest = 7;
            lowest = 1;
            degre = 0;
            offset = 0;
            tempIndex = 0;
            degreClicked = 0;
            turn = 0;
            indexClicked = 0;
            plusWaiting = 3;


            Score = 0; //
        }
        public override async Task Setup()
        {
            ArrayList tempArray = new ArrayList();
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                tempArray.Add(new Planet { Type = 0, Text = random.Next(1, 7).ToString() });
            }
            await InitializeThisLayout(tempArray, random.Next(30));
            //NewPlanet = new Planet { Type = 0, Text = random.Next(1, 7).ToString() };
            //AtomsLayout.Children.Add(NewPlanet);

            //await MoveAtoms();

            // the total end
            BG.GameOver();
            GenerateNewAtom();
        }
        public void GenerateNewAtom()
        {
            if (TelescopeActivated) // Telescope power-up things
            {
                if (NextPlanetsIndex > NextPlanets.Length - 1)
                {
                    TelescopeActivated = false;
                    NextPlanetsIndex = 0;
                }
                else
                {
                    PlanetBase tempPlanet;
                    if (NextPlanets[NextPlanetsIndex].Type == 0)
                    {
                        tempPlanet = new Planet
                        {
                            Type = NextPlanets[NextPlanetsIndex].Type,
                            Text = NextPlanets[NextPlanetsIndex].Text,
                        };

                        middleParticle.Play();
                    }
                    else if (NextPlanets[NextPlanetsIndex].Type == 1)
                    {
                        tempPlanet = new Planet
                        {
                            Type = NextPlanets[NextPlanetsIndex].Type,
                            Text = NextPlanets[NextPlanetsIndex].Text,
                            BGColor = Color.FromArgb("f00"),
                            TextColor = Color.FromArgb("fff"),
                        };
                    }
                    else if (NextPlanets[NextPlanetsIndex].Type == 2)
                    {
                        tempPlanet = new Supernova
                        {
                            Text = "3"
                        };
                        stars.Add(tempPlanet);
                    }
                    else
                    {
                        // this will never happen
                        tempPlanet = new Planet
                        {
                            Type = 0,
                            Text = "1"
                        };
                    }
                    
                        
                    
                    starCountdown = NextPlanets[NextPlanetsIndex].Type != 2;
                    newPlanet = tempPlanet;

                    atomsLayout.Children.Add(tempPlanet);
                    AddClickableAreasToLayout();

                    NextPlanetsIndex++;
                    return;
                }
            }

            Random random = new Random();

            PlanetBase temporaryPlanet;


            if (random.Next(5) == 0 || plusWaiting >= 6) //    <-- collision
            {
                temporaryPlanet = new Planet
                {
                    Type = 1,
                    Text = "+",
                    TextColor = Color.FromArgb("fff"),
                    BGColor = Color.FromArgb("f00"),
                };
                if (plusWaiting >= 6) plusWaiting = 0;
                else plusWaiting = 2;
                starCountdown = true;

            }

            else if (random.Next(20) == 0 && atoms.Count > 3 || random.Next(10) == 0 && atoms.Count > 10) // <-- blackhole // chance is around 20 +/- and 10 +/-
            {
                starCountdown = false;
                Blackhole bh = new Blackhole();
                blackholes.Add(bh);
                blackholeLayout.Children.Add(bh);

                AddBlackholeAreas();

                return;
            }



            else if (random.Next(20) == 0 && stars.Count < 1) // <-- star // chance is around 20 +/-
            {
                
                temporaryPlanet = new Supernova
                {
                    Text = random.Next(3, 9).ToString()
                };
                starCountdown = false;

                stars.Add(temporaryPlanet);
            }

            //                          <-- some more
            else // <-- normal planet
            {
                plusWaiting++;
                starCountdown = true;
                int v = random.Next(lowest, heighest);
                temporaryPlanet = new Planet
                {
                    Type = 0,
                    Text = v.ToString(),
                };

                middleParticle.Play();
            }

            if (BinaryActivated) temporaryPlanet.Binary.IsVisible = true;

            newPlanet = temporaryPlanet;
            atomsLayout.Children.Add(temporaryPlanet);

            AddClickableAreasToLayout();
        }
        public override async Task AreaClicked(int index)
        {
            //((BoxViewWithIndex)clickableAreas[index]).BackgroundColor = Color.Orange;
            if (Clicked)
            {
                Clicked = false;

                TurnPlusPlus();

                await AddingPlanet(index);
                //await Task.Delay(delay);
                await MergeAtoms();

                await StarBehaviour();

                await CheckChallenges();
                // the total end part

                await UpdateShrinkingGiants();
                BG.GameOver();

                GenerateNewAtom();

                await BG.LostScreenAnimation();
                //AddClickableAreasToLayout(); // maybe useless, delete later
                Clicked = true;
            }
        }


        public virtual async Task AddingPlanet(int index)
        {
            if (middleParticle.IsPlaying)
            {
                middleParticle.Stop(delay);
            }

            indexClicked = index;
            double prevDegre = degre;

            if (index + 1 < atoms.Count)
            {

                atoms.Insert(index + 1, newPlanet);

            }
            else
            {
                atoms.Add(newPlanet);
            }

            // Calculating offset
            degre = CirclePosition.CalculateDegre(atoms.Count);
            degreClicked = offset + prevDegre / 2 + index * prevDegre;
            tempIndex = (atoms.Count - index - 1) % atoms.Count;
            offset = (degre * tempIndex + degreClicked) % 360;

            // particles when clicked
            IntoCircleParticles particles = new IntoCircleParticles();
            Position p = CirclePosition.CalculatePosition(CirclePosition.ConvertDegreToRadian(degreClicked));
            particles.TranslationX = p.X;
            particles.TranslationY = p.Y;
            atomsLayout.Children.Add(particles);
            particles.Play(atomsLayout, 0, delay);


            //zeroTempIndex = atoms.Count - index;


            //debuggingLabel.Text = index + ""; // for debugging

            await MoveAtoms();
        }
        private async void BlackholeClicked(object sender, EventArgs args)
        {
            //((BoxViewWithIndex)clickableAreas[index]).BackgroundColor = Color.Orange
            if (Clicked)
            {
                Clicked = false;

                TurnPlusPlus();

                degreClicked = offset + ((ClickableArea)sender).Index * degre;
                degre = CirclePosition.CalculateDegre(atoms.Count - 1);

                tempIndex = (atoms.Count - ((ClickableArea)sender).Index - 1) % atoms.Count;

                // first part
                ArrayList positions = CirclePosition.GetCirclePositionsOffset(atoms.Count, offset);
                Position p = (Position)positions[((ClickableArea)sender).Index];

                Blackhole taker = new Blackhole
                {
                    //Scale = 0,
                };
                blackholes.Add(taker);
                blackholeLayout.Children.Add(taker);
                // /*
                taker.TranslationX = p.X;
                taker.TranslationY = p.Y;

                // second part
                await taker.ScaleTo(1, (uint)delay); ///
                //await Task.Delay(delay); /// -------
                await ((PlanetBase)atoms[((ClickableArea)sender).Index]).ScaleTo(0, (uint)delay / 2); ///
                //await Task.Delay(delay); /// -------
                // */
                //
                if (((PlanetBase)atoms[((ClickableArea)sender).Index]).Type == 2)
                {
                    stars.Remove((Supernova)atoms[((ClickableArea)sender).Index]);

                    if (MainMenuPage.IsUltra) GenerateShrinkingGiant();
                    else
                    {
                        Planet temporaryButton = new Planet();
                        Random random = new Random();
                        int v = random.Next(lowest, heighest + 1);

                        temporaryButton.Type = 0;
                        temporaryButton.Text = v + "";

                        newPlanet = temporaryButton;
                        atomsLayout.Children.Add(temporaryButton);
                    }


                    // Challenge 2
                    if (!Preferences.ContainsKey("org.tizen.myApp.challenge2") && MainMenuPage.IsUltra) { await Challenges.Challenge2(BG.MainLayout); }
                }
                else newPlanet = (PlanetBase)atoms[((ClickableArea)sender).Index];

                atoms.RemoveAt(((ClickableArea)sender).Index);

                // /*
                newPlanet.TranslationX = 0; // here change time
                newPlanet.TranslationY = 0;
                newPlanet.Scale = 0;
                //

                ///
                await Task.WhenAll(
                    taker.FadeTo(0, (uint)delay / 2),
                    newPlanet.ScaleTo(1, (uint)delay / 2));
                //await Task.Delay(delay); /// -------
                // */

                // Calculating offset
                offset = (degre * tempIndex + degreClicked + degre / 2) % 360;

                await MoveAtoms();
                //await ((Blackhole)blackholes[0]).FadeTo(0, (uint)delay / 2);

                blackholes.Clear();
                blackholeLayout.Children.Clear();



                await MoveAtoms();

                await MergeAtoms();

                await CheckChallenges();

                await UpdateShrinkingGiants();

                AddClickableAreasToLayout();

                Clicked = true;
            }
        }
        public virtual async Task MoveAtoms()
        {
            degre = CirclePosition.CalculateDegre(atoms.Count);
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(atoms.Count, offset);



            for (int i = 0; i < positions.Count; i++)
            {
                //int tempi = (i + tempIndex) % positions.Count;
                Position p = (Position)positions[i];
                ((PlanetBase)atoms[i]).TranslateTo(p.X, p.Y, (uint)delay);

            }
            await Task.Delay(delay + 50); /// -------
        }

        public async Task StarBehaviour()
        {
            if (stars.Count > 0 && starCountdown)
            {
                for (int i = 0; i < stars.Count; i++)
                {
                    ((PlanetBase)stars[i]).Text = Functions.GetAtomValue((PlanetBase)stars[i]) - 1 + "";
                    if (Functions.GetAtomValue((PlanetBase)stars[i]) == 0)
                    {
                        await StarExplosion();
                        UpdateShrinkingGiantsArray();

                        if (atoms.Count <= 1)
                        {
                            if (atoms.Count == 0 && MainMenuPage.IsUltra) await Challenges.Challenge10(BG.MainLayout); // challenge - empty board

                            Random random = new Random();
                            Planet temporaryButton = new Planet();
                            int v = random.Next(lowest, heighest + 1);

                            temporaryButton.Type = 0;
                            temporaryButton.Text = v + "";

                            atomsLayout.Children.Add(temporaryButton);
                            atoms.Add(temporaryButton);
                            await MoveAtoms();
                        }
                        else
                        {
                            await MergeAtoms();
                        }
                        break;
                    }
                }
                starCountdown = false;
            }
        }
        private async Task StarExplosion()
        {

            ArrayList positions = CirclePosition.GetCirclePositionsOffset(atoms.Count, offset);
            degre = CirclePosition.CalculateDegre(atoms.Count);
            int starDelay = delay * 2;

            for (int i = 0; i < atoms.Count; i++)
            {
                if (((PlanetBase)atoms[i]).Type == 2 && Functions.GetAtomValue((PlanetBase)atoms[i]) == 0)
                {
                    Supernova explodingStar = (Supernova)atoms[i];
                    int low;
                    int high;
                    double tempOffset = offset + i * degre;
                    int tempI = 0;

                    if (i != 0) low = i - 1;
                    else low = atoms.Count - 1;
                    if (i != atoms.Count - 1) high = i + 1;
                    else high = 0;

                    if (low < i) tempI = low;
                    else tempI = i;
                    if (low > high && low < i) tempI--;

                    Position supernovaPosition = (Position)positions[i];

                    double distance = ((Position)positions[low]).Distance(supernovaPosition);
                    // /*

                    /*
                    await explodingStar.ScaleTo(0, (uint)delay); ///
                    SupernovaExplosion supernova = new SupernovaExplosion();
                    supernova.TranslationX = supernovaPosition.X; 
                    supernova.TranslationY = supernovaPosition.Y;
                    await supernova.Play(particleLayout, distance, delay);
                    */
                    stars.Remove((PlanetBase)atoms[i]);
                    await explodingStar.FadeStarBase(starDelay);

                    explodingStar.ScaleTo(distance / 30, (uint)starDelay);
                    await Task.WhenAll(                                     ///
                        ((PlanetBase)atoms[low]).FadeTo(0, (uint)starDelay),
                        ((PlanetBase)atoms[i]).FadeTo(0, (uint)starDelay),
                        ((PlanetBase)atoms[high]).FadeTo(0, (uint)starDelay));
                    // */ 

                    //await supernova.FadeTo(0, (uint)delay);
                    //particleLayout.Children.Remove(supernova);

                    if (((PlanetBase)atoms[low]).Type == 4) debris.Remove((Debris)atoms[low]);
                    if (high != low && ((PlanetBase)atoms[high]).Type == 4) debris.Remove((Debris)atoms[high]);


                    atomsLayout.Children.Remove((PlanetBase)atoms[low]);
                    atomsLayout.Children.Remove((PlanetBase)atoms[i]);
                    atomsLayout.Children.Remove((PlanetBase)atoms[high]);



                    atoms.RemoveAt(low);
                    if (atoms.Count > low) atoms.RemoveAt(low);
                    else atoms.RemoveAt(0);

                    if (atoms.Count > 0)
                    {
                        if (atoms.Count > low) atoms.RemoveAt(low);
                        else atoms.RemoveAt(0);
                    }

                    CalculateDeletedOffset(tempOffset, tempI);
                    offset += degre / 2;

                    if (atoms.Count != 0)
                    {

                        await MoveAtoms();
                        BG.GameOver();

                    }

                    break;
                }
            }
        }

        public virtual async Task MergeAtoms()
        {
            double tempOffset = 0;
            int tempI = 0;

            bool repeatTwice = true;

            int atomValue = 0;
            int atomBonus = 0;
            PlanetBase[] selectedAtom = new PlanetBase[1];
            while (repeatTwice)
            {
                repeatTwice = false;

                GoHere:

                for (int i = 0; i < atoms.Count; i++)
                {
                    if (((PlanetBase)atoms[i]).Type == 1)
                    {
                        int low, high;


                        if (i == 0) low = atoms.Count - 1;
                        else low = i - 1;
                        if (i == atoms.Count - 1) high = 0;
                        else high = i + 1;

                        for (int dreamLow = 0; dreamLow < ((PlanetBase)atoms[low]).DreamNumber + 1; dreamLow++)
                        {
                            for (int dreamHigh = 0; dreamHigh < ((PlanetBase)atoms[high]).DreamNumber + 1; dreamHigh++)
                            {
                                if (((PlanetBase)atoms[low]).Type == 0 && ((PlanetBase)atoms[high]).Type == 0 && (GetAtomValue(low) + dreamLow) == (GetAtomValue(high) + dreamHigh) && low != high)
                                {
                                    if (atomBonus == 0)
                                    {
                                        tempOffset = offset + i * degre;
                                    }

                                    Score += (Functions.GetAtomValue((PlanetBase)atoms[low]) + dreamLow) * 2 + atomBonus * atomBonus * 5;

                                    repeatTwice = true;
                                    double x = ((PlanetBase)atoms[i]).TranslationX;
                                    double y = ((PlanetBase)atoms[i]).TranslationY;
                                    //debuggingLabel.Text = x + ""; // for debugging

                                    if (atomValue < GetAtomValue(high) + dreamHigh)
                                    {
                                        atomValue = GetAtomValue(high) + dreamHigh;
                                    }
                                    atomBonus++;

                                    if (atomValue + atomBonus > heighest) heighest = atomValue + atomBonus;
                                    //((Button)atoms[i]).Text = atomValue + "";

                                    ////////////////////////////////////////////////////// change later - Update (9.5.2022): I do not know what to change XD
                                    // /*

                                    OutOfCircleParticles particles = new OutOfCircleParticles();
                                    atomsLayout.Children.Add(particles);
                                    particles.TranslationX = x;
                                    particles.TranslationY = y;
                                    particles.Play(atomsLayout, (int)(delay / 1.25), delay);

                                    // Rasise
                                    atomsLayout.Children.Remove((PlanetBase)atoms[i]);
                                    atomsLayout.Children.Add((PlanetBase)atoms[i]);

                                    await Task.WhenAll(
                                        ((PlanetBase)atoms[low]).TranslateTo(x, y, (uint)delay, Easing.SinIn),
                                        ((PlanetBase)atoms[high]).TranslateTo(x, y, (uint)delay, Easing.SinIn),
                                        Task.Run(async () => {
                                            await Task.Delay((int)(delay / 1.25));
                                            await ((PlanetBase)atoms[i]).ScaleTo(1.2 * Math.Pow(1.1, atomBonus), (uint)(delay / (4 * Math.Pow(1.25, atomBonus))), Easing.SinIn);
                                            particles.ScaleTo(1.2 * Math.Pow(1.1, atomBonus), (uint)(delay / (4 * Math.Pow(1.25, atomBonus))), Easing.SinIn);
                                            await ((PlanetBase)atoms[i]).ScaleTo(1, (uint)delay / 2, Easing.SinOut);
                                        }));
                                    // */
                                    await Task.Delay(30); // optional - to make things look more fluid

                                    selectedAtom[0] = (PlanetBase)atoms[i];

                                    atomsLayout.Children.Remove((PlanetBase)atoms[low]);
                                    atoms.RemoveAt(low);

                                    if (low < i) tempI = low;
                                    else tempI = i;

                                    if (low < high)
                                    {
                                        if (!(high - 1 < 0))
                                        {
                                            atomsLayout.Children.Remove((PlanetBase)atoms[high - 1]);
                                            atoms.RemoveAt(high - 1);
                                        }
                                        else
                                        {
                                            atomsLayout.Children.Remove((PlanetBase)atoms[atoms.Count - 1]);
                                            atoms.RemoveAt(atoms.Count - 1);
                                        }
                                    }
                                    else //if (atoms.Count > 1) // maybe a useless check
                                    {
                                        atomsLayout.Children.Remove((PlanetBase)atoms[high]);
                                        atoms.RemoveAt(high);

                                        if (low < i) tempI--;
                                    }

                                    goto GoHere;
                                }
                            }
                        }
                    }
                }
            }
            if (atomValue != 0)
            {
                selectedAtom[0].Type = 0;
                selectedAtom[0].Text = (atomValue + atomBonus).ToString();

                if (atomBonus >= 4)
                {
                    if (MainMenuPage.IsUltra) await Challenges.Challenge13(BG.MainLayout);
                    if (atomBonus >= 6)
                    {
                        if (MainMenuPage.IsUltra) await Challenges.Challenge14(BG.MainLayout);
                    }
                }
                if (BinaryActivated) selectedAtom[0].Binary.IsVisible = true;

                UpdateShrinkingGiantsArray();
                CalculateDeletedOffset(tempOffset, tempI);

                await MoveAtoms();

                await MergeAtoms();
            }
            BG.GameOver();
        }
        public void CalculateDeletedOffset(double tempOffset, int tempI)
        {
            degre = CirclePosition.CalculateDegre(atoms.Count);
            offset = (tempOffset + (atoms.Count - tempI) * degre) % 360;
        }
        public void AddBlackholeAreas()
        {
            CheckPowerupPrerequisites();

            clickableAreaLayout.Children.Clear();
            clickableAreas.Clear();
            ArrayList positions = CirclePosition.GetCirclePositionsOffset(atoms.Count, offset);
            for (int i = 0; i < positions.Count; i++)
            {
                ClickableArea area = new ClickableArea
                {
                    BackgroundColor = Color.FromArgb("f00"),
                    Opacity = 0, // change this to see the touch areas
                    Index = i,
                };
                area.Clicked += BlackholeClicked;
                //area.Pressed += OnButtonPressed;
                //try { area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { try { await BlackholeClicked(area.Index); } catch (Exception ex) { } }) }); } catch { }
                clickableAreas.Add(area);

                //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
                Position p = (Position)positions[i];

                AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
                //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

                clickableAreaLayout.Children.Add(area);
            }
        }
        public void GenerateShrinkingGiant()
        {
            heighest += 3;
            Planet temporaryButton = new Planet
            {
                Type = 3,
                Text = (heighest + 1).ToString(),
                BGColor = Color.FromArgb("000"),
                TextColor = Color.FromArgb("be66ed"),
            };
            newPlanet = temporaryButton;
            shrinkingGiants.Add(temporaryButton);
            atomsLayout.Children.Add(temporaryButton);
        }
        public void AddClickableAreasToLayout()
        {
            CheckPowerupPrerequisites();

            clickableAreaLayout.Children.Clear();
            clickableAreas.Clear();

            ArrayList positions = CirclePosition.GetCirclePositionsOffset(atoms.Count, offset + degre / 2);
            for (int i = 0; i < positions.Count; i++)
            {
                BoxViewWithIndex area = new BoxViewWithIndex
                {
                    Index = i,
                    Opacity = 0,
                };
                clickableAreas.Add(area);
                area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AreaClicked(area.Index)) });
                //try { area.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { try { await areaClicked(area.Index); } catch (Exception ex) { } }) }); } catch { }
                //AbsoluteLayout.SetLayoutBounds(b, new Rect(150, 150, 60, 60)); //you will have to change values here, if you change the size
                Position p = (Position)positions[i];

                if (atoms.Count < 5)
                {
                    int size = 500 / atoms.Count;
                    AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 180 - size / 2, p.Y + 180 - size / 2, size, size));
                }
                else AbsoluteLayout.SetLayoutBounds(area, new Rect(p.X + 145, p.Y + 145, 70, 70));
                //AbsoluteLayout.SetLayoutFlags(b, AbsoluteLayoutFlags.PositionProportional);

                clickableAreaLayout.Children.Add(area);
            }
        }

        public int GetAtomValue(int index)
        {
            return int.Parse(((PlanetBase)atoms[index]).Text);
        }
        public override int Heighest { get { return heighest; } set { heighest = value; } }
        public int Score { get { return score; } set { score = value; BG?.UpdateScore(); } }
        

        /**
         * For a special moving background thanks to an accelerometer
         */
        public virtual void LoopTick()
        {

            if (MiddleParticle.IsPlaying)
            {
                MiddleParticle.Move();
            }

            if (Blackholes.Count > 0)
            {
                for (int i = 0; i < Blackholes.Count; i++)
                {
                    if (BlackholeLayout.Children.Count > i && Blackholes.Count > i) try { ((Blackhole)Blackholes[i]).RotateBlackhole += 2; } catch { }
                }
            }

            if (Stars.Count > 0)
            {
                for (int i = 0; i < Stars.Count; i++)
                {
                    if (Stars.Count > i) try { ((Supernova)Stars[i]).RotateStar -= 2; } catch { }
                }
            }
            
            if (Debris.Count > 0)
            {
                for (int i = 0; i < Debris.Count; i++)
                {
                    if (Debris.Count > i) try { ((Debris)Debris[i]).RotateDebris(); } catch { }
                }
            }
        }

        public async Task<bool> UpdateShrinkingGiants()
        {
            if (shrinkingGiants.Count > 0)
            {
                for (int i = 0; i < shrinkingGiants.Count; i++)
                {
                    if (!((Planet)shrinkingGiants[i]).Text.Equals("1")) ((Planet)shrinkingGiants[i]).Text = (int.Parse(((Planet)shrinkingGiants[i]).Text) - 1).ToString();
                    else if (MainMenuPage.IsUltra) await Challenges.Challenge12(BG.MainLayout);
                }

                await MergeAtoms();

                return true;
            }
            else return false;
        }
        public void UpdateShrinkingGiantsArray()
        {
            shrinkingGiants.Clear();

            for (int i = 0; i < atoms.Count; i++)
            {
                if (((PlanetBase)atoms[i]).IsTypeThree) shrinkingGiants.Add(atoms[i]);
            }
        }

        public async Task CheckChallenges()
        {
            if (MainMenuPage.IsUltra)
            {
                await Challenges.Challenge1(BG.MainLayout, atoms);
                await Challenges.Challenge3(BG.MainLayout, Score);
                await Challenges.Challenge4(BG.MainLayout, Score);
                await Challenges.Challenge5(BG.MainLayout, Score);
                await Challenges.Challenge6(BG.MainLayout, heighest);
                await Challenges.Challenge7(BG.MainLayout, heighest);
                await Challenges.Challenge8(BG.MainLayout, heighest);
                await Challenges.Challenge9(BG.MainLayout, atoms);
            }
        }

        public void SetNextPlanets()
        {
            bool starInArray = false;
            TelescopeActivated = true;
            for (int i = 0; i < NextPlanets.Length; i++)
            {
                Random random = new Random();

                if (random.Next(5) == 0 || plusWaiting >= 6) //    <-- collision
                {
                    NextPlanets[i] = new Planet
                    {
                        Type = 1,
                        Text = "+",
                        TextColor = Color.FromArgb("fff"),
                        BGColor = Color.FromArgb("f00"),
                    };
                    if (plusWaiting >= 6) plusWaiting = 0;
                    else plusWaiting = 2;
                }

                else if (!starInArray && random.Next(20) == 0 && stars.Count < 1) // <-- star // chance is around 20 +/-
                {
                    NextPlanets[i] = new Supernova
                    {
                        Text = "3"
                    };
                    starInArray = true;

                }
                //                          <-- some more
                else // <-- normal planet
                {
                    plusWaiting++;
                    int v = random.Next(lowest, heighest);
                    NextPlanets[i] = new Planet
                    {
                        Type = 0,
                        Text = v.ToString(),
                    };
                }
            }
        }
        public bool ContainsPlus()
        {
            foreach (PlanetBase planet in atoms)
            {
                if (planet.Type == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsPair()
        {
            // Not optimised, but time is more important.. So who cares?
            foreach (PlanetBase planet1 in atoms)
            {
                foreach (PlanetBase planet2 in atoms)
                {
                    if (planet1 != planet2 && planet1.Type == 0 && planet2.Type == 0 && planet1.Text.Equals(planet2.Text))
                    {
                        return true;
                    }
                }

            }
            return false;


        }

        public override async Task InitializeThisLayout(ArrayList array, double offset)
        {
            atoms = array;

            atomsLayout.Children.Clear();
            foreach (PlanetBase planet in atoms)
            {
                atomsLayout.Children.Add(planet);
            }

            this.offset = offset;

            await MoveAtoms();

            TurnPlusPlus();
        }
        /**
         * This method is used to count up the total number of turns and to update all the things that rely on it
         */
        public void TurnPlusPlus()
        {
            turn++;

            foreach (PowerupBase powerup in BG.Powerups)
            {
                powerup.UpdateCooldown();
            }
        }
        public void CheckPowerupPrerequisites()
        {
            foreach (PowerupBase powerup in BG.Powerups)
            {
                powerup.Prerequisites();
            }
        }
        public AbsoluteLayout EventCounterLayout { get => eventCounterLayout; set { eventCounterLayout = value; } }
        public AbsoluteLayout BlindnessLayout { get => blindnessLayout; set { blindnessLayout = value; } }
        public AbsoluteLayout AtomsLayout { get { return atomsLayout; } set { atomsLayout = value; } }
        public AbsoluteLayout BlackholeLayout { get => blackholeLayout; set { blackholeLayout = value; } }
        public double Offset { get { return offset; } set { offset = value; } }
        public double Degre { get => degre; set { degre = value; } }
        public double DegreClicked { get => degreClicked; set { degreClicked = value; } }
        public double TempIndex { get => tempIndex; set { tempIndex = value; } }
        public int Delay => delay;
        public PulsingParticle MiddleParticle { get => middleParticle; }
        public ArrayList Atoms { get => atoms; }
        public ArrayList Blackholes { get => blackholes; }
        public ArrayList Stars { get => stars; }
        public ArrayList Debris { get => debris; }
        public ArrayList ClickableAreas { get => clickableAreas; set { clickableAreas = value; } }
        public AbsoluteLayout ClickableAreaLayout { get => clickableAreaLayout; set { clickableAreaLayout = value; } }
        public PlanetBase NewPlanet { get => newPlanet; set { newPlanet = value; } }

        public bool StarCountdown { get => starCountdown; set { starCountdown = value; } }
        public int PlusWaiting { get => plusWaiting; set { plusWaiting = value; } }
        public int Lowest { get => lowest; }
        public int Limit => limit;
        public int Turn => turn;
        public bool TelescopeActivated { get; set; } = false;
        public PlanetBase[] NextPlanets { get; } = new PlanetBase[5];
        public int NextPlanetsIndex { get; set; } = 0;

        public virtual bool BinaryActivated => false;
        //public int Score { get => score; set { score = value; } }

        
        //public int Heighest { get => heighest; }
        
        public DreamsEvent Dreams { set { dreams = value; } }
    }
}