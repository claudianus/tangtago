using System;
using System.Text;

using static 땅따고.Game;
using static 땅따고.ConsoleEx;

namespace 땅따고
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(70, 30);
            Console.SetBufferSize(70, 9999);
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "땅따고";

            while (true)
            {
#if DEBUG
                PrintLine("[*] 새로운 게임", ConsoleColor.Cyan);
#endif
                BeginGame($"{Environment.CurrentDirectory}\\map.txt");
            }
        }
    }
}