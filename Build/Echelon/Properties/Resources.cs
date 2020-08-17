using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Echelon.Properties
{
	// Token: 0x02000051 RID: 81
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00008754 File Offset: 0x00006954
		internal Resources()
		{
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00009970 File Offset: 0x00007B70
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Echelon.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000999C File Offset: 0x00007B9C
		// (set) Token: 0x060001EA RID: 490 RVA: 0x000099A3 File Offset: 0x00007BA3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000099AB File Offset: 0x00007BAB
		internal static string Domains
		{
			get
			{
				return Resources.ResourceManager.GetString("Domains", Resources.resourceCulture);
			}
		}

		// Token: 0x0400010A RID: 266
		private static ResourceManager resourceMan;

		// Token: 0x0400010B RID: 267
		private static CultureInfo resourceCulture;
	}
}
