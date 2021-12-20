using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day18
    {
        class Item
        {
            public Item(Item parent, int literal = 0)
            {
                Parent = parent;
                Literal = literal;
            }
            public bool IsLiteralItem => Left == null && Right == null;
            public bool IsLiteralPair => !IsLiteralItem && Left.IsLiteralItem && Right.IsLiteralItem;
            
            public int Literal;
            public Item Left;
            public Item Right;
            public Item Parent;

            enum LR { Left, Right }
            enum UD { Up, Down }

            public int GetMagnitude()
            {
                if (IsLiteralItem) return Literal;

                return 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
            }

            public void Explode()
            {
                if (!IsLiteralPair)
                    return;
                var currentParent = this.Parent;
                var current = this;
                while (currentParent != null && currentParent.Left == current)
                {
                    current = currentParent;
                    currentParent = currentParent.Parent;
                }

                if (currentParent != null) currentParent.Left.TraverseExplodeAdd(Left.Literal, LR.Right);

                currentParent = this.Parent;
                current = this;
                while (currentParent != null && currentParent.Right == current)
                {
                    current = currentParent;
                    currentParent = currentParent.Parent;
                }
                if (currentParent != null) currentParent.Right.TraverseExplodeAdd(Right.Literal, LR.Left);

                Left = Right = null;
                Literal = 0;
            }

            void TraverseExplodeAdd(int val, LR aimingTowards)
            {
                if (IsLiteralItem)
                {
                    Literal += val;
                    return;
                }
                var childTarget = aimingTowards == LR.Right ? Right : Left;
                childTarget.TraverseExplodeAdd(val, aimingTowards);
            }

            public void Split()
            {
                if (!IsLiteralItem || Literal < 10)
                    return;
                int leftnum = Literal / 2;
                int rightnum = Literal - leftnum;
                Literal = 0;
                Left = new Item(this, leftnum);
                Left.Parent = this; // not strictly necessary but nice
                Right = new Item(this, rightnum);
                Right.Parent = this; // not strictly necessary but nice
            }

            public Item Clone()
            {
                if (!IsLiteralItem)
                {
                    if (Parent==null)
                        return Item.AddUnreduced(Left.Clone(), Right.Clone());
                    var left = Left.Clone();
                    var right = Right.Clone();
                    var c = new Item(null);
                    c.Left = left;
                    c.Right = right;
                    left.Parent = c;
                    right.Parent = c;
                    return c;
                }

                return new Item(null, Literal);
            }

            public static Item AddUnreduced(Item left, Item right)
            {
                Item i = new Item(null);
                left.Parent = i;
                right.Parent = i;
                i.Left = left;
                i.Right = right;
                return i;
            }

            public override string ToString()
            {
                if (IsLiteralItem) return Literal.ToString();
                return $"[{Left.ToString()},{Right.ToString()}]";
            }
            //public int Depth; // derivable instead?
        }

        static List<Item> Nums;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(18);
            Nums = new List<Item>();
            foreach(var line in inputLines)
            {
                Item top = new Item(null);
                if (line.Length > 0)
                    ParseNum(line, 0, top);
                //Console.WriteLine(top.ToString());
                Nums.Add(top);
            }
        }

        static int ParseNum(string input, int index, Item target)
        {
            if (index < 0 || index >= input.Length)
                return -1; ;
            var c = input[index];
            switch(c)
            {
                case '[':
                    target.Left = new Item(target);
                    index = ParseNum(input, index + 1, target.Left);
                    if (input[index] != ',') return -1;
                    target.Right = new Item(target);
                    index = ParseNum(input, index + 1, target.Right);
                    if (input[index] != ']') return -1;
                    index++;
                    break;
                default: // must be 1-digit num
                    target.Literal = Int32.Parse(input.Substring(index,1));
                    index++;
                    break;
            }
            return index;
        }

        enum ReduceType { Explosion, Split }
        static bool Reduce(Item i, ReduceType reduceType, int depth=0)
        {
            if (reduceType==ReduceType.Explosion && i.IsLiteralPair && depth >= 4)
            {
                i.Explode();
                return true;
            }
            else if (reduceType == ReduceType.Split && i.Literal >= 10)
            {
                i.Split();
                return true;
            }
            
            if (!i.IsLiteralItem)
            {
                // Won't evaluate right if anything hit in left
                if (Reduce(i.Left, reduceType, depth + 1) || Reduce(i.Right, reduceType, depth + 1))
                    return true;
            }

            return false;
        }

        static void TryReduce(Item item)
        {
            bool bAnyChanges = false;
            do
            {
                bAnyChanges = false;
                bAnyChanges = Reduce(item, ReduceType.Explosion);
                if (!bAnyChanges)
                    bAnyChanges = Reduce(item, ReduceType.Split);
            } while (bAnyChanges);            
        }

        public static void Part1()
        {
            Setup();
            var currentSum = Nums.First();
            TryReduce(currentSum);
            foreach (var n in Nums.Skip(1))
            {
                Console.WriteLine("Before :" + n);
                TryReduce(n);
                Console.WriteLine("After: " + n);
                var newSum = Item.AddUnreduced(currentSum, n);
                TryReduce(newSum);
                currentSum = newSum;
            }
            Console.WriteLine("Day 18: "+currentSum);

            // Magnitude
            Console.WriteLine("Magn: " + currentSum.GetMagnitude());
        }

        public static void Part2()
        {
            Setup();
            foreach (var n in Nums) TryReduce(n);

            Item largestX=null, largestY=null;
            int largestMagnitude = 0;
            // All combos
            for(var i=0; i<Nums.Count; i++)
            {
                for (var j=i+1; j<Nums.Count; j++)
                {
                    var x = Nums[i].Clone();
                    var y = Nums[j].Clone();
                    var sum = Item.AddUnreduced(x, y);
                    TryReduce(sum);
                    if (sum.GetMagnitude() > largestMagnitude)
                    {
                        largestMagnitude = sum.GetMagnitude();
                        largestX = x;
                        largestY = y;
                    }

                    x = Nums[j].Clone();
                    y = Nums[i].Clone();
                    sum = Item.AddUnreduced(x, y);
                    TryReduce(sum);
                    if (sum.GetMagnitude() > largestMagnitude)
                    {
                        largestMagnitude = sum.GetMagnitude();
                        largestX = x;
                        largestY = y;
                    }
                }
            }

            Console.WriteLine("Day 2: ");
            Console.WriteLine(largestX);
            Console.WriteLine(largestY);
            Console.WriteLine(largestMagnitude);
        }
    }
}
