using System.Xml.Linq;

namespace zip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // TODO: MakeDLL make another way, make tree also 
        // Then check if anything is wrong with new changes
        private void ZipButton_Click(object sender, EventArgs e)
        {
            byte[] file = HuffmanHelpers.ReadFile();
            if (file.Length == 0) return; // return if no file is selected or file is empty

            uint[] freq = HuffmanZip.CountBytes(file);

            // NEW: MakeDLL now returns the head of a sorted doubly linked list
            Node dllHead = HuffmanZip.MakeDLL(freq);

            // NEW: MakeTree now works with DLL head
            Node treeTop = HuffmanZip.MakeTree(dllHead);

            Dictionary<byte, string> table = HuffmanZip.MakeTable(treeTop);
            byte[] encodedFile = HuffmanZip.Translate(file, table);
            byte[] encodedTree = HuffmanZip.SaveTreeToBytes(treeTop);
            HuffmanZip.WriteEncS(encodedFile, encodedTree);
        }

        private void UnzipButton_Click(object sender, EventArgs e)
        {
            byte[] file = HuffmanHelpers.ReadFile();
            byte[] unzippedFile = HuffmanUnzip.UnzipFile(file);
            HuffmanUnzip.SaveFile(unzippedFile);
        }
    }
}
