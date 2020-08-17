using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002CD RID: 717
	[NullableContext(1)]
	[Nullable(0)]
	public class JTokenReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x000166BA File Offset: 0x000148BA
		[Nullable(2)]
		public JToken CurrentToken
		{
			[NullableContext(2)]
			get
			{
				return this._current;
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000166C2 File Offset: 0x000148C2
		public JTokenReader(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			this._root = token;
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000166DC File Offset: 0x000148DC
		public JTokenReader(JToken token, string initialPath) : this(token)
		{
			this._initialPath = initialPath;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0006ED90 File Offset: 0x0006CF90
		public override bool Read()
		{
			if (base.CurrentState != JsonReader.State.Start)
			{
				if (this._current == null)
				{
					return false;
				}
				JContainer jcontainer = this._current as JContainer;
				if (jcontainer != null && this._parent != jcontainer)
				{
					return this.ReadInto(jcontainer);
				}
				return this.ReadOver(this._current);
			}
			else
			{
				if (this._current == this._root)
				{
					return false;
				}
				this._current = this._root;
				this.SetToken(this._current);
				return true;
			}
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x0006EE08 File Offset: 0x0006D008
		private bool ReadOver(JToken t)
		{
			if (t == this._root)
			{
				return this.ReadToEnd();
			}
			JToken next = t.Next;
			if (next != null && next != t)
			{
				if (t != t.Parent.Last)
				{
					this._current = next;
					this.SetToken(this._current);
					return true;
				}
			}
			if (t.Parent == null)
			{
				return this.ReadToEnd();
			}
			return this.SetEnd(t.Parent);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000166EC File Offset: 0x000148EC
		private bool ReadToEnd()
		{
			this._current = null;
			base.SetToken(JsonToken.None);
			return false;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0006EE74 File Offset: 0x0006D074
		private JsonToken? GetEndToken(JContainer c)
		{
			switch (c.Type)
			{
			case JTokenType.Object:
				return new JsonToken?(JsonToken.EndObject);
			case JTokenType.Array:
				return new JsonToken?(JsonToken.EndArray);
			case JTokenType.Constructor:
				return new JsonToken?(JsonToken.EndConstructor);
			case JTokenType.Property:
				return null;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x0006EEDC File Offset: 0x0006D0DC
		private bool ReadInto(JContainer c)
		{
			JToken first = c.First;
			if (first == null)
			{
				return this.SetEnd(c);
			}
			this.SetToken(first);
			this._current = first;
			this._parent = c;
			return true;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0006EF14 File Offset: 0x0006D114
		private bool SetEnd(JContainer c)
		{
			JsonToken? endToken = this.GetEndToken(c);
			if (endToken != null)
			{
				base.SetToken(endToken.GetValueOrDefault());
				this._current = c;
				this._parent = c;
				return true;
			}
			return this.ReadOver(c);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0006EF58 File Offset: 0x0006D158
		private void SetToken(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Object:
				base.SetToken(JsonToken.StartObject);
				return;
			case JTokenType.Array:
				base.SetToken(JsonToken.StartArray);
				return;
			case JTokenType.Constructor:
				base.SetToken(JsonToken.StartConstructor, ((JConstructor)token).Name);
				return;
			case JTokenType.Property:
				base.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
				return;
			case JTokenType.Comment:
				base.SetToken(JsonToken.Comment, ((JValue)token).Value);
				return;
			case JTokenType.Integer:
				base.SetToken(JsonToken.Integer, ((JValue)token).Value);
				return;
			case JTokenType.Float:
				base.SetToken(JsonToken.Float, ((JValue)token).Value);
				return;
			case JTokenType.String:
				base.SetToken(JsonToken.String, ((JValue)token).Value);
				return;
			case JTokenType.Boolean:
				base.SetToken(JsonToken.Boolean, ((JValue)token).Value);
				return;
			case JTokenType.Null:
				base.SetToken(JsonToken.Null, ((JValue)token).Value);
				return;
			case JTokenType.Undefined:
				base.SetToken(JsonToken.Undefined, ((JValue)token).Value);
				return;
			case JTokenType.Date:
			{
				object obj = ((JValue)token).Value;
				if (obj is DateTime)
				{
					DateTime value = (DateTime)obj;
					obj = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
				}
				base.SetToken(JsonToken.Date, obj);
				return;
			}
			case JTokenType.Raw:
				base.SetToken(JsonToken.Raw, ((JValue)token).Value);
				return;
			case JTokenType.Bytes:
				base.SetToken(JsonToken.Bytes, ((JValue)token).Value);
				return;
			case JTokenType.Guid:
				base.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			case JTokenType.Uri:
			{
				object value2 = ((JValue)token).Value;
				JsonToken newToken = JsonToken.String;
				Uri uri = value2 as Uri;
				base.SetToken(newToken, (uri != null) ? uri.OriginalString : this.SafeToString(value2));
				return;
			}
			case JTokenType.TimeSpan:
				base.SetToken(JsonToken.String, this.SafeToString(((JValue)token).Value));
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x000166FD File Offset: 0x000148FD
		[NullableContext(2)]
		private string SafeToString(object value)
		{
			if (value == null)
			{
				return null;
			}
			return value.ToString();
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0006F154 File Offset: 0x0006D354
		bool IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				return false;
			}
			IJsonLineInfo current = this._current;
			return current != null && current.HasLineInfo();
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x0006F180 File Offset: 0x0006D380
		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo current = this._current;
				if (current != null)
				{
					return current.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0006F1AC File Offset: 0x0006D3AC
		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo current = this._current;
				if (current != null)
				{
					return current.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x0006F1D8 File Offset: 0x0006D3D8
		public override string Path
		{
			get
			{
				string text = base.Path;
				if (this._initialPath == null)
				{
					this._initialPath = this._root.Path;
				}
				if (!StringUtils.IsNullOrEmpty(this._initialPath))
				{
					if (StringUtils.IsNullOrEmpty(text))
					{
						return this._initialPath;
					}
					if (text.StartsWith('['))
					{
						text = this._initialPath + text;
					}
					else
					{
						text = this._initialPath + "." + text;
					}
				}
				return text;
			}
		}

		// Token: 0x04000C22 RID: 3106
		private readonly JToken _root;

		// Token: 0x04000C23 RID: 3107
		[Nullable(2)]
		private string _initialPath;

		// Token: 0x04000C24 RID: 3108
		[Nullable(2)]
		private JToken _parent;

		// Token: 0x04000C25 RID: 3109
		[Nullable(2)]
		private JToken _current;
	}
}
