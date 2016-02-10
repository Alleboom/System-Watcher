using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.ViewModels
{
    public class EditGroupViewModel : Screen
    {   
        
        public ViewModels.Groups.GroupFormViewModel GroupVM { get; set; }
        private Group GroupToEdit { get; set; }
        private Caliburn.Micro.IWindowManager _windowManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_windowManager">Caliburn window manager</param>
        /// <param name="_group">The group to edit, parameters will populate fields</param>
        public EditGroupViewModel(Caliburn.Micro.IWindowManager _windowManager, Group _group)
        {
            // TODO: Complete member initialization
            this._windowManager = _windowManager;

            GroupVM = new Groups.GroupFormViewModel();

            GroupVM.NewComputers = Helpers.Data_Access.DataAccessMethods.ComputersFromDB(_group);

            GroupVM.NewLocation = _group.BuildingFloor;
            GroupVM.NewName = _group.Name;
            GroupVM.NewOwnerName = _group.Owner;
            GroupToEdit = _group;
            
        }

        /// <summary>
        /// Edits the group in the database by calling the helper method
        /// </summary>
        public void EditGroup()
        {

            Helpers.Converters.Model_Converters.GroupConverters.GroupModify(GroupToEdit, GroupVM.NewName, GroupVM.NewOwnerName, GroupVM.NewLocation, GroupVM.NewComputers);
            Helpers.Data_Access.DataAccessMethods.EditGroup(this.GroupToEdit);

            this.TryClose();
            
        }

    }
}
