using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

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
            string sourcePdfPath = txtSourceFile.Text;
            string outputPdfPath = @"C:\Users\Thai\Desktop\TrekklisterSplit Testdata - Copy\ex\test.pdf";
            int pageNumber = 3;

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file
                reader = new PdfReader(sourcePdfPath);

                // Capture the correct size and orientation for the page
                sourceDocument = new Document(reader.GetPageSizeWithRotation(pageNumber));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();

                // Extract the desired page number
                importedPage = pdfCopyProvider.GetImportedPage(reader, 1);
                pdfCopyProvider.AddPage(importedPage);

                importedPage = pdfCopyProvider.GetImportedPage(reader, 2);
                pdfCopyProvider.AddPage(importedPage);

                importedPage = pdfCopyProvider.GetImportedPage(reader, 3);
                pdfCopyProvider.AddPage(importedPage);


                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw ex;
            }

        }
        
    }
}
