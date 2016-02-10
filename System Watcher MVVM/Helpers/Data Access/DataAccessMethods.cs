using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.Helpers.Data_Access
{
    public static class DataAccessMethods
    {
        /// <summary>
        /// Gets all the groups from the database
        /// </summary>
        /// <returns>An ObservableCollection of the Groups from the DB</returns>
        public static ObservableCollection<Group> GroupsFromDB()
        {
            var LTR = new ObservableCollection<Group>(); 
            try
            {
                using (var conn = new ModelsContainer())
                {
                    foreach (var _group in conn.Groups)
                    {
                        LTR.Add(_group);   
                    }                                            
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return LTR;
        }

        /// <summary>
        /// Gets all the computers from the database
        /// </summary>
        /// <returns>An observable collection of the computers from the database set</returns>
        public static ObservableCollection<Computer> ComputersFromDB()
        {
            var LTR = new ObservableCollection<Computer>();
            try
            {
                using (var conn = new ModelsContainer())
                {
                    foreach (var _comp in conn.Computers)
                    {
                        LTR.Add(_comp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return LTR;
        }

        /// <summary>
        /// Gets all the computers from the database that have the same group id as the group passed in
        /// </summary>
        /// <returns>An observable collection of the computers from the database set</returns>
        public static ObservableCollection<Computer> ComputersFromDB(Group _group)
        {
            var LTR = new ObservableCollection<Computer>();
            try
            {
                using (var conn = new ModelsContainer())
                {
                    foreach (var _comp in conn.Computers)
                    {
                        if (_comp.GroupsId == _group.Id)
                        {
                            LTR.Add(_comp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return LTR;
        }
        
        /// <summary>
        /// Edits a group in the database
        /// </summary>
        /// <param name="_group">The group you are looking to make changes to</param>
        public static void EditGroup(Group _group)
        {
            try
            {
                using (var conn = new ModelsContainer())
                {
                    conn.Groups.Attach(_group);
                    conn.Entry<Group>(_group).State = System.Data.Entity.EntityState.Modified;
                    conn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the computers from the table
        /// </summary>
        /// <param name="_group">The group of computers to remove</param>
        public static void RemoveComputersFromGroup(Group _group)
        {
            try
            {
                using (var conn = new ModelsContainer())
                {
                    var _computers = ComputersFromDB(_group);
                    foreach (var _comp in _computers.ToList<Computer>())
                    {
                        var _entry = conn.Entry(_comp);
                        if (_entry.State == System.Data.Entity.EntityState.Detached)
                        {
                            conn.Computers.Attach(_comp);
                        }
                        conn.Computers.Remove(_comp);
                        _entry.State = System.Data.Entity.EntityState.Deleted;
                    }
                    conn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Adds computers to the database attached to the group. The Group must already exist in the data base or this will fail.
        /// </summary>
        /// <param name="_group">The group to add the computer to</param>
        /// <param name="_computers">The list of computers to add</param>
        public static void AddComputersToDB(Group _group, IEnumerable<Computer> _computers)
        {
            try
            {
                using (var conn = new ModelsContainer())
                {
                    foreach (var _comp in _computers)
                    {
                        conn.Computers.Add(new Computer()
                        {
                            Name = _comp.Name,
                            GroupsId = _group.Id,
                        });
                        conn.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void RemoveGroupFromDB(Group group)
        {
            // start by clearing the computers from the group
            try
            {
                RemoveComputersFromGroup(group); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // now clear the group obejct
            try
            {
                using (var conn = new ModelsContainer())
                {
                    conn.Groups.Attach(group);
                    conn.Groups.Remove(group);
                    conn.Entry(group).State = System.Data.Entity.EntityState.Deleted;
                    conn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Modifies a single instance of a computer object in the DB
        /// </summary>
        /// <param name="_computer">The computer to modify</param>
        public static void ModifyComputer(Computer _computer)
        {
            try
            {
                using (var conn = new ModelsContainer())
                {
                    conn.Computers.Attach(_computer);
                    conn.Entry(_computer).State = System.Data.Entity.EntityState.Modified;
                    conn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Modifies all the records of computers in the list, only opens one connection
        /// </summary>
        /// <param name="_computers">The list of computers to modify</param>
        public static void ModifyAllComputers(IEnumerable<Computer> _computers)
        {
            try
            {
                using (var conn = new ModelsContainer())
                {
                    foreach (var _computer in _computers)
                    {
                        conn.Computers.Attach(_computer);
                        conn.Entry(_computer).State = System.Data.Entity.EntityState.Modified;
                        conn.SaveChanges();   
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
