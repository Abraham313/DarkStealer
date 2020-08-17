using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D7 RID: 215
	internal class AttributesCriterion : SelectionCriterion
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0002D514 File Offset: 0x0002B714
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0002D5B8 File Offset: 0x0002B7B8
		internal string AttributeString
		{
			get
			{
				string text = "";
				if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
				{
					text += "H";
				}
				if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
				{
					text += "S";
				}
				if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
				{
					text += "R";
				}
				if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
				{
					text += "A";
				}
				if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					text += "L";
				}
				if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
				{
					text += "I";
				}
				return text;
			}
			set
			{
				this._Attributes = FileAttributes.Normal;
				foreach (char c in value.ToUpper())
				{
					char c2 = c;
					if (c2 != 'A')
					{
						switch (c2)
						{
						case 'H':
							if ((this._Attributes & FileAttributes.Hidden) == (FileAttributes)0)
							{
								this._Attributes |= FileAttributes.Hidden;
								goto IL_122;
							}
							throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
						case 'I':
							if ((this._Attributes & FileAttributes.NotContentIndexed) == (FileAttributes)0)
							{
								this._Attributes |= FileAttributes.NotContentIndexed;
								goto IL_122;
							}
							throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
						case 'J':
						case 'K':
							break;
						case 'L':
							if ((this._Attributes & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								this._Attributes |= FileAttributes.ReparsePoint;
								goto IL_122;
							}
							throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
						default:
							switch (c2)
							{
							case 'R':
								if ((this._Attributes & FileAttributes.ReadOnly) == (FileAttributes)0)
								{
									this._Attributes |= FileAttributes.ReadOnly;
									goto IL_122;
								}
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							case 'S':
								if ((this._Attributes & FileAttributes.System) == (FileAttributes)0)
								{
									this._Attributes |= FileAttributes.System;
									goto IL_122;
								}
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							break;
						}
						throw new ArgumentException(value);
					}
					if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
					{
						throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
					}
					this._Attributes |= FileAttributes.Archive;
					IL_122:;
				}
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0002D7A8 File Offset: 0x0002B9A8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs)
		{
			return (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0002D820 File Offset: 0x0002BA20
		internal override bool Evaluate(string filename)
		{
			if (Directory.Exists(filename))
			{
				return this.Operator != ComparisonOperator.EqualTo;
			}
			FileAttributes attributes = File.GetAttributes(filename);
			return this._Evaluate(attributes);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0002D850 File Offset: 0x0002BA50
		private bool _Evaluate(FileAttributes fileAttrs)
		{
			bool flag;
			if (flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden))
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
			}
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0002D8BC File Offset: 0x0002BABC
		internal override bool Evaluate(ZipEntry entry)
		{
			FileAttributes attributes = entry.Attributes;
			return this._Evaluate(attributes);
		}

		// Token: 0x0400029D RID: 669
		private FileAttributes _Attributes;

		// Token: 0x0400029E RID: 670
		internal ComparisonOperator Operator;
	}
}
