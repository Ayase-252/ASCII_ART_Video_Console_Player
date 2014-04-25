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
        /// <summary>
        /// Main Function
        /// Last Updated: 2014-4-25 Change insctruction
        /// Version Number: 1.0.0.2
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string musicpath, filepath;
            Console.WriteLine(@"Input the path of your processed file(X:\XXX\XXX.op):");
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
