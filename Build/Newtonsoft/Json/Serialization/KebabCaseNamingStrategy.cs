using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000284 RID: 644
	public class KebabCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060012D0 RID: 4816 RVA: 0x00012720 File Offset: 0x00010920
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00013C6C File Offset: 0x00011E6C
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames) : this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00012747 File Offset: 0x00010947
		public KebabCaseNamingStrategy()
		{
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00013C7D File Offset: 0x00011E7D
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToKebabCase(name);
		}
	}
}
