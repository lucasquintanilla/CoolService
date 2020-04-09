using System; 
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.Configuration.Install;
using System.ComponentModel;
using System.Windows.Forms;

namespace CoolService
{
    /// <summary>
    /// Windows Service Self-Installer
    /// Version 3.0 by Lucas Quintanilla 2020
    /// Based on CoolService V2
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            bool _IsInstalled = false;
            bool serviceStarting = false; // Thanks to SMESSER's implementation V2.0
            string SERVICE_NAME = "CoolService"; //Change from WSInstaller, Classes and Namespaces too if you want

            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(SERVICE_NAME))       
                {   
                    _IsInstalled = true;          
                    if(service.Status == ServiceControllerStatus.StartPending)          
                    {             
                        // If the status is StartPending then the service was started via the SCM             
                        serviceStarting = true;          
                    }          
                    break;       
                }
            }

            if (!serviceStarting)
            {
                if (_IsInstalled == true)
                {                    
                    // Thanks to PIEBALDconsult's Concern V2.0
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Do you REALLY like to uninstall the " + SERVICE_NAME + "?", "Danger", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {                        
                        if (SelfInstaller.UninstallMe()) //Added on Version 3.0
                        {
                            MessageBox.Show("Successfully uninstalled the " + SERVICE_NAME, "Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Uninstallation failed " + SERVICE_NAME, "Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }                        
                    }
                }
                else
                {
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Do you REALLY like to install the " + SERVICE_NAME + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {                        
                        if (SelfInstaller.InstallMe()) //Added on Version 3.0
                        {
                            MessageBox.Show("Successfully installed the " + SERVICE_NAME, "Status",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Installation failed " + SERVICE_NAME, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {   
                // Started from the SCM
                System.ServiceProcess.ServiceBase[] servicestorun;
                servicestorun = new System.ServiceProcess.ServiceBase[] { new CoolService() };
                ServiceBase.Run(servicestorun);
            }
        }
    }

    public static class SelfInstaller
    {
        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });
            }
            catch
            {                
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}