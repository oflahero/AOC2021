using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day10
    {
        static List<string> inputs;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(10);
            inputs = inputLines.ToList();
        }
        public static void Part1()
        {
            Setup();
            Stack<char> stack = new Stack<char>();
            bool bIllegal = false;
            int corruptionTotal = 0;
            char popOpener;
            List<long> completionScores = new List<long>();

            foreach (var i in inputs)
            {
                bIllegal = false;
                stack.Clear();
                foreach (var c in i)
                {
                    popOpener = ' ';
                    switch (c)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            stack.Push(c);
                            break;
                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            if (stack.Count == 0)
                            {
                                bIllegal = true;
                            }
                            else
                            {
                                popOpener = stack.Pop();
                                if (c != GetComplementaryFor(popOpener))
                                    bIllegal = true;
                            }
                            break;
                        default:
                            // illegal char here too?
                            bIllegal = true;
                            break;
                    }
                    
                    if (bIllegal)
                    {
                        corruptionTotal += c == ')' ? 3 : c == ']' ? 57 : c == '}' ? 1197 : c == '>' ? 25137 : 0;
                        break;
                    }
                }
                if (stack.Count > 0 && !bIllegal)
                {
                    // Incomplete - work out the closing sequence gubbins - and stack contains openers, not closers
                    long score = 0;
                    while (stack.Count > 0)
                    {
                        score *= 5;
                        char c = stack.Pop();
                        score += (c == '(' ? 1 : c=='['?2:c=='{'?3:c=='<'?4:0);
                    }
                    completionScores.Add(score);
                }
            }
            Console.WriteLine($"Day 10(1): Corrupted total: {corruptionTotal}");
            Console.WriteLine($"Day 10(2): Middle incomplete score: {completionScores.OrderBy(n => n).ElementAt(completionScores.Count() / 2)}");
        }

        static char GetComplementaryFor(char c)
        {
            switch(c)
            {
                case '(': return ')';
                case '[': return ']';
                case '{': return '}';
                case '<': return '>';
                default: return 'x';
            }
        }

        public static void Part2()
        {
            //Setup();
            //Integrated in P1 thanx to stack
        }
    }
}
