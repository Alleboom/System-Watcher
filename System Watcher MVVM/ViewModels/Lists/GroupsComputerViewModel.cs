using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System_Watcher_MVVM.Helpers.Data_Access;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.ViewModels.Lists
{
    /// <summary>
    /// View model class for displaying the group data in a view
    /// </summary>
    public class GroupsComputersViewModel : PropertyChangedBase
    {
        public ObservableCollection<Group> Groups
        {
            get
            {
                return DataAccessMethods.GroupsFromDB();
            }
        }

        private Group _selectedGroup;
        private string _selectedGroupOwner;
        private string _selectedGroupLoc;
        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;
                NotifyOfPropertyChange(() => SelectedGroup);
                NotifyOfPropertyChange(() => Computers);

                if (SelectedGroup != null)
                {
                    SelectedGroupOwner = SelectedGroup.Owner;
                    SelectedGroupLoc = SelectedGroup.BuildingFloor;
                }
                
            }
        }

        public String SelectedGroupOwner
        {
            get
            {
                return _selectedGroupOwner;
            }
            set
            {
                _selectedGroupOwner = value;
                NotifyOfPropertyChange(() => SelectedGroupOwner);
            }
        }

        public String SelectedGroupLoc
        {
            get
            {
                return _selectedGroupLoc;
            }
            set
            {
                _selectedGroupLoc = value;
                NotifyOfPropertyChange(() => SelectedGroupLoc);
            }
        }

        /// <summary>
        /// The list of all computers belonging the selected group
        /// </summary>
        public ObservableCollection<Computer> Computers
        {
            get
            {
                if (SelectedGroup != null)
                {
                    return Helpers.Data_Access.DataAccessMethods.ComputersFromDB(SelectedGroup);
                }
                else
                {
                    return null;
                }
            }
        }

        public void GenerateReportForAll()
        {
            if (SelectedGroup != null)
            {
                var _result = new SaveFileDialog();
                _result.Filter = "Portable Document Format|*.pdf";
                _result.DefaultExt = "pdf";
                var props = new Dictionary<String, String>();

                props.Add("Name", "Name");
                props.Add("LastLogon", "Last Logged on");
                props.Add("EnabledInAD", "Enabled?");
                if (_result.ShowDialog() == DialogResult.OK)
                {
                    Helpers.PDFReportGenerator.GenerateReport<Computer>(DataAccessMethods.ComputersFromDB(), _result.FileName, "All Computers", props);
                }
            }
            else
            {
                MessageBox.Show("Please select a group");
            }
        
        }

        public void GenerateReportForGroup()
        {
            if (SelectedGroup != null)
            {
                var _result = new SaveFileDialog();
                _result.Filter = "Portable Document Format|*.pdf";
                _result.DefaultExt = "pdf";
                var props = new Dictionary<String, String>();

                props.Add("Name", "Name");
                props.Add("LastLogon", "Last Logged on");
                props.Add("EnabledInAD", "Enabled?");
                if (_result.ShowDialog() == DialogResult.OK)
                {
                    Helpers.PDFReportGenerator.GenerateReport<Computer>(Computers, _result.FileName, SelectedGroup.Name, props);
                }
            }
            else
            {
                MessageBox.Show("Please select a group");
            }
        }
    }
}
