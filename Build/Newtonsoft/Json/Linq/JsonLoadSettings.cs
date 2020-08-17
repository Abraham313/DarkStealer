using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002C3 RID: 707
	public class JsonLoadSettings
	{
		// Token: 0x060015C2 RID: 5570 RVA: 0x0001608B File Offset: 0x0001428B
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x000160A8 File Offset: 0x000142A8
		// (set) Token: 0x060015C4 RID: 5572 RVA: 0x000160B0 File Offset: 0x000142B0
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x000160CC File Offset: 0x000142CC
		// (set) Token: 0x060015C6 RID: 5574 RVA: 0x000160D4 File Offset: 0x000142D4
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x000160F0 File Offset: 0x000142F0
		// (set) Token: 0x060015C8 RID: 5576 RVA: 0x000160F8 File Offset: 0x000142F8
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x04000BEC RID: 3052
		private CommentHandling _commentHandling;

		// Token: 0x04000BED RID: 3053
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x04000BEE RID: 3054
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
