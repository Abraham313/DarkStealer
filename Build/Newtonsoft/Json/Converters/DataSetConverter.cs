using System;
using System.Data;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002F2 RID: 754
	[NullableContext(1)]
	[Nullable(0)]
	public class DataSetConverter : JsonConverter
	{
		// Token: 0x060017BD RID: 6077 RVA: 0x0007329C File Offset: 0x0007149C
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DataSet dataSet = (DataSet)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			DataTableConverter dataTableConverter = new DataTableConverter();
			writer.WriteStartObject();
			foreach (object obj in dataSet.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(dataTable.TableName) : dataTable.TableName);
				dataTableConverter.WriteJson(writer, dataTable, serializer);
			}
			writer.WriteEndObject();
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00073348 File Offset: 0x00071548
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DataSet dataSet = (objectType == typeof(DataSet)) ? new DataSet() : ((DataSet)Activator.CreateInstance(objectType));
			DataTableConverter dataTableConverter = new DataTableConverter();
			reader.ReadAndAssert();
			while (reader.TokenType == JsonToken.PropertyName)
			{
				DataTable dataTable = dataSet.Tables[(string)reader.Value];
				bool flag = dataTable != null;
				dataTable = (DataTable)dataTableConverter.ReadJson(reader, typeof(DataTable), dataTable, serializer);
				if (!flag)
				{
					dataSet.Tables.Add(dataTable);
				}
				reader.ReadAndAssert();
			}
			return dataSet;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000173EE File Offset: 0x000155EE
		public override bool CanConvert(Type valueType)
		{
			return typeof(DataSet).IsAssignableFrom(valueType);
		}
	}
}
