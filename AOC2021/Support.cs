using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AOC2021
{
    class Support
    {
        public static IEnumerable<string> InputAsEnumerable(int dayNum, int? partNum = null)
        {
            return File.ReadAllLines($"./input/day{dayNum}{(partNum != null ? "_"+partNum : "")}.txt");
        }

        public static IEnumerable<int> AsInts(IEnumerable<string> lines)
        {
            return lines.Select(l => Int32.Parse(l));
        }

        public static IEnumerable<double> AsDoubles(IEnumerable<string> lines)
        {
            return lines.Select(l => Double.Parse(l));
        }
    }
}
