using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200012A RID: 298
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0000CBC7 File Offset: 0x0000ADC7
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0000CBCF File Offset: 0x0000ADCF
		public ZlibCodec()
		{
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00046CE0 File Offset: 0x00044EE0
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				int num = this.InitializeDeflate();
				if (num != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				int num2 = this.InitializeInflate();
				if (num2 != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0000CBE6 File Offset: 0x0000ADE6
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0000CC03 File Offset: 0x0000AE03
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0000CC14 File Offset: 0x0000AE14
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0000CC49 File Offset: 0x0000AE49
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00046D40 File Offset: 0x00044F40
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0000CC8A File Offset: 0x0000AE8A
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0000CC93 File Offset: 0x0000AE93
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0000CCA3 File Offset: 0x0000AEA3
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0000CCB3 File Offset: 0x0000AEB3
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0000CCCA File Offset: 0x0000AECA
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00046D74 File Offset: 0x00044F74
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0000CCE1 File Offset: 0x0000AEE1
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0000CD02 File Offset: 0x0000AF02
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0000CD1F File Offset: 0x0000AF1F
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0000CD61 File Offset: 0x0000AF61
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate == null)
			{
				throw new ZlibException("No Inflate or Deflate state!");
			}
			return this.dstate.SetDictionary(dictionary);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00046DCC File Offset: 0x00044FCC
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length > this.dstate.nextPending && this.OutputBuffer.Length > this.NextOut && this.dstate.pending.Length >= this.dstate.nextPending + num && this.OutputBuffer.Length >= this.NextOut + num)
			{
				Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
				this.NextOut += num;
				this.dstate.nextPending += num;
				this.TotalBytesOut += (long)num;
				this.AvailableBytesOut -= num;
				this.dstate.pendingCount -= num;
				if (this.dstate.pendingCount == 0)
				{
					this.dstate.nextPending = 0;
				}
				return;
			}
			throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00046F24 File Offset: 0x00045124
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x04000617 RID: 1559
		public byte[] InputBuffer;

		// Token: 0x04000618 RID: 1560
		public int NextIn;

		// Token: 0x04000619 RID: 1561
		public int AvailableBytesIn;

		// Token: 0x0400061A RID: 1562
		public long TotalBytesIn;

		// Token: 0x0400061B RID: 1563
		public byte[] OutputBuffer;

		// Token: 0x0400061C RID: 1564
		public int NextOut;

		// Token: 0x0400061D RID: 1565
		public int AvailableBytesOut;

		// Token: 0x0400061E RID: 1566
		public long TotalBytesOut;

		// Token: 0x0400061F RID: 1567
		public string Message;

		// Token: 0x04000620 RID: 1568
		internal DeflateManager dstate;

		// Token: 0x04000621 RID: 1569
		internal InflateManager istate;

		// Token: 0x04000622 RID: 1570
		internal uint _Adler32;

		// Token: 0x04000623 RID: 1571
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x04000624 RID: 1572
		public int WindowBits = 15;

		// Token: 0x04000625 RID: 1573
		public CompressionStrategy Strategy;
	}
}
