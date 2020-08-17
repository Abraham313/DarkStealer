using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x020000FD RID: 253
	internal class ZipSegmentedStream : Stream
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x0000C004 File Offset: 0x0000A204
		private ZipSegmentedStream()
		{
			this._exceptionPending = false;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0003A584 File Offset: 0x00038784
		public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.ReadOnly,
				CurrentSegment = initialDiskNumber,
				_maxDiskNumber = maxDiskNumber,
				_baseName = name
			};
			zipSegmentedStream._SetReadStream();
			return zipSegmentedStream;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0003A5BC File Offset: 0x000387BC
		public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.Write,
				CurrentSegment = 0U,
				_baseName = name,
				_maxSegmentSize = maxSegmentSize,
				_baseDir = Path.GetDirectoryName(name)
			};
			if (zipSegmentedStream._baseDir == "")
			{
				zipSegmentedStream._baseDir = ".";
			}
			zipSegmentedStream._SetWriteStream(0U);
			return zipSegmentedStream;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0003A620 File Offset: 0x00038820
		public static Stream ForUpdate(string name, uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			string path = string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name)), diskNumber + 1U);
			return File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0000C013 File Offset: 0x0000A213
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0000C01B File Offset: 0x0000A21B
		public bool ContiguousWrite { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0000C024 File Offset: 0x0000A224
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0000C02C File Offset: 0x0000A22C
		public uint CurrentSegment
		{
			get
			{
				return this._currentDiskNumber;
			}
			private set
			{
				this._currentDiskNumber = value;
				this._currentName = null;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0000C03C File Offset: 0x0000A23C
		public string CurrentName
		{
			get
			{
				if (this._currentName == null)
				{
					this._currentName = this._NameForSegment(this.CurrentSegment);
				}
				return this._currentName;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0000C05E File Offset: 0x0000A25E
		public string CurrentTempName
		{
			get
			{
				return this._currentTempName;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0003A66C File Offset: 0x0003886C
		private string _NameForSegment(uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				this._exceptionPending = true;
				throw new OverflowException("The number of zip segments would exceed 99.");
			}
			return string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName)), diskNumber + 1U);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0000C066 File Offset: 0x0000A266
		public uint ComputeSegment(int length)
		{
			if (this._innerStream.Position + (long)length > (long)this._maxSegmentSize)
			{
				return this.CurrentSegment + 1U;
			}
			return this.CurrentSegment;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0003A6C0 File Offset: 0x000388C0
		public override string ToString()
		{
			return string.Format("{0}[{1}][{2}], pos=0x{3:X})", new object[]
			{
				"ZipSegmentedStream",
				this.CurrentName,
				this.rwMode.ToString(),
				this.Position
			});
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0003A714 File Offset: 0x00038914
		private void _SetReadStream()
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
			}
			if (this.CurrentSegment + 1U == this._maxDiskNumber)
			{
				this._currentName = this._baseName;
			}
			this._innerStream = File.OpenRead(this.CurrentName);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0003A764 File Offset: 0x00038964
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Read.");
			}
			int num = this._innerStream.Read(buffer, offset, count);
			int num2 = num;
			while (num2 != count)
			{
				if (this._innerStream.Position != this._innerStream.Length)
				{
					this._exceptionPending = true;
					throw new ZipException(string.Format("Read error in file {0}", this.CurrentName));
				}
				if (this.CurrentSegment + 1U == this._maxDiskNumber)
				{
					return num;
				}
				this.CurrentSegment += 1U;
				this._SetReadStream();
				offset += num2;
				count -= num2;
				num2 = this._innerStream.Read(buffer, offset, count);
				num += num2;
			}
			return num;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0003A820 File Offset: 0x00038A20
		private void _SetWriteStream(uint increment)
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this.CurrentName))
				{
					File.Delete(this.CurrentName);
				}
				File.Move(this._currentTempName, this.CurrentName);
			}
			if (increment > 0U)
			{
				this.CurrentSegment += increment;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
			if (this.CurrentSegment == 0U)
			{
				this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0003A8B4 File Offset: 0x00038AB4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Write.");
			}
			if (this.ContiguousWrite)
			{
				if (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					this._SetWriteStream(1U);
				}
			}
			else
			{
				while (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					int num = this._maxSegmentSize - (int)this._innerStream.Position;
					this._innerStream.Write(buffer, offset, num);
					this._SetWriteStream(1U);
					count -= num;
					offset += num;
				}
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0003A95C File Offset: 0x00038B5C
		public long TruncateBackward(uint diskNumber, long offset)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new ZipException("bad state.");
			}
			if (diskNumber == this.CurrentSegment)
			{
				return this._innerStream.Seek(offset, SeekOrigin.Begin);
			}
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this._currentTempName))
				{
					File.Delete(this._currentTempName);
				}
			}
			for (uint num = this.CurrentSegment - 1U; num > diskNumber; num -= 1U)
			{
				string path = this._NameForSegment(num);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			this.CurrentSegment = diskNumber;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					this._currentTempName = SharedUtilities.InternalGetTempFileName();
					File.Move(this.CurrentName, this._currentTempName);
					break;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			this._innerStream = new FileStream(this._currentTempName, FileMode.Open);
			return this._innerStream.Seek(offset, SeekOrigin.Begin);
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0000C08E File Offset: 0x0000A28E
		public override bool CanRead
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0000C0AE File Offset: 0x0000A2AE
		public override bool CanSeek
		{
			get
			{
				return this._innerStream != null && this._innerStream.CanSeek;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0000C0C5 File Offset: 0x0000A2C5
		public override bool CanWrite
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0000C0E5 File Offset: 0x0000A2E5
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0000C0F2 File Offset: 0x0000A2F2
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0000C0FF File Offset: 0x0000A2FF
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0000C10C File Offset: 0x0000A30C
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0003AA70 File Offset: 0x00038C70
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0000C11A File Offset: 0x0000A31A
		public override void SetLength(long value)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException();
			}
			this._innerStream.SetLength(value);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0003AA8C File Offset: 0x00038C8C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._innerStream != null)
				{
					this._innerStream.Dispose();
					if (this.rwMode == ZipSegmentedStream.RwMode.Write)
					{
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x040003F9 RID: 1017
		private ZipSegmentedStream.RwMode rwMode;

		// Token: 0x040003FA RID: 1018
		private bool _exceptionPending;

		// Token: 0x040003FB RID: 1019
		private string _baseName;

		// Token: 0x040003FC RID: 1020
		private string _baseDir;

		// Token: 0x040003FD RID: 1021
		private string _currentName;

		// Token: 0x040003FE RID: 1022
		private string _currentTempName;

		// Token: 0x040003FF RID: 1023
		private uint _currentDiskNumber;

		// Token: 0x04000400 RID: 1024
		private uint _maxDiskNumber;

		// Token: 0x04000401 RID: 1025
		private int _maxSegmentSize;

		// Token: 0x04000402 RID: 1026
		private Stream _innerStream;

		// Token: 0x020000FE RID: 254
		private enum RwMode
		{
			// Token: 0x04000405 RID: 1029
			None,
			// Token: 0x04000406 RID: 1030
			ReadOnly,
			// Token: 0x04000407 RID: 1031
			Write
		}
	}
}
