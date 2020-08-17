using System;
using System.IO;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x020000E3 RID: 227
	internal class ZipCrypto
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0000A914 File Offset: 0x00008B14
		private ZipCrypto()
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0002FAA4 File Offset: 0x0002DCA4
		public static ZipCrypto ForWrite(string password)
		{
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			return zipCrypto;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0002FAD0 File Offset: 0x0002DCD0
		public static ZipCrypto ForRead(string password, ZipEntry e)
		{
			Stream archiveStream = e._archiveStream;
			e._WeakEncryptionHeader = new byte[12];
			byte[] weakEncryptionHeader = e._WeakEncryptionHeader;
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			ZipEntry.ReadWeakEncryptionHeader(archiveStream, weakEncryptionHeader);
			byte[] array = zipCrypto.DecryptMessage(weakEncryptionHeader, weakEncryptionHeader.Length);
			if (array[11] != (byte)(e._Crc32 >> 24 & 255))
			{
				if ((e._BitField & 8) != 8)
				{
					throw new BadPasswordException("The password did not match.");
				}
				if (array[11] != (byte)(e._TimeBlob >> 8 & 255))
				{
					throw new BadPasswordException("The password did not match.");
				}
			}
			return zipCrypto;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0002FB74 File Offset: 0x0002DD74
		private byte MagicByte
		{
			get
			{
				ushort num = (ushort)(this._Keys[2] & 65535U) | 2;
				return (byte)(num * (num ^ 1) >> 8);
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0002FB9C File Offset: 0x0002DD9C
		public byte[] DecryptMessage(byte[] cipherText, int length)
		{
			if (cipherText == null)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (length > cipherText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte b = cipherText[i] ^ this.MagicByte;
				this.UpdateKeys(b);
				array[i] = b;
			}
			return array;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0002FBF8 File Offset: 0x0002DDF8
		public byte[] EncryptMessage(byte[] plainText, int length)
		{
			if (plainText == null)
			{
				throw new ArgumentNullException("plaintext");
			}
			if (length > plainText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte byteValue = plainText[i];
				array[i] = (plainText[i] ^ this.MagicByte);
				this.UpdateKeys(byteValue);
			}
			return array;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0002FC58 File Offset: 0x0002DE58
		public void InitCipher(string passphrase)
		{
			byte[] array = SharedUtilities.StringToByteArray(passphrase);
			for (int i = 0; i < passphrase.Length; i++)
			{
				this.UpdateKeys(array[i]);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0002FC88 File Offset: 0x0002DE88
		private void UpdateKeys(byte byteValue)
		{
			this._Keys[0] = (uint)this.crc32.ComputeCrc32((int)this._Keys[0], byteValue);
			this._Keys[1] = this._Keys[1] + (uint)((byte)this._Keys[0]);
			this._Keys[1] = this._Keys[1] * 134775813U + 1U;
			this._Keys[2] = (uint)this.crc32.ComputeCrc32((int)this._Keys[2], (byte)(this._Keys[1] >> 24));
		}

		// Token: 0x040002E2 RID: 738
		private uint[] _Keys = new uint[]
		{
			305419896U,
			591751049U,
			878082192U
		};

		// Token: 0x040002E3 RID: 739
		private CRC32 crc32 = new CRC32();
	}
}
