using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.ViewModels
{
    public class AddGroupViewModel : Screen
    {
        private Caliburn.Micro.IWindowManager _windowManager;

        public ViewModels.Groups.GroupFormViewModel GroupVM { get; set; }


        public AddGroupViewModel(Caliburn.Micro.IWindowManager _windowManager)
        {

            GroupVM = new Groups.GroupFormViewModel();
            
            this._windowManager = _windowManager;
        }

        public void SaveNewGroup()
        {
            try
            {
                var _group = new Group()
                {
                    Name = GroupVM.NewName,
                    Owner = GroupVM.NewOwnerName,
                    BuildingFloor = GroupVM.NewLocation,
                    Computers = GroupVM.NewComputers,
                };



                using (var conn = new ModelsContainer())
                {
                    conn.Groups.Add(_group);
                    conn.SaveChanges();
                }
                this.TryClose();     
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            

        }

        public bool CanSaveNewGroup()
        {
            return string.IsNullOrEmpty(GroupVM.NewName);
        }

    }
}
