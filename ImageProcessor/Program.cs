using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ImageToChar;

namespace ImageProcessor
{
    /// <summary>
    /// Discription: Program to convert bitmap to char image
    /// </summary>
    class Program
    {
        /// <summary>
        /// Discription: Main Function
        /// Last Update: 2014/4/20 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <param name="args"></param>
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
        /// <summary>
        /// Discription: Display a menu, and return the option number that user chose.
        /// Last Updated: 2014/4/20 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <returns>The option number user chose</returns>
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

        /// <summary>
        /// Discription: Convert single image to ASCII image, and output it as a text file(*.txt).
        /// Last Updated: 2014/4/20 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        private static void transformOnePicture()
        {
            Console.WriteLine(@"Input the full path of your picture.(Eg. D:\onepicture.bmp)");
            string path = Console.ReadLine();
            Console.WriteLine("Input the row number");
            int rownum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(@"Input the path the save file.(Eg. input D:\Dictionary\ then the save file will save as D:\Dictionary\demo.op)");
            string savepath = Console.ReadLine();
            Image2Char convert = new Image2Char(path, rownum, 0, null, savepath+"demo.op");
            convert.ConvertSinglePicture();
            Console.ReadLine();
        }

        /// <summary>
        /// Convert all images arranged by specific rule to ASCII image, and output them as text files.
        /// Last Updated: 2014/4/20 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        private static void transformSeriesofPictues()
        {
            Console.WriteLine(@"Input the root dictionary of your pictures(Include \ in the last of your path)");
            string root = Console.ReadLine();
            Console.WriteLine("Input prefix of your pictures(Eg. badapple0000.bmp prefix is badapple)");
            string prefix = Console.ReadLine();
            Console.WriteLine("Input the row number");
            int rownum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input the desired framerate");
            double frameRate = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine(@"Input the path the save file.(Eg. input D:\Dictionary\ then the save file will save as D:\Dictionary\demo.op)");
            string savefilePath = Console.ReadLine();
            Image2Char convert = new Image2Char(root, rownum, frameRate, prefix, savefilePath + "demo.op");
            convert.Start();
        }
    }
}
