using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            int countriesToShowCount = 10;            
            //  If user input is Not empty try parsing the first argument  
            //  to override countriesToShowCount if fails returns original
            if (args.Length != 0)
                countriesToShowCount = UserActions.TryAssignToCountriesToShowCount(args[0], countriesToShowCount);

            //  Get path of the file to read countries from and read it
            string filePath = @"C:\Users\Pete\CsCollections\practiseFiles\Pop by Largest Final.csv";

            //  Read
            CsvReader reader = new CsvReader(filePath);
            List<Country> countries = reader.ReadAllCountries();
            var countriesByRegion = reader.ArrangeIntoDictByRegion(countries);
        Beginning:

            //  User Decision: Continue to region selection or quit program
            Console.WriteLine("   Would you like to continue to region selection?   Yes/No");
            Console.Write("Enter input > ");
            string input_continueToRegion = UserValidation.ValidateCasing(Console.ReadLine());         
            bool validEntryInput = false;
            while (!validEntryInput)
            {
                validEntryInput = UserValidation.IsYesOrNo(input_continueToRegion);
                if (validEntryInput)
                {
                    if (input_continueToRegion == "Yes")
                        break;
                UserActions.QuitProgramMessage();
                return;          
                }

                Console.WriteLine("Invalid input, try again.");
                Console.Write("Enter input > ");
                input_continueToRegion = UserValidation.ValidateCasing(Console.ReadLine());                    
            }

            //  The actual program
            while (true)
            {
                //  List Regions for user to choose from 
                Console.WriteLine("\n   Choose one of the following regions to get a list of its countries by population");
                foreach (string countryName in countriesByRegion.Keys)
                    Console.WriteLine(countryName);         

                //  Get region from user
                Console.WriteLine("\n   To continue type in:");
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "All regions", "Continue by showing all countries of all regions"));  
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Region", "Insert the name of the region to continue"));  
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Q", "To quit"));  

                Console.Write("Enter input > ");
                var input_chosenRegion = UserValidation.ValidateCasing(Console.ReadLine());
                if (input_chosenRegion == "All Regions")
                {
                    for (var i = 0; i < countries.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {PopulationFormatter.FormatPopulation(countries[i].Population).PadLeft(15)}: {countries[i].Name}");
                        if (i == countries.Count - 1)
                        {
                            Console.WriteLine($"   Those were all the listed countries in the world");
                            goto Beginning;
                        }
                    }
                }
                else if (input_chosenRegion == "Q")
                {
                    UserActions.QuitProgramMessage();
                    return;
                }

                //  Check if region exists in Dictionary if not user has 3 retries        
                int retryInputCount = 0;
                int maxInputRetryCount = 3;
                while (retryInputCount < maxInputRetryCount)
                {
                    if (countriesByRegion.ContainsKey(input_chosenRegion))
                        break;
                    else
                    {
                        Console.WriteLine($"   Invalid input, please check for typos. You have {maxInputRetryCount-retryInputCount} retries left.");
                        Console.Write("Enter input > ");
                        input_chosenRegion = UserValidation.ValidateCasing(Console.ReadLine());
                        if (input_chosenRegion == "Q")
                        {
                            UserActions.QuitProgramMessage();
                            return;
                        }
                        retryInputCount++;
                    }
                }

                //  User decision: Show all countries in the region or 10 countries at a time or Q to quit
            regionOptions:
                Console.WriteLine($"\n   Would you like to view all the countries or {countriesToShowCount} at a time?:");
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Show (rank)", "Continue by showing the country ranked (number)"));  
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Show all", "Continue by showing all countries in the region"));  
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Anykey", $"Continue by showing {countriesToShowCount} countries of the region at a time"));  

                Console.Write("Enter input > ");
                var input_showAllCountries = UserValidation.ValidateCasing(Console.ReadLine());  
                if (input_showAllCountries == "Q")
                {
                    UserActions.QuitProgramMessage();
                    break;
                }

                //  Show country by name       
                // try
                // {         
                //     Country countryToShow = countries.Where(x => x.Name.Equals(input_showAllCountries.Split(" ")[1])).First();
                //     if (countryToShow != null)
                //     {
                //         Console.WriteLine($"{countriesByRegion[input_chosenRegion].IndexOf(countryToShow) + 1}.{PopulationFormatter.FormatPopulation(countryToShow.Population).PadLeft(15)}: {countryToShow.Name}");
                //         Console.Write("Press anykey to continue > ");
                //         Console.ReadLine();
                //         goto regionOptions;
                //     }
                // }
                // catch (System.Exception ex)
                // {
                //     Console.WriteLine(ex.Message);
                //     goto regionOptions;
                // }
                    

                //  Show country by rank 
                var tryGetCountryRank = input_showAllCountries.Split(" ");
                if (tryGetCountryRank.Length == 2)
                {
                    int.TryParse(tryGetCountryRank[1], out int countryRank);
                    if ((countryRank != 0) && (countryRank <= countriesByRegion[input_chosenRegion].Count))
                    {
                        Console.WriteLine($"{countryRank}. {PopulationFormatter.FormatPopulation(countriesByRegion[input_chosenRegion][countryRank - 1].Population).PadLeft(15)}: {countriesByRegion[input_chosenRegion][countryRank - 1].Name}");
                        Console.ReadLine();
                        goto regionOptions;
                    }
                }

                bool showAllCountries = (input_showAllCountries == "Show All" ? true : false);

                //  Show all countries of the region or {countriesToShowCount} at a time
                int loopingValue = (showAllCountries ? countriesByRegion[input_chosenRegion].Count : countriesToShowCount);
                int loopStartValue = 0;
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Anykey", $"To continue showing {countriesToShowCount} countries")); 
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Return", $"To return to region selection")); 
                Console.WriteLine(String.Format("{0, -15}      ||{1, -10}", "Q", $"To quit")); 
                Console.WriteLine($"   There are {countriesByRegion[input_chosenRegion].Count} countries listed in {input_chosenRegion}.");
                while (true)
                {
                    //              SIDENOTES
                    //  Sructure of the for statement
                    //  for (initializer; condition; iterator)
                    //
                    //  Math.Min in for loop condition to prohibit index going out of range during last iteration
                    for (var i = loopStartValue; i < Math.Min(loopStartValue + loopingValue, countriesByRegion[input_chosenRegion].Count); i++)
                    {
                        Console.WriteLine($"{i + 1}. {PopulationFormatter.FormatPopulation(countriesByRegion[input_chosenRegion][i].Population).PadLeft(15)}: {countriesByRegion[input_chosenRegion][i].Name}");
                        if (i == countriesByRegion[input_chosenRegion].Count - 1)
                        {
                            System.Console.WriteLine("");
                            goto Beginning;
                        }
                    }
                    
                    //  Get user decision:
                    //  goto Beginning if user decision is No 
                    //  return if user decision is Q       
                    Console.Write("Enter input > "); 
                    string input_getTheNextCountries = UserValidation.ValidateCasing(Console.ReadLine());
                    if (input_getTheNextCountries == "Return")
                        goto Beginning;
                    else if (input_getTheNextCountries == "Q")
                    {
                        UserActions.QuitProgramMessage();
                        return;
                    }
                loopStartValue += loopingValue;
                }
            }








            /* bool gotCountry = countries.TryGetValue(input_showAllCountries, out Country country);
            if (gotCountry)
                Console.WriteLine($"{country.Name} has population {PopulationFormatter.FormatPopulation(country.Population)}");
            else
                Console.WriteLine($"Sorry, there is no country with the code {input_showAllCountries}"); */



/*              Console.WriteLine("Enter no. of countries to display>");
            bool inputIsInt = int.TryParse(Console.ReadLine(), out int input_showAllCountries);
            if (!inputIsInt || input_showAllCountries <= 0)
            {
                Console.WriteLine("You must type in a +ve integer. Exiting");
                return;
            }

            int maxToDisplay = input_showAllCountries; */
       }
    }
}
 