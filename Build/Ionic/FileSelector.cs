using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x020000D9 RID: 217
	[ComVisible(true)]
	public class FileSelector
	{
		// Token: 0x060003DB RID: 987 RVA: 0x0000A4F2 File Offset: 0x000086F2
		public FileSelector(string selectionCriteria) : this(selectionCriteria, true)
		{
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000A4FC File Offset: 0x000086FC
		public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
		{
			if (!string.IsNullOrEmpty(selectionCriteria))
			{
				this._Criterion = FileSelector._ParseCriterion(selectionCriteria);
			}
			this.TraverseReparsePoints = traverseDirectoryReparsePoints;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000A51F File Offset: 0x0000871F
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000A536 File Offset: 0x00008736
		public string SelectionCriteria
		{
			get
			{
				if (this._Criterion == null)
				{
					return null;
				}
				return this._Criterion.ToString();
			}
			set
			{
				if (value == null)
				{
					this._Criterion = null;
					return;
				}
				if (value.Trim() == "")
				{
					this._Criterion = null;
					return;
				}
				this._Criterion = FileSelector._ParseCriterion(value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000A569 File Offset: 0x00008769
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000A571 File Offset: 0x00008771
		public bool TraverseReparsePoints { get; set; }

		// Token: 0x060003E1 RID: 993 RVA: 0x0002DA50 File Offset: 0x0002BC50
		private static string NormalizeCriteriaExpression(string source)
		{
			string[][] array = new string[][]
			{
				new string[]
				{
					"([^']*)\\(\\(([^']+)",
					"$1( ($2"
				},
				new string[]
				{
					"(.)\\)\\)",
					"$1) )"
				},
				new string[]
				{
					"\\((\\S)",
					"( $1"
				},
				new string[]
				{
					"(\\S)\\)",
					"$1 )"
				},
				new string[]
				{
					"^\\)",
					" )"
				},
				new string[]
				{
					"(\\S)\\(",
					"$1 ("
				},
				new string[]
				{
					"\\)(\\S)",
					") $1"
				},
				new string[]
				{
					"(=)('[^']*')",
					"$1 $2"
				},
				new string[]
				{
					"([^ !><])(>|<|!=|=)",
					"$1 $2"
				},
				new string[]
				{
					"(>|<|!=|=)([^ =])",
					"$1 $2"
				},
				new string[]
				{
					"/",
					"\\"
				}
			};
			string input = source;
			for (int i = 0; i < array.Length; i++)
			{
				string pattern = FileSelector.RegexAssertions.PrecededByEvenNumberOfSingleQuotes + array[i][0] + FileSelector.RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
				input = Regex.Replace(input, pattern, array[i][1]);
			}
			string pattern2 = "/" + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			input = Regex.Replace(input, pattern2, "\\");
			pattern2 = " " + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			return Regex.Replace(input, pattern2, "\u0006");
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0002DC9C File Offset: 0x0002BE9C
		private static SelectionCriterion _ParseCriterion(string s)
		{
			if (s == null)
			{
				return null;
			}
			s = FileSelector.NormalizeCriteriaExpression(s);
			if (s.IndexOf(" ") == -1)
			{
				s = "name = " + s;
			}
			string[] array = s.Trim().Split(new char[]
			{
				' ',
				'\t'
			});
			if (array.Length < 3)
			{
				throw new ArgumentException(s);
			}
			SelectionCriterion selectionCriterion = null;
			Stack<FileSelector.ParseState> stack = new Stack<FileSelector.ParseState>();
			Stack<SelectionCriterion> stack2 = new Stack<SelectionCriterion>();
			stack.Push(FileSelector.ParseState.Start);
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i].ToLower();
				string key;
				if ((key = text) != null)
				{
					if (<PrivateImplementationDetails>{510A9A0A-2EB8-4C1C-AA23-D4ACD845FEA7}.$$method0x6000091-1 == null)
					{
						<PrivateImplementationDetails>{510A9A0A-2EB8-4C1C-AA23-D4ACD845FEA7}.$$method0x6000091-1 = new Dictionary<string, int>(16)
						{
							{
								"and",
								0
							},
							{
								"xor",
								1
							},
							{
								"or",
								2
							},
							{
								"(",
								3
							},
							{
								")",
								4
							},
							{
								"atime",
								5
							},
							{
								"ctime",
								6
							},
							{
								"mtime",
								7
							},
							{
								"length",
								8
							},
							{
								"size",
								9
							},
							{
								"filename",
								10
							},
							{
								"name",
								11
							},
							{
								"attrs",
								12
							},
							{
								"attributes",
								13
							},
							{
								"type",
								14
							},
							{
								"",
								15
							}
						};
					}
					int num;
					if (<PrivateImplementationDetails>{510A9A0A-2EB8-4C1C-AA23-D4ACD845FEA7}.$$method0x6000091-1.TryGetValue(key, out num))
					{
						FileSelector.ParseState parseState;
						switch (num)
						{
						case 0:
						case 1:
						case 2:
						{
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.CriterionDone)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 3)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							LogicalConjunction conjunction = (LogicalConjunction)Enum.Parse(typeof(LogicalConjunction), array[i].ToUpper(), true);
							selectionCriterion = new CompoundCriterion
							{
								Left = selectionCriterion,
								Right = null,
								Conjunction = conjunction
							};
							stack.Push(parseState);
							stack.Push(FileSelector.ParseState.ConjunctionPending);
							stack2.Push(selectionCriterion);
							break;
						}
						case 3:
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.Start && parseState != FileSelector.ParseState.ConjunctionPending && parseState != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 4)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Push(FileSelector.ParseState.OpenParen);
							break;
						case 4:
							parseState = stack.Pop();
							if (stack.Peek() != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Pop();
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						case 5:
						case 6:
						case 7:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							DateTime dateTime;
							try
							{
								dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd-HH:mm:ss", null);
								goto IL_743;
							}
							catch (FormatException)
							{
								try
								{
									dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd-HH:mm:ss", null);
								}
								catch (FormatException)
								{
									try
									{
										dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd", null);
									}
									catch (FormatException)
									{
										try
										{
											dateTime = DateTime.ParseExact(array[i + 2], "MM/dd/yyyy", null);
										}
										catch (FormatException)
										{
											dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd", null);
										}
									}
								}
								goto IL_743;
							}
							goto IL_352;
							IL_743:
							dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
							selectionCriterion = new TimeCriterion
							{
								Which = (WhichTime)Enum.Parse(typeof(WhichTime), array[i], true),
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]),
								Time = dateTime
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 8:
						case 9:
							goto IL_352;
						case 10:
						case 11:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator != ComparisonOperator.NotEqualTo && comparisonOperator != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							string text2 = array[i + 2];
							if (text2.StartsWith("'") && text2.EndsWith("'"))
							{
								text2 = text2.Substring(1, text2.Length - 2).Replace("\u0006", " ");
							}
							selectionCriterion = new NameCriterion
							{
								MatchingFileSpec = text2,
								Operator = comparisonOperator
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 12:
						case 13:
						case 14:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator2 = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator2 != ComparisonOperator.NotEqualTo && comparisonOperator2 != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							selectionCriterion = ((text == "type") ? new TypeCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							} : new AttributesCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							});
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 15:
							stack.Push(FileSelector.ParseState.Whitespace);
							break;
						default:
							goto IL_8E3;
						}
						IL_6C1:
						parseState = stack.Peek();
						if (parseState == FileSelector.ParseState.CriterionDone)
						{
							stack.Pop();
							if (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
							{
								while (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
								{
									CompoundCriterion compoundCriterion = stack2.Pop() as CompoundCriterion;
									compoundCriterion.Right = selectionCriterion;
									selectionCriterion = compoundCriterion;
									stack.Pop();
									parseState = stack.Pop();
									if (parseState != FileSelector.ParseState.CriterionDone)
									{
										throw new ArgumentException("??");
									}
								}
							}
							else
							{
								stack.Push(FileSelector.ParseState.CriterionDone);
							}
						}
						if (parseState == FileSelector.ParseState.Whitespace)
						{
							stack.Pop();
						}
						i++;
						continue;
						IL_352:
						if (array.Length > i + 2)
						{
							string text3 = array[i + 2];
							long size;
							if (text3.ToUpper().EndsWith("K"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L;
							}
							else if (text3.ToUpper().EndsWith("KB"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L;
							}
							else if (text3.ToUpper().EndsWith("M"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("MB"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("G"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("GB"))
							{
								size = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L * 1024L * 1024L;
							}
							else
							{
								size = long.Parse(array[i + 2]);
							}
							selectionCriterion = new SizeCriterion
							{
								Size = size,
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1])
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							goto IL_6C1;
						}
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
				}
				IL_8E3:
				throw new ArgumentException("'" + array[i] + "'");
			}
			return selectionCriterion;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000A57A File Offset: 0x0000877A
		public override string ToString()
		{
			return "FileSelector(" + this._Criterion.ToString() + ")";
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000A596 File Offset: 0x00008796
		private bool Evaluate(string filename)
		{
			return this._Criterion.Evaluate(filename);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000A5A4 File Offset: 0x000087A4
		[Conditional("SelectorTrace")]
		private void SelectorTrace(string format, params object[] args)
		{
			if (this._Criterion != null && this._Criterion.Verbose)
			{
				Console.WriteLine(format, args);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000A5C2 File Offset: 0x000087C2
		public ICollection<string> SelectFiles(string directory)
		{
			return this.SelectFiles(directory, false);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0002E5E0 File Offset: 0x0002C7E0
		public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
		{
			if (this._Criterion == null)
			{
				throw new ArgumentException("SelectionCriteria has not been set");
			}
			List<string> list = new List<string>();
			try
			{
				if (Directory.Exists(directory))
				{
					string[] files = Directory.GetFiles(directory);
					foreach (string text in files)
					{
						if (this.Evaluate(text))
						{
							list.Add(text);
						}
					}
					if (recurseDirectories)
					{
						string[] directories = Directory.GetDirectories(directory);
						foreach (string text2 in directories)
						{
							if (this.TraverseReparsePoints || (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								if (this.Evaluate(text2))
								{
									list.Add(text2);
								}
								list.AddRange(this.SelectFiles(text2, recurseDirectories));
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return list.AsReadOnly();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000A5CC File Offset: 0x000087CC
		private bool Evaluate(ZipEntry entry)
		{
			return this._Criterion.Evaluate(entry);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0002E6D4 File Offset: 0x0002C8D4
		public ICollection<ZipEntry> SelectEntries(ZipFile zip)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry zipEntry in zip)
			{
				if (this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0002E73C File Offset: 0x0002C93C
		public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			string text = (directoryPathInArchive == null) ? null : directoryPathInArchive.Replace("/", "\\");
			if (text != null)
			{
				while (text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			foreach (ZipEntry zipEntry in zip)
			{
				if ((directoryPathInArchive == null || Path.GetDirectoryName(zipEntry.FileName) == directoryPathInArchive || Path.GetDirectoryName(zipEntry.FileName) == text) && this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x040002A2 RID: 674
		internal SelectionCriterion _Criterion;

		// Token: 0x020000DA RID: 218
		private enum ParseState
		{
			// Token: 0x040002A5 RID: 677
			Start,
			// Token: 0x040002A6 RID: 678
			OpenParen,
			// Token: 0x040002A7 RID: 679
			CriterionDone,
			// Token: 0x040002A8 RID: 680
			ConjunctionPending,
			// Token: 0x040002A9 RID: 681
			Whitespace
		}

		// Token: 0x020000DB RID: 219
		private static class RegexAssertions
		{
			// Token: 0x040002AA RID: 682
			public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";

			// Token: 0x040002AB RID: 683
			public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";

			// Token: 0x040002AC RID: 684
			public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";

			// Token: 0x040002AD RID: 685
			public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
		}
	}
}
