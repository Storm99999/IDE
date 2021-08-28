using Storm_IDE_V2.TextColors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storm_IDE_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string ProjectPath = "";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in|if|bool)\b";
            string writtenCode = codeBox.Text;
            MatchCollection keyWordsCollection = Regex.Matches(writtenCode, keywords);
            string types = @"\b(Console)\b";
            MatchCollection typesCollection = Regex.Matches(writtenCode, types);
            string comments = @"\/\/.+?$|\/\*.+?\*\/";
            MatchCollection commentsCollection = Regex.Matches(writtenCode, comments, RegexOptions.Multiline);
            string strings = "\".+?\"";
            MatchCollection stringCollection = Regex.Matches(writtenCode, strings);

            int originalIndex = codeBox.SelectionStart;
            int originalLength = codeBox.SelectionLength;
            Color originalColor = Color.Black;

            codeBox.SelectionStart = 0;
            codeBox.SelectionLength = codeBox.Text.Length;
            codeBox.SelectionColor = originalColor;

            foreach (Match match in keyWordsCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = TC.MagentaColor;
            }
            foreach (Match match in typesCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = TC.DarkCyanColor;
            }
            foreach (Match match in commentsCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = TC.GreenColor;
            }
            foreach (Match match in stringCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = TC.GreenColor;
            }
            codeBox.SelectionStart = originalIndex;
            codeBox.SelectionLength = originalLength;
            codeBox.SelectionColor = originalColor;
            codeBox.Focus();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.BackColor = Color.White;
            treeView1.BackColor = Color.White;
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.BackColor = Color.FromArgb(37, 37, 38);
            treeView1.BackColor = Color.FromArgb(37, 37, 38);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            treeView1.Nodes.Clear();
            foreach (var dirs in Directory.GetDirectories(folderBrowserDialog1.SelectedPath))
            {
                DirectoryInfo DirInfo = new DirectoryInfo(dirs);
                var nodes = treeView1.Nodes.Add(DirInfo.Name, DirInfo.Name, 0);
                nodes.Tag = DirInfo;
            }
            foreach (var Files in Directory.GetFiles(folderBrowserDialog1.SelectedPath))
            {
                FileInfo fileInfo = new FileInfo(Files);
                var nodes = treeView1.Nodes.Add(fileInfo.Name, fileInfo.Name, 1);
                nodes.Tag = fileInfo;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null)
            {
                // do nothing
            }
            else if (e.Node.Tag.GetType() == typeof(DirectoryInfo))
            {
                // Opens the folder
                e.Node.Nodes.Clear();
                foreach (var dirs in Directory.GetDirectories(((DirectoryInfo)e.Node.Tag).FullName))
                {
                    DirectoryInfo DirInfo = new DirectoryInfo(dirs);
                    var nodes = e.Node.Nodes.Add(DirInfo.Name);
                    nodes.Tag = DirInfo;
                }
                foreach (var dirs in Directory.GetFiles(((DirectoryInfo)e.Node.Tag).FullName))
                {
                    FileInfo DirInfo = new FileInfo(dirs);
                    var nodes = e.Node.Nodes.Add(DirInfo.Name);
                    nodes.Tag = DirInfo;
                }
            }
            else
            {
                codeBox.Text = File.ReadAllText(((FileInfo)e.Node.Tag).FullName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("no");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
