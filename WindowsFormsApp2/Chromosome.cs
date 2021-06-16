using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Chromosome
    {
        private static Random random = new Random();
        private int[] _genes   = new int[30];
        private double fittness;
        private double currentObjectiveFunctionValue = 0;
        private double probabilityForNextGeneration;
        //private double currentCumulativeProbability;

        public int[] genes { get => _genes; set => _genes = value; }
        public double ProbabilityForNextGeneration { get => probabilityForNextGeneration; set => probabilityForNextGeneration = value; }
        //public double CurrentCumulativeProbability { get => currentCumulativeProbability; set => currentCumulativeProbability = value; }
        public double Fittness { get => fittness; set => fittness = value; }

        override
        public String ToString()
        {
            return _genes[0].ToString() + "," + _genes[1].ToString() + "," + _genes[2].ToString() + "," + _genes[3].ToString() + "," + "....";
        }

        public String ToStringAll()
        {
            String field = "";
            foreach (int gene in _genes)
            {
                field += gene.ToString()+",";
            }
            return field;
        }

        public Chromosome()
        {
           
            for (int i = 0; i < 30; i++)
            {
                _genes[i] = random.Next(-30, 30);
            }
        }

        public void setInitialGenes()
        {

        }
        public void calculateProbability(double chromosomesTotalFitness)
        {
            this.probabilityForNextGeneration = this.fittness / chromosomesTotalFitness;
        }
        public double calculateFitness()
        {
            this.fittness = 1 / (1 + currentObjectiveFunctionValue);
            return this.fittness;
        }
        public void calculateObjectiveFunction()
        {
            for (int i = 0; i < 29; i++)
            {
                double _firstNumber = (_genes[i + 1] - (_genes[i] * _genes[i]));
                double _secondNumber = _genes[i] - 1;
                this.currentObjectiveFunctionValue += (100 * _firstNumber * _firstNumber + _secondNumber * _secondNumber);
            }
        }



    }
}
