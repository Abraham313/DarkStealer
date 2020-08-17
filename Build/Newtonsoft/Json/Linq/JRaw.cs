using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C2 RID: 706
	[NullableContext(1)]
	[Nullable(0)]
	public class JRaw : JValue
	{
		// Token: 0x060015BE RID: 5566 RVA: 0x0001606F File Offset: 0x0001426F
		public JRaw(JRaw other) : base(other)
		{
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00016078 File Offset: 0x00014278
		[NullableContext(2)]
		public JRaw(object rawJson) : base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0006C758 File Offset: 0x0006A958
		public static JRaw Create(JsonReader reader)
		{
			JRaw result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					result = new JRaw(stringWriter.ToString());
				}
			}
			return result;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00016083 File Offset: 0x00014283
		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
