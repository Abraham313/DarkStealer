using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020002A4 RID: 676
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum UndefinedSchemaIdHandling
	{
		// Token: 0x04000BA0 RID: 2976
		None,
		// Token: 0x04000BA1 RID: 2977
		UseTypeName,
		// Token: 0x04000BA2 RID: 2978
		UseAssemblyQualifiedName
	}
}
