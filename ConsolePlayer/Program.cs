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
    /// 播放器程序
    /// Program to play
    /// </summary>
    class Program
    {
        private static string readroot;
        public static void Main(string[] args)
        {
            SoundPlayer bgmPlayers = new SoundPlayer(@"D:\OneWeekFriendOP.wav");
            readroot = @"D:\Processed Text\Demo.op";
            CharPlayer player = new CharPlayer(readroot);
            Console.WriteLine("Ready to play?");
            Console.ReadLine();
            player.Play();
            bgmPlayers.Play();
            Console.ReadLine();
        }

    }
}
