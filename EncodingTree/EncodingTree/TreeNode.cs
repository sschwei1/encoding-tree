namespace EncodingTree;

public class TreeNode : IComparable
{
    //----------------------------
    // Properties
    //----------------------------
    public char Key { get; set; }
    public int Value { get; set; }
    public TreeNode Children0 { get; set; }
    public TreeNode Children1 { get; set; }

    //----------------------------
    // Private Methods
    //----------------------------

    /// <summary>
    /// Prints Node point to end, so all values are listed underneath each other<br />
    ///     - totalDepth: depth difference, print to pretty frontend<br />
    /// </summary>
    /// <param name="totalDepth"></param>
    private static void PrintDepth(int totalDepth)
    {
        Console.Write($"X─");
        for (int i = 0; i < totalDepth; i++)
        {
            Console.Write("──");
        }

        Console.Write("»");
    }

    /// <summary>
    /// Prints out nested paths for tree<br />
    ///     - path: path to current node<br />
    /// </summary>
    /// <param name="path"></param>
    private static void PrintTreePaths(string path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            if (i == path.Length - 1) Console.Write(path[^1] == '0' ? "├─" : "└─");
            else Console.Write(path[i] == '0' ? "│ " : "  ");
        }
    }

    /// <summary>
    /// Method called in case a sub node is null<br />
    ///     - depth: indicates how deeply nested the current node is<br />
    ///     - path: path to current node<br />
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="path"></param>
    private static void PrintNullPath(int depth, string path)
    {
        PrintTreePaths(path);
        PrintDepth(depth - path.Length);

        ConsoleHandler.SetConsoleColor(ConsoleColor.Gray, ConsoleColor.White);
        Console.WriteLine("NULL");
        ConsoleHandler.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
    }


    //----------------------------
    // Public Methods
    //----------------------------

    /// <summary>
    /// Recursive method to print tree<br />
    ///     - depth: indicates how deeply nested the current node is<br />
    ///     - path: path to current node<br />
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="path"></param>
    public void PrintTree(int depth, string path)
    {
        PrintTreePaths(path);
        PrintDepth(depth - path.Length);

        if (Key != default(char))
        {
            ConsoleHandler.SetConsoleColor(ConsoleColor.DarkRed, ConsoleColor.Yellow);
            Console.Write($"Path:{path.PadRight(depth, ' ')} │ Key:{Key} │ Occ:{Value:D2}");
            ConsoleHandler.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.Write("\n");
        }
        else
        {
            ConsoleHandler.SetConsoleColor(ConsoleColor.Gray, ConsoleColor.White);
            Console.Write(string.IsNullOrEmpty(path)
                ? $"Tree Node - Occ:{Value:D2}"
                : $"Path:{path.PadRight(depth, ' ')} │ Node  │ Occ:{Value:D2}");
            ConsoleHandler.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.Write("\n");

            if (Children0 == null) PrintNullPath(depth, path + "0");
            else Children0?.PrintTree(depth, path + "0");

            if (Children1 == null) PrintNullPath(depth, path + "1");
            else Children1?.PrintTree(depth, path + "1");
        }
    }

    /// <summary>
    /// Recursive method to print character / Huffman Tree path<br />
    ///     - path: path to current node<br />
    /// </summary>
    /// <param name="path"></param>
    public void PrintTable(string path = "")
    {
        if (Key != default(char))
        {
            Console.Write($"{Key}:{Value}");
            Console.CursorLeft = 6;
            Console.WriteLine($"│ {path}");
        }
        else
        {
            Children0?.PrintTable(path + "0");
            Children1?.PrintTable(path + "1");
        }
    }

    /// <summary>
    /// Returns a table with all the char / Huffman Tree codes<br />
    ///     - dic: dictionary which will be filled up with the values<br />
    ///     - path: path to current node<br />
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="path"></param>
    public void GetTable(Dictionary<TreeNode, string> dic, string path = "")
    {
        if (Key != default(char))
        {
            dic.Add(this, path);
        }
        else
        {
            Children0?.GetTable(dic, path + "0");
            Children1?.GetTable(dic, path + "1");
        }
    }

    /// <summary>
    /// Gets the longest path of the whole Tree<br />
    ///     - depth: indicates how deeply nested the current node is<br />
    /// </summary>
    /// <param name="depth"></param>
    /// <returns></returns>
    public int GetDepth(int depth = 0)
    {
        if (Key != default(char)) return depth;

        int c1depth = Children0?.GetDepth(depth + 1) ?? 0;
        int c2depth = Children1?.GetDepth(depth + 1) ?? 0;

        return c1depth > c2depth ? c1depth : c2depth;
    }

    //----------------------------
    // Interfaces
    //----------------------------

    public int CompareTo(Object obj)
    {
        var t = obj as TreeNode;
        return t.Value.CompareTo(this.Value);
    }
}