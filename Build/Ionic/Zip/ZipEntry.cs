using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.BZip2;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x020000E6 RID: 230
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00004")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class ZipEntry
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000A971 File Offset: 0x00008B71
		internal bool AttributesIndicateDirectory
		{
			get
			{
				return this._InternalFileAttrs == 0 && (this._ExternalFileAttrs & 16) == 16;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000A98A File Offset: 0x00008B8A
		internal void ResetDirEntry()
		{
			this.__FileDataPosition = -1L;
			this._LengthOfHeader = 0;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0002FDEC File Offset: 0x0002DFEC
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipEntry: {0}\n", this.FileName)).Append(string.Format("   Version Made By: {0}\n", this._VersionMadeBy)).Append(string.Format(" Needed to extract: {0}\n", this.VersionNeeded));
				if (this._IsDirectory)
				{
					stringBuilder.Append("        Entry type: directory\n");
				}
				else
				{
					stringBuilder.Append(string.Format("         File type: {0}\n", this._IsText ? "text" : "binary")).Append(string.Format("       Compression: {0}\n", this.CompressionMethod)).Append(string.Format("        Compressed: 0x{0:X}\n", this.CompressedSize)).Append(string.Format("      Uncompressed: 0x{0:X}\n", this.UncompressedSize)).Append(string.Format("             CRC32: 0x{0:X8}\n", this._Crc32));
				}
				stringBuilder.Append(string.Format("       Disk Number: {0}\n", this._diskNumber));
				if (this._RelativeOffsetOfLocalHeader > 4294967295L)
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X16}\n", this._RelativeOffsetOfLocalHeader));
				}
				else
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X8}\n", this._RelativeOffsetOfLocalHeader));
				}
				stringBuilder.Append(string.Format("         Bit Field: 0x{0:X4}\n", this._BitField)).Append(string.Format("        Encrypted?: {0}\n", this._sourceIsEncrypted)).Append(string.Format("          Timeblob: 0x{0:X8}\n", this._TimeBlob)).Append(string.Format("              Time: {0}\n", SharedUtilities.PackedToDateTime(this._TimeBlob)));
				stringBuilder.Append(string.Format("         Is Zip64?: {0}\n", this._InputUsesZip64));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("           Comment: {0}\n", this._Comment));
				}
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00030018 File Offset: 0x0002E218
		internal static ZipEntry ReadDirEntry(ZipFile zf, Dictionary<string, object> previouslySeen)
		{
			Stream readStream = zf.ReadStream;
			Encoding encoding = (zf.AlternateEncodingUsage == ZipOption.Always) ? zf.AlternateEncoding : ZipFile.DefaultEncoding;
			int num = SharedUtilities.ReadSignature(readStream);
			if (ZipEntry.IsNotValidZipDirEntrySig(num))
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				if ((long)num != 101010256L && (long)num != 101075792L && num != 67324752)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position 0x{1:X8}", num, readStream.Position));
				}
				return null;
			}
			else
			{
				int num2 = 46;
				byte[] array = new byte[42];
				int num3 = readStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return null;
				}
				ZipEntry zipEntry = new ZipEntry();
				zipEntry.AlternateEncoding = encoding;
				zipEntry._Source = ZipEntrySource.ZipFile;
				zipEntry._container = new ZipContainer(zf);
				zipEntry._VersionMadeBy = (short)((int)array[0] + (int)array[1] * 256);
				zipEntry._VersionNeeded = (short)((int)array[2] + (int)array[3] * 256);
				zipEntry._BitField = (short)((int)array[4] + (int)array[5] * 256);
				zipEntry._CompressionMethod = (short)((int)array[6] + (int)array[7] * 256);
				zipEntry._TimeBlob = (int)array[8] + (int)array[9] * 256 + (int)array[10] * 256 * 256 + (int)array[11] * 256 * 256 * 256;
				zipEntry._LastModified = SharedUtilities.PackedToDateTime(zipEntry._TimeBlob);
				zipEntry._timestamp |= ZipEntryTimestamp.DOS;
				zipEntry._Crc32 = (int)array[12] + (int)array[13] * 256 + (int)array[14] * 256 * 256 + (int)array[15] * 256 * 256 * 256;
				zipEntry._CompressedSize = (long)((ulong)((int)array[16] + (int)array[17] * 256 + (int)array[18] * 256 * 256 + (int)array[19] * 256 * 256 * 256));
				zipEntry._UncompressedSize = (long)((ulong)((int)array[20] + (int)array[21] * 256 + (int)array[22] * 256 * 256 + (int)array[23] * 256 * 256 * 256));
				zipEntry._CompressionMethod_FromZipFile = zipEntry._CompressionMethod;
				zipEntry._filenameLength = (short)((int)array[24] + (int)array[25] * 256);
				zipEntry._extraFieldLength = (short)((int)array[26] + (int)array[27] * 256);
				zipEntry._commentLength = (short)((int)array[28] + (int)array[29] * 256);
				zipEntry._diskNumber = (uint)array[30] + (uint)array[31] * 256U;
				zipEntry._InternalFileAttrs = (short)((int)array[32] + (int)array[33] * 256);
				zipEntry._ExternalFileAttrs = (int)array[34] + (int)array[35] * 256 + (int)array[36] * 256 * 256 + (int)array[37] * 256 * 256 * 256;
				zipEntry._RelativeOffsetOfLocalHeader = (long)((ulong)((int)array[38] + (int)array[39] * 256 + (int)array[40] * 256 * 256 + (int)array[41] * 256 * 256 * 256));
				zipEntry.IsText = ((zipEntry._InternalFileAttrs & 1) == 1);
				array = new byte[(int)zipEntry._filenameLength];
				num3 = readStream.Read(array, 0, array.Length);
				num2 += num3;
				if ((zipEntry._BitField & 2048) == 2048)
				{
					zipEntry._FileNameInArchive = SharedUtilities.Utf8StringFromBuffer(array);
				}
				else
				{
					zipEntry._FileNameInArchive = SharedUtilities.StringFromBuffer(array, encoding);
				}
				while (previouslySeen.ContainsKey(zipEntry._FileNameInArchive))
				{
					zipEntry._FileNameInArchive = ZipEntry.CopyHelper.AppendCopyToFileName(zipEntry._FileNameInArchive);
					zipEntry._metadataChanged = true;
				}
				if (zipEntry.AttributesIndicateDirectory)
				{
					zipEntry.MarkAsDirectory();
				}
				else if (zipEntry._FileNameInArchive.EndsWith("/"))
				{
					zipEntry.MarkAsDirectory();
				}
				zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				if ((zipEntry._BitField & 1) == 1)
				{
					ZipEntry zipEntry2 = zipEntry;
					zipEntry._Encryption = EncryptionAlgorithm.PkzipWeak;
					zipEntry2._Encryption_FromZipFile = EncryptionAlgorithm.PkzipWeak;
					zipEntry._sourceIsEncrypted = true;
				}
				if (zipEntry._extraFieldLength > 0)
				{
					zipEntry._InputUsesZip64 = (zipEntry._CompressedSize == 4294967295L || zipEntry._UncompressedSize == 4294967295L || zipEntry._RelativeOffsetOfLocalHeader == 4294967295L);
					num2 += zipEntry.ProcessExtraField(readStream, zipEntry._extraFieldLength);
					zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				}
				if (zipEntry._Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					zipEntry._CompressedFileDataSize -= 12L;
				}
				else if (zipEntry.Encryption == EncryptionAlgorithm.WinZipAes128 || zipEntry.Encryption == EncryptionAlgorithm.WinZipAes256)
				{
					zipEntry._CompressedFileDataSize = zipEntry.CompressedSize - (long)(ZipEntry.GetLengthOfCryptoHeaderBytes(zipEntry.Encryption) + 10);
					zipEntry._LengthOfTrailer = 10;
				}
				if ((zipEntry._BitField & 8) == 8)
				{
					if (zipEntry._InputUsesZip64)
					{
						zipEntry._LengthOfTrailer += 24;
					}
					else
					{
						zipEntry._LengthOfTrailer += 16;
					}
				}
				zipEntry.AlternateEncoding = (((zipEntry._BitField & 2048) == 2048) ? Encoding.UTF8 : encoding);
				zipEntry.AlternateEncodingUsage = ZipOption.Always;
				if (zipEntry._commentLength > 0)
				{
					array = new byte[(int)zipEntry._commentLength];
					num3 = readStream.Read(array, 0, array.Length);
					num2 += num3;
					if ((zipEntry._BitField & 2048) == 2048)
					{
						zipEntry._Comment = SharedUtilities.Utf8StringFromBuffer(array);
					}
					else
					{
						zipEntry._Comment = SharedUtilities.StringFromBuffer(array, encoding);
					}
				}
				return zipEntry;
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000A9A2 File Offset: 0x00008BA2
		internal static bool IsNotValidZipDirEntrySig(int signature)
		{
			return signature != 33639248;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00030608 File Offset: 0x0002E808
		public ZipEntry()
		{
			this._CompressionMethod = 8;
			this._CompressionLevel = CompressionLevel.Default;
			this._Encryption = EncryptionAlgorithm.None;
			this._Source = ZipEntrySource.None;
			this.AlternateEncoding = Encoding.GetEncoding("IBM437");
			this.AlternateEncodingUsage = ZipOption.Default;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000A9AF File Offset: 0x00008BAF
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x00030678 File Offset: 0x0002E878
		public DateTime LastModified
		{
			get
			{
				return this._LastModified.ToLocalTime();
			}
			set
			{
				this._LastModified = ((value.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind(value, DateTimeKind.Local) : value.ToLocalTime());
				this._Mtime = SharedUtilities.AdjustTime_Reverse(this._LastModified).ToUniversalTime();
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000A9BC File Offset: 0x00008BBC
		private int BufferSize
		{
			get
			{
				return this._container.BufferSize;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000A9C9 File Offset: 0x00008BC9
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0000A9D1 File Offset: 0x00008BD1
		public DateTime ModifiedTime
		{
			get
			{
				return this._Mtime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, this._Atime, value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000A9E6 File Offset: 0x00008BE6
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x0000A9EE File Offset: 0x00008BEE
		public DateTime AccessedTime
		{
			get
			{
				return this._Atime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, value, this._Mtime);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000AA03 File Offset: 0x00008C03
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0000AA0B File Offset: 0x00008C0B
		public DateTime CreationTime
		{
			get
			{
				return this._Ctime;
			}
			set
			{
				this.SetEntryTimes(value, this._Atime, this._Mtime);
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000306C4 File Offset: 0x0002E8C4
		public void SetEntryTimes(DateTime created, DateTime accessed, DateTime modified)
		{
			this._ntfsTimesAreSet = true;
			if (created == ZipEntry._zeroHour && created.Kind == ZipEntry._zeroHour.Kind)
			{
				created = ZipEntry._win32Epoch;
			}
			if (accessed == ZipEntry._zeroHour && accessed.Kind == ZipEntry._zeroHour.Kind)
			{
				accessed = ZipEntry._win32Epoch;
			}
			if (modified == ZipEntry._zeroHour && modified.Kind == ZipEntry._zeroHour.Kind)
			{
				modified = ZipEntry._win32Epoch;
			}
			this._Ctime = created.ToUniversalTime();
			this._Atime = accessed.ToUniversalTime();
			this._Mtime = modified.ToUniversalTime();
			this._LastModified = this._Mtime;
			if (!this._emitUnixTimes && !this._emitNtfsTimes)
			{
				this._emitNtfsTimes = true;
			}
			this._metadataChanged = true;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000AA20 File Offset: 0x00008C20
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x0000AA28 File Offset: 0x00008C28
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000AA38 File Offset: 0x00008C38
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x0000AA40 File Offset: 0x00008C40
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0000AA50 File Offset: 0x00008C50
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000AA58 File Offset: 0x00008C58
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000AA60 File Offset: 0x00008C60
		public FileAttributes Attributes
		{
			get
			{
				return (FileAttributes)this._ExternalFileAttrs;
			}
			set
			{
				this._ExternalFileAttrs = (int)value;
				this._VersionMadeBy = 45;
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000AA78 File Offset: 0x00008C78
		internal string LocalFileName
		{
			get
			{
				return this._LocalFileName;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0000AA80 File Offset: 0x00008C80
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x000307A0 File Offset: 0x0002E9A0
		public string FileName
		{
			get
			{
				return this._FileNameInArchive;
			}
			set
			{
				if (this._container.ZipFile == null)
				{
					throw new ZipException("Cannot rename; this is not supported in ZipOutputStream/ZipInputStream.");
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ZipException("The FileName must be non empty and non-null.");
				}
				string text = ZipEntry.NameInArchive(value, null);
				if (this._FileNameInArchive == text)
				{
					return;
				}
				this._container.ZipFile.RemoveEntry(this);
				this._container.ZipFile.InternalAddEntry(text, this);
				this._FileNameInArchive = text;
				this._container.ZipFile.NotifyEntryChanged();
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0000AA88 File Offset: 0x00008C88
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x0000AA90 File Offset: 0x00008C90
		public Stream InputStream
		{
			get
			{
				return this._sourceStream;
			}
			set
			{
				if (this._Source != ZipEntrySource.Stream)
				{
					throw new ZipException("You must not set the input stream for this entry.");
				}
				this._sourceWasJitProvided = true;
				this._sourceStream = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		public bool InputStreamWasJitProvided
		{
			get
			{
				return this._sourceWasJitProvided;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000AABC File Offset: 0x00008CBC
		public ZipEntrySource Source
		{
			get
			{
				return this._Source;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		public short VersionNeeded
		{
			get
			{
				return this._VersionNeeded;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000AACC File Offset: 0x00008CCC
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000AAE4 File Offset: 0x00008CE4
		public bool? RequiresZip64
		{
			get
			{
				return this._entryRequiresZip64;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000AAEC File Offset: 0x00008CEC
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public short BitField
		{
			get
			{
				return this._BitField;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000AAFC File Offset: 0x00008CFC
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00030830 File Offset: 0x0002EA30
		public CompressionMethod CompressionMethod
		{
			get
			{
				return (CompressionMethod)this._CompressionMethod;
			}
			set
			{
				if (value == (CompressionMethod)this._CompressionMethod)
				{
					return;
				}
				if (value != CompressionMethod.None && value != CompressionMethod.Deflate && value != CompressionMethod.BZip2)
				{
					throw new InvalidOperationException("Unsupported compression method.");
				}
				this._CompressionMethod = (short)value;
				if (this._CompressionMethod == 0)
				{
					this._CompressionLevel = CompressionLevel.None;
				}
				else if (this.CompressionLevel == CompressionLevel.None)
				{
					this._CompressionLevel = CompressionLevel.Default;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000AB04 File Offset: 0x00008D04
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x000308AC File Offset: 0x0002EAAC
		public CompressionLevel CompressionLevel
		{
			get
			{
				return this._CompressionLevel;
			}
			set
			{
				if (this._CompressionMethod != 8 && this._CompressionMethod != 0)
				{
					return;
				}
				if (value == CompressionLevel.Default && this._CompressionMethod == 8)
				{
					return;
				}
				this._CompressionLevel = value;
				if (value == CompressionLevel.None && this._CompressionMethod == 0)
				{
					return;
				}
				if (this._CompressionLevel == CompressionLevel.None)
				{
					this._CompressionMethod = 0;
				}
				else
				{
					this._CompressionMethod = 8;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000AB0C File Offset: 0x00008D0C
		public long CompressedSize
		{
			get
			{
				return this._CompressedSize;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000AB14 File Offset: 0x00008D14
		public long UncompressedSize
		{
			get
			{
				return this._UncompressedSize;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00030928 File Offset: 0x0002EB28
		public double CompressionRatio
		{
			get
			{
				if (this.UncompressedSize == 0L)
				{
					return 0.0;
				}
				return 100.0 * (1.0 - 1.0 * (double)this.CompressedSize / (1.0 * (double)this.UncompressedSize));
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000AB1C File Offset: 0x00008D1C
		public int Crc
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0000AB24 File Offset: 0x00008D24
		public bool IsDirectory
		{
			get
			{
				return this._IsDirectory;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public bool UsesEncryption
		{
			get
			{
				return this._Encryption_FromZipFile != EncryptionAlgorithm.None;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000AB3A File Offset: 0x00008D3A
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x00030988 File Offset: 0x0002EB88
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == this._Encryption)
				{
					return;
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
				this._restreamRequiredOnSave = true;
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0000AB82 File Offset: 0x00008D82
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0000AB42 File Offset: 0x00008D42
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this._Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._Source == ZipEntrySource.ZipFile && !this._sourceIsEncrypted)
				{
					this._restreamRequiredOnSave = true;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this._Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000AB8A File Offset: 0x00008D8A
		internal bool IsChanged
		{
			get
			{
				return this._restreamRequiredOnSave | this._metadataChanged;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000AB99 File Offset: 0x00008D99
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x0000ABA1 File Offset: 0x00008DA1
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000ABAA File Offset: 0x00008DAA
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0000ABB2 File Offset: 0x00008DB2
		public ZipErrorAction ZipErrorAction { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0000ABBB File Offset: 0x00008DBB
		public bool IncludedInMostRecentSave
		{
			get
			{
				return !this._skippedDuringSave;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000ABC6 File Offset: 0x00008DC6
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000ABCE File Offset: 0x00008DCE
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000ABD7 File Offset: 0x00008DD7
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000ABF6 File Offset: 0x00008DF6
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this.AlternateEncoding == Encoding.GetEncoding("UTF-8") && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this.AlternateEncoding = Encoding.GetEncoding("UTF-8");
					this.AlternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this.AlternateEncoding = ZipFile.DefaultEncoding;
				this.AlternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000AC25 File Offset: 0x00008E25
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x0000AC2D File Offset: 0x00008E2D
		[Obsolete("This property is obsolete since v1.9.1.6. Use AlternateEncoding and AlternateEncodingUsage instead.", true)]
		public Encoding ProvisionalAlternateEncoding { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000AC36 File Offset: 0x00008E36
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000AC3E File Offset: 0x00008E3E
		public Encoding AlternateEncoding { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000AC47 File Offset: 0x00008E47
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0000AC4F File Offset: 0x00008E4F
		public ZipOption AlternateEncodingUsage { get; set; }

		// Token: 0x0600049D RID: 1181 RVA: 0x000309DC File Offset: 0x0002EBDC
		internal static string NameInArchive(string filename, string directoryPathInArchive)
		{
			string pathName;
			if (directoryPathInArchive == null)
			{
				pathName = filename;
			}
			else if (string.IsNullOrEmpty(directoryPathInArchive))
			{
				pathName = Path.GetFileName(filename);
			}
			else
			{
				pathName = Path.Combine(directoryPathInArchive, Path.GetFileName(filename));
			}
			return SharedUtilities.NormalizePathForUseInZipFile(pathName);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000AC58 File Offset: 0x00008E58
		internal static ZipEntry CreateFromNothing(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.None, null, null);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000AC63 File Offset: 0x00008E63
		internal static ZipEntry CreateFromFile(string filename, string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.FileSystem, filename, null);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000AC6E File Offset: 0x00008E6E
		internal static ZipEntry CreateForStream(string entryName, Stream s)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.Stream, s, null);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000AC79 File Offset: 0x00008E79
		internal static ZipEntry CreateForWriter(string entryName, WriteDelegate d)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.WriteDelegate, d, null);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000AC84 File Offset: 0x00008E84
		internal static ZipEntry CreateForJitStreamProvider(string nameInArchive, OpenDelegate opener, CloseDelegate closer)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.JitStream, opener, closer);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000AC8F File Offset: 0x00008E8F
		internal static ZipEntry CreateForZipOutputStream(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.ZipOutputStream, null, null);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00030A18 File Offset: 0x0002EC18
		private static ZipEntry Create(string nameInArchive, ZipEntrySource source, object arg1, object arg2)
		{
			if (string.IsNullOrEmpty(nameInArchive))
			{
				throw new ZipException("The entry name must be non-null and non-empty.");
			}
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._VersionMadeBy = 45;
			zipEntry._Source = source;
			zipEntry._Mtime = (zipEntry._Atime = (zipEntry._Ctime = DateTime.UtcNow));
			if (source == ZipEntrySource.Stream)
			{
				zipEntry._sourceStream = (arg1 as Stream);
			}
			else if (source == ZipEntrySource.WriteDelegate)
			{
				zipEntry._WriteDelegate = (arg1 as WriteDelegate);
			}
			else if (source == ZipEntrySource.JitStream)
			{
				zipEntry._OpenDelegate = (arg1 as OpenDelegate);
				zipEntry._CloseDelegate = (arg2 as CloseDelegate);
			}
			else if (source != ZipEntrySource.ZipOutputStream)
			{
				if (source == ZipEntrySource.None)
				{
					zipEntry._Source = ZipEntrySource.FileSystem;
				}
				else
				{
					string text = arg1 as string;
					if (string.IsNullOrEmpty(text))
					{
						throw new ZipException("The filename must be non-null and non-empty.");
					}
					try
					{
						zipEntry._Mtime = File.GetLastWriteTime(text).ToUniversalTime();
						zipEntry._Ctime = File.GetCreationTime(text).ToUniversalTime();
						zipEntry._Atime = File.GetLastAccessTime(text).ToUniversalTime();
						if (File.Exists(text) || Directory.Exists(text))
						{
							zipEntry._ExternalFileAttrs = (int)File.GetAttributes(text);
						}
						zipEntry._ntfsTimesAreSet = true;
						zipEntry._LocalFileName = Path.GetFullPath(text);
					}
					catch (PathTooLongException innerException)
					{
						string message = string.Format("The path is too long, filename={0}", text);
						throw new ZipException(message, innerException);
					}
				}
			}
			zipEntry._LastModified = zipEntry._Mtime;
			zipEntry._FileNameInArchive = SharedUtilities.NormalizePathForUseInZipFile(nameInArchive);
			return zipEntry;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000AC9A File Offset: 0x00008E9A
		internal void MarkAsDirectory()
		{
			this._IsDirectory = true;
			if (!this._FileNameInArchive.EndsWith("/"))
			{
				this._FileNameInArchive += "/";
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000ACCB File Offset: 0x00008ECB
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0000ACD3 File Offset: 0x00008ED3
		public bool IsText
		{
			get
			{
				return this._IsText;
			}
			set
			{
				this._IsText = value;
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000ACDC File Offset: 0x00008EDC
		public override string ToString()
		{
			return string.Format("ZipEntry::{0}", this.FileName);
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00030B98 File Offset: 0x0002ED98
		internal Stream ArchiveStream
		{
			get
			{
				if (this._archiveStream == null)
				{
					if (this._container.ZipFile != null)
					{
						ZipFile zipFile = this._container.ZipFile;
						zipFile.Reset(false);
						this._archiveStream = zipFile.StreamForDiskNumber(this._diskNumber);
					}
					else
					{
						this._archiveStream = this._container.ZipOutputStream.OutputStream;
					}
				}
				return this._archiveStream;
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00030C00 File Offset: 0x0002EE00
		private void SetFdpLoh()
		{
			long position = this.ArchiveStream.Position;
			try
			{
				this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			}
			catch (IOException innerException)
			{
				string message = string.Format("Exception seeking  entry({0}) offset(0x{1:X8}) len(0x{2:X8})", this.FileName, this._RelativeOffsetOfLocalHeader, this.ArchiveStream.Length);
				throw new BadStateException(message, innerException);
			}
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			short num = (short)((int)array[26] + (int)array[27] * 256);
			short num2 = (short)((int)array[28] + (int)array[29] * 256);
			this.ArchiveStream.Seek((long)(num + num2), SeekOrigin.Current);
			this._LengthOfHeader = (int)(30 + num2 + num) + ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
			this.__FileDataPosition = this._RelativeOffsetOfLocalHeader + (long)this._LengthOfHeader;
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000ACEE File Offset: 0x00008EEE
		private static int GetKeyStrengthInBits(EncryptionAlgorithm a)
		{
			if (a == EncryptionAlgorithm.WinZipAes256)
			{
				return 256;
			}
			if (a == EncryptionAlgorithm.WinZipAes128)
			{
				return 128;
			}
			return -1;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00030D00 File Offset: 0x0002EF00
		internal static int GetLengthOfCryptoHeaderBytes(EncryptionAlgorithm a)
		{
			if (a == EncryptionAlgorithm.None)
			{
				return 0;
			}
			if (a != EncryptionAlgorithm.WinZipAes128)
			{
				if (a != EncryptionAlgorithm.WinZipAes256)
				{
					if (a == EncryptionAlgorithm.PkzipWeak)
					{
						return 12;
					}
					throw new ZipException("internal error");
				}
			}
			int keyStrengthInBits = ZipEntry.GetKeyStrengthInBits(a);
			return keyStrengthInBits / 8 / 2 + 2;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0000AD05 File Offset: 0x00008F05
		internal long FileDataPosition
		{
			get
			{
				if (this.__FileDataPosition == -1L)
				{
					this.SetFdpLoh();
				}
				return this.__FileDataPosition;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0000AD24 File Offset: 0x00008F24
		private int LengthOfHeader
		{
			get
			{
				if (this._LengthOfHeader == 0)
				{
					this.SetFdpLoh();
				}
				return this._LengthOfHeader;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000AD3A File Offset: 0x00008F3A
		public void Extract()
		{
			this.InternalExtract(".", null, null);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000AD49 File Offset: 0x00008F49
		public void Extract(ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, null);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000AD5F File Offset: 0x00008F5F
		public void Extract(Stream stream)
		{
			this.InternalExtract(null, stream, null);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000AD6A File Offset: 0x00008F6A
		public void Extract(string baseDirectory)
		{
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000AD75 File Offset: 0x00008F75
		public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000AD87 File Offset: 0x00008F87
		public void ExtractWithPassword(string password)
		{
			this.InternalExtract(".", null, password);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000AD96 File Offset: 0x00008F96
		public void ExtractWithPassword(string baseDirectory, string password)
		{
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000ADA1 File Offset: 0x00008FA1
		public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, password);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000ADB7 File Offset: 0x00008FB7
		public void ExtractWithPassword(string baseDirectory, ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000ADC9 File Offset: 0x00008FC9
		public void ExtractWithPassword(Stream stream, string password)
		{
			this.InternalExtract(null, stream, password);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		public CrcCalculatorStream OpenReader()
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(this._Password ?? this._container.Password);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000AE09 File Offset: 0x00009009
		public CrcCalculatorStream OpenReader(string password)
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(password);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00030D40 File Offset: 0x0002EF40
		internal CrcCalculatorStream InternalOpenReader(string password)
		{
			this.ValidateCompression();
			this.ValidateEncryption();
			this.SetupCryptoForExtract(password);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling OpenReader");
			}
			long length = (this._CompressionMethod_FromZipFile == 0) ? this._CompressedFileDataSize : this.UncompressedSize;
			Stream archiveStream = this.ArchiveStream;
			this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
			this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
			Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
			return new CrcCalculatorStream(extractDecompressor, length);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000AE2A File Offset: 0x0000902A
		private void OnExtractProgress(long bytesWritten, long totalBytesToWrite)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000AE52 File Offset: 0x00009052
		private void OnBeforeExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSingleEntryExtract(this, path, true);
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000AE8C File Offset: 0x0000908C
		private void OnAfterExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._container.ZipFile.OnSingleEntryExtract(this, path, false);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000AEC1 File Offset: 0x000090C1
		private void OnExtractExisting(string path)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractExisting(this, path);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000AEE8 File Offset: 0x000090E8
		private static void ReallyDelete(string fileName)
		{
			if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			File.Delete(fileName);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000AF06 File Offset: 0x00009106
		private void WriteStatus(string format, params object[] args)
		{
			if (this._container.ZipFile != null && this._container.ZipFile.Verbose)
			{
				this._container.ZipFile.StatusMessageTextWriter.WriteLine(format, args);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00030DC8 File Offset: 0x0002EFC8
		private void InternalExtract(string baseDir, Stream outstream, string password)
		{
			if (this._container == null)
			{
				throw new BadStateException("This entry is an orphan");
			}
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use Extract() only with ZipFile.");
			}
			this._container.ZipFile.Reset(false);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
			}
			this.OnBeforeExtract(baseDir);
			this._ioOperationCanceled = false;
			string text = null;
			Stream stream = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				this.ValidateCompression();
				this.ValidateEncryption();
				if (this.ValidateOutput(baseDir, outstream, out text))
				{
					this.WriteStatus("extract dir {0}...", new object[]
					{
						text
					});
					this.OnAfterExtract(baseDir);
				}
				else
				{
					if (text != null && File.Exists(text))
					{
						flag = true;
						int num = this.CheckExtractExistingFile(baseDir, text);
						if (num == 2)
						{
							goto IL_28E;
						}
						if (num == 1)
						{
							return;
						}
					}
					string text2 = password ?? (this._Password ?? this._container.Password);
					if (this._Encryption_FromZipFile != EncryptionAlgorithm.None)
					{
						if (text2 == null)
						{
							throw new BadPasswordException();
						}
						this.SetupCryptoForExtract(text2);
					}
					if (text != null)
					{
						this.WriteStatus("extract file {0}...", new object[]
						{
							text
						});
						text += ".tmp";
						string directoryName = Path.GetDirectoryName(text);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						else if (this._container.ZipFile != null)
						{
							flag2 = this._container.ZipFile._inExtractAll;
						}
						stream = new FileStream(text, FileMode.CreateNew);
					}
					else
					{
						this.WriteStatus("extract entry {0} to stream...", new object[]
						{
							this.FileName
						});
						stream = outstream;
					}
					if (!this._ioOperationCanceled)
					{
						int actualCrc = this.ExtractOne(stream);
						if (!this._ioOperationCanceled)
						{
							this.VerifyCrcAfterExtract(actualCrc);
							if (text != null)
							{
								stream.Close();
								stream = null;
								string text3 = text;
								string text4 = null;
								text = text3.Substring(0, text3.Length - 4);
								if (flag)
								{
									text4 = text + ".PendingOverwrite";
									File.Move(text, text4);
								}
								File.Move(text3, text);
								this._SetTimes(text, true);
								if (text4 != null && File.Exists(text4))
								{
									ZipEntry.ReallyDelete(text4);
								}
								if (flag2 && this.FileName.IndexOf('/') != -1)
								{
									string directoryName2 = Path.GetDirectoryName(this.FileName);
									if (this._container.ZipFile[directoryName2] == null)
									{
										this._SetTimes(Path.GetDirectoryName(text), false);
									}
								}
								if (((int)this._VersionMadeBy & 65280) == 2560 || ((int)this._VersionMadeBy & 65280) == 0)
								{
									File.SetAttributes(text, (FileAttributes)this._ExternalFileAttrs);
								}
							}
							this.OnAfterExtract(baseDir);
						}
					}
					IL_28E:;
				}
			}
			catch (Exception)
			{
				this._ioOperationCanceled = true;
				throw;
			}
			finally
			{
				if (this._ioOperationCanceled && text != null)
				{
					if (stream != null)
					{
						stream.Close();
					}
					if (File.Exists(text) && !flag)
					{
						File.Delete(text);
					}
				}
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000310CC File Offset: 0x0002F2CC
		internal void VerifyCrcAfterExtract(int actualCrc32)
		{
			if (actualCrc32 != this._Crc32 && ((this.Encryption != EncryptionAlgorithm.WinZipAes128 && this.Encryption != EncryptionAlgorithm.WinZipAes256) || this._WinZipAesMethod != 2))
			{
				throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + string.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", this._Crc32, actualCrc32));
			}
			if (this.UncompressedSize == 0L)
			{
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				WinZipAesCipherStream winZipAesCipherStream = this._inputDecryptorStream as WinZipAesCipherStream;
				this._aesCrypto_forExtract.CalculatedMac = winZipAesCipherStream.FinalAuthentication;
				this._aesCrypto_forExtract.ReadAndVerifyMac(this.ArchiveStream);
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0003117C File Offset: 0x0002F37C
		private int CheckExtractExistingFile(string baseDir, string targetFileName)
		{
			int num = 0;
			for (;;)
			{
				switch (this.ExtractExistingFile)
				{
				default:
					goto IL_3E;
				case ExtractExistingFileAction.OverwriteSilently:
					goto IL_4F;
				case ExtractExistingFileAction.DoNotOverwrite:
					goto IL_68;
				case ExtractExistingFileAction.InvokeExtractProgressEvent:
					if (num > 0)
					{
						goto IL_8D;
					}
					this.OnExtractExisting(baseDir);
					if (this._ioOperationCanceled)
					{
						return 2;
					}
					num++;
					break;
				}
			}
			IL_3E:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
			IL_4F:
			this.WriteStatus("the file {0} exists; will overwrite it...", new object[]
			{
				targetFileName
			});
			return 0;
			IL_68:
			this.WriteStatus("the file {0} exists; not extracting entry...", new object[]
			{
				this.FileName
			});
			this.OnAfterExtract(baseDir);
			return 1;
			IL_8D:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000AF3E File Offset: 0x0000913E
		private void _CheckRead(int nbytes)
		{
			if (nbytes == 0)
			{
				throw new BadReadException(string.Format("bad read of entry {0} from compressed archive.", this.FileName));
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00031228 File Offset: 0x0002F428
		private int ExtractOne(Stream output)
		{
			int result = 0;
			Stream archiveStream = this.ArchiveStream;
			try
			{
				archiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
				byte[] array = new byte[this.BufferSize];
				long num = (this._CompressionMethod_FromZipFile != 0) ? this.UncompressedSize : this._CompressedFileDataSize;
				this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
				Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
				long num2 = 0L;
				using (CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(extractDecompressor))
				{
					while (num > 0L)
					{
						int count = (num > (long)array.Length) ? array.Length : ((int)num);
						int num3 = crcCalculatorStream.Read(array, 0, count);
						this._CheckRead(num3);
						output.Write(array, 0, num3);
						num -= (long)num3;
						num2 += (long)num3;
						this.OnExtractProgress(num2, this.UncompressedSize);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
					result = crcCalculatorStream.Crc;
				}
			}
			finally
			{
				ZipSegmentedStream zipSegmentedStream = archiveStream as ZipSegmentedStream;
				if (zipSegmentedStream != null)
				{
					zipSegmentedStream.Dispose();
					this._archiveStream = null;
				}
			}
			return result;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00031350 File Offset: 0x0002F550
		internal Stream GetExtractDecompressor(Stream input2)
		{
			short compressionMethod_FromZipFile = this._CompressionMethod_FromZipFile;
			if (compressionMethod_FromZipFile == 0)
			{
				return input2;
			}
			if (compressionMethod_FromZipFile == 8)
			{
				return new DeflateStream(input2, CompressionMode.Decompress, true);
			}
			if (compressionMethod_FromZipFile != 12)
			{
				return null;
			}
			return new BZip2InputStream(input2, true);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00031388 File Offset: 0x0002F588
		internal Stream GetExtractDecryptor(Stream input)
		{
			Stream result;
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				result = new ZipCipherStream(input, this._zipCrypto_forExtract, CryptoMode.Decrypt);
			}
			else
			{
				if (this._Encryption_FromZipFile != EncryptionAlgorithm.WinZipAes128)
				{
					if (this._Encryption_FromZipFile != EncryptionAlgorithm.WinZipAes256)
					{
						return input;
					}
				}
				result = new WinZipAesCipherStream(input, this._aesCrypto_forExtract, this._CompressedFileDataSize, CryptoMode.Decrypt);
			}
			return result;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000313E0 File Offset: 0x0002F5E0
		internal void _SetTimes(string fileOrDirectory, bool isFile)
		{
			try
			{
				if (this._ntfsTimesAreSet)
				{
					if (isFile)
					{
						if (File.Exists(fileOrDirectory))
						{
							File.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
							File.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
							File.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
						}
					}
					else if (Directory.Exists(fileOrDirectory))
					{
						Directory.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
						Directory.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
						Directory.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
					}
				}
				else
				{
					DateTime lastWriteTime = SharedUtilities.AdjustTime_Reverse(this.LastModified);
					if (isFile)
					{
						File.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
					else
					{
						Directory.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
				}
			}
			catch (IOException ex)
			{
				this.WriteStatus("failed to set time on {0}: {1}", new object[]
				{
					fileOrDirectory,
					ex.Message
				});
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000314A8 File Offset: 0x0002F6A8
		private string UnsupportedAlgorithm
		{
			get
			{
				string empty = string.Empty;
				uint unsupportedAlgorithmId = this._UnsupportedAlgorithmId;
				if (unsupportedAlgorithmId <= 26128U)
				{
					if (unsupportedAlgorithmId <= 26115U)
					{
						if (unsupportedAlgorithmId == 0U)
						{
							return "--";
						}
						switch (unsupportedAlgorithmId)
						{
						case 26113U:
							return "DES";
						case 26114U:
							return "RC2";
						case 26115U:
							return "3DES-168";
						}
					}
					else
					{
						if (unsupportedAlgorithmId == 26121U)
						{
							return "3DES-112";
						}
						switch (unsupportedAlgorithmId)
						{
						case 26126U:
							return "PKWare AES128";
						case 26127U:
							return "PKWare AES192";
						case 26128U:
							return "PKWare AES256";
						}
					}
				}
				else if (unsupportedAlgorithmId <= 26401U)
				{
					if (unsupportedAlgorithmId == 26370U)
					{
						return "RC2";
					}
					switch (unsupportedAlgorithmId)
					{
					case 26400U:
						return "Blowfish";
					case 26401U:
						return "Twofish";
					}
				}
				else
				{
					if (unsupportedAlgorithmId == 26625U)
					{
						return "RC4";
					}
					if (unsupportedAlgorithmId != 65535U)
					{
					}
				}
				return string.Format("Unknown (0x{0:X4})", this._UnsupportedAlgorithmId);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x000315D4 File Offset: 0x0002F7D4
		private string UnsupportedCompressionMethod
		{
			get
			{
				string empty = string.Empty;
				int compressionMethod = (int)this._CompressionMethod;
				if (compressionMethod <= 14)
				{
					switch (compressionMethod)
					{
					case 0:
						return "Store";
					case 1:
						return "Shrink";
					default:
						switch (compressionMethod)
						{
						case 8:
							return "DEFLATE";
						case 9:
							return "Deflate64";
						case 12:
							return "BZIP2";
						case 14:
							return "LZMA";
						}
						break;
					}
				}
				else
				{
					if (compressionMethod == 19)
					{
						return "LZ77";
					}
					if (compressionMethod == 98)
					{
						return "PPMd";
					}
				}
				return string.Format("Unknown (0x{0:X4})", this._CompressionMethod);
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00031688 File Offset: 0x0002F888
		internal void ValidateEncryption()
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak || this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256 || this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._UnsupportedAlgorithmId != 0U)
			{
				throw new ZipException(string.Format("Cannot extract: Entry {0} is encrypted with an algorithm not supported by DotNetZip: {1}", this.FileName, this.UnsupportedAlgorithm));
			}
			throw new ZipException(string.Format("Cannot extract: Entry {0} uses an unsupported encryption algorithm ({1:X2})", this.FileName, (int)this.Encryption));
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00031700 File Offset: 0x0002F900
		private void ValidateCompression()
		{
			if (this._CompressionMethod_FromZipFile != 0 && this._CompressionMethod_FromZipFile != 8 && this._CompressionMethod_FromZipFile != 12)
			{
				throw new ZipException(string.Format("Entry {0} uses an unsupported compression method (0x{1:X2}, {2})", this.FileName, this._CompressionMethod_FromZipFile, this.UnsupportedCompressionMethod));
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00031750 File Offset: 0x0002F950
		private void SetupCryptoForExtract(string password)
		{
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._Encryption_FromZipFile != EncryptionAlgorithm.PkzipWeak)
			{
				if (this._Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes128 || this._Encryption_FromZipFile == EncryptionAlgorithm.WinZipAes256)
				{
					if (password == null)
					{
						throw new ZipException("Missing password.");
					}
					if (this._aesCrypto_forExtract != null)
					{
						this._aesCrypto_forExtract.Password = password;
						return;
					}
					int lengthOfCryptoHeaderBytes = ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
					this.ArchiveStream.Seek(this.FileDataPosition - (long)lengthOfCryptoHeaderBytes, SeekOrigin.Begin);
					int keyStrengthInBits = ZipEntry.GetKeyStrengthInBits(this._Encryption_FromZipFile);
					this._aesCrypto_forExtract = WinZipAesCrypto.ReadFromStream(password, keyStrengthInBits, this.ArchiveStream);
				}
				return;
			}
			if (password == null)
			{
				throw new ZipException("Missing password.");
			}
			this.ArchiveStream.Seek(this.FileDataPosition - 12L, SeekOrigin.Begin);
			this._zipCrypto_forExtract = ZipCrypto.ForRead(password, this);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00031820 File Offset: 0x0002FA20
		private bool ValidateOutput(string basedir, Stream outstream, out string outFileName)
		{
			if (basedir != null)
			{
				string text = this.FileName.Replace("\\", "/");
				if (text.IndexOf(':') == 1)
				{
					text = text.Substring(2);
				}
				if (text.StartsWith("/"))
				{
					text = text.Substring(1);
				}
				if (this._container.ZipFile.FlattenFoldersOnExtract)
				{
					outFileName = Path.Combine(basedir, (text.IndexOf('/') != -1) ? Path.GetFileName(text) : text);
				}
				else
				{
					outFileName = Path.Combine(basedir, text);
				}
				outFileName = outFileName.Replace("/", "\\");
				if (!this.IsDirectory && !this.FileName.EndsWith("/"))
				{
					return false;
				}
				if (!Directory.Exists(outFileName))
				{
					Directory.CreateDirectory(outFileName);
					this._SetTimes(outFileName, false);
				}
				else if (this.ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
				{
					this._SetTimes(outFileName, false);
				}
				return true;
			}
			else
			{
				if (outstream != null)
				{
					outFileName = null;
					return this.IsDirectory || this.FileName.EndsWith("/");
				}
				throw new ArgumentNullException("outstream");
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00031938 File Offset: 0x0002FB38
		private void ReadExtraField()
		{
			this._readExtraDepth++;
			long position = this.ArchiveStream.Position;
			this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			short num = (short)((int)array[26] + (int)array[27] * 256);
			short extraFieldLength = (short)((int)array[28] + (int)array[29] * 256);
			this.ArchiveStream.Seek((long)num, SeekOrigin.Current);
			this.ProcessExtraField(this.ArchiveStream, extraFieldLength);
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
			this._readExtraDepth--;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000319E8 File Offset: 0x0002FBE8
		private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
		{
			ze._RelativeOffsetOfLocalHeader = ze.ArchiveStream.Position;
			int num = SharedUtilities.ReadEntrySignature(ze.ArchiveStream);
			int num2 = 4;
			if (ZipEntry.IsNotValidSig(num))
			{
				ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
				if (ZipEntry.IsNotValidZipDirEntrySig(num) && (long)num != 101010256L)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position  0x{1:X8}", num, ze.ArchiveStream.Position));
				}
				return false;
			}
			else
			{
				byte[] array = new byte[26];
				int num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return false;
				}
				num2 += num3;
				ze._VersionNeeded = (short)((int)array[0] + (int)array[1] * 256);
				ze._BitField = (short)((int)array[2] + (int)array[3] * 256);
				ze._CompressionMethod_FromZipFile = (ze._CompressionMethod = (short)((int)array[4] + (int)array[5] * 256));
				int num4 = (int)array[6] + (int)array[7] * 256 + (int)array[8] * 256 * 256;
				byte[] array2 = array;
				int num5 = 9;
				int num6 = 10;
				ze._TimeBlob = num4 + array2[num5] * 256 * 256 * 256;
				ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
				ze._timestamp |= ZipEntryTimestamp.DOS;
				if ((ze._BitField & 1) == 1)
				{
					ze._Encryption = EncryptionAlgorithm.PkzipWeak;
					ze._Encryption_FromZipFile = EncryptionAlgorithm.PkzipWeak;
					ze._sourceIsEncrypted = true;
				}
				ze._Crc32 = (int)array[num6++] + (int)array[num6++] * 256 + (int)array[num6++] * 256 * 256 + (int)array[num6++] * 256 * 256 * 256;
				ze._CompressedSize = (long)((ulong)((int)array[num6++] + (int)array[num6++] * 256 + (int)array[num6++] * 256 * 256 + (int)array[num6++] * 256 * 256 * 256));
				ze._UncompressedSize = (long)((ulong)((int)array[num6++] + (int)array[num6++] * 256 + (int)array[num6++] * 256 * 256 + (int)array[num6++] * 256 * 256 * 256));
				if ((uint)ze._CompressedSize == 4294967295U || (uint)ze._UncompressedSize == 4294967295U)
				{
					ze._InputUsesZip64 = true;
				}
				short num7 = (short)((int)array[num6++] + (int)array[num6++] * 256);
				short extraFieldLength = (short)((int)array[num6++] + (int)array[num6++] * 256);
				array = new byte[(int)num7];
				num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				num2 += num3;
				if ((ze._BitField & 2048) == 2048)
				{
					ze.AlternateEncoding = Encoding.UTF8;
					ze.AlternateEncodingUsage = ZipOption.Always;
				}
				ze._FileNameInArchive = ze.AlternateEncoding.GetString(array, 0, array.Length);
				if (ze._FileNameInArchive.EndsWith("/"))
				{
					ze.MarkAsDirectory();
				}
				num2 += ze.ProcessExtraField(ze.ArchiveStream, extraFieldLength);
				ze._LengthOfTrailer = 0;
				if (!ze._FileNameInArchive.EndsWith("/") && (ze._BitField & 8) == 8)
				{
					long position = ze.ArchiveStream.Position;
					bool flag = true;
					long num8 = 0L;
					int num9 = 0;
					while (flag)
					{
						num9++;
						if (ze._container.ZipFile != null)
						{
							ze._container.ZipFile.OnReadBytes(ze);
						}
						long num10 = SharedUtilities.FindSignature(ze.ArchiveStream, 134695760);
						if (num10 == -1L)
						{
							return false;
						}
						num8 += num10;
						if (ze._InputUsesZip64)
						{
							array = new byte[20];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 20)
							{
								return false;
							}
							ze._Crc32 = (int)array[0] + (int)array[1] * 256 + (int)array[2] * 256 * 256 + (int)array[3] * 256 * 256 * 256;
							ze._CompressedSize = BitConverter.ToInt64(array, 4);
							ze._UncompressedSize = BitConverter.ToInt64(array, 12);
							ze._LengthOfTrailer += 24;
						}
						else
						{
							array = new byte[12];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 12)
							{
								return false;
							}
							ze._Crc32 = (int)array[0] + (int)array[1] * 256 + (int)array[2] * 256 * 256 + (int)array[3] * 256 * 256 * 256;
							ze._CompressedSize = (long)((ulong)((int)array[4] + (int)array[5] * 256 + (int)array[6] * 256 * 256 + (int)array[7] * 256 * 256 * 256));
							ze._UncompressedSize = (long)((ulong)((int)array[8] + (int)array[9] * 256 + (int)array[10] * 256 * 256 + (int)array[11] * 256 * 256 * 256));
							ze._LengthOfTrailer += 16;
						}
						if (flag = (num8 != ze._CompressedSize))
						{
							ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
							num8 += 4L;
						}
					}
					ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
				}
				ze._CompressedFileDataSize = ze._CompressedSize;
				if ((ze._BitField & 1) == 1)
				{
					if (ze.Encryption != EncryptionAlgorithm.WinZipAes128)
					{
						if (ze.Encryption != EncryptionAlgorithm.WinZipAes256)
						{
							ze._WeakEncryptionHeader = new byte[12];
							num2 += ZipEntry.ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
							ze._CompressedFileDataSize -= 12L;
							goto IL_6B8;
						}
					}
					int keyStrengthInBits = ZipEntry.GetKeyStrengthInBits(ze._Encryption_FromZipFile);
					ze._aesCrypto_forExtract = WinZipAesCrypto.ReadFromStream(null, keyStrengthInBits, ze.ArchiveStream);
					num2 += ze._aesCrypto_forExtract.SizeOfEncryptionMetadata - 10;
					ze._CompressedFileDataSize -= (long)ze._aesCrypto_forExtract.SizeOfEncryptionMetadata;
					ze._LengthOfTrailer += 10;
				}
				IL_6B8:
				ze._LengthOfHeader = num2;
				ze._TotalEntrySize = (long)ze._LengthOfHeader + ze._CompressedFileDataSize + (long)ze._LengthOfTrailer;
				return true;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000320D4 File Offset: 0x000302D4
		internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
		{
			int num = s.Read(buffer, 0, 12);
			if (num != 12)
			{
				throw new ZipException(string.Format("Unexpected end of data at position 0x{0:X8}", s.Position));
			}
			return num;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000AF59 File Offset: 0x00009159
		private static bool IsNotValidSig(int signature)
		{
			return signature != 67324752;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00032110 File Offset: 0x00030310
		internal static ZipEntry ReadEntry(ZipContainer zc, bool first)
		{
			ZipFile zipFile = zc.ZipFile;
			Stream readStream = zc.ReadStream;
			Encoding alternateEncoding = zc.AlternateEncoding;
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._Source = ZipEntrySource.ZipFile;
			zipEntry._container = zc;
			zipEntry._archiveStream = readStream;
			if (zipFile != null)
			{
				zipFile.OnReadEntry(true, null);
			}
			if (first)
			{
				ZipEntry.HandlePK00Prefix(readStream);
			}
			if (!ZipEntry.ReadHeader(zipEntry, alternateEncoding))
			{
				return null;
			}
			zipEntry.__FileDataPosition = zipEntry.ArchiveStream.Position;
			readStream.Seek(zipEntry._CompressedFileDataSize + (long)zipEntry._LengthOfTrailer, SeekOrigin.Current);
			ZipEntry.HandleUnexpectedDataDescriptor(zipEntry);
			if (zipFile != null)
			{
				zipFile.OnReadBytes(zipEntry);
				zipFile.OnReadEntry(false, zipEntry);
			}
			return zipEntry;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000321AC File Offset: 0x000303AC
		internal static void HandlePK00Prefix(Stream s)
		{
			uint num = (uint)SharedUtilities.ReadInt(s);
			if (num != 808471376U)
			{
				s.Seek(-4L, SeekOrigin.Current);
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000321DC File Offset: 0x000303DC
		private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
		{
			Stream archiveStream = entry.ArchiveStream;
			uint num = (uint)SharedUtilities.ReadInt(archiveStream);
			if ((ulong)num != (ulong)((long)entry._Crc32))
			{
				archiveStream.Seek(-4L, SeekOrigin.Current);
				return;
			}
			int num2 = SharedUtilities.ReadInt(archiveStream);
			if ((long)num2 != entry._CompressedSize)
			{
				archiveStream.Seek(-8L, SeekOrigin.Current);
				return;
			}
			num2 = SharedUtilities.ReadInt(archiveStream);
			if ((long)num2 == entry._UncompressedSize)
			{
				return;
			}
			archiveStream.Seek(-12L, SeekOrigin.Current);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0003225C File Offset: 0x0003045C
		internal static int FindExtraFieldSegment(byte[] extra, int offx, ushort targetHeaderId)
		{
			int num = offx;
			while (num + 3 < extra.Length)
			{
				ushort num2 = (ushort)((int)extra[num++] + (int)extra[num++] * 256);
				if (num2 == targetHeaderId)
				{
					return num - 2;
				}
				short num3 = (short)((int)extra[num++] + (int)extra[num++] * 256);
				num += (int)num3;
			}
			return -1;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000322B4 File Offset: 0x000304B4
		internal int ProcessExtraField(Stream s, short extraFieldLength)
		{
			int num = 0;
			if (extraFieldLength > 0)
			{
				byte[] array = this._Extra = new byte[(int)extraFieldLength];
				num = s.Read(array, 0, array.Length);
				long posn = s.Position - (long)num;
				int num2 = 0;
				while (num2 + 3 < array.Length)
				{
					int num3 = num2;
					ushort num4 = (ushort)((int)array[num2++] + (int)array[num2++] * 256);
					short num5 = (short)((int)array[num2++] + (int)array[num2++] * 256);
					ushort num6 = num4;
					if (num6 <= 21589)
					{
						if (num6 <= 10)
						{
							if (num6 != 1)
							{
								if (num6 == 10)
								{
									num2 = this.ProcessExtraFieldWindowsTimes(array, num2, num5, posn);
								}
							}
							else
							{
								num2 = this.ProcessExtraFieldZip64(array, num2, num5, posn);
							}
						}
						else if (num6 != 23)
						{
							if (num6 == 21589)
							{
								num2 = this.ProcessExtraFieldUnixTimes(array, num2, num5, posn);
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldPkwareStrongEncryption(array, num2);
						}
					}
					else if (num6 <= 30805)
					{
						if (num6 != 22613)
						{
							if (num6 != 30805)
							{
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldInfoZipTimes(array, num2, num5, posn);
						}
					}
					else if (num6 != 30837)
					{
						if (num6 == 39169)
						{
							num2 = this.ProcessExtraFieldWinZipAes(array, num2, num5, posn);
						}
					}
					num2 = num3 + (int)num5 + 4;
				}
			}
			return num;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000AF66 File Offset: 0x00009166
		private int ProcessExtraFieldPkwareStrongEncryption(byte[] Buffer, int j)
		{
			j += 2;
			this._UnsupportedAlgorithmId = (uint)((ushort)((int)Buffer[j++] + (int)Buffer[j++] * 256));
			this._Encryption = EncryptionAlgorithm.Unsupported;
			this._Encryption_FromZipFile = EncryptionAlgorithm.Unsupported;
			return j;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00032410 File Offset: 0x00030610
		private int ProcessExtraFieldWinZipAes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (this._CompressionMethod == 99)
			{
				if ((this._BitField & 1) != 1)
				{
					throw new BadReadException(string.Format("  Inconsistent metadata at position 0x{0:X16}", posn));
				}
				this._sourceIsEncrypted = true;
				if (dataSize != 7)
				{
					throw new BadReadException(string.Format("  Inconsistent size (0x{0:X4}) in WinZip AES field at position 0x{1:X16}", dataSize, posn));
				}
				this._WinZipAesMethod = BitConverter.ToInt16(buffer, j);
				j += 2;
				if (this._WinZipAesMethod != 1 && this._WinZipAesMethod != 2)
				{
					throw new BadReadException(string.Format("  Unexpected vendor version number (0x{0:X4}) for WinZip AES metadata at position 0x{1:X16}", this._WinZipAesMethod, posn));
				}
				short num = BitConverter.ToInt16(buffer, j);
				j += 2;
				if (num != 17729)
				{
					throw new BadReadException(string.Format("  Unexpected vendor ID (0x{0:X4}) for WinZip AES metadata at position 0x{1:X16}", num, posn));
				}
				int num2 = (buffer[j] == 1) ? 128 : ((buffer[j] == 3) ? 256 : -1);
				if (num2 < 0)
				{
					throw new BadReadException(string.Format("Invalid key strength ({0})", num2));
				}
				this._Encryption_FromZipFile = (this._Encryption = ((num2 == 128) ? EncryptionAlgorithm.WinZipAes128 : EncryptionAlgorithm.WinZipAes256));
				j++;
				this._CompressionMethod_FromZipFile = (this._CompressionMethod = BitConverter.ToInt16(buffer, j));
				j += 2;
			}
			return j;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00032560 File Offset: 0x00030760
		private int ProcessExtraFieldZip64(byte[] buffer, int j, short dataSize, long posn)
		{
			this._InputUsesZip64 = true;
			if (dataSize > 28)
			{
				throw new BadReadException(string.Format("  Inconsistent size (0x{0:X4}) for ZIP64 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<long> func = delegate()
			{
				if (remainingData < 8)
				{
					throw new BadReadException(string.Format("  Missing data for ZIP64 extra field, position 0x{0:X16}", posn));
				}
				long result = BitConverter.ToInt64(buffer, j);
				j += 8;
				remainingData -= 8;
				return result;
			};
			if (this._UncompressedSize == 4294967295L)
			{
				this._UncompressedSize = func();
			}
			if (this._CompressedSize == 4294967295L)
			{
				this._CompressedSize = func();
			}
			if (this._RelativeOffsetOfLocalHeader == 4294967295L)
			{
				this._RelativeOffsetOfLocalHeader = func();
			}
			return j;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00032628 File Offset: 0x00030828
		private int ProcessExtraFieldInfoZipTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 12 && dataSize != 8)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for InfoZip v1 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int num = BitConverter.ToInt32(buffer, j);
			this._Mtime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			num = BitConverter.ToInt32(buffer, j);
			this._Atime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			this._Ctime = DateTime.UtcNow;
			this._ntfsTimesAreSet = true;
			this._timestamp |= ZipEntryTimestamp.InfoZip1;
			return j;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000326BC File Offset: 0x000308BC
		private int ProcessExtraFieldUnixTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 13 && dataSize != 9 && dataSize != 5)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for Extended Timestamp extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<DateTime> func = delegate()
			{
				int num = BitConverter.ToInt32(buffer, j);
				j += 4;
				remainingData -= 4;
				return ZipEntry._unixEpoch.AddSeconds((double)num);
			};
			if (dataSize != 13 && this._readExtraDepth <= 0)
			{
				this.ReadExtraField();
			}
			else
			{
				byte b = buffer[j++];
				remainingData--;
				if ((b & 1) != 0 && remainingData >= 4)
				{
					this._Mtime = func();
				}
				this._Atime = (((b & 2) == 0 || remainingData < 4) ? DateTime.UtcNow : func());
				this._Ctime = (((b & 4) == 0 || remainingData < 4) ? DateTime.UtcNow : func());
				this._timestamp |= ZipEntryTimestamp.Unix;
				this._ntfsTimesAreSet = true;
				this._emitUnixTimes = true;
			}
			return j;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000327E4 File Offset: 0x000309E4
		private int ProcessExtraFieldWindowsTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 32)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for NTFS times extra field at position 0x{1:X16}", dataSize, posn));
			}
			j += 4;
			short num = (short)((int)buffer[j] + (int)buffer[j + 1] * 256);
			short num2 = (short)((int)buffer[j + 2] + (int)buffer[j + 3] * 256);
			j += 4;
			if (num == 1 && num2 == 24)
			{
				long fileTime = BitConverter.ToInt64(buffer, j);
				this._Mtime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Atime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Ctime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				this._ntfsTimesAreSet = true;
				this._timestamp |= ZipEntryTimestamp.Windows;
				this._emitNtfsTimes = true;
			}
			return j;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000328B4 File Offset: 0x00030AB4
		internal void WriteCentralDirectoryEntry(Stream s)
		{
			byte[] array = new byte[4096];
			array[0] = 80;
			array[1] = 75;
			array[2] = 1;
			array[3] = 2;
			array[4] = (byte)(this._VersionMadeBy & 255);
			byte[] array2 = array;
			int num = 5;
			int num2 = 6;
			array2[num] = (byte)(((int)this._VersionMadeBy & 65280) >> 8);
			short num3 = (this.VersionNeeded != 0) ? this.VersionNeeded : 20;
			if (this._OutputUsesZip64 == null)
			{
				this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always);
			}
			short num4 = this._OutputUsesZip64.Value ? 45 : num3;
			if (this.CompressionMethod == CompressionMethod.BZip2)
			{
				num4 = 46;
			}
			array[num2++] = (byte)(num4 & 255);
			array[num2++] = (byte)(((int)num4 & 65280) >> 8);
			array[num2++] = (byte)(this._BitField & 255);
			array[num2++] = (byte)(((int)this._BitField & 65280) >> 8);
			array[num2++] = (byte)(this._CompressionMethod & 255);
			array[num2++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				num2 -= 2;
				array[num2++] = 99;
				array[num2++] = 0;
			}
			array[num2++] = (byte)(this._TimeBlob & 255);
			array[num2++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num2++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num2++] = (byte)(((long)this._TimeBlob & 4278190080L) >> 24);
			array[num2++] = (byte)(this._Crc32 & 255);
			array[num2++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num2++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num2++] = (byte)(((long)this._Crc32 & 4278190080L) >> 24);
			if (this._OutputUsesZip64.Value)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num2++] = byte.MaxValue;
				}
			}
			else
			{
				array[num2++] = (byte)(this._CompressedSize & 255L);
				array[num2++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num2++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num2++] = (byte)((this._CompressedSize & 4278190080L) >> 24);
				array[num2++] = (byte)(this._UncompressedSize & 255L);
				array[num2++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num2++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num2++] = (byte)((this._UncompressedSize & 4278190080L) >> 24);
			}
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num5 = (short)encodedFileNameBytes.Length;
			array[num2++] = (byte)(num5 & 255);
			array[num2++] = (byte)(((int)num5 & 65280) >> 8);
			this._presumeZip64 = this._OutputUsesZip64.Value;
			this._Extra = this.ConstructExtraField(true);
			short num6 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num2++] = (byte)(num6 & 255);
			array[num2++] = (byte)(((int)num6 & 65280) >> 8);
			int num7 = (this._CommentBytes == null) ? 0 : this._CommentBytes.Length;
			if (num7 + num2 > array.Length)
			{
				num7 = array.Length - num2;
			}
			array[num2++] = (byte)(num7 & 255);
			array[num2++] = (byte)((num7 & 65280) >> 8);
			if (this._container.ZipFile != null && this._container.ZipFile.MaxOutputSegmentSize != 0)
			{
				array[num2++] = (byte)(this._diskNumber & 255U);
				array[num2++] = (byte)((this._diskNumber & 65280U) >> 8);
			}
			else
			{
				array[num2++] = 0;
				array[num2++] = 0;
			}
			array[num2++] = (this._IsText ? 1 : 0);
			array[num2++] = 0;
			array[num2++] = (byte)(this._ExternalFileAttrs & 255);
			array[num2++] = (byte)((this._ExternalFileAttrs & 65280) >> 8);
			array[num2++] = (byte)((this._ExternalFileAttrs & 16711680) >> 16);
			array[num2++] = (byte)(((long)this._ExternalFileAttrs & 4278190080L) >> 24);
			if (this._RelativeOffsetOfLocalHeader > 4294967295L)
			{
				array[num2++] = byte.MaxValue;
				array[num2++] = byte.MaxValue;
				array[num2++] = byte.MaxValue;
				array[num2++] = byte.MaxValue;
			}
			else
			{
				array[num2++] = (byte)(this._RelativeOffsetOfLocalHeader & 255L);
				array[num2++] = (byte)((this._RelativeOffsetOfLocalHeader & 65280L) >> 8);
				array[num2++] = (byte)((this._RelativeOffsetOfLocalHeader & 16711680L) >> 16);
				array[num2++] = (byte)((this._RelativeOffsetOfLocalHeader & 4278190080L) >> 24);
			}
			Buffer.BlockCopy(encodedFileNameBytes, 0, array, num2, (int)num5);
			num2 += (int)num5;
			if (this._Extra != null)
			{
				byte[] extra = this._Extra;
				Buffer.BlockCopy(extra, 0, array, num2, (int)num6);
				num2 += (int)num6;
			}
			if (num7 != 0)
			{
				Buffer.BlockCopy(this._CommentBytes, 0, array, num2, num7);
				num2 += num7;
			}
			s.Write(array, 0, num2);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00032E6C File Offset: 0x0003106C
		private byte[] ConstructExtraField(bool forCentralDirectory)
		{
			List<byte[]> list = new List<byte[]>();
			if (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && (!forCentralDirectory || this._entryRequiresZip64.Value)))
			{
				int num = 4 + (forCentralDirectory ? 28 : 16);
				byte[] array = new byte[num];
				int num2 = 0;
				if (!this._presumeZip64 && !forCentralDirectory)
				{
					array[num2++] = 153;
					array[num2++] = 153;
				}
				else
				{
					array[num2++] = 1;
					array[num2++] = 0;
				}
				array[num2++] = (byte)(num - 4);
				array[num2++] = 0;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num2, 8);
				num2 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num2, 8);
				num2 += 8;
				if (forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, array, num2, 8);
					num2 += 8;
					Array.Copy(BitConverter.GetBytes(0), 0, array, num2, 4);
				}
				list.Add(array);
			}
			if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				byte[] array = new byte[11];
				array[0] = 1;
				array[1] = 153;
				array[2] = 7;
				array[3] = 0;
				array[4] = 1;
				array[5] = 0;
				array[6] = 65;
				byte[] array2 = array;
				int num3 = 7;
				int num4 = 8;
				array2[num3] = 69;
				int keyStrengthInBits = ZipEntry.GetKeyStrengthInBits(this.Encryption);
				if (keyStrengthInBits == 128)
				{
					array[num4] = 1;
				}
				else if (keyStrengthInBits == 256)
				{
					array[num4] = 3;
				}
				else
				{
					array[num4] = byte.MaxValue;
				}
				num4++;
				array[num4++] = (byte)(this._CompressionMethod & 255);
				array[num4++] = (byte)((int)this._CompressionMethod & 65280);
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitNtfsTimes)
			{
				byte[] array = new byte[36];
				array[0] = 10;
				array[1] = 0;
				array[2] = 32;
				array[3] = 0;
				array[8] = 1;
				array[9] = 0;
				array[10] = 24;
				array[11] = 0;
				long value = this._Mtime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, 12, 8);
				value = this._Atime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, 20, 8);
				value = this._Ctime.ToFileTime();
				Array.Copy(BitConverter.GetBytes(value), 0, array, 28, 8);
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitUnixTimes)
			{
				int num5 = 9;
				if (!forCentralDirectory)
				{
					num5 += 8;
				}
				byte[] array = new byte[num5];
				array[0] = 85;
				array[1] = 84;
				array[2] = (byte)(num5 - 4);
				array[3] = 0;
				array[4] = 7;
				int value2 = (int)(this._Mtime - ZipEntry._unixEpoch).TotalSeconds;
				Array.Copy(BitConverter.GetBytes(value2), 0, array, 5, 4);
				int num6 = 9;
				if (!forCentralDirectory)
				{
					value2 = (int)(this._Atime - ZipEntry._unixEpoch).TotalSeconds;
					Array.Copy(BitConverter.GetBytes(value2), 0, array, num6, 4);
					num6 += 4;
					value2 = (int)(this._Ctime - ZipEntry._unixEpoch).TotalSeconds;
					Array.Copy(BitConverter.GetBytes(value2), 0, array, num6, 4);
					num6 += 4;
				}
				list.Add(array);
			}
			byte[] array3 = null;
			if (list.Count > 0)
			{
				int num7 = 0;
				int num8 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num7 += list[i].Length;
				}
				array3 = new byte[num7];
				for (int i = 0; i < list.Count; i++)
				{
					Array.Copy(list[i], 0, array3, num8, list[i].Length);
					num8 += list[i].Length;
				}
			}
			return array3;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0003325C File Offset: 0x0003145C
		private string NormalizeFileName()
		{
			string text = this.FileName.Replace("\\", "/");
			string result;
			if (this._TrimVolumeFromFullyQualifiedPaths && this.FileName.Length >= 3 && this.FileName[1] == ':' && text[2] == '/')
			{
				result = text.Substring(3);
			}
			else if (this.FileName.Length >= 4 && text[0] == '/' && text[1] == '/')
			{
				int num = text.IndexOf('/', 2);
				if (num == -1)
				{
					throw new ArgumentException("The path for that entry appears to be badly formatted");
				}
				result = text.Substring(num + 1);
			}
			else if (this.FileName.Length >= 3 && text[0] == '.' && text[1] == '/')
			{
				result = text.Substring(2);
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00033338 File Offset: 0x00031538
		private byte[] GetEncodedFileNameBytes()
		{
			string text = this.NormalizeFileName();
			switch (this.AlternateEncodingUsage)
			{
			case ZipOption.Default:
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = ZipEntry.ibm437.GetBytes(this._Comment);
				}
				this._actualEncoding = ZipEntry.ibm437;
				return ZipEntry.ibm437.GetBytes(text);
			case ZipOption.Always:
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return this.AlternateEncoding.GetBytes(text);
			}
			byte[] bytes = ZipEntry.ibm437.GetBytes(text);
			string @string = ZipEntry.ibm437.GetString(bytes, 0, bytes.Length);
			this._CommentBytes = null;
			if (@string != text)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._actualEncoding = ZipEntry.ibm437;
			if (this._Comment == null || this._Comment.Length == 0)
			{
				return bytes;
			}
			byte[] bytes2 = ZipEntry.ibm437.GetBytes(this._Comment);
			string string2 = ZipEntry.ibm437.GetString(bytes2, 0, bytes2.Length);
			if (string2 != this.Comment)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._CommentBytes = bytes2;
			return bytes;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000334F4 File Offset: 0x000316F4
		private bool WantReadAgain()
		{
			return this._UncompressedSize >= 16L && this._CompressionMethod != 0 && this.CompressionLevel != CompressionLevel.None && this._CompressedSize >= this._UncompressedSize && (this._Source != ZipEntrySource.Stream || this._sourceStream.CanSeek) && (this._aesCrypto_forWrite == null || this.CompressedSize - (long)this._aesCrypto_forWrite.SizeOfEncryptionMetadata > this.UncompressedSize + 16L) && (this._zipCrypto_forWrite == null || this.CompressedSize - 12L > this.UncompressedSize);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000335A4 File Offset: 0x000317A4
		private void MaybeUnsetCompressionMethodForWriting(int cycle)
		{
			if (cycle > 1)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.IsDirectory)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				return;
			}
			if (this._Source == ZipEntrySource.Stream)
			{
				if (this._sourceStream != null && this._sourceStream.CanSeek)
				{
					long length = this._sourceStream.Length;
					if (length == 0L)
					{
						this._CompressionMethod = 0;
						return;
					}
				}
			}
			else if (this._Source == ZipEntrySource.FileSystem && SharedUtilities.GetFileLength(this.LocalFileName) == 0L)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.SetCompression != null)
			{
				this.CompressionLevel = this.SetCompression(this.LocalFileName, this._FileNameInArchive);
			}
			if (this.CompressionLevel == CompressionLevel.None && this.CompressionMethod == CompressionMethod.Deflate)
			{
				this._CompressionMethod = 0;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0003367C File Offset: 0x0003187C
		internal void WriteHeader(Stream s, int cycle)
		{
			CountingStream countingStream = s as CountingStream;
			this._future_ROLH = ((countingStream != null) ? countingStream.ComputedPosition : s.Position);
			byte[] array = new byte[30];
			array[0] = 80;
			array[1] = 75;
			array[2] = 3;
			byte[] array2 = array;
			int num = 3;
			int num2 = 4;
			array2[num] = 4;
			this._presumeZip64 = (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && !s.CanSeek));
			short num3 = this._presumeZip64 ? 45 : 20;
			if (this.CompressionMethod == CompressionMethod.BZip2)
			{
				num3 = 46;
			}
			array[num2++] = (byte)(num3 & 255);
			array[num2++] = (byte)(((int)num3 & 65280) >> 8);
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num4 = (short)encodedFileNameBytes.Length;
			if (this._Encryption == EncryptionAlgorithm.None)
			{
				this._BitField &= -2;
			}
			else
			{
				this._BitField |= 1;
			}
			if (this._actualEncoding.CodePage == Encoding.UTF8.CodePage)
			{
				this._BitField |= 2048;
			}
			if (!this.IsDirectory)
			{
				if (cycle != 99)
				{
					if (!s.CanSeek)
					{
						this._BitField |= 8;
						goto IL_16E;
					}
					goto IL_16E;
				}
			}
			this._BitField &= -9;
			this._BitField &= -2;
			this.Encryption = EncryptionAlgorithm.None;
			this.Password = null;
			IL_16E:
			array[num2++] = (byte)(this._BitField & 255);
			array[num2++] = (byte)(((int)this._BitField & 65280) >> 8);
			if (this.__FileDataPosition == -1L)
			{
				this._CompressedSize = 0L;
				this._crcCalculated = false;
			}
			this.MaybeUnsetCompressionMethodForWriting(cycle);
			array[num2++] = (byte)(this._CompressionMethod & 255);
			array[num2++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			if (cycle == 99)
			{
				this.SetZip64Flags();
			}
			else if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				num2 -= 2;
				array[num2++] = 99;
				array[num2++] = 0;
			}
			this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
			array[num2++] = (byte)(this._TimeBlob & 255);
			array[num2++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num2++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num2++] = (byte)(((long)this._TimeBlob & 4278190080L) >> 24);
			array[num2++] = (byte)(this._Crc32 & 255);
			array[num2++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num2++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num2++] = (byte)(((long)this._Crc32 & 4278190080L) >> 24);
			if (this._presumeZip64)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num2++] = byte.MaxValue;
				}
			}
			else
			{
				array[num2++] = (byte)(this._CompressedSize & 255L);
				array[num2++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num2++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num2++] = (byte)((this._CompressedSize & 4278190080L) >> 24);
				array[num2++] = (byte)(this._UncompressedSize & 255L);
				array[num2++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num2++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num2++] = (byte)((this._UncompressedSize & 4278190080L) >> 24);
			}
			array[num2++] = (byte)(num4 & 255);
			array[num2++] = (byte)(((int)num4 & 65280) >> 8);
			this._Extra = this.ConstructExtraField(false);
			short num5 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num2++] = (byte)(num5 & 255);
			array[num2++] = (byte)(((int)num5 & 65280) >> 8);
			byte[] array3 = new byte[num2 + (int)num4 + (int)num5];
			Buffer.BlockCopy(array, 0, array3, 0, num2);
			Buffer.BlockCopy(encodedFileNameBytes, 0, array3, num2, encodedFileNameBytes.Length);
			num2 += encodedFileNameBytes.Length;
			if (this._Extra != null)
			{
				Buffer.BlockCopy(this._Extra, 0, array3, num2, this._Extra.Length);
				num2 += this._Extra.Length;
			}
			this._LengthOfHeader = num2;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
				uint num6 = zipSegmentedStream.ComputeSegment(num2);
				if (num6 != zipSegmentedStream.CurrentSegment)
				{
					this._future_ROLH = 0L;
				}
				else
				{
					this._future_ROLH = zipSegmentedStream.Position;
				}
				this._diskNumber = num6;
			}
			if (this._container.Zip64 == Zip64Option.Default && (uint)this._RelativeOffsetOfLocalHeader >= 4294967295U)
			{
				throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			s.Write(array3, 0, num2);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			this._EntryHeader = array3;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00033BC4 File Offset: 0x00031DC4
		private int FigureCrc32()
		{
			if (!this._crcCalculated)
			{
				Stream stream = null;
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(Stream.Null);
					this._WriteDelegate(this.FileName, crcCalculatorStream);
					this._Crc32 = crcCalculatorStream.Crc;
				}
				else if (this._Source != ZipEntrySource.ZipFile)
				{
					if (this._Source == ZipEntrySource.Stream)
					{
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source == ZipEntrySource.JitStream)
					{
						if (this._sourceStream == null)
						{
							this._sourceStream = this._OpenDelegate(this.FileName);
						}
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source != ZipEntrySource.ZipOutputStream)
					{
						stream = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					}
					CRC32 crc = new CRC32();
					this._Crc32 = crc.GetCrc32(stream);
					if (this._sourceStream == null)
					{
						stream.Dispose();
					}
				}
				this._crcCalculated = true;
			}
			return this._Crc32;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00033CB8 File Offset: 0x00031EB8
		private void PrepSourceStream()
		{
			if (this._sourceStream == null)
			{
				throw new ZipException(string.Format("The input stream is null for entry '{0}'.", this.FileName));
			}
			if (this._sourceStreamOriginalPosition != null)
			{
				this._sourceStream.Position = this._sourceStreamOriginalPosition.Value;
				return;
			}
			if (this._sourceStream.CanSeek)
			{
				this._sourceStreamOriginalPosition = new long?(this._sourceStream.Position);
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak && this._Source != ZipEntrySource.ZipFile && (this._BitField & 8) != 8)
			{
				throw new ZipException("It is not possible to use PKZIP encryption on a non-seekable input stream");
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00033D54 File Offset: 0x00031F54
		internal void CopyMetaData(ZipEntry source)
		{
			this.__FileDataPosition = source.__FileDataPosition;
			this.CompressionMethod = source.CompressionMethod;
			this._CompressionMethod_FromZipFile = source._CompressionMethod_FromZipFile;
			this._CompressedFileDataSize = source._CompressedFileDataSize;
			this._UncompressedSize = source._UncompressedSize;
			this._BitField = source._BitField;
			this._Source = source._Source;
			this._LastModified = source._LastModified;
			this._Mtime = source._Mtime;
			this._Atime = source._Atime;
			this._Ctime = source._Ctime;
			this._ntfsTimesAreSet = source._ntfsTimesAreSet;
			this._emitUnixTimes = source._emitUnixTimes;
			this._emitNtfsTimes = source._emitNtfsTimes;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000AF9A File Offset: 0x0000919A
		private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00033E0C File Offset: 0x0003200C
		private void _WriteEntryData(Stream s)
		{
			Stream stream = null;
			long _FileDataPosition = -1L;
			try
			{
				_FileDataPosition = s.Position;
			}
			catch (Exception)
			{
			}
			try
			{
				long num = this.SetInputAndFigureFileLength(ref stream);
				CountingStream countingStream = new CountingStream(s);
				Stream stream2;
				Stream stream3;
				if (num != 0L)
				{
					stream2 = this.MaybeApplyEncryption(countingStream);
					stream3 = this.MaybeApplyCompression(stream2, num);
				}
				else
				{
					stream3 = (stream2 = countingStream);
				}
				CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream3, true);
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					this._WriteDelegate(this.FileName, crcCalculatorStream);
				}
				else
				{
					byte[] array = new byte[this.BufferSize];
					int count;
					while ((count = SharedUtilities.ReadWithRetry(stream, array, 0, array.Length, this.FileName)) != 0)
					{
						crcCalculatorStream.Write(array, 0, count);
						this.OnWriteBlock(crcCalculatorStream.TotalBytesSlurped, num);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
				}
				this.FinishOutputStream(s, countingStream, stream2, stream3, crcCalculatorStream);
			}
			finally
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._CloseDelegate != null)
					{
						this._CloseDelegate(this.FileName, stream);
					}
				}
				else if (stream is FileStream)
				{
					stream.Dispose();
				}
			}
			if (this._ioOperationCanceled)
			{
				return;
			}
			this.__FileDataPosition = _FileDataPosition;
			this.PostProcessOutput(s);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00033F54 File Offset: 0x00032154
		private long SetInputAndFigureFileLength(ref Stream input)
		{
			long result = -1L;
			if (this._Source == ZipEntrySource.Stream)
			{
				this.PrepSourceStream();
				input = this._sourceStream;
				try
				{
					return this._sourceStream.Length;
				}
				catch (NotSupportedException)
				{
					return result;
				}
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				string password = (this._Encryption_FromZipFile == EncryptionAlgorithm.None) ? null : (this._Password ?? this._container.Password);
				this._sourceStream = this.InternalOpenReader(password);
				this.PrepSourceStream();
				input = this._sourceStream;
				result = this._sourceStream.Length;
			}
			else
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._sourceStream == null)
					{
						this._sourceStream = this._OpenDelegate(this.FileName);
					}
					this.PrepSourceStream();
					input = this._sourceStream;
					try
					{
						return this._sourceStream.Length;
					}
					catch (NotSupportedException)
					{
						return result;
					}
				}
				if (this._Source == ZipEntrySource.FileSystem)
				{
					input = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
					result = input.Length;
				}
			}
			return result;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00034074 File Offset: 0x00032274
		internal void FinishOutputStream(Stream s, CountingStream entryCounter, Stream encryptor, Stream compressor, CrcCalculatorStream output)
		{
			if (output == null)
			{
				return;
			}
			output.Close();
			if (compressor is DeflateStream)
			{
				compressor.Close();
			}
			else if (compressor is BZip2OutputStream)
			{
				compressor.Close();
			}
			else if (compressor is ParallelBZip2OutputStream)
			{
				compressor.Close();
			}
			else if (compressor is ParallelDeflateOutputStream)
			{
				compressor.Close();
			}
			encryptor.Flush();
			encryptor.Close();
			this._LengthOfTrailer = 0;
			this._UncompressedSize = output.TotalBytesSlurped;
			WinZipAesCipherStream winZipAesCipherStream = encryptor as WinZipAesCipherStream;
			if (winZipAesCipherStream != null && this._UncompressedSize > 0L)
			{
				s.Write(winZipAesCipherStream.FinalAuthentication, 0, 10);
				this._LengthOfTrailer += 10;
			}
			this._CompressedFileDataSize = entryCounter.BytesWritten;
			this._CompressedSize = this._CompressedFileDataSize;
			this._Crc32 = output.Crc;
			this.StoreRelativeOffset();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00034158 File Offset: 0x00032358
		internal void PostProcessOutput(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			if (this._UncompressedSize == 0L && this._CompressedSize == 0L)
			{
				if (this._Source == ZipEntrySource.ZipOutputStream)
				{
					return;
				}
				if (this._Password != null)
				{
					int num = 0;
					if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
					{
						num = 12;
					}
					else if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
					{
						num = this._aesCrypto_forWrite._Salt.Length + this._aesCrypto_forWrite.GeneratedPV.Length;
					}
					if (this._Source == ZipEntrySource.ZipOutputStream && !s.CanSeek)
					{
						throw new ZipException("Zero bytes written, encryption in use, and non-seekable output.");
					}
					if (this.Encryption != EncryptionAlgorithm.None)
					{
						s.Seek((long)(-1 * num), SeekOrigin.Current);
						s.SetLength(s.Position);
						if (countingStream != null)
						{
							countingStream.Adjust((long)num);
						}
						this._LengthOfHeader -= num;
						this.__FileDataPosition -= (long)num;
					}
					this._Password = null;
					this._BitField &= -2;
					this._EntryHeader[6] = (byte)(this._BitField & 255);
					this._EntryHeader[7] = (byte)(((int)this._BitField & 65280) >> 8);
					if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
					{
						short num2 = (short)((int)this._EntryHeader[26] + (int)this._EntryHeader[27] * 256);
						int offx = (int)(30 + num2);
						int num3 = ZipEntry.FindExtraFieldSegment(this._EntryHeader, offx, 39169);
						if (num3 >= 0)
						{
							this._EntryHeader[num3++] = 153;
							this._EntryHeader[num3++] = 153;
						}
					}
				}
				this.CompressionMethod = CompressionMethod.None;
				this.Encryption = EncryptionAlgorithm.None;
			}
			else if (this._zipCrypto_forWrite != null || this._aesCrypto_forWrite != null)
			{
				if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					this._CompressedSize += 12L;
				}
				else if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
				{
					this._CompressedSize += (long)this._aesCrypto_forWrite.SizeOfEncryptionMetadata;
				}
			}
			this._EntryHeader[8] = (byte)(this._CompressionMethod & 255);
			this._EntryHeader[9] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			this._EntryHeader[14] = (byte)(this._Crc32 & 255);
			this._EntryHeader[15] = (byte)((this._Crc32 & 65280) >> 8);
			this._EntryHeader[16] = (byte)((this._Crc32 & 16711680) >> 16);
			byte[] entryHeader = this._EntryHeader;
			int num4 = 17;
			int num5 = 18;
			entryHeader[num4] = (byte)(((long)this._Crc32 & 4278190080L) >> 24);
			this.SetZip64Flags();
			short num6 = (short)((int)this._EntryHeader[26] + (int)this._EntryHeader[27] * 256);
			short num7 = (short)((int)this._EntryHeader[28] + (int)this._EntryHeader[29] * 256);
			if (this._OutputUsesZip64.Value)
			{
				this._EntryHeader[4] = 45;
				this._EntryHeader[5] = 0;
				for (int i = 0; i < 8; i++)
				{
					this._EntryHeader[num5++] = byte.MaxValue;
				}
				num5 = (int)(30 + num6);
				this._EntryHeader[num5++] = 1;
				this._EntryHeader[num5++] = 0;
				num5 += 2;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, this._EntryHeader, num5, 8);
				num5 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, this._EntryHeader, num5, 8);
			}
			else
			{
				this._EntryHeader[4] = 20;
				this._EntryHeader[5] = 0;
				this._EntryHeader[18] = (byte)(this._CompressedSize & 255L);
				this._EntryHeader[19] = (byte)((this._CompressedSize & 65280L) >> 8);
				this._EntryHeader[20] = (byte)((this._CompressedSize & 16711680L) >> 16);
				this._EntryHeader[21] = (byte)((this._CompressedSize & 4278190080L) >> 24);
				this._EntryHeader[22] = (byte)(this._UncompressedSize & 255L);
				this._EntryHeader[23] = (byte)((this._UncompressedSize & 65280L) >> 8);
				this._EntryHeader[24] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				this._EntryHeader[25] = (byte)((this._UncompressedSize & 4278190080L) >> 24);
				if (num7 != 0)
				{
					num5 = (int)(30 + num6);
					short num8 = (short)((int)this._EntryHeader[num5 + 2] + (int)this._EntryHeader[num5 + 3] * 256);
					if (num8 == 16)
					{
						this._EntryHeader[num5++] = 153;
						this._EntryHeader[num5++] = 153;
					}
				}
			}
			if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				this._EntryHeader[8] = 99;
				this._EntryHeader[9] = 0;
				num5 = (int)(30 + num6);
				do
				{
					ushort num9 = (ushort)((int)this._EntryHeader[num5] + (int)this._EntryHeader[num5 + 1] * 256);
					short num10 = (short)((int)this._EntryHeader[num5 + 2] + (int)this._EntryHeader[num5 + 3] * 256);
					if (num9 != 39169)
					{
						num5 += (int)(num10 + 4);
					}
					else
					{
						num5 += 9;
						this._EntryHeader[num5++] = (byte)(this._CompressionMethod & 255);
						this._EntryHeader[num5++] = (byte)((int)this._CompressionMethod & 65280);
					}
				}
				while (num5 < (int)(num7 - 30 - num6));
			}
			if ((this._BitField & 8) != 8 || (this._Source == ZipEntrySource.ZipOutputStream && s.CanSeek))
			{
				ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
				if (zipSegmentedStream != null && this._diskNumber != zipSegmentedStream.CurrentSegment)
				{
					using (Stream stream = ZipSegmentedStream.ForUpdate(this._container.ZipFile.Name, this._diskNumber))
					{
						stream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
						stream.Write(this._EntryHeader, 0, this._EntryHeader.Length);
						goto IL_6C1;
					}
				}
				s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
				s.Write(this._EntryHeader, 0, this._EntryHeader.Length);
				if (countingStream != null)
				{
					countingStream.Adjust((long)this._EntryHeader.Length);
				}
				s.Seek(this._CompressedSize, SeekOrigin.Current);
			}
			IL_6C1:
			if ((this._BitField & 8) == 8 && !this.IsDirectory)
			{
				byte[] array = new byte[16 + (this._OutputUsesZip64.Value ? 8 : 0)];
				Array.Copy(BitConverter.GetBytes(134695760), 0, array, 0, 4);
				Array.Copy(BitConverter.GetBytes(this._Crc32), 0, array, 4, 4);
				num5 = 8;
				if (this._OutputUsesZip64.Value)
				{
					Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num5, 8);
					num5 += 8;
					Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num5, 8);
					num5 += 8;
				}
				else
				{
					array[num5++] = (byte)(this._CompressedSize & 255L);
					array[num5++] = (byte)((this._CompressedSize & 65280L) >> 8);
					array[num5++] = (byte)((this._CompressedSize & 16711680L) >> 16);
					array[num5++] = (byte)((this._CompressedSize & 4278190080L) >> 24);
					array[num5++] = (byte)(this._UncompressedSize & 255L);
					array[num5++] = (byte)((this._UncompressedSize & 65280L) >> 8);
					array[num5++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
					array[num5++] = (byte)((this._UncompressedSize & 4278190080L) >> 24);
				}
				s.Write(array, 0, array.Length);
				this._LengthOfTrailer += array.Length;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000349EC File Offset: 0x00032BEC
		private void SetZip64Flags()
		{
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= 4294967295L || this._UncompressedSize >= 4294967295L || this._RelativeOffsetOfLocalHeader >= 4294967295L);
			if (this._container.Zip64 == Zip64Option.Default && this._entryRequiresZip64.Value)
			{
				throw new ZipException("Compressed or Uncompressed size, or offset exceeds the maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00034A8C File Offset: 0x00032C8C
		internal void PrepOutputStream(Stream s, long streamLength, out CountingStream outputCounter, out Stream encryptor, out Stream compressor, out CrcCalculatorStream output)
		{
			outputCounter = new CountingStream(s);
			if (streamLength != 0L)
			{
				encryptor = this.MaybeApplyEncryption(outputCounter);
				compressor = this.MaybeApplyCompression(encryptor, streamLength);
			}
			else
			{
				Stream stream;
				compressor = (stream = outputCounter);
				encryptor = stream;
			}
			output = new CrcCalculatorStream(compressor, true);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00034AE0 File Offset: 0x00032CE0
		private Stream MaybeApplyCompression(Stream s, long streamLength)
		{
			if (this._CompressionMethod == 8 && this.CompressionLevel != CompressionLevel.None)
			{
				if (this._container.ParallelDeflateThreshold != 0L && (streamLength <= this._container.ParallelDeflateThreshold || this._container.ParallelDeflateThreshold <= 0L))
				{
					DeflateStream deflateStream = new DeflateStream(s, CompressionMode.Compress, this.CompressionLevel, true);
					if (this._container.CodecBufferSize > 0)
					{
						deflateStream.BufferSize = this._container.CodecBufferSize;
					}
					deflateStream.Strategy = this._container.Strategy;
					return deflateStream;
				}
				if (this._container.ParallelDeflater == null)
				{
					this._container.ParallelDeflater = new ParallelDeflateOutputStream(s, this.CompressionLevel, this._container.Strategy, true);
					if (this._container.CodecBufferSize > 0)
					{
						this._container.ParallelDeflater.BufferSize = this._container.CodecBufferSize;
					}
					if (this._container.ParallelDeflateMaxBufferPairs > 0)
					{
						this._container.ParallelDeflater.MaxBufferPairs = this._container.ParallelDeflateMaxBufferPairs;
					}
				}
				ParallelDeflateOutputStream parallelDeflater = this._container.ParallelDeflater;
				parallelDeflater.Reset(s);
				return parallelDeflater;
			}
			else
			{
				if (this._CompressionMethod != 12)
				{
					return s;
				}
				if (this._container.ParallelDeflateThreshold != 0L && (streamLength <= this._container.ParallelDeflateThreshold || this._container.ParallelDeflateThreshold <= 0L))
				{
					return new BZip2OutputStream(s, true);
				}
				return new ParallelBZip2OutputStream(s, true);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000AFC2 File Offset: 0x000091C2
		private Stream MaybeApplyEncryption(Stream s)
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				return new ZipCipherStream(s, this._zipCrypto_forWrite, CryptoMode.Encrypt);
			}
			if (this.Encryption != EncryptionAlgorithm.WinZipAes128)
			{
				if (this.Encryption != EncryptionAlgorithm.WinZipAes256)
				{
					return s;
				}
			}
			return new WinZipAesCipherStream(s, this._aesCrypto_forWrite, CryptoMode.Encrypt);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000AFFE File Offset: 0x000091FE
		private void OnZipErrorWhileSaving(Exception e)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnZipErrorSaving(this, e);
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00034C70 File Offset: 0x00032E70
		internal void Write(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			bool flag = false;
			do
			{
				try
				{
					if (this._Source == ZipEntrySource.ZipFile && !this._restreamRequiredOnSave)
					{
						this.CopyThroughOneEntry(s);
						break;
					}
					if (this.IsDirectory)
					{
						this.WriteHeader(s, 1);
						this.StoreRelativeOffset();
						this._entryRequiresZip64 = new bool?(this._RelativeOffsetOfLocalHeader >= 4294967295L);
						this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
						if (zipSegmentedStream != null)
						{
							this._diskNumber = zipSegmentedStream.CurrentSegment;
						}
						break;
					}
					int num = 0;
					bool flag2;
					do
					{
						num++;
						this.WriteHeader(s, num);
						this.WriteSecurityMetadata(s);
						this._WriteEntryData(s);
						this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
						flag2 = (num <= 1 && s.CanSeek && this.WantReadAgain());
						if (flag2)
						{
							if (zipSegmentedStream != null)
							{
								zipSegmentedStream.TruncateBackward(this._diskNumber, this._RelativeOffsetOfLocalHeader);
							}
							else
							{
								s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
							}
							s.SetLength(s.Position);
							if (countingStream != null)
							{
								countingStream.Adjust(this._TotalEntrySize);
							}
						}
					}
					while (flag2);
					this._skippedDuringSave = false;
					flag = true;
				}
				catch (Exception ex)
				{
					ZipErrorAction zipErrorAction = this.ZipErrorAction;
					int num2 = 0;
					while (this.ZipErrorAction != ZipErrorAction.Throw)
					{
						if (this.ZipErrorAction != ZipErrorAction.Skip)
						{
							if (this.ZipErrorAction != ZipErrorAction.Retry)
							{
								if (num2 <= 0)
								{
									if (this.ZipErrorAction == ZipErrorAction.InvokeErrorEvent)
									{
										this.OnZipErrorWhileSaving(ex);
										if (this._ioOperationCanceled)
										{
											flag = true;
											goto IL_24C;
										}
									}
									num2++;
									continue;
								}
								throw;
							}
						}
						long num3 = (countingStream != null) ? countingStream.ComputedPosition : s.Position;
						long num4 = num3 - this._future_ROLH;
						if (num4 > 0L)
						{
							s.Seek(num4, SeekOrigin.Current);
							long position = s.Position;
							s.SetLength(s.Position);
							if (countingStream != null)
							{
								countingStream.Adjust(num3 - position);
							}
						}
						if (this.ZipErrorAction == ZipErrorAction.Skip)
						{
							this.WriteStatus("Skipping file {0} (exception: {1})", new object[]
							{
								this.LocalFileName,
								ex.ToString()
							});
							this._skippedDuringSave = true;
							flag = true;
						}
						else
						{
							this.ZipErrorAction = zipErrorAction;
						}
						IL_24C:
						goto IL_24E;
					}
					throw;
				}
				IL_24E:;
			}
			while (!flag);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000B025 File Offset: 0x00009225
		internal void StoreRelativeOffset()
		{
			this._RelativeOffsetOfLocalHeader = this._future_ROLH;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000B033 File Offset: 0x00009233
		internal void NotifySaveComplete()
		{
			this._Encryption_FromZipFile = this._Encryption;
			this._CompressionMethod_FromZipFile = this._CompressionMethod;
			this._restreamRequiredOnSave = false;
			this._metadataChanged = false;
			this._Source = ZipEntrySource.ZipFile;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00034EF0 File Offset: 0x000330F0
		internal void WriteSecurityMetadata(Stream outstream)
		{
			if (this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			string password = this._Password;
			if (this._Source == ZipEntrySource.ZipFile && password == null)
			{
				password = this._container.Password;
			}
			if (password == null)
			{
				this._zipCrypto_forWrite = null;
				this._aesCrypto_forWrite = null;
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				this._zipCrypto_forWrite = ZipCrypto.ForWrite(password);
				Random random = new Random();
				byte[] array = new byte[12];
				random.NextBytes(array);
				if ((this._BitField & 8) == 8)
				{
					this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
					array[11] = (byte)(this._TimeBlob >> 8 & 255);
				}
				else
				{
					this.FigureCrc32();
					array[11] = (byte)(this._Crc32 >> 24 & 255);
				}
				byte[] array2 = this._zipCrypto_forWrite.EncryptMessage(array, array.Length);
				outstream.Write(array2, 0, array2.Length);
				this._LengthOfHeader += array2.Length;
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.WinZipAes128 || this.Encryption == EncryptionAlgorithm.WinZipAes256)
			{
				int keyStrengthInBits = ZipEntry.GetKeyStrengthInBits(this.Encryption);
				this._aesCrypto_forWrite = WinZipAesCrypto.Generate(password, keyStrengthInBits);
				outstream.Write(this._aesCrypto_forWrite.Salt, 0, this._aesCrypto_forWrite._Salt.Length);
				outstream.Write(this._aesCrypto_forWrite.GeneratedPV, 0, this._aesCrypto_forWrite.GeneratedPV.Length);
				this._LengthOfHeader += this._aesCrypto_forWrite._Salt.Length + this._aesCrypto_forWrite.GeneratedPV.Length;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00035074 File Offset: 0x00033274
		private void CopyThroughOneEntry(Stream outStream)
		{
			if (this.LengthOfHeader == 0)
			{
				throw new BadStateException("Bad header length.");
			}
			if (this._metadataChanged || this.ArchiveStream is ZipSegmentedStream || outStream is ZipSegmentedStream || (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default) || (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always))
			{
				this.CopyThroughWithRecompute(outStream);
			}
			else
			{
				this.CopyThroughWithNoChange(outStream);
			}
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= 4294967295L || this._UncompressedSize >= 4294967295L || this._RelativeOffsetOfLocalHeader >= 4294967295L);
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00035160 File Offset: 0x00033360
		private void CopyThroughWithRecompute(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			long relativeOffsetOfLocalHeader = this._RelativeOffsetOfLocalHeader;
			int lengthOfHeader = this.LengthOfHeader;
			this.WriteHeader(outstream, 0);
			this.StoreRelativeOffset();
			if (!this.FileName.EndsWith("/"))
			{
				long num = relativeOffsetOfLocalHeader + (long)lengthOfHeader;
				int num2 = ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
				num -= (long)num2;
				this._LengthOfHeader += num2;
				countingStream.Seek(num, SeekOrigin.Begin);
				long num3 = this._CompressedSize;
				while (num3 > 0L)
				{
					num2 = ((num3 > (long)array.Length) ? array.Length : ((int)num3));
					int num4 = countingStream.Read(array, 0, num2);
					outstream.Write(array, 0, num4);
					num3 -= (long)num4;
					this.OnWriteBlock(countingStream.BytesRead, this._CompressedSize);
					if (this._ioOperationCanceled)
					{
						break;
					}
				}
				if ((this._BitField & 8) == 8)
				{
					int num5 = 16;
					if (this._InputUsesZip64)
					{
						num5 += 8;
					}
					byte[] buffer = new byte[num5];
					countingStream.Read(buffer, 0, num5);
					if (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default)
					{
						outstream.Write(buffer, 0, 8);
						if (this._CompressedSize > 4294967295L)
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 8, 4);
						if (this._UncompressedSize > 4294967295L)
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 16, 4);
						this._LengthOfTrailer -= 8;
					}
					else if (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always)
					{
						byte[] buffer2 = new byte[4];
						outstream.Write(buffer, 0, 8);
						outstream.Write(buffer, 8, 4);
						outstream.Write(buffer2, 0, 4);
						outstream.Write(buffer, 12, 4);
						outstream.Write(buffer2, 0, 4);
						this._LengthOfTrailer += 8;
					}
					else
					{
						outstream.Write(buffer, 0, num5);
					}
				}
			}
			this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00035384 File Offset: 0x00033584
		private void CopyThroughWithNoChange(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			countingStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			if (this._TotalEntrySize == 0L)
			{
				this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
			}
			CountingStream countingStream2 = outstream as CountingStream;
			this._RelativeOffsetOfLocalHeader = ((countingStream2 != null) ? countingStream2.ComputedPosition : outstream.Position);
			long num = this._TotalEntrySize;
			while (num > 0L)
			{
				int count = (num > (long)array.Length) ? array.Length : ((int)num);
				int num2 = countingStream.Read(array, 0, count);
				outstream.Write(array, 0, num2);
				num -= (long)num2;
				this.OnWriteBlock(countingStream.BytesRead, this._TotalEntrySize);
				if (this._ioOperationCanceled)
				{
					return;
				}
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00035460 File Offset: 0x00033660
		[Conditional("Trace")]
		private void TraceWriteLine(string format, params object[] varParams)
		{
			lock (this._outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
				Console.Write("{0:000} ZipEntry.Write ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x040002EA RID: 746
		private short _VersionMadeBy;

		// Token: 0x040002EB RID: 747
		private short _InternalFileAttrs;

		// Token: 0x040002EC RID: 748
		private int _ExternalFileAttrs;

		// Token: 0x040002ED RID: 749
		private short _filenameLength;

		// Token: 0x040002EE RID: 750
		private short _extraFieldLength;

		// Token: 0x040002EF RID: 751
		private short _commentLength;

		// Token: 0x040002F0 RID: 752
		private ZipCrypto _zipCrypto_forExtract;

		// Token: 0x040002F1 RID: 753
		private ZipCrypto _zipCrypto_forWrite;

		// Token: 0x040002F2 RID: 754
		private WinZipAesCrypto _aesCrypto_forExtract;

		// Token: 0x040002F3 RID: 755
		private WinZipAesCrypto _aesCrypto_forWrite;

		// Token: 0x040002F4 RID: 756
		private short _WinZipAesMethod;

		// Token: 0x040002F5 RID: 757
		internal DateTime _LastModified;

		// Token: 0x040002F6 RID: 758
		private DateTime _Mtime;

		// Token: 0x040002F7 RID: 759
		private DateTime _Atime;

		// Token: 0x040002F8 RID: 760
		private DateTime _Ctime;

		// Token: 0x040002F9 RID: 761
		private bool _ntfsTimesAreSet;

		// Token: 0x040002FA RID: 762
		private bool _emitNtfsTimes = true;

		// Token: 0x040002FB RID: 763
		private bool _emitUnixTimes;

		// Token: 0x040002FC RID: 764
		private bool _TrimVolumeFromFullyQualifiedPaths = true;

		// Token: 0x040002FD RID: 765
		internal string _LocalFileName;

		// Token: 0x040002FE RID: 766
		private string _FileNameInArchive;

		// Token: 0x040002FF RID: 767
		internal short _VersionNeeded;

		// Token: 0x04000300 RID: 768
		internal short _BitField;

		// Token: 0x04000301 RID: 769
		internal short _CompressionMethod;

		// Token: 0x04000302 RID: 770
		private short _CompressionMethod_FromZipFile;

		// Token: 0x04000303 RID: 771
		private CompressionLevel _CompressionLevel;

		// Token: 0x04000304 RID: 772
		internal string _Comment;

		// Token: 0x04000305 RID: 773
		private bool _IsDirectory;

		// Token: 0x04000306 RID: 774
		private byte[] _CommentBytes;

		// Token: 0x04000307 RID: 775
		internal long _CompressedSize;

		// Token: 0x04000308 RID: 776
		internal long _CompressedFileDataSize;

		// Token: 0x04000309 RID: 777
		internal long _UncompressedSize;

		// Token: 0x0400030A RID: 778
		internal int _TimeBlob;

		// Token: 0x0400030B RID: 779
		private bool _crcCalculated;

		// Token: 0x0400030C RID: 780
		internal int _Crc32;

		// Token: 0x0400030D RID: 781
		internal byte[] _Extra;

		// Token: 0x0400030E RID: 782
		private bool _metadataChanged;

		// Token: 0x0400030F RID: 783
		private bool _restreamRequiredOnSave;

		// Token: 0x04000310 RID: 784
		private bool _sourceIsEncrypted;

		// Token: 0x04000311 RID: 785
		private bool _skippedDuringSave;

		// Token: 0x04000312 RID: 786
		private uint _diskNumber;

		// Token: 0x04000313 RID: 787
		private static Encoding ibm437 = Encoding.GetEncoding("IBM437");

		// Token: 0x04000314 RID: 788
		private Encoding _actualEncoding;

		// Token: 0x04000315 RID: 789
		internal ZipContainer _container;

		// Token: 0x04000316 RID: 790
		private long __FileDataPosition = -1L;

		// Token: 0x04000317 RID: 791
		private byte[] _EntryHeader;

		// Token: 0x04000318 RID: 792
		internal long _RelativeOffsetOfLocalHeader;

		// Token: 0x04000319 RID: 793
		private long _future_ROLH;

		// Token: 0x0400031A RID: 794
		private long _TotalEntrySize;

		// Token: 0x0400031B RID: 795
		private int _LengthOfHeader;

		// Token: 0x0400031C RID: 796
		private int _LengthOfTrailer;

		// Token: 0x0400031D RID: 797
		internal bool _InputUsesZip64;

		// Token: 0x0400031E RID: 798
		private uint _UnsupportedAlgorithmId;

		// Token: 0x0400031F RID: 799
		internal string _Password;

		// Token: 0x04000320 RID: 800
		internal ZipEntrySource _Source;

		// Token: 0x04000321 RID: 801
		internal EncryptionAlgorithm _Encryption;

		// Token: 0x04000322 RID: 802
		internal EncryptionAlgorithm _Encryption_FromZipFile;

		// Token: 0x04000323 RID: 803
		internal byte[] _WeakEncryptionHeader;

		// Token: 0x04000324 RID: 804
		internal Stream _archiveStream;

		// Token: 0x04000325 RID: 805
		private Stream _sourceStream;

		// Token: 0x04000326 RID: 806
		private long? _sourceStreamOriginalPosition;

		// Token: 0x04000327 RID: 807
		private bool _sourceWasJitProvided;

		// Token: 0x04000328 RID: 808
		private bool _ioOperationCanceled;

		// Token: 0x04000329 RID: 809
		private bool _presumeZip64;

		// Token: 0x0400032A RID: 810
		private bool? _entryRequiresZip64;

		// Token: 0x0400032B RID: 811
		private bool? _OutputUsesZip64;

		// Token: 0x0400032C RID: 812
		private bool _IsText;

		// Token: 0x0400032D RID: 813
		private ZipEntryTimestamp _timestamp;

		// Token: 0x0400032E RID: 814
		private static DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400032F RID: 815
		private static DateTime _win32Epoch = DateTime.FromFileTimeUtc(0L);

		// Token: 0x04000330 RID: 816
		private static DateTime _zeroHour = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000331 RID: 817
		private WriteDelegate _WriteDelegate;

		// Token: 0x04000332 RID: 818
		private OpenDelegate _OpenDelegate;

		// Token: 0x04000333 RID: 819
		private CloseDelegate _CloseDelegate;

		// Token: 0x04000334 RID: 820
		private Stream _inputDecryptorStream;

		// Token: 0x04000335 RID: 821
		private int _readExtraDepth;

		// Token: 0x04000336 RID: 822
		private object _outputLock = new object();

		// Token: 0x020000E7 RID: 231
		private class CopyHelper
		{
			// Token: 0x060004FC RID: 1276 RVA: 0x0003551C File Offset: 0x0003371C
			internal static string AppendCopyToFileName(string f)
			{
				ZipEntry.CopyHelper.callCount++;
				if (ZipEntry.CopyHelper.callCount > 25)
				{
					throw new OverflowException("overflow while creating filename");
				}
				int num = 1;
				int num2 = f.LastIndexOf(".");
				if (num2 == -1)
				{
					Match match = ZipEntry.CopyHelper.re.Match(f);
					if (match.Success)
					{
						num = int.Parse(match.Groups[1].Value) + 1;
						string str = string.Format(" (copy {0})", num);
						f = f.Substring(0, match.Index) + str;
					}
					else
					{
						string str2 = string.Format(" (copy {0})", num);
						f += str2;
					}
				}
				else
				{
					Match match2 = ZipEntry.CopyHelper.re.Match(f.Substring(0, num2));
					if (match2.Success)
					{
						num = int.Parse(match2.Groups[1].Value) + 1;
						string str3 = string.Format(" (copy {0})", num);
						f = f.Substring(0, match2.Index) + str3 + f.Substring(num2);
					}
					else
					{
						string str4 = string.Format(" (copy {0})", num);
						f = f.Substring(0, num2) + str4 + f.Substring(num2);
					}
				}
				return f;
			}

			// Token: 0x0400033D RID: 829
			private static Regex re = new Regex(" \\(copy (\\d+)\\)$");

			// Token: 0x0400033E RID: 830
			private static int callCount = 0;
		}

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x06000500 RID: 1280
		private delegate T Func<T>();
	}
}
