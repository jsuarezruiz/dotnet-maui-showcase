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

namespace GalaxyLogicGame.Events
{

    public partial class WorldUpsideDownEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "WorldUpsideDownEvent";
        public WorldUpsideDownEvent()
        {
            InitializeComponent();
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "For this one moment, everything is upside down and everything is posible meaning even planets far apart can combine."
        };
        public GameJamEventDescription GetGameJamEventDescription(TapGestureRecognizer openGameJamPage)
        {
            return new GameJamEventDescription
            {
                Title = eventTitle.Text,
                TitleColor = eventTitle.TextColor,
                IconImageSource = eventIcon.Source,
                Description = "For this one moment, everything is upside down and everything is posible meaning even planets far apart can combine.",
                OpenGameJamPage = openGameJamPage,
            };
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return (!game.CustomMode || Preferences.Get(NAME, true)) && game.Gamemode == Gamemode.GameJam;
        }
        public bool Prerequisites(GameWithEvents game)
        {
            return IsActivated(game) && game.ContainsPair() && game.ContainsPlus();
        }
        public async Task Appear(GameWithEvents game)
        {
            game.BG.ShowEvent(mainLayout);
            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);

            // merging
            int plusIndex = -1;
            int planetValue = -1;

            for (int i = 0; i < game.Atoms.Count; i++)
            {
                if (((PlanetBase)game.Atoms[i]).Type == 1)
                {
                    plusIndex = i;
                    break;
                }
            }

            PlanetBase plus = (PlanetBase)game.Atoms[plusIndex];
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                for (int j = 0; j < game.Atoms.Count; j++)
                {
                    if (i != j && ((PlanetBase)game.Atoms[i]).Type == 0 && ((PlanetBase)game.Atoms[j]).Type == 0 && ((PlanetBase)game.Atoms[i]).Text.Equals(((PlanetBase)game.Atoms[j]).Text))
                    {

                        // Very needed for proper positions
                        double tempOffset = game.Offset + plusIndex * game.Degre;

                        planetValue = Functions.GetAtomValue((PlanetBase)game.Atoms[i]);
                        game.Score += Functions.GetAtomValue((PlanetBase)game.Atoms[i]) * 2 + 20;

                        double x = ((PlanetBase)game.Atoms[plusIndex]).TranslationX;
                        double y = ((PlanetBase)game.Atoms[plusIndex]).TranslationY;
                        //debuggingLabel.Text = x + ""; // for debugging

                        //((Button)atoms[i]).Text = atomValue + "";

                        ////////////////////////////////////////////////////// change later - Update (9.5.2022): I do not know what to change XD
                        // /*

                        OutOfCircleParticles particles = new OutOfCircleParticles();
                        game.AtomsLayout.Children.Add(particles);
                        particles.TranslationX = x;
                        particles.TranslationY = y;
                        particles.Play(game.AtomsLayout, (int)(game.Delay / 1.25), game.Delay);

                        // Raise
                        game.AtomsLayout.Children.Remove((PlanetBase)game.Atoms[plusIndex]);
                        game.AtomsLayout.Children.Add((PlanetBase)game.Atoms[plusIndex]);


                        await Task.WhenAll(
                            ((PlanetBase)game.Atoms[i]).TranslateTo(x, y, (uint)game.Delay, Easing.SinIn),
                            ((PlanetBase)game.Atoms[j]).TranslateTo(x, y, (uint)game.Delay, Easing.SinIn),
                            Task.Run(async () => {
                                await Task.Delay((int)(game.Delay / 1.25));
                                await ((PlanetBase)game.Atoms[plusIndex]).ScaleTo(1.2 * Math.Pow(1.1, 1), (uint)(game.Delay / (4 * Math.Pow(1.25, 1))), Easing.SinIn);
                                particles.ScaleTo(1.2 * Math.Pow(1.1, 1), (uint)(game.Delay / (4 * Math.Pow(1.25, 1))), Easing.SinIn);
                                await ((PlanetBase)game.Atoms[plusIndex]).ScaleTo(1, (uint)game.Delay / 2, Easing.SinOut);
                            }));
                        // */
                        game.AtomsLayout.Children.Remove((PlanetBase)game.Atoms[j]);
                        game.Atoms.Remove((PlanetBase)game.Atoms[j]);
                        if (j < plusIndex) plusIndex--;
                        game.AtomsLayout.Children.Remove((PlanetBase)game.Atoms[i]);
                        game.Atoms.Remove((PlanetBase)game.Atoms[i]);
                        if (i < plusIndex) plusIndex--;


                        await Task.Delay(30); // optional - to make things look more fluid

                        plus.Type = 0;
                        plus.Text = (planetValue + 1).ToString();

                        game.CalculateDeletedOffset(tempOffset, plusIndex);

                        await game.MoveAtoms();

                        await game.MergeAtoms();


                        goto GoHere;
                    }
                }
            }
            GoHere:

            await Task.Delay(500);
            await this.FadeTo(0, 500);
            game.BG.HideAllEvents();
        }
        public bool EventHappening { get => false; }
    }
}