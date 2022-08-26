using System;
using System.Collections;
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

    public partial class DreamsEvent : AbsoluteLayout, IEvent, IEventObject, IEventInfo
    {
        public const string NAME = "DreamsEvent";

        private GameWithEvents game;
        private int duration = 5;
        private Image dreamsBG = new Image
        {
            Source = "dreamsWallpaper.png",
            Opacity = 0
        };
        public DreamsEvent()
        {
            InitializeComponent();
        }
        public Label EventTitle => eventTitle;
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
            Description = "Randomly raises 3 planets (number in a cloud). Be sure to take advantage of that while you are still dreaming."
        };
        public bool Prerequisites(GameWithEvents game)
        {
            int counter = 0;
            if (game.Atoms.Count >= 4)
            {
                for (int i = 0; i < game.Atoms.Count; i++)
                {
                    if (((PlanetBase)game.Atoms[i]).Type == 0) counter++;
                }
            }
            return counter >= 3 && IsActivated(game);

        }
        public async Task Appear(GameWithEvents game)
        {
            this.game = game;
            game.EventObject = this;
            Random random = new Random();

            if (Device.RuntimePlatform == Device.Tizen) { 
                game.BG.BackgroundLayout.Children.Add(dreamsBG);
                dreamsBG.FadeTo(1, 500);
            }
                
            else game.BG.ShowEvent(this);


            //game.Dreams = this;
            game.EventCounter = -2; // change this

            ArrayList availablePlanets = new ArrayList();
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                if (((PlanetBase)game.Atoms[i]).Type == 0) availablePlanets.Add(game.Atoms[i]);
            }

            // generate dream numbers
            for (int dreamNum = 1; dreamNum <= 3; dreamNum++)
            {
                int index = random.Next(availablePlanets.Count);
                ((Planet)availablePlanets[index]).DreamNumber = dreamNum;
                availablePlanets.RemoveAt(index);
            }



            await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);

            game.BG.HideAllEvents();
            game.BlindnessLayout.Children.Add(this);

            await game.MergeAtoms();
        }
        public async Task Disappear()
        {
            // removing dream numbers
            for (int i = 0; i < game.Atoms.Count; i++)
            {
                if (((PlanetBase)game.Atoms[i]).Type == 0) ((Planet)game.Atoms[i]).DreamNumber = 0;
            }

            await Task.WhenAll(
                this.FadeTo(0, 250),
                dreamsBG.FadeTo(0, 500));
            game.BlindnessLayout.Children.Remove(this);
            game.EventObject = null;
            game.BG.BackgroundLayout.Children.Remove(dreamsBG);
        }
        public async Task Move()
        {
            duration--;
            if (duration == 0)
            {
                await Disappear();
            }
        }
        public bool EventHappening { get => false; }

    }
}