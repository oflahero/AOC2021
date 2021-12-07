using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2021
{
    class Day4
    {
        static IEnumerable<int> drawnNumbers;
        static List<IBingoCard> cards;

        static void Setup(bool isVersion2)
        {
            var lines = Support.InputAsEnumerable(4);
            drawnNumbers = lines.ElementAt(0).Split(',').Select(n => int.Parse(n));

            cards = new List<IBingoCard>();

            List<string> currentCard = new List<string>();
            foreach (var l in lines.Skip(1))
            {
                if (l.Trim() == string.Empty)
                {
                    if (currentCard.Count > 0)
                    {
                        if (!isVersion2)
                            cards.Add(new BingoCard(currentCard.ToArray()));
                        else
                            cards.Add(new BingoCard2(currentCard.ToArray()));
                        currentCard.Clear();
                    }
                }
                else
                    currentCard.Add(l);
            }
            if (currentCard.Count > 0)
            {
                if (!isVersion2)
                    cards.Add(new BingoCard(currentCard.ToArray()));
                else
                    cards.Add(new BingoCard2(currentCard.ToArray()));
                currentCard.Clear();
            }
        }

        public static void Part1()
        {
            // Should be using BingoCard2 cos of the 0 issue (can't -0) but it'll work to get the first
            Setup(isVersion2: false);

            foreach(var call in drawnNumbers)
            {
                foreach(var c in cards)
                {
                    if (c.Mark(call))
                    {
                        Console.WriteLine("Bingo! On "+call);
                        Console.WriteLine(c);
                        Console.WriteLine($"Unmarked sum: {c.SumUnmarked()}");
                        Console.WriteLine($"Product: {c.SumUnmarked() * call}");
                        break;
                    }
                }
                if (cards.Any(c => c.isBingo()))
                    break;
            }

            Console.WriteLine("OK");
        }

        public static void Part2()
        {
            Setup(isVersion2: true);
            Console.WriteLine("Doing all:");
            var lastToWin = 0;
            foreach (var call in drawnNumbers)
            {
                foreach (var c in cards)
                {
                    if (!c.isBingo() && c.Mark(call))
                    {
                        Console.WriteLine("Bingo! On " + call);
                        Console.WriteLine(c);
                        Console.WriteLine($"Unmarked sum: {c.SumUnmarked()}");
                        Console.WriteLine($"Product: {c.SumUnmarked() * call}");
                        lastToWin = c.SumUnmarked() * call;
                    }
                }
            }
            Console.WriteLine("Day 4 (2): last product is " + lastToWin);
            Console.WriteLine("OK");
        }
    }

    interface IBingoCard
    {
        bool isBingo();
        bool Mark(int called);
        int SumUnmarked();
        string ToString();
    }

    class BingoCard : IBingoCard
    {
        public BingoCard(string[] lines)
        {
            var converted = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n)).ToList()).ToList();
            nums = new int[converted.Count(), converted[0].Count()];
            for (var i = 0; i < nums.GetLength(0); i++)
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    nums[i, j] = converted[i][j];
                }
            // Num is marked by inverting it (minus).

            //marks = new bool[nums.Length, nums[0].Length];
        }

        int[,] nums;
        //bool[,] marks;

        public bool Mark(int called)
        {
            for (var i = 0; i < nums.GetLength(0); i++)
            {
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    if (nums[i, j] == called && nums[i, j] > 0)
                        nums[i, j] = -nums[i, j];
                }
            }
            return isBingo();
        }

        public bool isBingo()
        {
            return Enumerable.Range(0, nums.GetLength(0)).Any(r => Row(r).All(n => n < 0)) ||
                Enumerable.Range(0, nums.GetLength(1)).Any(c => Column(c).All(n => n < 0));
        }

        IEnumerable<int> Row(int rowIndex)
        {
            return Enumerable.Range(0, nums.GetLength(1)).Select(r => nums[rowIndex, r]);
        }

        IEnumerable<int> Column(int colIndex)
        {
            return Enumerable.Range(0, nums.GetLength(0)).Select(r => nums[r, colIndex]);
        }

        public int SumUnmarked()
        {
            int total = 0;
            for (var i = 0; i < nums.GetLength(0); i++)
            {
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    if (nums[i, j] > 0) total += nums[i, j];
                }
            }
            return total;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            for (var i = 0; i < nums.GetLength(0); i++)
            {
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    s.Append(nums[i, j]);
                    s.Append(" ");
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }
    }

    class BingoCard2 : IBingoCard
    {
        public BingoCard2(string[] lines)
        {
            var converted = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n)).ToList()).ToList();
            nums = new int[converted.Count(), converted[0].Count()];
            for (var i = 0; i < nums.GetLength(0); i++)
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    nums[i, j] = converted[i][j];
                }

            marks = new bool[nums.GetLength(0), nums.GetLength(1)];
        }

        int[,] nums;
        bool[,] marks;

        public bool Mark(int called)
        {
            for (var i = 0; i < nums.GetLength(0); i++)
            {
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    if (nums[i, j] == called && !marks[i, j])
                        marks[i, j] = true;
                }
            }
            return isBingo();
        }

        public bool isBingo()
        {
            return Enumerable.Range(0, marks.GetLength(0)).Any(r => Row(r).All(n => n)) ||
                Enumerable.Range(0, marks.GetLength(1)).Any(c => Column(c).All(n => n));
        }

        IEnumerable<bool> Row(int rowIndex)
        {
            return Enumerable.Range(0, marks.GetLength(1)).Select(r => marks[rowIndex, r]);
        }

        IEnumerable<bool> Column(int colIndex)
        {
            return Enumerable.Range(0, marks.GetLength(0)).Select(r => marks[r, colIndex]);
        }

        public int SumUnmarked()
        {
            int total = 0;
            for (var i = 0; i < marks.GetLength(0); i++)
            {
                for (var j = 0; j < marks.GetLength(1); j++)
                {
                    if (!marks[i, j]) total += nums[i, j];
                }
            }
            return total;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            for (var i = 0; i < nums.GetLength(0); i++)
            {
                for (var j = 0; j < nums.GetLength(1); j++)
                {
                    s.Append(nums[i, j]);
                    if (marks[i, j]) s.Append("!");
                    s.Append(" ");
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }
    }
}
