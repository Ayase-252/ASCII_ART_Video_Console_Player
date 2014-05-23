﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Player;
using ConsoleExtender;

namespace TestMode
{
    class Program
    {
        static void Main(string[] args)
        {
            var fonts = ConsoleHelper.ConsoleFonts;
            for (int f = 0; f < fonts.Length; f++)
                Console.WriteLine("{0}: X={1}, Y={2}",
                   fonts[f].Index, fonts[f].SizeX, fonts[f].SizeY);
            ConsoleHelper.SetConsoleFont(5);
            ConsoleHelper.SetConsoleIcon(SystemIcons.Information);
        }
    }
}
