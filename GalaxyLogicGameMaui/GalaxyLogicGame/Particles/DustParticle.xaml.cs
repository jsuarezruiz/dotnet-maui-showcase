using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Particles
{

    public partial class DustParticle : AbsoluteLayout
    {
        private Image dust = new Image
        {
            Source = "dustParticle.png",
            TranslationX = 30,
            Opacity = 0,
        };
        public DustParticle()
        {
            InitializeComponent();
            this.Children.Add(dust);

        }
        public async Task Play()
        {
            Random random = new Random();
            this.TranslationX = random.Next(30);
            this.TranslationY = random.Next(60);
            dust.TranslationX = 30;
            if (dust.Opacity != 0) dust.Opacity = 0;
            await Task.WhenAny(dust.FadeTo(0.5, 1500),
                dust.TranslateTo(0, 0, 3000));
            await dust.FadeTo(0, 1500);
        }
    }
}