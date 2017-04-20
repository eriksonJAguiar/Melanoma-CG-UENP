using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class Melanoma
    {
        public Bitmap ProcessMelanoma(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            //var img1 = hslFiltering(imagem);
            var img2 = grayScale(imagem);
            var img3 = binary(img2);
            var img4 = openingFilter(img3);
            var img5 = noiseFilter(img4);

            return img5;

        }
        public int contBlack(Bitmap img)
        {

            var imagem = (Bitmap)img.Clone();
            int black = 0;

            int j = imagem.Width / 2;
            for (int i = 0; i < imagem.Height; i++)
            {
                Color c = imagem.GetPixel(i, j);

                if (c.R == 0 && c.G == 0 && c.B == 0)
                        black++;
            }

            return black;
        }
        public Bitmap grayScale(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            var gray =  Grayscale.CommonAlgorithms.Y.Apply(imagem);
           
            return gray;
        }
        public Bitmap binary(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();
            Threshold t = new Threshold();
            var binary =  t.Apply(imagem);

            return binary;
        }
        public Bitmap openingFilter(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            Opening filtro = new Opening();
            filtro.ApplyInPlace(imagem, new Rectangle(0,0,imagem.Width,imagem.Height));

            return imagem;
        }
        public Bitmap noiseFilter(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            AdditiveNoise addN = new AdditiveNoise();
            var noise = addN.Apply(imagem);

            return noise;
        }
    }
}
