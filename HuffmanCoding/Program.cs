using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HuffmanCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayHeading("Original Text");
            var fileText = File.ReadAllText("Target.txt");
            Console.WriteLine(fileText);

            DisplayHeading("Letters by Frequency");
            var lettersByFrequency = OrderLettersByFrequencyUsingLinq(fileText);
            foreach (var item in lettersByFrequency)
                Console.WriteLine(item);

            DisplayHeading("Huffman Codes");
            var tree = BuildTree(lettersByFrequency);
            var lookup = new Dictionary<char, string>();
            DisplayTree(tree);
            BuildLookupTable(lookup, tree);

            DisplayHeading("Encoded Text");
            var encodedText = string.Join("", fileText.Select(c => lookup[c]));
            Console.WriteLine(encodedText);

            DisplayHeading("Decoded Text");
            var decodedText = DecodeText(tree, encodedText);
            Console.WriteLine(decodedText);


            //while (encodedText.Length >= 64)
            //{
            //    var n = Convert.ToUInt64(encodedText.Substring(0, 64), 2);
            //    Console.Write(n);
            //    encodedText = encodedText.Substring(64);
            //}
            //var nLeft = Convert.ToUInt64(encodedText, 2);
            //Console.Write(nLeft);

            // 
            Console.ReadKey(true);
        }

        private static string DecodeText(CombinedNode tree, string encodedText)
        {
            var result = "";
            var offset = 1;
            INode currentNode = tree;
            while (offset <= encodedText.Length)
            {
                if (currentNode is LeafNode leafNode)
                {
                    result += leafNode.Key;
                    currentNode = tree;
                }
                else if (currentNode is CombinedNode combinedNode)
                {
                    if (encodedText[offset] == '0')
                        currentNode = combinedNode.LHS;
                    else
                        currentNode = combinedNode.RHS;
                }
                offset++;
            }
            return result;
        }

        private static void DisplayHeading(string caption)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine(caption);
            Console.ResetColor();
        }

        private static void BuildLookupTable(Dictionary<char, string> lookup, INode node, string parentValue = "1")
        {
            if (node is LeafNode leafNode)
                lookup[leafNode.Key] = parentValue;

            if (node is CombinedNode combinedNode)
            {
                BuildLookupTable(lookup, combinedNode.LHS, parentValue + "0");
                BuildLookupTable(lookup, combinedNode.RHS, parentValue + "1");
            }
        }

        private static void DisplayTree(INode node, string parentValue = "1")
        {
            if (node is LeafNode leafNode)
                Console.WriteLine($"{leafNode.Key} is {parentValue}");

            if (node is CombinedNode combinedNode)
            {
                DisplayTree(combinedNode.LHS, parentValue + "0");
                DisplayTree(combinedNode.RHS, parentValue + "1");
            }
        }

        private static CombinedNode BuildTree(IEnumerable<LeafNode> lettersByFrequency)
        {
            var leafQueue = new Queue<LeafNode>();
            var combinedQueue = new Queue<CombinedNode>();

            foreach (var item in lettersByFrequency)
                leafQueue.Enqueue(item);

            while (leafQueue.Count() + combinedQueue.Count() > 1)
            {
                var lhs = GetLowestItem(leafQueue, combinedQueue);
                var rhs = GetLowestItem(leafQueue, combinedQueue);
                var combinedNode = new CombinedNode(lhs, rhs);
                combinedQueue.Enqueue(combinedNode);
            }
            return combinedQueue.Dequeue();
        }

        private static INode GetLowestItem(Queue<LeafNode> leafQueue, Queue<CombinedNode> combinedQueue)
        {
            if (leafQueue.Count() == 0)
                return combinedQueue.Dequeue();
            else if (combinedQueue.Count() == 0)
                return leafQueue.Dequeue();
            else if (leafQueue.Peek().Weight < combinedQueue.Peek().Weight)
                return leafQueue.Dequeue();
            else
                return combinedQueue.Dequeue();
        }

        static IEnumerable<LeafNode> OrderLettersByFrequencyUsingLinq(IEnumerable<char> fileText)
        {
            return fileText.GroupBy(c => c)
                           .Select(grp => new LeafNode(grp.Key, grp.Count()))
                           .OrderBy(grp => grp.Weight);
        }
    }
}
