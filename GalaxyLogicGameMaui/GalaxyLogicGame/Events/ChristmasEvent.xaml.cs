using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{

    public partial class ChristmasEvent : AbsoluteLayout, IEvent, IEventInfo
    {
        public const string NAME = "ChristmasEvent";

        private IEvent[] eventsChoice = new IEvent[2];
        private EventDescription[] descriptions = new EventDescription[2];
        private ArrayList events = new ArrayList();


        public ChristmasEvent()
        {
            InitializeComponent();
        }
        public string GetTitle => eventTitle.Text;
        public Color GetColor => eventTitle.TextColor;
        public ImageSource GetIcon => eventIcon.Source;
        public string Name => NAME;
        private bool IsActivated(GameWithEvents game)
        {
            return game.CustomMode && Preferences.Get(NAME, true);
        }
        public EventDescription GetEventDescription => new EventDescription
        {
            Title = eventTitle.Text, TitleColor = eventTitle.TextColor, IconImageSource = eventIcon.Source,
            Description = "A special time exclusive event that's available until january 20th 2022. Enjoy the presents and time with your family. <3"
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

             
            for (int i = 0; i < 8; i++)
            {
                events.Add(i);
            }


            for (int i = 0; i < 2; i++)
            {

                IEvent currentEvent = RecursiveEventPrerequisites(game);


                EventDescription eventDescription = currentEvent.GetEventDescription;

                eventDescription.TranslationX = 360;
                eventDescription.TranslationY = 120;

                eventDescription.Rotation = 70;

                eventsChoice[i] = currentEvent;
                descriptions[i] = eventDescription;
                eventDescription.Index = i;
                choiceLayout.Children.Add(eventDescription);

                double transform = i;
                if (i == 0)
                {
                    transform = -0.33;
                }
                eventDescription.TranslateTo(transform * offsetBetweenPlanets, 120, 500, Easing.SpringOut);
                await Task.Delay(200);

                eventDescription.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await OnClicked(game, eventDescription.Index)) });

            }
            await Task.Delay(350);
            protectiveLayout.IsVisible = false;
            game.EventHappening = game.Clicked = true; // escaping the barrier
        }
        private IEvent RecursiveEventPrerequisites(GameWithEvents game)
        {
            Random random = new Random();
            
            IEvent tempEvent;
            if (events.Count != 0)
            {
                int index = random.Next(events.Count);
                tempEvent = GetEvent((int)events[index]);
                events.RemoveAt(index);
            }
            else
                tempEvent = GetEvent(-1);
            //int index = ;
            

            if (!tempEvent.Prerequisites(game))
            {
                return RecursiveEventPrerequisites(game);
            }
            else
            {
                return tempEvent;
            }
        }
        public IEvent GetEvent(int i)
        {
            if (i == 0)
            {
                return new ShootingStarEvent();
            }
            else if (i == 1)
            {
                return new BlindnessEvent();
            }
            else if (i == 2)
            {
                return new ThreeInRowEvent();
            }
            else if (i == 3)
            {
                return new Polymorph();
            }
            else if (i == 4)
            {
                return new BinaryEvent();
            }
            else if (i == 5)
            {
                return new AstronautEvent();
            }
            else if (i == 6)
            {
                return new DreamsEvent();
            }
            else if (i == 7)
            {
                return new AtomicBombEvent();
            }
            else
            {
                // Error
                //AtomsLayout.BackgroundColor = Color.Green;
                return new NothingEvent();
            }
        }
        private async Task OnClicked(GameWithEvents game, int index)
        {
            if (game.Clicked)
            {
                game.Clicked = false;

                for (int i = 0; i < descriptions.Length; i++)
                {
                    descriptions[i].TranslateTo(descriptions[i].TranslationX, 360, 350, Easing.SpringIn);
                    if (i == 0) await Task.Delay(200);
                }
                //await Task.Delay(200);

                await mainLayout.FadeTo(0, 500);
                //await planetsChoice[index].TranslateTo(0, 0, 250);

                game.BG.HideAllEvents();

                if (eventsChoice[index] is AtomicBombEvent)
                {
                    game.GenerateNewAtom();
                }
                await eventsChoice[index].Appear(game);

                while (eventsChoice[index].EventHappening){
                    await Task.Delay(500);
                }
                if (!(eventsChoice[index] is ShootingStarEvent || eventsChoice[index] is ThreeInRowEvent || eventsChoice[index] is AtomicBombEvent || eventsChoice[index] is Polymorph)) game.GenerateNewAtom();
                game.EventHappening = false;
                
            }

        }
        public bool EventHappening { get => true; } // useless

    }
}