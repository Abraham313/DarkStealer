using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200031B RID: 795
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonReader : JsonReader
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00017F36 File Offset: 0x00016136
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x00017F3E File Offset: 0x0001613E
		[Obsolete("JsonNet35BinaryCompatibility will be removed in a future version of Json.NET.")]
		public bool JsonNet35BinaryCompatibility
		{
			get
			{
				return this._jsonNet35BinaryCompatibility;
			}
			set
			{
				this._jsonNet35BinaryCompatibility = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00017F47 File Offset: 0x00016147
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x00017F4F File Offset: 0x0001614F
		public bool ReadRootValueAsArray
		{
			get
			{
				return this._readRootValueAsArray;
			}
			set
			{
				this._readRootValueAsArray = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00017F58 File Offset: 0x00016158
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x00017F60 File Offset: 0x00016160
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._dateTimeKindHandling;
			}
			set
			{
				this._dateTimeKindHandling = value;
			}
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00017F69 File Offset: 0x00016169
		public BsonReader(Stream stream) : this(stream, false, DateTimeKind.Local)
		{
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00017F74 File Offset: 0x00016174
		public BsonReader(BinaryReader reader) : this(reader, false, DateTimeKind.Local)
		{
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00017F7F File Offset: 0x0001617F
		public BsonReader(Stream stream, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._reader = new BinaryReader(stream);
			this._stack = new List<BsonReader.ContainerContext>();
			this._readRootValueAsArray = readRootValueAsArray;
			this._dateTimeKindHandling = dateTimeKindHandling;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00017FB7 File Offset: 0x000161B7
		public BsonReader(BinaryReader reader, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new List<BsonReader.ContainerContext>();
			this._readRootValueAsArray = readRootValueAsArray;
			this._dateTimeKindHandling = dateTimeKindHandling;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00017FEA File Offset: 0x000161EA
		private string ReadElement()
		{
			this._currentElementType = this.ReadType();
			return this.ReadString();
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00077540 File Offset: 0x00075740
		public override bool Read()
		{
			bool result;
			try
			{
				bool flag;
				switch (this._bsonReaderState)
				{
				case BsonReader.BsonReaderState.Normal:
					flag = this.ReadNormal();
					break;
				case BsonReader.BsonReaderState.ReferenceStart:
				case BsonReader.BsonReaderState.ReferenceRef:
				case BsonReader.BsonReaderState.ReferenceId:
					flag = this.ReadReference();
					break;
				case BsonReader.BsonReaderState.CodeWScopeStart:
				case BsonReader.BsonReaderState.CodeWScopeCode:
				case BsonReader.BsonReaderState.CodeWScopeScope:
				case BsonReader.BsonReaderState.CodeWScopeScopeObject:
				case BsonReader.BsonReaderState.CodeWScopeScopeEnd:
					flag = this.ReadCodeWScope();
					break;
				default:
					throw JsonReaderException.Create(this, "Unexpected state: {0}".FormatWith(CultureInfo.InvariantCulture, this._bsonReaderState));
				}
				if (!flag)
				{
					base.SetToken(JsonToken.None);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (EndOfStreamException)
			{
				base.SetToken(JsonToken.None);
				result = false;
			}
			return result;
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00017FFE File Offset: 0x000161FE
		public override void Close()
		{
			base.Close();
			if (base.CloseInput)
			{
				BinaryReader reader = this._reader;
				if (reader == null)
				{
					return;
				}
				reader.Close();
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x000775E8 File Offset: 0x000757E8
		private bool ReadCodeWScope()
		{
			switch (this._bsonReaderState)
			{
			case BsonReader.BsonReaderState.CodeWScopeStart:
				base.SetToken(JsonToken.PropertyName, "$code");
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeCode;
				return true;
			case BsonReader.BsonReaderState.CodeWScopeCode:
				this.ReadInt32();
				base.SetToken(JsonToken.String, this.ReadLengthString());
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScope;
				return true;
			case BsonReader.BsonReaderState.CodeWScopeScope:
			{
				if (base.CurrentState == JsonReader.State.PostValue)
				{
					base.SetToken(JsonToken.PropertyName, "$scope");
					return true;
				}
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScopeObject;
				BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(BsonType.Object);
				this.PushContext(containerContext);
				containerContext.Length = this.ReadInt32();
				return true;
			}
			case BsonReader.BsonReaderState.CodeWScopeScopeObject:
			{
				bool flag = this.ReadNormal();
				if (flag && this.TokenType == JsonToken.EndObject)
				{
					this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeScopeEnd;
				}
				return flag;
			}
			case BsonReader.BsonReaderState.CodeWScopeScopeEnd:
				base.SetToken(JsonToken.EndObject);
				this._bsonReaderState = BsonReader.BsonReaderState.Normal;
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000776C0 File Offset: 0x000758C0
		private bool ReadReference()
		{
			JsonReader.State currentState = base.CurrentState;
			if (currentState != JsonReader.State.Property)
			{
				if (currentState == JsonReader.State.ObjectStart)
				{
					base.SetToken(JsonToken.PropertyName, "$ref");
					this._bsonReaderState = BsonReader.BsonReaderState.ReferenceRef;
					return true;
				}
				if (currentState != JsonReader.State.PostValue)
				{
					throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + base.CurrentState.ToString());
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceRef)
				{
					base.SetToken(JsonToken.PropertyName, "$id");
					this._bsonReaderState = BsonReader.BsonReaderState.ReferenceId;
					return true;
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceId)
				{
					base.SetToken(JsonToken.EndObject);
					this._bsonReaderState = BsonReader.BsonReaderState.Normal;
					return true;
				}
				throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + this._bsonReaderState.ToString());
			}
			else
			{
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceRef)
				{
					base.SetToken(JsonToken.String, this.ReadLengthString());
					return true;
				}
				if (this._bsonReaderState == BsonReader.BsonReaderState.ReferenceId)
				{
					base.SetToken(JsonToken.Bytes, this.ReadBytes(12));
					return true;
				}
				throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + this._bsonReaderState.ToString());
			}
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000777D4 File Offset: 0x000759D4
		private bool ReadNormal()
		{
			switch (base.CurrentState)
			{
			case JsonReader.State.Start:
				goto IL_6D;
			case JsonReader.State.Complete:
			case JsonReader.State.Closed:
				return false;
			case JsonReader.State.Property:
				this.ReadType(this._currentElementType);
				return true;
			case JsonReader.State.ObjectStart:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.PostValue:
			{
				BsonReader.ContainerContext currentContext = this._currentContext;
				if (currentContext == null)
				{
					if (!base.SupportMultipleContent)
					{
						return false;
					}
					goto IL_6D;
				}
				else
				{
					int num = currentContext.Length - 1;
					if (currentContext.Position < num)
					{
						if (currentContext.Type == BsonType.Array)
						{
							this.ReadElement();
							this.ReadType(this._currentElementType);
							return true;
						}
						base.SetToken(JsonToken.PropertyName, this.ReadElement());
						return true;
					}
					else
					{
						if (currentContext.Position != num)
						{
							throw JsonReaderException.Create(this, "Read past end of current container context.");
						}
						if (this.ReadByte() != 0)
						{
							throw JsonReaderException.Create(this, "Unexpected end of object byte value.");
						}
						this.PopContext();
						if (this._currentContext != null)
						{
							this.MovePosition(currentContext.Length);
						}
						JsonToken token = (currentContext.Type == BsonType.Object) ? JsonToken.EndObject : JsonToken.EndArray;
						base.SetToken(token);
						return true;
					}
				}
				break;
			}
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
			case JsonReader.State.Error:
			case JsonReader.State.Finished:
				return false;
			}
			throw new ArgumentOutOfRangeException();
			IL_6D:
			JsonToken token2 = (!this._readRootValueAsArray) ? JsonToken.StartObject : JsonToken.StartArray;
			BsonType type = (!this._readRootValueAsArray) ? BsonType.Object : BsonType.Array;
			base.SetToken(token2);
			BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(type);
			this.PushContext(containerContext);
			containerContext.Length = this.ReadInt32();
			return true;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0007792C File Offset: 0x00075B2C
		private void PopContext()
		{
			this._stack.RemoveAt(this._stack.Count - 1);
			if (this._stack.Count == 0)
			{
				this._currentContext = null;
				return;
			}
			this._currentContext = this._stack[this._stack.Count - 1];
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0001801E File Offset: 0x0001621E
		private void PushContext(BsonReader.ContainerContext newContext)
		{
			this._stack.Add(newContext);
			this._currentContext = newContext;
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00018033 File Offset: 0x00016233
		private byte ReadByte()
		{
			this.MovePosition(1);
			return this._reader.ReadByte();
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00077984 File Offset: 0x00075B84
		private void ReadType(BsonType type)
		{
			switch (type)
			{
			case BsonType.Number:
			{
				double num = this.ReadDouble();
				if (this._floatParseHandling == FloatParseHandling.Decimal)
				{
					base.SetToken(JsonToken.Float, Convert.ToDecimal(num, CultureInfo.InvariantCulture));
					return;
				}
				base.SetToken(JsonToken.Float, num);
				return;
			}
			case BsonType.String:
			case BsonType.Symbol:
				base.SetToken(JsonToken.String, this.ReadLengthString());
				return;
			case BsonType.Object:
			{
				base.SetToken(JsonToken.StartObject);
				BsonReader.ContainerContext containerContext = new BsonReader.ContainerContext(BsonType.Object);
				this.PushContext(containerContext);
				containerContext.Length = this.ReadInt32();
				return;
			}
			case BsonType.Array:
			{
				base.SetToken(JsonToken.StartArray);
				BsonReader.ContainerContext containerContext2 = new BsonReader.ContainerContext(BsonType.Array);
				this.PushContext(containerContext2);
				containerContext2.Length = this.ReadInt32();
				return;
			}
			case BsonType.Binary:
			{
				BsonBinaryType bsonBinaryType;
				byte[] array = this.ReadBinary(out bsonBinaryType);
				object value = (bsonBinaryType != BsonBinaryType.Uuid) ? array : new Guid(array);
				base.SetToken(JsonToken.Bytes, value);
				return;
			}
			case BsonType.Undefined:
				base.SetToken(JsonToken.Undefined);
				return;
			case BsonType.Oid:
			{
				byte[] value2 = this.ReadBytes(12);
				base.SetToken(JsonToken.Bytes, value2);
				return;
			}
			case BsonType.Boolean:
			{
				bool flag = Convert.ToBoolean(this.ReadByte());
				base.SetToken(JsonToken.Boolean, flag);
				return;
			}
			case BsonType.Date:
			{
				DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(this.ReadInt64());
				DateTimeKind dateTimeKindHandling = this.DateTimeKindHandling;
				DateTime dateTime2;
				if (dateTimeKindHandling != DateTimeKind.Unspecified)
				{
					if (dateTimeKindHandling != DateTimeKind.Local)
					{
						dateTime2 = dateTime;
					}
					else
					{
						dateTime2 = dateTime.ToLocalTime();
					}
				}
				else
				{
					dateTime2 = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
				}
				base.SetToken(JsonToken.Date, dateTime2);
				return;
			}
			case BsonType.Null:
				base.SetToken(JsonToken.Null);
				return;
			case BsonType.Regex:
			{
				string str = this.ReadString();
				string str2 = this.ReadString();
				string value3 = "/" + str + "/" + str2;
				base.SetToken(JsonToken.String, value3);
				return;
			}
			case BsonType.Reference:
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.ReferenceStart;
				return;
			case BsonType.Code:
				base.SetToken(JsonToken.String, this.ReadLengthString());
				return;
			case BsonType.CodeWScope:
				base.SetToken(JsonToken.StartObject);
				this._bsonReaderState = BsonReader.BsonReaderState.CodeWScopeStart;
				return;
			case BsonType.Integer:
				base.SetToken(JsonToken.Integer, (long)this.ReadInt32());
				return;
			case BsonType.TimeStamp:
			case BsonType.Long:
				base.SetToken(JsonToken.Integer, this.ReadInt64());
				return;
			default:
				throw new ArgumentOutOfRangeException("type", "Unexpected BsonType value: " + type.ToString());
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00077BCC File Offset: 0x00075DCC
		private byte[] ReadBinary(out BsonBinaryType binaryType)
		{
			int count = this.ReadInt32();
			binaryType = (BsonBinaryType)this.ReadByte();
			if (binaryType == BsonBinaryType.BinaryOld && !this._jsonNet35BinaryCompatibility)
			{
				count = this.ReadInt32();
			}
			return this.ReadBytes(count);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00077C04 File Offset: 0x00075E04
		private string ReadString()
		{
			this.EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			int num4;
			for (;;)
			{
				int num3 = num2;
				byte b;
				while (num3 < 128 && (b = this._reader.ReadByte()) > 0)
				{
					this._byteBuffer[num3++] = b;
				}
				num4 = num3 - num2;
				num += num4;
				if (num3 < 128 && stringBuilder == null)
				{
					break;
				}
				int lastFullCharStop = this.GetLastFullCharStop(num3 - 1);
				int chars = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(256);
				}
				stringBuilder.Append(this._charBuffer, 0, chars);
				if (lastFullCharStop < num4 - 1)
				{
					num2 = num4 - lastFullCharStop - 1;
					Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, num2);
				}
				else
				{
					if (num3 < 128)
					{
						goto IL_105;
					}
					num2 = 0;
				}
			}
			int chars2 = Encoding.UTF8.GetChars(this._byteBuffer, 0, num4, this._charBuffer, 0);
			this.MovePosition(num + 1);
			return new string(this._charBuffer, 0, chars2);
			IL_105:
			this.MovePosition(num + 1);
			return stringBuilder.ToString();
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00077D28 File Offset: 0x00075F28
		private string ReadLengthString()
		{
			int num = this.ReadInt32();
			this.MovePosition(num);
			string @string = this.GetString(num - 1);
			this._reader.ReadByte();
			return @string;
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x00077D58 File Offset: 0x00075F58
		private string GetString(int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			this.EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			int num3;
			for (;;)
			{
				int count = (length - num > 128 - num2) ? (128 - num2) : (length - num);
				num3 = this._reader.Read(this._byteBuffer, num2, count);
				if (num3 == 0)
				{
					goto IL_E1;
				}
				num += num3;
				num3 += num2;
				if (num3 == length)
				{
					break;
				}
				int lastFullCharStop = this.GetLastFullCharStop(num3 - 1);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length);
				}
				int chars = Encoding.UTF8.GetChars(this._byteBuffer, 0, lastFullCharStop + 1, this._charBuffer, 0);
				stringBuilder.Append(this._charBuffer, 0, chars);
				if (lastFullCharStop < num3 - 1)
				{
					num2 = num3 - lastFullCharStop - 1;
					Array.Copy(this._byteBuffer, lastFullCharStop + 1, this._byteBuffer, 0, num2);
				}
				else
				{
					num2 = 0;
				}
				if (num >= length)
				{
					goto Block_6;
				}
			}
			int chars2 = Encoding.UTF8.GetChars(this._byteBuffer, 0, num3, this._charBuffer, 0);
			return new string(this._charBuffer, 0, chars2);
			Block_6:
			return stringBuilder.ToString();
			IL_E1:
			throw new EndOfStreamException("Unable to read beyond the end of the stream.");
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00077E84 File Offset: 0x00076084
		private int GetLastFullCharStop(int start)
		{
			int i = start;
			int num = 0;
			while (i >= 0)
			{
				num = this.BytesInSequence(this._byteBuffer[i]);
				if (num == 0)
				{
					i--;
				}
				else
				{
					if (num != 1)
					{
						i--;
					}
					IL_2A:
					if (num == start - i)
					{
						return start;
					}
					return i;
				}
			}
			goto IL_2A;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00077EC4 File Offset: 0x000760C4
		private int BytesInSequence(byte b)
		{
			if (b <= BsonReader.SeqRange1[1])
			{
				return 1;
			}
			if (b >= BsonReader.SeqRange2[0] && b <= BsonReader.SeqRange2[1])
			{
				return 2;
			}
			if (b >= BsonReader.SeqRange3[0] && b <= BsonReader.SeqRange3[1])
			{
				return 3;
			}
			if (b >= BsonReader.SeqRange4[0] && b <= BsonReader.SeqRange4[1])
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00077F20 File Offset: 0x00076120
		private void EnsureBuffers()
		{
			if (this._byteBuffer == null)
			{
				this._byteBuffer = new byte[128];
			}
			if (this._charBuffer == null)
			{
				int maxCharCount = Encoding.UTF8.GetMaxCharCount(128);
				this._charBuffer = new char[maxCharCount];
			}
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00018047 File Offset: 0x00016247
		private double ReadDouble()
		{
			this.MovePosition(8);
			return this._reader.ReadDouble();
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0001805B File Offset: 0x0001625B
		private int ReadInt32()
		{
			this.MovePosition(4);
			return this._reader.ReadInt32();
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0001806F File Offset: 0x0001626F
		private long ReadInt64()
		{
			this.MovePosition(8);
			return this._reader.ReadInt64();
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00018083 File Offset: 0x00016283
		private BsonType ReadType()
		{
			this.MovePosition(1);
			return (BsonType)this._reader.ReadSByte();
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00018097 File Offset: 0x00016297
		private void MovePosition(int count)
		{
			this._currentContext.Position += count;
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x000180AC File Offset: 0x000162AC
		private byte[] ReadBytes(int count)
		{
			this.MovePosition(count);
			return this._reader.ReadBytes(count);
		}

		// Token: 0x04000D16 RID: 3350
		private const int MaxCharBytesSize = 128;

		// Token: 0x04000D17 RID: 3351
		private static readonly byte[] SeqRange1 = new byte[]
		{
			0,
			127
		};

		// Token: 0x04000D18 RID: 3352
		private static readonly byte[] SeqRange2 = new byte[]
		{
			194,
			223
		};

		// Token: 0x04000D19 RID: 3353
		private static readonly byte[] SeqRange3 = new byte[]
		{
			224,
			239
		};

		// Token: 0x04000D1A RID: 3354
		private static readonly byte[] SeqRange4 = new byte[]
		{
			240,
			244
		};

		// Token: 0x04000D1B RID: 3355
		private readonly BinaryReader _reader;

		// Token: 0x04000D1C RID: 3356
		private readonly List<BsonReader.ContainerContext> _stack;

		// Token: 0x04000D1D RID: 3357
		private byte[] _byteBuffer;

		// Token: 0x04000D1E RID: 3358
		private char[] _charBuffer;

		// Token: 0x04000D1F RID: 3359
		private BsonType _currentElementType;

		// Token: 0x04000D20 RID: 3360
		private BsonReader.BsonReaderState _bsonReaderState;

		// Token: 0x04000D21 RID: 3361
		private BsonReader.ContainerContext _currentContext;

		// Token: 0x04000D22 RID: 3362
		private bool _readRootValueAsArray;

		// Token: 0x04000D23 RID: 3363
		private bool _jsonNet35BinaryCompatibility;

		// Token: 0x04000D24 RID: 3364
		private DateTimeKind _dateTimeKindHandling;

		// Token: 0x0200031C RID: 796
		private enum BsonReaderState
		{
			// Token: 0x04000D26 RID: 3366
			Normal,
			// Token: 0x04000D27 RID: 3367
			ReferenceStart,
			// Token: 0x04000D28 RID: 3368
			ReferenceRef,
			// Token: 0x04000D29 RID: 3369
			ReferenceId,
			// Token: 0x04000D2A RID: 3370
			CodeWScopeStart,
			// Token: 0x04000D2B RID: 3371
			CodeWScopeCode,
			// Token: 0x04000D2C RID: 3372
			CodeWScopeScope,
			// Token: 0x04000D2D RID: 3373
			CodeWScopeScopeObject,
			// Token: 0x04000D2E RID: 3374
			CodeWScopeScopeEnd
		}

		// Token: 0x0200031D RID: 797
		private class ContainerContext
		{
			// Token: 0x06001913 RID: 6419 RVA: 0x000180C1 File Offset: 0x000162C1
			public ContainerContext(BsonType type)
			{
				this.Type = type;
			}

			// Token: 0x04000D2F RID: 3375
			public readonly BsonType Type;

			// Token: 0x04000D30 RID: 3376
			public int Length;

			// Token: 0x04000D31 RID: 3377
			public int Position;
		}
	}
}
