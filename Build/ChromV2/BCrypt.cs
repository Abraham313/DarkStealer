using System;
using System.Runtime.InteropServices;
using SmartAssembly.StringsEncoding;

namespace ChromV2
{
	// Token: 0x0200009C RID: 156
	public static class BCrypt
	{
		// Token: 0x060002FF RID: 767
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x06000300 RID: 768
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

		// Token: 0x06000301 RID: 769
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x06000302 RID: 770
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptSetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x06000303 RID: 771
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x06000304 RID: 772
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x06000305 RID: 773
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, uint dwFlags);

		// Token: 0x06000306 RID: 774
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptDecrypt(IntPtr intptr_0, byte[] byte_0, int int_0, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO_0, byte[] byte_1, int int_1, byte[] byte_2, int int_2, ref int int_3, int int_4);

		// Token: 0x040001D9 RID: 473
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x040001DA RID: 474
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x040001DB RID: 475
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x040001DC RID: 476
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x040001DD RID: 477
		public static readonly string BCRYPT_OBJECT_LENGTH = ChromV266351.Strings.Get(107396440);

		// Token: 0x040001DE RID: 478
		public static readonly string BCRYPT_CHAIN_MODE_GCM = ChromV266351.Strings.Get(107396391);

		// Token: 0x040001DF RID: 479
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = ChromV266351.Strings.Get(107396402);

		// Token: 0x040001E0 RID: 480
		public static readonly string BCRYPT_CHAINING_MODE = ChromV266351.Strings.Get(107396381);

		// Token: 0x040001E1 RID: 481
		public static readonly string BCRYPT_KEY_DATA_BLOB = ChromV266351.Strings.Get(107395820);

		// Token: 0x040001E2 RID: 482
		public static readonly string BCRYPT_AES_ALGORITHM = ChromV266351.Strings.Get(107395835);

		// Token: 0x040001E3 RID: 483
		public static readonly string MS_PRIMITIVE_PROVIDER = ChromV266351.Strings.Get(107395830);

		// Token: 0x040001E4 RID: 484
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x040001E5 RID: 485
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x040001E6 RID: 486
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x0200009D RID: 157
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x06000308 RID: 776 RVA: 0x0000A0AF File Offset: 0x000082AF
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x040001E7 RID: 487
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040001E8 RID: 488
			public int cbSalt;
		}

		// Token: 0x0200009E RID: 158
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x06000309 RID: 777 RVA: 0x000297EC File Offset: 0x000279EC
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = BCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				if (iv != null)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				if (aad != null)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				if (tag != null)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x0600030A RID: 778 RVA: 0x000298CC File Offset: 0x00027ACC
			public void Dispose()
			{
				if (this.pbNonce != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				if (this.pbTag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				if (this.pbAuthData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				if (this.pbMacContext != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x040001E9 RID: 489
			public int cbSize;

			// Token: 0x040001EA RID: 490
			public int dwInfoVersion;

			// Token: 0x040001EB RID: 491
			public IntPtr pbNonce;

			// Token: 0x040001EC RID: 492
			public int cbNonce;

			// Token: 0x040001ED RID: 493
			public IntPtr pbAuthData;

			// Token: 0x040001EE RID: 494
			public int cbAuthData;

			// Token: 0x040001EF RID: 495
			public IntPtr pbTag;

			// Token: 0x040001F0 RID: 496
			public int cbTag;

			// Token: 0x040001F1 RID: 497
			public IntPtr pbMacContext;

			// Token: 0x040001F2 RID: 498
			public int cbMacContext;

			// Token: 0x040001F3 RID: 499
			public int cbAAD;

			// Token: 0x040001F4 RID: 500
			public long cbData;

			// Token: 0x040001F5 RID: 501
			public int dwFlags;
		}

		// Token: 0x0200009F RID: 159
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x040001F6 RID: 502
			public int dwMinLength;

			// Token: 0x040001F7 RID: 503
			public int dwMaxLength;

			// Token: 0x040001F8 RID: 504
			public int dwIncrement;
		}

		// Token: 0x020000A0 RID: 160
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x0600030B RID: 779 RVA: 0x0000A0BF File Offset: 0x000082BF
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x040001F9 RID: 505
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040001FA RID: 506
			public IntPtr pbLabel;

			// Token: 0x040001FB RID: 507
			public int cbLabel;
		}
	}
}
