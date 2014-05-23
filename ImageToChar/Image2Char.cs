using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Player;

namespace ImageToChar
{
    /// <summary>
    /// Class library to implement the conversion of bitmap into ASCII art.
    /// </summary>
    public class Image2Char
    {
        #region data members
        /* Defination of Data Members
         * Last Update: 2014/4/21 Initail comment
         * Version Number: 1.0.0.0
         * */
        private StreamWriter processedText; //The stream writer of processed text file.
        private string root;
        private string prefix;
        private int count; 
        //In continuous conversion mode, the path of bitmap are organized as the format
        //root(D:\OneFile\)+prefix(OneVideo)+count(0000)+".bmp"
        //In single conversion mode, the root will be the full path of bitmap
        //root(D:\OneFile\OnePicture.bmp)
        //Root and prefix are required to input, and count is under management of this class.

        private int rowsize; //The amount of row
        private int colsize; //The amount of column
        //For keep the ascpect ratio of bitmap, the colsize will be calculated automatically
        //rowsize is required to input.

        private double desiredFrameRate; //The desired frame rate you want to play at
        //desired frame rate is required to input in continuous conversion mode
        private int pixelH; //The amount of pixel of bitmap in y
        private int pixelW; //The amount of pixel of bitmap in x
        private int spaceH; //The amount of pixel of a simple block in y
        private int spaceW; //The amount of pixel of a simple block in y

        /* Defination of Data Members ended here */
        #endregion

