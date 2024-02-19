using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using OfficeOpenXml;
using ExcelDataReader;
using System.Drawing.Printing;
using System.Drawing;
using ZXing;
using Action = System.Action;

namespace ProjetoZeta1
{
    public partial class Communication_Manager : Form
    {
        private string filePath = @"C:\Users\User\Desktop\yes\Teste1.xlsx";
        private List<Tuple<int, int, string>> spliceCellPositions = new List<Tuple<int, int, string>>(); // Store positions and values here
        string MachineFiles = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProjetoZeta", "Ficheiros", "Producti.sdc");
        string machineFilePath = @"C:\Users\User\Desktop\ProjetoZeta\Ficheiros";
        StringBuilder messageBuilder = new StringBuilder();
        private string ZetaInfo = "";
        private static readonly object locker = new object();
        private FileSystemWatcher fileWatcher;
        private StringBuilder messageBuilderReadExcel;
        private DateTime lastFileChangeTime = DateTime.MinValue;
        private readonly TimeSpan debounceInterval = TimeSpan.FromSeconds(1); // Adjust the interval as needed
        private Timer checkFileChangesTimer;
        private string baseDirectory = "";
        public Communication_Manager()
        {
            InitializeComponent();
            StartFileMonitoring();
        }
        private void StartFileMonitoring()
        {
            // Initialize the StringBuilder for building the message
            messageBuilderReadExcel = new StringBuilder();

            // Create a new FileSystemWatcher
            fileWatcher = new FileSystemWatcher(@"C:\Users\User\Desktop\ProjetoZeta");

            // Set the notification filters
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;

            // Attach the event handler for file changes
            fileWatcher.Changed += MachineFileChanged;
            fileWatcher.Created += MachineFileChanged;

            // Enable the FileSystemWatcher
            fileWatcher.EnableRaisingEvents = true;


            // Initialize the timer for checking file changes
            checkFileChangesTimer = new Timer();
            checkFileChangesTimer.Interval = 1000; // Adjust the interval as needed (in milliseconds)
            checkFileChangesTimer.Tick += CheckFileChangesTimer_Tick;
            checkFileChangesTimer.Start();
        }

        private void MachineFileChanged(object sender, FileSystemEventArgs e)
        {
            // Record the time of the last file change
            lastFileChangeTime = DateTime.Now;
        }

