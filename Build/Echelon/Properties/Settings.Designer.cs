using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Echelon.Properties
{
	// Token: 0x02000052 RID: 82
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.5.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000099C1 File Offset: 0x00007BC1
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x0400010C RID: 268
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
