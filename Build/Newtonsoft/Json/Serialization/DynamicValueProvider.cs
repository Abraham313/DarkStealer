using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025A RID: 602
	[NullableContext(1)]
	[Nullable(0)]
	public class DynamicValueProvider : IValueProvider
	{
		// Token: 0x0600110C RID: 4364 RVA: 0x00012B31 File Offset: 0x00010D31
		public DynamicValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00060810 File Offset: 0x0005EA10
		public void SetValue(object target, [Nullable(2)] object value)
		{
			try
			{
				if (this._setter == null)
				{
					this._setter = DynamicReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
				}
				this._setter(target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), innerException);
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00060884 File Offset: 0x0005EA84
		[return: Nullable(2)]
		public object GetValue(object target)
		{
			object result;
			try
			{
				if (this._getter == null)
				{
					this._getter = DynamicReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
				}
				result = this._getter(target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), innerException);
			}
			return result;
		}

		// Token: 0x04000A4E RID: 2638
		private readonly MemberInfo _memberInfo;

		// Token: 0x04000A4F RID: 2639
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private Func<object, object> _getter;

		// Token: 0x04000A50 RID: 2640
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private Action<object, object> _setter;
	}
}
