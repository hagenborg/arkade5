using System.IO;
using Arkivverket.Arkade.Util;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;

namespace Arkivverket.Arkade.UI.ViewModels
{
    public class LoadArchiveExtractionViewModel : BindableBase, INavigationAware
    {
        private ILogger _log = Log.ForContext<LoadArchiveExtractionViewModel>();

        private readonly IRegionManager _regionManager;
        private string _archiveFileName;
        private string _metadataFileName;

        public string MetadataFileName
        {
            get { return _metadataFileName; }
            set
            {
                SetProperty(ref _metadataFileName, value);
                NavigateCommand.RaiseCanExecuteChanged();
            }
        }

        public string ArchiveFileName
        {
            get { return _archiveFileName; }
            set
            {
                SetProperty(ref _archiveFileName, value);
                NavigateCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand NavigateCommand { get; set; }
        public DelegateCommand OpenMetadataFileCommand { get; set; }
        public DelegateCommand OpenArchiveFileCommand { get; set; }
        public DelegateCommand OpenArchiveFolderCommand { get; set; }

        public LoadArchiveExtractionViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            OpenMetadataFileCommand = new DelegateCommand(OpenMetadataFileDialog);
            OpenArchiveFileCommand = new DelegateCommand(OpenArchiveFileDialog);
            OpenArchiveFolderCommand = new DelegateCommand(OpenArchiveFolderDialog);
            NavigateCommand = new DelegateCommand(Navigate, CanRunTests);
        }

        private void Navigate()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("archiveFileName", ArchiveFileName);
            navigationParameters.Add("metadataFileName", MetadataFileName);

            _log.Debug("Navigating to TestRunner window with archive file {ArchiveFile} and metadata file {MetadataFile}", ArchiveFileName, MetadataFileName);

            _regionManager.RequestNavigate("MainContentRegion", "TestRunner", navigationParameters);
        }

        private bool CanRunTests()
        {
            return !string.IsNullOrEmpty(_archiveFileName) && !string.IsNullOrEmpty(_metadataFileName);
        }

        private void OpenMetadataFileDialog()
        {
            MetadataFileName = OpenFileDialog();
        }

        private void OpenArchiveFileDialog()
        {
            ArchiveFileName = OpenFileDialog();

            if (ArchiveFileName == null)
            {
                MetadataFileName = null;
                return;
            }

            string infoXmlFileName = Path.Combine(new FileInfo(ArchiveFileName).Directory?.FullName, ArkadeConstants.InfoXmlFileName);
            if (File.Exists(infoXmlFileName))
            {
                MetadataFileName = infoXmlFileName;
            }
            else
            {
                MetadataFileName = null;
            }
        }

        private void OpenArchiveFolderDialog()
        {
            ArchiveFileName = OpenFolderDialog();

            if (ArchiveFileName == null)
            {
                MetadataFileName = null;
                return;
            }

            string infoXmlFileName = Path.Combine(new DirectoryInfo(ArchiveFileName).Parent?.FullName, ArkadeConstants.InfoXmlFileName);
            if (File.Exists(infoXmlFileName))
            {
                MetadataFileName = infoXmlFileName;
            }
            else
            {
                MetadataFileName = null;
            }
        }


        private string OpenFolderDialog()
        {
            string selected = null;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = false
                //Title = ""
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                selected = dialog.FileName;
            }

            return selected;
        }

        private string OpenFileDialog()
        {
            string selectedFileName = null;
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result == true)
            {
                selectedFileName = dialog.FileName;
            }
            return selectedFileName;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}