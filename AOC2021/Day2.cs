using System;
using System.Collections.Generic;
using System.Text;

namespace AOC2021
{
    class Day2
    {
        public static void Part1()
        {
            var lines = Support.InputAsEnumerable(2);
            int hpos = 0, depth = 0;
            foreach (var instruction in lines)
            {
                string instr = instruction.Split(' ')[0], val = instruction.Split(' ')[1];
                switch (instr)
                {
                    case "forward":
                        hpos += Int32.Parse(val);
                        break;
                    case "back":
                        hpos -= Int32.Parse(val);
                        break;
                    case "up":
                        depth -= Int32.Parse(val);
                        break;
                    case "down":
                        depth += Int32.Parse(val);
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine($"Day 2(1): Hpos is {hpos}, depth {depth}; product {hpos * depth}");
        }

        public static void Part2()
        {
            var lines = Support.InputAsEnumerable(2);
            int hpos = 0, depth = 0, aim = 0;
            foreach (var instruction in lines)
            {
                string instr = instruction.Split(' ')[0], val = instruction.Split(' ')[1];
                var nVal = Int32.Parse(val);
                switch (instr)
                {
                    case "forward":
                        hpos += nVal;
                        depth += aim * nVal;
                        break;
                    case "back":
                        hpos -= nVal;
                        break;
                    case "up":
                        aim -= nVal;
                        break;
                    case "down":
                        aim += nVal;
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine($"Day 2(2): Hpos is {hpos}, depth {depth}; product {hpos * depth}");
        }
    }
}
