using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day15
    {
        static short[,] Grid;
        static int xdim, ydim;
        static GridCostWithPath[,] GridCostWithPathCache;

        class GridCostWithPath
        {
            public int Cost;
            public List<(int, int)> Path;
        }

        class GridCostWithPathAndTotal : GridCostWithPath
        {
            public long TotalCost;
            public GridCostWithPathAndTotal Prev;
        }

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(15).ToList();
            Grid = new short[inputLines[0].Length, inputLines.Count];
            for (var y = 0; y < inputLines.Count; y++)
                for (var x = 0; x < inputLines[y].Length; x++)
                    Grid[x, y] = short.Parse(inputLines[y].Substring(x, 1));
            xdim = Grid.GetLength(0); ydim = Grid.GetLength(1);
        }
        enum DirectionOfIteration { NA, FromBelow, FromAbove, FromRight, FromLeft };
        public static void Part1()
        {
            Setup();
            var start = (0, 0);
            GridCostWithPathCache = new GridCostWithPath[xdim, ydim];
            GridCostWithPathCache[xdim - 1, ydim - 1] = new GridCostWithPath() { Cost = Grid[xdim - 1, ydim - 1], Path = new List<(int, int)> { (xdim - 1, ydim - 1) } }; 
            // last cheapest cost is known

            //var cheapest = GetLeastCostFromPosition1(start, DirectionOfIteration.NA);
            //var cheapest = GetLeastCostFromPosition2(start, new HashSet<(int, int)>());
            //Console.WriteLine($"Day 15(1): CHEAPEST: {cheapest.Cost - Grid[0,0]}");

            GetLeastCostFromPosition3();

            Console.WriteLine($"Day 15(1): CHEAPEST: {(GridCostWithPathCache[Grid.GetLength(0) - 1, Grid.GetLength(1) - 1] as GridCostWithPathAndTotal).TotalCost}");

        }

        // This works, but only for p1 & test cases. Doesn't go back/up.
        static GridCostWithPath GetLeastCostFromPosition1((int, int) point, DirectionOfIteration direction)
        {
            if (GridCostWithPathCache[point.Item1, point.Item2] != null) // Cache hit
            {
                return GridCostWithPathCache[point.Item1, point.Item2];
            }

            var currentCost = Grid[point.Item1, point.Item2];
            //var newlyTraversed = traversed.Append(point);

            /*if (point == (xdim - 1, ydim - 1))  // end!
            {
                Console.WriteLine($"Cost: {newlyTraversed.Sum(p => Grid[p.Item1, p.Item2])}");
                foreach (var p in newlyTraversed) Console.Write(p + " ");
                Console.Write(Environment.NewLine);
                return currentCost; // success!
            }*/
            // Right, left, down, up. X increasing to the right, y increasing downwards.
            // Try right + down as priorities.
            GridCostWithPath[] costs = new GridCostWithPath[4]; // Right, down, left, up

            (int, int) pointRight = (point.Item1 + 1, point.Item2), pointLeft = (point.Item1 - 1, point.Item2),
                pointBelow = (point.Item1, point.Item2 + 1), pointAbove = (point.Item1, point.Item2 - 1);

            GridCostWithPath cheapestCostFromHere = null, currentCheapest = null;
            if (point.Item1 < xdim - 1 && direction!=DirectionOfIteration.FromRight)
            {
               cheapestCostFromHere = GetLeastCostFromPosition1(pointRight, DirectionOfIteration.FromLeft);
            }
            if (point.Item2 < ydim - 1 && direction != DirectionOfIteration.FromBelow)
            {
                currentCheapest = GetLeastCostFromPosition1(pointBelow, DirectionOfIteration.FromAbove);
                if (cheapestCostFromHere==null || (currentCheapest != null && currentCheapest.Cost < cheapestCostFromHere.Cost))
                    cheapestCostFromHere = currentCheapest;
            }

            /*
if (point.Item1 > 0 && direction != DirectionOfIteration.FromLeft)
{
    currentCheapest = GetLeastCostFromPosition_Part1(pointLeft, DirectionOfIteration.FromRight);
    if (cheapestCostFromHere == null || (currentCheapest != null && currentCheapest.Cost < cheapestCostFromHere.Cost))
        cheapestCostFromHere = currentCheapest;
}



if (point.Item2 > 0 && direction != DirectionOfIteration.FromAbove)
{
    currentCheapest = GetLeastCostFromPosition_Part1(pointAbove, DirectionOfIteration.FromBelow);
    if (cheapestCostFromHere==null || (currentCheapest != null && currentCheapest.Cost < cheapestCostFromHere.Cost))
        cheapestCostFromHere = currentCheapest;
}*/

   /*         if (costs.All(c => c==null || c.Cost==0)) // need second comparison?
            {
                return null; // Blocked here, path complete/deadend but no good, get out
            }
            var relevantCosts = costs.Where(c => c!=null && c.Cost != 0);
            if (!relevantCosts.Any())
                return null;*/

            // add to cache
            //var cheapestCostFromHere = relevantCosts.OrderBy(c => c.Cost).First();
            if (cheapestCostFromHere == null) return null;

            var costHere = new GridCostWithPath() { Cost = currentCost + cheapestCostFromHere.Cost, Path = cheapestCostFromHere.Path.Append(point).ToList() };
            GridCostWithPathCache[point.Item1, point.Item2] = costHere;
            return costHere;
        }



        public static void Part2()
        {
            Setup();

            var start = (0, 0);
            var oldGrid = Grid;
            var oldXDim = xdim;
            var oldYDim = ydim;

            xdim *= 5;
            ydim *= 5;
            Grid = new short[xdim,ydim];

            for (short y=0; y<5; y++)
            {
                for (short x = 0; x < 5; x++)
                {
                    for (var yy=0; yy < oldYDim; yy++)
                        for (var xx=0; xx<oldXDim; xx++)
                        {
                            var newval = oldGrid[xx, yy] + x + y;
                            newval = newval > 9 ? newval - 9: newval;
                            Grid[xx + (oldXDim * x), yy + (oldYDim * y)] = (short)newval;
                        }
                }
            }

            //GridCostWithPathCache = new GridCostWithPath[xdim, ydim];
            //GridCostWithPathCache[xdim - 1, ydim - 1] = new GridCostWithPath() { Cost = Grid[xdim - 1, ydim - 1], Path = new List<(int, int)> { (xdim - 1, ydim - 1) } };
            // last cheapest cost is known

            //var cheapest = GetLeastCostFromPosition2(start, new HashSet<(int,int)>());
            GetLeastCostFromPosition3();

            Console.WriteLine($"Day 15(2): CHEAPEST: {(GridCostWithPathCache[Grid.GetLength(0)-1, Grid.GetLength(1)-1] as GridCostWithPathAndTotal).TotalCost}");
        }

        static GridCostWithPath GetLeastCostFromPosition2((int, int) point, HashSet<(int, int)> traversed)
        {
            int x = point.Item1, y = point.Item2;
            if (GridCostWithPathCache[point.Item1, point.Item2] != null) // Cache hit
            {
                return GridCostWithPathCache[point.Item1, point.Item2];
            }

            var currentCost = Grid[point.Item1, point.Item2];
            var newlyTraversed = new HashSet<(int, int)>(traversed);
            newlyTraversed.Add(point);

            /*if (point == (xdim - 1, ydim - 1))  // end!
            {
                Console.WriteLine($"Cost: {newlyTraversed.Sum(p => Grid[p.Item1, p.Item2])}");
                foreach (var p in newlyTraversed) Console.Write(p + " ");
                Console.Write(Environment.NewLine);
                return currentCost; // success!
            }*/
            // Right, left, down, up. X increasing to the right, y increasing downwards.
            // Try right + down as priorities.
            GridCostWithPath[] costs = new GridCostWithPath[4]; // Right, down, left, up

            (int, int) pointRight = (point.Item1 + 1, point.Item2), pointLeft = (point.Item1 - 1, point.Item2),
                pointBelow = (point.Item1, point.Item2 + 1), pointAbove = (point.Item1, point.Item2 - 1);

            if (point.Item1 < xdim - 1 && !traversed.Contains(pointRight))
            {
                costs[0] = GetLeastCostFromPosition2(pointRight, newlyTraversed);
            }
            if (point.Item2 < ydim - 1 && !traversed.Contains(pointBelow))
            {
                costs[1] = GetLeastCostFromPosition2(pointBelow, newlyTraversed);
            }       
            if (point.Item1 > 0 && !traversed.Contains(pointLeft))
            {
                costs[2] = GetLeastCostFromPosition2(pointLeft, newlyTraversed);
            }
            if (point.Item2 > 0 && !traversed.Contains(pointAbove))
            {
                costs[3] = GetLeastCostFromPosition2(pointAbove, newlyTraversed);
            }

            var relevantCosts = costs.Where(c => c!=null && c.Cost != 0);
            if (!relevantCosts.Any())
                return null;// Blocked here, path complete/deadend but no good, get out

            // add to cache
            var cheapestCostFromHere = relevantCosts.OrderBy(c => c.Cost).First();

            var costHere = new GridCostWithPath() { Cost = currentCost + cheapestCostFromHere.Cost, Path = cheapestCostFromHere.Path.Append(point).ToList() };
            GridCostWithPathCache[point.Item1, point.Item2] = costHere;
            return costHere;
        }

        static void GetLeastCostFromPosition3()
        {
            // Set up cache initially
            GridCostWithPathCache = new GridCostWithPath[xdim, ydim];
            //GridCostWithPathCache[xdim - 1, ydim - 1] = new GridCostWithPathAndTotal() { Cost = Grid[xdim - 1, ydim - 1], Path = new List<(int, int)> { (xdim - 1, ydim - 1) } };
            //GridCostWithPathCache[xdim - 1, ydim - 1] = new GridCostWithPathAndTotal() { Cost = Grid[xdim - 1, ydim - 1] };
            int x, y;

            for (x=0; x<Grid.GetLength(0); x++)
                for(y=0; y<Grid.GetLength(1); y++)
                {
                    //GridCostWithPathCache[x, y] = new GridCostWithPathAndTotal() { Cost = Grid[x,y], Path = new List<(int, int)>(), TotalCost=long.MaxValue};
                    GridCostWithPathCache[x, y] = new GridCostWithPathAndTotal() { Cost = Grid[x, y], TotalCost = long.MaxValue };
                }

            Queue<(int x, int y)> evaluationQ = new Queue<(int x, int y)>();

            (GridCostWithPathCache[0, 0] as GridCostWithPathAndTotal).TotalCost = 0;
            evaluationQ.Enqueue((0, 0));
            GridCostWithPathAndTotal tile;
            do
            {
                var hereCoords = evaluationQ.Dequeue();
                x = hereCoords.x;y = hereCoords.y;
                var here = GridCostWithPathCache[x, y] as GridCostWithPathAndTotal;

                // Eval all four available directions...
                if (hereCoords.x < GridCostWithPathCache.GetLength(0)-1) // try right
                {
                    tile = GridCostWithPathCache[x + 1, y] as GridCostWithPathAndTotal;
                    if (here.TotalCost + tile.Cost < tile.TotalCost)
                    {
                        tile.TotalCost = here.TotalCost + tile.Cost;
                        //tile.Path = here.Path.Append(hereCoords).ToList();
                        tile.Prev = here;
                        evaluationQ.Enqueue((x + 1, y));
                    }
                }
                if (hereCoords.y < GridCostWithPathCache.GetLength(1) - 1) // try down
                {
                    tile = GridCostWithPathCache[x, y+1] as GridCostWithPathAndTotal;
                    if (here.TotalCost + tile.Cost < tile.TotalCost)
                    {
                        tile.TotalCost = here.TotalCost + tile.Cost;
                        //tile.Path = here.Path.Append(hereCoords).ToList();
                        tile.Prev = here;
                        evaluationQ.Enqueue((x, y+1));
                    }
                }
                if (hereCoords.x > 0) // try left
                {
                    tile = GridCostWithPathCache[x - 1, y] as GridCostWithPathAndTotal;
                    if (here.TotalCost + tile.Cost < tile.TotalCost)
                    {
                        tile.TotalCost = here.TotalCost + tile.Cost;
                        //tile.Path = here.Path.Append(hereCoords).ToList();
                        tile.Prev = here;
                        evaluationQ.Enqueue((x - 1, y));
                    }
                }
                if (hereCoords.y > 0) // try up
                {
                    tile = GridCostWithPathCache[x, y - 1] as GridCostWithPathAndTotal;
                    if (here.TotalCost + tile.Cost < tile.TotalCost)
                    {
                        tile.TotalCost = here.TotalCost + tile.Cost;
                        //tile.Path = here.Path.Append(hereCoords).ToList();
                        tile.Prev = here;
                        evaluationQ.Enqueue((x, y - 1));
                    }
                }
            } while (evaluationQ.Any());
        }
    }
}
