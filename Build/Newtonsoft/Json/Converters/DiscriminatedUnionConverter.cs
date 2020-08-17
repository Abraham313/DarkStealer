using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002F5 RID: 757
	[NullableContext(1)]
	[Nullable(0)]
	public class DiscriminatedUnionConverter : JsonConverter
	{
		// Token: 0x060017C9 RID: 6089 RVA: 0x0007386C File Offset: 0x00071A6C
		private static Type CreateUnionTypeLookup(Type t)
		{
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object target = null;
			object[] array = new object[2];
			array[0] = t;
			object arg = ((object[])getUnionCases(target, array)).First<object>();
			return (Type)FSharpUtils.Instance.GetUnionCaseInfoDeclaringType(arg);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000738B4 File Offset: 0x00071AB4
		private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
		{
			MethodCall<object, object> preComputeUnionTagReader = FSharpUtils.Instance.PreComputeUnionTagReader;
			object target = null;
			object[] array = new object[2];
			array[0] = t;
			DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union((FSharpFunction)preComputeUnionTagReader(target, array), new List<DiscriminatedUnionConverter.UnionCase>());
			MethodCall<object, object> getUnionCases = FSharpUtils.Instance.GetUnionCases;
			object target2 = null;
			object[] array2 = new object[2];
			array2[0] = t;
			foreach (object obj in (object[])getUnionCases(target2, array2))
			{
				int tag = (int)FSharpUtils.Instance.GetUnionCaseInfoTag(obj);
				string name = (string)FSharpUtils.Instance.GetUnionCaseInfoName(obj);
				PropertyInfo[] fields = (PropertyInfo[])FSharpUtils.Instance.GetUnionCaseInfoFields(obj, new object[0]);
				MethodCall<object, object> preComputeUnionReader = FSharpUtils.Instance.PreComputeUnionReader;
				object target3 = null;
				object[] array4 = new object[2];
				array4[0] = obj;
				FSharpFunction fieldReader = (FSharpFunction)preComputeUnionReader(target3, array4);
				MethodCall<object, object> preComputeUnionConstructor = FSharpUtils.Instance.PreComputeUnionConstructor;
				object target4 = null;
				object[] array5 = new object[2];
				array5[0] = obj;
				DiscriminatedUnionConverter.UnionCase item = new DiscriminatedUnionConverter.UnionCase(tag, name, fields, fieldReader, (FSharpFunction)preComputeUnionConstructor(target4, array5));
				union.Cases.Add(item);
			}
			return union;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000739BC File Offset: 0x00071BBC
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type key = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
			DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(key);
			int tag = (int)union.TagReader.Invoke(new object[]
			{
				value
			});
			DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single((DiscriminatedUnionConverter.UnionCase c) => c.Tag == tag);
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue(unionCase.Name);
			if (unionCase.Fields != null && unionCase.Fields.Length != 0)
			{
				object[] array = (object[])unionCase.FieldReader.Invoke(new object[]
				{
					value
				});
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				writer.WriteStartArray();
				foreach (object value2 in array)
				{
					serializer.Serialize(writer, value2);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00073AEC File Offset: 0x00071CEC
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DiscriminatedUnionConverter.UnionCase unionCase = null;
			string caseName = null;
			JArray jarray = null;
			reader.ReadAndAssert();
			Func<DiscriminatedUnionConverter.UnionCase, bool> <>9__0;
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (string.Equals(text, "Case", StringComparison.OrdinalIgnoreCase))
				{
					reader.ReadAndAssert();
					DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
					caseName = reader.Value.ToString();
					IEnumerable<DiscriminatedUnionConverter.UnionCase> cases = union.Cases;
					Func<DiscriminatedUnionConverter.UnionCase, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((DiscriminatedUnionConverter.UnionCase c) => c.Name == caseName));
					}
					unionCase = cases.SingleOrDefault(predicate);
					if (unionCase == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
					}
				}
				else
				{
					if (!string.Equals(text, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text));
					}
					reader.ReadAndAssert();
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jarray = (JArray)JToken.ReadFrom(reader);
				}
				reader.ReadAndAssert();
			}
			if (unionCase == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			object[] array = new object[unionCase.Fields.Length];
			if (unionCase.Fields.Length != 0 && jarray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jarray != null)
			{
				if (unionCase.Fields.Length != jarray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, caseName));
				}
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					PropertyInfo propertyInfo = unionCase.Fields[i];
					array[i] = jtoken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			object[] args = new object[]
			{
				array
			};
			return unionCase.Constructor.Invoke(args);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00073CF8 File Offset: 0x00071EF8
		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(true);
			bool flag = false;
			object[] array = customAttributes;
			int i = 0;
			while (i < array.Length)
			{
				Type type = array[i].GetType();
				if (!(type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute"))
				{
					i++;
				}
				else
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					IL_56:
					if (!flag)
					{
						return false;
					}
					MethodCall<object, object> isUnion = FSharpUtils.Instance.IsUnion;
					object target = null;
					object[] array2 = new object[2];
					array2[0] = objectType;
					return (bool)isUnion(target, array2);
				}
			}
			goto IL_56;
		}

		// Token: 0x04000CD2 RID: 3282
		private const string CasePropertyName = "Case";

		// Token: 0x04000CD3 RID: 3283
		private const string FieldsPropertyName = "Fields";

		// Token: 0x04000CD4 RID: 3284
		private static readonly ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));

		// Token: 0x04000CD5 RID: 3285
		private static readonly ThreadSafeStore<Type, Type> UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));

		// Token: 0x020002F6 RID: 758
		[Nullable(0)]
		internal class Union
		{
			// Token: 0x060017D0 RID: 6096 RVA: 0x00017440 File Offset: 0x00015640
			public Union(FSharpFunction tagReader, List<DiscriminatedUnionConverter.UnionCase> cases)
			{
				this.TagReader = tagReader;
				this.Cases = cases;
			}

			// Token: 0x04000CD6 RID: 3286
			public readonly FSharpFunction TagReader;

			// Token: 0x04000CD7 RID: 3287
			public readonly List<DiscriminatedUnionConverter.UnionCase> Cases;
		}

		// Token: 0x020002F7 RID: 759
		[Nullable(0)]
		internal class UnionCase
		{
			// Token: 0x060017D1 RID: 6097 RVA: 0x00017456 File Offset: 0x00015656
			public UnionCase(int tag, string name, PropertyInfo[] fields, FSharpFunction fieldReader, FSharpFunction constructor)
			{
				this.Tag = tag;
				this.Name = name;
				this.Fields = fields;
				this.FieldReader = fieldReader;
				this.Constructor = constructor;
			}

			// Token: 0x04000CD8 RID: 3288
			public readonly int Tag;

			// Token: 0x04000CD9 RID: 3289
			public readonly string Name;

			// Token: 0x04000CDA RID: 3290
			public readonly PropertyInfo[] Fields;

			// Token: 0x04000CDB RID: 3291
			public readonly FSharpFunction FieldReader;

			// Token: 0x04000CDC RID: 3292
			public readonly FSharpFunction Constructor;
		}
	}
}
