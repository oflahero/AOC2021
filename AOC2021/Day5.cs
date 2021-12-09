using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace AOC2021
{
    static class Day5
    {
        static Dictionary<Point, int> pointOccupancy;
        static List<(Point, Point)> lines;

        static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(5);
            lines = new List<(Point, Point)>();
            foreach (var l in inputLines)
            {
                var p = l.Split(" -> ");
                var p1 = p[0].Split(',');
                var p2 = p[1].Split(',');
                lines.Add((new Point(int.Parse(p1[0]), int.Parse(p1[1])), new Point(int.Parse(p2[0]), int.Parse(p2[1]))));
            }

            pointOccupancy = new Dictionary<Point, int>();
        }

        public static void Part1()
        {
            Setup();

            var straightLines = lines.Where(l => l.Item1.X == l.Item2.X || l.Item1.Y == l.Item2.Y).ToList();
            foreach (var l in straightLines)
                pointOccupancy.Update(l);

            Console.WriteLine("Day 5 (1): Overlapping points: " + pointOccupancy.Where(p => p.Value > 1).Sum(p => 1));

            Console.WriteLine("OK");
        }

        // Can assume all lines are either straight or have slope 1
        static void Update(this Dictionary<Point, int> d, (Point, Point) line)
        {
            var xIncrement = line.Item1.X < line.Item2.X ? 1 : line.Item1.X > line.Item2.X ? -1 : 0;
            var yIncrement = line.Item1.Y < line.Item2.Y ? 1 : line.Item1.Y > line.Item2.Y ? -1 : 0;
            var currentPoint = line.Item1;
            d.Update(currentPoint);
            while (currentPoint != line.Item2)
            {
                currentPoint.X += xIncrement;
                currentPoint.Y += yIncrement;
                d.Update(currentPoint);
            }

        }

        public static void Update(this Dictionary<Point, int> d, Point p)
        {
            if (d.ContainsKey(p))
                d[p] = d[p] + 1;
            else
                d[p] = 1;
        }


        public static void Part2()
        {
            Setup();

            foreach (var l in lines)
                pointOccupancy.Update(l);

            Console.WriteLine("Day 5 (2): Overlapping points: " + pointOccupancy.Where(p => p.Value > 1).Sum(p => 1));

            Console.WriteLine("OK");
        }
    }
}
