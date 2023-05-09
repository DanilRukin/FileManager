using FileManager.Models.Services.Executors;
using FileManager.Models.Services.FileRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.UnitTests.FIleManagerTests
{
    public class FileRuleExecutorTests
    {
        private FileRuleExecutor _executor;

        public FileRuleExecutorTests() => _executor = new FileRuleExecutor();

        [Fact]
        public void Invoke_UseRulesPriority_ApplyLimitFileNameLengthRuleAfterTranslateRule()
        {
            DefaultTranslateRule translateRule = new DefaultTranslateRule();
            int length = 7;
            LimitFileNameLengthRule limitRule = new LimitFileNameLengthRule(length);
            string fileName = "имя файла";
            string translatedName = translateRule.Apply(fileName);
            string limitedName = limitRule.Apply(translatedName);
            _executor.Add(limitRule);
            _executor.Add(translateRule);

            string actual = _executor.Invoke(fileName);

            Assert.Equal(limitedName, actual);

            _executor.Clear();
        }

        [Theory]
        [MemberData(nameof(FileNames))]
        public void Invoke_NoRules_FileNameWithDirectoryAndExtension_ParseTheNameCorrectly(string name)
        {
            string actual = _executor.Invoke(name);

            Assert.Equal(name, actual);
        }

        [Theory]
        [MemberData(nameof(FileNames))]
        public void Invoke_OnlyTranslateRule_FileNameWithDirectoryAndExtension_ParseTheNameCorrectly(string name)
        {
            DefaultTranslateRule rule = new DefaultTranslateRule();
            _executor.Add(rule);
            string actual = _executor.Invoke(name);           
            Assert.Equal(name, actual);
        }

        public static IEnumerable<object[]> FileNames =>
            new List<object[]>
            {
                new object[] {""},
                new object[] {@"C:\Users\Rukin\source\repos\FileManager\FileManager.sln"},
                new object[] {@"C:\Users\Rukin\source\repos\FileManager\FileManager.sln.sln"},
                new object[] {@"C:\FileManager.sln"},
                new object[] {@"C:\FileManager.sln.ext"},
            };
    }
}
