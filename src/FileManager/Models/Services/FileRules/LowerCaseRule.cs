using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.FileRules
{
    public class LowerCaseRule : FileRule
    {
        public override string Apply(string param)
        {
            return param.ToLower();
        }
    }
}
