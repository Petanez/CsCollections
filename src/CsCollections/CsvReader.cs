using System;
using System.IO;
using System.Collections.Generic;

namespace CsCollections
{
    public class CsvReader
    {
        private string _csvFilePath;

        public CsvReader(string csvFilePath)
        {
            this._csvFilePath = csvFilePath;
        }

        

        public Country[] ReadFirstNCountries(int nCountries)
        {
            Country[] countries = new Country[nCountries];

/*             using (var reader = File.OpenText(_csvFilePath))
            {
                //read header line
                reader.ReadLine();

                for(int i = 0; i < nCountries; i++) 
                {
                    var line = reader.ReadLine();
                    countries[i] = ReadCountryFromCsvLine(line);
                }


            } */

            using (StreamReader reader = new StreamReader(_csvFilePath))
            {
                //  Read header
                reader.ReadLine();

                for (var i = 0; i < nCountries; i++)
                {
                    var line = reader.ReadLine();
                    countries[i] = ReadCountryFromCsvLine(line);
                }
            }

            return countries;
        }

        public Dictionary<string, List<Country>> ArrangeIntoDictByRegion(List<Country> countries)
        {
            var countriesByRegionDict = new Dictionary<string, List<Country>>();
            for (var i = 0; i < countries.Count; i++)
            {
                if (!countriesByRegionDict.ContainsKey(countries[i].Region))
                {
                    countriesByRegionDict.Add(countries[i].Region, new List<Country>(){countries[i]});
                    
                }
                else
                {
                    countriesByRegionDict[countries[i].Region].Add(countries[i]);
                }

            }
            return countriesByRegionDict;
        }


/*         public Dictionary<string, Country> ReadAllCountries()
        {
            var countries = new Dictionary<string, Country>();

            using (StreamReader reader = new StreamReader(_csvFilePath))
            {
                //read header
                reader.ReadLine();

                string line;
                while((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Country country = ReadCountryFromCsvLine(line);
                        countries.Add(country.Code, country);
                    }
                    catch(System.Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            return countries;
        } */

        public List<Country> ReadAllCountries()
        {
            var countries = new List<Country>();

            using (StreamReader reader = new StreamReader(_csvFilePath))
            {
                //read header
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        countries.Add(ReadCountryFromCsvLine(line));
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            return countries;
        }

        public void RemoveCommaCountries(List<Country> countries)
        {
            for(var i = 0; i < countries.Count;)
            {
                if(countries[i].Name.Contains(","))
                    countries.RemoveAt(i);
                else
                    i++;
            }
        }


        public Country ReadCountryFromCsvLine(string csvLine)
        {
            string name;
            string code;
            string region;
            string popText;

            string[] parts = csvLine.Split(",");

            if (parts.Length == 4)
            {
                name = parts[0];
                code = parts[1];
                region = parts[2];
                popText = parts[3];
            }
            else if (parts.Length > 4)
            {
                name = parts[0] + ", " + parts[1];
                name = name.Replace("\"", null).Trim();
                code = parts[2];
                region = parts[3];
                popText = parts[4];
            }
            else
            {
                throw new Exception($"Can't parse country from csvLine: {csvLine}");
            }

            //TryParse leaves population=0 if can't parse
            int.TryParse(popText, out int population);
            return new Country(name, code, region, population);
        }
    }
}