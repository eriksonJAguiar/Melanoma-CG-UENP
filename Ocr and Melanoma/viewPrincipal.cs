using AForge.Imaging;
using AForge.Imaging.Filters;
using Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Orc_and_Melanoma
{
    public partial class viewPrincipal : Form
    {
        public viewPrincipal()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = GetImageFilter();
            var res = openFileDialog.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            var fn = openFileDialog.FileName;
            var img = AForge.Imaging.Image.FromFile(fn);
            pictureBox.Image = img;
            pictureBox.Refresh();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = GetImageFilter();
            var res = saveFileDialog.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            var fn = saveFileDialog.FileName;

            var img = pictureBox.Image;
            img.Save(fn);
        }

        private void melanomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var img = (Bitmap)pictureBox.Image;
            var melanoma = new Melanoma().ProcessMelanoma(img);
            pictureBox.Image = melanoma;
            pictureBox.Refresh();
        }

        private string GetImageFilter()
        {
            StringBuilder allImageExtensions = new StringBuilder();
            string separator = "";
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            Dictionary<string, string> images = new Dictionary<string, string>();
            foreach (ImageCodecInfo codec in codecs)
            {
                allImageExtensions.Append(separator);
                allImageExtensions.Append(codec.FilenameExtension);
                separator = ";";
                images.Add(string.Format("{0} Files: ({1})", codec.FormatDescription, codec.FilenameExtension),
                           codec.FilenameExtension);
            }
            StringBuilder sb = new StringBuilder();
            if (allImageExtensions.Length > 0)
            {
                sb.AppendFormat("{0}|{1}", "All Images", allImageExtensions.ToString());
            }
            foreach (KeyValuePair<string, string> image in images)
            {
                sb.AppendFormat("|{0}|{1}", image.Key, image.Value);
            }
            return sb.ToString();
        }

        private void ocrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var img = (Bitmap)pictureBox.Image;
            //var title = new Ocr().ProcessOcrTitle(img,1);
            var texto = new Ocr().ProcessOcrTexto(img, 10);
            pictureBox.Image = texto;
            pictureBox.Refresh();

            BlobCounter bc = new BlobCounter();
            Invert inv = new Invert();

            var t = inv.Apply(texto);

            bc.ProcessImage(t);

            Rectangle[] r = bc.GetObjectsRectangles();

            List<string> txt = new List<string>();

            foreach(var rec in r)
            {
                var part = new Crop(rec).Apply(img);
                var part1 = new Median().Apply(part);
                txt.Add(new Ocr().processTess(part1));
            }

            /*var txtTitle = new Ocr().processTess(texto) + "\n\n";*/

            string str = "";
           foreach(var txtt in txt)
            {
                str += txtt;
            }


            MessageBox.Show(str, txt[0],
            MessageBoxButtons.OK);

        }
      


    }
}
