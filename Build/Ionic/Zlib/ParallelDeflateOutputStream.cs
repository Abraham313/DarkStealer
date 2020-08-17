using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x0200011B RID: 283
	[ComVisible(true)]
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x060007B1 RID: 1969 RVA: 0x0000C98D File Offset: 0x0000AB8D
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0000C999 File Offset: 0x0000AB99
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0000C9B1 File Offset: 0x0000ABB1
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00044F18 File Offset: 0x00043118
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0000C9BD File Offset: 0x0000ABBD
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0000C9CE File Offset: 0x0000ABCE
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0000C9D6 File Offset: 0x0000ABD6
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0000C9FB File Offset: 0x0000ABFB
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000CA24 File Offset: 0x0000AC24
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00044F88 File Offset: 0x00043188
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00045040 File Offset: 0x00043240
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_84;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_84;
				}
				mustWait = true;
				IL_127:
				if (count <= 0)
				{
					break;
				}
				continue;
				IL_84:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable != workItem.buffer.Length)
				{
					this._currentlyFilling = num;
					goto IL_127;
				}
				if (ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
				{
					this._currentlyFilling = -1;
					goto IL_127;
				}
				goto IL_153;
			}
			return;
			IL_153:
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000451AC File Offset: 0x000433AC
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00045268 File Offset: 0x00043468
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000452D0 File Offset: 0x000434D0
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00045314 File Offset: 0x00043514
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000CA2C File Offset: 0x0000AC2C
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000CA42 File Offset: 0x0000AC42
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0004537C File Offset: 0x0004357C
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0004544C File Offset: 0x0004364C
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								lock (this._toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
				if (!doAll)
				{
					break;
				}
			}
			while (this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00045610 File Offset: 0x00043810
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				lock (this._latestLock)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				lock (this._toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				lock (this._eLock)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0004571C File Offset: 0x0004391C
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00045794 File Offset: 0x00043994
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				lock (this._outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0000CA4B File Offset: 0x0000AC4B
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0000CA58 File Offset: 0x0000AC58
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000983A File Offset: 0x00007A3A
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400058F RID: 1423
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x04000590 RID: 1424
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04000591 RID: 1425
		private List<WorkItem> _pool;

		// Token: 0x04000592 RID: 1426
		private bool _leaveOpen;

		// Token: 0x04000593 RID: 1427
		private bool emitting;

		// Token: 0x04000594 RID: 1428
		private Stream _outStream;

		// Token: 0x04000595 RID: 1429
		private int _maxBufferPairs;

		// Token: 0x04000596 RID: 1430
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x04000597 RID: 1431
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04000598 RID: 1432
		private object _outputLock = new object();

		// Token: 0x04000599 RID: 1433
		private bool _isClosed;

		// Token: 0x0400059A RID: 1434
		private bool _firstWriteDone;

		// Token: 0x0400059B RID: 1435
		private int _currentlyFilling;

		// Token: 0x0400059C RID: 1436
		private int _lastFilled;

		// Token: 0x0400059D RID: 1437
		private int _lastWritten;

		// Token: 0x0400059E RID: 1438
		private int _latestCompressed;

		// Token: 0x0400059F RID: 1439
		private int _Crc32;

		// Token: 0x040005A0 RID: 1440
		private CRC32 _runningCrc;

		// Token: 0x040005A1 RID: 1441
		private object _latestLock = new object();

		// Token: 0x040005A2 RID: 1442
		private Queue<int> _toWrite;

		// Token: 0x040005A3 RID: 1443
		private Queue<int> _toFill;

		// Token: 0x040005A4 RID: 1444
		private long _totalBytesProcessed;

		// Token: 0x040005A5 RID: 1445
		private CompressionLevel _compressLevel;

		// Token: 0x040005A6 RID: 1446
		private volatile Exception _pendingException;

		// Token: 0x040005A7 RID: 1447
		private bool _handlingException;

		// Token: 0x040005A8 RID: 1448
		private object _eLock = new object();

		// Token: 0x040005A9 RID: 1449
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x0200011C RID: 284
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x040005AC RID: 1452
			None = 0U,
			// Token: 0x040005AD RID: 1453
			NotUsed1 = 1U,
			// Token: 0x040005AE RID: 1454
			EmitLock = 2U,
			// Token: 0x040005AF RID: 1455
			EmitEnter = 4U,
			// Token: 0x040005B0 RID: 1456
			EmitBegin = 8U,
			// Token: 0x040005B1 RID: 1457
			EmitDone = 16U,
			// Token: 0x040005B2 RID: 1458
			EmitSkip = 32U,
			// Token: 0x040005B3 RID: 1459
			EmitAll = 58U,
			// Token: 0x040005B4 RID: 1460
			Flush = 64U,
			// Token: 0x040005B5 RID: 1461
			Lifecycle = 128U,
			// Token: 0x040005B6 RID: 1462
			Session = 256U,
			// Token: 0x040005B7 RID: 1463
			Synch = 512U,
			// Token: 0x040005B8 RID: 1464
			Instance = 1024U,
			// Token: 0x040005B9 RID: 1465
			Compress = 2048U,
			// Token: 0x040005BA RID: 1466
			Write = 4096U,
			// Token: 0x040005BB RID: 1467
			WriteEnter = 8192U,
			// Token: 0x040005BC RID: 1468
			WriteTake = 16384U,
			// Token: 0x040005BD RID: 1469
			All = 4294967295U
		}
	}
}
