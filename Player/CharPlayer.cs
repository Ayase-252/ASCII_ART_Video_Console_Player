using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;

namespace Player
{
    public class CharPlayer
    {
        private static int frameCount;
        private static string readRoot;
        private static string currentFrame;
        /*以下成员与帧率限制有关
         The data members below is related to the frame limiter.
         */
        private static Timer frameControl; //主帧率控制计时器，以实现逼近预定帧率的间隔时间触发。
        private static Timer frameOffsetTimer; //播放中帧数延迟计时器，每3s执行一次帧率检测。
        private static int previousFrame; //之前一次执行帧数延迟的帧数。
        private static double frameRate; //当前的平均帧率
        private static double desiredFrameRate; //预定播放帧率
        private static int frameControlInterval; //主帧率控制计时器间隔。
        private static int noneChangeCount; //执行帧数延迟检测时没有进行延迟的次数。


        private CharPlayer()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processedTextRoot">处理过的字符画帧根目录</param>
        public CharPlayer(string processedTextRoot)
        {
            frameCount = 0;
            Console.WriteLine("Initialing....");
            using (StreamReader config = new StreamReader(processedTextRoot + "Config.txt"))
            {
                Console.WindowHeight = Convert.ToInt32(config.ReadLine());
                Console.WindowWidth = Convert.ToInt32(config.ReadLine());
                Console.BufferHeight = Console.WindowHeight;
                Console.BufferWidth = Console.WindowWidth;
                desiredFrameRate = Convert.ToDouble(config.ReadLine());
            }
            readRoot = processedTextRoot;
            initializeInterval();
            frameControl = new Timer(frameControlInterval);
            frameOffsetTimer = new Timer(1000);
            frameControl.Elapsed += new ElapsedEventHandler(OnElapsed);
            frameOffsetTimer.Elapsed += new ElapsedEventHandler(OnOffsetElapsed);
            Console.WriteLine("Initiallized...");
        }

        /// <summary>
        /// 播放字符画。
        /// </summary>
        public void Play()
        {
            frameControl.Enabled = true;
            frameOffsetTimer.Enabled = true;
            previousFrame = 0;
        }

        /// <summary>
        /// 在主帧率控制器下向控制台写入当前帧数代表的字符画。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnElapsed(object source, ElapsedEventArgs e)
        {
            Console.Clear();
            try
            {
                loadCurrentFrame();
                Console.Write(currentFrame);
                Console.WriteLine("Now framerate is {0} FPS.", frameRate);
            }
            catch
            {
                Console.WriteLine("Finished..");
            }
        }

        private static void OnOffsetElapsed(object source, ElapsedEventArgs e)
        {
            int checkInterval = noneChangeCount + 1;
            frameRate = ((double)frameCount - (double)previousFrame)/checkInterval;
            int offset = (int)(frameRate - desiredFrameRate) * checkInterval;
            if (offset != 0)
            {
                frameCount -= offset;
                previousFrame = frameCount;
                noneChangeCount = 0;
            }
            else
            {
                noneChangeCount++;
            }
        }

        private static void initializeInterval()
        {
            frameCount = 0;
            frameControl = new Timer(100);//每0.1s触发一次将帧写入控制台的操作。
            frameControl.Elapsed += new ElapsedEventHandler(OnElapsed);
            DateTime start = new DateTime();
            start = DateTime.Now;
            DateTime end = new DateTime();
            Console.WriteLine("Initializing the interval...");
            frameControl.Enabled = true;
            Console.ReadLine();
            frameControl.Enabled = false;
            frameControl.Dispose();
            end = DateTime.Now;
            TimeSpan offset = end - start;
            double averoffset = (offset.TotalMilliseconds - 100 * frameCount) / frameCount;
            frameControlInterval = 1000 /(int) desiredFrameRate - (int)averoffset;
            Console.WriteLine("Now the interval is {0} ms.(Push Enter to Continue)",frameControlInterval);
            Console.ReadLine();
            Console.Clear();
            frameCount=0;
        }

        private static void loadCurrentFrame()
        {
            try
            {
                using (StreamReader protext = new StreamReader(readRoot + frameCount.ToString() + @".txt"))
                {
                    currentFrame = protext.ReadToEnd();
                }
                frameCount++;
            }
            catch
            {
                frameControl.Enabled = false;
                frameOffsetTimer.Enabled = false;
                Console.WriteLine("Finished..");
            }
        }
    }
}
