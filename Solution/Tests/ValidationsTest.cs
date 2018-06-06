using LoggerTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerTest.Tests
{
    [TestClass]
    public class ValidationsTest
    {
        List<string> filterLines = new List<string>();
        List<Filter> filters = new List<Filter>();

        [TestMethod()]
        public void ValidateFilterFileTest()
        {
            try
            {
                filterLines.Add("ResponseOK,Response OK/ OK");
                filterLines.Add("ErrorLine,ERROR");
                filterLines.Add("Thread # 29 Count Log Lines,[29]");

                Validations.ValidateFilterFile(filterLines.ToArray(), filters);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ValidateParametersTest()
        {
            try
            {
                string[] arguments = { "OrderLoader.log", "Filter1.def", "Filter1Result.txt" };

                Validations.ValidateParameters(arguments);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void CommaSeparatorExistsTest()
        {
            try
            {
                Validations.CommaSeparatorExists("ThereAre.NoCommas");
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod()]
        public void OnlyOneCommaInTheLineTest()
        {
            try
            {
                Validations.OnlyOneCommaInTheLine("W,r,o,n,g,,,F,o,r,m,a,t");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod()]
        public void LabelIsNotEmptyTest()
        {
            try
            {
                Validations.LabelIsNotEmpty(",SearchSubtring");
                Assert.Fail();
            }
            catch
            {
            }
        }


        [TestMethod()]
        public void SearchSubstringIsNotEmptyTest()
        {
            try
            {
                Validations.SearchSubstringIsNotEmpty("Label,");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod()]
        public void LabelIsNotRepeatedTest()
        {
            try
            {
                filters.Add(new Filter
                {
                    Label = "Label",
                    Substring = "SearchSubtring"
                });
                Validations.LabelIsNotRepeated("Label,Using same label for second time", filters);
                Assert.Fail();
            }
            catch
            {
            }
        }
    }
}
