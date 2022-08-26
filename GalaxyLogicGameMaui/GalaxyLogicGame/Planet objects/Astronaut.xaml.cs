using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{

    public partial class Astronaut : AbsoluteLayout
    {
        
        private IGameBG bg;
        private double accelerationX;
        private double accelerationY;
        private int degre;
        public Astronaut()
        {
            InitializeComponent();
        }

        public void Appear(IGameBG bg)
        {
            this.bg = bg;
            Random random = new Random();
            degre = random.Next(360);
            Position startingPosition = CirclePosition.CalculatePosition(degre, 210);

            this.TranslationX = startingPosition.X;
            this.TranslationY = startingPosition.Y;

            RedirectAcceleration(startingPosition);

            astronaut.Rotation = random.Next(360);
            bg.Astronaut = this;

            bg.GameLayout.Children.Add(this);
        }
        public void Move()
        {
            this.TranslationX += accelerationX;
            this.TranslationY += accelerationY;

            // making sure he will disappear
            
            if (Functions.Distance(0, 0, this.TranslationX, this.TranslationY) > 180)
            {
                this.Opacity = 1 - (Functions.Distance(0, 0, this.TranslationX, this.TranslationY) - 180) / 30;
                if (Functions.Distance(0, 0, this.TranslationX, this.TranslationY) > 220) Disappear();
            }
            else if (this.Opacity != 1) this.Opacity = 1;
        }

        private void Disappear()
        {
            //bg.GameLayout.BackgroundColor = Color.Red; // for debugging purposes

            bg.Astronaut = null;
            bg.GameLayout.Children.Remove(this);
        }
        private void OnClicked(object sender, EventArgs e)
        {
            Random random = new Random();
            degre = random.Next(360);
            Position startingPosition = CirclePosition.CalculatePosition(degre, 210);

            RedirectAcceleration(startingPosition);
        }
        private void RedirectAcceleration (Position p)
        {
            Random random = new Random();

            accelerationX = -p.X / 105 + random.NextDouble() - 0.5;
            accelerationY = -p.Y / 105 + random.NextDouble() - 0.5;
        }
    }
}