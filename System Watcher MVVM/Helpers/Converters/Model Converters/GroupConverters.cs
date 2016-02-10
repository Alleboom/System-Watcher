using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System_Watcher_MVVM.Models;

namespace System_Watcher_MVVM.Helpers.Converters.Model_Converters
{
    /// <summary>
    /// Methods for converting or modifying groups
    /// </summary>
    public static class GroupConverters
    {

        /// <summary>
        /// Modifies the group instance with the passed in parameters
        /// </summary>
        /// <param name="_group">Group to modify</param>
        /// <param name="_name">New name</param>
        /// <param name="_owner">New Owner</param>
        /// <param name="_location">New Location</param>
        /// <param name="_computers">New List of Computers</param>
        /// <returns></returns>
        public static Group GroupModify(Group _group, String _name, String _owner, String _location, IEnumerable<Computer> _computers)
        {
            _group.Name = _name;
            _group.BuildingFloor = _location;
            _group.Owner = _owner;

            Data_Access.DataAccessMethods.RemoveComputersFromGroup(_group);
            Data_Access.DataAccessMethods.AddComputersToDB(_group, _computers);

            return _group;
        }

    }
}
