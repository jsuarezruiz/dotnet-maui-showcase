using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;

namespace GalaxyLogicGame.Tutorial
{
    public class TutorialPlacingPlanets : TutorialGameBase
    {
        public TutorialPlacingPlanets()
        {

        }
        public override async Task SetupTutorial()
        {
            ArrayList tempArray = new ArrayList();
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = 0, Text = "6" });
            tempArray.Add(new Planet { Type = 0, Text = "4" });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });

            TutorialText("You play by placing planets between other planets in a logical way.", 1);

            await InitializeThisLayout(tempArray, 0);


            AddNewPlanet(5);
            MiddleParticle.Play();
        }
        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);



                await SecondTutorialText("Well done! ^^", new TutorialMergingPlanets());

                Clicked = true;
            }
        }
    }
}
