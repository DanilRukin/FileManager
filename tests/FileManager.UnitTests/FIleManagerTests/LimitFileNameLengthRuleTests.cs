using FileManager.Models.Services.FileRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.UnitTests.FIleManagerTests
{
    public class LimitFileNameLengthRuleTests
    {
        [Fact]
        public void Apply_FileNameLengthIsLessThanRuleParameter_ReturnNotLimitedFileName()
        {
            string fileName = "fileName";
            int length = fileName.Length + 1;
            LimitFileNameLengthRule rule = new LimitFileNameLengthRule(length);

            Assert.Equal(fileName.Length, rule.Apply(fileName).Length);
        }

        [Fact]
        public void Apply_FileNameLengthIsMoreThanRuleParameter_ReturnLimitedFileName()
        {
            string fileName = "fileName";
            int length = fileName.Length - 1;
            LimitFileNameLengthRule rule = new LimitFileNameLengthRule(length);

            Assert.Equal(length, rule.Apply(fileName).Length);
        }

        [Fact]
        public void Apply_RuleParameterIsLessThanOne_ReturnNotLimitedFileName()
        {
            string fileName = "fileName";
            int length = 0;
            LimitFileNameLengthRule rule = new LimitFileNameLengthRule(length);

            Assert.Equal(fileName.Length, rule.Apply(fileName).Length);
        }

        [Theory]
        [InlineData("fileName", 8, 8)]
        [InlineData("fileName", 7, 7)]
        [InlineData("fileName", 6, 6)]
        [InlineData("fileName", 5, 5)]
        [InlineData("fileName", 4, 4)]
        [InlineData("fileName", 3, 3)]
        [InlineData("fileName", 2, 2)]
        [InlineData("fileName", 1, 1)]
        [InlineData("fileName", 9, 8)]
        [InlineData("fileName", -1, 8)]
        [InlineData("fileName", 0, 8)]
        public void Apply_LimitFileNamesCorrectly(string name, int ruleParameter, int expectedLength)
        {
            LimitFileNameLengthRule rule = new LimitFileNameLengthRule(ruleParameter);
            Assert.Equal(expectedLength, rule.Apply(name).Length);
        }
    }
}
