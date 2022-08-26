using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{

    public partial class ChallengeDescription : AbsoluteLayout
    {
        private int difficultyLevel;
        public ChallengeDescription()
        {
            InitializeComponent();
        }
        public bool Completed { set { if (value) completedTick.IsVisible = true; else completedTick.IsVisible = false; } }
        public string Title { set { title.Text = value; /*if (value.Length > 25) title.FontSize = 16; */ } get { return title.Text; } }
        public int DifficultyLevel { set { difficultyLevel = value;
                if (difficultyLevel == 1) { title.TextColor = Color.FromArgb("fff"); difficultyBG.Source = "challengesBackgroundEasy.png"; }
                else if (difficultyLevel == 2) { title.TextColor = Color.FromArgb("fff"); difficultyBG.Source = "challengesBackgroundNormal.png"; }
                else if (difficultyLevel == 3) { title.TextColor = Color.FromArgb("fff"); difficultyBG.Source = "challengesBackgroundHard.png"; }
            } }
        public string Description { set { description.Text = value; } get { return description.Text; } }
    }
}