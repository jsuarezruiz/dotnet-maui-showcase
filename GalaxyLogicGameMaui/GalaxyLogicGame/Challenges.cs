using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    /**
     *  This Class serves as a checker for challenges and saves its completion.
     */
    class Challenges
    {
        /**
         * Name: 6 next to each other
         * Goal: Have 6 planets with the same value next to each other
         */
        public static async Task Challenge1(AbsoluteLayout layout, ArrayList planets)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge1")) return;
            if (planets.Count < 6) return;

            int inRow = 1;
            string currentValue = ((PlanetBase)planets[0]).Text;
             
            for (int i = 1; i < planets.Count; i++)
            {
                if (((PlanetBase)planets[i]).Type == 0 && ((PlanetBase)planets[i]).Text.Equals(currentValue))
                {
                    inRow++;
                    if (inRow == 6) // Challenge Completed!
                    {
                        Preferences.Set("org.tizen.myApp.challenge1", true);
                        ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                        await popup.Play(layout, "6 next to each other");
                    }


                }
                else
                {
                    currentValue = ((PlanetBase)planets[i]).Text;
                    inRow = 1;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (((PlanetBase)planets[i]).Type == 0 && ((PlanetBase)planets[i]).Text.Equals(currentValue))
                {
                    inRow++;
                    if (inRow == 6) // Challenge Completed!
                    {
                        Preferences.Set("org.tizen.myApp.challenge1", true);
                        ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                        await popup.Play(layout, "6 next to each other");
                    }
                }
                else return;
            }
            
        }

        /**
         * Name: Star disappearance
         * Goal: Make a star disappear
         * 
         * Done in CasualGame - Called only if completed
         */
        public static async Task Challenge2(AbsoluteLayout layout) 
        {
            if (!Preferences.Get("org.tizen.myApp.challenge2", false))
            {
                Preferences.Set("org.tizen.myApp.challenge2", true);
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Star disappearance");
            }
        }

        /**
         * Name: The beginnings
         * Goal: Reach score of 500
         */
        public static async Task Challenge3(AbsoluteLayout layout, int score)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge3")) return;
            if (score >= 500) { Preferences.Set("org.tizen.myApp.challenge3", true);
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "The beginnings");
            }
        }

        /**
         * Name: Scoring high
         * Goal: Reach score of 1000
         */
        public static async Task Challenge4(AbsoluteLayout layout, int score)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge4")) return;
            if (score >= 1000) { Preferences.Set("org.tizen.myApp.challenge4", true);
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Scoring high");
            }
            
        }

        /**
         * Name: To the moon!
         * Goal: Reach score of 2000
         */
        public static async Task Challenge5(AbsoluteLayout layout, int score)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge5")) return;
            if (score >= 2000)
            { 
                Preferences.Set("org.tizen.myApp.challenge5", true);
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "To the moon!");
            }
        }

        /**
         * Name: Bigger than Jupiter
         * Goal: Get a planet with value 15
         */
        public static async Task Challenge6(AbsoluteLayout layout, int value)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge6")) return;
            if (value >= 15) { Preferences.Set("org.tizen.myApp.challenge6", true); 
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Bigger than Jupiter");
            
            }
        }
        /**
         * Name: Bigger than the Sun
         * Goal: Get a planet with value 20
         */
        public static async Task Challenge7(AbsoluteLayout layout, int value)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge7")) return;
            if (value >= 20) { Preferences.Set("org.tizen.myApp.challenge7", true); 
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Bigger than the Sun");
            }
        }

        /**
         * Name: ´Bigger than a blackhole
         * Goal: Get a planet with value 21 - Better then the developer
         */
        public static async Task Challenge8(AbsoluteLayout layout, int value)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge8")) return;
            if (value >= 25) { Preferences.Set("org.tizen.myApp.challenge8", true); 
                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Bigger than a blackhole");
            }
        }
        /**
         * Name: Huge galaxy
         * Goal: All planets in your galaxy must have two-digit values
         */
        public static async Task Challenge9(AbsoluteLayout layout, ArrayList planets)
        {
            if (Preferences.ContainsKey("org.tizen.myApp.challenge9")) return;

            bool maybeTrue = false;
            for (int i = 0; i < planets.Count; i++)
            {
                if (((PlanetBase)planets[i]).Type == 0 && Functions.GetAtomValue((PlanetBase)planets[i]) < 10) return;
                if (((PlanetBase)planets[i]).Type == 0 && Functions.GetAtomValue((PlanetBase)planets[i]) >= 10) maybeTrue = true;
            }
            if (maybeTrue)
            {
                Preferences.Set("org.tizen.myApp.challenge9", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Huge galaxy");
            }
        }
        /**
         * Name: The end?
         * Goal: Make a galaxy empty somehow
         * 
         * Done in CasualGame - Called only if completed
         */
        public static async Task Challenge10(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge10", false))
            {
                Preferences.Set("org.tizen.myApp.challenge10", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "The end?");
            }
        }


        /**
         * Name: This is trash
         * Goal: Have 3 debris in your galaxy
         * 
         * Done in GameWithEvents (BinaryEvent) - Called only if completed
         */
        public static async Task Challenge11(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge11", false))
            {
                Preferences.Set("org.tizen.myApp.challenge11", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "This is trash");
            }
        }

        /**
         * Name: Wait for it to disappear
         * Goal: Wait for a shrinking giant to reach value 1
         * 
         * Done in CasualGame - Called only if completed
         */
        public static async Task Challenge12(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge12", false))
            {
                Preferences.Set("org.tizen.myApp.challenge12", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Wait for it to disappear");
            }
        }



        /**
         * Name: Combo
         * Goal: combine 8 planets at once with only +
         * 
         * Done in CasualGame - Called only if completed
         */
        public static async Task Challenge13(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge13", false))
            {
                Preferences.Set("org.tizen.myApp.challenge13", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Combo");
            }
        }

        /**
         * Name: Jumbo combo
         * Goal: combine 12 planets at once with only +
         * 
         * Done in CasualGame - Called only if completed
         */
        public static async Task Challenge14(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge14", false))
            {
                Preferences.Set("org.tizen.myApp.challenge14", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Jumbo combo");
            }
        }

        /**
         * Name: Dustman
         * Goal: Use atomic bomb power-up to get rid of a debris
         * 
         * Done in AtomicBombEvent - Called only if completed
         */
        public static async Task Challenge15(AbsoluteLayout layout)
        {
            if (!Preferences.Get("org.tizen.myApp.challenge15", false))
            {
                Preferences.Set("org.tizen.myApp.challenge15", true);

                ChallengeCompletedPopup popup = new ChallengeCompletedPopup();
                await popup.Play(layout, "Dustman");
            }
        }
    }
}
