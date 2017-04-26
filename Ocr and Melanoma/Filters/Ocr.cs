using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Filters
{
    class Ocr
    {
        public Bitmap ProcessOcrTexto(Bitmap img,int n)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var img1 = grayScale(imagem);
            var img2 = treshold(img1);
            var img3 = erosion(img2,n);
            var img4 = mediana(img3);

            BlobsFiltering f = new BlobsFiltering();

            var img5 = f.Apply(img4);

            return img5;

        }
        public Bitmap ProcessOcrTitle(Bitmap img, int n)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var img1 = grayScale(imagem);
            var img2 = treshold(img1);
            var img3 = dilata(img2,n);
            var img4 = mediana(img3);

            return img4;

        }
        public Bitmap grayScale(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var gray = Grayscale.CommonAlgorithms.Y.Apply(imagem);

            return gray;
        }

        public Bitmap treshold(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Threshold t = new Threshold();
            var binary = t.Apply(imagem);

            return binary;
        }

        public Bitmap erosion(Bitmap img,int n)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Erosion e = new Erosion();
            var binary = e.Apply(imagem);

            for (int i = 1; i < n; i++)
            {
                binary = e.Apply(binary);
            }

            return binary;
        }

        public Bitmap dilata(Bitmap img, int n)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Dilatation e = new Dilatation();
            var binary = e.Apply(imagem);

            for (int i = 1; i < n; i++)
            {
                binary = e.Apply(binary);
            }

            return binary;
        }
        public Bitmap mediana(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var m = new Median().Apply(imagem);

            return m;
        }
        public Bitmap inverter(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Invert t = new Invert();
            var binary = t.Apply(imagem);

            return binary;
        }
        public List<string> contagem(Bitmap img,Bitmap original, Rectangle[] rec)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            BlobCounter bc = new BlobCounter();
            // process binary image
            bc.ProcessImage(imagem);

            Rectangle[] rects = bc.GetObjectsRectangles();

            List<string> textos = new List<string>();
            
            foreach(var r in rects)
            {
                if (rec.Contains(r))
                {
                    var c = new Crop(r).Apply(original);

                   var str = processTess(c);

                    textos.Add(str);
                    
                }
                  
            }

            return textos;
        }
        public string processTess(Bitmap img)
        {
            var datapath = @"C:\Users\eriks\Documents\GitHub Projects\Ocr-and-Melanoma\Ocr and Melanoma\tessdata";
            var lang = "por";
            var imagem = img.Clone();

            var ocr = new TesseractEngine(datapath, lang, EngineMode.TesseractOnly);
            var page = ocr.Process(img);

            return page.GetText();
        }


    }
}
