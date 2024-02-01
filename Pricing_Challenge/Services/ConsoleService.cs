using System;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class ConsoleService : IConsole
    {
        #region IConsoleService Implementation

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void Write(string message, object? arg)
        {
            Console.Write(message);
        }

        public void WriteLine(string message, object? arg)
        {
            Console.WriteLine(message, arg);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        #endregion
    }
}