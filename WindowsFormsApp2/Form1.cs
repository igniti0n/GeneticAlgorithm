using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private int populationSize = 10;
        private double crossOverRatio = 0.25;
        private double mutationRate = 0.25;
        private int numberOfElitism = 2;
        private int numberOfIterations = 10;


        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "10";
            textBox2.Text = "0.25";
            textBox3.Text = "0.25";
            textBox4.Text = "2";
            textBox5.Text = "10";

        }

        private void button1_Click(object sender, EventArgs e)



        {

            try
            {
                _parseTextFieldsToProperies();
                _executeAlgorithmAndPrepareGraph();

            }
            catch
            {
               
                MessageBox.Show("Not all inputs are valid!");
            }


        }

        private void _parseTextFieldsToProperies()
        {
            this.populationSize = int.Parse(textBox1.Text);
            if (populationSize <= 0) throw new FormatException();
            this.crossOverRatio = Double.Parse(textBox2.Text);
            if (crossOverRatio <= 0 || crossOverRatio > 1) throw new FormatException();
            this.mutationRate = Double.Parse(textBox3.Text);
            if (mutationRate <= 0 || mutationRate > 1) throw new FormatException();
            this.numberOfElitism = int.Parse(textBox4.Text);
            if (numberOfElitism >= populationSize) throw new FormatException();
            this.numberOfIterations = int.Parse(textBox5.Text);
            if (numberOfIterations <= 0) throw new FormatException();
        }

        private void _executeAlgorithmAndPrepareGraph()
        {
            var objChart = chart1.ChartAreas[0];
            objChart.AxisX.Minimum = 1;
            objChart.AxisX.Maximum = numberOfIterations;



            objChart.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            objChart.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;

            chart1.Series.Clear();
            chart1.Series.Add("Points");
            chart1.Series["Points"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            var _algorithm = new GeneticAlgorithm(populationSize, mutationRate, numberOfElitism, numberOfIterations, crossOverRatio, new EventHandler(_updateGraph));
            _algorithm.doAlgorithm();
        }

        private void _updateGraph(object sender, EventArgs e)
        {
            var eventArgs = e as PointEventArgs;
            Console.WriteLine("Event::::::::::::::::::::::::");

            Console.WriteLine("Itteration: " + eventArgs.Itteration+1);
            Console.WriteLine("Fittness: " + eventArgs.Fitness);
            chart1.Series["Points"].Points.AddXY(eventArgs.Itteration+1, eventArgs.Fitness);
           
        }

       
    }
}
