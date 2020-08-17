using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E1 RID: 737
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class PathFilter
	{
		// Token: 0x06001769 RID: 5993
		public abstract IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch);

		// Token: 0x0600176A RID: 5994 RVA: 0x000721E0 File Offset: 0x000703E0
		[return: Nullable(2)]
		protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
		{
			JArray jarray = t as JArray;
			if (jarray != null)
			{
				if (jarray.Count > index)
				{
					return jarray[index];
				}
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
				}
				return null;
			}
			else
			{
				JConstructor jconstructor = t as JConstructor;
				if (jconstructor != null)
				{
					if (jconstructor.Count > index)
					{
						return jconstructor[index];
					}
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith(CultureInfo.InvariantCulture, index));
					}
					return null;
				}
				else
				{
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, t.GetType().Name));
					}
					return null;
				}
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00072294 File Offset: 0x00070494
		[NullableContext(2)]
		protected static JToken GetNextScanValue([Nullable(1)] JToken originalParent, JToken container, JToken value)
		{
			if (container == null || !container.HasValues)
			{
				while (value != null && value != originalParent)
				{
					if (value != value.Parent.Last)
					{
						break;
					}
					value = value.Parent;
				}
				if (value != null)
				{
					if (value != originalParent)
					{
						return value.Next;
					}
				}
				return null;
			}
			value = container.First;
			return value;
		}
	}
}
