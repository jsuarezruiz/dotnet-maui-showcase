using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{
    class Telescope : PowerupBase, IPowerup
    {

        public Telescope()
        {
            //BGColor = Color.Blue;
            Cooldown = 30;
        }

        public override void Prerequisites()
        {
            IsAllowed = true;
        }

        public override async void UsePowerupClicked()
        {
            if (BG.Game is GameWithEvents)
            {
                ((GameWithEvents)BG.Game).EventCounter = 0;
                if (((GameWithEvents)BG.Game).TIR != null)
                {
                    ((GameWithEvents)BG.Game).TIR.Disappear();
                }
            }
            BG.Game.SetNextPlanets();

            // animation


            await BG.ShowTelescopeView();

            //Prerequisites();
        }
        public override async void SeePowerupDetailsClicked()
        {
            if (BG.Game.TelescopeActivated) await BG.ShowTelescopeView();

            // add something (Maybe)
        }
        public bool Equiped { get => Preferences.Get("org.tizen.myApp.challenge3", false); set { Preferences.Set("kindleEquiped", value); } } // change to false
        public bool Owned { get => Preferences.Get("kindleOwned", true); set { Preferences.Set("kindleOwned", value); } } // change to false
    }
}
