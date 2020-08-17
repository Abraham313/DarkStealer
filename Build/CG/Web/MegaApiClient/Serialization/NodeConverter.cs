using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000185 RID: 389
	internal class NodeConverter : JsonConverter
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		public NodeConverter(byte[] masterKey, ref List<SharedKey> sharedKeys)
		{
			this.masterKey = masterKey;
			this.sharedKeys = sharedKeys;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		public override bool CanConvert(Type objectType)
		{
			return typeof(Node) == objectType;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0004DBF0 File Offset: 0x0004BDF0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JToken jtoken = JObject.Load(reader);
			Node node = new Node(this.masterKey, ref this.sharedKeys);
			JsonReader jsonReader = jtoken.CreateReader();
			jsonReader.Culture = reader.Culture;
			jsonReader.DateFormatString = reader.DateFormatString;
			jsonReader.DateParseHandling = reader.DateParseHandling;
			jsonReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
			jsonReader.FloatParseHandling = reader.FloatParseHandling;
			jsonReader.MaxDepth = reader.MaxDepth;
			jsonReader.SupportMultipleContent = reader.SupportMultipleContent;
			serializer.Populate(jsonReader, node);
			return node;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400073F RID: 1855
		private readonly byte[] masterKey;

		// Token: 0x04000740 RID: 1856
		private List<SharedKey> sharedKeys;
	}
}
