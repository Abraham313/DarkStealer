using System;
using System.IO;
using System.Threading;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000169 RID: 361
	public class CancellableStream : Stream
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
		public CancellableStream(Stream stream, CancellationToken cancellationToken)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0004CE78 File Offset: 0x0004B078
		public override bool CanRead
		{
			get
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return this.stream.CanRead;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0004CEA0 File Offset: 0x0004B0A0
		public override bool CanSeek
		{
			get
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return this.stream.CanSeek;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0004CEC8 File Offset: 0x0004B0C8
		public override bool CanWrite
		{
			get
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return this.stream.CanWrite;
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0004CEF0 File Offset: 0x0004B0F0
		public override void Flush()
		{
			this.cancellationToken.ThrowIfCancellationRequested();
			this.stream.Flush();
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0004CF18 File Offset: 0x0004B118
		public override long Length
		{
			get
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return this.stream.Length;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0004CF40 File Offset: 0x0004B140
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0004CF68 File Offset: 0x0004B168
		public override long Position
		{
			get
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return this.stream.Position;
			}
			set
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.stream.Position = value;
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0004CF90 File Offset: 0x0004B190
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.cancellationToken.ThrowIfCancellationRequested();
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0004CFBC File Offset: 0x0004B1BC
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.cancellationToken.ThrowIfCancellationRequested();
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0004CFE4 File Offset: 0x0004B1E4
		public override void SetLength(long value)
		{
			this.cancellationToken.ThrowIfCancellationRequested();
			this.stream.SetLength(value);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0004D00C File Offset: 0x0004B20C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.cancellationToken.ThrowIfCancellationRequested();
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0000DC14 File Offset: 0x0000BE14
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream stream = this.stream;
				if (stream != null)
				{
					stream.Dispose();
				}
				this.stream = null;
			}
		}

		// Token: 0x040006EA RID: 1770
		private Stream stream;

		// Token: 0x040006EB RID: 1771
		private readonly CancellationToken cancellationToken;
	}
}
