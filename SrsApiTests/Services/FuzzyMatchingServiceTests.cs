using SrsApi.Enums;
using SrsApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrsApiTests.Services
{
    //AI generated via deepseek

    [TestClass]
    public class FuzzyMatchingServiceTests
    {
        private readonly FuzzyMatchingService _fuzzyMatchingService;

        public FuzzyMatchingServiceTests()
        {
            _fuzzyMatchingService = new FuzzyMatchingService();
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_ExactMatch_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello";
            string compareString = "hello";
            var method = FuzzySearchMethod.ExactMatch;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.AreEqual(1, ratio);
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_ExactMatch_CaseInsensitive_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "HELLO";
            string compareString = "hello";
            var method = FuzzySearchMethod.ExactMatch;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.AreEqual(1, ratio);
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_SimpleRatio_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello";
            string compareString = "helo";
            var method = FuzzySearchMethod.SimpleRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.IsTrue(ratio > 0); // Simple ratio should be greater than 0 for similar strings
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_PartialRatio_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello world";
            string compareString = "world";
            var method = FuzzySearchMethod.PartialRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.IsTrue(ratio > 0); // Partial ratio should be greater than 0 for partial matches
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_TokenSortRatio_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello world";
            string compareString = "world hello";
            var method = FuzzySearchMethod.TokenSortRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.IsTrue(ratio > 0); // Token sort ratio should be greater than 0 for tokenized matches
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_TokenSetRatio_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello world";
            string compareString = "world hello extra";
            var method = FuzzySearchMethod.TokenSetRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.IsTrue(ratio > 0); // Token set ratio should be greater than 0 for tokenized matches
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_WeightedRatio_ReturnsCorrectRatio()
        {
            // Arrange
            string userInput = "hello world";
            string compareString = "helo world";
            var method = FuzzySearchMethod.WeightedRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.IsTrue(ratio > 0); // Weighted ratio should be greater than 0 for weighted matches
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_InvalidMethod_ReturnsZero()
        {
            // Arrange
            string userInput = "hello";
            string compareString = "world";
            var method = (FuzzySearchMethod)999; // Invalid method

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.AreEqual(0, ratio); // Should return 0 for invalid methods
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_NullInput_ReturnsZero()
        {
            // Arrange
            string userInput = null;
            string compareString = "hello";
            var method = FuzzySearchMethod.SimpleRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.AreEqual(0, ratio); // Should return 0 for null input
        }

        [TestMethod]
        public void GetFuzzyMatchRatio_NullCompareString_ReturnsZero()
        {
            // Arrange
            string userInput = "hello";
            string compareString = null;
            var method = FuzzySearchMethod.SimpleRatio;

            // Act
            int ratio = _fuzzyMatchingService.GetFuzzyMatchRatio(userInput, compareString, method);

            // Assert
            Assert.AreEqual(0, ratio); // Should return 0 for null compare string
        }
    }
}
