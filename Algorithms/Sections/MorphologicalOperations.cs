using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace Algorithms.Sections
{
    public class MorphologicalOperations
    {

        public static Image<Gray, byte> Dilate (Image<Gray, byte> inputImage, int w, int h, int T, bool white = true) { 
            Image<Gray, byte> binary = Tools.Tools.Binar(inputImage, (byte)T);
            Image<Gray, byte> expandedBinary = Filters.ExpandImage(binary, w / 2, h / 2);

            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Width, inputImage.Height);

            byte selectedValute = white ? (byte)255 : (byte)0;
            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bool flag = false;
                    for (int offY = -h / 2; offY <= h / 2; offY++)
                    {
                        for (int offX = -w / 2; offX <= w / 2; offX++)
                        {
                            if (expandedBinary.Data[y + offY + h / 2, x + offX + w / 2, 0] == selectedValute)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag) break;
                    }
                    result.Data[y, x, 0] = flag ? selectedValute : (byte)(255 - selectedValute);
                }
            }
            return result;
        }

        public static Image<Gray, byte> Erode(Image<Gray, byte> inputImage, int w, int h, int T, bool white = true)
        {
            Image<Gray, byte> binary = Tools.Tools.Binar(inputImage, (byte)T);
            Image<Gray, byte> expandedBinary = Filters.ExpandImage(binary, w / 2, h / 2);

            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Width, inputImage.Height);

            byte selectedValue = white ? (byte)255 : (byte)0;
            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bool flag = false;
                    for (int offY = -h / 2; offY <= h / 2; offY++)
                    {
                        for (int offX = -w / 2; offX <= w / 2; offX++)
                        {
                            if (expandedBinary.Data[y + offY + h / 2, x + offX + w / 2, 0] != selectedValue)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag) break;
                    }
                    result.Data[y, x, 0] = flag ? (byte)(255 - selectedValue) : selectedValue;
                }
            }
            return result;
        }

        public static Image<Gray, byte> Open(Image<Gray, byte> inputImage, int w, int h, int T, bool white = true) { 
        
            Image<Gray, byte> eroded = Erode(inputImage, w, h, T, white);
            Image<Gray, byte> opened = Dilate(eroded, w, h, T, white);
            return opened;
        }


        public static Image<Gray, byte> Close(Image<Gray, byte> inputImage, int w, int h, int T, bool white = true)
        {
            Image<Gray, byte> dilated = Dilate(inputImage, w, h, T, white);
            Image<Gray, byte> closed = Erode(dilated, w, h, T, white);
            return closed;
        }
    }
}