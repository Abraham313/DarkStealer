using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ionic.BZip2
{
	// Token: 0x02000109 RID: 265
	[ComVisible(true)]
	public class ParallelBZip2OutputStream : Stream
	{
		// Token: 0x06000705 RID: 1797 RVA: 0x0000C3B2 File Offset: 0x0000A5B2
		public ParallelBZip2OutputStream(Stream output) : this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0000C3C1 File Offset: 0x0000A5C1
		public ParallelBZip2OutputStream(Stream output, int blockSize) : this(output, blockSize, false)
		{
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		public ParallelBZip2OutputStream(Stream output, bool leaveOpen) : this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0003E5C8 File Offset: 0x0003C7C8
		public ParallelBZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
		{
			if (blockSize < BZip2.MinBlockSize || blockSize > BZip2.MaxBlockSize)
			{
				string message = string.Format("blockSize={0} is out of range; must be between {1} and {2}", blockSize, BZip2.MinBlockSize, BZip2.MaxBlockSize);
				throw new ArgumentException(message, "blockSize");
			}
			this.output = output;
			if (!this.output.CanWrite)
			{
				throw new ArgumentException("The stream is not writable.", "output");
			}
			this.bw = new BitWriter(this.output);
			this.blockSize100k = blockSize;
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.MaxWorkers = 16;
			this.EmitHeader();
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0003E69C File Offset: 0x0003C89C
		private void InitializePoolOfWorkItems()
		{
			this.toWrite = new Queue<int>();
			this.toFill = new Queue<int>();
			this.pool = new List<WorkItem>();
			int num = ParallelBZip2OutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this.MaxWorkers);
			for (int i = 0; i < num; i++)
			{
				this.pool.Add(new WorkItem(i, this.blockSize100k));
				this.toFill.Enqueue(i);
			}
			this.newlyCompressedBlob = new AutoResetEvent(false);
			this.currentlyFilling = -1;
			this.lastFilled = -1;
			this.lastWritten = -1;
			this.latestCompressed = -1;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0000C3DB File Offset: 0x0000A5DB
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x0000C3E3 File Offset: 0x0000A5E3
		public int MaxWorkers
		{
			get
			{
				return this._maxWorkers;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxWorkers", "Value must be 4 or greater.");
				}
				this._maxWorkers = value;
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0003E73C File Offset: 0x0003C93C
		public override void Close()
		{
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
			if (this.handlingException)
			{
				return;
			}
			if (this.output == null)
			{
				return;
			}
			Stream stream = this.output;
			try
			{
				this.FlushOutput(true);
			}
			finally
			{
				this.output = null;
				this.bw = null;
			}
			if (!this.leaveOpen)
			{
				stream.Close();
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0003E7C0 File Offset: 0x0003C9C0
		private void FlushOutput(bool lastInput)
		{
			if (this.emitting)
			{
				return;
			}
			if (this.currentlyFilling >= 0)
			{
				WorkItem wi = this.pool[this.currentlyFilling];
				this.CompressOne(wi);
				this.currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this.EmitTrailer();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0000C400 File Offset: 0x0000A600
		public override void Flush()
		{
			if (this.output != null)
			{
				this.FlushOutput(false);
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0003E81C File Offset: 0x0003CA1C
		private void EmitHeader()
		{
			byte[] array = new byte[]
			{
				66,
				90,
				104,
				0
			};
			array[3] = (byte)(48 + this.blockSize100k);
			byte[] array2 = array;
			this.output.Write(array2, 0, array2.Length);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0003E85C File Offset: 0x0003CA5C
		private void EmitTrailer()
		{
			this.bw.WriteByte(23);
			this.bw.WriteByte(114);
			this.bw.WriteByte(69);
			this.bw.WriteByte(56);
			this.bw.WriteByte(80);
			this.bw.WriteByte(144);
			this.bw.WriteInt(this.combinedCRC);
			this.bw.FinishAndPad();
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0000C427 File Offset: 0x0000A627
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0003E8D8 File Offset: 0x0003CAD8
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (count == 0)
			{
				return;
			}
			if (!this.firstWriteDone)
			{
				this.InitializePoolOfWorkItems();
				this.firstWriteDone = true;
			}
			int num = 0;
			int num2 = count;
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int index;
				if (this.currentlyFilling >= 0)
				{
					index = this.currentlyFilling;
					goto IL_EB;
				}
				if (this.toFill.Count != 0)
				{
					index = this.toFill.Dequeue();
					this.lastFilled++;
					goto IL_EB;
				}
				mustWait = true;
				IL_153:
				if (num2 <= 0)
				{
					break;
				}
				continue;
				IL_EB:
				WorkItem workItem = this.pool[index];
				workItem.ordinal = this.lastFilled;
				int num3 = workItem.Compressor.Fill(buffer, offset, num2);
				if (num3 != num2)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.CompressOne), workItem))
					{
						goto IL_17E;
					}
					this.currentlyFilling = -1;
					offset += num3;
				}
				else
				{
					this.currentlyFilling = index;
				}
				num2 -= num3;
				num += num3;
				goto IL_153;
			}
			this.totalBytesWrittenIn += (long)num;
			return;
			IL_17E:
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0003EA80 File Offset: 0x0003CC80
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this.newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this.toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this.toWrite.Count > 0)
							{
								num3 = this.toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this.toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this.pool[num3];
							if (workItem.ordinal != this.lastWritten + 1)
							{
								lock (this.toWrite)
								{
									this.toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this.newlyCompressedBlob.WaitOne();
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
								BitWriter bitWriter = workItem.bw;
								bitWriter.Flush();
								MemoryStream ms = workItem.ms;
								ms.Seek(0L, SeekOrigin.Begin);
								long num4 = 0L;
								byte[] array = new byte[1024];
								int num5;
								while ((num5 = ms.Read(array, 0, array.Length)) > 0)
								{
									for (int i = 0; i < num5; i++)
									{
										this.bw.WriteByte(array[i]);
									}
									num4 += (long)num5;
								}
								if (bitWriter.NumRemainingBits > 0)
								{
									this.bw.WriteBits(bitWriter.NumRemainingBits, (uint)bitWriter.RemainingBits);
								}
								this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
								this.combinedCRC ^= workItem.Compressor.Crc32;
								this.totalBytesWrittenOut += num4;
								bitWriter.Reset();
								this.lastWritten = workItem.ordinal;
								workItem.ordinal = -1;
								this.toFill.Enqueue(workItem.index);
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
			while (this.lastWritten != this.latestCompressed);
			this.emitting = false;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0003ECE8 File Offset: 0x0003CEE8
		private void CompressOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				workItem.Compressor.CompressAndWrite();
				lock (this.latestLock)
				{
					if (workItem.ordinal > this.latestCompressed)
					{
						this.latestCompressed = workItem.ordinal;
					}
				}
				lock (this.toWrite)
				{
					this.toWrite.Enqueue(workItem.index);
				}
				this.newlyCompressedBlob.Set();
			}
			catch (Exception ex)
			{
				lock (this.eLock)
				{
					if (this.pendingException != null)
					{
						this.pendingException = ex;
					}
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0000C42F File Offset: 0x0000A62F
		public override bool CanWrite
		{
			get
			{
				if (this.output == null)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.output.CanWrite;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0000C44F File Offset: 0x0000A64F
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				return this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0000C457 File Offset: 0x0000A657
		public long BytesWrittenOut
		{
			get
			{
				return this.totalBytesWrittenOut;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000A639 File Offset: 0x00008839
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0003EDD0 File Offset: 0x0003CFD0
		[Conditional("Trace")]
		private void TraceOutput(ParallelBZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != ParallelBZip2OutputStream.TraceBits.None)
			{
				lock (this.outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.Green;
					Console.Write("{0:000} PBOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x04000494 RID: 1172
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04000495 RID: 1173
		private int _maxWorkers;

		// Token: 0x04000496 RID: 1174
		private bool firstWriteDone;

		// Token: 0x04000497 RID: 1175
		private int lastFilled;

		// Token: 0x04000498 RID: 1176
		private int lastWritten;

		// Token: 0x04000499 RID: 1177
		private int latestCompressed;

		// Token: 0x0400049A RID: 1178
		private int currentlyFilling;

		// Token: 0x0400049B RID: 1179
		private volatile Exception pendingException;

		// Token: 0x0400049C RID: 1180
		private bool handlingException;

		// Token: 0x0400049D RID: 1181
		private bool emitting;

		// Token: 0x0400049E RID: 1182
		private Queue<int> toWrite;

		// Token: 0x0400049F RID: 1183
		private Queue<int> toFill;

		// Token: 0x040004A0 RID: 1184
		private List<WorkItem> pool;

		// Token: 0x040004A1 RID: 1185
		private object latestLock = new object();

		// Token: 0x040004A2 RID: 1186
		private object eLock = new object();

		// Token: 0x040004A3 RID: 1187
		private object outputLock = new object();

		// Token: 0x040004A4 RID: 1188
		private AutoResetEvent newlyCompressedBlob;

		// Token: 0x040004A5 RID: 1189
		private long totalBytesWrittenIn;

		// Token: 0x040004A6 RID: 1190
		private long totalBytesWrittenOut;

		// Token: 0x040004A7 RID: 1191
		private bool leaveOpen;

		// Token: 0x040004A8 RID: 1192
		private uint combinedCRC;

		// Token: 0x040004A9 RID: 1193
		private Stream output;

		// Token: 0x040004AA RID: 1194
		private BitWriter bw;

		// Token: 0x040004AB RID: 1195
		private int blockSize100k;

		// Token: 0x040004AC RID: 1196
		private ParallelBZip2OutputStream.TraceBits desiredTrace = ParallelBZip2OutputStream.TraceBits.Crc | ParallelBZip2OutputStream.TraceBits.Write;

		// Token: 0x0200010A RID: 266
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x040004AE RID: 1198
			None = 0U,
			// Token: 0x040004AF RID: 1199
			Crc = 1U,
			// Token: 0x040004B0 RID: 1200
			Write = 2U,
			// Token: 0x040004B1 RID: 1201
			All = 4294967295U
		}
	}
}
