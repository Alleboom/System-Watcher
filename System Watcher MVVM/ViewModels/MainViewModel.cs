using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Windows.Forms;
using System_Watcher_MVVM.Models;
using System.Reflection;

namespace System_Watcher_MVVM.ViewModels
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : PropertyChangedBase 
    {

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            CurrentCount = 0;
            GroupsComputersVM = new Lists.GroupsComputersViewModel();
            
        }


        private readonly IWindowManager _windowManager;
        public Lists.GroupsComputersViewModel GroupsComputersVM { get; set; }



        #region COMMAND METHODS
        public void OpenAddGroup()
        {
            var agvm = new AddGroupViewModel(_windowManager);
            _windowManager.ShowWindow(agvm);
            agvm.Deactivated += agvm_Deactivated;
        }

        /// <summary>
        /// on view model deactiviation, refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void agvm_Deactivated(object sender, DeactivationEventArgs e)
        {
            GroupsComputersVM.NotifyOfPropertyChange(() => GroupsComputersVM.Groups);
        }

        /// <summary>
        /// Opens the window to edit a group
        /// </summary>
        public void OpenEditGroup()
        {
            if (GroupsComputersVM.SelectedGroup != null)
            {
                var egvm = new EditGroupViewModel(_windowManager, GroupsComputersVM.SelectedGroup);
                _windowManager.ShowWindow(egvm);
                egvm.Deactivated += egvm_Deactivated;
            }
            else
            {
                MessageBox.Show("Please select a group to modify");
            }
        }

        void egvm_Deactivated(object sender, DeactivationEventArgs e)
        {
            GroupsComputersVM.NotifyOfPropertyChange(() => GroupsComputersVM.Groups);
        }

        /// <summary>
        /// Refreshes the data on the computer objects on a seperate thread
        /// </summary>
        public void RefreshInfoAll()
        {
            System.Threading.Thread new_thread = new System.Threading.Thread(StartRefreshAll);

            new_thread.Start();
        } 

        /// <summary>
        /// Calls the buisness logic helper to poll for all of the machine data
        /// Inteded to run on a seperate thread as to not lock the UI. 
        /// </summary>
        private void StartRefreshAll(){
            try
            {
                var _allComputers = Helpers.Data_Access.DataAccessMethods.ComputersFromDB();

                this.Refresh(_allComputers);

                Helpers.Data_Access.DataAccessMethods.ModifyAllComputers(_allComputers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           GroupsComputersVM.NotifyOfPropertyChange(() => GroupsComputersVM.Computers);
        }

        /// <summary>
        /// Starts a thread to update the selected groups computers
        /// </summary>
        public void RefreshCurrent()
        {
            try
            {
                System.Threading.Thread new_therad = new System.Threading.Thread(StartRefreshSelectedGroup);

                new_therad.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Refreshes the selected groups Computers
        /// Inteded to be used with a thread as to not lock up the UI
        /// </summary>
        private void StartRefreshSelectedGroup()
        {
            try
            {

                var _computersToUpdate = Helpers.Data_Access.DataAccessMethods.ComputersFromDB(GroupsComputersVM.SelectedGroup);

                this.Refresh(_computersToUpdate);

                Helpers.Data_Access.DataAccessMethods.ModifyAllComputers(_computersToUpdate);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GroupsComputersVM.NotifyOfPropertyChange(() => GroupsComputersVM.Computers);
        }

        /// <summary>
        /// Grabs a Principal context and refreshes all of the computer objects that are passed into it
        /// </summary>
        /// <param name="_computers"></param>
        private void Refresh(IEnumerable<Computer> _computers)
        {
            CurrentCount = 0;
            MaxComputerCount = _computers.Count();
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                foreach (var _computer in _computers)
                {
                    var _computePrincipal = ComputerPrincipal.FindByIdentity(context, _computer.Name);
                    if (_computePrincipal != null)
                    {
                        BuisnessLogic.ComputerBuisnessLogic.UpdateAllProperties(_computer, _computePrincipal);
                    }
                    CurrentCount++;
                }
                MessageBox.Show("Refresh Complete");
                CurrentCount = 0;
            }
        }

        /// <summary>
        /// Prompts the user and then calls the data acess method to remove the group from the database
        /// </summary>
        public void RemoveGroup()
        {
            try
            {
                var result = MessageBox.Show("Are you sure you wish to erase this group?", "Remove Group?", MessageBoxButtons.YesNo);
                if (GroupsComputersVM.SelectedGroup != null)
                {

                    if (result == DialogResult.Yes)
                    {
                        Helpers.Data_Access.DataAccessMethods.RemoveGroupFromDB(GroupsComputersVM.SelectedGroup);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a group to remove");
                }
                GroupsComputersVM.NotifyOfPropertyChange(() => GroupsComputersVM.Groups);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Output()
        {
            var Params = new Dictionary<String, String>();

            Params.Add("Name", "Name");
            Params.Add("EnabledInAD", "Enabled");
            Params.Add("IsOnline", "Online");
            Params.Add("LastLogon", "Last Logon");
            var _save = new SaveFileDialog();
            _save.DefaultExt = ".pdf";
            _save.Filter = "Portable Document Format (*.pdf) | *.pdf";
            if (_save.ShowDialog() == DialogResult.OK)
            {
                Helpers.PDFReportGenerator.GenerateReport<Models.Computer>(GroupsComputersVM.Computers, _save.FileName, GroupsComputersVM.SelectedGroup.Name, Params);
            }
        }
        #endregion

        /// <summary>
        /// Max amount of computers. Used for progress bar
        /// </summary>
        public int MaxComputerCount {
            get
            {
                return _maxComputerCount;
            }

            set
            {
                _maxComputerCount = value;
                NotifyOfPropertyChange(() => MaxComputerCount);
            }
        }

        private int _currentCount;
        private int _maxComputerCount = 0;
        public int CurrentCount
        {
            get
            {
                return _currentCount;
            }
            set
            {
                _currentCount = value;
                NotifyOfPropertyChange(() => CurrentCount);
            }
        }
    }
}
