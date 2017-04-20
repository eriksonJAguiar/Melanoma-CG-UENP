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

 
            var img2 = grayScale(imagem);
            var img3 = binary(img2);
            var img4 = openingFilter(img3);
            var img5 = noiseFilter(img4);

            return img5;

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
            filtro.ApplyInPlace(imagem, new Rectangle(0,0,7,7));

            return imagem;
        }
        public Bitmap noiseFilter(Bitmap img)
        {
            Bitmap imagem = (Bitmap)img.Clone();

            Median filter = new Median();
            var noise = filter.Apply(imagem);
            return noise;
        }
    }
}
