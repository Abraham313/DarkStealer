using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020002B5 RID: 693
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JContainer : JToken, IEnumerable, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IList, ICollection, ITypedList, IBindingList, INotifyCollectionChanged
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060014AC RID: 5292 RVA: 0x00015602 File Offset: 0x00013802
		// (remove) Token: 0x060014AD RID: 5293 RVA: 0x0001561B File Offset: 0x0001381B
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Combine(this._listChanged, value);
			}
			remove
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Remove(this._listChanged, value);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060014AE RID: 5294 RVA: 0x00015634 File Offset: 0x00013834
		// (remove) Token: 0x060014AF RID: 5295 RVA: 0x0001564D File Offset: 0x0001384D
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Combine(this._addingNew, value);
			}
			remove
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Remove(this._addingNew, value);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060014B0 RID: 5296 RVA: 0x00015666 File Offset: 0x00013866
		// (remove) Token: 0x060014B1 RID: 5297 RVA: 0x0001567F File Offset: 0x0001387F
		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(this._collectionChanged, value);
			}
			remove
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(this._collectionChanged, value);
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060014B2 RID: 5298
		protected abstract IList<JToken> ChildrenTokens { get; }

		// Token: 0x060014B3 RID: 5299 RVA: 0x00015698 File Offset: 0x00013898
		internal JContainer()
		{
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0006AB60 File Offset: 0x00068D60
		internal JContainer(JContainer other) : this()
		{
			ValidationUtils.ArgumentNotNull(other, "other");
			int num = 0;
			foreach (JToken content in ((IEnumerable<JToken>)other))
			{
				this.AddInternal(num, content, false);
				num++;
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000156A0 File Offset: 0x000138A0
		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000156C5 File Offset: 0x000138C5
		internal virtual IList<JToken> CreateChildrenCollection()
		{
			return new List<JToken>();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x000156CC File Offset: 0x000138CC
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNew = this._addingNew;
			if (addingNew == null)
			{
				return;
			}
			addingNew(this, e);
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0006ABC4 File Offset: 0x00068DC4
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			ListChangedEventHandler listChanged = this._listChanged;
			if (listChanged != null)
			{
				this._busy = true;
				try
				{
					listChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0006AC04 File Offset: 0x00068E04
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler collectionChanged = this._collectionChanged;
			if (collectionChanged != null)
			{
				this._busy = true;
				try
				{
					collectionChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x000156E0 File Offset: 0x000138E0
		public override bool HasValues
		{
			get
			{
				return this.ChildrenTokens.Count > 0;
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0006AC44 File Offset: 0x00068E44
		internal bool ContentsEqual(JContainer container)
		{
			if (container == this)
			{
				return true;
			}
			IList<JToken> childrenTokens = this.ChildrenTokens;
			IList<JToken> childrenTokens2 = container.ChildrenTokens;
			if (childrenTokens.Count != childrenTokens2.Count)
			{
				return false;
			}
			for (int i = 0; i < childrenTokens.Count; i++)
			{
				if (!childrenTokens[i].DeepEquals(childrenTokens2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060014BC RID: 5308 RVA: 0x0006ACA0 File Offset: 0x00068EA0
		[Nullable(2)]
		public override JToken First
		{
			[NullableContext(2)]
			get
			{
				IList<JToken> childrenTokens = this.ChildrenTokens;
				if (childrenTokens.Count <= 0)
				{
					return null;
				}
				return childrenTokens[0];
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0006ACC8 File Offset: 0x00068EC8
		[Nullable(2)]
		public override JToken Last
		{
			[NullableContext(2)]
			get
			{
				IList<JToken> childrenTokens = this.ChildrenTokens;
				int count = childrenTokens.Count;
				if (count <= 0)
				{
					return null;
				}
				return childrenTokens[count - 1];
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x000156F0 File Offset: 0x000138F0
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenTokens);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x000156FD File Offset: 0x000138FD
		public override IEnumerable<T> Values<[Nullable(2)] T>()
		{
			return this.ChildrenTokens.Convert<JToken, T>();
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0001570A File Offset: 0x0001390A
		public IEnumerable<JToken> Descendants()
		{
			return this.GetDescendants(false);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00015713 File Offset: 0x00013913
		public IEnumerable<JToken> DescendantsAndSelf()
		{
			return this.GetDescendants(true);
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0001571C File Offset: 0x0001391C
		internal IEnumerable<JToken> GetDescendants(bool self)
		{
			JContainer.<GetDescendants>d__34 <GetDescendants>d__ = new JContainer.<GetDescendants>d__34(-2);
			<GetDescendants>d__.<>4__this = this;
			<GetDescendants>d__.<>3__self = self;
			return <GetDescendants>d__;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00015733 File Offset: 0x00013933
		[NullableContext(2)]
		internal bool IsMultiContent([NotNull] object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0001575B File Offset: 0x0001395B
		internal JToken EnsureParentToken([Nullable(2)] JToken item, bool skipParentCheck)
		{
			if (item == null)
			{
				return JValue.CreateNull();
			}
			if (skipParentCheck)
			{
				return item;
			}
			if (item.Parent != null || item == this || (item.HasValues && base.Root == item))
			{
				item = item.CloneToken();
			}
			return item;
		}

		// Token: 0x060014C5 RID: 5317
		[NullableContext(2)]
		internal abstract int IndexOfItem(JToken item);

		// Token: 0x060014C6 RID: 5318 RVA: 0x0006ACF4 File Offset: 0x00068EF4
		[NullableContext(2)]
		internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index > childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, skipParentCheck);
			JToken jtoken = (index == 0) ? null : childrenTokens[index - 1];
			JToken jtoken2 = (index == childrenTokens.Count) ? null : childrenTokens[index];
			this.ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jtoken;
			if (jtoken != null)
			{
				jtoken.Next = item;
			}
			item.Next = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Previous = item;
			}
			childrenTokens.Insert(index, item);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0006ADBC File Offset: 0x00068FBC
		internal virtual void RemoveItemAt(int index)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			this.CheckReentrancy();
			JToken jtoken = childrenTokens[index];
			JToken jtoken2 = (index == 0) ? null : childrenTokens[index - 1];
			JToken jtoken3 = (index == childrenTokens.Count - 1) ? null : childrenTokens[index + 1];
			if (jtoken2 != null)
			{
				jtoken2.Next = jtoken3;
			}
			if (jtoken3 != null)
			{
				jtoken3.Previous = jtoken2;
			}
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			childrenTokens.RemoveAt(index);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, jtoken, index));
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0006AE90 File Offset: 0x00069090
		[NullableContext(2)]
		internal virtual bool RemoveItem(JToken item)
		{
			if (item != null)
			{
				int num = this.IndexOfItem(item);
				if (num >= 0)
				{
					this.RemoveItemAt(num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00015791 File Offset: 0x00013991
		internal virtual JToken GetItem(int index)
		{
			return this.ChildrenTokens[index];
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0006AEB8 File Offset: 0x000690B8
		[NullableContext(2)]
		internal virtual void SetItem(int index, JToken item)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jtoken = childrenTokens[index];
			if (JContainer.IsTokenUnchanged(jtoken, item))
			{
				return;
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, false);
			this.ValidateToken(item, jtoken);
			JToken jtoken2 = (index == 0) ? null : childrenTokens[index - 1];
			JToken jtoken3 = (index == childrenTokens.Count - 1) ? null : childrenTokens[index + 1];
			item.Parent = this;
			item.Previous = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Next = item;
			}
			item.Next = jtoken3;
			if (jtoken3 != null)
			{
				jtoken3.Previous = item;
			}
			childrenTokens[index] = item;
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, jtoken, index));
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0006AFC0 File Offset: 0x000691C0
		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			IList<JToken> childrenTokens = this.ChildrenTokens;
			foreach (JToken jtoken in childrenTokens)
			{
				jtoken.Parent = null;
				jtoken.Previous = null;
				jtoken.Next = null;
			}
			childrenTokens.Clear();
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0006B054 File Offset: 0x00069254
		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing != null)
			{
				if (existing.Parent == this)
				{
					int index = this.IndexOfItem(existing);
					this.SetItem(index, replacement);
					return;
				}
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0001579F File Offset: 0x0001399F
		[NullableContext(2)]
		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0006B080 File Offset: 0x00069280
		internal virtual void CopyItemsTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken value in this.ChildrenTokens)
			{
				array.SetValue(value, arrayIndex + num);
				num++;
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0006B12C File Offset: 0x0006932C
		internal static bool IsTokenUnchanged(JToken currentValue, [Nullable(2)] JToken newValue)
		{
			JValue jvalue = currentValue as JValue;
			if (jvalue == null)
			{
				return false;
			}
			if (newValue == null)
			{
				return jvalue.Type == JTokenType.Null;
			}
			return jvalue.Equals(newValue);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000157AE File Offset: 0x000139AE
		internal virtual void ValidateToken(JToken o, [Nullable(2)] JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000157E5 File Offset: 0x000139E5
		[NullableContext(2)]
		public virtual void Add(object content)
		{
			this.AddInternal(this.ChildrenTokens.Count, content, false);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000157FA File Offset: 0x000139FA
		internal void AddAndSkipParentCheck(JToken token)
		{
			this.AddInternal(this.ChildrenTokens.Count, token, true);
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0001580F File Offset: 0x00013A0F
		[NullableContext(2)]
		public void AddFirst(object content)
		{
			this.AddInternal(0, content, false);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0006B15C File Offset: 0x0006935C
		[NullableContext(2)]
		internal void AddInternal(int index, object content, bool skipParentCheck)
		{
			if (this.IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object content2 = enumerator.Current;
						this.AddInternal(num, content2, skipParentCheck);
						num++;
					}
					return;
				}
			}
			JToken item = JContainer.CreateFromContent(content);
			this.InsertItem(index, item, skipParentCheck);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0006B1D4 File Offset: 0x000693D4
		internal static JToken CreateFromContent([Nullable(2)] object content)
		{
			JToken jtoken = content as JToken;
			if (jtoken != null)
			{
				return jtoken;
			}
			return new JValue(content);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0001581A File Offset: 0x00013A1A
		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00015822 File Offset: 0x00013A22
		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000154F1 File Offset: 0x000136F1
		public void RemoveAll()
		{
			this.ClearItems();
		}

		// Token: 0x060014D9 RID: 5337
		internal abstract void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings);

		// Token: 0x060014DA RID: 5338 RVA: 0x00015831 File Offset: 0x00013A31
		public void Merge(object content)
		{
			this.MergeItem(content, null);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0001583B File Offset: 0x00013A3B
		public void Merge(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			this.MergeItem(content, settings);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0006B1F4 File Offset: 0x000693F4
		internal void ReadTokenFrom(JsonReader reader, [Nullable(2)] JsonLoadSettings options)
		{
			int depth = reader.Depth;
			if (!reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
			this.ReadContentFrom(reader, options);
			if (reader.Depth > depth)
			{
				throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0006B264 File Offset: 0x00069464
		internal void ReadContentFrom(JsonReader r, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jcontainer = this;
			for (;;)
			{
				JProperty jproperty = jcontainer as JProperty;
				if (jproperty != null && jproperty.Value != null)
				{
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_1C2;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jobject);
					jcontainer = jobject;
					goto IL_1C2;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jarray);
					jcontainer = jarray;
					goto IL_1C2;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(r.Value.ToString());
					jconstructor.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jconstructor);
					jcontainer = jconstructor;
					goto IL_1C2;
				}
				case JsonToken.PropertyName:
				{
					JProperty jproperty2 = JContainer.ReadProperty(r, settings, lineInfo, jcontainer);
					if (jproperty2 != null)
					{
						jcontainer = jproperty2;
						goto IL_1C2;
					}
					r.Skip();
					goto IL_1C2;
				}
				case JsonToken.Comment:
					if (settings != null && settings.CommentHandling == CommentHandling.Load)
					{
						JValue jvalue = JValue.CreateComment(r.Value.ToString());
						jvalue.SetLineInfo(lineInfo, settings);
						jcontainer.Add(jvalue);
						goto IL_1C2;
					}
					goto IL_1C2;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jvalue = new JValue(r.Value);
					jvalue.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_1C2;
				}
				case JsonToken.Null:
				{
					JValue jvalue = JValue.CreateNull();
					jvalue.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_1C2;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = JValue.CreateUndefined();
					jvalue.SetLineInfo(lineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_1C2;
				}
				case JsonToken.EndObject:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_1C2;
				case JsonToken.EndArray:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_1C2;
				case JsonToken.EndConstructor:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_1C2;
				}
				break;
				IL_1C2:
				if (!r.Read())
				{
					return;
				}
			}
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, r.TokenType));
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0006B490 File Offset: 0x00069690
		[NullableContext(2)]
		private static JProperty ReadProperty([Nullable(1)] JsonReader r, JsonLoadSettings settings, IJsonLineInfo lineInfo, [Nullable(1)] JContainer parent)
		{
			DuplicatePropertyNameHandling duplicatePropertyNameHandling = (settings != null) ? settings.DuplicatePropertyNameHandling : DuplicatePropertyNameHandling.Replace;
			JObject jobject = (JObject)parent;
			string text = r.Value.ToString();
			JProperty jproperty = jobject.Property(text, StringComparison.Ordinal);
			if (jproperty != null)
			{
				if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Ignore)
				{
					return null;
				}
				if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Error)
				{
					throw JsonReaderException.Create(r, "Property with the name '{0}' already exists in the current JSON object.".FormatWith(CultureInfo.InvariantCulture, text));
				}
			}
			JProperty jproperty2 = new JProperty(text);
			jproperty2.SetLineInfo(lineInfo, settings);
			if (jproperty == null)
			{
				parent.Add(jproperty2);
			}
			else
			{
				jproperty.Replace(jproperty2);
			}
			return jproperty2;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0006B50C File Offset: 0x0006970C
		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				num ^= jtoken.GetDeepHashCode();
			}
			return num;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00015845 File Offset: 0x00013A45
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0001584C File Offset: 0x00013A4C
		[return: Nullable(2)]
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			ICustomTypeDescriptor customTypeDescriptor = this.First as ICustomTypeDescriptor;
			if (customTypeDescriptor == null)
			{
				return null;
			}
			return customTypeDescriptor.GetProperties();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x000154CB File Offset: 0x000136CB
		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x000154D4 File Offset: 0x000136D4
		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000154DF File Offset: 0x000136DF
		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x17000430 RID: 1072
		JToken IList<JToken>.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000154E8 File Offset: 0x000136E8
		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000154F1 File Offset: 0x000136F1
		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000154F9 File Offset: 0x000136F9
		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00015502 File Offset: 0x00013702
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x00009021 File Offset: 0x00007221
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0001550C File Offset: 0x0001370C
		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0006B560 File Offset: 0x00069760
		[return: Nullable(2)]
		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Argument is not a JToken.");
			}
			return jtoken;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00015864 File Offset: 0x00013A64
		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.Count - 1;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000154F1 File Offset: 0x000136F1
		void IList.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0001587B File Offset: 0x00013A7B
		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0001588A File Offset: 0x00013A8A
		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00015899 File Offset: 0x00013A99
		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value), false);
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00009021 File Offset: 0x00007221
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x00009021 File Offset: 0x00007221
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000158AA File Offset: 0x00013AAA
		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x000154DF File Offset: 0x000136DF
		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x17000434 RID: 1076
		object IList.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00015502 File Offset: 0x00013702
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x000158CA File Offset: 0x00013ACA
		public int Count
		{
			get
			{
				return this.ChildrenTokens.Count;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x00009021 File Offset: 0x00007221
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x000158D7 File Offset: 0x00013AD7
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00009B58 File Offset: 0x00007D58
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0006B588 File Offset: 0x00069788
		object IBindingList.AddNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			this.OnAddingNew(addingNewEventArgs);
			if (addingNewEventArgs.NewObject == null)
			{
				throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			JToken jtoken = addingNewEventArgs.NewObject as JToken;
			if (jtoken == null)
			{
				throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JToken)));
			}
			this.Add(jtoken);
			return jtoken;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x00009F16 File Offset: 0x00008116
		bool IBindingList.AllowEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x00009F16 File Offset: 0x00008116
		bool IBindingList.AllowNew
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x00009F16 File Offset: 0x00008116
		bool IBindingList.AllowRemove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0000983A File Offset: 0x00007A3A
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0000983A File Offset: 0x00007A3A
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00009021 File Offset: 0x00007221
		bool IBindingList.IsSorted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00009B58 File Offset: 0x00007D58
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0000983A File Offset: 0x00007A3A
		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x00009021 File Offset: 0x00007221
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x000158F9 File Offset: 0x00013AF9
		[Nullable(2)]
		PropertyDescriptor IBindingList.SortProperty
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x00009F16 File Offset: 0x00008116
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x00009021 File Offset: 0x00007221
		bool IBindingList.SupportsSearching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x00009021 File Offset: 0x00007221
		bool IBindingList.SupportsSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0006B5FC File Offset: 0x000697FC
		internal static void MergeEnumerableContent(JContainer target, IEnumerable content, [Nullable(2)] JsonMergeSettings settings)
		{
			if (settings != null)
			{
				switch (settings.MergeArrayHandling)
				{
				case MergeArrayHandling.Concat:
					break;
				case MergeArrayHandling.Union:
					goto IL_6B;
				case MergeArrayHandling.Replace:
					goto IL_C0;
				case MergeArrayHandling.Merge:
					goto IL_10A;
				default:
					throw new ArgumentOutOfRangeException("settings", "Unexpected merge array handling when merging JSON.");
				}
			}
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JToken content2 = (JToken)obj;
					target.Add(content2);
				}
				return;
			}
			IL_6B:
			HashSet<JToken> hashSet = new HashSet<JToken>(target, JToken.EqualityComparer);
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					JToken jtoken = (JToken)obj2;
					if (hashSet.Add(jtoken))
					{
						target.Add(jtoken);
					}
				}
				return;
			}
			IL_C0:
			if (target == content)
			{
				return;
			}
			target.ClearItems();
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj3 = enumerator.Current;
					JToken content3 = (JToken)obj3;
					target.Add(content3);
				}
				return;
			}
			IL_10A:
			int num = 0;
			foreach (object obj4 in content)
			{
				if (num < target.Count)
				{
					JContainer jcontainer = target[num] as JContainer;
					if (jcontainer != null)
					{
						jcontainer.Merge(obj4, settings);
					}
					else if (obj4 != null)
					{
						JToken jtoken2 = JContainer.CreateFromContent(obj4);
						if (jtoken2.Type != JTokenType.Null)
						{
							target[num] = jtoken2;
						}
					}
				}
				else
				{
					target.Add(obj4);
				}
				num++;
			}
		}

		// Token: 0x04000BC9 RID: 3017
		[Nullable(2)]
		internal ListChangedEventHandler _listChanged;

		// Token: 0x04000BCA RID: 3018
		[Nullable(2)]
		internal AddingNewEventHandler _addingNew;

		// Token: 0x04000BCB RID: 3019
		[Nullable(2)]
		internal NotifyCollectionChangedEventHandler _collectionChanged;

		// Token: 0x04000BCC RID: 3020
		[Nullable(2)]
		private object _syncRoot;

		// Token: 0x04000BCD RID: 3021
		private bool _busy;
	}
}
