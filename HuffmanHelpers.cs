using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{
    public static class HuffmanHelpers
    {
        #region ZIP

        /// <summary>
        /// Recursively builds the Huffman table. Being called from MakeTable as a helper function.
        /// </summary>
        /// <algo>
        /// 1. If the node is a leaf (has no children): assign the code to the byte value in the table.
        /// 2. Otherwise:
        /// - recursively call the function for the left child with "0" appended to the code.
        /// - recursively call the function for the right child with "1" appended to the code.
        /// </algo>

        public static void BuildTable(Node node, string code, Dictionary<byte, string> table)
        {
            if (node.Left == null && node.Right == null)
            {
                table[node.ByteValue] = code;
            }
            else
            {
                BuildTable(node.Left, code + "0", table); // ?
                BuildTable(node.Right, code + "1", table);
            }
        }

        /// <summary>
        /// Recursively writes the Huffman tree to a binary writer.
        /// Is being used as a helper function for SaveTreeToBytes.
        /// </summary>
        /// <algo>
        /// 1. If the node is null, return.
        /// 2. If the node is a leaf (has no children):
        ///     - write a marker byte (1) to indicate a leaf.
        ///     - write the byte value of the node.
        ///     - return.
        /// 3. Otherwise:
        ///     - write a marker byte (0) to indicate an internal node.
        ///     - recursively call the function for the left child.
        ///     - recursively call the function for the right child.
        /// </algo>
        public static void WriteTree(Node node, BinaryWriter writer)
        {
            if (node == null) return;

            if (node.Left == null && node.Right == null)
            {
                writer.Write((byte)1); // Leaf marker
                writer.Write(node.ByteValue);
            }
            else
            {
                writer.Write((byte)0); // Internal node
                WriteTree(node.Left, writer);
                WriteTree(node.Right, writer);
            }
        }

        #endregion ZIP

        #region UNZIP

        /// <summary>
        /// Recreates the Huffman tree from a byte array.
        /// </summary>
        /// <algo>
        /// 1. Create a memory stream and a binary reader.
        /// 2. Recursively read the tree from the stream using a helper function.
        /// </algo>
        public static Node RecreateTree(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return HuffmanUnzip.ReadTree(reader);
            }
        }

        #endregion UNZIP

        #region OtherHelpers

        /// <summary>
        /// Reads a file and returns its byte array.
        /// </summary>
        /// <algo>
        /// 1. Open modal and let user select a file.
        /// 2. If the user selects a file, read all bytes from the file.
        /// 3. Return the byte array after reading the file. Otherwise, return an empty byte array.
        /// </algo>
        public static byte[] ReadFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return File.ReadAllBytes(ofd.FileName);
                }
            }
            return new byte[0];
        }

        public static void InsertSorted(ref Node head, Node newNode)
        {
            if (head == null || newNode.Frequency < head.Frequency)
            {
                // Insert at the beginning
                newNode.Next = head;
                if (head != null) head.Prev = newNode;
                head = newNode;
                return;
            }

            Node current = head;
            while (current.Next != null && current.Next.Frequency <= newNode.Frequency)
            {
                current = current.Next;
            }

            // Insert after current
            newNode.Next = current.Next;
            if (current.Next != null)
            {
                current.Next.Prev = newNode;
            }
            current.Next = newNode;
            newNode.Prev = current;
        }

        #endregion
    }
}
