using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001F2 RID: 498
	[NullableContext(1)]
	[Nullable(0)]
	internal struct DateTimeParser
	{
		// Token: 0x06000E8D RID: 3725 RVA: 0x00011116 File Offset: 0x0000F316
		public bool Parse(char[] text, int startIndex, int length)
		{
			this._text = text;
			this._end = startIndex + length;
			return this.ParseDate(startIndex) && this.ParseChar(DateTimeParser.Lzyyyy_MM_dd + startIndex, 'T') && this.ParseTimeAndZoneAndWhitespace(DateTimeParser.Lzyyyy_MM_ddT + startIndex);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x000581F0 File Offset: 0x000563F0
		private bool ParseDate(int start)
		{
			return this.Parse4Digit(start, out this.Year) && 1 <= this.Year && this.ParseChar(start + DateTimeParser.Lzyyyy, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_, out this.Month) && 1 <= this.Month && this.Month <= 12 && this.ParseChar(start + DateTimeParser.Lzyyyy_MM, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_MM_, out this.Day) && 1 <= this.Day && this.Day <= DateTime.DaysInMonth(this.Year, this.Month);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00011154 File Offset: 0x0000F354
		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			return this.ParseTime(ref start) && this.ParseZone(start);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000582A4 File Offset: 0x000564A4
		private bool ParseTime(ref int start)
		{
			if (this.Parse2Digit(start, out this.Hour) && this.Hour <= 24 && this.ParseChar(start + DateTimeParser.LzHH, ':') && this.Parse2Digit(start + DateTimeParser.LzHH_, out this.Minute) && this.Minute < 60 && this.ParseChar(start + DateTimeParser.LzHH_mm, ':') && this.Parse2Digit(start + DateTimeParser.LzHH_mm_, out this.Second) && this.Second < 60 && (this.Hour != 24 || (this.Minute == 0 && this.Second == 0)))
			{
				start += DateTimeParser.LzHH_mm_ss;
				if (this.ParseChar(start, '.'))
				{
					this.Fraction = 0;
					int num = 0;
					for (;;)
					{
						int num2 = start + 1;
						start = num2;
						if (num2 >= this._end || num >= 7)
						{
							break;
						}
						int num3 = (int)(this._text[start] - '0');
						if (num3 < 0 || num3 > 9)
						{
							break;
						}
						this.Fraction = this.Fraction * 10 + num3;
						num++;
					}
					if (num < 7)
					{
						if (num == 0)
						{
							return false;
						}
						this.Fraction *= DateTimeParser.Power10[7 - num];
					}
					if (this.Hour == 24 && this.Fraction != 0)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00058400 File Offset: 0x00056600
		private bool ParseZone(int start)
		{
			if (start < this._end)
			{
				char c = this._text[start];
				if (c != 'Z')
				{
					if (c != 'z')
					{
						if (start + 2 < this._end && this.Parse2Digit(start + DateTimeParser.Lz_, out this.ZoneHour) && this.ZoneHour <= 99)
						{
							if (c != '+')
							{
								if (c == '-')
								{
									this.Zone = ParserTimeZone.LocalWestOfUtc;
									start += DateTimeParser.Lz_zz;
								}
							}
							else
							{
								this.Zone = ParserTimeZone.LocalEastOfUtc;
								start += DateTimeParser.Lz_zz;
							}
						}
						if (start >= this._end)
						{
							goto IL_F8;
						}
						if (this.ParseChar(start, ':'))
						{
							start++;
							if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
							{
								start += 2;
								goto IL_F8;
							}
							goto IL_F8;
						}
						else
						{
							if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
							{
								start += 2;
								goto IL_F8;
							}
							goto IL_F8;
						}
					}
				}
				this.Zone = ParserTimeZone.Utc;
				start++;
			}
			IL_F8:
			return start == this._end;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00058510 File Offset: 0x00056710
		private bool Parse4Digit(int start, out int num)
		{
			if (start + 3 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				int num4 = (int)(this._text[start + 2] - '0');
				int num5 = (int)(this._text[start + 3] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0005859C File Offset: 0x0005679C
		private bool Parse2Digit(int start, out int num)
		{
			if (start + 1 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00011169 File Offset: 0x0000F369
		private bool ParseChar(int start, char ch)
		{
			return start < this._end && this._text[start] == ch;
		}

		// Token: 0x04000943 RID: 2371
		public int Year;

		// Token: 0x04000944 RID: 2372
		public int Month;

		// Token: 0x04000945 RID: 2373
		public int Day;

		// Token: 0x04000946 RID: 2374
		public int Hour;

		// Token: 0x04000947 RID: 2375
		public int Minute;

		// Token: 0x04000948 RID: 2376
		public int Second;

		// Token: 0x04000949 RID: 2377
		public int Fraction;

		// Token: 0x0400094A RID: 2378
		public int ZoneHour;

		// Token: 0x0400094B RID: 2379
		public int ZoneMinute;

		// Token: 0x0400094C RID: 2380
		public ParserTimeZone Zone;

		// Token: 0x0400094D RID: 2381
		private char[] _text;

		// Token: 0x0400094E RID: 2382
		private int _end;

		// Token: 0x0400094F RID: 2383
		private static readonly int[] Power10 = new int[]
		{
			-1,
			10,
			100,
			1000,
			10000,
			100000,
			1000000
		};

		// Token: 0x04000950 RID: 2384
		private static readonly int Lzyyyy = "yyyy".Length;

		// Token: 0x04000951 RID: 2385
		private static readonly int Lzyyyy_ = "yyyy-".Length;

		// Token: 0x04000952 RID: 2386
		private static readonly int Lzyyyy_MM = "yyyy-MM".Length;

		// Token: 0x04000953 RID: 2387
		private static readonly int Lzyyyy_MM_ = "yyyy-MM-".Length;

		// Token: 0x04000954 RID: 2388
		private static readonly int Lzyyyy_MM_dd = "yyyy-MM-dd".Length;

		// Token: 0x04000955 RID: 2389
		private static readonly int Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;

		// Token: 0x04000956 RID: 2390
		private static readonly int LzHH = "HH".Length;

		// Token: 0x04000957 RID: 2391
		private static readonly int LzHH_ = "HH:".Length;

		// Token: 0x04000958 RID: 2392
		private static readonly int LzHH_mm = "HH:mm".Length;

		// Token: 0x04000959 RID: 2393
		private static readonly int LzHH_mm_ = "HH:mm:".Length;

		// Token: 0x0400095A RID: 2394
		private static readonly int LzHH_mm_ss = "HH:mm:ss".Length;

		// Token: 0x0400095B RID: 2395
		private static readonly int Lz_ = "-".Length;

		// Token: 0x0400095C RID: 2396
		private static readonly int Lz_zz = "-zz".Length;

		// Token: 0x0400095D RID: 2397
		private const short MaxFractionDigits = 7;
	}
}
