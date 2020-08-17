using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200026F RID: 623
	[NullableContext(1)]
	[Nullable(0)]
	public class JsonDynamicContract : JsonContainerContract
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00012F79 File Offset: 0x00011179
		public JsonPropertyCollection Properties { get; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00012F81 File Offset: 0x00011181
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x00012F89 File Offset: 0x00011189
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Func<string, string> PropertyNameResolver { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x06001188 RID: 4488 RVA: 0x00012F92 File Offset: 0x00011192
		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember((GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils))));
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00012FB3 File Offset: 0x000111B3
		[return: Nullable(new byte[]
		{
			1,
			1,
			1,
			1,
			2,
			1
		})]
		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember((SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils))));
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00061538 File Offset: 0x0005F738
		public JsonDynamicContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Dynamic;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00061594 File Offset: 0x0005F794
		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, [Nullable(2)] out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = this._callSiteGetters.Get(name);
			object obj = callSite.Target(callSite, dynamicProvider);
			if (obj != NoThrowExpressionVisitor.ErrorResult)
			{
				value = obj;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000615D8 File Offset: 0x0005F7D8
		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, [Nullable(2)] object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = this._callSiteSetters.Get(name);
			return callSite.Target(callSite, dynamicProvider, value) != NoThrowExpressionVisitor.ErrorResult;
		}

		// Token: 0x04000A9F RID: 2719
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object>>>(JsonDynamicContract.CreateCallSiteGetter));

		// Token: 0x04000AA0 RID: 2720
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			1,
			1,
			1,
			2,
			1
		})]
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object, object>>>(JsonDynamicContract.CreateCallSiteSetter));
	}
}
