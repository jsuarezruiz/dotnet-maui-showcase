using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Particles
{

    public partial class StarsParticlesLayout : AbsoluteLayout
    {
        Button[] starsArray = new Button[40];

        public StarsParticlesLayout()
        {
            InitializeComponent();
            InitiateStars();
        }
        private void InitiateStars()
        {
            AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 360, Functions.GetScreenRatio() > 1 ? 360 * Functions.GetScreenRatio() : 360));
            Random random = new Random();
            for (int i = 0; i < starsArray.Length; i++)
            {

                starsArray[i] = new Button
                {
                    BackgroundColor = Color.FromArgb("fff"),
                    Opacity = random.NextDouble(),
                    TranslationX = random.Next(0, 360),
                    TranslationY = random.Next(0, (int)(720 * Functions.GetScreenRatio())),
                };

                this.Children.Add(starsArray[i]);
                AbsoluteLayout.SetLayoutBounds(starsArray[i], new Rect(0, 0, 1, 1));
            }
        }
        public async Task TransitionUpIn()
        {
            this.FadeTo(1, 500);
            for (int i = 0; i < starsArray.Length; i++)
            {
                starsArray[i].TranslateTo(starsArray[i].TranslationX, starsArray[i].TranslationY - 180, 500, Easing.SinIn);
                starsArray[i].ScaleYTo(30, 500, Easing.SinIn);
            }
            await Task.Delay(500);
        }
        public async Task TransitionUpOut()
        {
            for (int i = 0; i < starsArray.Length; i++)
            {
                starsArray[i].TranslateTo(starsArray[i].TranslationX, starsArray[i].TranslationY - 180, 500, Easing.SinOut);
                starsArray[i].ScaleYTo(1, 500, Easing.SinOut);

            }
            await Task.Delay(500);
            this.FadeTo(0, 500);
        }
    }
}