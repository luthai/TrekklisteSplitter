using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace TrekklisterSplitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtSourceFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "PDF Files|*.pdf";
            openFileDialog.Title = "Velg en trekkliste";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSourceFile.Text = openFileDialog.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PdfDocument inputDocument = PdfReader.Open(txtSourceFile.Text, PdfDocumentOpenMode.Import);

            PdfDocument outputDocument = new PdfDocument();

            outputDocument.Version = inputDocument.Version;
            outputDocument.Info.Title = String.Format("Page {0} of {1}", 2, inputDocument.Info.Title);
            outputDocument.Info.Creator = inputDocument.Info.Creator;

            outputDocument.AddPage(inputDocument.Pages[0]);
            outputDocument.AddPage(inputDocument.Pages[1]);
            outputDocument.AddPage(inputDocument.Pages[2]);
            outputDocument.Save(String.Format("{0}.pdf", "Testfile"));
        }
    }
}
