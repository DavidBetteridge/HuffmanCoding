using System;
using System.Diagnostics;

namespace HuffmanCoding
{
    internal static class Timers
    {
        /// <summary>
        /// Writes to the console how long the function took to execute in Milliseconds
        /// </summary>
        internal static T Time<T>(Func<T> function)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = function();
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");

            return result;
        }

        /// <summary>
        /// Writes to the console how long the method took to execute in Milliseconds
        /// </summary>
        internal static void Time(this Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
        }

    }
}
