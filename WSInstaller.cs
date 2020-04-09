using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CoolService
{
    [RunInstaller(true)]
    public class WSInstaller : System.Configuration.Install.Installer
    {
        public WSInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            ServiceInstaller serviceAdmin = new ServiceInstaller();
            serviceAdmin.StartType = ServiceStartMode.Manual;
            serviceAdmin.ServiceName = "CoolService";
            serviceAdmin.DisplayName = "CoolService";
            serviceAdmin.Description = "CoolService";
            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }
}
