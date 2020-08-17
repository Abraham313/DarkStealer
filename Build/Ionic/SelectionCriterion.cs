using System;
using System.Diagnostics;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D2 RID: 210
	internal abstract class SelectionCriterion
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000A451 File Offset: 0x00008651
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000A459 File Offset: 0x00008659
		internal virtual bool Verbose { get; set; }

		// Token: 0x060003B3 RID: 947
		internal abstract bool Evaluate(string filename);

		// Token: 0x060003B4 RID: 948 RVA: 0x00009B58 File Offset: 0x00007D58
		[Conditional("SelectorTrace")]
		protected static void CriterionTrace(string format, params object[] args)
		{
		}

		// Token: 0x060003B5 RID: 949
		internal abstract bool Evaluate(ZipEntry entry);
	}
}
