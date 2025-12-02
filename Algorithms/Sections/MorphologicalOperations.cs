using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System;
using System.Collections.Generic;


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

        public static Image<Bgr, byte> ConnectedComponents(Image<Bgr, byte> inputImage)
        {
            Image<Bgr, byte> binary = Tools.Tools.MinErrorThresholding(inputImage);

            Image<Bgr, byte> result = new Image<Bgr, byte>(inputImage.Width, inputImage.Height);

            bool[,] visited = new bool[inputImage.Height, inputImage.Width];
            int label = 1;
            Random rand = new Random();

            for(int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    if (binary.Data[y, x, 0] == 255 && !visited[y, x])
                    {
                        Tuple<int, int> point = new Tuple<int, int>(x, y);
                        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
                        queue.Enqueue(point);
                        visited[y, x] = true;
                        Bgr color = new Bgr(rand.Next(256), rand.Next(256), rand.Next(256));
                        while (queue.Count > 0)
                        {
                            Tuple<int, int> current = queue.Dequeue();
                            result.Data[current.Item2, current.Item1, 0] = (byte)color.Blue;
                            result.Data[current.Item2, current.Item1, 1] = (byte)color.Green;
                            result.Data[current.Item2, current.Item1, 2] = (byte)color.Red;
                            for(int i = -1; i <= 1; i++)
                            {
                                for(int j = -1; j <= 1; j++)
                                {
                                    int newX = current.Item1 + j;
                                    int newY = current.Item2 + i;
                                    if (newX >= 0 && newX < inputImage.Width && newY >= 0 && newY < inputImage.Height)
                                    {
                                        if (binary.Data[newY, newX, 0] == 255 && !visited[newY, newX])
                                        {
                                            Tuple<int, int> newPoint = new Tuple<int, int>(newX, newY);
                                            queue.Enqueue(newPoint);
                                            visited[newY, newX] = true;
                                        }
                                    }
                                }
                            }
                            
                        }
                        label++;
                    }
                }
            }
            return result;
        }
    }
}