using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000322 RID: 802
	internal class BsonValue : BsonToken
	{
		// Token: 0x06001927 RID: 6439 RVA: 0x000181BE File Offset: 0x000163BE
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x000181D4 File Offset: 0x000163D4
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x000181DC File Offset: 0x000163DC
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04000D39 RID: 3385
		private readonly object _value;

		// Token: 0x04000D3A RID: 3386
		private readonly BsonType _type;
	}
}
