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
        /// Last Updated: 2014-4-24 Enable user to input the path.
        /// Version Number: 1.0.0.1
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string musicpath, filepath;
            Console.WriteLine(@"Input the path of your processed file(X;\XXX\XXX.op):");
            filepath=Console.ReadLine();
            Console.WriteLine("Input the path of BGM(*.wav only):");
            musicpath = Console.ReadLine();
            SoundPlayer bgmPlayers = new SoundPlayer(musicpath); // a simple music player which support the wave only. 
            CharPlayer player = new CharPlayer(filepath);
            Console.WriteLine("Ready to play?");
            Console.ReadLine();
            player.Play();
            bgmPlayers.Play();
            Console.ReadLine();
        }

    }
}
