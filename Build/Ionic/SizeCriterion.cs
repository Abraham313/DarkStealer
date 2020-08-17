using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D3 RID: 211
	internal class SizeCriterion : SelectionCriterion
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x0002D018 File Offset: 0x0002B218
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("size ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.Size.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0002D06C File Offset: 0x0002B26C
		internal override bool Evaluate(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			return this._Evaluate(fileInfo.Length);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0002D08C File Offset: 0x0002B28C
		private bool _Evaluate(long Length)
		{
			bool result;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				result = (Length > this.Size);
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				result = (Length >= this.Size);
				break;
			case ComparisonOperator.LesserThan:
				result = (Length < this.Size);
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				result = (Length <= this.Size);
				break;
			case ComparisonOperator.EqualTo:
				result = (Length == this.Size);
				break;
			case ComparisonOperator.NotEqualTo:
				result = (Length != this.Size);
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return result;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000A462 File Offset: 0x00008662
		internal override bool Evaluate(ZipEntry entry)
		{
			return this._Evaluate(entry.UncompressedSize);
		}

		// Token: 0x04000292 RID: 658
		internal ComparisonOperator Operator;

		// Token: 0x04000293 RID: 659
		internal long Size;
	}
}
