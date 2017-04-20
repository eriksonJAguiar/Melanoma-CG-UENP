using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Filters
{
    class Ocr
    {
        public Bitmap ProcessOcr(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var img1 = grayScale(imagem);
            var img2 = treshold(img1);
            var img3 = erosion(img2);
            var img4 = dilata(img3);
            var img5 = erosion(img4);
            Console.WriteLine(contagem(img5));

            return img5;

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

        public Bitmap erosion(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Erosion3x3 e = new Erosion3x3();
            var binary = e.Apply(imagem);

            for (int i = 0; i < 5; i++)
            {
                binary = e.Apply(binary);
            }

            return binary;
        }

        public Bitmap dilata(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Dilatation3x3 e = new Dilatation3x3();
            var binary = e.Apply(imagem);

            for (int i = 0; i < 8; i++)
            {
                binary = e.Apply(binary);
            }

            return binary;
        }

        public int contagem(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();


            BlobCounter bb = new BlobCounter();

            bb.ProcessImage(img);
            
            int x = bb.ObjectsCount; 
            //colocar funlção de contagem de blocos

            return x;
        }

    }
}
