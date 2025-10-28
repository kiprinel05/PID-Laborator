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
    }
    }