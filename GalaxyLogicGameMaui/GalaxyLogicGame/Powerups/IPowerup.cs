using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyLogicGame.Powerups
{
    public interface IPowerup
    {
        void Prerequisites();
        static bool Equiped { get; set; }
        static bool Owned { get; set; }
    }
}
