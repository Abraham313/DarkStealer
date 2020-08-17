using System;
using System.IO;
using System.Runtime.Serialization;
using DamienG.Security.Cryptography;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000175 RID: 373
	internal class Attributes
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x00008754 File Offset: 0x00006954
		[JsonConstructor]
		private Attributes()
		{
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0000DF11 File Offset: 0x0000C111
		public Attributes(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000DF20 File Offset: 0x0000C120
		public Attributes(string name, Attributes originalAttributes)
		{
			this.Name = name;
			this.SerializedFingerprint = originalAttributes.SerializedFingerprint;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0004D6E8 File Offset: 0x0004B8E8
		public Attributes(string name, Stream stream, DateTime? modificationDate = null)
		{
			this.Name = name;
			if (modificationDate != null)
			{
				byte[] array = new byte[25];
				Buffer.BlockCopy(this.ComputeCrc(stream), 0, array, 0, 16);
				byte[] array2 = modificationDate.Value.ToEpoch().SerializeToBytes();
				Buffer.BlockCopy(array2, 0, array, 16, array2.Length);
				Array.Resize<byte>(ref array, array.Length - 9 + array2.Length);
				this.SerializedFingerprint = array.ToBase64();
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0000DF3B File Offset: 0x0000C13B
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0000DF43 File Offset: 0x0000C143
		[JsonProperty("n")]
		public string Name { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0000DF4C File Offset: 0x0000C14C
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x0000DF54 File Offset: 0x0000C154
		[JsonProperty("c", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string SerializedFingerprint { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0000DF5D File Offset: 0x0000C15D
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0000DF65 File Offset: 0x0000C165
		[JsonIgnore]
		public DateTime? ModificationDate { get; private set; }

		// Token: 0x06000A42 RID: 2626 RVA: 0x0004D760 File Offset: 0x0004B960
		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			if (this.SerializedFingerprint != null)
			{
				byte[] array = this.SerializedFingerprint.FromBase64();
				this.ModificationDate = new DateTime?(array.DeserializeToLong(16, array.Length - 16).ToDateTime());
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0004D7A0 File Offset: 0x0004B9A0
		private uint[] ComputeCrc(Stream stream)
		{
			stream.Seek(0L, SeekOrigin.Begin);
			uint[] array = new uint[4];
			byte[] array2 = new byte[16];
			uint num = 0U;
			if (stream.Length <= 16L)
			{
				if (stream.Read(array2, 0, (int)stream.Length) != 0)
				{
					Buffer.BlockCopy(array2, 0, array, 0, array2.Length);
				}
			}
			else if (stream.Length <= 8192L)
			{
				byte[] buffer = new byte[stream.Length];
				int num2 = 0;
				while ((long)(num2 += stream.Read(buffer, num2, (int)stream.Length - num2)) < stream.Length)
				{
				}
				for (int i = 0; i < array.Length; i++)
				{
					int num3 = (int)((long)i * stream.Length / (long)array.Length);
					int num4 = (int)((long)(i + 1) * stream.Length / (long)array.Length);
					using (Crc32 crc = new Crc32(3988292384U, uint.MaxValue))
					{
						num = BitConverter.ToUInt32(crc.ComputeHash(buffer, num3, num4 - num3), 0);
					}
					array[i] = num;
				}
			}
			else
			{
				byte[] array3 = new byte[64];
				uint num5 = (uint)(8192 / (array3.Length * 4));
				long num6 = 0L;
				for (uint num7 = 0U; num7 < 4U; num7 += 1U)
				{
					byte[] array4 = null;
					uint num8 = uint.MaxValue;
					for (uint num9 = 0U; num9 < num5; num9 += 1U)
					{
						long num10 = (stream.Length - (long)array3.Length) * (long)((ulong)(num7 * num5 + num9)) / (long)((ulong)(4U * num5 - 1U));
						stream.Seek(num10 - num6, SeekOrigin.Current);
						num6 += num10 - num6;
						int num11 = stream.Read(array3, 0, array3.Length);
						num6 += (long)num11;
						using (Crc32 crc2 = new Crc32(3988292384U, num8))
						{
							array4 = crc2.ComputeHash(array3, 0, num11);
							byte[] array5 = new byte[array4.Length];
							array4.CopyTo(array5, 0);
							if (BitConverter.IsLittleEndian)
							{
								Array.Reverse(array5);
							}
							num8 = BitConverter.ToUInt32(array5, 0);
							num8 = ~num8;
						}
					}
					num = BitConverter.ToUInt32(array4, 0);
					array[(int)num7] = num;
				}
			}
			return array;
		}

		// Token: 0x04000715 RID: 1813
		private const int CrcArrayLength = 4;

		// Token: 0x04000716 RID: 1814
		private const int CrcSize = 16;

		// Token: 0x04000717 RID: 1815
		private const int FingerprintMaxSize = 25;

		// Token: 0x04000718 RID: 1816
		private const int MAXFULL = 8192;

		// Token: 0x04000719 RID: 1817
		private const uint CryptoPPCRC32Polynomial = 3988292384U;
	}
}
