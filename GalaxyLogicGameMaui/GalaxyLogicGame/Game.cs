using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{

    public abstract class Game : AbsoluteLayout
    {
        private bool clicked = true; // false = true (does not make sense, but it does)
        private IMainPage mainPage;
        private IGameBG bg;
        public abstract Task Setup();
        public abstract int Heighest { get; set; }


        public abstract Task InitializeThisLayout(ArrayList array, double offset);
        public abstract Task AreaClicked(int index);
        public bool Clicked { get => clicked; set { clicked = value; bg.ResetTime(); } }

        public IGameBG BG { get => bg; set { bg = value; } }
        public IMainPage MainMenuPage { get { return mainPage; } set { mainPage = value; } }
    }

}