        #region private functions
        /// <summary>
        /// Achieve converting image to char image by calculating the average brightness over a simple area. Used in continuous mode.
        /// The function referenced a algorithm written by soso_fy. You can find it on web. http://my.oschina.net/sosofy/blog/109259
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>ASCII Art</returns>
        private string imageToChar()
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', 'H','0','T','V','+','1', 'i', ':', '*', '|', '.', ' ' };
            Bitmap image = loadImage();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * rownum;
                    int cpixelW = spaceW * colnum;
                    float averBright = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++) //Traversal all pixels in simple block to get average brightness
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelW + offsetW, cpixelH + offsetH);
                                averBright += pixel.GetBrightness();
                            }
                            catch
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (spaceH * spaceW);
                    int pickup = (int)(averBright * (charset.Length-1));
                    result.Append(charset[charset.Length -1 - pickup]);
                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        /// <summary>
        /// Achieve converting image to char image by calculating the average of RGB of an area,and convert the color to the closest ConsoleColor. Used in continuous mode.
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>ASCII Art</returns>
        private string imageToChar_Colored()
        {
            StringBuilder result = new StringBuilder();
            Bitmap image = loadImage();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * rownum;
                    int cpixelW = spaceW * colnum;
                    int averR = 0;
                    int averG = 0;
                    int averB = 0;
                    int _count = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++)
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelW + offsetW, cpixelH + offsetH);
                                averR += pixel.R;
                                averG += pixel.G;
                                averB += pixel.B;
                                _count++;
                            }
                            catch
                            {
                                
                            }
                        }
                    }
                    if(_count==spaceH*spaceW)
                    {
                        _count = spaceH * spaceW;
                    }
                    averR /= _count;
                    averG /= _count;
                    averB /= _count;
                    result.Append(getClosestConsoleColor(Convert.ToByte(averR),Convert.ToByte(averG),Convert.ToByte(averB)).ToString("X"));
                    
                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        /// <summary>
        /// Achieve converting image to char image by calculating the average brightness over a simple area. Used in single mode.
        /// The function referenced a algorithm written by soso_fy. You can find it on web. http://my.oschina.net/sosofy/blog/109259
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>ASCII Art</returns>
        private string imageToCharWithoutCount()
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { 'M', '8', 'H', '0', 'T', 'V', '+', '1', 'i', ':', '*', '|', '.', ' ' };
            Bitmap image = loadImageWithoutCount();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * rownum;
                    int cpixelW = spaceW * colnum;
                    float averBright = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++)
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelW + offsetW, cpixelH + offsetH);
                                averBright += pixel.GetBrightness();
                            }
                            catch
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (spaceH * spaceW);
                    int pickup = (int)(averBright * (charset.Length - 1));
                    result.Append(charset[(charset.Length - 1) - pickup]);
                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        private string imageToChar_ColoredWithoutCount()
        {
            StringBuilder result = new StringBuilder();
            Bitmap image = loadImageWithoutCount();
            for (int rownum = 0; rownum < rowsize; rownum++)
            {
                for (int colnum = 0; colnum < colsize; colnum++)
                {
                    int cpixelH = spaceH * rownum;
                    int cpixelW = spaceW * colnum;
                    int averR = 0;
                    int averG = 0;
                    int averB = 0;
                    int _count = 0;
                    for (int offsetH = 0; offsetH < spaceH; offsetH++)
                    {
                        for (int offsetW = 0; offsetW < spaceW; offsetW++)
                        {
                            try
                            {
                                Color pixel = image.GetPixel(cpixelW + offsetW, cpixelH + offsetH);
                                averR += pixel.R;
                                averG += pixel.G;
                                averB += pixel.B;
                                _count++;
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (_count == spaceH * spaceW)
                    {
                        _count = spaceH * spaceW;
                    }
                    averR /= _count;
                    averG /= _count;
                    averB /= _count;
                    result.Append(getClosestConsoleColor(Convert.ToByte(averR), Convert.ToByte(averG), Convert.ToByte(averB)).ToString("X"));

                }
                result.Append("\n");
            }
            processedText.Write(result);
            processedText.Flush();
            count++;
            return result.ToString();
        }

        /// <summary>
        /// Write config information in the first 3 lines of the processed text file.
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        private void writeConfig()
        {
            processedText.WriteLine(rowsize+2);
            processedText.WriteLine(colsize+2);
            processedText.WriteLine(desiredFrameRate);
            processedText.Flush();
        }

        /// <summary>
        /// Calculate the parameter of conversion, using in continuous mode
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        private void calculateConfig()
        {
            Bitmap image = loadImage();
            pixelH = image.Height;
            pixelW = image.Width;
            spaceH = pixelH / rowsize;
            spaceW = Convert.ToInt32((double)spaceH/213.0*80.0);
            colsize = pixelW / spaceW;
        }

        /// <summary>
        /// Calculate the parameter of conversion, using in single mode
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        private void calculateConfigWithoutCount()
        {
            Bitmap image = loadImageWithoutCount();
            pixelH = image.Height;
            pixelW = image.Width;
            spaceH = pixelH / rowsize;
            spaceW = spaceH*8/18;
            colsize = pixelW/spaceW;
        }

        /// <summary>
        /// Load image from disk, using in continuous mode
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>Loaded image</returns>
        private Bitmap loadImage()
        {
            Bitmap loaded = new Bitmap(root + prefix + count.ToString("D4") + ".bmp");
            return loaded;
        }

        /// <summary>
        /// Load image from disk, using in single mode
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>Loaded image</returns>
        private Bitmap loadImageWithoutCount()
        {
            Bitmap loaded = new Bitmap(root);
            return loaded;
        }

        #endregion

        #region public functions
        /// <summary>
        /// Constructor
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <param name="rootdictionary">the root dictionary of bitmaps(Continuous mode)/the full path of bitmap(Single mode)</param>
        /// <param name="rownum">the amount of row</param>
        /// <param name="desiredframerate">the desired frame rate you want to play at (In single mode, input 0)</param>
        /// <param name="pprefix">the prefix of your file name</param>
        /// <param name="savefilePath">the root dictionary of processed text file</param>
        public Image2Char(string rootdictionary,int rownum,double desiredframerate,string pprefix,string savefilePath)
        {
            processedText = new StreamWriter(savefilePath);
            rowsize = rownum;
            desiredFrameRate = desiredframerate;
            prefix = pprefix;
            root = rootdictionary;
            count=0;
        }

        /// <summary>
        /// Continuous Conversion. Execute it directly to convert a group of bitmaps
        /// Which are organized correctly
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        public void Start()
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

        /// <summary>
        /// Single Conversion. Execute it directly to convert a bitmap
        /// Last Updated: 2014/4/21 Initial commment
        /// Version Number: 1.0.0.0
        /// </summary>
        public void ConvertSinglePicture()
        {
            calculateConfigWithoutCount();
            //ConsoleExtender.ConsoleHelper.SetConsoleFont(0);
            Console.WindowHeight = rowsize+2;
            Console.WindowWidth = colsize + 2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            string res=imageToChar_ColoredWithoutCount();
            for (int i = 0; i < res.Length; i++)
            {
                char colorbyte = res[i];
                if (colorbyte != '\n')
                {
                    ConsoleColor concolor = (ConsoleColor)int.Parse(colorbyte.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                    Console.ForegroundColor = concolor;
                    Console.Write("M");
                }
                else
                {
                    Console.Write("\n");
                }
            }
            Console.WriteLine("Finished Processing");
        }


        private ConsoleColor getClosestConsoleColor(byte R, byte G, byte B)
        {
            ConsoleColor result=0;
            double distance=double.MaxValue;
            foreach(ConsoleColor item in Enum.GetValues(typeof(ConsoleColor)))
            {
                var _simple = Enum.GetName(typeof(ConsoleColor),item);
                switch(_simple)
                {
                    case "DarkBlue": _simple = "Navy"; break;
                    case "DarkGreen": _simple = "Green"; break;
                    case "DarkCyan": _simple = "Teal"; break;
                    case "DarkRed": _simple = "Maroon"; break;
                    case "DarkMagenta": _simple= "Purple"; break;
                    case "DarkYellow": _simple = "Olive"; break;
                    case "Green": _simple = "Lime"; break;
                    default: break;
                }
                var simple = Color.FromName(_simple);
                var dis = Math.Pow(R - simple.R, 2) + Math.Pow(G - simple.G, 2) + Math.Pow(B - simple.B, 2);
                if(dis<distance)
                {
                    distance = dis;
                    result = item;
                }
            }
            return result;
        }
        #endregion
    }
}
