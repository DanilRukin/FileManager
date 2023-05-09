using FileManager.Models.Services.FileRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.UnitTests.FIleManagerTests
{
    public class DefaultTranslateRuleTests
    {
        private DefaultTranslateRule _rule;

        public DefaultTranslateRuleTests() => _rule = new DefaultTranslateRule();

        [Theory]
        [MemberData(nameof(Data))]
        public void Apply_DifferentVariantsOfInputNames_ReturnValidResult(string name, string expectedResult)
        {
            string actual = _rule.Apply(name);

            Assert.Equal(expectedResult, actual);
        }


        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {"имя файла", "imya_fayla"},
                new object[] {"файл1234567", "fayl1234567"}
            };

        [Fact]
        public void Apply_SpecialSymbolsInNames_SaveAllSpecialSymbols()
        {
            char[] specialSymbols = new char[]
            {
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+',
                '_', '{', '}', ',', '.', '/', '\\', '<', '>', '?', '\'', '~', '`',
                '"', '№', ';'
            };
            string patternRu = "имя";
            string patternEng = "imya";
            string name;
            string actual;
            string expected;
            for (int i = 0; i < specialSymbols.Length; i++)
            {
                name = patternRu + specialSymbols[i];
                expected = patternEng + specialSymbols[i];
                actual = _rule.Apply(name);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Apply_NamesContainsOnlyAsciiSymbols_NamesWereNotChanged()
        {
            string actual;
            for (char s = 'a'; s <= 'Z'; s++)
            {
                actual = _rule.Apply(s.ToString());
                Assert.Equal(s.ToString(), actual);
            }
        }

        [Fact]
        public void Apply_NamesContainsDigits_DigitsWereSaved()
        {
            string actual;
            for (char s = '0'; s <= '9'; s++)
            {
                actual = _rule.Apply(s.ToString());
                Assert.Equal(s.ToString(), actual);
            }
        }
    }
}
