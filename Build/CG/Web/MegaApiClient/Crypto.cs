using System;
using System.Security.Cryptography;
using CG.Web.MegaApiClient.Cryptography;
using CG.Web.MegaApiClient.Serialization;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000143 RID: 323
	internal class Crypto
	{
		// Token: 0x060008CB RID: 2251 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		static Crypto()
		{
			Crypto.AesCbc = new AesManaged();
			Crypto.IsKnownReusable = true;
			Crypto.AesCbc.Padding = PaddingMode.None;
			Crypto.AesCbc.Mode = CipherMode.CBC;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0004AE6C File Offset: 0x0004906C
		public static byte[] DecryptKey(byte[] data, byte[] key)
		{
			byte[] array = new byte[data.Length];
			for (int i = 0; i < data.Length; i += 16)
			{
				Array.Copy(Crypto.DecryptAes(data.CopySubArray(16, i), key), 0, array, i, 16);
			}
			return array;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0004AEAC File Offset: 0x000490AC
		public static byte[] EncryptKey(byte[] data, byte[] key)
		{
			byte[] array = new byte[data.Length];
			using (ICryptoTransform cryptoTransform = Crypto.CreateAesEncryptor(key))
			{
				for (int i = 0; i < data.Length; i += 16)
				{
					Array.Copy(Crypto.EncryptAes(data.CopySubArray(16, i), cryptoTransform), 0, array, i, 16);
				}
			}
			return array;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0004AF10 File Offset: 0x00049110
		public static void GetPartsFromDecryptedKey(byte[] decryptedKey, out byte[] iv, out byte[] metaMac, out byte[] fileKey)
		{
			iv = new byte[8];
			metaMac = new byte[8];
			Array.Copy(decryptedKey, 16, iv, 0, 8);
			Array.Copy(decryptedKey, 24, metaMac, 0, 8);
			fileKey = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				fileKey[i] = (decryptedKey[i] ^ decryptedKey[i + 16]);
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0004AF6C File Offset: 0x0004916C
		public static byte[] DecryptAes(byte[] data, byte[] key)
		{
			byte[] result;
			using (ICryptoTransform cryptoTransform = Crypto.AesCbc.CreateDecryptor(key, Crypto.DefaultIv))
			{
				result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
			}
			return result;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000D40C File Offset: 0x0000B60C
		public static ICryptoTransform CreateAesEncryptor(byte[] key)
		{
			return new CachedCryptoTransform(() => Crypto.AesCbc.CreateEncryptor(key, Crypto.DefaultIv), Crypto.IsKnownReusable);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000D42F File Offset: 0x0000B62F
		public static byte[] EncryptAes(byte[] data, ICryptoTransform encryptor)
		{
			return encryptor.TransformFinalBlock(data, 0, data.Length);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0004AFB4 File Offset: 0x000491B4
		public static byte[] EncryptAes(byte[] data, byte[] key)
		{
			byte[] result;
			using (ICryptoTransform cryptoTransform = Crypto.CreateAesEncryptor(key))
			{
				result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
			}
			return result;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0004AFF4 File Offset: 0x000491F4
		public static byte[] CreateAesKey()
		{
			byte[] key;
			using (Aes aes = Aes.Create())
			{
				aes.Mode = CipherMode.CBC;
				aes.KeySize = 128;
				aes.Padding = PaddingMode.None;
				aes.GenerateKey();
				key = aes.Key;
			}
			return key;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0004B04C File Offset: 0x0004924C
		public static byte[] EncryptAttributes(Attributes attributes, byte[] nodeKey)
		{
			byte[] array = ("MEGA" + JsonConvert.SerializeObject(attributes, Formatting.None)).ToBytes();
			array = array.CopySubArray(array.Length + 16 - array.Length % 16, 0);
			return Crypto.EncryptAes(array, nodeKey);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0004B08C File Offset: 0x0004928C
		public static Attributes DecryptAttributes(byte[] attributes, byte[] nodeKey)
		{
			byte[] data = Crypto.DecryptAes(attributes, nodeKey);
			Attributes result;
			try
			{
				string text = data.ToUTF8String().Substring(4);
				int num = text.IndexOf('\0');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				result = JsonConvert.DeserializeObject<Attributes>(text);
			}
			catch (Exception ex)
			{
				result = new Attributes(string.Format("Attribute deserialization failed: {0}", ex.Message));
			}
			return result;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0004B0F8 File Offset: 0x000492F8
		public static BigInteger[] GetRsaPrivateKeyComponents(byte[] encodedRsaPrivateKey, byte[] masterKey)
		{
			encodedRsaPrivateKey = encodedRsaPrivateKey.CopySubArray(encodedRsaPrivateKey.Length + (16 - encodedRsaPrivateKey.Length % 16), 0);
			byte[] array = Crypto.DecryptKey(encodedRsaPrivateKey, masterKey);
			BigInteger[] array2 = new BigInteger[4];
			for (int i = 0; i < 4; i++)
			{
				array2[i] = array.FromMPINumber();
				int num = ((int)array[0] * 256 + (int)array[1] + 7) / 8;
				array = array.CopySubArray(array.Length - num - 2, num + 2);
			}
			return array2;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000D43C File Offset: 0x0000B63C
		public static byte[] RsaDecrypt(BigInteger data, BigInteger p, BigInteger q, BigInteger d)
		{
			return data.modPow(d, p * q).getBytes();
		}

		// Token: 0x04000672 RID: 1650
		private static readonly Aes AesCbc;

		// Token: 0x04000673 RID: 1651
		private static readonly bool IsKnownReusable;

		// Token: 0x04000674 RID: 1652
		private static readonly byte[] DefaultIv = new byte[16];
	}
}
