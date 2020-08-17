using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x020000FB RID: 251
	[ComVisible(true)]
	public class ZipOutputStream : Stream
	{
		// Token: 0x0600062F RID: 1583 RVA: 0x0000BA78 File Offset: 0x00009C78
		public ZipOutputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0003A060 File Offset: 0x00038260
		public ZipOutputStream(string fileName)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000BA82 File Offset: 0x00009C82
		public ZipOutputStream(Stream stream, bool leaveOpen)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0003A0A0 File Offset: 0x000382A0
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._outputStream = (stream.CanRead ? stream : new CountingStream(stream));
			this.CompressionLevel = CompressionLevel.Default;
			this.CompressionMethod = CompressionMethod.Deflate;
			this._encryption = EncryptionAlgorithm.None;
			this._entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			this._zip64 = Zip64Option.Default;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this.Strategy = CompressionStrategy.Default;
			this._name = (name ?? "(stream)");
			this.ParallelDeflateThreshold = -1L;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000BAAB File Offset: 0x00009CAB
		public override string ToString()
		{
			return string.Format("ZipOutputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170000FC RID: 252
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x0003A120 File Offset: 0x00038320
		public string Password
		{
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._password = value;
				if (this._password == null)
				{
					this._encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._encryption == EncryptionAlgorithm.None)
				{
					this._encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0000BAD0 File Offset: 0x00009CD0
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._encryption;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._encryption = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0000BB09 File Offset: 0x00009D09
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x0000BB11 File Offset: 0x00009D11
		public int CodecBufferSize { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0000BB1A File Offset: 0x00009D1A
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x0000BB22 File Offset: 0x00009D22
		public CompressionStrategy Strategy { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0000BB2B File Offset: 0x00009D2B
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0000BB33 File Offset: 0x00009D33
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._timestamp = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0000BB56 File Offset: 0x00009D56
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x0000BB5E File Offset: 0x00009D5E
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0000BB67 File Offset: 0x00009D67
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0000BB6F File Offset: 0x00009D6F
		public CompressionMethod CompressionMethod { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0000BB78 File Offset: 0x00009D78
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x0000BB80 File Offset: 0x00009D80
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._comment = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0000BBA3 File Offset: 0x00009DA3
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x0000BBAB File Offset: 0x00009DAB
		public Zip64Option EnableZip64
		{
			get
			{
				return this._zip64;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._zip64 = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0000BBCE File Offset: 0x00009DCE
		public bool OutputUsedZip64
		{
			get
			{
				return this._anyEntriesUsedZip64 || this._directoryNeededZip64;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0000BBEB File Offset: 0x00009DEB
		public bool IgnoreCase
		{
			get
			{
				return !this._DontIgnoreCase;
			}
			set
			{
				this._DontIgnoreCase = !value;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0000BBF7 File Offset: 0x00009DF7
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0000BC11 File Offset: 0x00009E11
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.UTF8;
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipOutputStream.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0000BC3B File Offset: 0x00009E3B
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0000BC4E File Offset: 0x00009E4E
		[Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0000BC5E File Offset: 0x00009E5E
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000BC66 File Offset: 0x00009E66
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0000BC6F File Offset: 0x00009E6F
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0000BC77 File Offset: 0x00009E77
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0000BC80 File Offset: 0x00009E80
		public static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.GetEncoding("IBM437");
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0000BCC4 File Offset: 0x00009EC4
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0000BC8C File Offset: 0x00009E8C
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("value must be greater than 64k, or 0, or -1");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0000BCCC File Offset: 0x00009ECC
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0000BCD4 File Offset: 0x00009ED4
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000BCF1 File Offset: 0x00009EF1
		private void InsureUniqueEntry(ZipEntry ze1)
		{
			if (this._entriesWritten.ContainsKey(ze1.FileName))
			{
				this._exceptionPending = true;
				throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0000BD23 File Offset: 0x00009F23
		internal Stream OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0000BD2B File Offset: 0x00009F2B
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0000BD33 File Offset: 0x00009F33
		public bool ContainsEntry(string name)
		{
			return this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0003A170 File Offset: 0x00038370
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (buffer == null)
			{
				this._exceptionPending = true;
				throw new ArgumentNullException("buffer");
			}
			if (this._currentEntry == null)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
			}
			if (this._currentEntry.IsDirectory)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
			}
			if (this._needToWriteEntryHeader)
			{
				this._InitiateCurrentEntry(false);
			}
			if (count != 0)
			{
				this._entryOutputStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0003A208 File Offset: 0x00038408
		public ZipEntry PutNextEntry(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			this._FinishCurrentEntry();
			this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
			this._currentEntry._container = new ZipContainer(this);
			ZipEntry currentEntry = this._currentEntry;
			currentEntry._BitField |= 8;
			this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			this._currentEntry.CompressionLevel = this.CompressionLevel;
			this._currentEntry.CompressionMethod = this.CompressionMethod;
			this._currentEntry.Password = this._password;
			this._currentEntry.Encryption = this.Encryption;
			this._currentEntry.AlternateEncoding = this.AlternateEncoding;
			this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			if (entryName.EndsWith("/"))
			{
				this._currentEntry.MarkAsDirectory();
			}
			this._currentEntry.EmitTimesInWindowsFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Windows) != ZipEntryTimestamp.None);
			this._currentEntry.EmitTimesInUnixFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Unix) != ZipEntryTimestamp.None);
			this.InsureUniqueEntry(this._currentEntry);
			this._needToWriteEntryHeader = true;
			return this._currentEntry;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0003A35C File Offset: 0x0003855C
		private void _InitiateCurrentEntry(bool finishing)
		{
			this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
			this._entryCount++;
			if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
			}
			this._currentEntry.WriteHeader(this._outputStream, finishing ? 99 : 0);
			this._currentEntry.StoreRelativeOffset();
			if (!this._currentEntry.IsDirectory)
			{
				this._currentEntry.WriteSecurityMetadata(this._outputStream);
				this._currentEntry.PrepOutputStream(this._outputStream, finishing ? 0L : -1L, out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
			}
			this._needToWriteEntryHeader = false;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0003A434 File Offset: 0x00038634
		private void _FinishCurrentEntry()
		{
			if (this._currentEntry != null)
			{
				if (this._needToWriteEntryHeader)
				{
					this._InitiateCurrentEntry(true);
				}
				this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
				this._currentEntry.PostProcessOutput(this._outputStream);
				if (this._currentEntry.OutputUsedZip64 != null)
				{
					this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
				}
				this._outputCounter = null;
				this._encryptor = (this._deflater = null);
				this._entryOutputStream = null;
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0003A4E8 File Offset: 0x000386E8
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && !this._exceptionPending)
			{
				this._FinishCurrentEntry();
				this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, this._entriesWritten.Values, 1U, this._zip64, this.Comment, new ZipContainer(this));
				CountingStream countingStream = this._outputStream as CountingStream;
				Stream stream;
				if (countingStream != null)
				{
					stream = countingStream.WrappedStream;
					countingStream.Dispose();
				}
				else
				{
					stream = this._outputStream;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					stream.Dispose();
				}
				this._outputStream = null;
			}
			this._disposed = true;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00009F16 File Offset: 0x00008116
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0000BD46 File Offset: 0x00009F46
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Position
		{
			get
			{
				return this._outputStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00009B58 File Offset: 0x00007D58
		public override void Flush()
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0000BD53 File Offset: 0x00009F53
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Read");
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0000BD5F File Offset: 0x00009F5F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040003D8 RID: 984
		private EncryptionAlgorithm _encryption;

		// Token: 0x040003D9 RID: 985
		private ZipEntryTimestamp _timestamp;

		// Token: 0x040003DA RID: 986
		internal string _password;

		// Token: 0x040003DB RID: 987
		private string _comment;

		// Token: 0x040003DC RID: 988
		private Stream _outputStream;

		// Token: 0x040003DD RID: 989
		private ZipEntry _currentEntry;

		// Token: 0x040003DE RID: 990
		internal Zip64Option _zip64;

		// Token: 0x040003DF RID: 991
		private Dictionary<string, ZipEntry> _entriesWritten;

		// Token: 0x040003E0 RID: 992
		private int _entryCount;

		// Token: 0x040003E1 RID: 993
		private ZipOption _alternateEncodingUsage;

		// Token: 0x040003E2 RID: 994
		private Encoding _alternateEncoding;

		// Token: 0x040003E3 RID: 995
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x040003E4 RID: 996
		private bool _disposed;

		// Token: 0x040003E5 RID: 997
		private bool _exceptionPending;

		// Token: 0x040003E6 RID: 998
		private bool _anyEntriesUsedZip64;

		// Token: 0x040003E7 RID: 999
		private bool _directoryNeededZip64;

		// Token: 0x040003E8 RID: 1000
		private CountingStream _outputCounter;

		// Token: 0x040003E9 RID: 1001
		private Stream _encryptor;

		// Token: 0x040003EA RID: 1002
		private Stream _deflater;

		// Token: 0x040003EB RID: 1003
		private CrcCalculatorStream _entryOutputStream;

		// Token: 0x040003EC RID: 1004
		private bool _needToWriteEntryHeader;

		// Token: 0x040003ED RID: 1005
		private string _name;

		// Token: 0x040003EE RID: 1006
		private bool _DontIgnoreCase;

		// Token: 0x040003EF RID: 1007
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x040003F0 RID: 1008
		private long _ParallelDeflateThreshold;

		// Token: 0x040003F1 RID: 1009
		private int _maxBufferPairs;
	}
}
