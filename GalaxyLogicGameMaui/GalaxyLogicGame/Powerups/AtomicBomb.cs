using System;
using System.Collections.Generic;
using System.Text;
using GalaxyLogicGame.Events;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{
    class AtomicBomb : PowerupBase, IPowerup
    {
        public AtomicBomb()
        {
            //BGColor = Color.LightGreen;
            Cooldown = 40;
            BGImage = "AtomicBombBG.png";
            FillImage = "AtomicBombFill.png";
            ShinyImage = "AtomicBombShiny.png";
        }
        public override void Prerequisites()
        {
            if (BG.Game is GameWithEvents) IsAllowed = BG.Game.Atoms.Count > 1 && !((GameWithEvents)BG.Game).EventHappening;
            else IsAllowed = BG.Game.Atoms.Count > 1;
        }

        public override async void UsePowerupClicked()
        {
            AtomicBombEvent atomicBombEvent = new AtomicBombEvent();
            await atomicBombEvent.Appear(BG.Game);

            Prerequisites();
        }
        public override void SeePowerupDetailsClicked()
        {
            if (BG.Game.Atoms.Count  <= 1) FullscreenTitlePopup.Appear(BG.MainLayout, "You need to have at least 2 planets", Color.FromArgb("fff"), 0.5);
            // add details page maybe
        }

        public static bool Equiped { get => Preferences.Get(EthFunctions.GetEthereumContractAddress, false); /*Preferences.Get("org.tizen.myApp.challenge11", false);*/ set { Preferences.Set("atomicBombEquiped", value); } } // change to false
        public static bool Owned { get => Preferences.Get("atomicBombOwned", true); set { Preferences.Set("atomicBombOwned", value); } } // change to false
    }
}
