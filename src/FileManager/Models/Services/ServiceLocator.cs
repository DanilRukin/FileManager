using FileManager.Models.Services.Dialogs;
using FileManager.Models.Services.Executors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services
{
    public class ServiceLocator
    {
        public FileRuleExecutor FileRuleExecutor => App.Services.GetRequiredService<FileRuleExecutor>();

        public FileBrowserDialog FileBrowserDialog => App.Services.GetRequiredService<FileBrowserDialog>();

        public FolderBrowserDialog FolderBrowserDialog => App.Services.GetRequiredService<FolderBrowserDialog>();
    }
}
