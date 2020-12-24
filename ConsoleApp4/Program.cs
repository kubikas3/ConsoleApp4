using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApp4
{
    class Program
    {
        static readonly string text = "While typewriters are the definitive ancestor of all key-based text entry devices, " +
            "the computer keyboard as a device for electromechanical data entry and communication derives largely " +
            "from the utility of two devices: teleprinters (or teletypes) and keypunches. It was through such devices " +
            "that modern computer keyboards inherited their layouts.";
        static readonly Stopwatch sw = new Stopwatch();
        static readonly Timer infoUpdateTimer = new Timer(100)
        {
            AutoReset = true,
            Enabled = false
        };

        static string userInput = string.Empty;
        static int charIndex = 0;
        static int wordIndex = 0;
        static double wordPerMinute = 0;

        static void Main()
        {
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight); // bbz kazkodel cia taip reik idk

            Console.Title = "Start typing...";
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

                if ((char.IsLetter(keyInfo.KeyChar) || char.IsPunctuation(keyInfo.KeyChar)) &&
                    (char.IsLetter(text[charIndex]) || char.IsPunctuation(text[charIndex])))
                {
                    if (!infoUpdateTimer.Enabled)
                    {
                        infoUpdateTimer.Start();
                        sw.Start();
                    }

                    userInput += keyInfo.KeyChar;

                    if (keyInfo.KeyChar.Equals(text[charIndex]))
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }

                    Console.Write(text[charIndex++]);
                    Console.ResetColor();

                    if (charIndex >= text.Length)
                    {
                        break;
                    }
                }
                else if (char.IsWhiteSpace(keyInfo.KeyChar) && charIndex > 0 && !char.IsWhiteSpace(text[charIndex - 1]))
                {
                    wordIndex++;
                    Console.BackgroundColor = ConsoleColor.Red;

                    while (!char.IsWhiteSpace(text[charIndex]))
                    {
                        userInput += ' ';
                        Console.Write(text[charIndex++]);
                    } 

                    Console.ResetColor();
                    userInput += ' ';
                    Console.Write(text[charIndex++]);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && charIndex > 0)
                {
                    SetCursorPositionInBuffer(--charIndex); // cia krc grizta atgal bsk trumpam
                    Console.Write(text[charIndex]);
                    SetCursorPositionInBuffer(charIndex);

                    if (char.IsWhiteSpace(userInput[charIndex]) && !char.IsWhiteSpace(userInput[charIndex - 1]))
                    {
                        wordIndex--;
                    }

                    userInput = userInput.Remove(userInput.Length - 1);
                }

                Console.Title = userInput;
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

        static void SetCursorPositionInBuffer(int index)
        {
            Console.CursorTop = index / Console.BufferWidth;
            Console.CursorLeft = index % Console.BufferWidth;
        }

        static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sw.Elapsed.TotalSeconds > 60)
            {
                sw.Stop();
            }

            //int correctChars = 0;

            //for (int i = 0; i < userInput.Length; i++)
            //{
            //    if (text[i].Equals(userInput[i]))
            //    {
            //        correctChars++;
            //    }
            //}

            //int accuracy = (userInput.Length > 0) ? (int)((double)correctChars / userInput.Length * 100) : 0;
            wordPerMinute = wordIndex / sw.Elapsed.TotalMinutes;
            //Console.Title = $"Time: {60 - (int)sw.Elapsed.TotalSeconds} sec; Speed: {(int)wordPerMinute} WPM; Accuracy: 0%; Word count: {wordIndex}";
        }
    }
}