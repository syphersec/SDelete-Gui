using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using Microsoft.Win32;

namespace SDelete_Gui.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; RaisePropertyChanged("ErrorMessage"); }
        }

        public ICommand ConfigureCommand { get; set; }
        public ICommand UnConfigureCommand { get; set; }


        public MainViewModel()
        {
            ConfigureCommand = new RelayCommand(() => ExecuteConfigure());
        }

        private void ExecuteConfigure()
        {
            if (AddContextMenuToFiles("Secure Delete", "sdelete -p 10 -s -q \"%1\""))
            {
                ErrorMessage = "Configured";
            }
            else
            {
                ErrorMessage = "Removed";
            };
        }


        private bool AddContextMenuToFiles(string name, string command)
        {
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            bool result;
            try
            {
                string keyName = "*\\shell\\" + name;
                regmenu = Registry.ClassesRoot.CreateSubKey(keyName);
                regcmd = Registry.ClassesRoot.CreateSubKey(keyName + "\\command");
                regcmd.SetValue("", command);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Dispose();
                if (regcmd != null)
                    regcmd.Dispose();
            }

            return result;

        }
        private bool RemoveContextMenuOfFiles(string name)
        {
            try
            {
                string keyName = "*\\shell\\" + name;
                Registry.ClassesRoot.DeleteSubKeyTree(keyName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}