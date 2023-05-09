using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.FileRules
{
    public class LimitFileNameLengthRule : FileRule
    {
        private int _length;
        public LimitFileNameLengthRule(int length)
        {
            _length = length;
        }
        public override string Apply(string param)
        {
            int legalLen;
            if (_length < 1 || _length > param.Length)
                legalLen = param.Length;
            else
                legalLen = _length;
            return param.Substring(0, legalLen);
        }
    }
}
