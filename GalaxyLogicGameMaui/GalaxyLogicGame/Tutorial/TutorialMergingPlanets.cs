using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Tutorial
{
    class TutorialMergingPlanets : TutorialGameBase
    {
        public override async Task SetupTutorial()
        {
            ArrayList tempArray = new ArrayList();
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = 0, Text = "3" });
            tempArray.Add(new Planet { Type = 0, Text = "3" });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });
            tempArray.Add(new Planet { Type = -1 });

            await TutorialText("Put \"+\" between planets with the same value and they will merge forming a bigger one.", 1);

            InitializeThisLayout(tempArray, 0);


            AddNewPlanet(new Planet { Type = 1, Text = "+", BGColor = Color.FromArgb("f00"), TextColor = Functions.DetermineWhiteOrBlack(Color.FromArgb("f00")) });
            //MiddleParticle.Play();
        }
        public override async Task AreaClicked(int index)
        {
            if (Clicked)
            {
                Clicked = false;

                await AddingPlanet(index);

                await MergeAtoms();

                await SecondTutorialText("See? It's easy.\nNow you will learn something more complex.", new TutorialMergingMultiplePlanets());
    
                Clicked = true;
            }
        }
    }
    
}
