using System;

namespace 땅따고
{
    internal static class ConsoleEx
    {
        internal static void Print(string text, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.ForegroundColor = foreColor;
            Console.Write(text);
            Console.ResetColor();
        }

        internal static void PrintLine(string text, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.ForegroundColor = foreColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}