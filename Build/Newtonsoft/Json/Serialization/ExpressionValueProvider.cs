using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025D RID: 605
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpressionValueProvider : IValueProvider
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x00012BD8 File Offset: 0x00010DD8
		public ExpressionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x000608F8 File Offset: 0x0005EAF8
		public void SetValue(object target, [Nullable(2)] object value)
		{
			try
			{
				if (this._setter == null)
				{
					this._setter = ExpressionReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
				}
				this._setter(target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), innerException);
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0006096C File Offset: 0x0005EB6C
		[return: Nullable(2)]
		public object GetValue(object target)
		{
			object result;
			try
			{
				if (this._getter == null)
				{
					this._getter = ExpressionReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
				}
				result = this._getter(target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), innerException);
			}
			return result;
		}

		// Token: 0x04000A59 RID: 2649
		private readonly MemberInfo _memberInfo;

		// Token: 0x04000A5A RID: 2650
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private Func<object, object> _getter;

		// Token: 0x04000A5B RID: 2651
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private Action<object, object> _setter;
	}
}
