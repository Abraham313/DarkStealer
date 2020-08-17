using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200021C RID: 540
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ILGeneratorExtensions
	{
		// Token: 0x06000F93 RID: 3987 RVA: 0x00011CD4 File Offset: 0x0000FED4
		public static void PushInstance(this ILGenerator generator, Type type)
		{
			generator.Emit(OpCodes.Ldarg_0);
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Unbox, type);
				return;
			}
			generator.Emit(OpCodes.Castclass, type);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00011D02 File Offset: 0x0000FF02
		public static void PushArrayInstance(this ILGenerator generator, int argsIndex, int arrayIndex)
		{
			generator.Emit(OpCodes.Ldarg, argsIndex);
			generator.Emit(OpCodes.Ldc_I4, arrayIndex);
			generator.Emit(OpCodes.Ldelem_Ref);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00011D27 File Offset: 0x0000FF27
		public static void BoxIfNeeded(this ILGenerator generator, Type type)
		{
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Box, type);
				return;
			}
			generator.Emit(OpCodes.Castclass, type);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00011D4A File Offset: 0x0000FF4A
		public static void UnboxIfNeeded(this ILGenerator generator, Type type)
		{
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Unbox_Any, type);
				return;
			}
			generator.Emit(OpCodes.Castclass, type);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00011D6D File Offset: 0x0000FF6D
		public static void CallMethod(this ILGenerator generator, MethodInfo methodInfo)
		{
			if (!methodInfo.IsFinal && methodInfo.IsVirtual)
			{
				generator.Emit(OpCodes.Callvirt, methodInfo);
				return;
			}
			generator.Emit(OpCodes.Call, methodInfo);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00011D98 File Offset: 0x0000FF98
		public static void Return(this ILGenerator generator)
		{
			generator.Emit(OpCodes.Ret);
		}
	}
}
