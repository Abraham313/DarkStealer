using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000295 RID: 661
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal static class JsonSchemaConstants
	{
		// Token: 0x04000B3F RID: 2879
		public const string TypePropertyName = "type";

		// Token: 0x04000B40 RID: 2880
		public const string PropertiesPropertyName = "properties";

		// Token: 0x04000B41 RID: 2881
		public const string ItemsPropertyName = "items";

		// Token: 0x04000B42 RID: 2882
		public const string AdditionalItemsPropertyName = "additionalItems";

		// Token: 0x04000B43 RID: 2883
		public const string RequiredPropertyName = "required";

		// Token: 0x04000B44 RID: 2884
		public const string PatternPropertiesPropertyName = "patternProperties";

		// Token: 0x04000B45 RID: 2885
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x04000B46 RID: 2886
		public const string RequiresPropertyName = "requires";

		// Token: 0x04000B47 RID: 2887
		public const string MinimumPropertyName = "minimum";

		// Token: 0x04000B48 RID: 2888
		public const string MaximumPropertyName = "maximum";

		// Token: 0x04000B49 RID: 2889
		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		// Token: 0x04000B4A RID: 2890
		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		// Token: 0x04000B4B RID: 2891
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x04000B4C RID: 2892
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x04000B4D RID: 2893
		public const string PatternPropertyName = "pattern";

		// Token: 0x04000B4E RID: 2894
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x04000B4F RID: 2895
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x04000B50 RID: 2896
		public const string EnumPropertyName = "enum";

		// Token: 0x04000B51 RID: 2897
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x04000B52 RID: 2898
		public const string TitlePropertyName = "title";

		// Token: 0x04000B53 RID: 2899
		public const string DescriptionPropertyName = "description";

		// Token: 0x04000B54 RID: 2900
		public const string FormatPropertyName = "format";

		// Token: 0x04000B55 RID: 2901
		public const string DefaultPropertyName = "default";

		// Token: 0x04000B56 RID: 2902
		public const string TransientPropertyName = "transient";

		// Token: 0x04000B57 RID: 2903
		public const string DivisibleByPropertyName = "divisibleBy";

		// Token: 0x04000B58 RID: 2904
		public const string HiddenPropertyName = "hidden";

		// Token: 0x04000B59 RID: 2905
		public const string DisallowPropertyName = "disallow";

		// Token: 0x04000B5A RID: 2906
		public const string ExtendsPropertyName = "extends";

		// Token: 0x04000B5B RID: 2907
		public const string IdPropertyName = "id";

		// Token: 0x04000B5C RID: 2908
		public const string UniqueItemsPropertyName = "uniqueItems";

		// Token: 0x04000B5D RID: 2909
		public const string OptionValuePropertyName = "value";

		// Token: 0x04000B5E RID: 2910
		public const string OptionLabelPropertyName = "label";

		// Token: 0x04000B5F RID: 2911
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping = new Dictionary<string, JsonSchemaType>
		{
			{
				"string",
				JsonSchemaType.String
			},
			{
				"object",
				JsonSchemaType.Object
			},
			{
				"integer",
				JsonSchemaType.Integer
			},
			{
				"number",
				JsonSchemaType.Float
			},
			{
				"null",
				JsonSchemaType.Null
			},
			{
				"boolean",
				JsonSchemaType.Boolean
			},
			{
				"array",
				JsonSchemaType.Array
			},
			{
				"any",
				JsonSchemaType.Any
			}
		};
	}
}
