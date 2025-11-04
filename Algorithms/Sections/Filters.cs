using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace Algorithms.Sections
{
    public class Filters
    {
        public static Image<Gray, byte> ExpandImage(Image<Gray, byte> inputImage, int width, int height)
        {
            Image<Gray, byte> expandedIamge = new Image<Gray, byte>(inputImage.Width + width * 2, inputImage.Height + height * 2);

            for (int y = 0; y < inputImage.Height + height * 2; y++)
            {
                for (int x = 0; x < inputImage.Width + width * 2; x++)
                {
                    int coord_y = Math.Min(inputImage.Height - 1, Math.Max(0, y - height));
                    int coord_x = Math.Min(inputImage.Width - 1, Math.Max(0, x - width));
                    expandedIamge.Data[y, x, 0] = inputImage.Data[coord_y, coord_x, 0];
                }
            }
            return expandedIamge;
        }

        public static Image<Gray, byte> ApplyFilter(Image<Gray, byte> initialImage, double[,] filter)
        {
            int half_h = filter.GetLength(0) / 2;
            int half_w = filter.GetLength(1) / 2;

            Image<Gray, byte> resultImage = new Image<Gray, byte>(initialImage.Width, initialImage.Height);
            Image<Gray, byte> temp = ExpandImage(initialImage, half_w, half_h);

            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    double kernelValue = 0.0;

                    for (int offY = -half_h; offY <= half_h; offY++)
                    {
                        for (int offX = -half_w; offX <= half_w; offX++)
                        {
                            kernelValue += filter[offY + half_h, offX + half_w] *
                                           temp.Data[y + offY + half_h, x + offX + half_w, 0];
                        }
                    }

                    resultImage.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, (int)kernelValue));
                }
            }

            return resultImage;
        }

        #region Low Pass Filters 

        private  static double[,] GaussMask(double sigmaX, double sigmaY)
        {
            int w = Math.Ceiling(4 * sigmaX) % 2 == 0 ? (int)(4 * sigmaX) + 1 : (int)(4 * sigmaX);
            int h = Math.Ceiling(4 * sigmaY) % 2 == 0 ? (int)(4 * sigmaY) + 1 : (int)(4 * sigmaY);

            double [,] gaussMask = new double[w, h];

            for (int x = -w/2; x <= w / 2; x++)
            {
                for(int y = -h / 2; y <= h / 2; y++)
                {
                    double gauss = (1 / (2 * Math.PI * sigmaX * sigmaY)) * 
                               Math.Exp(-(x * x / (2 * sigmaX * sigmaX) + y * y / (2 * sigmaY * sigmaY)));
                    gaussMask[x + w / 2, y + h / 2] = gauss;
                }
            }

            double sum = 0.0;
            for (int i = 0; i < gaussMask.GetLength(0); i++)
            {
                for (int j = 0; j < gaussMask.GetLength(1); j++)
                {
                    sum += gaussMask[i, j];
                }
            }
            for (int i = 0; i < gaussMask.GetLength(0); i++)
            {
                for (int j = 0; j < gaussMask.GetLength(1); j++)
                {
                    gaussMask[i, j] /= sum;
                }
            }
            return gaussMask;

        }

        public static Image<Gray, byte> GaussFilter(Image<Gray, byte> initialImage, double sigmaX, double sigmaY)
        {
            double[,] gaussMask = GaussMask(sigmaX, sigmaY);
            return ApplyFilter(initialImage, gaussMask);
        }

        public static Image<Bgr, byte> GaussFilter(Image<Bgr, byte> initialImage, double sigmaX, double sigmaY)
        {
            Image<Gray, byte>[] channels = initialImage.Split();
            for (int i = 0; i < channels.Length; i++)
            {
                channels[i] = GaussFilter(channels[i], sigmaX, sigmaY);
            }
            return new Image<Bgr, byte>(channels);
        }

        #endregion
    }
}