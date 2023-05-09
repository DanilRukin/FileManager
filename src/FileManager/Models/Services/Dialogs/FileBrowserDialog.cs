using FileManager.Models.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models.Services.Dialogs
{
    public class FileBrowserDialog : IDialog
    {
        private string _parentDirectory = Environment.CurrentDirectory;
        public string ParentDirectory { get => _parentDirectory; set => _parentDirectory = value; }
        private string _selectedPath = "";
        public string SelectedPath { get => _selectedPath; set => _selectedPath = value; }
        public string[] SelectedFiles { get; private set; } = new string[0];
        public bool ShowDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                SelectedFiles = dialog.FileNames;
                SelectedPath = SelectedFiles[0].Substring(0, SelectedFiles[0].LastIndexOf("\\") + 1);
                ParentDirectory = SelectedPath;
                for (int i = 0; i < SelectedFiles.Length; i++)
                {
                    SelectedFiles[i] = SelectedFiles[i].Substring(SelectedFiles[i].LastIndexOf("\\") + 1);
                }
                SelectedPath = dialog.FileNames[0];
                return true;
            }
            return false;
        }
    }
}
