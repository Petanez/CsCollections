using System;
using System.Collections.Generic;

namespace CsCollections.Lists
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> daysOfWeek = new List<string>(){"monday", "tuesday", "wednesday"};
            
            foreach(string day in daysOfWeek)
            {
                System.Console.WriteLine(day);
            }

        }
    }
}