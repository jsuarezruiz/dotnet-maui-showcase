using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalaxyLogicGame
{
    public class Position
    {
        private double x;
        private double y;

        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Distance (Position p)
        {
            double temp = (Math.Abs(this.x - p.x)) * (Math.Abs(this.x - p.x)) + (Math.Abs(this.y - p.y)) * (Math.Abs(this.y - p.y));
            return Math.Sqrt(temp);
        }
        public void Add(Position p)
        {
            this.x += p.X;
            this.y += p.Y;
        }
        public void Difference(Position p)
        {
            this.x -= p.X;
            this.y -= p.Y;
        }
        public double X { get { return x; } }
        public double Y { get { return y; } }
    }
}
