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
            byte[] file = HuffmanZip.ReadFile();
            if (file.Length == 0) return; // return if no file is selected or file is empty

            uint[] freq = HuffmanZip.CountBytes(file);
            List<Node> nodeList = HuffmanZip.MakeDLL(freq);
            Node treeTop = HuffmanZip.MakeTree(nodeList);
            Dictionary<byte, string> table = HuffmanZip.MakeTable(treeTop);
            byte[] encodedFile = HuffmanZip.Translate(file, table);
            byte[] encodedTree = HuffmanZip.SaveTreeToBytes(treeTop);
            HuffmanZip.WriteEncS(encodedFile, encodedTree);
        }

        private void UnzipButton_Click(object sender, EventArgs e)
        {
            HuffmanUnzip.UnzipFile();
        }
    }
}
