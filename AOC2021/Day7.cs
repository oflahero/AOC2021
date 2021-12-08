using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2021
{
    static class Day7
    {
        static List<int> hPosList;
        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(7);
            hPosList = inputLines.ElementAt(0).Split(',').Select(n => int.Parse(n)).ToList();
        }

        public static void Part1()
        {
            Setup();
            // Must be just be median? Try and see:
            // Order:
            var median = Median();

            Console.WriteLine($"Median is {median}");

            var totalFuel = 0;
            foreach(var pos in hPosList)
            {
                var distance = median - pos;
                distance = (distance < 0) ? -distance : distance;
                totalFuel += distance;
            }

            Console.WriteLine($"Day 7 (1): Fuel bill: {totalFuel}");
        }

        static int Median()
        {
            hPosList.Sort();
            var median = 0;

            // Ignoring edge cases (null, zerolength, onelength)
            if (hPosList.Count % 2 == 0)
            {
                median = (hPosList[hPosList.Count / 2] + hPosList[(hPosList.Count / 2) - 1]) / 2;
            }
            else
            {
                median = hPosList[hPosList.Count / 2];
            }
            return median;
        }

        public static void Part2()
        {
            Setup();

            // Median no good here, and I can't see the insight shortcut. So... brute force??
            Dictionary<ulong, List<int>> positionCostAnalysis = new Dictionary<ulong, List<int>>();

            //var median = Median();

            // All possible values...
            var positionsBruteForce = Enumerable.Range(hPosList.Min(), hPosList.Max() - hPosList.Min() + 1);

            foreach(var consider_pos in positionsBruteForce)
            {
                // Calc the cost of moving every crab here to pos, and store in the dictionary
                ulong totalFuel = 0;
                foreach (var pos in hPosList)
                {
                    var distance = consider_pos - pos;
                    distance = (distance < 0) ? -distance : distance;
                    ulong increasingDistance = (ulong) Enumerable.Range(1, distance).Sum();

                    totalFuel += increasingDistance;
                }
                if (!positionCostAnalysis.ContainsKey(totalFuel))
                    positionCostAnalysis.Add(totalFuel, new List<int> { consider_pos });
                else
                    positionCostAnalysis[totalFuel].Add(consider_pos);
            }

            var leastFuel = positionCostAnalysis.Keys.Min();
            var leastPos = positionCostAnalysis[leastFuel];


            Console.WriteLine($"Day 7 (2): Fuel bill: {leastFuel}");

        }

        public static void Part2_Optimization()
        {
            Setup();

            // Median no good here, and I can't see the insight shortcut. So... brute force??
            Dictionary<long, List<int>> positionCostAnalysis = new Dictionary<long, List<int>>();

            //var median = Median();

            // All possible values...
            var positionsBruteForce = Enumerable.Range(hPosList.Min(), hPosList.Max() - hPosList.Min() + 1);

            // Optimize: precalculate all distance sums
            var maxDist = hPosList.Max();
            Dictionary<int, int> distanceCosts = new Dictionary<int, int>();
            distanceCosts.Add(0, 0);
            for (var i=1; i<=maxDist; i++)
            {
                distanceCosts.Add(i, Enumerable.Range(1, i).Sum());
            }

            foreach (var consider_pos in positionsBruteForce)
            {
                // Calc the cost of moving every crab here to pos, and store in the dictionary
                long totalFuel = 0;
                foreach (var pos in hPosList)
                {
                    var distance = consider_pos - pos;
                    distance = (distance < 0) ? -distance : distance;
                    var increasingDistance = distanceCosts[distance];

                    totalFuel += increasingDistance;
                }
                if (!positionCostAnalysis.ContainsKey(totalFuel))
                    positionCostAnalysis.Add(totalFuel, new List<int> { consider_pos });
                else
                    positionCostAnalysis[totalFuel].Add(consider_pos);
            }

            var leastFuel = positionCostAnalysis.Keys.Min();
            var leastPos = positionCostAnalysis[leastFuel];


            Console.WriteLine($"Day 7 (2, optimized): Fuel bill: {leastFuel}");
        }
    }
}
