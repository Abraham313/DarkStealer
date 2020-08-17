using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000123 RID: 291
	internal class SharedUtils
	{
		// Token: 0x060007DE RID: 2014 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00045DE4 File Offset: 0x00043FE4
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0000CAA4 File Offset: 0x0000ACA4
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0000CAB1 File Offset: 0x0000ACB1
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
