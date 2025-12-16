using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithms.Sections
{
    public class Segmentation
    {

        public static Image<Bgr, byte> KMeans(Image<Bgr, byte> inputImage, int k)
        {
            Image<Bgr, byte> resultImage = new Image<Bgr, byte>(inputImage.Width, inputImage.Height);

            float[] normalisedHistogram = Tools.Tools.NormalizedHistogram(inputImage);

            double[] centers = new double[k + 1];

            double k_pk, pk;

            for (int i = k; i > 0; i--)
            {
                centers[i] = normalisedHistogram.Length / i;
            }

            for (int iter = 0; iter < 10; iter++)
            {

                for (int l = k; l > 0; l--)
                {
                    pk = 0;
                    for (double j = centers[(l - 1) / 2] + centers[l / 2]; j < centers[l / 2] + centers[(l + 1) / 2]; j++)
                    {
                        pk += normalisedHistogram[(int)j];
                    }

                    k_pk = 0;
                    for (double i = centers[(l - 1) / 2] + centers[l / 2]; i < centers[l / 2] + centers[(l + 1) / 2]; i++)
                    {
                        k_pk = i * normalisedHistogram[(int)i];
                    }
                    double miu = k_pk / pk;
                    centers[l] = miu;
                }
            }

            for(int i = 0; i < inputImage.Height; i++)
            {
                for (int j = 0; j < inputImage.Width; j++)
                {
                 
                    // se seteaza culoarea aia nenorocita

                }
            }


            /// MA DAU BATUT, am invaratit programul asta pana nu am mai inteles nimic si m am enervat !!

            return resultImage;
        }

    }
}