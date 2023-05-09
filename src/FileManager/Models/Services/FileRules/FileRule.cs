using FileManager.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.FileRules
{
    public abstract class FileRule : IRule<string>
    {
        public virtual int Priority { get; set; } = FileRuleConstants.MIN_PRIORITY;

        public abstract string Apply(string param);
    }
}
