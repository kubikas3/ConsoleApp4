using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApp4
{
    class Program
    {
        static string text = "While typewriters are the definitive ancestor of all key-based text entry devices, " +
            "the computer keyboard as a device for electromechanical data entry and communication derives largely " +
            "from the utility of two devices: teleprinters (or teletypes) and keypunches. It was through such devices " +
            "that modern computer keyboards inherited their layouts.";

        static string userInput = string.Empty;
        static int charIndex = 0;
        static int wordCount = 0;
        static double wordPerMinute = 0;
        static Stopwatch sw = new Stopwatch();
        static Timer infoUpdateTimer = new Timer(100)
        {
            AutoReset = true,
            Enabled = false
        };

        private static void Main()
        {
            ConsoleColor defaultBg = Console.BackgroundColor;
            
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight); // bbz kazkodel cia taip reik idk

            Console.Title = string.Empty;
            Console.CursorSize = 100;
            Console.CursorVisible = true;

            Console.Write(text);
            Console.SetCursorPosition(0, 0);
            infoUpdateTimer.Elapsed += Timer_Elapsed;

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

                if (sw.Elapsed.TotalSeconds > 60)
                {
                    break;
                }

                if (keyInfo.Key != ConsoleKey.Enter &&
                    keyInfo.Key != ConsoleKey.Backspace &&
                    keyInfo.Key != ConsoleKey.Tab)
                {
                    if (!infoUpdateTimer.Enabled)
                    {
                        infoUpdateTimer.Start();
                        sw.Start();
                    }

                    if ((char.IsWhiteSpace(text[charIndex]) && char.IsWhiteSpace(keyInfo.KeyChar)) ||
                        (!char.IsWhiteSpace(text[charIndex]) && !char.IsWhiteSpace(keyInfo.KeyChar)))
                    {
                        userInput += keyInfo.KeyChar;

                        if (char.IsWhiteSpace(text[charIndex]))
                        {
                            wordCount++;
                        }

                        if (userInput[charIndex].Equals(text[charIndex]))
                        {
                            if (char.IsWhiteSpace(userInput[charIndex]))
                            {
                                Console.BackgroundColor = defaultBg;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                            }
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }

                        Console.Write(text[charIndex++]);
                        Console.BackgroundColor = defaultBg;

                        if (charIndex >= text.Length)
                        {
                            break;
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && charIndex > 0)
                {
                    MoveCursorBack();

                    userInput = userInput.Remove(userInput.Length - 1);
                    Console.BackgroundColor = defaultBg;
                    Console.Write(text[--charIndex]); // cia sitas sudas vapshe kaip alien atrodo xdd
                    MoveCursorBack();

                    if (char.IsWhiteSpace(text[charIndex]))
                    {
                        wordCount--;
                    }
                }
            } while (keyInfo.Key != ConsoleKey.Escape);

            sw.Stop();
            infoUpdateTimer.Stop();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Console.Write("Press Escape to exit...");

            do
            {
                keyInfo = Console.ReadKey(true);
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        static void MoveCursorBack()
        {
            if (Console.CursorLeft == 0)
            {
                if (Console.CursorTop > 0)
                {
                    Console.CursorTop--;
                    Console.CursorLeft = Console.BufferWidth - 1;
                }
            }
            else
            {
                Console.CursorLeft--;
            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sw.Elapsed.TotalSeconds > 60)
            {
                sw.Stop();
            }

            int correctChars = 0;

            for (int i = 0; i < userInput.Length; i++)
            {
                if (text[i].Equals(userInput[i]))
                {
                    correctChars++;
                }
            }

            int accuracy = (userInput.Length > 0) ? (int)((double)correctChars / userInput.Length * 100) : 0;
            wordPerMinute = wordCount / sw.Elapsed.TotalMinutes;
            Console.Title = $"Time: {60 - (int)sw.Elapsed.TotalSeconds} min; Speed: {(int)wordPerMinute} WPM; Accuracy: {accuracy}%; Word count: {wordCount}";
        }
    }
}