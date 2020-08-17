using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200016F RID: 367
	public class WebClient : IWebClient
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public WebClient(int responseTimeout = -1, string userAgent = null)
		{
			this.BufferSize = 65536;
			this.responseTimeout = responseTimeout;
			this.userAgent = (userAgent ?? this.GenerateUserAgent());
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0000DD83 File Offset: 0x0000BF83
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0000DD8B File Offset: 0x0000BF8B
		public int BufferSize { get; set; }

		// Token: 0x06000A17 RID: 2583 RVA: 0x0004D58C File Offset: 0x0004B78C
		public string PostRequestJson(Uri url, string jsonData)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(jsonData.ToBytes()))
			{
				result = this.PostRequest(url, memoryStream, "application/json");
			}
			return result;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000DD94 File Offset: 0x0000BF94
		public string PostRequestRaw(Uri url, Stream dataStream)
		{
			return this.PostRequest(url, dataStream, "application/octet-stream");
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000DDA3 File Offset: 0x0000BFA3
		public Stream GetRequestRaw(Uri url)
		{
			HttpWebRequest httpWebRequest = this.CreateRequest(url);
			httpWebRequest.Method = "GET";
			return httpWebRequest.GetResponse().GetResponseStream();
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0004D5D0 File Offset: 0x0004B7D0
		private string PostRequest(Uri url, Stream dataStream, string contentType)
		{
			HttpWebRequest httpWebRequest = this.CreateRequest(url);
			httpWebRequest.ContentLength = dataStream.Length;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = contentType;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				dataStream.Position = 0L;
				dataStream.CopyTo(requestStream, this.BufferSize);
			}
			string result;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000DDC1 File Offset: 0x0000BFC1
		private HttpWebRequest CreateRequest(Uri url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Timeout = this.responseTimeout;
			httpWebRequest.UserAgent = this.userAgent;
			return httpWebRequest;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0004D6B4 File Offset: 0x0004B8B4
		private string GenerateUserAgent()
		{
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();
			return string.Format("{0} v{1}", name.Name, name.Version.ToString(2));
		}

		// Token: 0x04000705 RID: 1797
		private const int DefaultResponseTimeout = -1;

		// Token: 0x04000706 RID: 1798
		private readonly int responseTimeout;

		// Token: 0x04000707 RID: 1799
		private readonly string userAgent;
	}
}
