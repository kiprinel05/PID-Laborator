using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace Algorithms.Sections
{
    public class GeometricTransformations
    {

        public static double LiniarInterpolation(double x, double f0, double f1)
        {
            double fracX = x - Math.Floor(x);

            return fracX * f1 + (1 - fracX) * f0;
        }

        public static double BilinearInterpolation(double x, double y, int[] f)
        {
            double p0 = LiniarInterpolation(x, f[0], f[1]);
            double p1 = LiniarInterpolation(x, f[2], f[3]);
            double result = LiniarInterpolation(y, p0, p1);
            return result;
        }

        public static Image<Gray, byte> BiliniarScale(Image<Gray, byte> inputImage, double scaleX, double scaleY)
        {
            int newWidth = (int)(inputImage.Width * scaleX);
            int newHeight = (int)(inputImage.Height * scaleY);
            Image<Gray, byte> result = new Image<Gray, byte>(newWidth, newHeight);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    double Xc = x / scaleX;
                    double Yc = y / scaleY;
                    int x0 = (int)Math.Floor(Xc);
                    int y0 = (int)Math.Floor(Yc);
                    int[] f = new int[4];

                    if (x0 >= 0 && x0 < inputImage.Width - 1 && y0 >= 0 && y0 < inputImage.Height - 1)
                    {
                        for (int i = 0; i < 1; i++)
                        {

                            for (int j = 0; j < 1; j++)
                            {
                                f[2 * i + j] = inputImage.Data[y0 + i, x0 + j, 0];
                            }
                        }
                        double resultValue = BilinearInterpolation(Xc, Yc, f);
                        result.Data[y, x, 0] = (byte)(resultValue + .5);
                    }
                }
            }
            return result;
        }

        public static Image<Bgr, byte> BiliniarScale(Image<Bgr, byte> inputImage, double scaleX, double scaleY)
        {
            int newWidth = (int)(inputImage.Width * scaleX);
            int newHeight = (int)(inputImage.Height * scaleY);
            Image<Bgr, byte> result = new Image<Bgr, byte>(newWidth, newHeight);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    double Xc = x / scaleX;
                    double Yc = y / scaleY;
                    int x0 = (int)Math.Floor(Xc);
                    int y0 = (int)Math.Floor(Yc);
                    int[] f = new int[4];
                    if (x0 >= 0 && x0 < inputImage.Width - 1 && y0 >= 0 && y0 < inputImage.Height - 1)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    f[2 * i + j] = inputImage.Data[y0 + i, x0 + j, c];
                                }
                            }
                            double resultValue = BilinearInterpolation(Xc - x0, Yc - y0, f);
                            result.Data[y, x, c] = (byte)(resultValue + .5);
                        }
                    }
                }
            }
            return result;
        }

        public static double cubicInterpolation(double x, double f0, double f1, double f2, double f3)
        {
            double fracX = x - Math.Floor(x);

            double xx = fracX * fracX;
            double xxx = xx * fracX;

            double result = 0.5 * ((-xxx + 2 * xx - fracX) * f0 + (3 * xxx - 5 * xx + 2) * f1 + (-3 * xxx + 4 * xx + fracX) * f2 + (xxx - xx) * f3);

            return result;

        }

        public static double BicubicInterpolation(double x, double y, double[,] f)
        {
            double p0 = cubicInterpolation(x, f[0, 0], f[0, 1], f[0, 2], f[0, 3]);
            double p1 = cubicInterpolation(x, f[1, 0], f[1, 1], f[1, 2], f[1, 3]);
            double p2 = cubicInterpolation(x, f[2, 0], f[2, 1], f[2, 2], f[2, 3]);
            double p3 = cubicInterpolation(x, f[3, 0], f[3, 1], f[3, 2], f[3, 3]);
            double result = cubicInterpolation(y, p0, p1, p2, p3);
            return result;
        }

        public static Image<Gray, byte> BicubicScale(Image<Gray, byte> inputImage, double scaleX, double scaleY)
        {
            int newWidth = (int)(inputImage.Width * scaleX);
            int newHeight = (int)(inputImage.Height * scaleY);
            Image<Gray, byte> result = new Image<Gray, byte>(newWidth, newHeight);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    double Xc = x / scaleX;
                    double Yc = y / scaleY;
                    int x0 = (int)Math.Floor(Xc);
                    int y0 = (int)Math.Floor(Yc);
                    double[,] f = new double[4, 4];
                    if (x0 >= 1 && x0 < inputImage.Width - 2 && y0 >= 1 && y0 < inputImage.Height - 2)
                    {
                        for (int i = -1; i <= 2; i++)
                        {
                            for (int j = -1; j <= 2; j++)
                            {
                                f[i + 1, j + 1] = inputImage.Data[y0 + i, x0 + j, 0];
                            }
                        }
                        double resultValue = BicubicInterpolation(Xc, Yc, f);
                        result.Data[y, x, 0] = (byte)(Math.Max(0, Math.Min(255, resultValue + .5)));
                    }
                }
            }
            return result;
        }

        public static Image<Bgr, byte> BicubicScale(Image<Bgr, byte> inputImage, double scaleX, double scaleY)
        {
            int newWidth = (int)(inputImage.Width * scaleX);
            int newHeight = (int)(inputImage.Height * scaleY);
            Image<Bgr, byte> result = new Image<Bgr, byte>(newWidth, newHeight);
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    double Xc = x / scaleX;
                    double Yc = y / scaleY;
                    int x0 = (int)Math.Floor(Xc);
                    int y0 = (int)Math.Floor(Yc);
                    double[,] f = new double[4, 4];
                    if (x0 >= 1 && x0 < inputImage.Width - 2 && y0 >= 1 && y0 < inputImage.Height - 2)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            for (int i = -1; i <= 2; i++)
                            {
                                for (int j = -1; j <= 2; j++)
                                {
                                    f[i + 1, j + 1] = inputImage.Data[y0 + i, x0 + j, c];
                                }
                            }
                            double resultValue = BicubicInterpolation(Xc - x0, Yc - y0, f);
                            result.Data[y, x, c] = (byte)(Math.Max(0, Math.Min(255, resultValue + .5)));
                        }
                    }
                }
            }
            return result;
        }
    }
}