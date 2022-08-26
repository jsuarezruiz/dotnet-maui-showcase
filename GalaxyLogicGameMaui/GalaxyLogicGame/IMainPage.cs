using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyLogicGame
{
    public interface IMainPage
    {
        int Highscore { get; set; }
        string HighscoreLabel { get; set; }
        bool IsUltra { get; }
        //Game Game { get; set; }
    }
}
