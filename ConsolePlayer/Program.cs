using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Media;
using Player;

namespace ConsolePlayer
{
    /// <summary>
    /// Program to play
    /// </summary>
    class Program
    {
        /* Defination of Data members
         * Last Updated: 2014/4/20 Initial comment
         * Version Number: 1.0.0.0
         */
        private static string readroot; //The path of the file contained ASCII images.

        /* Defination of members ended here*/

        /// <summary>
        /// Main Function
        /// Last Updated: 2014-4-20 Initail comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            SoundPlayer bgmPlayers = new SoundPlayer(@"D:\Bad Apple!!.wav"); // a simple music player which support the wave only. 
            readroot = @"D:\Processed Text\Demo.op"; // Should change to appeal the requirement of users.
            CharPlayer player = new CharPlayer(readroot);
            Console.WriteLine("Ready to play?");
            Console.ReadLine();
            player.Play();
            bgmPlayers.Play();
            Console.ReadLine();
        }

    }
}
