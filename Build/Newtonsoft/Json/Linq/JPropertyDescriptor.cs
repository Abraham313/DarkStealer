using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C0 RID: 704
	[NullableContext(1)]
	[Nullable(0)]
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x0600159F RID: 5535 RVA: 0x00015EE6 File Offset: 0x000140E6
		public JPropertyDescriptor(string name) : base(name, null)
		{
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00015EF0 File Offset: 0x000140F0
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00015EF8 File Offset: 0x000140F8
		[return: Nullable(2)]
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00009B58 File Offset: 0x00007D58
		public override void ResetValue(object component)
		{
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0006C4DC File Offset: 0x0006A6DC
		public override void SetValue(object component, object value)
		{
			JObject jobject = component as JObject;
			if (jobject != null)
			{
				JToken value2 = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = value2;
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00009021 File Offset: 0x00007221
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00015F11 File Offset: 0x00014111
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00009021 File Offset: 0x00007221
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00015F1D File Offset: 0x0001411D
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00015F29 File Offset: 0x00014129
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
