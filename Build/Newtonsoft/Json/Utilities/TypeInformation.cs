using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020001EC RID: 492
	[NullableContext(1)]
	[Nullable(0)]
	internal class TypeInformation
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00011090 File Offset: 0x0000F290
		public Type Type { get; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00011098 File Offset: 0x0000F298
		public PrimitiveTypeCode TypeCode { get; }

		// Token: 0x06000E74 RID: 3700 RVA: 0x000110A0 File Offset: 0x0000F2A0
		public TypeInformation(Type type, PrimitiveTypeCode typeCode)
		{
			this.Type = type;
			this.TypeCode = typeCode;
		}
	}
}
