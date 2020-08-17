using System;
using System.IO;
using System.Security.Cryptography;

namespace Ionic.Zip
{
	// Token: 0x020000E0 RID: 224
	internal class WinZipAesCrypto
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0000A84F File Offset: 0x00008A4F
		private WinZipAesCrypto(string password, int KeyStrengthInBits)
		{
			this._Password = password;
			this._KeyStrengthInBits = KeyStrengthInBits;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0002F11C File Offset: 0x0002D31C
		public static WinZipAesCrypto Generate(string password, int KeyStrengthInBits)
		{
			WinZipAesCrypto winZipAesCrypto = new WinZipAesCrypto(password, KeyStrengthInBits);
			int num = winZipAesCrypto._KeyStrengthInBytes / 2;
			winZipAesCrypto._Salt = new byte[num];
			Random random = new Random();
			random.NextBytes(winZipAesCrypto._Salt);
			return winZipAesCrypto;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0002F15C File Offset: 0x0002D35C
		public static WinZipAesCrypto ReadFromStream(string password, int KeyStrengthInBits, Stream s)
		{
			WinZipAesCrypto winZipAesCrypto = new WinZipAesCrypto(password, KeyStrengthInBits);
			int num = winZipAesCrypto._KeyStrengthInBytes / 2;
			winZipAesCrypto._Salt = new byte[num];
			winZipAesCrypto._providedPv = new byte[2];
			s.Read(winZipAesCrypto._Salt, 0, winZipAesCrypto._Salt.Length);
			s.Read(winZipAesCrypto._providedPv, 0, winZipAesCrypto._providedPv.Length);
			winZipAesCrypto.PasswordVerificationStored = (short)((int)winZipAesCrypto._providedPv[0] + (int)winZipAesCrypto._providedPv[1] * 256);
			if (password != null)
			{
				winZipAesCrypto.PasswordVerificationGenerated = (short)((int)winZipAesCrypto.GeneratedPV[0] + (int)winZipAesCrypto.GeneratedPV[1] * 256);
				if (winZipAesCrypto.PasswordVerificationGenerated != winZipAesCrypto.PasswordVerificationStored)
				{
					throw new BadPasswordException("bad password");
				}
			}
			return winZipAesCrypto;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000A870 File Offset: 0x00008A70
		public byte[] GeneratedPV
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._generatedPv;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000A886 File Offset: 0x00008A86
		public byte[] Salt
		{
			get
			{
				return this._Salt;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000A88E File Offset: 0x00008A8E
		private int _KeyStrengthInBytes
		{
			get
			{
				return this._KeyStrengthInBits / 8;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000A898 File Offset: 0x00008A98
		public int SizeOfEncryptionMetadata
		{
			get
			{
				return this._KeyStrengthInBytes / 2 + 10 + 2;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0002F218 File Offset: 0x0002D418
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password != null)
				{
					this.PasswordVerificationGenerated = (short)((int)this.GeneratedPV[0] + (int)this.GeneratedPV[1] * 256);
					if (this.PasswordVerificationGenerated != this.PasswordVerificationStored)
					{
						throw new BadPasswordException();
					}
				}
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0002F268 File Offset: 0x0002D468
		private void _GenerateCryptoBytes()
		{
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(this._Password, this.Salt, this.Rfc2898KeygenIterations);
			this._keyBytes = rfc2898DeriveBytes.GetBytes(this._KeyStrengthInBytes);
			this._MacInitializationVector = rfc2898DeriveBytes.GetBytes(this._KeyStrengthInBytes);
			this._generatedPv = rfc2898DeriveBytes.GetBytes(2);
			this._cryptoGenerated = true;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000A8AF File Offset: 0x00008AAF
		public byte[] KeyBytes
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._keyBytes;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000A8C5 File Offset: 0x00008AC5
		public byte[] MacIv
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._MacInitializationVector;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0002F2C8 File Offset: 0x0002D4C8
		public void ReadAndVerifyMac(Stream s)
		{
			bool flag = false;
			this._StoredMac = new byte[10];
			s.Read(this._StoredMac, 0, this._StoredMac.Length);
			if (this._StoredMac.Length != this.CalculatedMac.Length)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = 0; i < this._StoredMac.Length; i++)
				{
					if (this._StoredMac[i] != this.CalculatedMac[i])
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				throw new BadStateException("The MAC does not match.");
			}
		}

		// Token: 0x040002B7 RID: 695
		internal byte[] _Salt;

		// Token: 0x040002B8 RID: 696
		internal byte[] _providedPv;

		// Token: 0x040002B9 RID: 697
		internal byte[] _generatedPv;

		// Token: 0x040002BA RID: 698
		internal int _KeyStrengthInBits;

		// Token: 0x040002BB RID: 699
		private byte[] _MacInitializationVector;

		// Token: 0x040002BC RID: 700
		private byte[] _StoredMac;

		// Token: 0x040002BD RID: 701
		private byte[] _keyBytes;

		// Token: 0x040002BE RID: 702
		private short PasswordVerificationStored;

		// Token: 0x040002BF RID: 703
		private short PasswordVerificationGenerated;

		// Token: 0x040002C0 RID: 704
		private int Rfc2898KeygenIterations = 1000;

		// Token: 0x040002C1 RID: 705
		private string _Password;

		// Token: 0x040002C2 RID: 706
		private bool _cryptoGenerated;

		// Token: 0x040002C3 RID: 707
		public byte[] CalculatedMac;
	}
}
