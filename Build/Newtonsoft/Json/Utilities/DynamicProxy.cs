using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001F8 RID: 504
	[NullableContext(1)]
	[Nullable(0)]
	internal class DynamicProxy<[Nullable(2)] T>
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x0001173D File Offset: 0x0000F93D
		public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
		{
			return CollectionUtils.ArrayEmpty<string>();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool TryBinaryOperation(T instance, BinaryOperationBinder binder, object arg, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0001174B File Offset: 0x0000F94B
		public virtual bool TryConvert(T instance, ConvertBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool TryCreateInstance(T instance, CreateInstanceBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00009021 File Offset: 0x00007221
		public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
		{
			return false;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00009021 File Offset: 0x00007221
		public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder)
		{
			return false;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool TryGetIndex(T instance, GetIndexBinder binder, object[] indexes, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0001174B File Offset: 0x0000F94B
		public virtual bool TryGetMember(T instance, GetMemberBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool TryInvokeMember(T instance, InvokeMemberBinder binder, object[] args, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00009021 File Offset: 0x00007221
		public virtual bool TrySetIndex(T instance, SetIndexBinder binder, object[] indexes, object value)
		{
			return false;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00009021 File Offset: 0x00007221
		public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value)
		{
			return false;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0001174B File Offset: 0x0000F94B
		public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, [Nullable(2)] out object result)
		{
			result = null;
			return false;
		}
	}
}
