using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C4 RID: 708
	public class JsonMergeSettings
	{
		// Token: 0x060015C9 RID: 5577 RVA: 0x00016114 File Offset: 0x00014314
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00016123 File Offset: 0x00014323
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x0001612B File Offset: 0x0001432B
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00016147 File Offset: 0x00014347
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x0001614F File Offset: 0x0001434F
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0001616B File Offset: 0x0001436B
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x00016173 File Offset: 0x00014373
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x04000BEF RID: 3055
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x04000BF0 RID: 3056
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x04000BF1 RID: 3057
		private StringComparison _propertyNameComparison;
	}
}
