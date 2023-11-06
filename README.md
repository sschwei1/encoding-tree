# EncodingTree

EncodingTree is a project demonstrating text compression using the Huffman Code.

## Example

```cs
var tree = new Tree(txt);

// print encoding table to console
tree.PrintTable();

// print tree structure and some information of each node to console
tree.PrintTree();

// print information about compression to console (bitsize of encoding is taken into consideration)
tree.PrintCompressionRate(16);

// get bits of encoded data
var encodedText = tree.GetEncodedText();
Console.WriteLine($"\"{txt}\" encoded:\n{tree.GetEncodedText()}\n\n");
```
