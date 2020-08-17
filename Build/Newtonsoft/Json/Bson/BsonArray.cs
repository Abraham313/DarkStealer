using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000320 RID: 800
	internal class BsonArray : BsonToken, IEnumerable, IEnumerable<BsonToken>
	{
		// Token: 0x0600191F RID: 6431 RVA: 0x0001814C File Offset: 0x0001634C
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x00015DBE File Offset: 0x00013FBE
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x00018161 File Offset: 0x00016361
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x00018173 File Offset: 0x00016373
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000D35 RID: 3381
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
