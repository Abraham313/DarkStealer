using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200014D RID: 333
	public interface IMegaApiClient
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060008F7 RID: 2295
		// (remove) Token: 0x060008F8 RID: 2296
		event EventHandler<ApiRequestFailedEventArgs> ApiRequestFailed;

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008F9 RID: 2297
		bool IsLoggedIn { get; }

		// Token: 0x060008FA RID: 2298
		MegaApiClient.LogonSessionToken Login(string email, string password);

		// Token: 0x060008FB RID: 2299
		MegaApiClient.LogonSessionToken Login(string email, string password, string mfaKey);

		// Token: 0x060008FC RID: 2300
		MegaApiClient.LogonSessionToken Login(MegaApiClient.AuthInfos authInfos);

		// Token: 0x060008FD RID: 2301
		void Login(MegaApiClient.LogonSessionToken logonSessionToken);

		// Token: 0x060008FE RID: 2302
		void Login();

		// Token: 0x060008FF RID: 2303
		void LoginAnonymous();

		// Token: 0x06000900 RID: 2304
		void Logout();

		// Token: 0x06000901 RID: 2305
		string GetRecoveryKey();

		// Token: 0x06000902 RID: 2306
		IAccountInformation GetAccountInformation();

		// Token: 0x06000903 RID: 2307
		IEnumerable<ISession> GetSessionsHistory();

		// Token: 0x06000904 RID: 2308
		IEnumerable<INode> GetNodes();

		// Token: 0x06000905 RID: 2309
		IEnumerable<INode> GetNodes(INode parent);

		// Token: 0x06000906 RID: 2310
		void Delete(INode node, bool moveToTrash = true);

		// Token: 0x06000907 RID: 2311
		INode CreateFolder(string name, INode parent);

		// Token: 0x06000908 RID: 2312
		Uri GetDownloadLink(INode node);

		// Token: 0x06000909 RID: 2313
		void DownloadFile(INode node, string outputFile, CancellationToken? cancellationToken = null);

		// Token: 0x0600090A RID: 2314
		void DownloadFile(Uri uri, string outputFile, CancellationToken? cancellationToken = null);

		// Token: 0x0600090B RID: 2315
		Stream Download(INode node, CancellationToken? cancellationToken = null);

		// Token: 0x0600090C RID: 2316
		Stream Download(Uri uri, CancellationToken? cancellationToken = null);

		// Token: 0x0600090D RID: 2317
		INodeInfo GetNodeFromLink(Uri uri);

		// Token: 0x0600090E RID: 2318
		IEnumerable<INode> GetNodesFromLink(Uri uri);

		// Token: 0x0600090F RID: 2319
		INode UploadFile(string filename, INode parent, CancellationToken? cancellationToken = null);

		// Token: 0x06000910 RID: 2320
		INode Upload(Stream stream, string name, INode parent, DateTime? modificationDate = null, CancellationToken? cancellationToken = null);

		// Token: 0x06000911 RID: 2321
		INode Move(INode node, INode destinationParentNode);

		// Token: 0x06000912 RID: 2322
		INode Rename(INode node, string newName);

		// Token: 0x06000913 RID: 2323
		MegaApiClient.AuthInfos GenerateAuthInfos(string email, string password, string mfaKey = null);
	}
}
