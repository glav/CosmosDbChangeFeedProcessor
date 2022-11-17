using System;

namespace CosmosChangeFeedExample
{
    public static class Logger
    {
        private static ConsoleColor _originalForegroundColour;
        static Logger()
        {
            _originalForegroundColour = Console.ForegroundColor;
        }
        public static void Write(string msg, ConsoleColor colour = ConsoleColor.White)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine($">> {msg}");
            Console.ForegroundColor = _originalForegroundColour;
        }
        public static void Error(string msg)
        {
            Write(msg,ConsoleColor.Red);
        }
        public static void Info(string msg)
        {
            Write(msg,ConsoleColor.Green);
        }
        public static void Verbose(string msg)
        {
            Write(msg,ConsoleColor.Cyan);
        }
    }
}