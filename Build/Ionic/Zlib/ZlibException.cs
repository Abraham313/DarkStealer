using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000122 RID: 290
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	[ComVisible(true)]
	public class ZlibException : Exception
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x00009C87 File Offset: 0x00007E87
		public ZlibException()
		{
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00009C8F File Offset: 0x00007E8F
		public ZlibException(string s) : base(s)
		{
		}
	}
}
