using FileManager.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.Executors
{
    public class FileRuleExecutor : IRuleExecutor<string>
    {
        public FileRuleExecutor()
        {
            rules = new List<IRule<string>>();
        }
        protected List<IRule<string>> rules;

        public bool Add(IRule<string> rule)
        {
            if (rules.Contains(rule))
                return false;
            rules.Add(rule);
            return true;
        }

        public bool Remove(IRule<string> rule)
        {
            if (!rules.Contains(rule))
                return false;
            rules.Remove(rule);
            return true;
        }

        public void Clear() => rules?.Clear();

        public string Invoke(string param)
        {
            string directory, name, ext;
            int directoryEndIndex = param.LastIndexOf("\\") + 1;
            if (directoryEndIndex < 0)
                directory = "";
            else
                directory = param.Substring(0, directoryEndIndex);
            int extStartIndex = param.LastIndexOf('.');
            if (extStartIndex < 0)
                ext = "";
            else
                ext = param.Substring(extStartIndex);
            name = param.Substring(directory.Length,
                param.Length - directory.Length - ext.Length);
            rules.Sort((first, second) =>
            {
                if (first.Priority < second.Priority) return 1;
                if (first.Priority > second.Priority) return -1;
                return 0;
            });
            foreach (var rule in rules)
            {
                name = rule.Apply(name);
            }
            return directory + name + ext;
        }
    }
}
