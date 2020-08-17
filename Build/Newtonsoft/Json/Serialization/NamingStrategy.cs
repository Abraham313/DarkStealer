using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000286 RID: 646
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NamingStrategy
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x00013CC3 File Offset: 0x00011EC3
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x00013CCB File Offset: 0x00011ECB
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x00013CD4 File Offset: 0x00011ED4
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x00013CDC File Offset: 0x00011EDC
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x00013CE5 File Offset: 0x00011EE5
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x00013CED File Offset: 0x00011EED
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x060012E0 RID: 4832 RVA: 0x00013CF6 File Offset: 0x00011EF6
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00013D0C File Offset: 0x00011F0C
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00013D1F File Offset: 0x00011F1F
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x060012E3 RID: 4835
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006766C File Offset: 0x0006586C
		public override int GetHashCode()
		{
			return ((base.GetType().GetHashCode() * 397 ^ this.ProcessDictionaryKeys.GetHashCode()) * 397 ^ this.ProcessExtensionDataNames.GetHashCode()) * 397 ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00013D32 File Offset: 0x00011F32
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000676C4 File Offset: 0x000658C4
		[NullableContext(2)]
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
