using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200018B RID: 395
	[JsonConverter(typeof(SessionHistoryResponse.SessionHistoryConverter))]
	internal class SessionHistoryResponse : Collection<ISession>
	{
		// Token: 0x0200018C RID: 396
		internal class SessionHistoryConverter : JsonConverter
		{
			// Token: 0x06000AB1 RID: 2737 RVA: 0x0000E3A7 File Offset: 0x0000C5A7
			public override bool CanConvert(Type objectType)
			{
				return typeof(SessionHistoryResponse.SessionHistoryConverter.Session) == objectType;
			}

			// Token: 0x06000AB2 RID: 2738 RVA: 0x0004DC88 File Offset: 0x0004BE88
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				if (reader.TokenType == JsonToken.Null)
				{
					return null;
				}
				SessionHistoryResponse sessionHistoryResponse = new SessionHistoryResponse();
				foreach (JArray jArray in JArray.Load(reader).OfType<JArray>())
				{
					sessionHistoryResponse.Add(new SessionHistoryResponse.SessionHistoryConverter.Session(jArray));
				}
				return sessionHistoryResponse;
			}

			// Token: 0x06000AB3 RID: 2739 RVA: 0x0000983A File Offset: 0x00007A3A
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0200018D RID: 397
			private class Session : ISession
			{
				// Token: 0x06000AB5 RID: 2741 RVA: 0x0004DCF4 File Offset: 0x0004BEF4
				public Session(JArray jArray)
				{
					try
					{
						this.LoginTime = jArray.Value<long>(0).ToDateTime();
						this.LastSeenTime = jArray.Value<long>(1).ToDateTime();
						this.Client = jArray.Value<string>(2);
						this.IpAddress = IPAddress.Parse(jArray.Value<string>(3));
						this.Country = jArray.Value<string>(4);
						this.SessionId = jArray.Value<string>(6);
						jArray.Value<long>(7);
						if (jArray.Value<long>(5) == 1L)
						{
							this.Status |= SessionStatus.Current;
						}
						if (jArray.Value<long>(7) == 1L)
						{
							this.Status |= SessionStatus.Active;
						}
						if (this.Status == SessionStatus.Undefined)
						{
							this.Status = SessionStatus.Expired;
						}
					}
					catch (Exception ex)
					{
						this.Client = "Deserialization error: " + ex.Message;
					}
				}

				// Token: 0x17000237 RID: 567
				// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0000E3C1 File Offset: 0x0000C5C1
				// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0000E3C9 File Offset: 0x0000C5C9
				public string Client { get; private set; }

				// Token: 0x17000238 RID: 568
				// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0000E3D2 File Offset: 0x0000C5D2
				// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0000E3DA File Offset: 0x0000C5DA
				public IPAddress IpAddress { get; private set; }

				// Token: 0x17000239 RID: 569
				// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0000E3E3 File Offset: 0x0000C5E3
				// (set) Token: 0x06000ABB RID: 2747 RVA: 0x0000E3EB File Offset: 0x0000C5EB
				public string Country { get; private set; }

				// Token: 0x1700023A RID: 570
				// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0000E3F4 File Offset: 0x0000C5F4
				// (set) Token: 0x06000ABD RID: 2749 RVA: 0x0000E3FC File Offset: 0x0000C5FC
				public DateTime LoginTime { get; private set; }

				// Token: 0x1700023B RID: 571
				// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0000E405 File Offset: 0x0000C605
				// (set) Token: 0x06000ABF RID: 2751 RVA: 0x0000E40D File Offset: 0x0000C60D
				public DateTime LastSeenTime { get; private set; }

				// Token: 0x1700023C RID: 572
				// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0000E416 File Offset: 0x0000C616
				// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0000E41E File Offset: 0x0000C61E
				public SessionStatus Status { get; private set; }

				// Token: 0x1700023D RID: 573
				// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0000E427 File Offset: 0x0000C627
				// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0000E42F File Offset: 0x0000C62F
				public string SessionId { get; private set; }
			}
		}
	}
}
