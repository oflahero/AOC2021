using System;
using System.Diagnostics;
using System.Linq;

namespace AOC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2021!");

            Day1.Part1();
            Day1.Part2();

            Day2.Part1();
            Day2.Part2();

            Day3.Part1();
            Day3.Part2();

            Day4.Part1();
            Day4.Part2();

            Day5.Part1();
            Day5.Part2();

            Day6.Part1();
            Day6.Part2();

            Day7.Part1();
            Stopwatch timer = new Stopwatch();
            /*timer.Start();
            Day7.Part2();
            timer.Stop();
            Console.WriteLine($"Day 7 part 2, initial version, took {timer.Elapsed}.");*/
            // Old part2 took 13sec!
            timer.Restart();
            Day7.Part2_Optimization();
            timer.Stop();
            Console.WriteLine($"Day 7 part 2, optimized version, took {timer.Elapsed} ({timer.ElapsedMilliseconds}ms).");

            Day8.Part1();
            Day8.Part2();

            Day9.Part1();
            Day9.Part2();

            Day10.Part1();
            Day10.Part2();

            Day11.Part1();
            Day11.Part2();

            Day12.Part1();
            Day12.Part2();

            Day13.Part1();
            Day13.Part2();
            Console.WriteLine("Exiting.");
        }



 
    }
}
