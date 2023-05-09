using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.Interfaces
{
    public interface IRuleExecutor<T>
    {
        bool Add(IRule<T> rule);
        bool Remove(IRule<T> rule);
        T Invoke(T param);
    }
}
