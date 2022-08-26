using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaxyLogicGame;

namespace GalaxyLogicGame
{
    class CirclePosition
    {
        public const double DEGREES = 360;
        public const int SCREEN_WIDTH = 360;
        public const int ICON_SIZE = 60;
        public static double CalculateDegre(int count)
        {
            if (count == 0) return 0;
            return DEGREES / count;
        }
        
        public static ArrayList GetCirclePositions(int count)
        {


            ArrayList circlePositions = new ArrayList();
            double degre = DEGREES / count;
            int distance = (SCREEN_WIDTH - ICON_SIZE) / 2;
            //Position center = new Position(distance, distance);

            for (int i = 0; i < count; i++)
            {
                double angle = ConvertDegreToRadian(i * degre);
                Position p = new Position(Math.Sin(angle) * distance, Math.Cos(angle) * distance);
                //Position p = new Position(80, 80);
                circlePositions.Add(p);
            }



            return circlePositions;
        }
        public static ArrayList GetCirclePositionsOffset(int count, double offset)
        {
            // specs

            ArrayList circlePositions = new ArrayList();
            double degre = DEGREES / count;
            int distance = (SCREEN_WIDTH - ICON_SIZE) / 2;
            //Position center = new Position(distance, distance);


            for (int i = 0; i < count; i++)
            {
                double angle = ConvertDegreToRadian(i * degre + offset);
                //Position p = new Position(80, 80);
                circlePositions.Add(CalculatePosition(angle, distance));
            }



            return circlePositions;
        }


        public static Position CalculatePosition(double angle, double distance)
        {
            return new Position(Math.Sin(angle) * distance, Math.Cos(angle) * distance);
        }
        public static Position CalculatePosition(double angle, int distance)
        {
            return new Position(Math.Sin(angle) * distance, Math.Cos(angle) * distance);
        }
        public static Position CalculatePosition(double angle)
        {
            int distance = (SCREEN_WIDTH - ICON_SIZE) / 2;
            return new Position(Math.Sin(angle) * distance, Math.Cos(angle) * distance);
        }
        public static double ConvertDegreToRadian(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}