using System;

namespace Echelon.Global
{
	// Token: 0x02000059 RID: 89
	internal class GenString
	{
		// Token: 0x06000209 RID: 521 RVA: 0x00022148 File Offset: 0x00020348
		public static string Generate()
		{
			string text = "acegikmoqsuwyBDFHJLNPRTVXZ";
			string text2 = "";
			Random random = new Random();
			int num = random.Next(0, text.Length);
			for (int i = 0; i < num; i++)
			{
				text2 += text[random.Next(10, text.Length)].ToString();
			}
			return text2;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009A68 File Offset: 0x00007C68
		public static int GeneNumbersTo()
		{
			return new Random().Next(11, 99);
		}
	}
}
