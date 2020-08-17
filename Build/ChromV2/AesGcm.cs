using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using SmartAssembly.StringsEncoding;

namespace ChromV2
{
	// Token: 0x0200009B RID: 155
	internal sealed class AesGcm
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x00029404 File Offset: 0x00027604
		public byte[] Decrypt(byte[] byte_0, byte[] byte_1, byte[] byte_2, byte[] byte_3, byte[] byte_4)
		{
			IntPtr intPtr = this.OpenAlgorithmProvider(BCrypt.BCRYPT_AES_ALGORITHM, BCrypt.MS_PRIMITIVE_PROVIDER, BCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr intPtr2;
			IntPtr hglobal = this.ImportKey(intPtr, byte_0, out intPtr2);
			BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte_1, byte_2, byte_4);
			byte[] array2;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
				int num = 0;
				uint num2 = BCrypt.BCryptDecrypt(intPtr2, byte_3, byte_3.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396637), num2));
				}
				array2 = new byte[num];
				num2 = BCrypt.BCryptDecrypt(intPtr2, byte_3, byte_3.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
				if (num2 == BCrypt.STATUS_AUTH_TAG_MISMATCH)
				{
					throw new CryptographicException(ChromV266351.Strings.Get(107397032));
				}
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396963), num2));
				}
			}
			BCrypt.BCryptDestroyKey(intPtr2);
			Marshal.FreeHGlobal(hglobal);
			BCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return array2;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00029524 File Offset: 0x00027724
		private int MaxAuthTagSize(IntPtr intptr_0)
		{
			byte[] property = this.GetProperty(intptr_0, BCrypt.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00029564 File Offset: 0x00027764
		private IntPtr OpenAlgorithmProvider(string string_0, string string_1, string string_2)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = BCrypt.BCryptOpenAlgorithmProvider(out zero, string_0, string_1, 0U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396926), num));
			}
			byte[] bytes = Encoding.Unicode.GetBytes(string_2);
			num = BCrypt.BCryptSetProperty(zero, BCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396293), num));
			}
			return zero;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000295DC File Offset: 0x000277DC
		private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			int num = BitConverter.ToInt32(this.GetProperty(hAlg, BCrypt.BCRYPT_OBJECT_LENGTH), 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = this.Concat(new byte[][]
			{
				BCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			uint num2 = BCrypt.BCryptImportKey(hAlg, IntPtr.Zero, BCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396131), num2));
			}
			return intPtr;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00029668 File Offset: 0x00027868
		private byte[] GetProperty(IntPtr intptr_0, string string_0)
		{
			int num = 0;
			uint num2 = BCrypt.BCryptGetProperty(intptr_0, string_0, null, 0, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396602), num2));
			}
			byte[] array = new byte[num];
			num2 = BCrypt.BCryptGetProperty(intptr_0, string_0, array, array.Length, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format(ChromV266351.Strings.Get(107396481), num2));
			}
			return array;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000296D8 File Offset: 0x000278D8
		public byte[] Concat(byte[][] byte_0)
		{
			int num = 0;
			foreach (byte[] array in byte_0)
			{
				if (array != null)
				{
					num += array.Length;
				}
			}
			byte[] array2 = new byte[num - 1 + 1];
			int num2 = 0;
			foreach (byte[] array3 in byte_0)
			{
				if (array3 != null)
				{
					Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
					num2 += array3.Length;
				}
			}
			return array2;
		}
	}
}
