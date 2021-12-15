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

            Day14.Part1();
            Time("D14(2)", Day14.Part2);
            
            Day15.Part1();
            Time("D15(2)", Day15.Part2);

            Day16.Part1();
            Day16.Part2();

            Day17.Part1();
            Day17.Part2();

            Day18.Part1();
            Day18.Part2();

            Day19.Part1();
            Day19.Part2();

            Day20.Part1();
            Day20.Part2();

            Day21.Part1();
            Day21.Part2();

            Day22.Part1();
            Day22.Part2();

            Day23.Part1();
            Day23.Part2();

            Day24.Part1();
            Day24.Part2();

            Day25.Part1();
            Day25.Part2();
            Console.WriteLine("Exiting.");
        }

        static void Time(string desc, Action dayPart)
        {
            var sw = new Stopwatch();
            sw.Start();
            dayPart();
            sw.Stop();
            Console.WriteLine($"{desc}: {sw.ElapsedMilliseconds} ms. ({sw.Elapsed})");
        }

 
    }
}
