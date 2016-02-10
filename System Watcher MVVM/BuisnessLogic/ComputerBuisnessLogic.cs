using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System_Watcher_MVVM.Models;
using System.DirectoryServices.AccountManagement;
using System.Net.NetworkInformation;
using System.Windows;
using Caliburn.Micro;
using System_Watcher_MVVM.ViewModels;

namespace System_Watcher_MVVM.BuisnessLogic
{
    /// <summary>
    /// Class for determining values for Computer objects
    /// </summary>
    public static class ComputerBuisnessLogic
    {
        /// <summary>
        /// Updates all the properties on the computer using functions defined in this class. Slower but much easier to write
        /// </summary>
        /// <param name="_computer">The computer object we would like to update</param>
        public static void UpdateAllProperties(Computer _computer)
        {

            DetermineIfOnline(_computer);
            var compPrinc = GetComputerPrincipal(_computer);
            DetermineLastLogon(_computer, compPrinc);
            DetermineIfEnabled(_computer, compPrinc);

        }

         

        /// <summary>
        /// Updates all the properties of every element in a list.
        /// This should be a faster method than calling the individual UpdateAllProperties, as this only uses one context
        /// However, Progress bars and the like will not work with this, as there is no context to the view model that can
        /// be established with type saftey 
        /// </summary>
        /// <param name="_computers">A list containing every computer you wanted updated.</param>
        /// <param name="_viewModel">The viewmodel to check against. Should derive from PropertyChangedBase from Caliburn.Micro to notify of property updates. Not type safe at all, do not use if you cannot modify this code. </param>
        /// <param name="_counter">Counter to increment.</param>
        public static void UpdateAllProperties(IEnumerable<Computer> _computers)
        {

           
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    foreach (var _computer in _computers)
                    {
                        var _compPrinc = ComputerPrincipal.FindByIdentity(context, _computer.Name);
                        if (_compPrinc != null)
                        {
                            DetermineLastLogon(_computer, _compPrinc);
                            DetermineIfOnline(_computer);
                            DetermineIfEnabled(_computer, _compPrinc);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// Determines from AD if the system is enabled in the domain
        /// </summary>
        /// <param name="_computer">The computer object to modify</param>
        /// <param name="compPrinc">The AD computer principle that we looked up</param>
        private static void DetermineIfEnabled(Computer _computer, ComputerPrincipal compPrinc)
        {
            _computer.EnabledInAD = compPrinc.Enabled;
        }

        /// <summary>
        /// Determine the last logon date for a pc
        /// </summary>
        /// <param name="_computer">The computer object we want to check</param>
        /// <param name="compPrinc">The computer principle that we look up to determine last logon date from AD</param>
        private static void DetermineLastLogon(Computer _computer, ComputerPrincipal compPrinc)
        {
            ApplyLastLogonConverted(_computer, compPrinc);
        }

        /// <summary>
        /// Helper method for applying the last logon date time value with the Current systems time zone
        /// </summary>
        /// <param name="_computer">Computer to modify</param>
        /// <param name="compPrinc">The Principal that we derive the last logon from</param>
        private static void ApplyLastLogonConverted(Computer _computer, ComputerPrincipal compPrinc)
        {
            _computer.LastLogon = Helpers.Converters.DateTimeConverter.DateTimeTimeZoneConversion.ConvertTimeZone(compPrinc.LastLogon);
        }



        /// <summary>
        /// Determines if the computer is enabled in AD
        /// </summary>
        /// <param name="_computer">The computer to check in AD</param>
        public static void DetermineIfEnabled(Computer _computer)
        {
            var _compPrincipal = GetComputerPrincipal(_computer);
            _computer.EnabledInAD = _compPrincipal.Enabled;
        }

        /// <summary>
        /// Helper function to get computer principal for determining certain properties from AD
        /// </summary>
        /// <param name="_computer"></param>
        /// <returns></returns>
        private static ComputerPrincipal GetComputerPrincipal(Computer _computer)
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    var _compPrincipal = ComputerPrincipal.FindByIdentity(context, _computer.Name);
                    if (_compPrincipal != null)
                    {
                        return _compPrincipal;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Dtermines the last logon date
        /// </summary>
        /// <param name="_computer">The computer to check for the last logon date</param>
        public static void DetermineLastLogon(Computer _computer)
        {
            var _compPrinc = GetComputerPrincipal(_computer);
            ApplyLastLogonConverted(_computer, _compPrinc);
        }

        /// <summary>
        /// Pings the machine to see if it is online
        /// </summary>
        /// <param name="_computer">The computer to ping</param>
        public static void DetermineIfOnline(Computer _computer)
        {
            try
            {
                var _ping = new Ping();
                var _result = _ping.Send(_computer.Name);
                if (_result.Status == IPStatus.Success)
                {
                    _computer.IsOnline = true;
                }
                else
                {
                    _computer.IsOnline = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _computer.IsOnline = false;
            }
        }


        /// <summary>
        /// Determines all of the properties of a computer object. This method is the fastest
        /// for one pc at a time, or iterating over a list knowing the Computer Principal ahead of time
        /// </summary>
        /// <param name="_computer">The computer object to modify</param>
        /// <param name="_computePrinciapal">The Principal required to determine some information</param>
        public static void UpdateAllProperties(Computer _computer, ComputerPrincipal _computePrinciapal)
        {
            DetermineIfEnabled(_computer, _computePrinciapal);
            DetermineIfOnline(_computer);
            DetermineLastLogon(_computer, _computePrinciapal);
        }
    }
}
