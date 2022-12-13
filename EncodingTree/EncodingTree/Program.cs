using EncodingTree;

var txt = "";
while (txt != "stop")
{
    ConsoleHandler.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
    Console.Clear();
                
    Console.Write("Enter Text: ");
    txt = Console.ReadLine();
    Console.Write("\n\n");

    var tree = new Tree(txt);
    tree.PrintTable();
    tree.PrintTree();
    Console.WriteLine($"\"{txt}\" encoded:\n{tree.GetEncodedText()}\n\n");
    tree.PrintCompressionRate(16);
    Console.ReadKey();
}