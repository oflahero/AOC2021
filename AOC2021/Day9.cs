using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day9
    {
        class SeafloorPoint
        {
            public int Height;
            public SeafloorPoint LocalMinimum;
            public bool IsLocalMinimum => LocalMinimum != null && LocalMinimum == this;

            public SeafloorPoint(int height)
            {
                this.Height = height;
                this.LocalMinimum = null;
            }

            public override string ToString()
            {
                return Height.ToString();
            }
        }

        static SeafloorPoint[,] SeafloorMatrix;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(9).ToList();
            int y = inputLines.Count, x = inputLines[0].Length;
            SeafloorMatrix = new SeafloorPoint[x, y];
            for(var j=0; j<inputLines.Count; j++)
            {
                for (var i = 0; i < inputLines[j].Length; i++)
                    SeafloorMatrix[i, j] = new SeafloorPoint(int.Parse(inputLines[j].Substring(i,1)));
            }

            for (x = 0; x < SeafloorMatrix.GetLength(0); x += 1)
            {
                for (y = 0; y < SeafloorMatrix.GetLength(1); y += 1)
                {
                    GetLocalMinimumFor(x, y);
                    //Console.WriteLine($"{SeafloorMatrix[x, y]} -> {p}");
                }
            }
        }

        public static void Part1()
        {
            Setup();
            var localMinima = Enumerable.Cast<SeafloorPoint>(SeafloorMatrix).Where(p => p.IsLocalMinimum).ToList();
            var riskFactorSum = localMinima.Sum(m => m.Height + 1);

            Console.WriteLine($"Day 9(1): {riskFactorSum}");
        }

        static void PrintMatrix()
        {
            SeafloorPoint p;
            for (int x = 0; x < SeafloorMatrix.GetLength(0); x += 1)
            {
                for (int y = 0; y < SeafloorMatrix.GetLength(1); y += 1)
                {
                    p = SeafloorMatrix[x, y];
                    if (p.IsLocalMinimum) Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(p.Height);
                    Console.ResetColor();

                }
                Console.Write(Environment.NewLine);
            }
        }

        enum DirectionOfIteration { FromBelow, FromAbove, FromRight, FromLeft };
        static SeafloorPoint GetLocalMinimumFor(int x, int y, DirectionOfIteration? direction = null)
        {
            if (SeafloorMatrix[x, y].LocalMinimum != null)
                return SeafloorMatrix[x, y].LocalMinimum;
            //Up, down, left, right
            var lowestSoFar = SeafloorMatrix[x, y];
            if (y > 0 && direction != DirectionOfIteration.FromAbove && SeafloorMatrix[x,y-1].Height <= lowestSoFar.Height)
                lowestSoFar = GetLocalMinimumFor(x, y - 1, DirectionOfIteration.FromBelow);
            if (y < SeafloorMatrix.GetLength(1) - 1 && direction != DirectionOfIteration.FromBelow && SeafloorMatrix[x, y + 1].Height <= lowestSoFar.Height)        
                lowestSoFar = GetLocalMinimumFor(x, y + 1, DirectionOfIteration.FromAbove);
            if (x > 0 && direction != DirectionOfIteration.FromLeft && SeafloorMatrix[x-1, y].Height <= lowestSoFar.Height)
                lowestSoFar = GetLocalMinimumFor(x-1, y, DirectionOfIteration.FromRight);
            if (x < SeafloorMatrix.GetLength(0) - 1 && direction != DirectionOfIteration.FromRight && SeafloorMatrix[x+1, y].Height <= lowestSoFar.Height)
                lowestSoFar = GetLocalMinimumFor(x+1, y, DirectionOfIteration.FromLeft);

            SeafloorMatrix[x, y].LocalMinimum = lowestSoFar;
            return lowestSoFar;
        }

        public static void Part2()
        {
            Setup();

            var localMinima = Enumerable.Cast<SeafloorPoint>(SeafloorMatrix).Where(p => p.IsLocalMinimum).ToList();

            var grps = Enumerable.Cast<SeafloorPoint>(SeafloorMatrix).Where(p => p.Height != 9).GroupBy(p => p.LocalMinimum);
            // Get 3 largest basins
            var grps3 = grps.OrderByDescending(g => g.Count()).Take(3).Aggregate(1, (total, next) => total * next.Count());
             Console.WriteLine($"Day 9(2): {grps3}");
        }
    }
}
