using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200029F RID: 671
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x00014FA5 File Offset: 0x000131A5
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x00014FAD File Offset: 0x000131AD
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x06001429 RID: 5161 RVA: 0x00014FB6 File Offset: 0x000131B6
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00069B30 File Offset: 0x00067D30
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
