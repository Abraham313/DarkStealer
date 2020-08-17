using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000224 RID: 548
	internal static class JsonTokenUtils
	{
		// Token: 0x06000FB6 RID: 4022 RVA: 0x00011EA0 File Offset: 0x000100A0
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00011EAC File Offset: 0x000100AC
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00011EB7 File Offset: 0x000100B7
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
