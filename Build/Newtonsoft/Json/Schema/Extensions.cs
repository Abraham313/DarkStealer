using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200028F RID: 655
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public static class Extensions
	{
		// Token: 0x06001349 RID: 4937 RVA: 0x000679DC File Offset: 0x00065BDC
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static bool IsValid(this JToken source, JsonSchema schema)
		{
			bool valid = true;
			source.Validate(schema, delegate(object sender, ValidationEventArgs e)
			{
				valid = false;
			});
			return valid;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00067A10 File Offset: 0x00065C10
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static bool IsValid(this JToken source, JsonSchema schema, out IList<string> errorMessages)
		{
			IList<string> errors = new List<string>();
			source.Validate(schema, delegate(object sender, ValidationEventArgs e)
			{
				errors.Add(e.Message);
			});
			errorMessages = errors;
			return errorMessages.Count == 0;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00014764 File Offset: 0x00012964
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static void Validate(this JToken source, JsonSchema schema)
		{
			source.Validate(schema, null);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00067A54 File Offset: 0x00065C54
		[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
		public static void Validate(this JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			ValidationUtils.ArgumentNotNull(schema, "schema");
			using (JsonValidatingReader jsonValidatingReader = new JsonValidatingReader(source.CreateReader()))
			{
				jsonValidatingReader.Schema = schema;
				if (validationEventHandler != null)
				{
					jsonValidatingReader.ValidationEventHandler += validationEventHandler;
				}
				while (jsonValidatingReader.Read())
				{
				}
			}
		}
	}
}
