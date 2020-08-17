using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D5 RID: 213
	internal class NameCriterion : SelectionCriterion
	{
		// Token: 0x1700005B RID: 91
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
		internal virtual string MatchingFileSpec
		{
			set
			{
				if (Directory.Exists(value))
				{
					this._MatchingFileSpec = ".\\" + value + "\\*.*";
				}
				else
				{
					this._MatchingFileSpec = value;
				}
				this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]") + "$";
				this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0002D388 File Offset: 0x0002B588
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("name ").Append(EnumUtil.GetDescription(this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A478 File Offset: 0x00008678
		internal override bool Evaluate(string filename)
		{
			return this._Evaluate(filename);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0002D3E4 File Offset: 0x0002B5E4
		private bool _Evaluate(string fullpath)
		{
			string input = (this._MatchingFileSpec.IndexOf('\\') == -1) ? Path.GetFileName(fullpath) : fullpath;
			bool flag = this._re.IsMatch(input);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0002D428 File Offset: 0x0002B628
		internal override bool Evaluate(ZipEntry entry)
		{
			string fullpath = entry.FileName.Replace("/", "\\");
			return this._Evaluate(fullpath);
		}

		// Token: 0x04000297 RID: 663
		private Regex _re;

		// Token: 0x04000298 RID: 664
		private string _regexString;

		// Token: 0x04000299 RID: 665
		internal ComparisonOperator Operator;

		// Token: 0x0400029A RID: 666
		private string _MatchingFileSpec;
	}
}
