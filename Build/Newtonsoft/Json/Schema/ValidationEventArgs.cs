using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020002A5 RID: 677
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x06001438 RID: 5176 RVA: 0x00015037 File Offset: 0x00013237
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00015051 File Offset: 0x00013251
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00015059 File Offset: 0x00013259
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00015066 File Offset: 0x00013266
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x04000BA3 RID: 2979
		private readonly JsonSchemaException _ex;
	}
}
