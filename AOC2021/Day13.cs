using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace AOC2021
{
    static class Day13
    {
        enum axis { X, Y }
        static HashSet<Point> lps;
        static List<(axis, int)> folds;
        static int XDimension, YDimension;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(13, isTest: false);
            lps = new HashSet<Point>(); folds = new List<(axis, int)>();
            foreach(var p in inputLines)
            {
                if (p.Length == 0) break; // points done
                var ps = p.Split(',');
                lps.Add(new Point(int.Parse(ps[0]), int.Parse(ps[1])));
            }
            XDimension = lps.Max(p => p.X) + 1;
            YDimension = lps.Max(p => p.Y) + 1;
            foreach(var p in inputLines)
            {
                if (p.StartsWith("fold along "))
                {
                    var f = p.Substring("fold along ".Length);
                    var fs = f.Split('=');
                    folds.Add((fs[0] == "x" ? axis.X : axis.Y, int.Parse(fs[1])));
                }
            }

            // Better data structure than list of points for merging?
        }
        public static void Part1()
        {
            Setup();
            var fold = folds.First();
            DoFold(fold);
 
            // Don't care about negative coords as only doing one fold!
            // Will any folds go into negatives? Easy enough to do a coord shift.
            Console.WriteLine($"Day 1: dot count is {lps.Count}");
        }

        static void DoFold((axis,int) fold)
        {
            var foldAxis = fold.Item1;

            // All the columns/rows to the right of/beneath the fold must be merged with corresponding mirror.
            // Start either side of fold
            var foldFlipPointSet = new HashSet<Point>();
            var foldLine = fold.Item2;
            var lineToFlipCountIndex = foldAxis == axis.X ? XDimension - foldLine : YDimension - foldLine;

            for (var shift = 1; shift < lineToFlipCountIndex; shift++)
            {
                // Row/col points to flip
                var pts = lps.Where(pt => foldAxis == axis.X ? pt.X == foldLine + shift : pt.Y == foldLine + shift);
                foreach (var pt in pts)
                    foldFlipPointSet.Add(foldAxis == axis.X ? new Point(foldLine - shift, pt.Y) : new Point(pt.X, foldLine - shift));
            }
            // Exclude columns/rows to the right of/below the fold, and union the flipped point set.
            lps.RemoveWhere(p => foldAxis == axis.X ? p.X >= foldLine : p.Y >= foldLine);
            lps.UnionWith(foldFlipPointSet);

            // Makes no difference really as we're always folding 'smaller' (i.e. to the left, and back up) but this is more
            // accurate.
            XDimension = lps.Max(p => p.X) + 1;
            YDimension = lps.Max(p => p.Y) + 1;
        }

        public static void Part2()
        {
            Setup();
            foreach (var fold in folds)
            {
                DoFold(fold);
                if (lps.Any(pt => pt.X < 0 || pt.Y < 0))
                    Console.WriteLine("Neg detected - have to do a coordinate shift to origin!");
            }
                
            Console.WriteLine("Dammit, print codes coherently!");
            // Ug-ly
            for (var y = 0; y <= lps.Max(p => p.Y); y++)
        
            {
                for (var x = 0; x <= lps.Max(p => p.X); x++)
                {
                    var pt = new Point(x, y);
                    if (lps.Contains(pt)) Console.Write("#"); else Console.Write(".");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
