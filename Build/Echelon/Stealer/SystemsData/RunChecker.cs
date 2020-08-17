using System;
using System.Security.Principal;

namespace Echelon.Stealer.SystemsData
{
	// Token: 0x02000025 RID: 37
	public static class RunChecker
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00008ABC File Offset: 0x00006CBC
		public static bool IsAdmin
		{
			get
			{
				return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00008AD2 File Offset: 0x00006CD2
		public static bool IsWin64
		{
			get
			{
				return Environment.Is64BitOperatingSystem;
			}
		}
	}
}
