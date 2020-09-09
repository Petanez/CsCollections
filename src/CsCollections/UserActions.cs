using System;

namespace CsCollections
{
    public class UserActions
    {
        public static int TryAssignToCountriesToShowCount(string replacement, int original)
        {
            try
            {
                int.TryParse(replacement, out int countriesToShowCount);
                return countriesToShowCount;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Invalid input");
            }
            return original;
        }

        public static void QuitProgramMessage()
        {
            Console.WriteLine("\nExiting program...");
        }
    }
}