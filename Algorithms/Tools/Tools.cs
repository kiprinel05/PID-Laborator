using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Security.Cryptography;

namespace Algorithms.Tools
{
    public class Tools
    {
        #region Copy
        public static Image<Gray, byte> Copy(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = inputImage.Clone();
            return result;
        }

        public static Image<Bgr, byte> Copy(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = inputImage.Clone();
            return result;
        }
        #endregion

        #region Invert
        public static Image<Gray, byte> Invert(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (byte)(255 - inputImage.Data[y, x, 0]);
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Invert(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (byte)(255 - inputImage.Data[y, x, 0]);
                    result.Data[y, x, 1] = (byte)(255 - inputImage.Data[y, x, 1]);
                    result.Data[y, x, 2] = (byte)(255 - inputImage.Data[y, x, 2]);
                }
            }
            return result;
        }
        #endregion

        #region Convert color image to grayscale image
        public static Image<Gray, byte> Convert(Image<Bgr, byte> inputImage)
        {
            Image<Gray, byte> result = inputImage.Convert<Gray, byte>();
            return result;
        }
        #endregion

        #region Binar

        public static Image<Gray, byte> Binar(Image<Gray, byte> inputImage, byte threshold)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = (inputImage.Data[y, x, 0] >= threshold) ? (byte)255 : (byte)0;
                }
            }
            return result;
        }

        #endregion


        #region Mirror

        public static Image<Gray, byte> Mirror(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = inputImage.Data[y, inputImage.Width - 1 - x, 0];
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Mirror(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[y, x, 0] = inputImage.Data[y, inputImage.Width - 1 - x, 0];
                    result.Data[y, x, 1] = inputImage.Data[y, inputImage.Width - 1 - x, 1];
                    result.Data[y, x, 2] = inputImage.Data[y, inputImage.Width - 1 - x, 2];
                }
            }
            return result;
        }

        #endregion

        #region Rotate 90 degrees clockwise

        public static Image<Gray, byte> Rotate90Clockwise(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Height, inputImage.Width);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[x, inputImage.Height - 1 - y, 0] = inputImage.Data[y, x, 0];
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Rotate90Clockwise(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Height, inputImage.Width);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[x, inputImage.Height - 1 - y, 0] = inputImage.Data[y, x, 0];
                    result.Data[x, inputImage.Height - 1 - y, 1] = inputImage.Data[y, x, 1];
                    result.Data[x, inputImage.Height - 1 - y, 2] = inputImage.Data[y, x, 2];
                }
            }
            return result;
        }

        #endregion

        #region Rotate 90 degrees trigonometric

        public static Image<Gray, byte> Rotate90Trigonometric(Image<Gray, byte> inputImage)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Height, inputImage.Width);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[inputImage.Width - 1 - x, y, 0] = inputImage.Data[y, x, 0];
                }
            }
            return result;
        }

        public static Image<Bgr, byte> Rotate90Trigonometric(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Height, inputImage.Width);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    result.Data[inputImage.Width - 1 - x, y, 0] = inputImage.Data[y, x, 0];
                    result.Data[inputImage.Width - 1 - x, y, 1] = inputImage.Data[y, x, 1];
                    result.Data[inputImage.Width - 1 - x, y, 2] = inputImage.Data[y, x, 2];
                }
            }
            return result;
        }


        #endregion

        #region Contrast and Brightness Adjustment

        public static Image<Gray, byte> AdjustContrastBrightness(Image<Gray, byte> inputImage, double alpha, int beta)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    int newValue = (int)(alpha * inputImage.Data[y, x, 0] + beta);
                    result.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, newValue));
                }
            }
            return result;
        }

        public static Image<Bgr, byte> AdjustContrastBrightness(Image<Bgr, byte> inputImage, double alpha, int beta)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    for (int c = 0; c < 3; ++c)
                    {
                        int newValue = (int)(alpha * inputImage.Data[y, x, c] + beta);
                        result.Data[y, x, c] = (byte)Math.Max(0, Math.Min(255, newValue));
                    }
                }
            }
            return result;
        }

        #endregion

        #region Gamma operator

        public static Image<Gray, byte> Gamma(Image<Gray, byte> inputImage, double gamma)
        {
            if (gamma < 0) gamma = 1;

            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            double factor = Math.Pow(255.0, 1.0 - gamma);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    int newValue = (int)(factor * Math.Pow(inputImage.Data[y, x, 0], gamma));
                    result.Data[y, x, 0] = (byte)Math.Max(0, Math.Min(255, newValue));
                }
            }

            return result;
        }


        public static Image<Bgr, byte> Gamma(Image<Bgr, byte> inputImage, double gamma)
        {
            if (gamma < 0) gamma = 1;

            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Size);

            double factor = Math.Pow(255.0, 1.0 - gamma);

            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    for (int c = 0; c < 3; ++c)
                    {
                        int newValue = (int)(factor * Math.Pow(inputImage.Data[y, x, c], gamma));
                        result.Data[y, x, c] = (byte)Math.Max(0, Math.Min(255, newValue));
                    }
                }
            }

            return result;
        }

        #endregion


        #region Normalized Histogram

        public static float[] NormalizedHistogram(Image<Gray, byte> inputImage)
        {
            float[] histogram = new float[256];
            for (int y = 0; y < inputImage.Height; ++y)
            {
                for (int x = 0; x < inputImage.Width; ++x)
                {
                    byte pixelValue = inputImage.Data[y, x, 0];
                    histogram[pixelValue]++;
                }
            }
            return histogram;
        }


        #endregion

        #region Min Error Thresholding

        public static Image<Gray, byte> MinErrorThresholding(Image<Gray, byte> inputImage)
        {
            float[] histogram = NormalizedHistogram(inputImage);
            double n = inputImage.Width * inputImage.Height;

            float[] p = new float[256];
            for (byte k = 1; k < 255; k++)
            {
                p[k] = (float)(histogram[k] / n);
            }

            double minError = 10000;
            byte optimalThreshold = 0;

            for (byte t = 1; t < 255; t++)
            {
                double p1 = 0, p2 = 0;
                double mu1 = 0, mu2 = 0;
                double sigma1 = 0, sigma2 = 0;
                for (byte k = 0; k <= t; k++)
                {
                    p1 += p[k];
                    mu1 += k * p[k];
                }
                if (p1 > 0) mu1 /= p1; else mu1 = 0;

                p2 = 1 - p1;
                for(byte k = (byte)(t + 1); k < 255; k++)
                {
                    mu2 += k * p[k];
                }
                if (p2 > 0) mu2 /= p2; else mu2 = 0;
                for (byte k = 0; k <= t; k++)
                {
                    sigma1 += ((k - mu1) * (k - mu1)) * p[k];
                }
                for (byte k = (byte)(t + 1); k < 255; k++)
                {
                    sigma2 += ((k - mu2) * (k - mu2)) * p[k];
                }
                if (p1 > 0) sigma1 /= p1; else sigma1 = 0;
                if (p2 > 0) sigma2 /= p2; else sigma2 = 0;


                double t1 = 0, t2 = 0, t3 = 0, t4 = 0;
                if(sigma1 <= 0) t1 = 0; else t1 = p1 * Math.Log(sigma1);
                if (sigma2 <= 0) t2 = 0; else t2 = p2 * Math.Log(sigma2);
                if (p1 <= 0) t3 = 0; else t3 = 2 * p1 * Math.Log(p1);
                if (p2 <= 0) t4 = 0; else t4 = 2 * p2 * Math.Log(p2);

                double error = 1 + t1 + t2 - t3 - t4;

                if (error < minError)
                {
                    minError = error;
                    optimalThreshold = t;
                }
            }

            return Binar(inputImage, optimalThreshold);
        }

        #endregion

    }
}