using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000232 RID: 562
	[NullableContext(2)]
	[Nullable(0)]
	internal class ReflectionMember
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x000120D9 File Offset: 0x000102D9
		// (set) Token: 0x06000FF6 RID: 4086 RVA: 0x000120E1 File Offset: 0x000102E1
		public Type MemberType { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x000120EA File Offset: 0x000102EA
		// (set) Token: 0x06000FF8 RID: 4088 RVA: 0x000120F2 File Offset: 0x000102F2
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		public Func<object, object> Getter { [return: Nullable(new byte[]
		{
			2,
			1,
			2
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			2
		})] set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x000120FB File Offset: 0x000102FB
		// (set) Token: 0x06000FFA RID: 4090 RVA: 0x00012103 File Offset: 0x00010303
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		public Action<object, object> Setter { [return: Nullable(new byte[]
		{
			2,
			1,
			2
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			2
		})] set; }
	}
}
