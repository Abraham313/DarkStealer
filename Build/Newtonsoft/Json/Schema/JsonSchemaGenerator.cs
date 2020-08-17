using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000297 RID: 663
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaGenerator
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x00014BCD File Offset: 0x00012DCD
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x00014BD5 File Offset: 0x00012DD5
		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00014BDE File Offset: 0x00012DDE
		// (set) Token: 0x060013C2 RID: 5058 RVA: 0x00014BF4 File Offset: 0x00012DF4
		public IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00014BFD File Offset: 0x00012DFD
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00014C05 File Offset: 0x00012E05
		private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
		{
			this._currentSchema = typeSchema.Schema;
			this._stack.Add(typeSchema);
			this._resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00068AF0 File Offset: 0x00066CF0
		private JsonSchemaGenerator.TypeSchema Pop()
		{
			JsonSchemaGenerator.TypeSchema result = this._stack[this._stack.Count - 1];
			this._stack.RemoveAt(this._stack.Count - 1);
			JsonSchemaGenerator.TypeSchema typeSchema = this._stack.LastOrDefault<JsonSchemaGenerator.TypeSchema>();
			if (typeSchema != null)
			{
				this._currentSchema = typeSchema.Schema;
				return result;
			}
			this._currentSchema = null;
			return result;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00014C35 File Offset: 0x00012E35
		public JsonSchema Generate(Type type)
		{
			return this.Generate(type, new JsonSchemaResolver(), false);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00014C44 File Offset: 0x00012E44
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return this.Generate(type, resolver, false);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00014C4F File Offset: 0x00012E4F
		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00014C5E File Offset: 0x00012E5E
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			this._resolver = resolver;
			return this.GenerateInternal(type, (!rootSchemaNullable) ? Required.Always : Required.Default, false);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00068B50 File Offset: 0x00066D50
		private string GetTitle(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (!StringUtils.IsNullOrEmpty((cachedAttribute != null) ? cachedAttribute.Title : null))
			{
				return cachedAttribute.Title;
			}
			return null;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00068B80 File Offset: 0x00066D80
		private string GetDescription(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (!StringUtils.IsNullOrEmpty((cachedAttribute != null) ? cachedAttribute.Description : null))
			{
				return cachedAttribute.Description;
			}
			DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>(type);
			if (attribute == null)
			{
				return null;
			}
			return attribute.Description;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00068BC0 File Offset: 0x00066DC0
		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (!StringUtils.IsNullOrEmpty((cachedAttribute != null) ? cachedAttribute.Id : null))
			{
				return cachedAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			UndefinedSchemaIdHandling undefinedSchemaIdHandling = this.UndefinedSchemaIdHandling;
			if (undefinedSchemaIdHandling == UndefinedSchemaIdHandling.UseTypeName)
			{
				return type.FullName;
			}
			if (undefinedSchemaIdHandling != UndefinedSchemaIdHandling.UseAssemblyQualifiedName)
			{
				return null;
			}
			return type.AssemblyQualifiedName;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00068C14 File Offset: 0x00066E14
		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = this.GetTypeId(type, false);
			string typeId2 = this.GetTypeId(type, true);
			if (!StringUtils.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = this._resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
					{
						schema.Type |= JsonSchemaType.Null;
					}
					if (required)
					{
						bool? required2 = schema.Required;
						if (!(required2.GetValueOrDefault() & required2 != null))
						{
							schema.Required = new bool?(true);
						}
					}
					return schema;
				}
			}
			if (this._stack.Any((JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new JsonException("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
			bool flag = (jsonContract.Converter ?? jsonContract.InternalConverter) != null;
			this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				this.CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				this.CurrentSchema.Required = new bool?(true);
			}
			this.CurrentSchema.Title = this.GetTitle(type);
			this.CurrentSchema.Description = this.GetDescription(type);
			if (flag)
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
			}
			else
			{
				switch (jsonContract.ContractType)
				{
				case JsonContractType.Object:
					this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
					this.CurrentSchema.Id = this.GetTypeId(type, false);
					this.GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
					break;
				case JsonContractType.Array:
				{
					this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
					this.CurrentSchema.Id = this.GetTypeId(type, false);
					JsonArrayAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonArrayAttribute>(type);
					bool flag2 = cachedAttribute == null || cachedAttribute.AllowNullItems;
					Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
					if (collectionItemType != null)
					{
						this.CurrentSchema.Items = new List<JsonSchema>();
						this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, (!flag2) ? Required.Always : Required.Default, false));
					}
					break;
				}
				case JsonContractType.Primitive:
				{
					this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
					JsonSchemaType? type2 = this.CurrentSchema.Type;
					if ((type2.GetValueOrDefault() == JsonSchemaType.Integer & type2 != null) && type.IsEnum() && !type.IsDefined(typeof(FlagsAttribute), true))
					{
						this.CurrentSchema.Enum = new List<JToken>();
						EnumInfo enumValuesAndNames = EnumUtils.GetEnumValuesAndNames(type);
						for (int i = 0; i < enumValuesAndNames.Names.Length; i++)
						{
							ulong value = enumValuesAndNames.Values[i];
							JToken item = JToken.FromObject(Enum.ToObject(type, value));
							this.CurrentSchema.Enum.Add(item);
						}
					}
					break;
				}
				case JsonContractType.String:
				{
					JsonSchemaType value2 = (!ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired);
					this.CurrentSchema.Type = new JsonSchemaType?(value2);
					break;
				}
				case JsonContractType.Dictionary:
				{
					this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
					Type type3;
					Type type4;
					ReflectionUtils.GetDictionaryKeyValueTypes(type, out type3, out type4);
					if (type3 != null && this.ContractResolver.ResolveContract(type3).ContractType == JsonContractType.Primitive)
					{
						this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type4, Required.Default, false);
					}
					break;
				}
				case JsonContractType.Dynamic:
				case JsonContractType.Linq:
					this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
					break;
				case JsonContractType.Serializable:
					this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
					this.CurrentSchema.Id = this.GetTypeId(type, false);
					this.GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
					break;
				default:
					throw new JsonException("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContract));
				}
			}
			return this.Pop().Schema;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00014C8C File Offset: 0x00012E8C
		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired != Required.Always)
			{
				return type | JsonSchemaType.Null;
			}
			return type;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00013680 File Offset: 0x00011880
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000690C0 File Offset: 0x000672C0
		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			foreach (JsonProperty jsonProperty in contract.Properties)
			{
				if (!jsonProperty.Ignored)
				{
					NullValueHandling? nullValueHandling = jsonProperty.NullValueHandling;
					bool flag = (nullValueHandling.GetValueOrDefault() == NullValueHandling.Ignore & nullValueHandling != null) || this.HasFlag(jsonProperty.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || jsonProperty.ShouldSerialize != null || jsonProperty.GetIsSpecified != null;
					JsonSchema jsonSchema = this.GenerateInternal(jsonProperty.PropertyType, jsonProperty.Required, !flag);
					if (jsonProperty.DefaultValue != null)
					{
						jsonSchema.Default = JToken.FromObject(jsonProperty.DefaultValue);
					}
					this.CurrentSchema.Properties.Add(jsonProperty.PropertyName, jsonSchema);
				}
			}
			if (type.IsSealed())
			{
				this.CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00014C98 File Offset: 0x00012E98
		private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
		{
			this.CurrentSchema.AllowAdditionalProperties = true;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000691D0 File Offset: 0x000673D0
		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			if (value == null)
			{
				return true;
			}
			JsonSchemaType? jsonSchemaType = value & flag;
			if (jsonSchemaType.GetValueOrDefault() == flag & jsonSchemaType != null)
			{
				return true;
			}
			if (flag == JsonSchemaType.Integer)
			{
				jsonSchemaType = (value & JsonSchemaType.Float);
				if (jsonSchemaType.GetValueOrDefault() == JsonSchemaType.Float & jsonSchemaType != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0006926C File Offset: 0x0006746C
		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
			case PrimitiveTypeCode.Empty:
			case PrimitiveTypeCode.Object:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.Char:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.Boolean:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
			case PrimitiveTypeCode.BigInteger:
				return jsonSchemaType | JsonSchemaType.Integer;
			case PrimitiveTypeCode.Single:
			case PrimitiveTypeCode.Double:
			case PrimitiveTypeCode.Decimal:
				return jsonSchemaType | JsonSchemaType.Float;
			case PrimitiveTypeCode.DateTime:
			case PrimitiveTypeCode.DateTimeOffset:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.Guid:
			case PrimitiveTypeCode.TimeSpan:
			case PrimitiveTypeCode.Uri:
			case PrimitiveTypeCode.String:
			case PrimitiveTypeCode.Bytes:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.DBNull:
				return jsonSchemaType | JsonSchemaType.Null;
			}
			throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeCode, type));
		}

		// Token: 0x04000B64 RID: 2916
		private IContractResolver _contractResolver;

		// Token: 0x04000B65 RID: 2917
		private JsonSchemaResolver _resolver;

		// Token: 0x04000B66 RID: 2918
		private readonly IList<JsonSchemaGenerator.TypeSchema> _stack = new List<JsonSchemaGenerator.TypeSchema>();

		// Token: 0x04000B67 RID: 2919
		private JsonSchema _currentSchema;

		// Token: 0x02000298 RID: 664
		private class TypeSchema
		{
			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00014CB9 File Offset: 0x00012EB9
			public Type Type { get; }

			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x060013D6 RID: 5078 RVA: 0x00014CC1 File Offset: 0x00012EC1
			public JsonSchema Schema { get; }

			// Token: 0x060013D7 RID: 5079 RVA: 0x00014CC9 File Offset: 0x00012EC9
			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				this.Type = type;
				this.Schema = schema;
			}
		}
	}
}
