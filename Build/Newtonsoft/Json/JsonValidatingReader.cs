using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001D2 RID: 466
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000D69 RID: 3433 RVA: 0x00054180 File Offset: 0x00052380
		// (remove) Token: 0x06000D6A RID: 3434 RVA: 0x000541B8 File Offset: 0x000523B8
		public event ValidationEventHandler ValidationEventHandler;

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x000102B8 File Offset: 0x0000E4B8
		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x000102C5 File Offset: 0x0000E4C5
		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x000102D2 File Offset: 0x0000E4D2
		public override string Path
		{
			get
			{
				return this._reader.Path;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x000102DF File Offset: 0x0000E4DF
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00009B58 File Offset: 0x00007D58
		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x000102EC File Offset: 0x0000E4EC
		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x000102F9 File Offset: 0x0000E4F9
		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00010306 File Offset: 0x0000E506
		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0001031B File Offset: 0x0000E51B
		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope result = this._stack.Pop();
			this._currentScope = ((this._stack.Count != 0) ? this._stack.Peek() : null);
			return result;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00010349 File Offset: 0x0000E549
		private IList<JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return this._currentScope.Schemas;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x000541F0 File Offset: 0x000523F0
		private IList<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (this._currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[]
					{
						this._model
					});
				}
				if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
				{
					return JsonValidatingReader.EmptySchemaList;
				}
				switch (this._currentScope.TokenType)
				{
				case JTokenType.None:
					return this._currentScope.Schemas;
				case JTokenType.Object:
				{
					if (this._currentScope.CurrentPropertyName == null)
					{
						throw new JsonReaderException("CurrentPropertyName has not been set on scope.");
					}
					IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
					{
						JsonSchemaModel item;
						if (jsonSchemaModel.Properties != null && jsonSchemaModel.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out item))
						{
							list.Add(item);
						}
						if (jsonSchemaModel.PatternProperties != null)
						{
							foreach (KeyValuePair<string, JsonSchemaModel> keyValuePair in jsonSchemaModel.PatternProperties)
							{
								if (Regex.IsMatch(this._currentScope.CurrentPropertyName, keyValuePair.Key))
								{
									list.Add(keyValuePair.Value);
								}
							}
						}
						if (list.Count == 0 && jsonSchemaModel.AllowAdditionalProperties && jsonSchemaModel.AdditionalProperties != null)
						{
							list.Add(jsonSchemaModel.AdditionalProperties);
						}
					}
					return list;
				}
				case JTokenType.Array:
				{
					IList<JsonSchemaModel> list2 = new List<JsonSchemaModel>();
					foreach (JsonSchemaModel jsonSchemaModel2 in this.CurrentSchemas)
					{
						if (!jsonSchemaModel2.PositionalItemsValidation)
						{
							if (jsonSchemaModel2.Items != null && jsonSchemaModel2.Items.Count > 0)
							{
								list2.Add(jsonSchemaModel2.Items[0]);
							}
						}
						else
						{
							if (jsonSchemaModel2.Items != null && jsonSchemaModel2.Items.Count > 0 && jsonSchemaModel2.Items.Count > this._currentScope.ArrayItemCount - 1)
							{
								list2.Add(jsonSchemaModel2.Items[this._currentScope.ArrayItemCount - 1]);
							}
							if (jsonSchemaModel2.AllowAdditionalItems && jsonSchemaModel2.AdditionalItems != null)
							{
								list2.Add(jsonSchemaModel2.AdditionalItems);
							}
						}
					}
					return list2;
				}
				case JTokenType.Constructor:
					return JsonValidatingReader.EmptySchemaList;
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, this._currentScope.TokenType));
				}
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000544B4 File Offset: 0x000526B4
		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string message2 = ((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition)) : message;
			this.OnValidationEvent(new JsonSchemaException(message2, null, this.Path, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0005451C File Offset: 0x0005271C
		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler == null)
			{
				throw exception;
			}
			validationEventHandler(this, new ValidationEventArgs(exception));
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00010356 File Offset: 0x0000E556
		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0001037B File Offset: 0x0000E57B
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00010383 File Offset: 0x0000E583
		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new InvalidOperationException("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x000103A6 File Offset: 0x0000E5A6
		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000103AE File Offset: 0x0000E5AE
		public override void Close()
		{
			base.Close();
			if (base.CloseInput)
			{
				JsonReader reader = this._reader;
				if (reader == null)
				{
					return;
				}
				reader.Close();
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00054544 File Offset: 0x00052744
		private void ValidateNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType != null && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.GetValueOrDefault()))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, currentNodeSchemaType), schema);
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0005459C File Offset: 0x0005279C
		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				return new JsonSchemaType?(JsonSchemaType.Object);
			case JsonToken.StartArray:
				return new JsonSchemaType?(JsonSchemaType.Array);
			case JsonToken.Integer:
				return new JsonSchemaType?(JsonSchemaType.Integer);
			case JsonToken.Float:
				return new JsonSchemaType?(JsonSchemaType.Float);
			case JsonToken.String:
				return new JsonSchemaType?(JsonSchemaType.String);
			case JsonToken.Boolean:
				return new JsonSchemaType?(JsonSchemaType.Boolean);
			case JsonToken.Null:
				return new JsonSchemaType?(JsonSchemaType.Null);
			}
			return null;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000103CE File Offset: 0x0000E5CE
		public override int? ReadAsInt32()
		{
			int? result = this._reader.ReadAsInt32();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000103E1 File Offset: 0x0000E5E1
		public override byte[] ReadAsBytes()
		{
			byte[] result = this._reader.ReadAsBytes();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._reader.ReadAsDecimal();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00010407 File Offset: 0x0000E607
		public override double? ReadAsDouble()
		{
			double? result = this._reader.ReadAsDouble();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0001041A File Offset: 0x0000E61A
		public override bool? ReadAsBoolean()
		{
			bool? result = this._reader.ReadAsBoolean();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0001042D File Offset: 0x0000E62D
		public override string ReadAsString()
		{
			string result = this._reader.ReadAsString();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00010440 File Offset: 0x0000E640
		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = this._reader.ReadAsDateTime();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00010453 File Offset: 0x0000E653
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._reader.ReadAsDateTimeOffset();
			this.ValidateCurrentToken();
			return result;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00010466 File Offset: 0x0000E666
		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			this.ValidateCurrentToken();
			return true;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00054628 File Offset: 0x00052828
		private void ValidateCurrentToken()
		{
			if (this._model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				this._model = jsonSchemaModelBuilder.Build(this._schema);
				if (!JsonTokenUtils.IsStartToken(this._reader.TokenType))
				{
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.None, this.CurrentMemberSchemas));
				}
			}
			switch (this._reader.TokenType)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateObject)).ToList<JsonSchemaModel>();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, schemas));
				this.WriteToken(this.CurrentSchemas);
				return;
			}
			case JsonToken.StartArray:
			{
				this.ProcessValue();
				IList<JsonSchemaModel> schemas2 = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateArray)).ToList<JsonSchemaModel>();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, schemas2));
				this.WriteToken(this.CurrentSchemas);
				return;
			}
			case JsonToken.StartConstructor:
				this.ProcessValue();
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
				this.WriteToken(this.CurrentSchemas);
				return;
			case JsonToken.PropertyName:
				this.WriteToken(this.CurrentSchemas);
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel schema = enumerator.Current;
						this.ValidatePropertyName(schema);
					}
					return;
				}
				break;
			case JsonToken.Raw:
				this.ProcessValue();
				return;
			case JsonToken.Integer:
				this.ProcessValue();
				this.WriteToken(this.CurrentMemberSchemas);
				using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonSchemaModel schema2 = enumerator.Current;
						this.ValidateInteger(schema2);
					}
					return;
				}
				goto IL_1DE;
			case JsonToken.Float:
				goto IL_1DE;
			case JsonToken.String:
				goto IL_22B;
			case JsonToken.Boolean:
				goto IL_278;
			case JsonToken.Null:
				goto IL_2C2;
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.WriteToken(this.CurrentMemberSchemas);
				return;
			case JsonToken.EndObject:
				this.WriteToken(this.CurrentSchemas);
				foreach (JsonSchemaModel schema3 in this.CurrentSchemas)
				{
					this.ValidateEndObject(schema3);
				}
				this.Pop();
				return;
			case JsonToken.EndArray:
				this.WriteToken(this.CurrentSchemas);
				foreach (JsonSchemaModel schema4 in this.CurrentSchemas)
				{
					this.ValidateEndArray(schema4);
				}
				this.Pop();
				return;
			case JsonToken.EndConstructor:
				this.WriteToken(this.CurrentSchemas);
				this.Pop();
				return;
			}
			throw new ArgumentOutOfRangeException();
			IL_1DE:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel schema5 = enumerator.Current;
					this.ValidateFloat(schema5);
				}
				return;
			}
			IL_22B:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel schema6 = enumerator.Current;
					this.ValidateString(schema6);
				}
				return;
			}
			IL_278:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentMemberSchemas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonSchemaModel schema7 = enumerator.Current;
					this.ValidateBoolean(schema7);
				}
				return;
			}
			IL_2C2:
			this.ProcessValue();
			this.WriteToken(this.CurrentMemberSchemas);
			foreach (JsonSchemaModel schema8 in this.CurrentMemberSchemas)
			{
				this.ValidateNull(schema8);
			}
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00054A60 File Offset: 0x00052C60
		private void WriteToken(IList<JsonSchemaModel> schemas)
		{
			foreach (JsonValidatingReader.SchemaScope schemaScope in this._stack)
			{
				bool flag;
				if (!(flag = (schemaScope.TokenType == JTokenType.Array && schemaScope.IsUniqueArray && schemaScope.ArrayItemCount > 0)))
				{
					if (!schemas.Any((JsonSchemaModel s) => s.Enum != null))
					{
						continue;
					}
				}
				if (schemaScope.CurrentItemWriter == null)
				{
					if (JsonTokenUtils.IsEndToken(this._reader.TokenType))
					{
						continue;
					}
					schemaScope.CurrentItemWriter = new JTokenWriter();
				}
				schemaScope.CurrentItemWriter.WriteToken(this._reader, false);
				if (schemaScope.CurrentItemWriter.Top == 0 && this._reader.TokenType != JsonToken.PropertyName)
				{
					JToken token = schemaScope.CurrentItemWriter.Token;
					schemaScope.CurrentItemWriter = null;
					if (flag)
					{
						if (schemaScope.UniqueArrayItems.Contains(token, JToken.EqualityComparer))
						{
							this.RaiseError("Non-unique array item at index {0}.".FormatWith(CultureInfo.InvariantCulture, schemaScope.ArrayItemCount - 1), schemaScope.Schemas.First((JsonSchemaModel s) => s.UniqueItems));
						}
						schemaScope.UniqueArrayItems.Add(token);
					}
					else if (schemas.Any((JsonSchemaModel s) => s.Enum != null))
					{
						foreach (JsonSchemaModel jsonSchemaModel in schemas)
						{
							if (jsonSchemaModel.Enum != null && !jsonSchemaModel.Enum.ContainsValue(token, JToken.EqualityComparer))
							{
								StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
								token.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
								this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, stringWriter.ToString()), jsonSchemaModel);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00054CB4 File Offset: 0x00052EB4
		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				if (requiredProperties.Values.Any((bool v) => !v))
				{
					IEnumerable<string> values = from kv in requiredProperties
					where !kv.Value
					select kv.Key;
					this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", values)), schema);
				}
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00054D70 File Offset: 0x00052F70
		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			if (schema.MaximumItems != null)
			{
				int num = arrayItemCount;
				int? num2 = schema.MaximumItems;
				if (num > num2.GetValueOrDefault() & num2 != null)
				{
					this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
				}
			}
			if (schema.MinimumItems != null)
			{
				int num3 = arrayItemCount;
				int? num2 = schema.MinimumItems;
				if (num3 < num2.GetValueOrDefault() & num2 != null)
				{
					this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
				}
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0001048E File Offset: 0x0000E68E
		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000104A7 File Offset: 0x0000E6A7
		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00054E34 File Offset: 0x00053034
		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			string text = this._reader.Value.ToString();
			if (schema.MaximumLength != null)
			{
				int length = text.Length;
				int? num = schema.MaximumLength;
				if (length > num.GetValueOrDefault() & num != null)
				{
					this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MaximumLength), schema);
				}
			}
			if (schema.MinimumLength != null)
			{
				int length2 = text.Length;
				int? num = schema.MinimumLength;
				if (length2 < num.GetValueOrDefault() & num != null)
				{
					this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MinimumLength), schema);
				}
			}
			if (schema.Patterns != null)
			{
				foreach (string text2 in schema.Patterns)
				{
					if (!Regex.IsMatch(text, text2))
					{
						this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, text, text2), schema);
					}
				}
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00054F70 File Offset: 0x00053170
		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			object value = this._reader.Value;
			if (schema.Maximum != null)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Maximum) > 0)
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && JValue.Compare(JTokenType.Integer, value, schema.Maximum) == 0)
				{
					this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
			}
			if (schema.Minimum != null)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Minimum) < 0)
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && JValue.Compare(JTokenType.Integer, value, schema.Minimum) == 0)
				{
					this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy != null)
			{
				bool flag;
				if (value is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger bigInteger = (System.Numerics.BigInteger)value;
					if (!Math.Abs(schema.DivisibleBy.Value - Math.Truncate(schema.DivisibleBy.Value)).Equals(0.0))
					{
						flag = (bigInteger != 0L);
					}
					else
					{
						flag = (bigInteger % new System.Numerics.BigInteger(schema.DivisibleBy.Value) != 0L);
					}
				}
				else
				{
					flag = !JsonValidatingReader.IsZero((double)Convert.ToInt64(value, CultureInfo.InvariantCulture) % schema.DivisibleBy.GetValueOrDefault());
				}
				if (flag)
				{
					this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
				}
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0005519C File Offset: 0x0005339C
		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				JsonValidatingReader.SchemaScope currentScope = this._currentScope;
				int arrayItemCount = currentScope.ArrayItemCount;
				currentScope.ArrayItemCount = arrayItemCount + 1;
				foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
				{
					if (jsonSchemaModel != null && jsonSchemaModel.PositionalItemsValidation && !jsonSchemaModel.AllowAdditionalItems && (jsonSchemaModel.Items == null || this._currentScope.ArrayItemCount - 1 >= jsonSchemaModel.Items.Count))
					{
						this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, this._currentScope.ArrayItemCount), jsonSchemaModel);
					}
				}
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00055270 File Offset: 0x00053470
		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = num;
				double? num3 = schema.Maximum;
				if (num2 > num3.GetValueOrDefault() & num3 != null)
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					double num4 = num;
					num3 = schema.Maximum;
					if (num4 == num3.GetValueOrDefault() & num3 != null)
					{
						this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
					}
				}
			}
			if (schema.Minimum != null)
			{
				double num5 = num;
				double? num3 = schema.Minimum;
				if (num5 < num3.GetValueOrDefault() & num3 != null)
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					double num6 = num;
					num3 = schema.Minimum;
					if (num6 == num3.GetValueOrDefault() & num3 != null)
					{
						this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
					}
				}
			}
			if (schema.DivisibleBy != null && !JsonValidatingReader.IsZero(JsonValidatingReader.FloatingPointRemainder(num, schema.DivisibleBy.GetValueOrDefault())))
			{
				this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.DivisibleBy), schema);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000104BF File Offset: 0x0000E6BF
		private static double FloatingPointRemainder(double dividend, double divisor)
		{
			return dividend - Math.Floor(dividend / divisor) * divisor;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000104CD File Offset: 0x0000E6CD
		private static bool IsZero(double value)
		{
			return Math.Abs(value) < 4.4408920985006262E-15;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0005543C File Offset: 0x0005363C
		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			if (this._currentScope.RequiredProperties.ContainsKey(text))
			{
				this._currentScope.RequiredProperties[text] = true;
			}
			if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, text))
			{
				this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, text), schema);
			}
			this._currentScope.CurrentPropertyName = text;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000554C0 File Offset: 0x000536C0
		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				using (IEnumerator<string> enumerator = schema.PatternProperties.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string pattern = enumerator.Current;
						if (Regex.IsMatch(propertyName, pattern))
						{
							return true;
						}
					}
					return false;
				}
				bool result;
				return result;
			}
			return false;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000104E0 File Offset: 0x0000E6E0
		private bool ValidateArray(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Array);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000104F0 File Offset: 0x0000E6F0
		private bool ValidateObject(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Object);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00010500 File Offset: 0x0000E700
		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
				return false;
			}
			return true;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0005553C File Offset: 0x0005373C
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00055560 File Offset: 0x00053760
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00055584 File Offset: 0x00053784
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x0400088A RID: 2186
		private readonly JsonReader _reader;

		// Token: 0x0400088B RID: 2187
		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		// Token: 0x0400088C RID: 2188
		private JsonSchema _schema;

		// Token: 0x0400088D RID: 2189
		private JsonSchemaModel _model;

		// Token: 0x0400088E RID: 2190
		private JsonValidatingReader.SchemaScope _currentScope;

		// Token: 0x04000890 RID: 2192
		private static readonly IList<JsonSchemaModel> EmptySchemaList = new List<JsonSchemaModel>();

		// Token: 0x020001D3 RID: 467
		private class SchemaScope
		{
			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0001054B File Offset: 0x0000E74B
			// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00010553 File Offset: 0x0000E753
			public string CurrentPropertyName { get; set; }

			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0001055C File Offset: 0x0000E75C
			// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00010564 File Offset: 0x0000E764
			public int ArrayItemCount { get; set; }

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0001056D File Offset: 0x0000E76D
			public bool IsUniqueArray { get; }

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00010575 File Offset: 0x0000E775
			public IList<JToken> UniqueArrayItems { get; }

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0001057D File Offset: 0x0000E77D
			// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x00010585 File Offset: 0x0000E785
			public JTokenWriter CurrentItemWriter { get; set; }

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0001058E File Offset: 0x0000E78E
			public IList<JsonSchemaModel> Schemas
			{
				get
				{
					return this._schemas;
				}
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00010596 File Offset: 0x0000E796
			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0001059E File Offset: 0x0000E79E
			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			// Token: 0x06000DA8 RID: 3496 RVA: 0x000555A8 File Offset: 0x000537A8
			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				this._tokenType = tokenType;
				this._schemas = schemas;
				this._requiredProperties = schemas.SelectMany(new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties)).Distinct<string>().ToDictionary((string p) => p, (string p) => false);
				if (tokenType == JTokenType.Array)
				{
					if (schemas.Any((JsonSchemaModel s) => s.UniqueItems))
					{
						this.IsUniqueArray = 1;
						this.UniqueArrayItems = new List<JToken>();
					}
				}
			}

			// Token: 0x06000DA9 RID: 3497 RVA: 0x00055668 File Offset: 0x00053868
			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				if (((schema != null) ? schema.Properties : null) == null)
				{
					return Enumerable.Empty<string>();
				}
				return from p in schema.Properties
				where p.Value.Required
				select p.Key;
			}

			// Token: 0x04000891 RID: 2193
			private readonly JTokenType _tokenType;

			// Token: 0x04000892 RID: 2194
			private readonly IList<JsonSchemaModel> _schemas;

			// Token: 0x04000893 RID: 2195
			private readonly Dictionary<string, bool> _requiredProperties;
		}
	}
}
