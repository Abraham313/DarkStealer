using System;
using System.IO;

namespace Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON
{
	// Token: 0x0200003B RID: 59
	public static class JsonExt
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000090FF File Offset: 0x000072FF
		public static JsonValue FromJSON(this string json)
		{
			return JsonValue.Load(new StringReader(json));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000910C File Offset: 0x0000730C
		public static string ToJSON<T>(this T instance)
		{
			return JsonValue.ToJsonValue<T>(instance);
		}
	}
}
