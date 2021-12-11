using MedHelper_EF.Models;
using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MedHelperDB db = new MedHelperDB())
            {
                // 1
                foreach (var i in db.Medicines)
                    Console.WriteLine($"Назва: {i.Name} Група: {i.pharmacotherapeuticGroup}");
                Console.WriteLine();

                //2
                foreach (var i in db.Compositions)
                    Console.WriteLine(i.Description);
                Console.WriteLine();
            }
        }
    }
}
