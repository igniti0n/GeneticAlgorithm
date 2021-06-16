using System;

namespace WindowsFormsApp2
{
    internal class PointEventArgs : EventArgs
    {
       private int itteration;
       private double fitness;

        public PointEventArgs(int itteration, double fitness)
        {
            this.itteration = itteration;
            this.fitness = fitness;
        }

        public int Itteration { get => itteration;  }
        public double Fitness { get => fitness; }
    }
}