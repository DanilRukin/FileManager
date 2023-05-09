using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.Interfaces
{
    public interface IDialog
    {
        bool ShowDialog();
        string ParentDirectory { get; set; }
        string SelectedPath { get; set; }
    }
}
