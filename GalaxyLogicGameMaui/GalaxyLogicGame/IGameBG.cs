using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Events.EventChallengesBoards;
using GalaxyLogicGame.Planet_objects;
using GalaxyLogicGame.Powerups;
using GalaxyLogicGame.Tutorial;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    public interface IGameBG
    {
        List<PowerupBase> Powerups { get; }
        CasualGame Game { get; }
        void ResetTime();
        void UpdateScore();
        void GameOver();
        Task LostScreenAnimation();
        void StartLoop();
        void StopLoop();
        Task Setup(CasualGame game);
        Task NavigateToOtherTutorial(TutorialGameBase game);
        AbsoluteLayout LowerUILayout { get; set; }
        AbsoluteLayout MainLayout { get; set; }
        
        AbsoluteLayout BackgroundLayout { get; set; }
       
        AbsoluteLayout TransitionLayout { get; set; }
        AbsoluteLayout LostScreen { get; }
        Label LostScreenText { get; }
        Label LostScreenButton { get; }
        Image BackgroundImage { get; }
        AbsoluteLayout TutorialLayout { get; }
        AbsoluteLayout LowerEventLayout { get; }
        AbsoluteLayout EventLayout { get; }
        Task ShowTelescopeView();

        void ShowEvent(AbsoluteLayout layout);
        void HideAllEvents();
        IBoard Board { set; }
        Astronaut Astronaut { set; }
        AbsoluteLayout GameLayout { get; }
        bool PowerupsAllowed { set; }
        AbsoluteLayout PowerUpAnimationLayout { get; }

    }
}
