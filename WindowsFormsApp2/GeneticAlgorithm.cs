using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class GeneticAlgorithm
    {
        public EventHandler bestPoint;

        private static Random random = new Random();
        private int populationSize;
        private double mutationRate;
        private int numberOfElitism;
        private int numberOfIterations;
        private double crossoverRate;




        List<Chromosome> population;
        List<Chromosome> currentGenerationbest;

        List<double> currentCommulativeProbability;

        public GeneticAlgorithm(int populationSize, double mutationRate, int numberOfElitism, int numberOfIterations, double crossoverRate, EventHandler bestPoint)
        {
            this.populationSize = populationSize;
            this.mutationRate = mutationRate;
            this.numberOfElitism = numberOfElitism;
            this.numberOfIterations = numberOfIterations;
            this.crossoverRate = crossoverRate;
            this.bestPoint = bestPoint;
            population = new List<Chromosome>();
            currentGenerationbest = new List<Chromosome>();
            currentCommulativeProbability = new List<double>();
        }

        public void doAlgorithm()
        {
            _initializeRandomStartValues();
            for (int i = 0; i < numberOfIterations; i++)
            {
                Console.WriteLine("ITERATION ::::" + i.ToString());

                _doSingleIteration(i);
            }
        }

        private void _initializeRandomStartValues()
        {
            Console.WriteLine("Initialization ----");
            if(population != null)
            population.Clear();
            Console.WriteLine(populationSize.ToString());

            for (int i = 0; i < populationSize; i++)
            {
                var _newChromosome = new Chromosome();
                population.Add(_newChromosome);
                Console.WriteLine(i.ToString() +" : "+_newChromosome.ToString());

            }

        }

        private void _doSingleIteration(int itterationNumber)
        {
            if(currentGenerationbest != null)
            {

            population.AddRange(currentGenerationbest);
                foreach (Chromosome chromosome in population)
                {
                Console.WriteLine(chromosome.ToStringAll());
                }

            }
            _evaluation();
            _selection();
            _pickCurrentGenerationBest(itterationNumber);
            _crossover();
            _mutation();
        }

        private void _evaluation()
        {
            foreach (Chromosome chromosome in population)
            {
                chromosome.calculateObjectiveFunction();
            }
        }

        private void _selection()
        {
            Console.WriteLine("Selection ----");

            double _totalFitness = 0;

            foreach (Chromosome chromosome in population)
            {
                _totalFitness += chromosome.calculateFitness();
                Console.WriteLine("fittness: " + chromosome.Fittness);
            }
            foreach (Chromosome chromosome in population)
            {
                chromosome.calculateProbability(_totalFitness);

            }
            currentCommulativeProbability.Clear();

            double _currentComulativeProbability  = 0.0;

            foreach (Chromosome chromosome in population)
            {
                _currentComulativeProbability += chromosome.ProbabilityForNextGeneration;
              currentCommulativeProbability.Add(_currentComulativeProbability) ;
                Console.WriteLine("comulative: " + _currentComulativeProbability);
            }

            _selectChromosomes();
        }

        private void _pickCurrentGenerationBest(int currentItteration)
        {
           if(currentGenerationbest != null)
            {
                currentGenerationbest.Clear();
            }
            population.Sort((a,b) => -a.Fittness.CompareTo(b.Fittness));
            for (int i = 0; i<numberOfElitism; i++)
            {
                currentGenerationbest.Add(population[i]);
                Console.WriteLine("Best "+ i.ToString() + " : " + population[i].Fittness.ToString());
            }

            if(bestPoint != null)
            {
                bestPoint(this, new PointEventArgs(currentItteration, population[0].Fittness));
            }
        }

       

        private void _crossover()
        {
            List<int> parents = new List<int>();
            List<Chromosome> newChromosomes = new List<Chromosome>();
            double[] R = new double[population.Count];
            _generateRandomCrossoverValues(R);
            _selectParentsForMating(parents, R);
            int[] crossoverPoints = new int[parents.Count()];
            if (crossoverPoints.Count() == 0) return;
            _generateCrossOverPoints(parents, crossoverPoints);
            Console.WriteLine("mating....");
            int crossoverFieldLength = 0;
            Chromosome newChromosome;
            for (int i = 0; i < parents.Count() - 1; i++)
            {
                _crossOverGenes(parents, newChromosomes, crossoverPoints, out crossoverFieldLength, out newChromosome, i);
            }
            _crossOverGenesForLastParent(parents, newChromosomes, crossoverPoints, out crossoverFieldLength, out newChromosome);

            int switchIndex = 0;
            foreach (int i in parents)
            {
                population[i].genes = newChromosomes[switchIndex].genes;
                switchIndex++;
            }
            Console.WriteLine("Ended mating....");

            Console.WriteLine(population[parents.First()].ToStringAll());

        }

        private static void _generateCrossOverPoints(List<int> parents, int[] crossoverPoints)
        {
            for (int i = 0; i < parents.Count; i++)
            {
                crossoverPoints[i] = random.Next(1, 30);
            }
        }

        private void _selectParentsForMating(List<int> parents, double[] R)
        {
            for (int i = 0; i < population.Count; i++)
            {
                if (R[i] < this.crossoverRate)
                {
                    parents.Add(i);
                    Console.WriteLine(population[i]);

                }
            }
        }

        private void _crossOverGenesForLastParent(List<int> parents, List<Chromosome> newChromosomes, int[] crossoverPoints, out int crossoverFieldLength, out Chromosome newChromosome)
        {
            crossoverFieldLength = 30 - crossoverPoints.Last();
            newChromosome = new Chromosome();
            Array.Copy(population[parents.Last()].genes, newChromosome.genes, 30);

            Console.WriteLine(newChromosome.ToStringAll());
            Console.WriteLine(crossoverPoints.Last());


            Array.Copy(population[parents[0]].genes, crossoverPoints.Last(), newChromosome.genes, crossoverPoints.Last(), crossoverFieldLength);
            Console.WriteLine(newChromosome.ToStringAll());

            newChromosomes.Add(newChromosome);
        }

        private void _crossOverGenes(List<int> parents, List<Chromosome> newChromosomes, int[] crossoverPoints, out int crossoverFieldLength, out Chromosome newChromosome, int i)
        {
            crossoverFieldLength = 30 - crossoverPoints[i];
            newChromosome = new Chromosome();
            Array.Copy(population[parents[i]].genes, newChromosome.genes, 30);

            Console.WriteLine(newChromosome.ToStringAll());
            Console.WriteLine(crossoverPoints[i]);


            Array.Copy(population[parents[i + 1]].genes, crossoverPoints[i], newChromosome.genes, crossoverPoints[i], crossoverFieldLength);
            Console.WriteLine(newChromosome.ToStringAll());
            Console.WriteLine("---------------------");


            newChromosomes.Add(newChromosome);
        }

        private void _selectChromosomes()
        {
            Console.WriteLine("Crossover ----");
            double[] R = new double[population.Count];
            _generateRandomCrossoverValues(R);
            Console.WriteLine(R.ToString());

            List<Chromosome> newPopulation = new List<Chromosome>();

            for (int i = 0; i < population.Count; i++)
            {

                int chromosomeToTakeItsPlace = -1;
                while (R[i] >currentCommulativeProbability[chromosomeToTakeItsPlace + 1])
                {
                    chromosomeToTakeItsPlace++;
                }
                Chromosome temp = population[chromosomeToTakeItsPlace + 1];
                Console.WriteLine(R[i].ToString() + " :::: " + currentCommulativeProbability[chromosomeToTakeItsPlace + 1].ToString());
                Console.WriteLine(population[i].ToString());
                newPopulation.Add(temp);
                Console.WriteLine(", CHANGED TO: " + (chromosomeToTakeItsPlace + 1).ToString() + ", Chrom:" + temp.ToString());

            }
            population.Clear();
            population.AddRange(newPopulation);
        }

        private static void _generateRandomCrossoverValues(double[] R)
        {
            for (int i = 0; i < R.Length; i++)
            {
                R[i] = random.NextDouble();
            }
        }

        private void _mutation()
        {
            Console.WriteLine("MUTATING");

            int totalLengthOfGenesInPopulation = population.Count * 30;
            int numberOfMutations = (int)Math.Round(totalLengthOfGenesInPopulation * mutationRate);
           
            for (int i = 0; i < numberOfMutations; i++)
            {
               int mutation = random.Next(-30,30);
               int mutationIndex = random.Next(1, totalLengthOfGenesInPopulation);

                int chromosomeIndex = mutationIndex / 30;
                int geneIndex = mutationIndex - (chromosomeIndex * 30);
                Console.WriteLine("index: " + geneIndex);

                Console.WriteLine(population[chromosomeIndex].ToStringAll());
                Console.WriteLine("mutation:" + mutation);

                population[chromosomeIndex].genes[geneIndex] = mutation;
                Console.WriteLine(population[chromosomeIndex].ToStringAll());
                Console.WriteLine("-----------------------");

            }



        }


    }
}
