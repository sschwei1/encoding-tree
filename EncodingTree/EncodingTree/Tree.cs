using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EncodingTree;

public class Tree
{
    //----------------------------
    // Properties
    //----------------------------
    private TreeNode Node { get; set; }
    public Dictionary<TreeNode, string> Table { get; private set; }
    private string OriginalText { get; set; }

    //----------------------------
    // Constructor
    //----------------------------
    public Tree(string text, bool recursive = false)
    {
        OriginalText = text;
        var originalValues = CreateNodeList();

        if (recursive) CreateTreeRecursive(originalValues);
        else CreateTreeIterative(originalValues);

        if (this.Node == null)
        {
            this.Node = new TreeNode();
            if (originalValues.Count > 0)
            {
                this.Node.Children0 = originalValues[0];
                this.Node.Value = originalValues[0].Value;
            }
        }

        CreateTable();
    }

    //----------------------------
    // Private Methods
    //----------------------------

    /// <summary>
    /// Creates character occurence table<br />
    /// </summary>
    private List<TreeNode> CreateNodeList()
    {
        var originalValues = new List<TreeNode>();
        foreach (char c in OriginalText)
        {
            var keyValPair = originalValues.FirstOrDefault(e => e.Key == c);

            if (keyValPair != null) originalValues[originalValues.IndexOf(keyValPair)].Value++;
            else originalValues.Add(new TreeNode() {Key = c, Value = 1});
        }

        originalValues.Sort();

        return originalValues;
    }

    /// <summary>
    /// Creates Huffman Tree with iterative algorithm<br />
    ///     - data: list which contains character occurence<br />
    /// </summary>
    /// <param name="data"></param>
    private void CreateTreeIterative(List<TreeNode> data)
    {
        int cnt = data.Count();

        for (int i = 0; i < cnt - 1; i++)
        {
            int length = data.Count();
            var childNode = new TreeNode()
            {
                Children0 = data[length - 1],
                Children1 = data[length - 2],
                Value = data[length - 1].Value + data[length - 2].Value
            };

            data.RemoveAt(length - 1);
            data.RemoveAt(length - 2);

            int index = data.BinarySearch(childNode);
            if (index < 0) index = ~index;
            data.Insert(index, childNode);

            if (i == cnt - 2)
                this.Node = childNode;
        }
    }

    /// <summary>
    /// Creates Huffman Tree with recursive algorithm<br />
    ///     - data: list which contains character occurence<br />
    /// </summary>
    /// <param name="data"></param>
    [SuppressMessage("ReSharper", "TailRecursiveCall")]
    private void CreateTreeRecursive(List<TreeNode> data)
    {
        if (data.Count() < 2)
            return;

        int length = data.Count();
        data.Sort();
        var childNode = new TreeNode()
        {
            Children0 = data[length - 1],
            Children1 = data[length - 2],
            Value = data[length - 1].Value + data[length - 2].Value
        };

        data.RemoveAt(length - 1);
        data.RemoveAt(length - 2);

        int index = data.BinarySearch(childNode);
        if (index < 0) index = ~index;
        data.Insert(index, childNode);

        if (data.Count() == 1)
            this.Node = childNode;

        CreateTreeRecursive(data);
    }

    /// <summary>
    /// Creates encoding table<br />
    /// </summary>
    private void CreateTable()
    {
        Table = new Dictionary<TreeNode, string>();
        Node.GetTable(Table);
    }

    //----------------------------
    // Public Methods
    //----------------------------

    /// <summary>
    /// Prints tree structure in console<br />
    /// </summary>
    public void PrintTree()
    {
        Console.WriteLine("Tree:");
        Node.PrintTree(Node.GetDepth(), "");
        Console.Write("\n\n");
    }

    /// <summary>
    /// Prints character / Huffman Tree table<br />
    /// </summary>
    public void PrintTable()
    {
        Console.WriteLine("Conversion Table:");

        foreach (var pair in Table)
        {
            Console.Write($"{pair.Key.Key}:{pair.Key.Value}");
            Console.CursorLeft = 6;
            Console.WriteLine($"| {pair.Value}");
        }

        Console.Write("\n\n");
    }

    /// <summary>
    /// Returns encoded string<br />
    ///     - seperator: char to seperates each encoded char<br />
    /// </summary>
    /// <param name="seperator"></param>
    /// <returns></returns>
    public string GetEncodedText(char seperator = '\0')
    {
        string encodedString = "";

        for (int i = 0; i < OriginalText.Length; i++)
        {
            encodedString += Table.FirstOrDefault(e => e.Key.Key == OriginalText[i]).Value;
            if (i != OriginalText.Length - 1 && seperator != default(char)) encodedString += seperator;
        }

        return encodedString;
    }

    /// <summary>
    /// Writes compression rate to console<br />
    ///     - bits: bit usage of original encoding
    /// </summary>
    /// <param name="bits></param>
    /// <returns></returns>
    public void PrintCompressionRate(int bits)
    {
        var originalBits = bits * this.OriginalText.Length;
        var tableBits = 0;
        var newBits = GetEncodedText().Length;

        foreach (var pair in Table)
        {
            tableBits += bits + pair.Value.Length;
        }

        Console.WriteLine($"original: {originalBits}");
        Console.WriteLine($"new: {tableBits + newBits} (table:{tableBits} / data:{newBits})");
        Console.WriteLine(
            $"compression: {tableBits + newBits} / {originalBits} = {(tableBits + newBits) / (float) originalBits}");
        Console.WriteLine("\n\n");
    }
}