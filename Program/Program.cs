namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using SimpleChart;

    public class Program
    {
        private static readonly Random _rndGen = new Random();

        public static void Main()
        {
            var chart = SimpleChart.LaunchInNewThread();
            chart.Title = "My funky chart";
            var a = chart.Title;
            chart.MinValueAxisY = 0.1;
            var b = chart.MinValueAxisY;
            chart.PointCountRescaleAxisX = 100;
            var c = chart.PointCountRescaleAxisX;
            chart.Animate = false;
            var d = chart.Animate;

            var values = PopulateChartWithValues(chart);
            Thread.Sleep(1000);
            chart.Clear();
            Thread.Sleep(1000);
            chart.SetSeries(values);
            Thread.Sleep(3000);
            chart.Close();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static List<double> PopulateChartWithValues(ISimpleChart chart)
        {
            var result = new List<double>();
            for (var i = 0; i < 600; i++)
            {
                if (i == 200) chart.Animate = true;
                if (i == 400) chart.Clear();
                var point = i < 100 ? 10 - _rndGen.NextDouble() : i < 200 ? 1 + _rndGen.NextDouble() : 5 + _rndGen.NextDouble();
                result.Add(point);
                //Task.Run(() => chart.AddPoint(point));
                chart.AddPoint(point);
                Thread.Sleep(10);
            }

            return result;
        }
    }
}
