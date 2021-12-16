using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{
    static class Day16
    {
        static List<string> AllBinaryInputNibbles;
        static string AllBinaryInput;

        public static void Setup()
        {
            var inputLines = Support.InputAsEnumerable(16);
            AllBinaryInputNibbles = inputLines.First().Select(c => 
                Convert.ToString(int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(4, '0'))
                .ToList();
            AllBinaryInput = string.Join(null, AllBinaryInputNibbles);
        }

        class BITSPacket
        {
            static int VersionHeaderLength = 3, PacketTypeIdLength = 3, LiteralNibbleLength = 5,
                SubPacketLengthBitLength = 15, SubPacketNumberBitLength = 11;
            public enum PacketTypeId { Sum=0, Product = 1, Minimum = 2, Maximum = 3, Literal = 4, GreaterThan = 5, LessThan=6, Equals=7}

            public short Version;
            public PacketTypeId PacketType;

            public long Literal; // if literal

            // if operator:
            public enum SubPacketSizeIndicatorType { Length, Number }
            public SubPacketSizeIndicatorType SubPacketSizeIndicator;
            public int SubPacketLengthOrNumber;
            public List<BITSPacket> SubPackets;

            static int ParseIndex;

            // Not threadsafe
            public static (BITSPacket packet, int inputCharsRead) ReadPacket(int indexFrom = -1)
            {
                var p = new BITSPacket();
                ParseIndex = indexFrom > -1 ? indexFrom : ParseIndex;
                var startIndex = ParseIndex;
                p.ReadVersionHeader();
                p.ReadTypeId();
                switch (p.PacketType)
                {
                    case PacketTypeId.Literal:
                        p.ReadLiteralValue();
                        break;
                    default:
                        var lenTypeIndic = p.ReadInput(1);
                        p.SubPacketSizeIndicator = lenTypeIndic=="0" ? SubPacketSizeIndicatorType.Length : 
                            SubPacketSizeIndicatorType.Number;
                        if (p.SubPacketSizeIndicator==SubPacketSizeIndicatorType.Number)
                        {
                            p.SubPacketLengthOrNumber = Convert.ToInt32(p.ReadInput(SubPacketNumberBitLength), 2);
                        }
                        else // SubPacketSizeIndicatorType.Length
                        {
                            p.SubPacketLengthOrNumber = Convert.ToInt32(p.ReadInput(SubPacketLengthBitLength), 2);
                        }
                        int lenOrNum = p.SubPacketLengthOrNumber;
                        while(lenOrNum > 0)
                        {
                            if (p.SubPackets==null) p.SubPackets=new List<BITSPacket>();
                            var subP = ReadPacket();
                            p.SubPackets.Add(subP.packet);
                            if (p.SubPacketSizeIndicator == SubPacketSizeIndicatorType.Length)
                            {
                                lenOrNum -= subP.inputCharsRead;
                            }
                            else // number of packets
                                lenOrNum--;
                        }

                        break;
                }
                return (p, ParseIndex - startIndex);
            }

            public long Evaluate()
            {
                switch (PacketType)
                {
                    case PacketTypeId.Literal:
                        return Literal;
                    case PacketTypeId.Sum:
                        if (SubPackets != null)
                            return SubPackets.Sum(p => p.Evaluate());
                        break;
                    case PacketTypeId.Product:
                        if (SubPackets != null)
                            return SubPackets.Select(p => p.Evaluate()).Aggregate((p,next) => p * next);
                        break;
                    case PacketTypeId.Minimum:
                        if (SubPackets != null)
                            return SubPackets.Min(p => p.Evaluate());
                        break;
                    case PacketTypeId.Maximum:
                        if (SubPackets != null)
                            return SubPackets.Max(p => p.Evaluate());
                        break;
                    case PacketTypeId.GreaterThan:
                    case PacketTypeId.LessThan:
                    case PacketTypeId.Equals:
                        if (SubPackets != null && SubPackets.Count == 2)
                        {
                            var l = SubPackets[0].Evaluate();
                            var r = SubPackets[1].Evaluate();
                            var ret = -1;
                            ret = PacketType == PacketTypeId.GreaterThan ?
                                (l > r ? 1 : 0) :
                                PacketType == PacketTypeId.LessThan ?
                                (l < r ? 1 : 0) :
                                PacketType == PacketTypeId.Equals ?
                                (l == r ? 1 : 0) : -1;
                            if (ret != -1) return ret;
                        }
                        break;
                }
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                switch (PacketType)
                {
                    case PacketTypeId.Literal:
                        return $" {Literal} ";
                    case PacketTypeId.Sum:
                        if (SubPackets != null)
                            return "( "+string.Join(" + ", SubPackets.Select(p => p.ToString()))+" )";
                        break;
                    case PacketTypeId.Product:
                        if (SubPackets != null)
                            return "( " + string.Join(" * ", SubPackets.Select(p => p.ToString())) + " )";
                        break;
                    case PacketTypeId.Minimum:
                        if (SubPackets != null)
                            return $"min( {string.Join(", ", SubPackets.Select(p => p.ToString()))} )";
                        break;
                    case PacketTypeId.Maximum:
                        if (SubPackets != null)
                            return $"max( {string.Join(", ", SubPackets.Select(p => p.ToString()))} )";
                        break;
                    case PacketTypeId.GreaterThan:
                    case PacketTypeId.LessThan:
                    case PacketTypeId.Equals:
                        if (SubPackets != null && SubPackets.Count == 2)
                        {
                            var l = SubPackets[0].ToString();
                            var r = SubPackets[1].ToString();
                            var ret = -1;
                            return PacketType == PacketTypeId.GreaterThan ?
                                $"({l} > {r})" :
                                PacketType == PacketTypeId.LessThan ?
                                $"({l} < {r})" :
                                PacketType == PacketTypeId.Equals ?
                                $"({l} == {r})" : "bollocks";
                        }
                        break;
                }
                throw new NotImplementedException();
            }

            string ReadInput(int len)
            {
                var s = AllBinaryInput.Substring(ParseIndex, len);
                ParseIndex += len;
                return s;
            }

            public string ReadPadding(int totalRead)
            {
                var paddingLen = totalRead % 4;
                var padding = ReadInput(paddingLen > 0 ? 4 - paddingLen : 0);

                return padding;
            }

            bool ReadVersionHeader()
            {
                var vh = ReadInput(3);
                Version = Convert.ToInt16(vh, 2);
                return true;
            }

            public int GetVersionNumberTotal()
            {
                int total = Version;
                if (SubPackets != null)
                {
                    total += SubPackets.Sum(p => p.GetVersionNumberTotal());
                }
                return total;
            }

            bool ReadTypeId()
            {
                var vh = ReadInput(3);
                PacketType = (PacketTypeId) Convert.ToInt16(vh, 2);
                return true;
            }

            bool ReadLiteralValue() // param for calculating padding
            {
                // parse set of 5bits, end case is 5bit starts with 0, remember 4bitpadding, 6 already read
                string chunk; bool isLastNibble;
                long literal = 0;
                do
                {
                    chunk = ReadInput(LiteralNibbleLength);
                    isLastNibble = chunk.StartsWith("0");
                    literal <<= 4;
                    literal += Convert.ToInt16(chunk.Substring(1), 2);
                }
                while (!isLastNibble);

                Literal = literal;
                return true;
            }

        }
        public static void Part1()
        {
            Setup();
            var p = BITSPacket.ReadPacket(0);
            var packet = p.packet;

            // Padding Not strictly necessary, but indication of a correct parsing.
            var padding = packet.ReadPadding(p.inputCharsRead);

            // All version numbers?
            var vnumtotal = packet.GetVersionNumberTotal();
            Console.WriteLine($"Day 16 (1): vnum total {vnumtotal}");


        }
        public static void Part2()
        {
            Setup();
            var p = BITSPacket.ReadPacket(0);
            var packet = p.packet;

            // Padding Not strictly necessary, but indication of a correct parsing.
            var padding = packet.ReadPadding(p.inputCharsRead);
            var eval = packet.Evaluate();
            Console.WriteLine(packet.ToString());
            Console.WriteLine($"Day 16 (2): evaluation {eval}");
        }
    }
}
