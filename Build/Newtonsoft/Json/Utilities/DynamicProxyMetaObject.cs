using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001F9 RID: 505
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicProxyMetaObject<[Nullable(2)] T> : DynamicMetaObject
	{
		// Token: 0x06000EF0 RID: 3824 RVA: 0x00011751 File Offset: 0x0000F951
		internal DynamicProxyMetaObject(Expression expression, T value, DynamicProxy<T> proxy) : base(expression, BindingRestrictions.Empty, value)
		{
			this._proxy = proxy;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0001176C File Offset: 0x0000F96C
		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(this._proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00059648 File Offset: 0x00057848
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!this.IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return this.CallMethodWithResult("TryGetMember", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackGetMember(this, e), null);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000596A8 File Offset: 0x000578A8
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return this.CallMethodReturnLast("TrySetMember", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[]
			{
				value
			}), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00059724 File Offset: 0x00057924
		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!this.IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return this.CallMethodNoResult("TryDeleteMember", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00059784 File Offset: 0x00057984
		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!this.IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return this.CallMethodWithResult("TryConvert", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackConvert(this, e), null);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000597E4 File Offset: 0x000579E4
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			DynamicProxyMetaObject<T>.Fallback fallback = ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			return this.BuildCallMethodWithResult("TryInvokeMember", binder, DynamicProxyMetaObject<T>.GetArgArray(args), this.BuildCallMethodWithResult("TryGetMember", new DynamicProxyMetaObject<T>.GetBinderAdapter(binder), DynamicProxyMetaObject<T>.NoArgs, fallback(null), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00059884 File Offset: 0x00057A84
		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return this.CallMethodWithResult("TryCreateInstance", binder, DynamicProxyMetaObject<T>.GetArgArray(args), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e), null);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x000598F8 File Offset: 0x00057AF8
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return this.CallMethodWithResult("TryInvoke", binder, DynamicProxyMetaObject<T>.GetArgArray(args), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvoke(this, args, e), null);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0005996C File Offset: 0x00057B6C
		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!this.IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return this.CallMethodWithResult("TryBinaryOperation", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[]
			{
				arg
			}), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e), null);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x000599E8 File Offset: 0x00057BE8
		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!this.IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return this.CallMethodWithResult("TryUnaryOperation", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e), null);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00059A48 File Offset: 0x00057C48
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return this.CallMethodWithResult("TryGetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e), null);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00059ABC File Offset: 0x00057CBC
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return this.CallMethodReturnLast("TrySetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes, value), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00059B40 File Offset: 0x00057D40
		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return this.CallMethodNoResult("TryDeleteIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x00011789 File Offset: 0x0000F989
		private static Expression[] NoArgs
		{
			get
			{
				return CollectionUtils.ArrayEmpty<Expression>();
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00011790 File Offset: 0x0000F990
		private static IEnumerable<Expression> GetArgs(params DynamicMetaObject[] args)
		{
			return args.Select(delegate(DynamicMetaObject arg)
			{
				Expression expression = arg.Expression;
				if (!expression.Type.IsValueType())
				{
					return expression;
				}
				return Expression.Convert(expression, typeof(object));
			});
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00059BB0 File Offset: 0x00057DB0
		private static Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[]
			{
				Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args))
			};
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00059BE0 File Offset: 0x00057DE0
		private static Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			Expression expression = value.Expression;
			return new Expression[]
			{
				Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)),
				expression.Type.IsValueType() ? Expression.Convert(expression, typeof(object)) : expression
			};
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00059C38 File Offset: 0x00057E38
		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return Expression.Constant(binder, type);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00059C64 File Offset: 0x00057E64
		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, [Nullable(new byte[]
		{
			1,
			0
		})] DynamicProxyMetaObject<T>.Fallback fallback, [Nullable(new byte[]
		{
			2,
			0
		})] DynamicProxyMetaObject<T>.Fallback fallbackInvoke = null)
		{
			DynamicMetaObject fallbackResult = fallback(null);
			return this.BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00059C88 File Offset: 0x00057E88
		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, DynamicMetaObject fallbackResult, [Nullable(new byte[]
		{
			2,
			0
		})] DynamicProxyMetaObject<T>.Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			list.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				dynamicMetaObject = new DynamicMetaObject(Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType), dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType)
			}), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00059DA4 File Offset: 0x00057FA4
		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, [Nullable(new byte[]
		{
			1,
			0
		})] DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			list[list.Count - 1] = Expression.Assign(parameterExpression, list[list.Count - 1]);
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), parameterExpression, dynamicMetaObject.Expression, typeof(object))
			}), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00059E88 File Offset: 0x00058088
		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, [Nullable(new byte[]
		{
			1,
			0
		})] DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			return new DynamicMetaObject(Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000117B7 File Offset: 0x0000F9B7
		private BindingRestrictions GetRestrictions()
		{
			if (base.Value == null && base.HasValue)
			{
				return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
			}
			return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x000117E7 File Offset: 0x0000F9E7
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._proxy.GetDynamicMemberNames((T)((object)base.Value));
		}

		// Token: 0x0400096D RID: 2413
		private readonly DynamicProxy<T> _proxy;

		// Token: 0x020001FA RID: 506
		// (Invoke) Token: 0x06000F0A RID: 3850
		[NullableContext(0)]
		private delegate DynamicMetaObject Fallback([Nullable(2)] DynamicMetaObject errorSuggestion);

		// Token: 0x020001FB RID: 507
		[Nullable(0)]
		private sealed class GetBinderAdapter : GetMemberBinder
		{
			// Token: 0x06000F0D RID: 3853 RVA: 0x000117FF File Offset: 0x0000F9FF
			internal GetBinderAdapter(InvokeMemberBinder binder) : base(binder.Name, binder.IgnoreCase)
			{
			}

			// Token: 0x06000F0E RID: 3854 RVA: 0x0000983A File Offset: 0x00007A3A
			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}
	}
}
