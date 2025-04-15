using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{
    public static class HuffmanZip
    {
        #region ZIP

       

        /// <summary>
        /// Counts the frequency of each byte in the file.
        /// </summary>
        /// <algo>
        /// 1. Create an array of 256 empty uints to store frequencies.
        /// 2. Iterate through each byte in the file.
        /// 3. Increment the frequency of the byte in the array if it is found.
        /// 4. Return the array of frequencies.
        /// </algo>

        public static uint[] CountBytes(byte[] file)
        {
            uint[] frequencies = new uint[256];
            foreach (byte b in file)
            {
                frequencies[b]++;
            }
            return frequencies;
        }


        /// <summary>
        /// Creates a doubly linked list of nodes based on the frequency array.
        /// </summary>
        /// <algo>
        /// 1. Create an empty list of nodes.
        /// 2. Iterate through the frequency array.
        /// 3. If the frequency of a byte is greater than 0, 
        /// create a new node with the byte and its frequency.
        /// 4. Add the node to the list.
        /// 5. Return the list of nodes.
        /// </algo>
        public static Node MakeDLL(uint[] freq)
        {
            Node head = null;

            for (int i = 0; i < 256; i++)
            {
                if (freq[i] > 0)
                {
                    Node newNode = new Node((byte)i, freq[i]);
                    HuffmanHelpers.InsertSorted(ref head, newNode); // insert in ascending order
                }
            }

            return head; // return head of the DLL
        }

        /// <summary>
        /// Creates a Huffman tree from the list of nodes.
        /// </summary>
        /// <algo>
        /// 1. While the list of nodes has more than one node:
        /// 2. Sort the list of nodes by frequency.
        /// 3. Take the two nodes with the lowest frequency.
        /// 4. Remove them from the list.
        /// 5. Create a new parent node with the two nodes as children.
        /// 6. Add the parent node to the list.
        /// 7. Return the root node of the tree. (can be used to recreate the tree)

        public static Node MakeTree(Node head)
        {
            while (head != null && head.Next != null)
            {
                // Take first two nodes
                Node left = head;
                Node right = head.Next;

                // Remove them from the DLL
                head = right.Next;
                if (head != null) head.Prev = null;

                // Create new parent node
                Node parent = new Node(left, right);

                // Insert back into sorted DLL
                HuffmanHelpers.InsertSorted(ref head, parent);
            }

            return head; // this is the root of the Huffman tree
        }

        /// <summary>
        /// Creates a Huffman table from the tree.
        /// </summary>
        /// <algo>
        /// 1. Create an empty dictionary to store the table.
        /// 2. Call the recursive function to build the table.
        /// 3. Return the dictionary.
        /// </algo>
        public static Dictionary<byte, string> MakeTable(Node root)
        {
            Dictionary<byte, string> table = new Dictionary<byte, string>();

            // Begin to build table from head of the tree (empty code because it's head of tree)
            HuffmanHelpers.BuildTable(root, "", table);
            return table;
        }



        /// <summary>
        /// Translates the file into a byte array using the Huffman table.
        /// </summary>
        /// <algo>
        /// 1. Create an empty list of bits.
        /// 2. For each byte in the file:
        ///     - Get the code from the table.
        ///     - For each bit in the code:
        ///         * Add the bit to the list.
        ///         * If the bit is 1, set the corresponding bit in the byte.
        ///         * If the bit is 0, do nothing.
        /// 3. Calculate the number of bytes needed to store the bits.
        /// 4. Create a new byte array of the calculated size.
        /// 5. For each bit in the list:
        ///     - If the bit is 1, set the corresponding bit in the byte.
        ///     - If the bit is 0, do nothing.
        ///     - Store the byte in the result array.
        /// 6. Return the byte array.
        /// </algo>
        public static byte[] Translate(byte[] file, Dictionary<byte, string> table)
        {
            List<bool> bitList = new List<bool>();
            foreach (byte b in file)
            {
                string code = table[b];
                foreach (char bit in code)
                {
                    // Add the bit to the list (true or false, so 1 or 0)
                    bitList.Add(bit == '1');
                }
            }

            int byteCount = (bitList.Count + 7) / 8;
            byte[] result = new byte[byteCount];
            for (int i = 0; i < bitList.Count; i++)
            {
                if (bitList[i])
                {
                    // Move bite in its place and save it in result
                    result[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the Huffman tree to a byte array.
        /// </summary>
        /// <algo>
        /// 1. Create a memory stream and a binary writer.
        /// 2. Write the tree to the stream using a recursive function.
        /// 3. Return the byte array from the memory stream.
        /// </algo>
        public static byte[] SaveTreeToBytes(Node root)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                HuffmanHelpers.WriteTree(root, writer);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Writes the encoded file and the tree data to a new file.
        /// </summary>
        /// <algo>
        /// 1. Open a save file dialog to let the user choose a location.
        /// 2. If the user chooses a file name and location, create a file stream and a binary writer.
        /// 3. Write the length of the tree data to the file.
        /// 4. Write the tree data to the file.
        /// 5. Write the encoded file to the file.
        /// </algo>
        public static void WriteEncS(byte[] encodedFile, byte[] treeData)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Huffman Compressed File|*.zip";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(treeData.Length);
                        writer.Write(treeData);
                        writer.Write(encodedFile);
                    }
                }
            }
        }

        #endregion ZIP
    }
}
