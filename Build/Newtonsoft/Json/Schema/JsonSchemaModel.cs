using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200029A RID: 666
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00014D08 File Offset: 0x00012F08
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x00014D10 File Offset: 0x00012F10
		public bool Required { get; set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00014D19 File Offset: 0x00012F19
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x00014D21 File Offset: 0x00012F21
		public JsonSchemaType Type { get; set; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00014D2A File Offset: 0x00012F2A
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00014D32 File Offset: 0x00012F32
		public int? MinimumLength { get; set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00014D3B File Offset: 0x00012F3B
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00014D43 File Offset: 0x00012F43
		public int? MaximumLength { get; set; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00014D4C File Offset: 0x00012F4C
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x00014D54 File Offset: 0x00012F54
		public double? DivisibleBy { get; set; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00014D5D File Offset: 0x00012F5D
		// (set) Token: 0x060013E5 RID: 5093 RVA: 0x00014D65 File Offset: 0x00012F65
		public double? Minimum { get; set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x00014D6E File Offset: 0x00012F6E
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x00014D76 File Offset: 0x00012F76
		public double? Maximum { get; set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x00014D7F File Offset: 0x00012F7F
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x00014D87 File Offset: 0x00012F87
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00014D90 File Offset: 0x00012F90
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x00014D98 File Offset: 0x00012F98
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00014DA1 File Offset: 0x00012FA1
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x00014DA9 File Offset: 0x00012FA9
		public int? MinimumItems { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x00014DB2 File Offset: 0x00012FB2
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x00014DBA File Offset: 0x00012FBA
		public int? MaximumItems { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00014DC3 File Offset: 0x00012FC3
		// (set) Token: 0x060013F1 RID: 5105 RVA: 0x00014DCB File Offset: 0x00012FCB
		public IList<string> Patterns { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00014DD4 File Offset: 0x00012FD4
		// (set) Token: 0x060013F3 RID: 5107 RVA: 0x00014DDC File Offset: 0x00012FDC
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00014DE5 File Offset: 0x00012FE5
		// (set) Token: 0x060013F5 RID: 5109 RVA: 0x00014DED File Offset: 0x00012FED
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00014DF6 File Offset: 0x00012FF6
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x00014DFE File Offset: 0x00012FFE
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00014E07 File Offset: 0x00013007
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x00014E0F File Offset: 0x0001300F
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00014E18 File Offset: 0x00013018
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x00014E20 File Offset: 0x00013020
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00014E29 File Offset: 0x00013029
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x00014E31 File Offset: 0x00013031
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00014E3A File Offset: 0x0001303A
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x00014E42 File Offset: 0x00013042
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00014E4B File Offset: 0x0001304B
		// (set) Token: 0x06001401 RID: 5121 RVA: 0x00014E53 File Offset: 0x00013053
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x00014E5C File Offset: 0x0001305C
		// (set) Token: 0x06001403 RID: 5123 RVA: 0x00014E64 File Offset: 0x00013064
		public bool UniqueItems { get; set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x00014E6D File Offset: 0x0001306D
		// (set) Token: 0x06001405 RID: 5125 RVA: 0x00014E75 File Offset: 0x00013075
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00014E7E File Offset: 0x0001307E
		// (set) Token: 0x06001407 RID: 5127 RVA: 0x00014E86 File Offset: 0x00013086
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x06001408 RID: 5128 RVA: 0x00014E8F File Offset: 0x0001308F
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00069390 File Offset: 0x00067590
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x000693E0 File Offset: 0x000675E0
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = (model.Required || schema.Required.GetValueOrDefault());
			model.Type &= (schema.Type ?? JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = (model.ExclusiveMinimum || schema.ExclusiveMinimum.GetValueOrDefault());
			model.ExclusiveMaximum = (model.ExclusiveMaximum || schema.ExclusiveMaximum.GetValueOrDefault());
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = (model.PositionalItemsValidation || schema.PositionalItemsValidation);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			model.AllowAdditionalItems = (model.AllowAdditionalItems && schema.AllowAdditionalItems);
			model.UniqueItems = (model.UniqueItems || schema.UniqueItems);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow.GetValueOrDefault();
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
