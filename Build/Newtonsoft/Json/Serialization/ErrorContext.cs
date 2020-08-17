using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200025B RID: 603
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorContext
	{
		// Token: 0x0600110F RID: 4367 RVA: 0x00012B4B File Offset: 0x00010D4B
		internal ErrorContext([Nullable(2)] object originalObject, [Nullable(2)] object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00012B70 File Offset: 0x00010D70
		// (set) Token: 0x06001111 RID: 4369 RVA: 0x00012B78 File Offset: 0x00010D78
		internal bool Traced { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00012B81 File Offset: 0x00010D81
		public Exception Error { get; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00012B89 File Offset: 0x00010D89
		[Nullable(2)]
		public object OriginalObject { [NullableContext(2)] get; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00012B91 File Offset: 0x00010D91
		[Nullable(2)]
		public object Member { [NullableContext(2)] get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00012B99 File Offset: 0x00010D99
		public string Path { get; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00012BA1 File Offset: 0x00010DA1
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x00012BA9 File Offset: 0x00010DA9
		public bool Handled { get; set; }
	}
}
