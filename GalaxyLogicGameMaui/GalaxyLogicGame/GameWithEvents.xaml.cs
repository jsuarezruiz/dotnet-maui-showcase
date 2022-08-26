using GalaxyLogicGame.Events;
using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Planet_objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using GalaxyLogicGame.Types;

namespace GalaxyLogicGame
{
    public partial class GameWithEvents : CasualGame
    {
        private Gamemode gamemode = Gamemode.Clasic;
        private int eventCounter;
        private bool generateNewPlanet = true;

        private bool eventHappening = false;
        private bool customMode = false;

        //private IEvent[] allEvents = { new ShootingStarEvent() };
        private ArrayList events;
        private IEvent tempEvent;
        private Random random = new Random();
        private BlindnessEvent blindness;
        private ThreeInRowEvent tir;
        private BinaryEvent binary;
        private PolymorphParticle polymorphParticle;

        private IEventObject eventObject;
        public GameWithEvents()
        {
            InitializeComponent();
        }
        public GameWithEvents(Gamemode gamemode)
        {
            InitializeComponent();
            this.gamemode = gamemode;
        }
        public override async Task Setup()
        {
            //eventCounterLayout.IsVisible = true;
            Random random = new Random();
            
            eventCounter = random.Next(-10, -3); // makes you fucking lucky

            await base.Setup();
        }

        public override async Task AreaClicked(int index)
        {
            //((BoxViewWithIndex)clickableAreas[index]).BackgroundColor = Color.Orange;
            if (Clicked)
            {
                Clicked = false;

                TurnPlusPlus();

                await AddingPlanet(index);
                if (binary != null) await binary.CheckFormation();

                //await Task.Delay(delay);
                await MergeAtoms();

                await StarBehaviour();

                await CheckChallenges();
                // the total end part

                BG.GameOver(); // this needs to be improved // probably already done :D


                generateNewPlanet = true;

                //if (binary != null) await binary.MoveMade(); // probably not needed
                if (eventObject != null) await eventObject.Move(); // hopefully this works - BTW this replaces the line above


                if (!TelescopeActivated) await DoEventStuff(); // can change the value of generateNewPlanet to false


                await UpdateShrinkingGiants();
                if (generateNewPlanet) GenerateNewAtom();
                //AddClickableAreasToLayout(); // maybe useless, delete later

                await BG.LostScreenAnimation();

                Clicked = true;
            }
        }

        private async Task DoEventStuff()
        {
            eventCounter++;


            await SetEventCounterLayout();
            


            
        }

        private async Task SetEventCounterLayout()
        {
            /*
            if (eventCounter < 0)
            {
                EventCounterLayout.IsVisible = false;
            }
            else
            {
                EventCounterLayout.IsVisible = true;
            }
            */
            await DetermineEvent();
        }
        private async Task DetermineEvent()
        {
            
            if (eventCounter < 3)
            {
                TIRNewPlanet();
                return;

            }

            eventCounter = -1; // default value - can differ depending on the event

            //if (blindness != null) await blindness.Disappear(this);
            //else if (tir != null) await tir.Disappear(this); // this will get changed ...

            //if (eventObject != null) await eventObject.Move(); // hopefully this works - BTW this replaces the 2 lines above

            events = SetAllEventsArrayList(); // will fill with default index values

            RecursiveEventPrerequisites();

            if (tempEvent is AtomicBombEvent)
            {
                GenerateNewPlanet = false;
                GenerateNewAtom();
            }
            await tempEvent.Appear(this);

            CheckPowerupPrerequisites();
            // this is needed to fully await the event procedure
            while (eventHappening)
            {
                await Task.Delay(500);
            }
            
            
        }
        public void TIRNewPlanet()
        {
            if (tir != null) // Three in row
            {
                generateNewPlanet = false;
                NewPlanet = new Planet
                {
                    Type = 0,
                    Text = tir.Value.ToString()
                };
                StarCountdown = true;
                PlusWaiting++;
                AtomsLayout.Children.Add(NewPlanet);
                MiddleParticle.Play();


                AddClickableAreasToLayout();
            }
        }
        private void RecursiveEventPrerequisites()
        {

            int index;
            if (events.Count != 0)
            {
                index = random.Next(events.Count);
                tempEvent = GetEvent((int)events[index]);
            }
            else { 
                index = -1;
                tempEvent = GetEvent(-1);
            }   
            //int index = ;
            

            if (!tempEvent.Prerequisites(this))
            {
                events.RemoveAt(index);
                RecursiveEventPrerequisites();
            }
        }
        private IEvent GetEvent(int i)
        {
            if (i == 0)
            {
                //return new BlueberriesEvent();
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
            else if (i == 8)
            {
                return new ChristmasEvent();
            }
            else if (i == 9)
            {
                return new WorldUpsideDownEvent();
            }
            else if (i == 10)
            {
                return new BlueberriesEvent();
            }
            else if (i == 11)
            {
                return new WorldUpsideDownEvent();
            }
            else
            {
                //AtomsLayout.BackgroundColor = Color.Green; // for debugging purposes
                return new NothingEvent();
            }
        }
        public ArrayList SetAllEventsArrayList()
        {
            ArrayList tempEvents = new ArrayList();
            for (int i = 0; i < 12; i++)
            {
                tempEvents.Add(i);
            }
            return tempEvents;
        }

        public override async Task MoveAtoms()
        {
            await base.MoveAtoms();

            if (binary != null) await binary.CheckFormation();
            blindness?.Update(this);

        }

        public override void LoopTick()
        {
            base.LoopTick();

            if (polymorphParticle != null)
            {
                polymorphParticle.Move();
            }
        }

        public bool EventHappening { get => eventHappening; set { eventHappening = value; BG.PowerupsAllowed = !value; } }
        public int EventCounter { get => eventCounter; set { eventCounter = value; } }
        public BlindnessEvent Blindness { get => blindness; set { blindness = value; } }
        public BinaryEvent Binary { get => binary; set { binary = value; } }
        public ThreeInRowEvent TIR { get => tir; set { tir = value; } }
        public bool GenerateNewPlanet { get => generateNewPlanet; set { generateNewPlanet = value; } }
        public PolymorphParticle PolyParticle { get => polymorphParticle; set { polymorphParticle = value; } }

        public override bool BinaryActivated => binary != null;
        public IEventObject EventObject { set { eventObject = value; } get => eventObject; }
        public bool CustomMode { get => customMode; set { customMode = value; } }
        public Gamemode Gamemode { get => gamemode; }
    }
}