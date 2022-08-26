using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;

namespace GalaxyLogicGame.Powerups
{

    public abstract partial class PowerupBase : AbsoluteLayout
    {
        private bool allowed = true;
        private int cooldown;
        private IGameBG gameBG;
        private IPowerupView powerupView;
        private int lastTurn = 0;
        private bool clicked = true;
        public PowerupBase()
        {
            InitializeComponent();
        }

        public int Cooldown { set { cooldown = value; lastTurn = -value; } }
        public void UpdateCooldown()
        {
            if (gameBG.Game.Turn - lastTurn < cooldown)
            {
                int height = (int)((double)(gameBG.Game.Turn - lastTurn) / cooldown * 60);
                //AbsoluteLayout.SetLayoutBounds(fill, );
                fill.Clip = new RectangleGeometry(new Rect(0, 60-height, 60, height));
            }
            else
            {
                fill.Clip = new RectangleGeometry(new Rect(0, 0, 60, 60));
                //AbsoluteLayout.SetLayoutBounds(fill, new Rect(15, 15, 60, 60));
                shiny.IsVisible = true;
            }
        }

        public bool IsAllowed { get { return allowed; } set { 
                allowed = value;
                if (value)
                {
                    shiny.Opacity = 1;
                    fill.Opacity = 1;
                }
                else
                {
                    shiny.Opacity = 0;
                    fill.Opacity = 0.5;
                }
            } }
        public string BGImage { set { bg.Source = value; } }
        public string FillImage { set { fill.Source = value; } }
        public string ShinyImage { set { shiny.Source = value; } }
        public abstract void Prerequisites();
        public IGameBG BG { get => gameBG; set { gameBG = value; } }
        public IPowerupView PowerupView { set { powerupView = value; } }

        public int LastTurn { get => lastTurn; set { lastTurn = value; } }
        public abstract void UsePowerupClicked();
        public abstract void SeePowerupDetailsClicked();
        
        public async void OnUsePowerupClicked(object sender, EventArgs args)
        {
            if (gameBG.Game.Turn - lastTurn >= cooldown && IsAllowed && gameBG.Game.Clicked && clicked)
            {
                clicked = false;

                shiny.IsVisible = false;
                if (powerupView != null) powerupView.Disappear();

                // Animation here

                UsePowerupClicked();
                lastTurn = BG.Game.Turn;
                UpdateCooldown();

                clicked = true;
            }
            else if (clicked)
            {
                //await Task.WhenAll(
                //    this.TranslateTo(0, -100, 750, Easing.CubicIn),
                //   this.ScaleTo(1.2, 750, Easing.CubicIn));
                SeePowerupDetailsClicked();
                // special animation
            }
        }
    }
}