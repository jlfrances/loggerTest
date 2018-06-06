using LoggerTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoggerTest
{
    class Program
    {
        /// <summary>
        /// Entry point of the app
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                string sourceFileName, filterFileName, resultFileName, currentDomainPath;
                List<Filter> filters = new List<Filter>();
                currentDomainPath = AppDomain.CurrentDomain.BaseDirectory;

                Console.WriteLine("Application started");

                // Check args paths
                Validations.ValidateParameters(args);
                sourceFileName = args[0];
                filterFileName = args[1];
                resultFileName = args[2];

                // Check Source and Filter files exists
                Validations.CheckFileExistsInPath(sourceFileName, currentDomainPath);

                Validations.CheckFileExistsInPath(filterFileName, currentDomainPath);

                // Get filters from file 
                // Check all kinds of errors
                GetFiltersFromFile(filterFileName, filters);

                // Open log file and read data
                FilterAndSaveResult(filters, currentDomainPath + sourceFileName, currentDomainPath + resultFileName);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex.Message);
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Filter and save result into a new file
        /// </summary>
        private static void FilterAndSaveResult(List<Filter> filters, string sourceFileName, string resultFileName)
        {
            try
            {
                Logger.LogMessage("Reading file " + sourceFileName + "... ");

                string[] source = File.ReadAllLines(sourceFileName);

                using (StreamWriter writer = new StreamWriter(resultFileName))
                {
                    foreach (Filter filter in filters)
                    {
                        writer.WriteLine(CreateHeader(filter, source.Count(line => line.IndexOf(filter.Substring) >= 0)));

                        for (int index = 0; index < source.Length; index++)
                        {
                            if (source[index].IndexOf(filter.Substring) >= 0)
                            {
                                writer.WriteLine(CreateLine(index + 1, source[index]));
                            }
                        }
                    }
                }

                Logger.LogMessage("Reading file finished.");

                Logger.LogMessage("The file " + resultFileName + " was succesfully created.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get filters specification from Filter file
        /// </summary>
        private static void GetFiltersFromFile(string filterFileName, List<Filter> filters)
        {
            try
            {
                Logger.LogMessage("Getting filters from file... ");

                string[] filterLines = File.ReadAllLines(filterFileName);

                Validations.ValidateFilterFile(filterLines, filters);

                Logger.LogMessage("Filters loaded.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a formatted Header for the Result file
        /// </summary>
        private static string CreateHeader(Filter filter, int matchesCount)
        {
            return string.Format("Label \"{0}\" with search pattern \"{1}\" matched {2} lines.", filter.Label, filter.Substring, matchesCount);
        }

        /// <summary>
        /// Creates a formatted Line for the Result file
        /// </summary>
        private static string CreateLine(int lineNumber, string substring)
        {
            return string.Format(" [{0}] {1}", lineNumber, substring);
        }
    }
}
