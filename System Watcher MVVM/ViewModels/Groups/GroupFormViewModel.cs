using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.ViewModels.Groups
{
    /// <summary>
    /// Base view model class for adding or editing groups of computers and their properties
    /// </summary>
    public class GroupFormViewModel : PropertyChangedBase
    {
        private string _newName;
        private string _newOwnerName;
        private string _newLocation;
        private ObservableCollection<Computer> _newComputers;

        public GroupFormViewModel()
        {
            NewComputers = new ObservableCollection<Computer>();
        }

        public String NewName
        {
            get
            {
                return _newName;
            }

            set
            {
                _newName = value;
                NotifyOfPropertyChange(() => NewName);
            }
        }

        public String NewOwnerName
        {
            get
            {
                return _newOwnerName;
            }
            set
            {
                _newOwnerName = value;
                NotifyOfPropertyChange(() => NewOwnerName);
            }
        }

        public String NewLocation
        {
            get
            {
                return _newLocation;
            }
            set
            {
                _newLocation = value;
                NotifyOfPropertyChange(() => NewLocation);
            }
        }

        public ObservableCollection<Computer> NewComputers
        {
            get
            {
                return _newComputers;
            }
            set
            {
                _newComputers = value;
                NotifyOfPropertyChange(() => NewComputers);
            }

        }

        /// <summary>
        /// Opens a file dialog and parses through each line to add to our view
        /// </summary>
        public void ImportList()
        {
            NewComputers.Clear();
            try
            {
                var fd = new System.Windows.Forms.OpenFileDialog();
                fd.Filter = "Text Files | *.txt";
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (System.IO.TextReader tr = new System.IO.StreamReader(fd.FileName))
                    {
                        string _line = string.Empty;
                        do
                        {
                            _line = tr.ReadLine();
                            if (_line != null)
                            {
                                NewComputers.Add(new Computer()
                                {
                                    Name = _line
                                });
                            }
                        }
                        while (_line != null);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
