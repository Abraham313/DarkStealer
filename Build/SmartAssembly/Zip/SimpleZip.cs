using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace SmartAssembly.Zip
{
	// Token: 0x0200008C RID: 140
	public static class SimpleZip
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x00009F16 File Offset: 0x00008116
		private static bool PublicKeysMatch(Assembly assembly_0, Assembly assembly_1)
		{
			return true;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00027FEC File Offset: 0x000261EC
		private static ICryptoTransform GetAesTransform(byte[] byte_0, byte[] byte_1, bool bool_0)
		{
			ICryptoTransform result;
			using (SymmetricAlgorithm symmetricAlgorithm = new RijndaelManaged())
			{
				result = (bool_0 ? symmetricAlgorithm.CreateDecryptor(byte_0, byte_1) : symmetricAlgorithm.CreateEncryptor(byte_0, byte_1));
			}
			return result;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00028034 File Offset: 0x00026234
		private static ICryptoTransform GetDesTransform(byte[] byte_0, byte[] byte_1, bool bool_0)
		{
			ICryptoTransform result;
			using (DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider())
			{
				result = (bool_0 ? descryptoServiceProvider.CreateDecryptor(byte_0, byte_1) : descryptoServiceProvider.CreateEncryptor(byte_0, byte_1));
			}
			return result;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0002807C File Offset: 0x0002627C
		public static byte[] Unzip(byte[] byte_0)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (callingAssembly != executingAssembly && !SimpleZip.PublicKeysMatch(executingAssembly, callingAssembly))
			{
				return null;
			}
			SimpleZip.ZipStream zipStream = new SimpleZip.ZipStream(byte_0);
			byte[] array = new byte[0];
			int num = zipStream.ReadInt();
			if (num == 67324752)
			{
				short num2 = (short)zipStream.ReadShort();
				int num3 = zipStream.ReadShort();
				int num4 = zipStream.ReadShort();
				if (num == 67324752 && num2 == 20 && num3 == 0)
				{
					if (num4 == 8)
					{
						zipStream.ReadInt();
						zipStream.ReadInt();
						zipStream.ReadInt();
						int num5 = zipStream.ReadInt();
						int num6 = zipStream.ReadShort();
						int num7 = zipStream.ReadShort();
						if (num6 > 0)
						{
							byte[] buffer = new byte[num6];
							zipStream.Read(buffer, 0, num6);
						}
						if (num7 > 0)
						{
							byte[] buffer2 = new byte[num7];
							zipStream.Read(buffer2, 0, num7);
						}
						byte[] array2 = new byte[zipStream.Length - zipStream.Position];
						zipStream.Read(array2, 0, array2.Length);
						SimpleZip.Inflater inflater = new SimpleZip.Inflater(array2);
						array = new byte[num5];
						inflater.Inflate(array, 0, array.Length);
						goto IL_265;
					}
				}
				throw new FormatException("Wrong Header Signature");
			}
			int num8 = num >> 24;
			num -= num8 << 24;
			if (num == 8223355)
			{
				if (num8 == 1)
				{
					int num9 = zipStream.ReadInt();
					array = new byte[num9];
					int num11;
					for (int i = 0; i < num9; i += num11)
					{
						int num10 = zipStream.ReadInt();
						num11 = zipStream.ReadInt();
						byte[] array3 = new byte[num10];
						zipStream.Read(array3, 0, array3.Length);
						new SimpleZip.Inflater(array3).Inflate(array, i, num11);
					}
				}
				if (num8 == 2)
				{
					byte[] byte_ = new byte[]
					{
						72,
						196,
						226,
						220,
						77,
						90,
						77,
						41
					};
					byte[] byte_2 = new byte[]
					{
						17,
						242,
						176,
						1,
						248,
						117,
						118,
						164
					};
					using (ICryptoTransform desTransform = SimpleZip.GetDesTransform(byte_, byte_2, true))
					{
						array = SimpleZip.Unzip(desTransform.TransformFinalBlock(byte_0, 4, byte_0.Length - 4));
					}
				}
				if (num8 != 3)
				{
					goto IL_265;
				}
				byte[] byte_3 = new byte[]
				{
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1
				};
				byte[] byte_4 = new byte[]
				{
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2
				};
				using (ICryptoTransform aesTransform = SimpleZip.GetAesTransform(byte_3, byte_4, true))
				{
					array = SimpleZip.Unzip(aesTransform.TransformFinalBlock(byte_0, 4, byte_0.Length - 4));
					goto IL_265;
				}
			}
			throw new FormatException("Unknown Header");
			IL_265:
			zipStream.Close();
			zipStream = null;
			return array;
		}

		// Token: 0x0200008D RID: 141
		internal sealed class Inflater
		{
			// Token: 0x060002CB RID: 715 RVA: 0x00009F19 File Offset: 0x00008119
			public Inflater(byte[] byte_0)
			{
				this.input = new SimpleZip.StreamManipulator();
				this.outputWindow = new SimpleZip.OutputWindow();
				this.mode = 2;
				this.input.SetInput(byte_0, 0, byte_0.Length);
			}

			// Token: 0x060002CC RID: 716 RVA: 0x00028314 File Offset: 0x00026514
			private bool DecodeHuffman()
			{
				int i = this.outputWindow.GetFreeSpace();
				while (i >= 258)
				{
					int symbol;
					switch (this.mode)
					{
					case 7:
						while (((symbol = this.litlenTree.GetSymbol(this.input)) & -256) == 0)
						{
							this.outputWindow.Write(symbol);
							if (--i < 258)
							{
								return true;
							}
						}
						if (symbol >= 257)
						{
							this.repLength = SimpleZip.Inflater.CPLENS[symbol - 257];
							this.neededBits = SimpleZip.Inflater.CPLEXT[symbol - 257];
							goto IL_9C;
						}
						if (symbol < 0)
						{
							return false;
						}
						this.distTree = null;
						this.litlenTree = null;
						this.mode = 2;
						return true;
					case 8:
						goto IL_9C;
					case 9:
						goto IL_EC;
					case 10:
						break;
					default:
						continue;
					}
					IL_11F:
					if (this.neededBits > 0)
					{
						this.mode = 10;
						int num = this.input.PeekBits(this.neededBits);
						if (num < 0)
						{
							return false;
						}
						this.input.DropBits(this.neededBits);
						this.repDist += num;
					}
					this.outputWindow.Repeat(this.repLength, this.repDist);
					i -= this.repLength;
					this.mode = 7;
					continue;
					IL_EC:
					symbol = this.distTree.GetSymbol(this.input);
					if (symbol >= 0)
					{
						this.repDist = SimpleZip.Inflater.CPDIST[symbol];
						this.neededBits = SimpleZip.Inflater.CPDEXT[symbol];
						goto IL_11F;
					}
					return false;
					IL_9C:
					if (this.neededBits > 0)
					{
						this.mode = 8;
						int num2 = this.input.PeekBits(this.neededBits);
						if (num2 < 0)
						{
							return false;
						}
						this.input.DropBits(this.neededBits);
						this.repLength += num2;
					}
					this.mode = 9;
					goto IL_EC;
				}
				return true;
			}

			// Token: 0x060002CD RID: 717 RVA: 0x000284E4 File Offset: 0x000266E4
			private bool Decode()
			{
				switch (this.mode)
				{
				case 2:
				{
					if (this.isLastBlock)
					{
						this.mode = 12;
						return false;
					}
					int num = this.input.PeekBits(3);
					if (num < 0)
					{
						return false;
					}
					this.input.DropBits(3);
					if ((num & 1) != 0)
					{
						this.isLastBlock = true;
					}
					switch (num >> 1)
					{
					case 0:
						this.input.SkipToByteBoundary();
						this.mode = 3;
						break;
					case 1:
						this.litlenTree = SimpleZip.InflaterHuffmanTree.defLitLenTree;
						this.distTree = SimpleZip.InflaterHuffmanTree.defDistTree;
						this.mode = 7;
						break;
					case 2:
						this.dynHeader = new SimpleZip.InflaterDynHeader();
						this.mode = 6;
						break;
					}
					return true;
				}
				case 3:
					if ((this.uncomprLen = this.input.PeekBits(16)) < 0)
					{
						return false;
					}
					this.input.DropBits(16);
					this.mode = 4;
					break;
				case 4:
					break;
				case 5:
					goto IL_131;
				case 6:
					if (!this.dynHeader.Decode(this.input))
					{
						return false;
					}
					this.litlenTree = this.dynHeader.BuildLitLenTree();
					this.distTree = this.dynHeader.BuildDistTree();
					this.mode = 7;
					goto IL_1B5;
				case 7:
				case 8:
				case 9:
				case 10:
					goto IL_1B5;
				case 11:
					return false;
				case 12:
					return false;
				default:
					return false;
				}
				if (this.input.PeekBits(16) < 0)
				{
					return false;
				}
				this.input.DropBits(16);
				this.mode = 5;
				IL_131:
				int num2 = this.outputWindow.CopyStored(this.input, this.uncomprLen);
				this.uncomprLen -= num2;
				if (this.uncomprLen == 0)
				{
					this.mode = 2;
					return true;
				}
				return !this.input.IsNeedingInput;
				IL_1B5:
				return this.DecodeHuffman();
			}

			// Token: 0x060002CE RID: 718 RVA: 0x000286B0 File Offset: 0x000268B0
			public int Inflate(byte[] byte_0, int int_0, int int_1)
			{
				int num = 0;
				for (;;)
				{
					if (this.mode != 11)
					{
						int num2 = this.outputWindow.CopyOutput(byte_0, int_0, int_1);
						int_0 += num2;
						num += num2;
						int_1 -= num2;
						if (int_1 == 0)
						{
							return num;
						}
					}
					if (!this.Decode())
					{
						if (this.outputWindow.GetAvailable() <= 0)
						{
							break;
						}
						if (this.mode == 11)
						{
							break;
						}
					}
				}
				return num;
			}

			// Token: 0x0400019B RID: 411
			private static readonly int[] CPLENS = new int[]
			{
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				13,
				15,
				17,
				19,
				23,
				27,
				31,
				35,
				43,
				51,
				59,
				67,
				83,
				99,
				115,
				131,
				163,
				195,
				227,
				258
			};

			// Token: 0x0400019C RID: 412
			private static readonly int[] CPLEXT = new int[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				5,
				5,
				5,
				5,
				0
			};

			// Token: 0x0400019D RID: 413
			private static readonly int[] CPDIST = new int[]
			{
				1,
				2,
				3,
				4,
				5,
				7,
				9,
				13,
				17,
				25,
				33,
				49,
				65,
				97,
				129,
				193,
				257,
				385,
				513,
				769,
				1025,
				1537,
				2049,
				3073,
				4097,
				6145,
				8193,
				12289,
				16385,
				24577
			};

			// Token: 0x0400019E RID: 414
			private static readonly int[] CPDEXT = new int[]
			{
				0,
				0,
				0,
				0,
				1,
				1,
				2,
				2,
				3,
				3,
				4,
				4,
				5,
				5,
				6,
				6,
				7,
				7,
				8,
				8,
				9,
				9,
				10,
				10,
				11,
				11,
				12,
				12,
				13,
				13
			};

			// Token: 0x0400019F RID: 415
			private int mode;

			// Token: 0x040001A0 RID: 416
			private int neededBits;

			// Token: 0x040001A1 RID: 417
			private int repLength;

			// Token: 0x040001A2 RID: 418
			private int repDist;

			// Token: 0x040001A3 RID: 419
			private int uncomprLen;

			// Token: 0x040001A4 RID: 420
			private bool isLastBlock;

			// Token: 0x040001A5 RID: 421
			private SimpleZip.StreamManipulator input;

			// Token: 0x040001A6 RID: 422
			private SimpleZip.OutputWindow outputWindow;

			// Token: 0x040001A7 RID: 423
			private SimpleZip.InflaterDynHeader dynHeader;

			// Token: 0x040001A8 RID: 424
			private SimpleZip.InflaterHuffmanTree litlenTree;

			// Token: 0x040001A9 RID: 425
			private SimpleZip.InflaterHuffmanTree distTree;
		}

		// Token: 0x0200008E RID: 142
		internal sealed class StreamManipulator
		{
			// Token: 0x060002D0 RID: 720 RVA: 0x00028780 File Offset: 0x00026980
			public int PeekBits(int int_0)
			{
				if (this.bits_in_buffer < int_0)
				{
					if (this.window_start == this.window_end)
					{
						return -1;
					}
					uint num = this.buffer;
					byte[] array = this.window;
					int num2 = this.window_start;
					this.window_start = num2 + 1;
					uint num3 = array[num2] & 255U;
					byte[] array2 = this.window;
					num2 = this.window_start;
					this.window_start = num2 + 1;
					this.buffer = (num | (num3 | (array2[num2] & 255U) << 8) << this.bits_in_buffer);
					this.bits_in_buffer += 16;
				}
				return (int)((ulong)this.buffer & (ulong)((long)((1 << int_0) - 1)));
			}

			// Token: 0x060002D1 RID: 721 RVA: 0x00009F4E File Offset: 0x0000814E
			public void DropBits(int int_0)
			{
				this.buffer >>= int_0;
				this.bits_in_buffer -= int_0;
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060002D2 RID: 722 RVA: 0x00009F6F File Offset: 0x0000816F
			public int AvailableBits
			{
				get
				{
					return this.bits_in_buffer;
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060002D3 RID: 723 RVA: 0x00009F77 File Offset: 0x00008177
			public int AvailableBytes
			{
				get
				{
					return this.window_end - this.window_start + (this.bits_in_buffer >> 3);
				}
			}

			// Token: 0x060002D4 RID: 724 RVA: 0x00009F8F File Offset: 0x0000818F
			public void SkipToByteBoundary()
			{
				this.buffer >>= (this.bits_in_buffer & 7);
				this.bits_in_buffer &= -8;
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x060002D5 RID: 725 RVA: 0x00009FB8 File Offset: 0x000081B8
			public bool IsNeedingInput
			{
				get
				{
					return this.window_start == this.window_end;
				}
			}

			// Token: 0x060002D6 RID: 726 RVA: 0x00028820 File Offset: 0x00026A20
			public int CopyBytes(byte[] byte_0, int int_0, int int_1)
			{
				int num = 0;
				while (this.bits_in_buffer > 0 && int_1 > 0)
				{
					byte_0[int_0++] = (byte)this.buffer;
					this.buffer >>= 8;
					this.bits_in_buffer -= 8;
					int_1--;
					num++;
				}
				if (int_1 == 0)
				{
					return num;
				}
				int num2 = this.window_end - this.window_start;
				if (int_1 > num2)
				{
					int_1 = num2;
				}
				Array.Copy(this.window, this.window_start, byte_0, int_0, int_1);
				this.window_start += int_1;
				if ((this.window_start - this.window_end & 1) != 0)
				{
					byte[] array = this.window;
					int num3 = this.window_start;
					this.window_start = num3 + 1;
					this.buffer = (array[num3] & 255U);
					this.bits_in_buffer = 8;
				}
				return num + int_1;
			}

			// Token: 0x060002D8 RID: 728 RVA: 0x000288F0 File Offset: 0x00026AF0
			public void SetInput(byte[] byte_0, int int_0, int int_1)
			{
				if (this.window_start < this.window_end)
				{
					throw new InvalidOperationException();
				}
				int num = int_0 + int_1;
				if (0 <= int_0 && int_0 <= num && num <= byte_0.Length)
				{
					if ((int_1 & 1) != 0)
					{
						this.buffer |= (uint)((uint)(byte_0[int_0++] & byte.MaxValue) << this.bits_in_buffer);
						this.bits_in_buffer += 8;
					}
					this.window = byte_0;
					this.window_start = int_0;
					this.window_end = num;
					return;
				}
				throw new ArgumentOutOfRangeException();
			}

			// Token: 0x040001AA RID: 426
			private byte[] window;

			// Token: 0x040001AB RID: 427
			private int window_start;

			// Token: 0x040001AC RID: 428
			private int window_end;

			// Token: 0x040001AD RID: 429
			private uint buffer;

			// Token: 0x040001AE RID: 430
			private int bits_in_buffer;
		}

		// Token: 0x0200008F RID: 143
		internal sealed class OutputWindow
		{
			// Token: 0x060002D9 RID: 729 RVA: 0x00028978 File Offset: 0x00026B78
			public void Write(int int_0)
			{
				int num = this.windowFilled;
				this.windowFilled = num + 1;
				if (num == 32768)
				{
					throw new InvalidOperationException();
				}
				byte[] array = this.window;
				num = this.windowEnd;
				this.windowEnd = num + 1;
				array[num] = (byte)int_0;
				this.windowEnd &= 32767;
			}

			// Token: 0x060002DA RID: 730 RVA: 0x000289D0 File Offset: 0x00026BD0
			private void SlowRepeat(int int_0, int int_1, int int_2)
			{
				while (int_1-- > 0)
				{
					byte[] array = this.window;
					int num = this.windowEnd;
					this.windowEnd = num + 1;
					array[num] = this.window[int_0++];
					this.windowEnd &= 32767;
					int_0 &= 32767;
				}
			}

			// Token: 0x060002DB RID: 731 RVA: 0x00028A2C File Offset: 0x00026C2C
			public void Repeat(int int_0, int int_1)
			{
				if ((this.windowFilled += int_0) > 32768)
				{
					throw new InvalidOperationException();
				}
				int num = this.windowEnd - int_1 & 32767;
				int num2 = 32768 - int_0;
				if (num > num2 || this.windowEnd >= num2)
				{
					this.SlowRepeat(num, int_0, int_1);
					return;
				}
				if (int_0 <= int_1)
				{
					Array.Copy(this.window, num, this.window, this.windowEnd, int_0);
					this.windowEnd += int_0;
					return;
				}
				while (int_0-- > 0)
				{
					byte[] array = this.window;
					int num3 = this.windowEnd;
					this.windowEnd = num3 + 1;
					array[num3] = this.window[num++];
				}
			}

			// Token: 0x060002DC RID: 732 RVA: 0x00028AE0 File Offset: 0x00026CE0
			public int CopyStored(SimpleZip.StreamManipulator streamManipulator_0, int int_0)
			{
				int_0 = Math.Min(Math.Min(int_0, 32768 - this.windowFilled), streamManipulator_0.AvailableBytes);
				int num = 32768 - this.windowEnd;
				int num2;
				if (int_0 > num)
				{
					num2 = streamManipulator_0.CopyBytes(this.window, this.windowEnd, num);
					if (num2 == num)
					{
						num2 += streamManipulator_0.CopyBytes(this.window, 0, int_0 - num);
					}
				}
				else
				{
					num2 = streamManipulator_0.CopyBytes(this.window, this.windowEnd, int_0);
				}
				this.windowEnd = (this.windowEnd + num2 & 32767);
				this.windowFilled += num2;
				return num2;
			}

			// Token: 0x060002DD RID: 733 RVA: 0x00009FC8 File Offset: 0x000081C8
			public int GetFreeSpace()
			{
				return 32768 - this.windowFilled;
			}

			// Token: 0x060002DE RID: 734 RVA: 0x00009FD6 File Offset: 0x000081D6
			public int GetAvailable()
			{
				return this.windowFilled;
			}

			// Token: 0x060002DF RID: 735 RVA: 0x00028B84 File Offset: 0x00026D84
			public int CopyOutput(byte[] byte_0, int int_0, int int_1)
			{
				int num = this.windowEnd;
				if (int_1 > this.windowFilled)
				{
					int_1 = this.windowFilled;
				}
				else
				{
					num = (this.windowEnd - this.windowFilled + int_1 & 32767);
				}
				int num2 = int_1;
				int num3 = int_1 - num;
				if (num3 > 0)
				{
					Array.Copy(this.window, 32768 - num3, byte_0, int_0, num3);
					int_0 += num3;
					int_1 = num;
				}
				Array.Copy(this.window, num - int_1, byte_0, int_0, int_1);
				this.windowFilled -= num2;
				if (this.windowFilled < 0)
				{
					throw new InvalidOperationException();
				}
				return num2;
			}

			// Token: 0x040001AF RID: 431
			private byte[] window = new byte[32768];

			// Token: 0x040001B0 RID: 432
			private int windowEnd;

			// Token: 0x040001B1 RID: 433
			private int windowFilled;
		}

		// Token: 0x02000090 RID: 144
		internal sealed class InflaterHuffmanTree
		{
			// Token: 0x060002E1 RID: 737 RVA: 0x00028C18 File Offset: 0x00026E18
			static InflaterHuffmanTree()
			{
				byte[] array = new byte[288];
				int i = 0;
				while (i < 144)
				{
					array[i++] = 8;
				}
				while (i < 256)
				{
					array[i++] = 9;
				}
				while (i < 280)
				{
					array[i++] = 7;
				}
				while (i < 288)
				{
					array[i++] = 8;
				}
				SimpleZip.InflaterHuffmanTree.defLitLenTree = new SimpleZip.InflaterHuffmanTree(array);
				array = new byte[32];
				i = 0;
				while (i < 32)
				{
					array[i++] = 5;
				}
				SimpleZip.InflaterHuffmanTree.defDistTree = new SimpleZip.InflaterHuffmanTree(array);
			}

			// Token: 0x060002E2 RID: 738 RVA: 0x00009FF6 File Offset: 0x000081F6
			public InflaterHuffmanTree(byte[] byte_0)
			{
				this.BuildTree(byte_0);
			}

			// Token: 0x060002E3 RID: 739 RVA: 0x00028CAC File Offset: 0x00026EAC
			private void BuildTree(byte[] byte_0)
			{
				int[] array = new int[16];
				int[] array2 = new int[16];
				foreach (int num in byte_0)
				{
					if (num > 0)
					{
						array[num]++;
					}
				}
				int num2 = 0;
				int num3 = 512;
				for (int j = 1; j <= 15; j++)
				{
					array2[j] = num2;
					num2 += array[j] << 16 - j;
					if (j >= 10)
					{
						int num4 = array2[j] & 130944;
						int num5 = num2 & 130944;
						num3 += num5 - num4 >> 16 - j;
					}
				}
				this.tree = new short[num3];
				int num6 = 512;
				for (int k = 15; k >= 10; k--)
				{
					int num7 = num2 & 130944;
					num2 -= array[k] << 16 - k;
					for (int l = num2 & 130944; l < num7; l += 128)
					{
						this.tree[(int)SimpleZip.DeflaterHuffman.BitReverse(l)] = (short)(-num6 << 4 | k);
						num6 += 1 << k - 9;
					}
				}
				for (int m = 0; m < byte_0.Length; m++)
				{
					int num8 = (int)byte_0[m];
					if (num8 != 0)
					{
						num2 = array2[num8];
						int num9 = (int)SimpleZip.DeflaterHuffman.BitReverse(num2);
						if (num8 <= 9)
						{
							do
							{
								this.tree[num9] = (short)(m << 4 | num8);
								num9 += 1 << num8;
							}
							while (num9 < 512);
						}
						else
						{
							int num10 = (int)this.tree[num9 & 511];
							int num11 = 1 << (num10 & 15);
							num10 = -(num10 >> 4);
							do
							{
								this.tree[num10 | num9 >> 9] = (short)(m << 4 | num8);
								num9 += 1 << num8;
							}
							while (num9 < num11);
						}
						array2[num8] = num2 + (1 << 16 - num8);
					}
				}
			}

			// Token: 0x060002E4 RID: 740 RVA: 0x00028E94 File Offset: 0x00027094
			public int GetSymbol(SimpleZip.StreamManipulator streamManipulator_0)
			{
				int num;
				if ((num = streamManipulator_0.PeekBits(9)) >= 0)
				{
					int num2;
					if ((num2 = (int)this.tree[num]) >= 0)
					{
						streamManipulator_0.DropBits(num2 & 15);
						return num2 >> 4;
					}
					int num3 = -(num2 >> 4);
					int int_ = num2 & 15;
					if ((num = streamManipulator_0.PeekBits(int_)) >= 0)
					{
						num2 = (int)this.tree[num3 | num >> 9];
						streamManipulator_0.DropBits(num2 & 15);
						return num2 >> 4;
					}
					int availableBits = streamManipulator_0.AvailableBits;
					num = streamManipulator_0.PeekBits(availableBits);
					num2 = (int)this.tree[num3 | num >> 9];
					if ((num2 & 15) <= availableBits)
					{
						streamManipulator_0.DropBits(num2 & 15);
						return num2 >> 4;
					}
					return -1;
				}
				else
				{
					int availableBits2 = streamManipulator_0.AvailableBits;
					num = streamManipulator_0.PeekBits(availableBits2);
					int num2 = (int)this.tree[num];
					if (num2 >= 0 && (num2 & 15) <= availableBits2)
					{
						streamManipulator_0.DropBits(num2 & 15);
						return num2 >> 4;
					}
					return -1;
				}
			}

			// Token: 0x040001B2 RID: 434
			private short[] tree;

			// Token: 0x040001B3 RID: 435
			public static readonly SimpleZip.InflaterHuffmanTree defLitLenTree;

			// Token: 0x040001B4 RID: 436
			public static readonly SimpleZip.InflaterHuffmanTree defDistTree;
		}

		// Token: 0x02000091 RID: 145
		internal sealed class InflaterDynHeader
		{
			// Token: 0x060002E6 RID: 742 RVA: 0x00028F6C File Offset: 0x0002716C
			public bool Decode(SimpleZip.StreamManipulator streamManipulator_0)
			{
				for (;;)
				{
					switch (this.mode)
					{
					case 0:
						this.lnum = streamManipulator_0.PeekBits(5);
						if (this.lnum >= 0)
						{
							this.lnum += 257;
							streamManipulator_0.DropBits(5);
							this.mode = 1;
							goto IL_1E0;
						}
						return false;
					case 1:
						goto IL_1E0;
					case 2:
						goto IL_192;
					case 3:
						goto IL_159;
					case 4:
						break;
					case 5:
						goto IL_2A;
					default:
						continue;
					}
					IL_E4:
					int symbol;
					while (((symbol = this.blTree.GetSymbol(streamManipulator_0)) & -16) == 0)
					{
						byte[] array = this.litdistLens;
						int num = this.ptr;
						this.ptr = num + 1;
						array[num] = (this.lastLen = (byte)symbol);
						if (this.ptr == this.num)
						{
							return true;
						}
					}
					if (symbol >= 0)
					{
						if (symbol >= 17)
						{
							this.lastLen = 0;
						}
						this.repSymbol = symbol - 16;
						this.mode = 5;
						goto IL_2A;
					}
					return false;
					IL_159:
					while (this.ptr < this.blnum)
					{
						int num2 = streamManipulator_0.PeekBits(3);
						if (num2 < 0)
						{
							return false;
						}
						streamManipulator_0.DropBits(3);
						this.blLens[SimpleZip.InflaterDynHeader.BL_ORDER[this.ptr]] = (byte)num2;
						this.ptr++;
					}
					this.blTree = new SimpleZip.InflaterHuffmanTree(this.blLens);
					this.blLens = null;
					this.ptr = 0;
					this.mode = 4;
					goto IL_E4;
					IL_2A:
					int int_ = SimpleZip.InflaterDynHeader.repBits[this.repSymbol];
					int num3 = streamManipulator_0.PeekBits(int_);
					if (num3 < 0)
					{
						return false;
					}
					streamManipulator_0.DropBits(int_);
					num3 += SimpleZip.InflaterDynHeader.repMin[this.repSymbol];
					while (num3-- > 0)
					{
						byte[] array2 = this.litdistLens;
						int num = this.ptr;
						this.ptr = num + 1;
						array2[num] = this.lastLen;
					}
					if (this.ptr == this.num)
					{
						break;
					}
					this.mode = 4;
					continue;
					IL_192:
					this.blnum = streamManipulator_0.PeekBits(4);
					if (this.blnum >= 0)
					{
						this.blnum += 4;
						streamManipulator_0.DropBits(4);
						this.blLens = new byte[19];
						this.ptr = 0;
						this.mode = 3;
						goto IL_159;
					}
					return false;
					IL_1E0:
					this.dnum = streamManipulator_0.PeekBits(5);
					if (this.dnum >= 0)
					{
						this.dnum++;
						streamManipulator_0.DropBits(5);
						this.num = this.lnum + this.dnum;
						this.litdistLens = new byte[this.num];
						this.mode = 2;
						goto IL_192;
					}
					return false;
				}
				return true;
			}

			// Token: 0x060002E7 RID: 743 RVA: 0x00029208 File Offset: 0x00027408
			public SimpleZip.InflaterHuffmanTree BuildLitLenTree()
			{
				byte[] array = new byte[this.lnum];
				Array.Copy(this.litdistLens, 0, array, 0, this.lnum);
				return new SimpleZip.InflaterHuffmanTree(array);
			}

			// Token: 0x060002E8 RID: 744 RVA: 0x0002923C File Offset: 0x0002743C
			public SimpleZip.InflaterHuffmanTree BuildDistTree()
			{
				byte[] array = new byte[this.dnum];
				Array.Copy(this.litdistLens, this.lnum, array, 0, this.dnum);
				return new SimpleZip.InflaterHuffmanTree(array);
			}

			// Token: 0x040001B5 RID: 437
			private static readonly int[] repMin = new int[]
			{
				3,
				3,
				11
			};

			// Token: 0x040001B6 RID: 438
			private static readonly int[] repBits = new int[]
			{
				2,
				3,
				7
			};

			// Token: 0x040001B7 RID: 439
			private byte[] blLens;

			// Token: 0x040001B8 RID: 440
			private byte[] litdistLens;

			// Token: 0x040001B9 RID: 441
			private SimpleZip.InflaterHuffmanTree blTree;

			// Token: 0x040001BA RID: 442
			private int mode;

			// Token: 0x040001BB RID: 443
			private int lnum;

			// Token: 0x040001BC RID: 444
			private int dnum;

			// Token: 0x040001BD RID: 445
			private int blnum;

			// Token: 0x040001BE RID: 446
			private int num;

			// Token: 0x040001BF RID: 447
			private int repSymbol;

			// Token: 0x040001C0 RID: 448
			private byte lastLen;

			// Token: 0x040001C1 RID: 449
			private int ptr;

			// Token: 0x040001C2 RID: 450
			private static readonly int[] BL_ORDER = new int[]
			{
				16,
				17,
				18,
				0,
				8,
				7,
				9,
				6,
				10,
				5,
				11,
				4,
				12,
				3,
				13,
				2,
				14,
				1,
				15
			};
		}

		// Token: 0x02000092 RID: 146
		internal sealed class DeflaterHuffman
		{
			// Token: 0x060002EA RID: 746 RVA: 0x0000A005 File Offset: 0x00008205
			public static short BitReverse(int int_0)
			{
				return (short)((int)SimpleZip.DeflaterHuffman.bit4Reverse[int_0 & 15] << 12 | (int)SimpleZip.DeflaterHuffman.bit4Reverse[int_0 >> 4 & 15] << 8 | (int)SimpleZip.DeflaterHuffman.bit4Reverse[int_0 >> 8 & 15] << 4 | (int)SimpleZip.DeflaterHuffman.bit4Reverse[int_0 >> 12]);
			}

			// Token: 0x060002EB RID: 747 RVA: 0x000292C4 File Offset: 0x000274C4
			static DeflaterHuffman()
			{
				int i = 0;
				while (i < 144)
				{
					SimpleZip.DeflaterHuffman.staticLCodes[i] = SimpleZip.DeflaterHuffman.BitReverse(48 + i << 8);
					SimpleZip.DeflaterHuffman.staticLLength[i++] = 8;
				}
				while (i < 256)
				{
					SimpleZip.DeflaterHuffman.staticLCodes[i] = SimpleZip.DeflaterHuffman.BitReverse(256 + i << 7);
					SimpleZip.DeflaterHuffman.staticLLength[i++] = 9;
				}
				while (i < 280)
				{
					SimpleZip.DeflaterHuffman.staticLCodes[i] = SimpleZip.DeflaterHuffman.BitReverse(-256 + i << 9);
					SimpleZip.DeflaterHuffman.staticLLength[i++] = 7;
				}
				while (i < 286)
				{
					SimpleZip.DeflaterHuffman.staticLCodes[i] = SimpleZip.DeflaterHuffman.BitReverse(-88 + i << 8);
					SimpleZip.DeflaterHuffman.staticLLength[i++] = 8;
				}
				SimpleZip.DeflaterHuffman.staticDCodes = new short[30];
				SimpleZip.DeflaterHuffman.staticDLength = new byte[30];
				for (i = 0; i < 30; i++)
				{
					SimpleZip.DeflaterHuffman.staticDCodes[i] = SimpleZip.DeflaterHuffman.BitReverse(i << 11);
					SimpleZip.DeflaterHuffman.staticDLength[i] = 5;
				}
			}

			// Token: 0x040001C3 RID: 451
			private static readonly int[] BL_ORDER = new int[]
			{
				16,
				17,
				18,
				0,
				8,
				7,
				9,
				6,
				10,
				5,
				11,
				4,
				12,
				3,
				13,
				2,
				14,
				1,
				15
			};

			// Token: 0x040001C4 RID: 452
			private static readonly byte[] bit4Reverse = new byte[]
			{
				0,
				8,
				4,
				12,
				2,
				10,
				6,
				14,
				1,
				9,
				5,
				13,
				3,
				11,
				7,
				15
			};

			// Token: 0x040001C5 RID: 453
			private static readonly short[] staticLCodes = new short[286];

			// Token: 0x040001C6 RID: 454
			private static readonly byte[] staticLLength = new byte[286];

			// Token: 0x040001C7 RID: 455
			private static readonly short[] staticDCodes;

			// Token: 0x040001C8 RID: 456
			private static readonly byte[] staticDLength;
		}

		// Token: 0x02000093 RID: 147
		internal sealed class ZipStream : MemoryStream
		{
			// Token: 0x060002EC RID: 748 RVA: 0x0000A03E File Offset: 0x0000823E
			public int ReadShort()
			{
				return this.ReadByte() | this.ReadByte() << 8;
			}

			// Token: 0x060002ED RID: 749 RVA: 0x0000A04F File Offset: 0x0000824F
			public int ReadInt()
			{
				return this.ReadShort() | this.ReadShort() << 16;
			}

			// Token: 0x060002EE RID: 750 RVA: 0x0000A061 File Offset: 0x00008261
			public ZipStream(byte[] byte_0) : base(byte_0, false)
			{
			}
		}
	}
}
