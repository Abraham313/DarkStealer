using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200031F RID: 799
	internal class BsonObject : BsonToken, IEnumerable, IEnumerable<BsonProperty>
	{
		// Token: 0x0600191A RID: 6426 RVA: 0x000180F2 File Offset: 0x000162F2
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x00009011 File Offset: 0x00007211
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0001811F File Offset: 0x0001631F
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x00018131 File Offset: 0x00016331
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000D34 RID: 3380
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
