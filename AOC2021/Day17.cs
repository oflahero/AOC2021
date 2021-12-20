using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2021
{
    static class Day17
    {
        static (int lower, int upper) XTargetDim, YTargetDim;
        static int XMinV, XMaxV, YMinV, YMaxV;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(17);
            var r = new Regex(@"target area: x=(\d+)\.\.(\d+), y=(-?\d+)\.\.(-?\d+)");
            var m = r.Match(inputLines.First());
            var grps = m.Groups.Values.Skip(1).ToList();
            XTargetDim = (Int32.Parse(grps[0].Value), Int32.Parse(grps[1].Value));
            YTargetDim = (Int32.Parse(grps[2].Value), Int32.Parse(grps[3].Value));


            // Starting from 0,0: largest possible 'trajectory' is (furthest_xdim, furthest_ydim)
            // min x is 1, max x is furthest_xdim
            // min y is 1 if furthest_ydim > 0, -1 if furthest_ydim < 0 

            // Min x velocity has to be enough to satisfy lower x bound as in 1+2+3+...n=lower x bound
            // (2*lowxbound)/n - 1. Solve for +ve quadratic root: (always +ve sqrt part here, so no complex checking)
            XMaxV = XTargetDim.upper;
            XMinV = (int)Math.Max(Math.Ceiling((-1 + Math.Sqrt(1 + 8 * XTargetDim.lower)) / 2),
                Math.Ceiling((-1 - Math.Sqrt(1 + 8 * XTargetDim.lower)) / 2));

            YMinV = YTargetDim.lower; // Can assume target is forward and down
            YMaxV = 1000; // Part 1 Special Brainfree
        }
        public static void Part1()
        {
            Setup();

            int currentHighest=0;
            foreach (var x in Enumerable.Range(XMinV, XMaxV - XMinV + 1))
                foreach (var y in Enumerable.Range(YMinV, YMaxV - YMinV + 1))
                    Boom(x, y, ref currentHighest);
            Console.WriteLine($"Day 17(1): highest is {currentHighest}");
        }

        static bool Boom(int initial_xv, int initial_yv, ref int currentHighest)
        {
            int x = 0, y = 0, xv=initial_xv, yv=initial_yv;
            var localHighest = y;
            bool bHasHit = isHit(x, y, xv);
            while (!bHasHit && !yTooFar(y, yv)) // TODO: remove xtoofar?? !xTooFar(x) && as can still hit after highest achieved
            {
                bHasHit = isHit(x, y, xv);
                x += xv; y += yv;
                yv--;
                if (xv > 0) xv--;

                localHighest = y > localHighest ? y : localHighest;
            }

            if (bHasHit && localHighest > currentHighest)
                currentHighest = localHighest;

            return bHasHit;
        }

        // Is a hit if in target area now, 
        // or is above and is dropping straight down and will hit based on progression Divide(saves useless steps - not implemented).
        static bool isHit(int x, int y, int xv = 1) => x >= XTargetDim.lower && x <= XTargetDim.upper &&
            ((y >= YTargetDim.lower && y <= YTargetDim.upper)); // || (xv == 0 && y > YTargetDim.upper && ));
      
        static bool xTooFar(int x) => x > XTargetDim.upper;

        static bool yTooFar(int y, int yv) =>  y < YTargetDim.lower && yv <= 0;

        public static void Part2()
        {
            Setup();

            var gooduns = new List<(int, int)>();
            int currentHighest = 0;
            foreach (var x in Enumerable.Range(XMinV, XMaxV - XMinV + 1))
                foreach (var y in Enumerable.Range(YMinV, YMaxV - YMinV - 1))
                    if (Boom(x, y, ref currentHighest))
                    { gooduns.Add((x, y)); /*Console.WriteLine((x,y));*/ }
            Console.WriteLine($"Day 17(2): num of candidates is {gooduns.Count}");
        }
    }
}
