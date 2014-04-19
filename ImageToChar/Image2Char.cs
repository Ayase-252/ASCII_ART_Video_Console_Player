using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace ImageToChar
{
    public class Image2Char
    {
        private StreamWriter processedText;
        private string root;
        private int rowsize;
        private int colsize;
        private double desiredFrameRate;
        private string prefix;
        private int count;
        private int pixelH;
        private int pixelW;
        private int spaceH;
        private int spaceW;

        /// <summary>
        /// 实现图像到字符的转化，通过计算一个区域内亮度平均值实现，参考自@soso_fy http://my.oschina.net/sosofy/blog/109259
        /// Achieve converting image to char image by calculating the average brightness over a simple area. 
        /// The function referenced a algorithm written by soso_fy. You can find it on web. http://my.oschina.net/sosofy/blog/109259
        /// </summary>
        /// <param name="imagepath">图像文件的路径/Path of image</param>
        /// <param name="rowsize">列显示数/The number of row sim</param>
        /// <param name="desiredFrameRate">预定播放帧数/The desired framerate</param>
        /// <returns>字符画/Char image</returns>
        private string imageToChar()
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', '0', 'V', '1', 'i', ':', '*', '|', '.', ' ' };
            Bitmap image = loadImage();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * colnum;
                    int cpixelW = spaceW * rownum;
                    float averBright = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++) //遍历获得平均亮度值
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelH + offsetH, cpixelW + offsetW);
                                averBright += pixel.GetBrightness();
                            }
                            catch
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (spaceH * spaceW);
                    int pickup = (int)(averBright * 10);
                    result.Append(charset[10 - pickup]);
                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        private string imageToCharWithoutCount()
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', '0', 'V', '1', 'i', ':', '*', '|', '.', ' ' };
            Bitmap image = loadImageWithoutCount();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * colnum;
                    int cpixelW = spaceW * rownum;
                    float averBright = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++) //遍历获得平均亮度值
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelH + offsetH, cpixelW + offsetW);
                                averBright += pixel.GetBrightness();
                            }
                            catch
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (spaceH * spaceW);
                    int pickup = (int)(averBright * 10);
                    result.Append(charset[10 - pickup]);
                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        private void writeConfig()
        {
            processedText.WriteLine(rowsize+2);
            processedText.WriteLine(colsize+2);
            processedText.WriteLine(desiredFrameRate);
            processedText.Flush();
        }

        private void calculateConfig()
        {
            Bitmap image = loadImage();
            pixelH = image.Height;
            pixelW = image.Width;
            colsize = (int)(rowsize * ((float)pixelW / (float)pixelH));
            spaceH = pixelH / rowsize;
            spaceW = pixelW / colsize;
        }

        private void calculateConfigWithoutCount()
        {
            Bitmap image = loadImageWithoutCount();
            pixelH = image.Height;
            pixelW = image.Width;
            colsize = (int)(rowsize * ((float)pixelW / (float)pixelH));
            spaceH = pixelH / rowsize;
            spaceW = pixelW / colsize;
        }

        private Bitmap loadImage()
        {
            Bitmap loaded = new Bitmap(root + prefix + count.ToString("D4") + ".bmp");
            return loaded;
        }

        private Bitmap loadImageWithoutCount()
        {
            Bitmap loaded = new Bitmap(root);
            return loaded;
        }

        public Image2Char(string rootdictionary,int rownum,double desiredframerate,string pprefix,string savefilePath)
        {
            processedText = new StreamWriter(savefilePath);
            rowsize = rownum;
            desiredFrameRate = desiredframerate;
            prefix = pprefix;
            root = rootdictionary;
            count=0;
        }

        public void Convert()
        {
            Console.WriteLine("Configuring the parameter");
            calculateConfig();
            writeConfig();
            try
            {
                while(true)
                {
                    Console.WriteLine("Now Processing No.{0}", count);
                    imageToChar();
                }
            }
            catch
            {
                processedText.Close();
                Console.WriteLine("Finished Processing");
                return;
            }
        }

        public void ConvertSinglePicture()
        {
            calculateConfigWithoutCount();
            Console.WindowHeight = rowsize+2;
            Console.WindowWidth = colsize + 2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.Write(imageToCharWithoutCount());
            Console.WriteLine("Finished Processing");
        }
    }
}
