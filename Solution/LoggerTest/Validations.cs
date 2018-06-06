using LoggerTest.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoggerTest
{
    /// <summary>
    /// Validation methods
    /// </summary>
    public static class Validations
    {
        /// <summary>
        /// Validates application input arguments
        /// </summary>
        public static void ValidateParameters(string[] args)
        {
            try
            {
                // Get defined file extensions
                string _sourceFileExtension = "log";
                string _filterFileExtension = "def";
                string _resultFileExtension = "txt";

                Logger.LogMessage("Validating parameters...");

                // Check parameters quantity
                if (args.Length != 3)
                    throw new Exception("There are more/less parameters than expected.");

                // Validate files extensions
                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        // Source file
                        case 0:
                            if (args[0].Split('.')[1].ToString() != _sourceFileExtension)
                            {
                                throw new Exception("Source file extension must be [." + _sourceFileExtension + "]\n");
                            }
                            break;
                        // Filter file
                        case 1:
                            if (args[1].Split('.')[1].ToString() != _filterFileExtension)
                            {
                                throw new Exception("Filter file extension must be [." + _filterFileExtension + "]\n");
                            }
                            break;
                        // Result file
                        case 2:
                            if (args[2].Split('.')[1].ToString() != _resultFileExtension)
                            {
                                throw new Exception("Result file extension must be [." + _resultFileExtension + "]\n");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error with the parameters: " + ex.Message +
                    "\nRestart program and make sure the parameters follow this instruction: " +
                    "[SourceFile].log [FilterFile].def [ResultFile].txt\n");
            }
        }

        /// <summary>
        /// Validates if file exists by full path
        /// </summary>
        public static void CheckFileExistsInPath(string fileName, string path)
        {
            try
            {
                Logger.LogMessage("Checking if file " + fileName + " exists... ");

                if (!File.Exists(fileName.Insert(0, path)))
                {
                    throw new Exception("The file " + fileName + " was not found. ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Filter file full validation
        /// </summary>
        public static void ValidateFilterFile(string[] filterLines, List<Filter> filters)
        {
            try
            {
                // Check if file is not empty
                FileIsNotEmpty(filterLines);

                foreach (string line in filterLines)
                {
                    // Check if at least one comma as separator
                    CommaSeparatorExists(line);

                    // Check if only one comma in a line
                    OnlyOneCommaInTheLine(line);

                    // Check if Label is not empty
                    LabelIsNotEmpty(line);

                    // Check if Substring is not empty
                    SearchSubstringIsNotEmpty(line);

                    // Check if Label is not twice
                    LabelIsNotRepeated(line, filters);

                    filters.Add(new Filter
                    {
                        Label = line.Split(',')[0],
                        Substring = line.Split(',')[1]
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates if array is not empty
        /// </summary>
        public static void FileIsNotEmpty(string[] filterLines)
        {
            if (filterLines.Length == 0)
            {
                throw new Exception("The file is empty.");
            }
        }

        /// <summary>
        /// Validates if Label is not repeated
        /// </summary>
        public static void LabelIsNotRepeated(string line, List<Filter> filters)
        {
            string[] splittedByComma = line.Split(',');

            if (filters.Exists(f => f.Label == splittedByComma[0]))
            {
                throw new Exception("There are two or more filters with the label: " + splittedByComma[0]);
            }
        }

        /// <summary>
        /// Validates if Substring field is not empty
        /// </summary>
        public static void SearchSubstringIsNotEmpty(string line)
        {
            if (line.LastIndexOf(',') == line.Length - 1)
            {
                throw new Exception("Sub-string field cannot be empty. Line: " + line);
            }
        }

        /// <summary>
        /// Validates if Label field is not empty
        /// </summary>
        public static void LabelIsNotEmpty(string line)
        {
            if (line.IndexOf(',') == 0)
            {
                throw new Exception("Label cannot be empty. Line: " + line);
            }
        }

        /// <summary>
        /// Validates if there is only one comma by line
        /// </summary>
        public static void OnlyOneCommaInTheLine(string line)
        {
            string[] splittedByComma = line.Split(',');

            if (splittedByComma.Length > 2)
            {
                throw new Exception("It is forbidden to use commas in both label and substring to search.\n" +
                    "Use comma only as separator.");
            }
        }

        /// <summary>
        /// Validates if is at least one comma by line
        /// </summary>
        public static void CommaSeparatorExists(string line)
        {
            string[] splittedByComma = line.Split(',');

            if (splittedByComma.Length < 2)
            {
                throw new Exception("Every line of the Filter must have the following format:" +
                    " <label>,<search sub-string>. Must use comma as separator.");
            }

        }
    }
}
