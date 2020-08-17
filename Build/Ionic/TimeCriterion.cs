using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D4 RID: 212
	internal class TimeCriterion : SelectionCriterion
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0002D11C File Offset: 0x0002B31C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
			return stringBuilder.ToString();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0002D18C File Offset: 0x0002B38C
		internal override bool Evaluate(string filename)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = File.GetLastAccessTime(filename).ToUniversalTime();
				break;
			case WhichTime.mtime:
				x = File.GetLastWriteTime(filename).ToUniversalTime();
				break;
			case WhichTime.ctime:
				x = File.GetCreationTime(filename).ToUniversalTime();
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return this._Evaluate(x);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
		private bool _Evaluate(DateTime x)
		{
			bool result;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				result = (x > this.Time);
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				result = (x >= this.Time);
				break;
			case ComparisonOperator.LesserThan:
				result = (x < this.Time);
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				result = (x <= this.Time);
				break;
			case ComparisonOperator.EqualTo:
				result = (x == this.Time);
				break;
			case ComparisonOperator.NotEqualTo:
				result = (x != this.Time);
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return result;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0002D290 File Offset: 0x0002B490
		internal override bool Evaluate(ZipEntry entry)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = entry.AccessedTime;
				break;
			case WhichTime.mtime:
				x = entry.ModifiedTime;
				break;
			case WhichTime.ctime:
				x = entry.CreationTime;
				break;
			default:
				throw new ArgumentException("??time");
			}
			return this._Evaluate(x);
		}

		// Token: 0x04000294 RID: 660
		internal ComparisonOperator Operator;

		// Token: 0x04000295 RID: 661
		internal WhichTime Which;

		// Token: 0x04000296 RID: 662
		internal DateTime Time;
	}
}
