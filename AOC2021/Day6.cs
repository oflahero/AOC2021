using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day6
    {
        static List<int> fish;
        // or, more cleverly:
        static UInt64[] fishDayIndex; // 9 entries, day 0 to day 8 counts

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(6); // new List<string>() { "3,4,3,1,2" }; 
            fish = inputLines.ElementAt(0).Split(',').Select(n => int.Parse(n)).ToList();
        }
        public static void Part1()
        {
            Setup();
            for (var days = 1; days <= 80; days++)
                PassDay();
            Console.WriteLine($"Day 6 (1): fish count is {fish.Count}");
        }

        static void PassDay()
        {
            var newFish = 0;
            for (var i = 0; i < fish.Count; i++)
            {
                fish[i] = fish[i] - 1;
                if (fish[i] == -1)
                {
                    fish[i] = 6;
                    newFish++;
                }
            }
            fish = fish.Concat(Enumerable.Range(1, newFish).Select(i => 8)).ToList();
        }

        public static void Part2()
        {
            //Can't maintain a list of the actual fish, gets too big.
            // Easier: fishon0, fishon1, fishon2... fishon8
            Setup();
            fishDayIndex = new UInt64[9]; //0...8
            //New setup
            foreach (var f in fish)
            {
                fishDayIndex[f]++;
            }

            for (var days = 1; days <= 256; days++)
                PassDay2();
            ulong total = 0;
            foreach (ulong cur in fishDayIndex)
                total += cur;
            Console.WriteLine($"Day 6 (2): fish count is {total}"); // fishDayIndex.Sum(u => u) is problematic
        }

        static void PassDay2()
        {
            // Shift each value to the left to simulate each day passing - 50 fish on day 8 must be moved to day 7 etc
            var carryingTotal = fishDayIndex[8];
            fishDayIndex[8] = 0;
            for (var i = 8; i > 0; i--)
            {
                var newCarry = fishDayIndex[i - 1];
                fishDayIndex[i - 1] = carryingTotal;
                carryingTotal = newCarry;
            }
            // The 'zero fish' start again at 6, adding to whoever's there already, and have reproduced by 1 fish each which start at 8.
            fishDayIndex[6] += carryingTotal;
            fishDayIndex[8] = carryingTotal;
        }
    }
}
