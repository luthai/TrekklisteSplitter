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
    public partial class frmMainWindow : Form
    {
        public frmMainWindow()
        {
            InitializeComponent();
        }

        private void frmMainWindow_Load(object sender, EventArgs e)
        {

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
            // Disable button
            btnStart.Enabled = false;
            btnCancel.Enabled = false;

            // Source file path
            string sourcePdfPath = txtSourceFile.Text;

            // Output file path
            string currentMonth = DateTime.Now.ToString("MMMMM");
            string outputFolderName = "Trekklister " + currentMonth;
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputPdfFolder = System.IO.Path.Combine(pathDesktop, outputFolderName);
            System.IO.Directory.CreateDirectory(outputPdfFolder);
            string extension = ".pdf";
            string outputPdfPath = string.Empty;

            // Starting page number
            int pageNumber = 1;

            string[,] arrayLeverandor = new string[36, 2] 
            {   
                {"Leverandør 56000", "MFO " + currentMonth.ToLower() + " trekkliste"}, 
                {"Leverandør 56001", "Skolenes "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56002", "FO "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56004", "Delta "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56006", "Akademikerforbundet "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56007", "Bibforb "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56009", "Ergoterapeutene "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56013", "NITO "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56019", "Elogit "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56022", "Espen Frankmoen "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56024", "Forskerforbundet "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56190", "Skolelederforbundet "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 56300", "Parat "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57559", "Conecto Østerås "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57600", "Dahl Inkasso "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57641", "Kreditorforeningen "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57643", "Visma Collectors "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57648", "Svea Finans "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57655", "Lindorff "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57666", "Bidragstrekk "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57706", "Kredittgjenvinning "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57712", "SI "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57716", "Lindorff Obligations "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57717", "Kredinor Oslo "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57719", "Skankred "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57726", "NAVI "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57727", "Credicare "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57728", "Ringsaker kommune "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57740", "PRA "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57744", "Skatt Øst "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57746", "Melin Collectors "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57747", "AHSD "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57748", "Eurocard "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57756", "Sergel Norge "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57757", "Conecto Hamar "  + currentMonth.ToLower() + " trekkliste"},
                {"Leverandør 57775", "Intrum Justitia "  + currentMonth.ToLower() + " trekkliste"}
            };

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

                for (int i = 0; i < arrayLeverandor.GetLength(0); ++i)
                {
                    List<int> pages = new List<int>();
                    pages = ReadPdfFile(sourcePdfPath, arrayLeverandor[i, 0]);

                    for (int j = 0; j < arrayLeverandor.GetLength(1); ++j)
                    {
                        outputPdfPath = System.IO.Path.Combine(outputPdfFolder, arrayLeverandor[i, j]) + extension;
                    }

                    if (pages.Count != 0)
                    {
                        // Initialize an instance of the PdfCopyClass with the source 
                        // document and an output file stream
                        pdfCopyProvider = new PdfCopy(sourceDocument,
                            new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                        sourceDocument.Open();

                        for (int idx = 0; idx < pages.Count; ++idx)
                        {
                            // Extract the desired page number
                            importedPage = pdfCopyProvider.GetImportedPage(reader, pages[idx]);
                            pdfCopyProvider.AddPage(importedPage);
                        }
                    }
                }

                sourceDocument.Close();
                reader.Close();

                System.Windows.Forms.MessageBox.Show("Trekkliste ferdig splittet!!!");

                // Enabled button
                btnStart.Enabled = true;
                btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw ex;
            }
        }

        public List<int> ReadPdfFile(string sourceFileName, string searchText)
        {
            List<int> pages = new List<int>();
            if (File.Exists(sourceFileName))
            {
                PdfReader pdfReader = new PdfReader(sourceFileName);
                for (int i = 1; i < pdfReader.NumberOfPages + 1; ++i)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, i, strategy);
                    if (currentPageText.Contains(searchText))
                    {
                        pages.Add(i);
                    }
                }
                pdfReader.Close();
            }

            return pages;
        }      
    }
}
