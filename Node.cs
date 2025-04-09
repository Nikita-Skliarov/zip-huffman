using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{

    public class Node
    {
        public byte ByteValue;
        public uint Frequency;
        public Node Left;
        public Node Right;

        public Node(byte byteValue, uint frequency)
        {
            ByteValue = byteValue;
            Frequency = frequency;
            Left = null;
            Right = null;
        }

        public Node(Node left, Node right)
        {
            Left = left;
            Right = right;
            Frequency = left.Frequency + right.Frequency;
        }
    }

}
