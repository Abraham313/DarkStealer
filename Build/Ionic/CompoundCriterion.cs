using System;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D8 RID: 216
	internal class CompoundCriterion : SelectionCriterion
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000A4C7 File Offset: 0x000086C7
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000A4CF File Offset: 0x000086CF
		internal SelectionCriterion Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
				if (value == null)
				{
					this.Conjunction = LogicalConjunction.NONE;
					return;
				}
				if (this.Conjunction == LogicalConjunction.NONE)
				{
					this.Conjunction = LogicalConjunction.AND;
				}
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0002D8D8 File Offset: 0x0002BAD8
		internal override bool Evaluate(string filename)
		{
			bool flag = this.Left.Evaluate(filename);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(filename);
				break;
			default:
				throw new ArgumentException("Conjunction");
			}
			return flag;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0002D94C File Offset: 0x0002BB4C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(").Append((this.Left != null) ? this.Left.ToString() : "null").Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append((this.Right != null) ? this.Right.ToString() : "null").Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0002D9E4 File Offset: 0x0002BBE4
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = this.Left.Evaluate(entry);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(entry);
				break;
			}
			return flag;
		}

		// Token: 0x0400029F RID: 671
		internal LogicalConjunction Conjunction;

		// Token: 0x040002A0 RID: 672
		internal SelectionCriterion Left;

		// Token: 0x040002A1 RID: 673
		private SelectionCriterion _Right;
	}
}
