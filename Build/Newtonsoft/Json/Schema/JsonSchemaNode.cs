using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200029C RID: 668
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaNode
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00014F2E File Offset: 0x0001312E
		public string Id { get; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x00014F36 File Offset: 0x00013136
		public ReadOnlyCollection<JsonSchema> Schemas { get; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00014F3E File Offset: 0x0001313E
		public Dictionary<string, JsonSchemaNode> Properties { get; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x00014F46 File Offset: 0x00013146
		public Dictionary<string, JsonSchemaNode> PatternProperties { get; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00014F4E File Offset: 0x0001314E
		public List<JsonSchemaNode> Items { get; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00014F56 File Offset: 0x00013156
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x00014F5E File Offset: 0x0001315E
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00014F67 File Offset: 0x00013167
		// (set) Token: 0x0600141C RID: 5148 RVA: 0x00014F6F File Offset: 0x0001316F
		public JsonSchemaNode AdditionalItems { get; set; }

		// Token: 0x0600141D RID: 5149 RVA: 0x000699D8 File Offset: 0x00067BD8
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[]
			{
				schema
			});
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00069A34 File Offset: 0x00067C34
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union(new JsonSchema[]
			{
				schema
			}).ToList<JsonSchema>());
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.AdditionalItems = source.AdditionalItems;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00014F78 File Offset: 0x00013178
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00069AC8 File Offset: 0x00067CC8
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", (from s in schemata
			select s.InternalId).OrderBy((string id) => id, StringComparer.Ordinal));
		}
	}
}
