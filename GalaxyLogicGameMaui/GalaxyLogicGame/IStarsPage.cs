using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Particles;

namespace GalaxyLogicGame
{
    public interface IStarsPage
    {
        Task TransitionIn();
        void AssingStars(StarsParticlesLayout stars);
    }
}
