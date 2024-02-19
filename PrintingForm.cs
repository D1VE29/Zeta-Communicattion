using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoZeta1
{
    public partial class PrintingForm : Form
    {
        private PictureBox pictureBox1;

        public PrintingForm()
        {
            InitializeComponent();

            // Initialize pictureBox1
            pictureBox1 = new PictureBox();
            Controls.Add(pictureBox1); // Make sure to add pictureBox1 to the Controls collection of the form
        }

        public void DisplayBarcodes(List<Bitmap> barcodeBitmaps)
        {
            int totalHeight = 0;
            foreach (Bitmap bmp in barcodeBitmaps)
            {
                pictureBox1.Image = CombineBitmaps(pictureBox1.Image, bmp, new Point(0, totalHeight));
                totalHeight += bmp.Height;
            }

            // Set PictureBox size to accommodate all barcodes
            pictureBox1.Size = new Size(300, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private Bitmap CombineBitmaps(Image background, Image overlay, Point location)
        {
            // Check if background is null
            if (background == null)
            {
                // If background is null, create a new bitmap with the size of overlay
                background = new Bitmap(overlay.Width, overlay.Height);
            }

            // Convert images to bitmaps
            Bitmap backgroundBitmap = new Bitmap(background.Clone() as Image);
            Bitmap overlayBitmap = new Bitmap(overlay.Clone() as Image);

            // Create a new bitmap with the larger height to accommodate the entire barcode
            Bitmap combinedBitmap = new Bitmap(background.Width, background.Height + overlay.Height);

            // Draw the background bitmap onto the new bitmap
            using (Graphics g = Graphics.FromImage(combinedBitmap))
            {
                g.DrawImage(backgroundBitmap, new Point(0, 0));
                g.DrawImage(overlayBitmap, location);
            }
            return combinedBitmap;
        }
    }
}