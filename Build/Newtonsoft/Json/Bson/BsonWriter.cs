using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000329 RID: 809
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x000182C1 File Offset: 0x000164C1
		// (set) Token: 0x0600193F RID: 6463 RVA: 0x000182CE File Offset: 0x000164CE
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x000182DC File Offset: 0x000164DC
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x00018300 File Offset: 0x00016500
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0001831F File Offset: 0x0001651F
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0001832C File Offset: 0x0001652C
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00018354 File Offset: 0x00016554
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00018362 File Offset: 0x00016562
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00018370 File Offset: 0x00016570
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00018370 File Offset: 0x00016570
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0001837E File Offset: 0x0001657E
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00018391 File Offset: 0x00016591
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x000183A4 File Offset: 0x000165A4
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x000183B4 File Offset: 0x000165B4
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x000183D4 File Offset: 0x000165D4
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x000183E4 File Offset: 0x000165E4
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x000183F7 File Offset: 0x000165F7
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00077FDC File Offset: 0x000761DC
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				BsonObject bsonObject = this._parent as BsonObject;
				if (bsonObject != null)
				{
					bsonObject.Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0007806C File Offset: 0x0007626C
		public override void WriteValue(object value)
		{
			if (value is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger bigInteger = (System.Numerics.BigInteger)value;
				base.SetWriteState(JsonToken.Integer, null);
				this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00018406 File Offset: 0x00016606
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x00018419 File Offset: 0x00016619
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0001842C File Offset: 0x0001662C
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddToken((value == null) ? BsonEmpty.Null : new BsonString(value, true));
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0001844C File Offset: 0x0001664C
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00018463 File Offset: 0x00016663
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0001848F File Offset: 0x0001668F
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000184A6 File Offset: 0x000166A6
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x000184D6 File Offset: 0x000166D6
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000184EC File Offset: 0x000166EC
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00018502 File Offset: 0x00016702
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00018520 File Offset: 0x00016720
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00018537 File Offset: 0x00016737
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x000780AC File Offset: 0x000762AC
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(value2, true));
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0001854E File Offset: 0x0001674E
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00018565 File Offset: 0x00016765
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0001857C File Offset: 0x0001677C
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00018592 File Offset: 0x00016792
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x000185B7 File Offset: 0x000167B7
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x000185CE File Offset: 0x000167CE
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x000185EE File Offset: 0x000167EE
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0001860A File Offset: 0x0001680A
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0001862C File Offset: 0x0001682C
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00018657 File Offset: 0x00016857
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00018689 File Offset: 0x00016889
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x04000D59 RID: 3417
		private readonly BsonBinaryWriter _writer;

		// Token: 0x04000D5A RID: 3418
		private BsonToken _root;

		// Token: 0x04000D5B RID: 3419
		private BsonToken _parent;

		// Token: 0x04000D5C RID: 3420
		private string _propertyName;
	}
}
