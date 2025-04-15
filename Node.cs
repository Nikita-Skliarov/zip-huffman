using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{

    public class Node
    {
        // Properties for Huffman tree
        public byte ByteValue;
        public uint Frequency;
        public Node Left;
        public Node Right;

        // Doubly Linked List references
        public Node Prev;
        public Node Next;

        // Constructor for leaf node
        public Node(byte byteValue, uint frequency)
        {
            ByteValue = byteValue;
            Frequency = frequency;
            Left = null;
            Right = null;
            Prev = null;
            Next = null;
        }

        // Constructor for internal parent node
        public Node(Node left, Node right)
        {
            Left = left;
            Right = right;
            Frequency = left.Frequency + right.Frequency;
            Prev = null;
            Next = null;
        }
    }

}
