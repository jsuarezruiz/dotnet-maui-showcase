using System;
using System.Collections.Generic;
using System.Text;
using GalaxyLogicGame.Powerups;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    public class GameBGBase : ContentPage
    {


        // all powerups
        public IPowerup[] AllPowerups =
        {
            new KindlePowerup(),
            new Telescope(),
            new AtomicBomb()
        };
    }
}