        private void CheckFileChangesTimer_Tick(object sender, EventArgs e)
        {
            // Check if the file has changed since the last check
            DateTime lastWriteTime = File.GetLastWriteTime(fileWatcher.Path);

            if (lastWriteTime > lastFileChangeTime)
            {
                // File has changed, read the new content
                ReadMachineFileContent(fileWatcher.Path);

                // Update the last change time
                lastFileChangeTime = lastWriteTime;
            }
        }
        private void ReadMachineFileContent(string filePath)
        {
            try
            {
                messageBuilderReadExcel.Clear();
                string[] lines = File.ReadAllLines(filePath);
                bool isInsideDesiredBlock = false;

                foreach (string line in lines)
                {
                    if (line.Contains("[ProductionTerminated]"))
                    {
                        Console.WriteLine(line);
                        isInsideDesiredBlock = true;
                        messageBuilderReadExcel.AppendLine(line);
                    }
                    else if (line.StartsWith("ArticleKey=") || line.StartsWith("DateTimeStamp="))
                    {
                        if (isInsideDesiredBlock)
                        {
                            messageBuilderReadExcel.AppendLine(line);
                            Console.WriteLine(line);
                        }
                    }
                }

                if (messageBuilderReadExcel.Length > 0)
                {
                    MessageBox.Show("This was found in machine file: \n" + messageBuilderReadExcel.ToString(), "Found inside machine file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Machine file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Console.WriteLine("Machine file not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong with the machine file, check all files and the behavior of the machines and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("An error occurred with the machine file: " + ex.Message);
            }
        }
     
        private void Form1_Load(object sender, EventArgs e)
        {
            btnCreateFiles.Enabled = true;
            txt1_FilePathSet.Enabled = true;
            UpdateFilePath();
            btn_SEND1.Select();
        }
        private void UpdateFilePath()
        {
            {
                // Get the current date
                DateTime currentDate = DateTime.Now;
                // Define the base directory where your files will be stored
                string baseDirectory = @"C:\Users\User\Desktop\ProjetoZeta\Ficheiros";

                // Create or access the year directory
                string yearDirectory = Path.Combine(baseDirectory, currentDate.Year.ToString("0000"));
                if (!Directory.Exists(yearDirectory))
                {
                    Directory.CreateDirectory(yearDirectory);
                }

                // Create or access the month directory
                string monthDirectory = Path.Combine(yearDirectory, currentDate.Month.ToString("00"));
                if (!Directory.Exists(monthDirectory))
                {
                    Directory.CreateDirectory(monthDirectory);
                }

                // Create or access the day directory
                string dayDirectory = Path.Combine(monthDirectory, currentDate.Day.ToString("00"));
                if (!Directory.Exists(dayDirectory))
                {
                    Directory.CreateDirectory(dayDirectory);
                }

                // Format the current date as "dd-MM-yyyy"
                string dateStr = currentDate.ToString("dd-MM-yyyy");

                // Search for .SDC files in the day-specific directory
                string[] sdcFiles = Directory.GetFiles(dayDirectory);

                if (sdcFiles.Length > 0)
                {
                    // Sort the files by creation date and get the most recent one
                    string mostRecentSdcFile = sdcFiles
                        .Select(file => new FileInfo(file))
                        .OrderByDescending(fileInfo => fileInfo.CreationTime)
                        .First()
                        .FullName;
                    // Extract the date from the most recent .SDC file's name
                    string sdcDateStr = Path.GetFileNameWithoutExtension(mostRecentSdcFile);
                    // Use the name of the most recent .SDC file without extension
                    dateStr = sdcDateStr;
                }

                // Create or access the file with the current date in the filename
                string filePath = Path.Combine(dayDirectory, $"{dateStr}.SDC");

                // Display the file path in a label or wherever you need it
                txt1_FilePathSet.Text = filePath;
                Console.WriteLine(filePath);
            }
        }
        private void btnReadFiles_Click(object sender, EventArgs e)
        {
            try
            {
                messageBuilder.Clear();
                string[] lines = File.ReadAllLines(MachineFiles);
                bool isInsideDesiredBlock = false;
                foreach (string line in lines)
                {
                    if (line.Contains("[ProductionTerminated]"))
                    {
                        Console.WriteLine(line);
                        isInsideDesiredBlock = true;
                        messageBuilder.AppendLine(line);
                    }
                    else if (line.StartsWith("ArticleKey=") || line.StartsWith("Date"))
                    {
                        if (isInsideDesiredBlock)
                        {
                            messageBuilder.AppendLine(line);
                            Console.WriteLine(line);
                        }
                    }
                }
                if (messageBuilder.Length > 0)
                {
                    ZetaInfo += messageBuilder.ToString();
                    MessageBox.Show("This was found: \n" + messageBuilder.ToString(), "Found inside file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Console.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, check all files and the behavior of the machines and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private StringBuilder spliceInfoBuilder = new StringBuilder();


        private void createProductiFile()
        {
            try
            {
                messageBuilder.Clear();
                string[] lines = File.ReadAllLines(MachineFiles);
                bool isInsideDesiredBlock = false;
                StringBuilder productionTerminatedInfoBuilder = new StringBuilder(); // Initialize StringBuilder for ProductionTerminated info

                foreach (string line in lines)
                {
                    if (line.Contains("[ProductionTerminated]"))
                    {
                        Console.WriteLine(line);
                        isInsideDesiredBlock = true;
                        messageBuilder.AppendLine(line);
                        productionTerminatedInfoBuilder.AppendLine(line); // Append the line containing [ProductionTerminated]
                    }
                    else if (isInsideDesiredBlock && (line.StartsWith("ArticleKey=") || line.StartsWith("DateTimeStamp=")))
                    {
                        messageBuilder.AppendLine(line);
                        Console.WriteLine(line);
                        productionTerminatedInfoBuilder.AppendLine(line); // Append the relevant lines inside the block
                    }
                }

                if (productionTerminatedInfoBuilder.Length > 0)
                {   
                    // Create a file and write the accumulated information to it
                    string outputProductionTerminatedPath = @"C:\Users\User\Desktop\yes\ProductionTerminatedInfo.txt";
                    File.WriteAllText(outputProductionTerminatedPath, productionTerminatedInfoBuilder.ToString());
                    MessageBox.Show("Production Terminated information saved to file!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Console.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, check all files and the behavior of the machines and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }
        private void btnCreateFiles_Click(object sender, EventArgs e)
        {
            Console.WriteLine(machineFilePath);
            DialogResult ExcelFilePathConfirmation = MessageBox.Show("You have chosen this: "
                + machineFilePath + " ,as your file path, do you want to continue?",
               "Chosen File Path", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (ExcelFilePathConfirmation == DialogResult.Yes)
            {
                try
                {
                    createProductiFile();
                    readExcel();
                    MessageBox.Show("File copied successfully!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Copying process not started", "Source File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception)
                {
                    MessageBox.Show("Copy of files not successful", "Error copying files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult ExcelFilePathConfirmation = MessageBox.Show("You have chosen this: "
                + MachineFiles + " ,as your file path, do you want to continue?",
               "Chosen File Path", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ExcelFilePathConfirmation == DialogResult.Yes)
            {
                Console.WriteLine(MachineFiles);
            }
        }
        private bool IsFilePathValid(string path)
        {
            return !string.IsNullOrEmpty(path) && System.IO.File.Exists(path);
        }
        private void btn_SEND1_Click(object sender, EventArgs e)
        {
            SearchSpliceInfo();
            readExcelConversionTable();
            DialogResult warningConfirmation = MessageBox.Show("Are you sure you want to SEND?", "Send Confirmation Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warningConfirmation == DialogResult.Yes)
            {
                if (IsFilePathValid(filePath))
                {
                    readExcel();
                }
                else
                {
                    MessageBox.Show("Invalid file path. Please enter a valid Excel file path.");
                }
            }
        }
        string firstColumnValue = "";
        private void SearchSpliceInfo()
        {
            string[] lines = spliceInfoBuilder1.ToString().Split('\n');
            for (int i = 0; i < spliceInfoBuilder1.Length; i++)
            {
                if (lines[i].StartsWith("Section:"))
                {
                    // Extract the current section value
                    string sectionLine = lines[i];
                    string[] parts = sectionLine.Split(':');
                    string currentSectionValue = parts[1].Trim();

                    // You can add your logic here to modify the section value
                    if (lines[i].StartsWith("Section: "))
                    {
                        // Update the line with the new section value
                        lines[i] = $"Section: {firstColumnValue:F2}"; // Format the double to two decimal places
                        break; // Exit the loop since we found and modified the "Section" line
                    }
                    Console.WriteLine(lines);
                }
            }
        }
        private StringBuilder spliceInfoBuilder1 = new StringBuilder();
        private void readExcelConversionTable()
        {
            // Specify the file path to the Excel file you want to read
            string filepath = @"C:\Users\User\Desktop\yes\Teste1.xlsx";
            try
            {
                // Open the Excel file using a stream for improved performance
                using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    // Create an ExcelDataReader to read the Excel file from the stream
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Loop through each worksheet in the Excel file
                        do
                        {
                            // Read each row in the current worksheet
                            while (reader.Read())
                            {
                                // Read and process data from the Excel file as needed
                                // In this example, we are reading values from the 1st and 2nd columns
                                // The GetString() method is used to read the cell values as strings
                                firstColumnValue = reader.GetString(0); // 1st column
                                string secondColumnValue = reader.GetString(1); // 2nd column

                                // Perform your processing here, such as storing or displaying the data
                                Console.WriteLine($"2nd Column Value: {secondColumnValue}, 1st Column Value: {firstColumnValue}");
                            }
                        } while (reader.NextResult()); // Move to the next worksheet (if any)
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the reading process
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void readExcel()
        {
            string filePath = @"C:\Users\User\Desktop\yes\Teste1.xlsx"; // Update with your file path
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming the first worksheet

                    int rowCount = worksheet.Dimension.Rows;
                    int zoneAColumn = 4;
                    int sectionColumn = 21;
                    string searchValue = "splice";
                    // Initialize the StringBuilder to store the information
                    StringBuilder spliceInfoBuilder1 = new StringBuilder();
                    for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                    {
                        string kitName = worksheet.Cells[row, 1].Text; // Read the value from the first column
                        for (int col = 4; col <= 21; col++)
                        {
                            if (worksheet.Cells[row, col].Text.ToLower() == searchValue)
                            {
                                string zoneAValue = worksheet.Cells[row, zoneAColumn].Text;
                                string sectionValue = worksheet.Cells[row, sectionColumn].Text;
                                if (!string.IsNullOrEmpty(zoneAValue))
                                {
                                    string spliceInfo = $"Found 'splice' at Row: {row}, Column: {col}" + Environment.NewLine +
                                                        $"Zone A: {zoneAValue}" + Environment.NewLine +
                                                        $"Section: {sectionValue}" + Environment.NewLine +
                                                        $"KitName: {kitName}" + Environment.NewLine;
                                    // Append the information to the StringBuilder
                                    spliceInfoBuilder1.AppendLine(spliceInfo);
                                    break; // Exit the inner loop when a match is found
                                }
                            }
                        }
                    }
                    // Create a file and write the accumulated information to it
                    string outputPath = @"C:\Users\User\Desktop\Yes\FicheiroEnvio.txt";
                    File.WriteAllText(outputPath, spliceInfoBuilder1.ToString());
                    MessageBox.Show("File created and information copied successfully!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InformationWindow form2 = new InformationWindow(spliceInfoBuilder1.ToString());
                    form2.Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            try
            {
                messageBuilder.Clear();
                string[] lines = File.ReadAllLines(MachineFiles);
                bool isInsideDesiredBlock = false;
                foreach (string line in lines)
                {
                    if (line.Contains("[ProductionTerminated]"))
                    {
                        Console.WriteLine(line);
                        isInsideDesiredBlock = true;
                        messageBuilder.AppendLine(line);
                    }
                    else if (line.StartsWith("ArticleKey=") || line.StartsWith("DateTimeStamp="))
                    {
                        if (isInsideDesiredBlock)
                        {
                            messageBuilder.AppendLine(line);
                            Console.WriteLine(line);
                        }
                    }
                }
                if (messageBuilder.Length > 0)
                {
                    MessageBox.Show("This was found: \n" + messageBuilder.ToString(), "Found inside file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Console.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, check all files and the behavior of the machines and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        /* private void btn_Print_Click(object sender, EventArgs e, string printerName)
         {
             // Create a PrintDocument object
             PrintDocument pd = new PrintDocument();

             // Set the printer name (you can obtain a list of available printers and select one)
             pd.PrinterSettings.PrinterName = "NOME_IMPRESSORA";

             // Hook up an event handler for the PrintPage event
             pd.PrintPage += (printSender, printEventArgs) =>
             {
                 // Create fonts and brushes for different types of text
                 System.Drawing.Font titleFont = new System.Drawing.Font("BarCodeFont", 16, FontStyle.Bold);
                 System.Drawing.Font normalFont = new System.Drawing.Font("BarCodeFont", 12);
                 Brush titleBrush = Brushes.Black;
                 Brush normalBrush = Brushes.Black;

                 // Define positions for different text sections
                 PointF titlePosition = new PointF(100, 100);
                 PointF normalTextPosition = new PointF(100, 150);

                 // Define the content of the letter
                 //string title = "Job Made";
                 //string normalText = "This is the main content of the letter...";

                 // Draw the title with a different font and style
                 // printEventArgs.Graphics.DrawString(title, titleFont, titleBrush, titlePosition);

                 // Move to a new line for the normal text
                 normalTextPosition.Y += 50; // Adjust the Y-coordinate as needed

                 // Draw the normal text using the normal font and style
                 printEventArgs.Graphics.DrawString(ZetaInfo, normalFont, normalBrush, normalTextPosition);
             };
             // Start the printing process
             pd.Print();
         }*/
        private void button1_Click_1(object sender, EventArgs e)
        {
            // Split ZetaInfo into parts using new lines
            string[] infoLines = ZetaInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Create a list to store barcode bitmaps
            List<Bitmap> barcodeBitmaps = new List<Bitmap>();

            // Iterate through lines and generate a barcode for each line
            foreach (string line in infoLines)
            {
                // Log the line to check if it contains the expected data
                Console.WriteLine($"Processing line: {line}");

                // Generate the barcode bitmap for the current line
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.CODE_128;
                barcodeWriter.Options = new ZXing.Common.EncodingOptions
                {
                    Width = 900,
                    Height = 150,
                    Margin = 10
                };

                if (line.Contains("ArticleKey="))
                {
                    Bitmap barcodeBitmap = barcodeWriter.Write(line);

                    // Add the barcode bitmap to the list
                    barcodeBitmaps.Add(barcodeBitmap);
                }
            }

            // Assuming PrintingForm is the name of the form instance
            PrintingForm printingForm = new PrintingForm();

            // Pass the list of barcode bitmaps to the method in PrintingForm
            printingForm.DisplayBarcodes(barcodeBitmaps);

            // Show the PrintingForm
            printingForm.Show();
        }

        private void txt1_FilePathSet_TextChanged(object sender, EventArgs e)
        {
            txt1_FilePathSet.Text += baseDirectory;
        }
    }
}