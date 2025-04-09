using System.Xml.Linq;

namespace zip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ZipButton_Click(object sender, EventArgs e)
        {
            byte[] file = Huffman.ReadFile();
            if (file.Length == 0) return;

            uint[] freq = Huffman.CountBytes(file);
            var nodeList = Huffman.MakeDLL(freq);
            Node treeTop = Huffman.MakeTree(nodeList);
            Dictionary<byte, string> table = Huffman.MakeTable(treeTop);
            byte[] encodedFile = Huffman.Translate(file, table);
            byte[] encodedTree = Huffman.SaveTreeToBytes(treeTop);
            Huffman.WriteEncS(encodedFile, encodedTree);
        }

        private void UnzipButton_Click(object sender, EventArgs e)
        {
            Huffman.UnzipFile();
        }
    }
}
