using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day12
    {
        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(12);
            List<CaveNode> allCaves = new List<CaveNode>();
            foreach (var c in inputLines)
            {
                var twoEnds = c.Split('-');
                var left = allCaves.FirstOrDefault(c => c.Name == twoEnds[0]);
                if (left == null)
                {
                    left = new CaveNode(twoEnds[0]);
                    allCaves.Add(left);
                }
                var right = allCaves.FirstOrDefault(c => c.Name == twoEnds[1]);
                if (right == null)
                {
                    right = new CaveNode(twoEnds[1]);
                    allCaves.Add(right);
                }
                left.TunnelLinks.Add(right);
                right.TunnelLinks.Add(left);
            }
            // Might be nicer to use Dictionary<CaveNode, List<CaveNode>> instead?
        }

        public static CaveNode Start, End;

        public static void Part1()
        {
            Setup();
            var SuccessfulTraversals = 0;
            Start.Traverse(ref SuccessfulTraversals, new List<CaveNode>(), oneDoubleVisitAllowed:false);
            Console.WriteLine($"Day 12(1): {SuccessfulTraversals} valid paths.");
        }

        public static void Part2()
        {
            Setup();
            var SuccessfulTraversals = 0;
            Start.Traverse(ref SuccessfulTraversals, new List<CaveNode>(), oneDoubleVisitAllowed: true);
            Console.WriteLine($"Day 12(2): {SuccessfulTraversals} valid paths.");
        }
    }

    class CaveNode
    {
        public string Name { get; set; }
        public List<CaveNode> TunnelLinks { get; set; } // Unordered
        public bool IsSingleVisitCave;

        public CaveNode(string name)
        {
            this.Name = name;
            if (Day12.Start == null && name == "start") Day12.Start = this;
            if (Day12.End == null && name == "end") Day12.End = this;
            TunnelLinks = new List<CaveNode>();
            IsSingleVisitCave = Name != Name.ToUpper();
        }

        public bool IsStart => this.Name=="start";
        public bool IsEnd => this.Name == "end";

        public void Traverse(ref int successfulTraversals, IEnumerable<CaveNode> path, 
            bool hasHitDoubleVisitAlready = false,
            bool oneDoubleVisitAllowed = false)
        {
            if (IsEnd)
            {
                successfulTraversals++;
                return;
            }
            else if (IsSingleVisitCave) // already visited this cave? Once/twice?
            {
                if (path.Contains(this))
                {
                    if (!oneDoubleVisitAllowed || hasHitDoubleVisitAlready || IsStart)
                        return;
                    if (!hasHitDoubleVisitAlready)
                        hasHitDoubleVisitAlready = true;
                }
            }

            path = path.Append(this);
            foreach (var link in TunnelLinks)
            {
                link.Traverse(ref successfulTraversals, path, hasHitDoubleVisitAlready, oneDoubleVisitAllowed);
            }
        }
    }
}
