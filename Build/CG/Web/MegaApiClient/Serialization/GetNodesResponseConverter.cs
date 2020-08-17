using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000180 RID: 384
	internal class GetNodesResponseConverter : JsonConverter
	{
		// Token: 0x06000A7E RID: 2686 RVA: 0x0000E184 File Offset: 0x0000C384
		public GetNodesResponseConverter(byte[] masterKey)
		{
			this.masterKey = masterKey;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0000E193 File Offset: 0x0000C393
		public override bool CanConvert(Type objectType)
		{
			return typeof(GetNodesResponse) == objectType;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0004DB60 File Offset: 0x0004BD60
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JToken jtoken = JObject.Load(reader);
			GetNodesResponse getNodesResponse = new GetNodesResponse(this.masterKey);
			JsonReader jsonReader = jtoken.CreateReader();
			jsonReader.Culture = reader.Culture;
			jsonReader.DateFormatString = reader.DateFormatString;
			jsonReader.DateParseHandling = reader.DateParseHandling;
			jsonReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
			jsonReader.FloatParseHandling = reader.FloatParseHandling;
			jsonReader.MaxDepth = reader.MaxDepth;
			jsonReader.SupportMultipleContent = reader.SupportMultipleContent;
			serializer.Populate(jsonReader, getNodesResponse);
			return getNodesResponse;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000735 RID: 1845
		private readonly byte[] masterKey;
	}
}
