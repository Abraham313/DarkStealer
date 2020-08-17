using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000214 RID: 532
	[NullableContext(1)]
	[Nullable(0)]
	internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x00011B3B File Offset: 0x0000FD3B
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return ExpressionReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0005B16C File Offset: 0x0005936C
		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression body = this.BuildMethodCall(method, typeFromHandle, null, parameterExpression);
			return (ObjectConstructor<object>)Expression.Lambda(typeof(ObjectConstructor<object>), body, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0005B1D4 File Offset: 0x000593D4
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "args");
			Expression body = this.BuildMethodCall(method, typeFromHandle, parameterExpression, parameterExpression2);
			return (MethodCall<T, object>)Expression.Lambda(typeof(MethodCall<T, object>), body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0005B24C File Offset: 0x0005944C
		private Expression BuildMethodCall(MethodBase method, Type type, [Nullable(2)] ParameterExpression targetParameterExpression, ParameterExpression argsParameterExpression)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Expression[] array;
			IList<ExpressionReflectionDelegateFactory.ByRefParameter> list;
			if (parameters.Length == 0)
			{
				array = CollectionUtils.ArrayEmpty<Expression>();
				list = CollectionUtils.ArrayEmpty<ExpressionReflectionDelegateFactory.ByRefParameter>();
			}
			else
			{
				array = new Expression[parameters.Length];
				list = new List<ExpressionReflectionDelegateFactory.ByRefParameter>();
				for (int i = 0; i < parameters.Length; i++)
				{
					ParameterInfo parameterInfo = parameters[i];
					Type type2 = parameterInfo.ParameterType;
					bool flag = false;
					if (type2.IsByRef)
					{
						type2 = type2.GetElementType();
						flag = true;
					}
					Expression index = Expression.Constant(i);
					Expression expression = Expression.ArrayIndex(argsParameterExpression, index);
					Expression expression2 = this.EnsureCastExpression(expression, type2, !flag);
					if (flag)
					{
						ParameterExpression parameterExpression = Expression.Variable(type2);
						list.Add(new ExpressionReflectionDelegateFactory.ByRefParameter(expression2, parameterExpression, parameterInfo.IsOut));
						expression2 = parameterExpression;
					}
					array[i] = expression2;
				}
			}
			Expression expression3;
			if (method.IsConstructor)
			{
				expression3 = Expression.New((ConstructorInfo)method, array);
			}
			else if (method.IsStatic)
			{
				expression3 = Expression.Call((MethodInfo)method, array);
			}
			else
			{
				expression3 = Expression.Call(this.EnsureCastExpression(targetParameterExpression, method.DeclaringType, false), (MethodInfo)method, array);
			}
			MethodInfo methodInfo = method as MethodInfo;
			if (methodInfo != null)
			{
				if (methodInfo.ReturnType != typeof(void))
				{
					expression3 = this.EnsureCastExpression(expression3, type, false);
				}
				else
				{
					expression3 = Expression.Block(expression3, Expression.Constant(null));
				}
			}
			else
			{
				expression3 = this.EnsureCastExpression(expression3, type, false);
			}
			if (list.Count > 0)
			{
				IList<ParameterExpression> list2 = new List<ParameterExpression>();
				IList<Expression> list3 = new List<Expression>();
				foreach (ExpressionReflectionDelegateFactory.ByRefParameter byRefParameter in list)
				{
					if (!byRefParameter.IsOut)
					{
						list3.Add(Expression.Assign(byRefParameter.Variable, byRefParameter.Value));
					}
					list2.Add(byRefParameter.Variable);
				}
				list3.Add(expression3);
				expression3 = Expression.Block(list2, list3);
			}
			return expression3;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0005B450 File Offset: 0x00059650
		public override Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsAbstract())
			{
				return () => (T)((object)Activator.CreateInstance(type));
			}
			Func<T> result;
			try
			{
				Type typeFromHandle = typeof(T);
				Expression expression = Expression.New(type);
				expression = this.EnsureCastExpression(expression, typeFromHandle, false);
				result = (Func<T>)Expression.Lambda(typeof(Func<T>), expression, new ParameterExpression[0]).Compile();
			}
			catch
			{
				result = (() => (T)((object)Activator.CreateInstance(type)));
			}
			return result;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0005B4FC File Offset: 0x000596FC
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property does not have a getter.");
			}
			Expression expression;
			if (getMethod.IsStatic)
			{
				expression = Expression.MakeMemberAccess(null, propertyInfo);
			}
			else
			{
				expression = Expression.MakeMemberAccess(this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType, false), propertyInfo);
			}
			expression = this.EnsureCastExpression(expression, typeFromHandle2, false);
			return (Func<T, object>)Expression.Lambda(typeof(Func<T, object>), expression, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0005B5A4 File Offset: 0x000597A4
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				expression = Expression.Field(this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType, false), fieldInfo);
			}
			expression = this.EnsureCastExpression(expression, typeof(object), false);
			return Expression.Lambda<Func<T, object>>(expression, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0005B620 File Offset: 0x00059820
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			if (!fieldInfo.DeclaringType.IsValueType() && !fieldInfo.IsInitOnly)
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "value");
				Expression expression;
				if (fieldInfo.IsStatic)
				{
					expression = Expression.Field(null, fieldInfo);
				}
				else
				{
					expression = Expression.Field(this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType, false), fieldInfo);
				}
				Expression right = this.EnsureCastExpression(parameterExpression2, expression.Type, false);
				BinaryExpression body = Expression.Assign(expression, right);
				return (Action<T, object>)Expression.Lambda(typeof(Action<T, object>), body, new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile();
			}
			return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0005B6F4 File Offset: 0x000598F4
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			if (propertyInfo.DeclaringType.IsValueType())
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
			}
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2, "value");
			Expression expression = this.EnsureCastExpression(parameterExpression2, propertyInfo.PropertyType, false);
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException("Property does not have a setter.");
			}
			Expression body;
			if (setMethod.IsStatic)
			{
				body = Expression.Call(setMethod, expression);
			}
			else
			{
				body = Expression.Call(this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType, false), setMethod, new Expression[]
				{
					expression
				});
			}
			return (Action<T, object>)Expression.Lambda(typeof(Action<T, object>), body, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0005B7E0 File Offset: 0x000599E0
		private Expression EnsureCastExpression(Expression expression, Type targetType, bool allowWidening = false)
		{
			Type type = expression.Type;
			if (type == targetType || (!type.IsValueType() && targetType.IsAssignableFrom(type)))
			{
				return expression;
			}
			if (targetType.IsValueType())
			{
				Expression expression2 = Expression.Unbox(expression, targetType);
				if (allowWidening && targetType.IsPrimitive())
				{
					MethodInfo method = typeof(Convert).GetMethod("To" + targetType.Name, new Type[]
					{
						typeof(object)
					});
					if (method != null)
					{
						expression2 = Expression.Condition(Expression.TypeIs(expression, targetType), expression2, Expression.Call(method, expression));
					}
				}
				return Expression.Condition(Expression.Equal(expression, Expression.Constant(null, typeof(object))), Expression.Default(targetType), expression2);
			}
			return Expression.Convert(expression, targetType);
		}

		// Token: 0x040009AC RID: 2476
		private static readonly ExpressionReflectionDelegateFactory _instance = new ExpressionReflectionDelegateFactory();

		// Token: 0x02000215 RID: 533
		[Nullable(0)]
		private class ByRefParameter
		{
			// Token: 0x06000F69 RID: 3945 RVA: 0x00011B4E File Offset: 0x0000FD4E
			public ByRefParameter(Expression value, ParameterExpression variable, bool isOut)
			{
				this.Value = value;
				this.Variable = variable;
				this.IsOut = isOut;
			}

			// Token: 0x040009AD RID: 2477
			public Expression Value;

			// Token: 0x040009AE RID: 2478
			public ParameterExpression Variable;

			// Token: 0x040009AF RID: 2479
			public bool IsOut;
		}
	}
}
