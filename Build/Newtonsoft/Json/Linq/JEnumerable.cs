using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B7 RID: 695
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct JEnumerable<[Nullable(0)] T> : IEnumerable, IEnumerable<T>, IJEnumerable<T>, IEquatable<JEnumerable<T>> where T : JToken
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x00015964 File Offset: 0x00013B64
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00015978 File Offset: 0x00013B78
		public IEnumerator<T> GetEnumerator()
		{
			return (this._enumerable ?? JEnumerable<T>.Empty).GetEnumerator();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00015993 File Offset: 0x00013B93
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000443 RID: 1091
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (this._enumerable == null)
				{
					return JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(this._enumerable.Values(key));
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000159C6 File Offset: 0x00013BC6
		public bool Equals([Nullable(new byte[]
		{
			0,
			1
		})] JEnumerable<T> other)
		{
			return object.Equals(this._enumerable, other._enumerable);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0006B9F0 File Offset: 0x00069BF0
		public override bool Equals(object obj)
		{
			if (obj is JEnumerable<T>)
			{
				JEnumerable<T> other = (JEnumerable<T>)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000159D9 File Offset: 0x00013BD9
		public override int GetHashCode()
		{
			if (this._enumerable == null)
			{
				return 0;
			}
			return this._enumerable.GetHashCode();
		}

		// Token: 0x04000BD7 RID: 3031
		[Nullable(new byte[]
		{
			0,
			1
		})]
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x04000BD8 RID: 3032
		private readonly IEnumerable<T> _enumerable;
	}
}
