using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200028C RID: 652
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060012F6 RID: 4854 RVA: 0x00012720 File Offset: 0x00010920
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00013DC0 File Offset: 0x00011FC0
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames) : this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00012747 File Offset: 0x00010947
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00013DD1 File Offset: 0x00011FD1
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
