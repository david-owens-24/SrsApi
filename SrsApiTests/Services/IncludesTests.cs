using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using SrsApi.DbContext;
using SrsApi.Enums;
using SrsApi.Interfaces;
using SrsApi.Services;
using SrsApi.Services.Helpers;
using SrsApiTests.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SrsApiTests.Services
{
    [TestClass]
    public class IncludesTests
    {
        //TODO: update these to use reflection to confirm the actual type? not sure if really needed though

        #region SrsItemIncludesHelper tests

        [TestMethod]
        public void SrsItem_GetIncludesHasAllExpectedProperties()
        {        
            List<string> includesStrings = new List<string>
            {
                "Answers",
                "Level",
                "Details"
            };

            var includes = SrsItemIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(includes.Count(), 3);
            
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswer")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemLevel")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemDetails")));
        }

        [TestMethod]
        public void SrsItem_GetIncludesOnlyReturnsExpectedIncludes()
        {
            List<string> includesStrings = new List<string>
            {
                "Answers"
            };

            var includes = SrsItemIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(includes.Count(), 1);

            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswer")));
        }

        [TestMethod]
        public void SrsItem_GetIncludesSplitsStringCorrectly()
        {
            string includesStrings = "Answers,Level,Details";

            var includes = SrsItemIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(includes.Count(), 3);

            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswer")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemLevel")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemDetails")));
        }

        [TestMethod]
        public void SrsItem_GetIncludesIgnoresUnexpectedStrings()
        {
            string includesStrings = "Answers,UNEXPECTED_STRING,Level,Details";

            var includes = SrsItemIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(includes.Count(), 3);

            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswer")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemLevel")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsItemDetails")));
        }

        [TestMethod]
        public void SrsItem_GetIncludesReturnsEmptyListByDefault()
        {  
            var includes = SrsItemIncludesHelper.GetIncludes("");

            Assert.AreEqual(includes.Count(), 0);
        }

        #endregion

        #region SrsAnswerIncludesHelper tests
        [TestMethod]
        public void SrsAnswer_GetIncludesHasAllExpectedProperties()
        {
            List<string> includesStrings = new List<string>
            {
                "SearchMethods"
            };

            var includes = SrsAnswerIncludesHelper.GetIncludes(includesStrings);

            // We're expecting two includes: one for SrsAnswerFuzzySearchMethod
            // and one for SrsFuzzySearchMethod, which is nested within SrsAnswerFuzzySearchMethod
            Assert.AreEqual(2, includes.Count());
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswerFuzzySearchMethod")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsFuzzySearchMethod")));
        }


        // Not needed as there is only one include for SearchMethods
        //[TestMethod]
        //public void SrsAnswer_GetIncludesOnlyReturnsExpectedIncludes()
        //{
        //    List<string> includesStrings = new List<string>
        //    {
        //        "SearchMethods"
        //    };

        //    var includes = SrsAnswerIncludesHelper.GetIncludes(includesStrings);

        //    Assert.AreEqual(2, includes.Count());
        //    Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswerFuzzySearchMethod")));
        //}

        // Not needed as there is only one include for SearchMethods
        //[TestMethod]
        //public void SrsAnswer_GetIncludesSplitsStringCorrectly()
        //{
        //    string includesStrings = "SearchMethods";

        //    var includes = SrsAnswerIncludesHelper.GetIncludes(includesStrings);

        //    Assert.AreEqual(2, includes.Count());
        //    Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswerFuzzySearchMethod")));
        //    Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsFuzzySearchMethod")));
        //}

        [TestMethod]
        public void SrsAnswer_GetIncludesIgnoresUnexpectedStrings()
        {
            string includesStrings = "SearchMethods,UNEXPECTED_STRING";

            var includes = SrsAnswerIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(2, includes.Count());
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsAnswerFuzzySearchMethod")));
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsFuzzySearchMethod")));
        }

        [TestMethod]
        public void SrsAnswer_GetIncludesReturnsEmptyListByDefault()
        {
            var includes = SrsAnswerIncludesHelper.GetIncludes("");

            Assert.AreEqual(includes.Count(), 0);
        }
        #endregion

        #region SrsAnswerFuzzySearchMethodIncludesHelper tests
        [TestMethod]
        public void SrsAnswerFuzzySearchMethod_GetIncludesHasAllExpectedProperties()
        {
            List<string> includesStrings = new List<string>
            {
                "SearchMethod"
            };

            var includes = SrsAnswerFuzzySearchMethodIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(1, includes.Count());
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsFuzzySearchMethod")));
        }        

        [TestMethod]
        public void SrsAnswerFuzzySearchMethod_GetIncludesIgnoresUnexpectedStrings()
        {
            string includesStrings = "SearchMethod,UNEXPECTED_STRING";

            var includes = SrsAnswerFuzzySearchMethodIncludesHelper.GetIncludes(includesStrings);

            Assert.AreEqual(1, includes.Count());
            Assert.IsTrue(includes.Any(x => x.Body.Type.FullName.ToString().Contains("SrsFuzzySearchMethod")));
        }

        [TestMethod]
        public void SrsAnswerFuzzySearchMethod_GetIncludesReturnsEmptyListByDefault()
        {
            var includes = SrsAnswerFuzzySearchMethodIncludesHelper.GetIncludes("");

            Assert.AreEqual(includes.Count(), 0);
        }
        #endregion
    }
}
