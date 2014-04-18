using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace ImageToChar
{
    public static class Image2Char
    {
        /// <summary>
        /// 实现图像到字符的转化，通过计算一个区域内亮度平均值实现，参考自@soso_fy http://my.oschina.net/sosofy/blog/109259
        /// </summary>
        /// <param name="imagepath">图像文件的路径</param>
        /// <param name="rowsize">列取样数</param>
        /// <param name="colsize">行取样数</param>
        /// <returns>字符画</returns>
        public static string ImageToChar(string imagepath,int rowsize,double desiredFrameRate)
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', '0', 'V', '1', 'i', ':', '*', '|', '.', ' ' };
            Bitmap image = new Bitmap(imagepath);
            int pixelH = image.Height;
            int pixelW = image.Width;
            int colsize = (int)(rowsize * ((float)pixelW/ (float)pixelH));
            Console.WindowHeight = rowsize;
            Console.WindowWidth = colsize;
            Console.BufferHeight = rowsize;
            Console.BufferWidth = colsize;
            using (StreamWriter config=new StreamWriter(@"D:\Processed Text\Config.txt"))
            {
                config.WriteLine(rowsize+2);
                config.WriteLine(colsize+2);
                config.WriteLine(desiredFrameRate);
            }
            int spaceH = pixelH / rowsize;
            int spaceW = pixelW / colsize;
            for(int rownum=0;rownum<rowsize;rownum++)
            {
                for(int colnum=0;colnum<colsize;colnum++)
                {
                    int cpixelH = spaceH * colnum;
                    int cpixelW = spaceW * rownum;
                    float averBright=0;
                    for(int offsetH=0;offsetH<spaceH;offsetH++) //遍历获得平均亮度值
                    {
                        for(int offsetW=0;offsetW<spaceW;offsetW++)
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
            return result.ToString();
        }

        public static void WriteToFile(string result,int count)
        {
            string root = @"D:\Processed Text\";
            using (StreamWriter outfile = new StreamWriter(root + count.ToString() + @".txt"))
            {
                outfile.Write(result);
            }
        }
    }
}
