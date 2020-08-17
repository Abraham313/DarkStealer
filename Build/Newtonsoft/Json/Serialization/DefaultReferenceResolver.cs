using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000257 RID: 599
	[NullableContext(1)]
	[Nullable(0)]
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x00060498 File Offset: 0x0005E698
		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase = context as JsonSerializerInternalBase;
			if (jsonSerializerInternalBase == null)
			{
				JsonSerializerProxy jsonSerializerProxy = context as JsonSerializerProxy;
				if (jsonSerializerProxy == null)
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = jsonSerializerProxy.GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x000604D4 File Offset: 0x0005E6D4
		public object ResolveReference(object context, string reference)
		{
			object result;
			this.GetMappings(context).TryGetByFirst(reference, out result);
			return result;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000604F4 File Offset: 0x0005E6F4
		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
			string text;
			if (!mappings.TryGetBySecond(value, out text))
			{
				this._referenceCount++;
				text = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(text, value);
			}
			return text;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00012A84 File Offset: 0x00010C84
		public void AddReference(object context, string reference, object value)
		{
			this.GetMappings(context).Set(reference, value);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0006053C File Offset: 0x0005E73C
		public bool IsReferenced(object context, object value)
		{
			string text;
			return this.GetMappings(context).TryGetBySecond(value, out text);
		}

		// Token: 0x04000A4A RID: 2634
		private int _referenceCount;
	}
}
