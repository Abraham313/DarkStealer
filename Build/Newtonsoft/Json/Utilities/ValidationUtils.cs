using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000246 RID: 582
	internal static class ValidationUtils
	{
		// Token: 0x0600108B RID: 4235 RVA: 0x000126EF File Offset: 0x000108EF
		[NullableContext(1)]
		public static void ArgumentNotNull([Nullable(2)] [NotNull] object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}
