using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    partial class Blackhole : AbsoluteLayout
    {
        
        public Blackhole()
        {
            InitializeComponent();
        }

        public double RotateBlackhole { set {
                bh.Rotation = value % 720;
                if (bh.Rotation % 50 == 0)
                {
                    BlackholeParticles particles = new BlackholeParticles();
                    mainLayout.Children.Add(particles);
                    particles.Play(mainLayout, 1300);
                }
                if (bh.Rotation % 134 == 0)
                {
                    BlackholeParticles particles = new BlackholeParticles();
                    mainLayout.Children.Add(particles);
                    particles.Play(mainLayout, 2200);
                }
                if (bh.Rotation % 66 == 0)
                {
                    BlackholeParticles particles = new BlackholeParticles();
                    mainLayout.Children.Add(particles);
                    particles.Play(mainLayout, 500);
                }

            } get { return bh.Rotation; } }
    }
}