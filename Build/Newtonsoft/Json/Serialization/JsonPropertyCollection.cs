using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000276 RID: 630
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1
	})]
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		// Token: 0x06001202 RID: 4610 RVA: 0x000134EB File Offset: 0x000116EB
		public JsonPropertyCollection(Type type) : base(StringComparer.Ordinal)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			this._type = type;
			this._list = (List<JsonProperty>)base.Items;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0001351B File Offset: 0x0001171B
		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00061908 File Offset: 0x0005FB08
		public void AddProperty(JsonProperty property)
		{
			if (base.Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base[property.PropertyName];
				bool flag = true;
				if (jsonProperty.Ignored)
				{
					base.Remove(jsonProperty);
					flag = false;
				}
				else if (property.DeclaringType != null && jsonProperty.DeclaringType != null)
				{
					if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType) || (jsonProperty.DeclaringType.IsInterface() && property.DeclaringType.ImplementInterface(jsonProperty.DeclaringType)))
					{
						base.Remove(jsonProperty);
						flag = false;
					}
					if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType) || (property.DeclaringType.IsInterface() && jsonProperty.DeclaringType.ImplementInterface(property.DeclaringType)))
					{
						return;
					}
					if (this._type.ImplementInterface(jsonProperty.DeclaringType) && this._type.ImplementInterface(property.DeclaringType))
					{
						return;
					}
				}
				if (flag)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, this._type));
				}
			}
			base.Add(property);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00061A3C File Offset: 0x0005FC3C
		[return: Nullable(2)]
		public JsonProperty GetClosestMatchProperty(string propertyName)
		{
			JsonProperty property = this.GetProperty(propertyName, StringComparison.Ordinal);
			if (property == null)
			{
				property = this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
			}
			return property;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00013523 File Offset: 0x00011723
		private bool TryGetValue(string key, [Nullable(2)] [NotNullWhen(true)] out JsonProperty item)
		{
			if (base.Dictionary == null)
			{
				item = null;
				return false;
			}
			return base.Dictionary.TryGetValue(key, out item);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00061A60 File Offset: 0x0005FC60
		[return: Nullable(2)]
		public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
		{
			if (comparisonType != StringComparison.Ordinal)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					JsonProperty jsonProperty = this._list[i];
					if (string.Equals(propertyName, jsonProperty.PropertyName, comparisonType))
					{
						return jsonProperty;
					}
				}
				return null;
			}
			JsonProperty result;
			if (this.TryGetValue(propertyName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04000AD5 RID: 2773
		private readonly Type _type;

		// Token: 0x04000AD6 RID: 2774
		private readonly List<JsonProperty> _list;
	}
}
