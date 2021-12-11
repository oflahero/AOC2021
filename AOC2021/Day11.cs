using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day11
    {
        static int[,] OctopusGrid;
        static int FlashCount, StepFlashCount = 0;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(11).ToList();
            OctopusGrid = new int[inputLines.Count, inputLines.First().Length];
            for (var i = 0; i < inputLines.Count; i++)
                for (var j = 0; j < inputLines[i].Length; j++)
                    OctopusGrid[i, j] = int.Parse(inputLines[i].Substring(j, 1));
        }
        public static void Part1()
        {
            Setup();
            FlashCount = 0;
            foreach (var step in Enumerable.Range(1, 100))
                Step();
            Console.WriteLine($"Day 11(1): flash count {FlashCount}");
        }

        public static void Part2()
        {
            Setup();
            FlashCount = 0;
            var step = 1;
            for (; step <= 10000; step++)
            {
                Step();
                if (StepFlashCount == OctopusGrid.GetLength(0) * OctopusGrid.GetLength(1))
                    break;
            }
            Console.WriteLine($"Day 11(2): simultaneous flash step {step}");
        }

        // Nothing clever here
        static void Step()
        {
            StepFlashCount = 0;
            // Basic increment
            for (var i = 0; i < OctopusGrid.GetLength(0); i++)
                for (var j = 0; j < OctopusGrid.GetLength(1); j++)
                {
                    Boost(i, j);
                }
            // Turn off flashes
            for (var i = 0; i < OctopusGrid.GetLength(0); i++)
                for (var j = 0; j < OctopusGrid.GetLength(1); j++)
                {
                    if (OctopusGrid[i,j] >= 10)
                    {
                        OctopusGrid[i, j] = 0;
                    }
                }
        }

        static void Boost(int i, int j)
        {
            OctopusGrid[i, j]++;
            if (OctopusGrid[i,j]==10)
            {
                FlashCount++;
                StepFlashCount++;

                if (i>0)
                {
                    if (j>0) Boost(i - 1, j - 1);
                    if (j < OctopusGrid.GetLength(1) - 1) Boost(i - 1, j + 1);
                    Boost(i - 1, j);
                }
                if (j > 0) Boost(i, j - 1);
                if (j < OctopusGrid.GetLength(1) - 1) Boost(i, j + 1);
                if (i < OctopusGrid.GetLength(0)-1)
                {
                    if (j > 0) Boost(i + 1, j - 1);
                    if (j < OctopusGrid.GetLength(1) - 1) Boost(i + 1, j + 1);
                    Boost(i + 1, j);
                }
            }
        }
    }
}
