using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ImageToChar;

namespace ImageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string returnedKey;
            returnedKey=menu();
            switch (returnedKey)
            {
                case "1":
                    transformOnePicture();
                    break;
                case "2":
                    transformSeriesofPictues();
                    break;
                default:
                    Console.WriteLine("Invid Input!");
                    break;
            }
            Console.ReadKey();
            
        }

        private static string menu()
        {
            string select;
            Console.WriteLine("Welcome to Image to Char Processor");
            Console.WriteLine("Please select a option by inputing the number before the option.");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Transform one picture to char(.txt)");
            Console.WriteLine("2. Transform series of picture to char");
            select = Console.ReadLine();
            return select;
        }


        private static void transformOnePicture()
        {
            string testRoute = @"D:\23.bmp"; //Change this to your test file
            string result;
            Console.WriteLine("Input the row number");
            int rownum=Convert.ToInt32(Console.ReadLine());
            result=Image2Char.ImageToChar(testRoute, rownum,0);
            Console.Clear();
            using (StreamReader config = new StreamReader(@"D:\Processed Text\Config.txt"))
            {
                Console.WindowHeight = Convert.ToInt32(config.ReadLine());
                Console.WindowWidth = Convert.ToInt32(config.ReadLine());
                Console.BufferHeight = Console.WindowHeight;
                Console.BufferWidth = Console.WindowWidth;
            }
            Image2Char.WriteToFile(result, 1);
            Console.Write(result);
        }

        private static void transformSeriesofPictues()
        {
            string root = @"D:\One Week Friend OP Frames\序列 01.mp4"; //test option
            Console.WriteLine("Input the row number");
            int rownum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input the desired framerate");
            double frameRate = Convert.ToDouble(Console.ReadLine());
            try
            {
                for (int count = 0; ; count++)
                {
                    Console.WriteLine("Now Scanning Picture No.{0}.\n", count);
                    Image2Char.WriteToFile(Image2Char.ImageToChar(root + count.ToString("D4") + ".bmp", rownum,frameRate), count);
                }
            }
            catch
            {
                Console.WriteLine("Processing Finished...");
                Console.Read();
            }
        }
    }
}
