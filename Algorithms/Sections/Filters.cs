using Emgu.CV;
using Emgu.CV.Structure;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.Remoting.Channels;

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

        private static double[,] GaussMask(double sigmaX, double sigmaY)
        {
            int w = Math.Ceiling(4 * sigmaX) % 2 == 0 ? (int)(4 * sigmaX) + 1 : (int)(4 * sigmaX);
            int h = Math.Ceiling(4 * sigmaY) % 2 == 0 ? (int)(4 * sigmaY) + 1 : (int)(4 * sigmaY);

            double[,] gaussMask = new double[w, h];

            for (int x = -w / 2; x <= w / 2; x++)
            {
                for (int y = -h / 2; y <= h / 2; y++)
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

        #region Prewitt Filter 

        public static Image<Gray, byte> PrewittFilter(Image<Gray, byte> initialImage, int T)
        {
            double[,] prewittX = new double[,]
            {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 }
            };
            double[,] prewittY = new double[,]
            {
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };

            Image<Gray, byte> gradientX = ApplyFilter(initialImage, prewittX);
            Image<Gray, byte> gradientY = ApplyFilter(initialImage, prewittX);
            Image<Gray, byte> resultImage = new Image<Gray, byte>(initialImage.Width, initialImage.Height);
            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    double n_grad = Math.Sqrt(
                        gradientX.Data[y, x, 0] * gradientX.Data[y, x, 0] +
                        gradientY.Data[y, x, 0] * gradientY.Data[y, x, 0]);
                    resultImage.Data[y, x, 0] = (byte)(n_grad <= T ? 0 : 255);
                }
            }
            return resultImage;
        }

        public static Image<Bgr, byte> PrewittFilter(Image<Bgr, byte> initialImage, double T)
        {
            double[,] prewittX = new double[,]
            {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 }
            };
            double[,] prewittY = new double[,]
            {
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };


            Image<Gray, byte> Rx = ApplyFilter(initialImage.Split()[0], prewittX);
            Image<Gray, byte> Gx = ApplyFilter(initialImage.Split()[1], prewittX);
            Image<Gray, byte> Bx = ApplyFilter(initialImage.Split()[2], prewittX);

            Image<Gray, byte> Ry = ApplyFilter(initialImage.Split()[0], prewittY);
            Image<Gray, byte> Gy = ApplyFilter(initialImage.Split()[1], prewittY);
            Image<Gray, byte> By = ApplyFilter(initialImage.Split()[2], prewittY);


            Image<Bgr, byte> resultImage = new Image<Bgr, byte>(initialImage.Width, initialImage.Height);

            for (int y = 0; y < initialImage.Height; y++)
            {
                for (int x = 0; x < initialImage.Width; x++)
                {
                    double r_grad = Math.Sqrt(Rx.Data[y, x, 0] * Rx.Data[y, x, 0] + Ry.Data[y, x, 0] * Ry.Data[y, x, 0]);
                    double g_grad = Math.Sqrt(Gx.Data[y, x, 0] * Gx.Data[y, x, 0] + Gy.Data[y, x, 0] * Gy.Data[y, x, 0]);
                    double b_grad = Math.Sqrt(Bx.Data[y, x, 0] * Bx.Data[y, x, 0] + By.Data[y, x, 0] * By.Data[y, x, 0]);

                    double sum = r_grad + g_grad * b_grad;

                    if (sum <= T)
                    {
                        resultImage.Data[y, x, 0] = 0;
                        resultImage.Data[y, x, 1] = 0;
                        resultImage.Data[y, x, 2] = 0;
                    }
                    else
                    {
                        if (r_grad > 255) r_grad = 255; if(r_grad < 0) r_grad = 0;
                        if (g_grad > 255) g_grad = 255; if(g_grad < 0) g_grad = 0;
                        if (b_grad > 255) b_grad = 255; if(b_grad < 0) b_grad = 0;

                        resultImage.Data[y, x, 0] = (byte)r_grad;
                        resultImage.Data[y, x, 1] = (byte)g_grad;
                        resultImage.Data[y, x, 2] = (byte)b_grad;
                    }
                }
            }
            return resultImage.Convert<Bgr, byte>();
        }

        #endregion
    }
}