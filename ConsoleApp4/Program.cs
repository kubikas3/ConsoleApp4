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
        static Timer timer = new Timer(1000)
        {
            AutoReset = true,
            Enabled = false
        };

        private static void Main()
        {
            ConsoleColor defaultBg = Console.BackgroundColor;
            Console.CursorSize = 100;
            Console.Write(text);
            Console.SetCursorPosition(0, 0);
            timer.Elapsed += Timer_Elapsed;

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key != ConsoleKey.Backspace)
                {
                    if (!timer.Enabled)
                    {
                        timer.Start();
                        sw.Start();
                    }

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
                else if (charIndex > 0)
                {
                    //if (Console.CursorLeft == 0)
                    //{
                    //    Console.CursorTop = (Console.CursorTop > 0) ? Console.CursorTop - 1 : 0;
                    //}
                    //else
                    //{
                    //    Console.CursorLeft--;
                    //}

                    userInput = userInput.Remove(userInput.Length - 1);
                    Console.BackgroundColor = defaultBg;
                    Console.Write($"\b{text[--charIndex]}\b"); // cia sitas sudas vapshe kaip alien atrodo xdd

                    if (char.IsWhiteSpace(text[charIndex]))
                    {
                        wordCount--;
                    }
                }
            } while (keyInfo.Key != ConsoleKey.Escape);
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
            Console.Title = $"Time: {60 - sw.Elapsed.Seconds} min; Speed: {wordPerMinute} WPM; Accuracy: {accuracy}%";
        }
    }
}