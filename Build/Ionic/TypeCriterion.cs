using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D6 RID: 214
	internal class TypeCriterion : SelectionCriterion
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000A481 File Offset: 0x00008681
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0000A48E File Offset: 0x0000868E
		internal string AttributeString
		{
			get
			{
				return this.ObjectType.ToString();
			}
			set
			{
				if (value.Length != 1 || (value[0] != 'D' && value[0] != 'F'))
				{
					throw new ArgumentException("Specify a single character: either D or F");
				}
				this.ObjectType = value[0];
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0002D454 File Offset: 0x0002B654
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0002D4A4 File Offset: 0x0002B6A4
		internal override bool Evaluate(string filename)
		{
			bool flag = (this.ObjectType == 'D') ? Directory.Exists(filename) : File.Exists(filename);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0002D4DC File Offset: 0x0002B6DC
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = (this.ObjectType == 'D') ? entry.IsDirectory : (!entry.IsDirectory);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x0400029B RID: 667
		private char ObjectType;

		// Token: 0x0400029C RID: 668
		internal ComparisonOperator Operator;
	}
}
