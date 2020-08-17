using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x020000F7 RID: 247
	internal static class ZipOutput
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00039774 File Offset: 0x00037974
		public static bool WriteCentralDirectoryStructure(Stream s, ICollection<ZipEntry> entries, uint numSegments, Zip64Option zip64, string comment, ZipContainer container)
		{
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
			}
			long num = 0L;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				foreach (ZipEntry zipEntry in entries)
				{
					if (zipEntry.IncludedInMostRecentSave)
					{
						zipEntry.WriteCentralDirectoryEntry(memoryStream);
					}
				}
				byte[] array = memoryStream.ToArray();
				s.Write(array, 0, array.Length);
				num = (long)array.Length;
			}
			CountingStream countingStream = s as CountingStream;
			long num2 = (countingStream != null) ? countingStream.ComputedPosition : s.Position;
			long num3 = num2 - num;
			uint num4 = (zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 0U;
			long num5 = num2 - num3;
			int num6 = ZipOutput.CountEntries(entries);
			bool flag = zip64 == Zip64Option.Always || num6 >= 65535 || num5 > 4294967295L || num3 > 4294967295L;
			byte[] array3;
			if (flag)
			{
				if (zip64 == Zip64Option.Default)
				{
					StackFrame stackFrame = new StackFrame(1);
					if (stackFrame.GetMethod().DeclaringType == typeof(ZipFile))
					{
						throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipFile.UseZip64WhenSaving property.");
					}
					throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipOutputStream.EnableZip64 property.");
				}
				else
				{
					byte[] array2 = ZipOutput.GenZip64EndOfCentralDirectory(num3, num2, num6, numSegments);
					array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
					if (num4 != 0U)
					{
						uint value = zipSegmentedStream.ComputeSegment(array2.Length + array3.Length);
						Array.Copy(BitConverter.GetBytes(value), 0, array2, 16, 4);
						Array.Copy(BitConverter.GetBytes(value), 0, array2, 20, 4);
						Array.Copy(BitConverter.GetBytes(value), 0, array2, 60, 4);
						Array.Copy(BitConverter.GetBytes(value), 0, array2, 72, 4);
					}
					s.Write(array2, 0, array2.Length);
				}
			}
			else
			{
				array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
			}
			if (num4 != 0U)
			{
				ushort value2 = (ushort)zipSegmentedStream.ComputeSegment(array3.Length);
				Array.Copy(BitConverter.GetBytes(value2), 0, array3, 4, 2);
				Array.Copy(BitConverter.GetBytes(value2), 0, array3, 6, 2);
			}
			s.Write(array3, 0, array3.Length);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			return flag;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000399B8 File Offset: 0x00037BB8
		private static Encoding GetEncoding(ZipContainer container, string t)
		{
			switch (container.AlternateEncodingUsage)
			{
			case ZipOption.Default:
				return container.DefaultEncoding;
			case ZipOption.Always:
				return container.AlternateEncoding;
			}
			Encoding defaultEncoding = container.DefaultEncoding;
			if (t == null)
			{
				return defaultEncoding;
			}
			byte[] bytes = defaultEncoding.GetBytes(t);
			string @string = defaultEncoding.GetString(bytes, 0, bytes.Length);
			if (@string.Equals(t))
			{
				return defaultEncoding;
			}
			return container.AlternateEncoding;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00039A20 File Offset: 0x00037C20
		private static byte[] GenCentralDirectoryFooter(long StartOfCentralDirectory, long EndOfCentralDirectory, Zip64Option zip64, int entryCount, string comment, ZipContainer container)
		{
			Encoding encoding = ZipOutput.GetEncoding(container, comment);
			int num = 22;
			byte[] array = null;
			short num2 = 0;
			if (comment != null && comment.Length != 0)
			{
				array = encoding.GetBytes(comment);
				num2 = (short)array.Length;
			}
			num += (int)num2;
			byte[] array2 = new byte[num];
			byte[] bytes = BitConverter.GetBytes(101010256U);
			Array.Copy(bytes, 0, array2, 0, 4);
			array2[4] = 0;
			array2[5] = 0;
			array2[6] = 0;
			byte[] array3 = array2;
			int num3 = 7;
			int num4 = 8;
			array3[num3] = 0;
			if (entryCount < 65535)
			{
				if (zip64 != Zip64Option.Always)
				{
					array2[num4++] = (byte)(entryCount & 255);
					array2[num4++] = (byte)((entryCount & 65280) >> 8);
					array2[num4++] = (byte)(entryCount & 255);
					array2[num4++] = (byte)((entryCount & 65280) >> 8);
					goto IL_F2;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				array2[num4++] = byte.MaxValue;
			}
			IL_F2:
			long num5 = EndOfCentralDirectory - StartOfCentralDirectory;
			if (num5 < 4294967295L && StartOfCentralDirectory < 4294967295L)
			{
				array2[num4++] = (byte)(num5 & 255L);
				array2[num4++] = (byte)((num5 & 65280L) >> 8);
				array2[num4++] = (byte)((num5 & 16711680L) >> 16);
				array2[num4++] = (byte)((num5 & 4278190080L) >> 24);
				array2[num4++] = (byte)(StartOfCentralDirectory & 255L);
				array2[num4++] = (byte)((StartOfCentralDirectory & 65280L) >> 8);
				array2[num4++] = (byte)((StartOfCentralDirectory & 16711680L) >> 16);
				array2[num4++] = (byte)((StartOfCentralDirectory & 4278190080L) >> 24);
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					array2[num4++] = byte.MaxValue;
				}
			}
			if (comment != null && comment.Length != 0)
			{
				if ((int)num2 + num4 + 2 > array2.Length)
				{
					num2 = (short)(array2.Length - num4 - 2);
				}
				array2[num4++] = (byte)(num2 & 255);
				array2[num4++] = (byte)(((int)num2 & 65280) >> 8);
				if (num2 != 0)
				{
					int i = 0;
					while (i < (int)num2 && num4 + i < array2.Length)
					{
						array2[num4 + i] = array[i];
						i++;
					}
					num4 += i;
				}
			}
			else
			{
				array2[num4++] = 0;
				array2[num4++] = 0;
			}
			return array2;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00039CB8 File Offset: 0x00037EB8
		private static byte[] GenZip64EndOfCentralDirectory(long StartOfCentralDirectory, long EndOfCentralDirectory, int entryCount, uint numSegments)
		{
			byte[] array = new byte[76];
			byte[] bytes = BitConverter.GetBytes(101075792U);
			Array.Copy(bytes, 0, array, 0, 4);
			Array.Copy(BitConverter.GetBytes(44L), 0, array, 4, 8);
			array[12] = 45;
			array[13] = 0;
			array[14] = 45;
			byte[] array2 = array;
			int num = 15;
			int num2 = 16;
			array2[num] = 0;
			for (int i = 0; i < 8; i++)
			{
				array[num2++] = 0;
			}
			long value = (long)entryCount;
			Array.Copy(BitConverter.GetBytes(value), 0, array, num2, 8);
			num2 += 8;
			Array.Copy(BitConverter.GetBytes(value), 0, array, num2, 8);
			num2 += 8;
			long value2 = EndOfCentralDirectory - StartOfCentralDirectory;
			Array.Copy(BitConverter.GetBytes(value2), 0, array, num2, 8);
			num2 += 8;
			Array.Copy(BitConverter.GetBytes(StartOfCentralDirectory), 0, array, num2, 8);
			num2 += 8;
			bytes = BitConverter.GetBytes(117853008U);
			Array.Copy(bytes, 0, array, num2, 4);
			num2 += 4;
			uint value3 = (numSegments == 0U) ? 0U : (numSegments - 1U);
			Array.Copy(BitConverter.GetBytes(value3), 0, array, num2, 4);
			num2 += 4;
			Array.Copy(BitConverter.GetBytes(EndOfCentralDirectory), 0, array, num2, 8);
			num2 += 8;
			Array.Copy(BitConverter.GetBytes(numSegments), 0, array, num2, 4);
			num2 += 4;
			return array;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00039DF0 File Offset: 0x00037FF0
		private static int CountEntries(ICollection<ZipEntry> _entries)
		{
			int num = 0;
			foreach (ZipEntry zipEntry in _entries)
			{
				if (zipEntry.IncludedInMostRecentSave)
				{
					num++;
				}
			}
			return num;
		}
	}
}
