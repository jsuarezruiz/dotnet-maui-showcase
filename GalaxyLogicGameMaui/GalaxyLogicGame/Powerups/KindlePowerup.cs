using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{
    public class KindlePowerup : PowerupBase, IPowerup
    {
        
        public KindlePowerup()
        {
            //BGColor = Color.Orange;
            Cooldown = 30;

            // more
        }

        public override void Prerequisites()
        {
            IsAllowed = BG.Game is GameWithEvents && ((GameWithEvents)BG.Game).Blindness != null;
        }

        public override async void UsePowerupClicked()
        { 
            await ((GameWithEvents)BG.Game).Blindness.Disappear();
            
            Prerequisites();
        }
        public override void SeePowerupDetailsClicked()
        {
            if (!(BG.Game is GameWithEvents)) FullscreenTitlePopup.Appear(BG.MainLayout, "Available only in eventful game mode", Color.FromArgb("fff"), 0.5);
            else if (((GameWithEvents)BG.Game).Blindness == null) FullscreenTitlePopup.Appear(BG.MainLayout, "There is no Blindness effect", Color.FromArgb("fff"), 0.5);
            // add details page
        }

        public bool Equiped { get => Preferences.Get("org.tizen.myApp.challenge10", false); set { Preferences.Set("kindleEquiped", value); } } // change to false
        public bool Owned { get => Preferences.Get("kindleOwned", true); set { Preferences.Set("kindleOwned", value); } } // change to false
    }
}
