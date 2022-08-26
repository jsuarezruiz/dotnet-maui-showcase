using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Particles
{

    public partial class ClickableAreaWithParticles : AbsoluteLayout
    {
        private Image[] particles = new Image[1];
        private bool playing;
        private int index;

        public ClickableAreaWithParticles()
        {
            InitializeComponent();
        }

        public void Move()
        {
            
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Scale -= 0.02;
                if (particles[i].Scale <= 0)
                {
                    particles[i].Scale = 1;
                }
            }
        }

        public void Play()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Image
                {
                    Source = "empty.png",
                    Scale = i * 0.5 + 0.5,
                };

                mainLayout.Children.Add(particles[i]);
                AbsoluteLayout.SetLayoutBounds(particles[i], new Rect(0.5, 0.5, 60, 60));
                AbsoluteLayout.SetLayoutFlags(particles[i], AbsoluteLayoutFlags.PositionProportional);
            }

            playing = true;
        }


        public async Task Stop(int duration)
        {
            playing = false;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].FadeTo(0, (uint)duration);
            }
            await Task.Delay(duration);

            for (int i = 0; i < particles.Length; i++)
            {
                mainLayout.Children.Remove(particles[i]);
            }
            
        }

        public bool IsPlaying { get => playing; }
        public int Index { get { return index; } set { index = value; } }
    }
}