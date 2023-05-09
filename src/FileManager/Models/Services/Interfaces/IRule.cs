using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.Interfaces
{
    public interface IRule<T>
    {
        T Apply(T param);
        /// <summary>
        /// Приоритет введен с целью соблюдения конечного результата. Для примера, символ 'я' может транслироваться, как 'ya', т.е. длина имени файла по факту
        /// увеличилась, но должна удовлетворять ограничениям, поэтому <see cref="FileRules.DefaultTranslateRule"/> (и его наследники) имеет наивысший приоритет, т.е. выполняется первым, а уже затем
        /// длина имени файла подгоняется (если надо).
        /// </summary>
        int Priority { get; set; }
    }
}
