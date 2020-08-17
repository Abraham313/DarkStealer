using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000190 RID: 400
	internal class ShareDataConverter : JsonConverter
	{
		// Token: 0x06000AD0 RID: 2768 RVA: 0x0004DE48 File Offset: 0x0004C048
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ShareData shareData = value as ShareData;
			if (shareData == null)
			{
				throw new ArgumentException("invalid data to serialize");
			}
			writer.WriteStartArray();
			writer.WriteStartArray();
			writer.WriteValue(shareData.NodeId);
			writer.WriteEndArray();
			writer.WriteStartArray();
			foreach (ShareData.ShareDataItem shareDataItem in shareData.Items)
			{
				writer.WriteValue(shareDataItem.NodeId);
			}
			writer.WriteEndArray();
			writer.WriteStartArray();
			int num = 0;
			foreach (ShareData.ShareDataItem shareDataItem2 in shareData.Items)
			{
				writer.WriteValue(0);
				writer.WriteValue(num++);
				writer.WriteValue(Crypto.EncryptKey(shareDataItem2.Data, shareDataItem2.Key).ToBase64());
			}
			writer.WriteEndArray();
			writer.WriteEndArray();
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0000A639 File Offset: 0x00008839
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0000E49E File Offset: 0x0000C69E
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ShareData);
		}
	}
}
