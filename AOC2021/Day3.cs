using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    class Day3
    {
        enum BitMajority { Majority1, Majority0, Equal }

        public static void Part1()
        {
            // All 12-bit numbers? Check;
            var lines = Support.InputAsEnumerable(3);
            var lineCount = lines.Count();
            var bitCount = lines.ElementAt(0).Length;
            var intLines = lines.Select(l => Convert.ToUInt32(l, 2)); // Base 2

            // Setup the bitmasks
            var masks = new int[bitCount];
            var currentMask = 1;
            for(var i=0; i<bitCount; i++)
            {
                masks[i] = currentMask;
                currentMask <<= 1;
            }

            var oneCounts = new int[bitCount]; // A total for each bit position
            foreach (var val in intLines)
            {
                for (var i=0; i<masks.Length; i++) // Note each bit position for each value
                {
                    oneCounts[i] += (val & masks[i]) > 0 ? 1 : 0;
                }
            }
            int gamma = 0, epsilon = 0;
            for(var i=oneCounts.Length-1; i>=0; i--)
            {
                gamma <<= 1; epsilon <<= 1;
                if (oneCounts[i] > lineCount / 2) // More than half;
                    gamma++;
                else
                    epsilon++;
            }

            Console.WriteLine($"Day 3(1): Gamma {gamma}, epsilon {epsilon}; product {gamma * epsilon}");
        }

        public static void Part2()
        {
            // All 12-bit numbers? Check;
            var lines = Support.InputAsEnumerable(3); //, partNum: 2);
            var lineCount = lines.Count();
            var bitCount = lines.ElementAt(0).Length;
            var intLines = lines.Select(l => Convert.ToUInt32(l, 2)).ToList(); // Base 2

            // Setup the bitmasks
            var masks = new int[bitCount];
            var currentMask = 1;
            for (var i = 0; i < bitCount; i++)
            {
                masks[i] = currentMask;
                currentMask <<= 1;
            }

            IEnumerable<uint> o2Nums = new List<uint>(intLines)
                ,co2Nums = new List<uint>(intLines); // Initially, all qualify. Use copies.
            for (var i = masks.Length - 1; i >= 0; i--) // left to right on the bit positions
            {
                // First, the o2:
                var oneCountAtI = o2Nums.Sum(l => (l & masks[i]) > 0 ? 1 : 0);
                BitMajority currentPositionBitMajority = oneCountAtI > Math.Ceiling((double)(o2Nums.Count() / 2.0)) ? BitMajority.Majority1 :
                    oneCountAtI < Math.Ceiling((double)(o2Nums.Count() / 2.0)) ? BitMajority.Majority0 : BitMajority.Equal;

                if (o2Nums.Count() > 1)
                {
                    switch (currentPositionBitMajority)
                    {
                        case BitMajority.Majority1:
                        case BitMajority.Equal:
                            o2Nums = o2Nums.Where(n => (n & masks[i]) > 0).ToList();
                            break;
                        case BitMajority.Majority0:
                            o2Nums = o2Nums.Where(n => (n & masks[i]) == 0).ToList();
                            break;
                        default:
                            break;
                    }
                }

                // Then co2:
                oneCountAtI = co2Nums.Sum(l => (l & masks[i]) > 0 ? 1 : 0);
                currentPositionBitMajority = oneCountAtI > Math.Ceiling((double)(co2Nums.Count() / 2.0)) ? BitMajority.Majority1 :
                    oneCountAtI < Math.Ceiling((double)(co2Nums.Count() / 2.0)) ? BitMajority.Majority0 : BitMajority.Equal;

                if (co2Nums.Count() > 1)
                {
                    switch (currentPositionBitMajority)
                    {
                        case BitMajority.Majority1:
                        case BitMajority.Equal:
                            co2Nums = co2Nums.Where(n => (n & masks[i]) == 0).ToList();
                            break;
                        case BitMajority.Majority0:
                            co2Nums = co2Nums.Where(n => (n & masks[i]) > 0).ToList();
                            break;
                        default:
                            break;
                    }
                }
            }

            var o2 = o2Nums.Single();
            var co2 = co2Nums.Single();

            Console.WriteLine($"Day 3(2): O2 {o2}, CO2 {co2}; product {o2 * co2}");
        }
    }
}
