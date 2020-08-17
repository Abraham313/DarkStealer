using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000218 RID: 536
	[NullableContext(1)]
	[Nullable(0)]
	internal class FSharpUtils
	{
		// Token: 0x06000F6F RID: 3951 RVA: 0x0005B8B0 File Offset: 0x00059AB0
		private FSharpUtils(Assembly fsharpCoreAssembly)
		{
			this.FSharpCoreAssembly = fsharpCoreAssembly;
			Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
			this.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodInfo methodWithNonPublicFallback2 = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
			this.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback2);
			Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
			this.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionTagReader");
			this.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionReader");
			this.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionConstructor");
			Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
			this.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
			this.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
			this.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
			this.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
			Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
			this._ofSeq = type4.GetMethod("OfSeq");
			this._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x00011BA7 File Offset: 0x0000FDA7
		public static FSharpUtils Instance
		{
			get
			{
				return FSharpUtils._instance;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x00011BAE File Offset: 0x0000FDAE
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x00011BB6 File Offset: 0x0000FDB6
		public Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x00011BBF File Offset: 0x0000FDBF
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x00011BC7 File Offset: 0x0000FDC7
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		public MethodCall<object, object> IsUnion { [return: Nullable(new byte[]
		{
			1,
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			1,
			2,
			1
		})] private set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		public MethodCall<object, object> GetUnionCases { [return: Nullable(new byte[]
		{
			1,
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			1,
			2,
			1
		})] private set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00011BE1 File Offset: 0x0000FDE1
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x00011BE9 File Offset: 0x0000FDE9
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		public MethodCall<object, object> PreComputeUnionTagReader { [return: Nullable(new byte[]
		{
			1,
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			1,
			2,
			1
		})] private set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00011BF2 File Offset: 0x0000FDF2
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x00011BFA File Offset: 0x0000FDFA
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		public MethodCall<object, object> PreComputeUnionReader { [return: Nullable(new byte[]
		{
			1,
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			1,
			2,
			1
		})] private set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00011C03 File Offset: 0x0000FE03
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x00011C0B File Offset: 0x0000FE0B
		[Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		public MethodCall<object, object> PreComputeUnionConstructor { [return: Nullable(new byte[]
		{
			1,
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			1,
			2,
			1
		})] private set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00011C14 File Offset: 0x0000FE14
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x00011C1C File Offset: 0x0000FE1C
		public Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x00011C25 File Offset: 0x0000FE25
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x00011C2D File Offset: 0x0000FE2D
		public Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x00011C36 File Offset: 0x0000FE36
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x00011C3E File Offset: 0x0000FE3E
		public Func<object, object> GetUnionCaseInfoTag { get; private set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x00011C47 File Offset: 0x0000FE47
		// (set) Token: 0x06000F84 RID: 3972 RVA: 0x00011C4F File Offset: 0x0000FE4F
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		public MethodCall<object, object> GetUnionCaseInfoFields { [return: Nullable(new byte[]
		{
			1,
			1,
			2
		})] get; [param: Nullable(new byte[]
		{
			1,
			1,
			2
		})] private set; }

		// Token: 0x06000F85 RID: 3973 RVA: 0x0005B9FC File Offset: 0x00059BFC
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (FSharpUtils._instance == null)
			{
				object @lock = FSharpUtils.Lock;
				lock (@lock)
				{
					if (FSharpUtils._instance == null)
					{
						FSharpUtils._instance = new FSharpUtils(fsharpCoreAssembly);
					}
				}
			}
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0005BA50 File Offset: 0x00059C50
		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo method = type.GetMethod(methodName, bindingFlags);
			if (method == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				method = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return method;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0005BA84 File Offset: 0x00059C84
		[return: Nullable(new byte[]
		{
			1,
			2,
			1
		})]
		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return ([Nullable(2)] object target, [Nullable(new byte[]
			{
				1,
				2
			})] object[] args) => new FSharpFunction(call(target, args), invoke);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0005BAE0 File Offset: 0x00059CE0
		public ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo method = this._ofSeq.MakeGenericMethod(new Type[]
			{
				t
			});
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(method);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00011C58 File Offset: 0x0000FE58
		public ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[]
			{
				keyType,
				valueType
			}).Invoke(this, null);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005BB10 File Offset: 0x00059D10
		[NullableContext(2)]
		[return: Nullable(1)]
		public ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = this._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			}).GetConstructor(new Type[]
			{
				typeof(IEnumerable<Tuple<TKey, TValue>>)
			});
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return delegate([Nullable(new byte[]
			{
				1,
				2
			})] object[] args)
			{
				IEnumerable<Tuple<TKey, TValue>> enumerable = from kv in (IEnumerable<KeyValuePair<TKey, TValue>>)args[0]
				select new Tuple<TKey, TValue>(kv.Key, kv.Value);
				return ctorDelegate(new object[]
				{
					enumerable
				});
			};
		}

		// Token: 0x040009B3 RID: 2483
		private static readonly object Lock = new object();

		// Token: 0x040009B4 RID: 2484
		[Nullable(2)]
		private static FSharpUtils _instance;

		// Token: 0x040009B5 RID: 2485
		private MethodInfo _ofSeq;

		// Token: 0x040009B6 RID: 2486
		private Type _mapType;

		// Token: 0x040009C1 RID: 2497
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x040009C2 RID: 2498
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x040009C3 RID: 2499
		public const string FSharpMapTypeName = "FSharpMap`2";
	}
}
