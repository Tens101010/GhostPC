//Please note: This application is purely for my own education, to run through coding 
//examples by following tutorials, and to just tinker around with coding.  
//I know it’s bad practice to heavily comment code (code smell), but comments in all of my 
//exercises will largely be left intact as this serves me 2 purposes:
//    I want to retain what my original thought process was at the time
//    I want to be able to look back in 1..5..10 years to see how far I’ve come
//    And I enjoy commenting on things, however redundant this may be . . . 

//Disclaimer: This program will make a user think their computer it possessed.  It is meant as a joke/prank ONLY.
//Left as is it will only play for 10 seconds.
//Update _totalRunTimeSeconds to control length of program runtime so it self aborts.
//Control the when the program executes by updating _startupDelaySeconds.
//Comment out certain modules you don't want affected by the prank: mouse, keyboard, sound, pop-ups

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace GhostPC
{
    class Program
    {
        public static Random _random = new Random();
        public static int _startupDelaySeconds = 3;
        public static int _totalRunTimeSeconds = 10;

        #region Main
        static void Main(string[] args)
        {
            Console.WriteLine("GhostPC Application");

            if (args.Length >= 2)
            {
                _startupDelaySeconds = Convert.ToInt32(args[0]);
                _totalRunTimeSeconds = Convert.ToInt32(args[1]);
            }

            // Create all threads that manipulate all the inputs and outputs to the system
            Thread ghostMouseThread = new Thread(new ThreadStart(GhostMouseThread));
            Thread ghostKeyboardThread = new Thread(new ThreadStart(GhostKeyboardThread));
            Thread ghostSoundThread = new Thread(new ThreadStart(GhostSoundThread));
            Thread ghostPopupThread = new Thread(new ThreadStart(GhostPopupThread));

            DateTime futureSetTime = DateTime.Now.AddSeconds(_startupDelaySeconds);
            Console.WriteLine("Waiting 10 seconds before starting threads");
            while (futureSetTime > DateTime.Now)
            {
                Thread.Sleep(1000);
            }

            // Initiate the threads
            ghostMouseThread.Start();
            ghostKeyboardThread.Start();
            ghostSoundThread.Start();
            ghostPopupThread.Start();

            // Waits for user input
            //Console.Read();

            // Runs the application for 20 seconds, then breaks out of this loop, therefore hitting the abort functions.
            // To kill the app automatically
            futureSetTime = DateTime.Now.AddSeconds(_totalRunTimeSeconds);
            while (futureSetTime > DateTime.Now)
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("Terminating all threads");
            // Kill the program
            ghostMouseThread.Abort();
            ghostKeyboardThread.Abort();
            ghostSoundThread.Abort();
            ghostPopupThread.Abort();
        }
        #endregion

        //
        //

        #region Ghost Threads

        #region Mouse
        // This thread will randomly affect the mouse movement
        public static void GhostMouseThread()
        {
            int moveX = 0;
            int moveY = 0;

            Console.WriteLine("GhostMouseThread Started");
            while (true)
            {
                //Console.WriteLine(Cursor.Position.ToString());

                // Generate the random ammount to move the mouse in X and Y
                moveX = _random.Next(40) - 20;
                moveY = _random.Next(40) - 20;
                Cursor.Position = new System.Drawing.Point(Cursor.Position.X + moveX, Cursor.Position.Y + moveY);
                Thread.Sleep(200);
            }
        }
        #endregion

        #region Keyboard
        // this thread will create random keyboard output
        public static void GhostKeyboardThread()
        {
            Console.WriteLine("GhostKeyboardThread Started");
            while (true)
            {
                if (_random.Next(100) < 75)
                {
                    // Generate a random capital letter - using the ASCII table (65-90 = A-Z)
                    char key1 = (char)(_random.Next(26) + 65);
                    // Generate a rule that 50% of the time a lower case letter will generate
                    if (_random.Next(2) == 0)
                    {
                        key1 = Char.ToLower(key1);
                    }
                    SendKeys.SendWait(key1.ToString());
                }
                Thread.Sleep(_random.Next(500));
            }
        }
        #endregion

        #region Sound
        // This thread will play random system sounds
        public static void GhostSoundThread()
        {
            Console.WriteLine("GhostSoundThread Started");
            while (true)
            {
                // 25% probability to play a sound
                if (_random.Next(100) < 75)
                {
                    switch (_random.Next(5))
                    {
                        case 0:
                            SystemSounds.Asterisk.Play();
                            break;
                        case 1:
                            SystemSounds.Beep.Play();
                            break;
                        case 2:
                            SystemSounds.Hand.Play();
                            break;
                        case 3:
                            SystemSounds.Exclamation.Play();
                            break;
                        case 4:
                            SystemSounds.Question.Play();
                            break;
                    }
                }
                // Plays every 0-2 seconds (75% of the time)
                Thread.Sleep(_random.Next(2000));
            }
        }
        #endregion

        #region Popups
        // This thread will annoy the user with popups
        public static void GhostPopupThread()
        {
            Console.WriteLine("GhostPopupThread Started");
            while (true)
            {
                // Every 3 seconds roll the dice.  25% chance for a popup
                if (_random.Next(100) > 60)
                {
                    // Determine which message to display
                    switch (_random.Next(2))
                    {
                        case 0:
                            MessageBox.Show("Internet Explorer has stopped functioning",
                            "Internet Explorer",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            break;
                        case 1:
                            MessageBox.Show("Your computer is acting strange. . .",
                            "Microsoft Windows",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                            break;
                    }
                }
                Thread.Sleep(3000);
            }
        }
        #endregion

        #endregion

    }
}
