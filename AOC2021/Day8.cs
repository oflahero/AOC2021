using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day8
    {
        static List<(string[], string[])> inputs;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(8);
            inputs = new List<(string[], string[])>();

            foreach (var l in inputLines)
            {
                var io = l.Split(" | ");
                inputs.Add((io[0].Split(" "), io[1].Split(" ")));
            }
        }

        public static void Part1()
        {
            Setup();

            var uniqueDigitLengths = new int[] { 2, 4, 3, 7 };
            var uniqueDigitCount = 0;
            foreach(var i in inputs)
            {
                uniqueDigitCount += i.Item2.Where(ov => uniqueDigitLengths.Contains(ov.Length)).Count();
            }
            Console.WriteLine($"Day 8(1): unique count is {uniqueDigitCount}");
        }

        public static void Part2()
        {
            Setup();
            var No1sCount = inputs.Where(i => !i.Item1.Any(n => n.Length == 2)).Count();
            var No7sCount = inputs.Where(i => !i.Item1.Any(n => n.Length == 3)).Count();
            var No4sCount = inputs.Where(i => !i.Item1.Any(n => n.Length == 4)).Count();
            Console.WriteLine($"No 1s: {No1sCount}, no 7s: {No7sCount}, no 4s: {No4sCount} - all inputs have at least one 1,4,7");
            // All inputs have at least 1 of a 1, 4 and 7. So can definitely deduce T, choice of TR and BR, and choice of TL and M.
            // 8 and 9: Deduce BL

            List<int> outputs = new List<int>();

            inputs.ForEach(input =>
            {
                var d1 = new DigitMappings();
                d1.Identify(input.Item1);
                if (d1.IsReady)
                {
                    var onums = input.Item2.Select(o => d1.TryGet(o)).ToList();
                    // Ugly list-of-single-digits-to-int conversion:
                    var sOutputNum = onums.Aggregate("", (a, b) => a + b);
                    var outputNum = int.Parse(sOutputNum);
                    outputs.Add(outputNum);
                }
                else
                    Console.WriteLine("Couldn't work out mappings");
            });

             Console.WriteLine($"Day 8)(2): sum is {outputs.Sum()}");
        }


        class DigitMappings
        {
            Dictionary<int, HashSet<char>> digitMappings;

            Dictionary<HashSet<char>, int> reverseMappings;

            public DigitMappings()
            {
                digitMappings = new Dictionary<int, HashSet<char>>();
                foreach (var n in Enumerable.Range(0, 10))
                    digitMappings.Add(n, null);
            }

            public void Identify(string[] inputs)
            {
                // We know we can always find 1,4,7 (and 8). So get those.
                var one = inputs.First(n => n.Length == 2);
                TryAdd(1, one);
                var seven = inputs.First(n => n.Length == 3);
                TryAdd(7, seven);
                var four = inputs.First(n => n.Length == 4);
                TryAdd(4, four);
                // Always there?
                var eight = inputs.FirstOrDefault(n => n.Length == 7);
                TryAdd(8, eight);

                // Get the fives (3,2,5)
                var fiveSegments = inputs.Where(n => n.Length == 5).Select(n5 => new HashSet<char>(n5)).ToList();
                // Contains 1 or 7? Must be 3. (2 and 5 don't).
                var three = fiveSegments.FirstOrDefault(f => f.IsProperSupersetOf(digitMappings[1]) || f.IsProperSupersetOf(digitMappings[7]));
                TryAdd(3, three);
                fiveSegments.RemoveAll(n => n.SetEquals(three));
                // 2 parts of 4, it's 2. 3 parts of 4, 5.
                var two = fiveSegments.FirstOrDefault(f => f.Intersect(digitMappings[4]).Count() == 2);
                var five = fiveSegments.FirstOrDefault(f => f.Intersect(digitMappings[4]).Count() == 3);
                TryAdd(2, two);
                TryAdd(5, five);

                // Get the sixes. (6,9,0)
                var sixSegments = inputs.Where(n => n.Length == 6).Select(n6 => new HashSet<char>(n6)).ToList();
                // Doesn't contains 1 or 7? Must be 6.
                var six = sixSegments.FirstOrDefault(f => !f.IsProperSupersetOf(digitMappings[7]));
                TryAdd(6, six);
                sixSegments.RemoveAll(n => n.SetEquals(six));
                // Contains 4, must be 9.
                var nine = sixSegments.FirstOrDefault(f => f.IsProperSupersetOf(digitMappings[4]));
                TryAdd(9, nine);
                sixSegments.RemoveAll(n => n.SetEquals(nine));
                if (sixSegments.Count == 1) // Must be zero
                    TryAdd(0, sixSegments.First());
            }

            bool TryAdd(int num, string combo)
            {
                return TryAdd(num, new HashSet<char>(combo));
            }

            bool TryAdd(int num, HashSet<char> combo)
            {
                if (!digitMappings.ContainsKey(num))
                    return false; // only allow 0..9
                if (combo == null) return false;
                if (digitMappings[num] != null)
                    return false;
                digitMappings[num] = combo;

                if (!digitMappings.Values.Any(v => v==null))
                    reverseMappings = digitMappings.ToDictionary(x => x.Value, x => x.Key, comparer: HashSet<char>.CreateSetComparer() ); // TIL!!!!
                return true;
            }

            public int? TryGet(string combo)
            {
                return TryGet(new HashSet<char>(combo));
            }

            public int? TryGet(HashSet<char> combo)
            {
                if (reverseMappings == null) return null;
                return reverseMappings[combo]; // NB ensure default comparer is SetEquals, not Equals or objectequals...!
            }

            public bool IsReady => reverseMappings != null;
        }
    }
}
