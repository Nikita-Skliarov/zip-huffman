using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{
    public static class HuffmanUnzip
    {
        #region UNZIP


        public static byte[] UnzipFile(byte[] file)
        {
            using (BinaryReader reader = new BinaryReader(new MemoryStream(file)))
            {
                int treeLength = reader.ReadInt32(); // read first 4 bytes
                byte[] treeData = reader.ReadBytes(treeLength); // read all tree with given tree length
                byte[] fileData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

                Node root = HuffmanHelpers.RecreateTree(treeData);
                byte[] original = RecreateFile(fileData, root);

                return original;
            }
        }

        public static void SaveFile(byte[] original)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Uncompressed File";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, original);
                    MessageBox.Show("Uncompressed and saved successfully.");
                }
            }
        }

        /// <summary>
        /// Recursively reads the Huffman tree from a binary reader.
        /// </summary>
        /// <algo>
        /// 1. Read a byte from the stream to determine if it's a leaf or an internal node.
        /// 2. If it's a leaf (1):
        ///     - read the byte value and create a new node.
        ///     - return the node.
        /// 3. Otherwise (0):
        ///     - recursively read the left child.
        ///     - recursively read the right child.
        ///     - return a new internal node with the left and right children.
        /// </algo>
        public static Node ReadTree(BinaryReader reader)
        {
            byte flag = reader.ReadByte();
            if (flag == 1)
            {
                byte value = reader.ReadByte();
                return new Node(value, 0);
            }
            else
            {
                Node left = ReadTree(reader);
                Node right = ReadTree(reader);
                return new Node(left, right);
            }
        }

        /// <summary>
        /// Recreates the original file from the encoded byte array and the Huffman tree.
        /// </summary>
        /// <algo>
        /// 1. Create an empty list to store the result.
        /// 2. Set the current node to the root of the tree.
        /// 3. Iterate through each bit in the encoded file:
        ///     - define the bit as the current bit.
        ///     - if the bit is 0, move to the left child.
        ///     - if the bit is 1, move to the right child.
        ///     - if the current node is a leaf, add the byte value to the result list and reset the current node to the root.
        /// 4. Return the result list as a byte array.
        /// </algo>
        public static byte[] RecreateFile(byte[] encodedFile, Node root)
        {
            List<byte> result = new List<byte>();
            Node currentNode = root;

            for (int i = 0; i < encodedFile.Length * 8; i++)
            {
                byte bit = (byte)((encodedFile[i / 8] >> (7 - (i % 8))) & 1); // ? what does it mean? 
                if (bit == 0)
                    currentNode = currentNode.Left;
                else
                    currentNode = currentNode.Right;

                if (currentNode.Left == null && currentNode.Right == null)
                {
                    result.Add(currentNode.ByteValue);
                    currentNode = root;
                }
            }

            return result.ToArray();
        }

        #endregion UNZIP
    }
}
