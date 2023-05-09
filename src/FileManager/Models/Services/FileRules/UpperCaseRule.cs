using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.FileRules
{
    class UpperCaseRule : FileRule
    {
        public override string Apply(string param)
        {
            return param.ToUpper();
        }
    }
}
