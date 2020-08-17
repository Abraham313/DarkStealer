using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x020000FC RID: 252
	internal class ZipContainer
	{
		// Token: 0x06000668 RID: 1640 RVA: 0x0000BD6B File Offset: 0x00009F6B
		public ZipContainer(object o)
		{
			this._zf = (o as ZipFile);
			this._zos = (o as ZipOutputStream);
			this._zis = (o as ZipInputStream);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0000BD97 File Offset: 0x00009F97
		public ZipFile ZipFile
		{
			get
			{
				return this._zf;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0000BD9F File Offset: 0x00009F9F
		public ZipOutputStream ZipOutputStream
		{
			get
			{
				return this._zos;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0000BDA7 File Offset: 0x00009FA7
		public string Name
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Name;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos.Name;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0000BDD6 File Offset: 0x00009FD6
		public string Password
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._Password;
				}
				if (this._zis != null)
				{
					return this._zis._Password;
				}
				return this._zos._password;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0000BE0B File Offset: 0x0000A00B
		public Zip64Option Zip64
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._zip64;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos._zip64;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0000BE3A File Offset: 0x0000A03A
		public int BufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.BufferSize;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return 0;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0000BE5F File Offset: 0x0000A05F
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x0000BE8A File Offset: 0x0000A08A
		public ParallelDeflateOutputStream ParallelDeflater
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflater;
				}
				if (this._zis != null)
				{
					return null;
				}
				return this._zos.ParallelDeflater;
			}
			set
			{
				if (this._zf != null)
				{
					this._zf.ParallelDeflater = value;
					return;
				}
				if (this._zos != null)
				{
					this._zos.ParallelDeflater = value;
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x0000BEB5 File Offset: 0x0000A0B5
		public long ParallelDeflateThreshold
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateThreshold;
				}
				return this._zos.ParallelDeflateThreshold;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0000BED6 File Offset: 0x0000A0D6
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateMaxBufferPairs;
				}
				return this._zos.ParallelDeflateMaxBufferPairs;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0000BEF7 File Offset: 0x0000A0F7
		public int CodecBufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.CodecBufferSize;
				}
				if (this._zis != null)
				{
					return this._zis.CodecBufferSize;
				}
				return this._zos.CodecBufferSize;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0000BF2C File Offset: 0x0000A12C
		public CompressionStrategy Strategy
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Strategy;
				}
				return this._zos.Strategy;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0000BF4D File Offset: 0x0000A14D
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.UseZip64WhenSaving;
				}
				return this._zos.EnableZip64;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0000BF6E File Offset: 0x0000A16E
		public Encoding AlternateEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncoding;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0000BF99 File Offset: 0x0000A199
		public Encoding DefaultEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return ZipFile.DefaultEncoding;
				}
				if (this._zos != null)
				{
					return ZipOutputStream.DefaultEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncodingUsage;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncodingUsage;
				}
				return ZipOption.Default;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000BFE3 File Offset: 0x0000A1E3
		public Stream ReadStream
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ReadStream;
				}
				return this._zis.ReadStream;
			}
		}

		// Token: 0x040003F6 RID: 1014
		private ZipFile _zf;

		// Token: 0x040003F7 RID: 1015
		private ZipOutputStream _zos;

		// Token: 0x040003F8 RID: 1016
		private ZipInputStream _zis;
	}
}
