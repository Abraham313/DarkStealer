using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000292 RID: 658
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0001478A File Offset: 0x0001298A
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x00014792 File Offset: 0x00012992
		public string Id { get; set; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0001479B File Offset: 0x0001299B
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x000147A3 File Offset: 0x000129A3
		public string Title { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x000147AC File Offset: 0x000129AC
		// (set) Token: 0x06001356 RID: 4950 RVA: 0x000147B4 File Offset: 0x000129B4
		public bool? Required { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x000147BD File Offset: 0x000129BD
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x000147C5 File Offset: 0x000129C5
		public bool? ReadOnly { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x000147CE File Offset: 0x000129CE
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x000147D6 File Offset: 0x000129D6
		public bool? Hidden { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x000147DF File Offset: 0x000129DF
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x000147E7 File Offset: 0x000129E7
		public bool? Transient { get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x000147F0 File Offset: 0x000129F0
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x000147F8 File Offset: 0x000129F8
		public string Description { get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x00014801 File Offset: 0x00012A01
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x00014809 File Offset: 0x00012A09
		public JsonSchemaType? Type { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x00014812 File Offset: 0x00012A12
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0001481A File Offset: 0x00012A1A
		public string Pattern { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x00014823 File Offset: 0x00012A23
		// (set) Token: 0x06001364 RID: 4964 RVA: 0x0001482B File Offset: 0x00012A2B
		public int? MinimumLength { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x00014834 File Offset: 0x00012A34
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x0001483C File Offset: 0x00012A3C
		public int? MaximumLength { get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x00014845 File Offset: 0x00012A45
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x0001484D File Offset: 0x00012A4D
		public double? DivisibleBy { get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00014856 File Offset: 0x00012A56
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0001485E File Offset: 0x00012A5E
		public double? Minimum { get; set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00014867 File Offset: 0x00012A67
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0001486F File Offset: 0x00012A6F
		public double? Maximum { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00014878 File Offset: 0x00012A78
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00014880 File Offset: 0x00012A80
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00014889 File Offset: 0x00012A89
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x00014891 File Offset: 0x00012A91
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0001489A File Offset: 0x00012A9A
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x000148A2 File Offset: 0x00012AA2
		public int? MinimumItems { get; set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x000148AB File Offset: 0x00012AAB
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x000148B3 File Offset: 0x00012AB3
		public int? MaximumItems { get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x000148BC File Offset: 0x00012ABC
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x000148C4 File Offset: 0x00012AC4
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x000148CD File Offset: 0x00012ACD
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x000148D5 File Offset: 0x00012AD5
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x000148DE File Offset: 0x00012ADE
		// (set) Token: 0x0600137A RID: 4986 RVA: 0x000148E6 File Offset: 0x00012AE6
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x000148EF File Offset: 0x00012AEF
		// (set) Token: 0x0600137C RID: 4988 RVA: 0x000148F7 File Offset: 0x00012AF7
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00014900 File Offset: 0x00012B00
		// (set) Token: 0x0600137E RID: 4990 RVA: 0x00014908 File Offset: 0x00012B08
		public bool UniqueItems { get; set; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00014911 File Offset: 0x00012B11
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x00014919 File Offset: 0x00012B19
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00014922 File Offset: 0x00012B22
		// (set) Token: 0x06001382 RID: 4994 RVA: 0x0001492A File Offset: 0x00012B2A
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00014933 File Offset: 0x00012B33
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x0001493B File Offset: 0x00012B3B
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x00014944 File Offset: 0x00012B44
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x0001494C File Offset: 0x00012B4C
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00014955 File Offset: 0x00012B55
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x0001495D File Offset: 0x00012B5D
		public string Requires { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00014966 File Offset: 0x00012B66
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x0001496E File Offset: 0x00012B6E
		public IList<JToken> Enum { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x00014977 File Offset: 0x00012B77
		// (set) Token: 0x0600138C RID: 5004 RVA: 0x0001497F File Offset: 0x00012B7F
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00014988 File Offset: 0x00012B88
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x00014990 File Offset: 0x00012B90
		public JToken Default { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00014999 File Offset: 0x00012B99
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x000149A1 File Offset: 0x00012BA1
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x000149AA File Offset: 0x00012BAA
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x000149B2 File Offset: 0x00012BB2
		public string Format { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x000149BB File Offset: 0x00012BBB
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x000149C3 File Offset: 0x00012BC3
		internal string Location { get; set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x000149CC File Offset: 0x00012BCC
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x000149D4 File Offset: 0x00012BD4
		// (set) Token: 0x06001397 RID: 5015 RVA: 0x000149DC File Offset: 0x00012BDC
		internal string DeferredReference { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x000149E5 File Offset: 0x00012BE5
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x000149ED File Offset: 0x00012BED
		internal bool ReferencesResolved { get; set; }

		// Token: 0x0600139A RID: 5018 RVA: 0x00067AB8 File Offset: 0x00065CB8
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000149F6 File Offset: 0x00012BF6
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00014A03 File Offset: 0x00012C03
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00014A27 File Offset: 0x00012C27
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00067AF4 File Offset: 0x00065CF4
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				result = JsonSchema.Read(jsonReader, resolver);
			}
			return result;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00014A34 File Offset: 0x00012C34
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00014A42 File Offset: 0x00012C42
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00067B40 File Offset: 0x00065D40
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x04000B36 RID: 2870
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
