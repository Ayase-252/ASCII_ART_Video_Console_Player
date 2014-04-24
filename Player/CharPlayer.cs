using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;

namespace Player
{
    /// <summary>
    /// Class library to play ASCII video
    /// </summary>
    public class CharPlayer
    {
        #region data members
        /* Defination of data members
         * Last Updated: 2014/4/21 Initial comment
         * Version Number: 1.0.0.0
         */
        private static int frameCount; //The current frame number
        private static int row; //The amount of rows. Read from the config area of processed text
        private static int col; //The amount of columns
        /* The data members below is related to the frame control.*/
        private static Timer frameControl; //Main frame control. Raise a event to change frame in tiny interval
        private static Timer frameOffsetTimer; //Offset timer, raise a event to check frame rate and offset frame in relative bigger interval
        private static int previousFrame; //The frame number when last offset was executed
        private static double frameRate; //The average frame rate from last offset to this check
        private static double desiredFrameRate; //The desired frame rate you want to play at
        private static int frameControlInterval; //The interval of main frame control.
        private static int noneChangeCount; //The time of frame check without offset

        private static string[] frame; //Frames
        /* Defination of data members ended here */

        #endregion



        #region public method
        /// <summary>
        /// Constructor
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <param name="processedTextRoot">root dictionary of the processed text file</param>
        public CharPlayer(string processedText)
        {
            loadFile(processedText);
            frameCount = 0;
            Console.WriteLine("Initialing....");
            initializeInterval();
            frameControl = new Timer(frameControlInterval);
            frameOffsetTimer = new Timer(1000);
            frameControl.Elapsed += new ElapsedEventHandler(OnElapsed);
            frameOffsetTimer.Elapsed += new ElapsedEventHandler(OnOffsetElapsed);
            Console.WriteLine("Initiallized...");
        }

        /// <summary>
        /// Start to play ASCII video
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        public void Play()
        {
            frameControl.Enabled = true;
            frameOffsetTimer.Enabled = true;
            previousFrame = 0;
        }
        #endregion

        #region private method

        private CharPlayer() //Forbidden to construct the class without required information
        {
        }

        /// <summary>
        /// The event handle raised by main frame control.
        /// To write the present frames into console
        /// Last Updated: 2014/4/21 Adjust structure. Make original content as an independent method
        /// Version Number: 1.0.0.2
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnElapsed(object source, ElapsedEventArgs e)
        {
            refreshnextFrame();
        }

        /// <summary>
        /// Event raized by offset timer
        /// Execute a frame check and offset the frame by the different between actual value and desired
        /// Solution II to Issue #1 :https://github.com/AragakiAyase/ASCII_ART_Video_Console_Player/issues/1
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.0
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnOffsetElapsed(object source, ElapsedEventArgs e)
        {
            int checkInterval = noneChangeCount + 1;
            frameRate = ((double)frameCount - (double)previousFrame) / checkInterval;
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

        /// <summary>
        /// To initialze interval of main frame control, depending on PC performance
        /// Solution I to Issue #1: https://github.com/AragakiAyase/ASCII_ART_Video_Console_Player/issues/1
        /// Last Updated: 2014/4/21 Change the strategy to determine the interval, now manual interference is not required
        /// Version Number: 1.0.0.2
        /// </summary>
        private static void initializeInterval()
        {
            frameCount = 0;
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            Console.WriteLine("Initializing the interval...");
            start = DateTime.Now;
            for (int time = 0; time < 100;time++ )
            {
                refreshnextFrame();
            }
            end = DateTime.Now;
            TimeSpan offset = end - start;
            double averoffset = offset.TotalMilliseconds / frameCount;
            frameControlInterval = 1000 /(int) desiredFrameRate - (int)averoffset;
            Console.WriteLine("Now the interval is {0} ms.(Push Enter to Continue)",frameControlInterval);
            Console.Clear();
            frameCount=0;
        }


        /// <summary>
        /// Read config and frames from processed text file
        /// Last Updated: 2014/4/21 Initial comment
        /// Version Number: 1.0.0.1 Change parameter name from dictionary to path to increase the readability
        /// </summary>
        /// <param name="dictionary">Path of processed text file</param>
        private static void loadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            row=Convert.ToInt32(reader.ReadLine())-2;
            col=Convert.ToInt32(reader.ReadLine())-2;
            Console.WindowHeight = row+2;
            Console.WindowWidth = col+2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            desiredFrameRate = Convert.ToDouble(reader.ReadLine());
            Console.WriteLine("Reading Frames...");
            int initialLength = 2400;
            int incresement = 500;
            frame = new string[initialLength];
            try
            {
                for (int index = 0; ; index++)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int rc = 0; rc < row; rc++)
                    {
                        if (reader.EndOfStream == true)
                        {
                            Console.WriteLine("Reading Finished...");
                            return;
                        }
                        builder.Append(reader.ReadLine() + "\n");
                    }
                    try
                    {
                        frame[index] = builder.ToString();
                    }
                    catch
                    {
                        string[] newarray = new string[frame.Length + incresement];
                        Array.Copy(frame, 0, newarray, 0, frame.Length);
                        frame = newarray;
                        frame[index] = builder.ToString();
                    }
                    Console.WriteLine("Reading frame {0}", index);
                }

            }
            catch
            {
                Console.WriteLine("Failure...");
            }

        }

        /// <summary>
        /// Fefresh next frame, separated from OnElapsed
        /// Last Updated: 2014/4/21 Created
        /// Version Number: 1.0.0.2
        /// </summary>
        private static void refreshnextFrame()
        {
            Console.Clear();
            try
            {
                Console.Write(frame[frameCount]);
                Console.WriteLine("Now framerate is {0} FPS.", frameRate);
                frameCount++;
            }
            catch
            {
                Console.WriteLine("Finished..");
                frameControl.Enabled = false;
                frameOffsetTimer.Enabled = false;
            }
        }
        #endregion
    }
}
