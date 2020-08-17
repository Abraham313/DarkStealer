using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000248 RID: 584
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x0600108E RID: 4238 RVA: 0x00012720 File Offset: 0x00010920
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00012736 File Offset: 0x00010936
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames) : this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00012747 File Offset: 0x00010947
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0001274F File Offset: 0x0001094F
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
