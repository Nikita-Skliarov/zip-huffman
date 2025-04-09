using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zip
{
    public static class Huffman
    {
        public static byte[] ReadFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files|*.txt|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return File.ReadAllBytes(ofd.FileName);
                }
            }
            return new byte[0];
        }

        public static uint[] CountBytes(byte[] file)
        {
            uint[] frequencies = new uint[256];
            foreach (byte b in file)
            {
                frequencies[b]++;
            }
            return frequencies;
        }

        public static List<Node> MakeDLL(uint[] freq)
        {
            List<Node> nodeList = new List<Node>();
            for (int i = 0; i < 256; i++) // Changed 'byte' to 'int' to avoid CS0652  
            {
                if (freq[i] > 0)
                {
                    nodeList.Add(new Node((byte)i, freq[i]));
                }
            }
            return nodeList;
        }

        public static Node MakeTree(List<Node> nodes)
        {
            while (nodes.Count > 1)
            {
                nodes.Sort((n1, n2) => (int)(n1.Frequency - n2.Frequency));

                Node left = nodes[0];
                Node right = nodes[1];

                nodes.RemoveAt(0);
                nodes.RemoveAt(0);

                Node parent = new Node(left, right);
                nodes.Add(parent);
            }
            return nodes[0];
        }

        public static Dictionary<byte, string> MakeTable(Node root)
        {
            Dictionary<byte, string> table = new Dictionary<byte, string>();
            BuildTable(root, "", table);
            return table;
        }

        private static void BuildTable(Node node, string code, Dictionary<byte, string> table)
        {
            if (node.Left == null && node.Right == null)
            {
                table[node.ByteValue] = code;
            }
            else
            {
                BuildTable(node.Left, code + "0", table);
                BuildTable(node.Right, code + "1", table);
            }
        }

        public static byte[] Translate(byte[] file, Dictionary<byte, string> table)
        {
            List<bool> bitList = new List<bool>();
            foreach (byte b in file)
            {
                string code = table[b];
                foreach (char bit in code)
                {
                    bitList.Add(bit == '1');
                }
            }

            int byteCount = (bitList.Count + 7) / 8;
            byte[] result = new byte[byteCount];
            for (int i = 0; i < bitList.Count; i++)
            {
                if (bitList[i])
                {
                    result[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return result;
        }

        public static byte[] SaveTreeToBytes(Node root)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                WriteTree(root, writer);
                return ms.ToArray();
            }
        }

        private static void WriteTree(Node node, BinaryWriter writer)
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

        public static Node RecreateTree(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return ReadTree(reader);
            }
        }

        private static Node ReadTree(BinaryReader reader)
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

        public static byte[] RecreateFile(byte[] encodedFile, Node root)
        {
            List<byte> result = new List<byte>();
            Node currentNode = root;

            for (int i = 0; i < encodedFile.Length * 8; i++)
            {
                byte bit = (byte)((encodedFile[i / 8] >> (7 - (i % 8))) & 1);
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

        public static void UnzipFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Huffman Compressed File|*.zip";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] allData = File.ReadAllBytes(ofd.FileName);
                    using (BinaryReader reader = new BinaryReader(new MemoryStream(allData)))
                    {
                        int treeLength = reader.ReadInt32();
                        byte[] treeData = reader.ReadBytes(treeLength);
                        byte[] fileData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

                        Node root = RecreateTree(treeData);
                        byte[] original = RecreateFile(fileData, root);

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
                }
            }
        }
    }

}
