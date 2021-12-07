using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2021
{
    static class Day1
    {
        public static void Part1()
        {
            var lines = Support.InputAsEnumerable(1);
            var nums = Support.AsInts(lines);
            var pairs = nums.Zip(nums.Skip(1), (previous, item) => (previous, item)).ToList();
            var increases = pairs.Sum(pair => pair.item > pair.previous ? 1 : 0);
            Console.WriteLine($"Day 1 increases: {increases}");
        }

        public static void Part2()
        {
            var lines = Support.InputAsEnumerable(1);
            var nums = Support.AsInts(lines);
            // Zipping again, but this time just to assemble the three-value-tuples, not for comparison:
            var pairs = nums.Zip(nums.Skip(1), (previous, item) => (previous, item)).ToList();
            var threesomes = pairs.Zip(nums.Skip(2), (pair, item) => (pair.previous, pair.item, item));
            var threesomeSums = threesomes.Select(ts => ts.Item1 + ts.Item2 + ts.Item3);

            // Now same comparison as part 1:
            var comparisonPairs = threesomeSums.Zip(threesomeSums.Skip(1), (previous, item) => (previous, item)).ToList();
            var increases = comparisonPairs.Sum(pair => pair.item > pair.previous ? 1 : 0);

            Console.WriteLine($"Day 1 (part 2) increases: {increases}");
        }
    }
}
