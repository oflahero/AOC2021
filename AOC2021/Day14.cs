using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day14
    {
        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(14);
            sInput = inputLines.First();
            insertionRules = new Dictionary<string, string>();
            foreach(var rule in inputLines.Skip(2))
            {
                var lr = rule.Split(" -> ");
                insertionRules[lr[0]] = lr[1];
            }
        }

        static string sInput;
        static Dictionary<string, string> insertionRules;

        public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + 1); // Not 'minIndex + searchstring.Length' - misses matches like 'BB' in 'BBB' - 2 matches
            }
        }

        public static void Part1()
        {
            Setup();
            var steps = 10;
            var sInputBuild = sInput; 
            var stepInsertions = new Dictionary<int, string>(); //<index, insertion>
            for (var i = 0; i < steps; i++)
            {
                stepInsertions.Clear();
                foreach(var r in insertionRules)
                {
                    var matches = sInputBuild.AllIndexesOf(r.Key);
                    //if (sInputBuild.IndexOf(r.Key) != -1)
                    foreach(var match in matches)
                      stepInsertions[match + 1] = r.Value;
                }
                // If we insert going backwards, we don't need to adjust the indices
                foreach (var insertPoint in stepInsertions.Keys.OrderByDescending(n=>n))
                {
                    sInputBuild = sInputBuild.Insert(insertPoint, stepInsertions[insertPoint]);
                }
            }
            var chars = sInputBuild.Select(c => c);
            var charGrps = chars.GroupBy(c => c).OrderByDescending(g => g.Count());
            int most = charGrps.First().Count(), least = charGrps.Last().Count();
            Console.WriteLine($"Day 14(1): Most is {charGrps.First().Key} ({most}), least is {charGrps.Last().Key} ({least}), difference is {most-least}.");
        }

        public static void Part2()
        {
            Setup();
            // Going to need to store the 'problem' as a dict of letterpair -> occurrences, e.g HH -> 4545, and single letter frequencies 
            // Applying a 'step occurrence' for each of the existing rules just ups a single letter frequency, removes one letterpair occurrence and adds two 
            // letterpair occurrences.

            var letterpairs = new Dictionary<string, long>();
            var singleletters = new Dictionary<string, long>(); // char more efficient but less readable code

            foreach (var c in sInput)
                singleletters.DictIncrement(c.ToString());

            for (var i = 0; i < sInput.Length - 1; i++)
                letterpairs.DictIncrement(sInput.Substring(i, 2));

            var steps = 40;
            var ruleHits = new Dictionary<string, long>();

            for (var i = 0; i < steps; i++)
            {
                ruleHits.Clear();    

                foreach (var r in letterpairs.Keys)
                {
                    if (insertionRules[r] != null) // Rule match on this pair. How many? As many insertions as current occurrences!
                    {
                        ruleHits.DictIncrement(r, letterpairs[r]);
                    }
                }

                // Apply all the rule hits
                foreach (var ruleHit in ruleHits)
                {
                    //ruleHit.Key // e.g. AB, ruleHit.Value- 39 - 39 occurrences
                    var insertedChar = insertionRules[ruleHit.Key];
                    // Num of inserted letter goes up by rule hit count
                    singleletters.DictIncrement(insertedChar, ruleHit.Value);
                    // Rule hit pair drops by the count as that pair is now split
                    letterpairs.DictIncrement(ruleHit.Key, -ruleHit.Value);
                    // Two new pairs formed by inserting the new char
                    var left = ruleHit.Key.Substring(0, 1) + insertionRules[ruleHit.Key];
                    var right = insertionRules[ruleHit.Key] + ruleHit.Key.Substring(1, 1);
                    letterpairs.DictIncrement(left, ruleHit.Value);
                    letterpairs.DictIncrement(right, ruleHit.Value);
                }

            }
            var most = singleletters.OrderByDescending(s => s.Value).First();
            var least = singleletters.OrderBy(s => s.Value).First();
            Console.WriteLine($"Day 14(2): Most is {most.Key} ({most.Value}), least is {least.Key} ({least.Value}), difference is {most.Value - least.Value}.");
        }

        public static void DictIncrement<T>(this Dictionary<T, long> d, T key, long num = 1)
        {
            if (d.Keys.Contains(key)) d[key] += num;
            else d[key] = num;
        }



    }
}
