using ProbeAssistedLeveler;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var leveler = new Leveler();
            leveler.LevelBed();
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
