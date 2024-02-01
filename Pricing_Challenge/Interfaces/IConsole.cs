using System;

namespace Pricing_Challenge.Interfaces
{
    public interface IConsole
    {
        void Write(string message);

        void WriteLine(string message);

        void Write(string message, object? arg);

        void WriteLine(string message, object? arg);

        string ReadLine();

        ConsoleKeyInfo ReadKey();
    }
}
