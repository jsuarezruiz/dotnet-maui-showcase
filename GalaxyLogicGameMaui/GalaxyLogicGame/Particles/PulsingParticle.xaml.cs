using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{

    public partial class PulsingParticle : AbsoluteLayout
    {
        private Button[] particles = new Button[2];
        private bool playing;
        private int xDirection;
        private int yDirection;
        private double angle; // in radians
        private double[] angularDistances = new double[2];
        public PulsingParticle()
        {
            InitializeComponent();
        }

        public void Move()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                angularDistances[i] = (angularDistances[i] + 0.017) % (Math.PI * 2);



                particles[i].ScaleTo(1 + Math.Sin(angularDistances[i]) / 3.0);
            }
        }

        public void Play()
        {
            for(int i = 0; i < particles.Length; i++) 
            {
                particles[i] = new Button
                {
                    BackgroundColor = Color.FromArgb("fff"),
                    Opacity = 0.5,
                    CornerRadius = 30,
                };

                mainLayout.Children.Add(particles[i]);
                AbsoluteLayout.SetLayoutBounds(particles[i], new Rect(0.5, 0.5, 60, 60));
                AbsoluteLayout.SetLayoutFlags(particles[i], AbsoluteLayoutFlags.PositionProportional);

                angularDistances[i] = i*270*0.017;
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
    }
}