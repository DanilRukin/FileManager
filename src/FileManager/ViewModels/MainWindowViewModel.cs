using FileManager.Infrastructure.Commands;
using FileManager.Models.Data;
using FileManager.Models.Services;
using FileManager.Models.Services.Dialogs;
using FileManager.Models.Services.FileRules;
using FileManager.Models.Services.Interfaces;
using FileManager.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Linq;

namespace FileManager.ViewModels
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            _services = new ServiceLocator();
            ChooseFolderCommand = new LambdaCommand(OnChooseFolderCommandExecuted, CanChooseFolderCommandExecute);
            LoadDataCommand = new LambdaCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
            ClearTableCommand = new LambdaCommand(OnClearTableCommandExecuted, CanClearTableCommandExecute);
            ApplyToAllCommand = new LambdaCommand(OnApplyToAllCommandExecuted, CanApplyToAllCommandExecute);
            ApplyToSelectedCommand = new LambdaCommand(OnApplyToSelectedCommandExecuted, CanApplyToSelectedCommandExecute);
            ChooseFileWithTranslateTableCommand = new LambdaCommand(OnChooseFileWithTranslateTableCommandExecuted,
                CanChooseFileWithTranslateTableCommandExecute);
        }

        private ServiceLocator _services;

        #region Properties
        private string _title = "FileManager";
        public string Title { get => _title; set => Set(ref _title, value); }

        private string _status = "Status";
        public string Status { get => _status; set => Set(ref _status, value); }

        private string _selectedFolder = "";
        public string SelectedFolder { get => _selectedFolder; set => Set(ref _selectedFolder, value); }

        public ObservableCollection<FileNames> FilesTable { get; private set; } = new ObservableCollection<FileNames>();
        private FileNames _selectedFile;
        public FileNames SelectedFile
        {
            get => _selectedFile;
            set
            {
                Set(ref _selectedFile, value);
                if (_selectedFile == null)
                {
                    ClearAttributes();
                }
                else
                {
                    SetAttributes();
                }
            }
        }

        private string _pathToTranslateFile = "Путь";
        public string PathToTranslateFile { get => _pathToTranslateFile; set => Set(ref _pathToTranslateFile, value); }

        private string _pathToAnotherFolder = "Путь";
        public string PathToAnotherFolder { get => _pathToAnotherFolder; set => Set(ref _pathToAnotherFolder, value); }

        public ObservableCollection<string> LogMessages { get; private set; } = new ObservableCollection<string>();

        #region FileAttributesCheckboxes
        private bool _archived = false;
        public bool Archived
        {
            get => _archived;
            set
            {
                Set(ref _archived, value);
            }
        }

        private bool _system = false;
        public bool System { get => _system; set => Set(ref _system, value); }

        private bool _hidden = false;
        public bool Hidden { get => _hidden; set => Set(ref _hidden, value); }

        private bool _normal = false;
        public bool Normal { get => _normal; set => Set(ref _normal, value); }

        private bool _temporary = false;
        public bool Temporary { get => _temporary; set => Set(ref _temporary, value); }

        private bool _compressed = false;
        public bool Compressed { get => _compressed; set => Set(ref _compressed, value); }

        private bool _encrypted = false;
        public bool Encrypted { get => _encrypted; set => Set(ref _encrypted, value); }

        private bool _readOnly = false;
        public bool ReadOnly { get => _readOnly; set => Set(ref _readOnly, value); }

        private bool _integrityStream = false;
        public bool IntegrityStream { get => _integrityStream; set => Set(ref _integrityStream, value); }

        private bool _noScrubData = false;
        public bool NoScrubData { get => _noScrubData; set => Set(ref _noScrubData, value); }

        private bool _offline = false;
        public bool Offline { get => _offline; set => Set(ref _offline, value); }

        private bool _sparseFile = false;
        public bool SparseFile { get => _sparseFile; set => Set(ref _sparseFile, value); }

        #endregion

        #region RadioButtons

        private bool _renameOnly = true;
        public bool RenameOnly
        {
            get => _renameOnly;
            set
            {
                Set(ref _renameOnly, value);
            }
        }

        private bool _saveRegister = true;
        public bool SaveRegister
        {
            get => _saveRegister;
            set
            {
                UpperCase = false;
                LowerCase = false;
                Set(ref _saveRegister, value);
            }
        }

        private bool _changeRegister = false;
        public bool ChangeRegister
        {
            get => _changeRegister;
            set
            {
                Set(ref _changeRegister, value);
                UpperCase = ChangeRegister;
                LowerCase = false;
            }
        }

        private bool _upperCase = false;
        public bool UpperCase { get => _upperCase; set => Set(ref _upperCase, value); }

        private bool _lowerCase = false;
        public bool LowerCase { get => _lowerCase; set => Set(ref _lowerCase, value); }

        private bool _limit = false;
        public bool Limit { get => _limit; set => Set(ref _limit, value); }

        private bool _useDefaultTranslator = true;
        public bool UseDefaultTranslator { get => _useDefaultTranslator; set => Set(ref _useDefaultTranslator, value); }

        private bool _useTranslatorFromFile = false;
        public bool UseTranslatorFromFile { get => _useTranslatorFromFile; set => Set(ref _useTranslatorFromFile, value); }

        #endregion


        private int _limitLength = 10;
        public int LimitLength { get => _limitLength; set => Set(ref _limitLength, value); }

        #endregion

        #region Commands

        #region ChooseFolderCommand
        public ICommand ChooseFolderCommand { get; }
        private void OnChooseFolderCommandExecuted(object p)
        {
            IDialog dialog = _services.FolderBrowserDialog;
            if (dialog.ShowDialog() == true)
            {
                SelectedFolder = dialog.SelectedPath;
            }
        }
        private bool CanChooseFolderCommandExecute(object p) => true;
        #endregion

        #region ChooseFileWithTranslateTableCommand
        public ICommand ChooseFileWithTranslateTableCommand { get; }
        private void OnChooseFileWithTranslateTableCommandExecuted(object p)
        {
            IDialog fileDialog = _services.FileBrowserDialog;

            if (fileDialog.ShowDialog() == true)
            {
                PathToTranslateFile = fileDialog.SelectedPath;
            }
        }
        private bool CanChooseFileWithTranslateTableCommandExecute(object p) => UseTranslatorFromFile;
        #endregion


        #region ApplyToAllCommand
        public ICommand ApplyToAllCommand { get; }
        private void OnApplyToAllCommandExecuted(object p)
        {
            try
            {
                LogMessages.Add($"Start renaming...\r\n");
                IRuleExecutor<string> ruleExecutor = ConfigureExecutor();
                string res;
                foreach (var name in FilesTable)
                {
                    try
                    {
                        res = ruleExecutor.Invoke(name.Location + name.Name);
                        File.Move(name.Location + name.Name, res);
                        LogMessages.Add($"Файл '{name.Location + name.Name}' переименован в '{res}'\r\n\r\n");
                        name.Location = res.Substring(0, res.LastIndexOf("\\") + 1);
                        name.Name = res.Substring(res.LastIndexOf("\\") + 1);
                    }
                    catch (FileNotFoundException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (ArgumentNullException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (ArgumentException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (PathTooLongException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (NotSupportedException e)
                    {
                        LogError(e.Message + "не критично");
                    }
                    catch (IOException)
                    {
                        throw;
                    }
                }
                LogMessages.Add($"Completed\r\n");
            }
            catch (Exception e)
            {
                LogError(e.Message + " FATAL");
            }
        }
        private bool CanApplyToAllCommandExecute(object p) => FilesTable.Count > 0;
        #endregion

        #region ApplyToSelectedCommand
        public ICommand ApplyToSelectedCommand { get; }
        private void OnApplyToSelectedCommandExecuted(object p)
        {
            try
            {
                IRuleExecutor<string> ruleExecutor = ConfigureExecutor();
                string res;
                res = ruleExecutor.Invoke(SelectedFile.Location + SelectedFile.Name);
                File.Move(SelectedFile.Location + SelectedFile.Name, res);
                LogMessages.Add($"Файл '{SelectedFile.Location + SelectedFile.Name}' переименован в '{res}'\r\n\r\n");
                int index = FilesTable.IndexOf(SelectedFile);
                SelectedFile = null;
                FilesTable[index].Location = res.Substring(0, res.LastIndexOf("\\") + 1);
                FilesTable[index].Name = res.Substring(res.LastIndexOf("\\") + 1);
            }
            catch (Exception e)
            {
                LogError(e.Message);
            }
        }
        private bool CanApplyToSelectedCommandExecute(object p) => SelectedFile != null;
        #endregion

        #region LoadDataCommand
        public ICommand LoadDataCommand { get; }
        private void OnLoadDataCommandExecuted(object p)
        {
            if (string.IsNullOrWhiteSpace(SelectedFolder))
            {
                FileBrowserDialog dialog = _services.FileBrowserDialog;
                if (dialog.ShowDialog() == true)
                {
                    FilesTable.Clear();
                    for (int i = 0; i < dialog.SelectedFiles.Length; i++)
                    {
                        FilesTable.Add(new FileNames(dialog.SelectedFiles[i], dialog.SelectedPath));
                    }
                }
            }
            else
            {
                if (Directory.Exists(SelectedFolder))
                {
                    string[] files = Directory.GetFiles(SelectedFolder);
                    FilesTable.Clear();
                    if (files != null)
                    {
                        string path = files[0].Substring(0, files[0].LastIndexOf("\\") + 1);
                        for (int i = 0; i < files.Length; i++)
                        {
                            FilesTable.Add(new FileNames(files[i].Substring(files[i].LastIndexOf("\\") + 1), path));
                        }
                    }
                }
                else
                {
                    Status = "Директория не существует";
                }
            }
        }
        private bool CanLoadDataCommandExecute(object p) => true;
        #endregion

        #region ClearTableCommand
        public ICommand ClearTableCommand { get; }
        private void OnClearTableCommandExecuted(object p)
        {
            FilesTable.Clear();
        }
        private bool CanClearTableCommandExecute(object p) => FilesTable.Count > 0;
        #endregion
        #endregion

        private void SetAttributes()
        {
            string path = GetFullName(SelectedFile);
            if (path != null)
            {
                Archived = (File.GetAttributes(path) & FileAttributes.Archive) == FileAttributes.Archive;
                System = (File.GetAttributes(path) & FileAttributes.System) == FileAttributes.System;
                Hidden = (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden;
                Compressed = (File.GetAttributes(path) & FileAttributes.Compressed) == FileAttributes.Compressed;
                Normal = (File.GetAttributes(path) & FileAttributes.Normal) == FileAttributes.Normal;
                Temporary = (File.GetAttributes(path) & FileAttributes.Temporary) == FileAttributes.Temporary;
                ReadOnly = (File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                Encrypted = (File.GetAttributes(path) & FileAttributes.Encrypted) == FileAttributes.Encrypted;
                IntegrityStream = (File.GetAttributes(path) & FileAttributes.IntegrityStream) == FileAttributes.IntegrityStream;
                NoScrubData = (File.GetAttributes(path) & FileAttributes.NoScrubData) == FileAttributes.NoScrubData;
                Offline = (File.GetAttributes(path) & FileAttributes.Offline) == FileAttributes.Offline;
                SparseFile = (File.GetAttributes(path) & FileAttributes.SparseFile) == FileAttributes.SparseFile;
            }
        }

        private string GetFullName(FileNames file)
        {
            if (file != null)
                return file.Location + file.Name;
            return null;
        }

        private void ClearAttributes()
        {
            Archived = false;
            System = false;
            Hidden = false;
            Compressed = false;
            Normal = false;
            Temporary = false;
            ReadOnly = false;
            Encrypted = false;
            IntegrityStream = false;
            NoScrubData = false;
            Offline = false;
            SparseFile = false;
        }

        private IRuleExecutor<string> ConfigureExecutor()
        {
            IRuleExecutor<string> ruleExecutor = _services.FileRuleExecutor;
            if (UpperCase)
                ruleExecutor.Add(new UpperCaseRule());
            if (LowerCase)
                ruleExecutor.Add(new LowerCaseRule());
            if (Limit)
                ruleExecutor.Add(new LimitFileNameLengthRule(LimitLength));
            if (UseDefaultTranslator)
                ruleExecutor.Add(new DefaultTranslateRule());
            else
                ruleExecutor.Add(new TranslateRule(PathToTranslateFile));
            return ruleExecutor;
        }

        private void LogError(string message)
        {
            LogMessages.Add($"\r\n\\============Ошибка============/\r\n{message}\r\n/===========Конец ошибки=========\\\r\n");
        }
    }
}
