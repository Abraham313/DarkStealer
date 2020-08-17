using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using CG.Web.MegaApiClient.Serialization;
using Medo.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000155 RID: 341
	public class MegaApiClient : IMegaApiClient
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x0000D52F File Offset: 0x0000B72F
		public MegaApiClient() : this(new Options("axhQiYyQ", true, null, 65536, 1048576), new WebClient(-1, null))
		{
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000D554 File Offset: 0x0000B754
		public MegaApiClient(Options options) : this(options, new WebClient(-1, null))
		{
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000D564 File Offset: 0x0000B764
		public MegaApiClient(IWebClient webClient) : this(new Options("axhQiYyQ", true, null, 65536, 1048576), webClient)
		{
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0004B3B4 File Offset: 0x000495B4
		public MegaApiClient(Options options, IWebClient webClient)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (webClient == null)
			{
				throw new ArgumentNullException("webClient");
			}
			this.options = options;
			this.webClient = webClient;
			this.webClient.BufferSize = options.BufferSize;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0004B428 File Offset: 0x00049628
		public MegaApiClient.AuthInfos GenerateAuthInfos(string email, string password, string mfaKey = null)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email");
			}
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password");
			}
			PreLoginRequest request = new PreLoginRequest(email);
			PreLoginResponse preLoginResponse = this.Request<PreLoginResponse>(request, null);
			if (preLoginResponse.Version == 2 && !string.IsNullOrEmpty(preLoginResponse.Salt))
			{
				byte[] salt = preLoginResponse.Salt.FromBase64();
				byte[] password2 = password.ToBytesPassword();
				byte[] array = new byte[32];
				using (HMACSHA512 hmacsha = new HMACSHA512())
				{
					array = new Pbkdf2(hmacsha, password2, salt, 100000).GetBytes(array.Length);
				}
				if (!string.IsNullOrEmpty(mfaKey))
				{
					return new MegaApiClient.AuthInfos(email, array.Skip(16).ToArray<byte>().ToBase64(), array.Take(16).ToArray<byte>(), mfaKey);
				}
				return new MegaApiClient.AuthInfos(email, array.Skip(16).ToArray<byte>().ToBase64(), array.Take(16).ToArray<byte>(), null);
			}
			else
			{
				if (preLoginResponse.Version != 1)
				{
					throw new NotSupportedException("Version of account not supported");
				}
				byte[] passwordAesKey = MegaApiClient.PrepareKey(password.ToBytesPassword());
				string hash = MegaApiClient.GenerateHash(email.ToLowerInvariant(), passwordAesKey);
				if (!string.IsNullOrEmpty(mfaKey))
				{
					return new MegaApiClient.AuthInfos(email, hash, passwordAesKey, mfaKey);
				}
				return new MegaApiClient.AuthInfos(email, hash, passwordAesKey, null);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000933 RID: 2355 RVA: 0x0004B58C File Offset: 0x0004978C
		// (remove) Token: 0x06000934 RID: 2356 RVA: 0x0004B5C4 File Offset: 0x000497C4
		public event EventHandler<ApiRequestFailedEventArgs> ApiRequestFailed;

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0000D583 File Offset: 0x0000B783
		public bool IsLoggedIn
		{
			get
			{
				return this.sessionId != null;
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0000D58E File Offset: 0x0000B78E
		public MegaApiClient.LogonSessionToken Login(string email, string password)
		{
			return this.Login(this.GenerateAuthInfos(email, password, null));
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0000D59F File Offset: 0x0000B79F
		public MegaApiClient.LogonSessionToken Login(string email, string password, string mfaKey)
		{
			return this.Login(this.GenerateAuthInfos(email, password, mfaKey));
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0004B5FC File Offset: 0x000497FC
		public MegaApiClient.LogonSessionToken Login(MegaApiClient.AuthInfos authInfos)
		{
			if (authInfos == null)
			{
				throw new ArgumentNullException("authInfos");
			}
			this.EnsureLoggedOut();
			this.authenticatedLogin = true;
			LoginRequest request;
			if (!string.IsNullOrEmpty(authInfos.MFAKey))
			{
				request = new LoginRequest(authInfos.Email, authInfos.Hash, authInfos.MFAKey);
			}
			else
			{
				request = new LoginRequest(authInfos.Email, authInfos.Hash);
			}
			LoginResponse loginResponse = this.Request<LoginResponse>(request, null);
			byte[] data = loginResponse.MasterKey.FromBase64();
			this.masterKey = Crypto.DecryptKey(data, authInfos.PasswordAesKey);
			BigInteger[] rsaPrivateKeyComponents = Crypto.GetRsaPrivateKeyComponents(loginResponse.PrivateKey.FromBase64(), this.masterKey);
			byte[] source = Crypto.RsaDecrypt(loginResponse.SessionId.FromBase64().FromMPINumber(), rsaPrivateKeyComponents[0], rsaPrivateKeyComponents[1], rsaPrivateKeyComponents[2]);
			this.sessionId = source.Take(43).ToArray<byte>().ToBase64();
			return new MegaApiClient.LogonSessionToken(this.sessionId, this.masterKey);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000D5B0 File Offset: 0x0000B7B0
		public void Login(MegaApiClient.LogonSessionToken logonSessionToken)
		{
			this.EnsureLoggedOut();
			this.authenticatedLogin = true;
			this.sessionId = logonSessionToken.SessionId;
			this.masterKey = logonSessionToken.MasterKey;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0000D5D7 File Offset: 0x0000B7D7
		public void Login()
		{
			this.LoginAnonymous();
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0004B6F0 File Offset: 0x000498F0
		public void LoginAnonymous()
		{
			this.EnsureLoggedOut();
			this.authenticatedLogin = false;
			Random random = new Random();
			this.masterKey = new byte[16];
			random.NextBytes(this.masterKey);
			byte[] array = new byte[16];
			random.NextBytes(array);
			byte[] array2 = new byte[16];
			random.NextBytes(array2);
			byte[] data = Crypto.EncryptAes(this.masterKey, array);
			byte[] array3 = Crypto.EncryptAes(array2, this.masterKey);
			byte[] array4 = new byte[32];
			Array.Copy(array2, 0, array4, 0, 16);
			Array.Copy(array3, 0, array4, 16, array3.Length);
			AnonymousLoginRequest request = new AnonymousLoginRequest(data.ToBase64(), array4.ToBase64());
			LoginRequest request2 = new LoginRequest(this.Request(request), null);
			LoginResponse loginResponse = this.Request<LoginResponse>(request2, null);
			this.sessionId = loginResponse.TemporarySessionId;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0000D5DF File Offset: 0x0000B7DF
		public void Logout()
		{
			this.EnsureLoggedIn();
			if (this.authenticatedLogin)
			{
				this.Request(new LogoutRequest());
			}
			this.masterKey = null;
			this.sessionId = null;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000D609 File Offset: 0x0000B809
		public string GetRecoveryKey()
		{
			this.EnsureLoggedIn();
			if (!this.authenticatedLogin)
			{
				throw new NotSupportedException("Anonymous login is not supported");
			}
			return this.masterKey.ToBase64();
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0004B7B8 File Offset: 0x000499B8
		public IAccountInformation GetAccountInformation()
		{
			this.EnsureLoggedIn();
			AccountInformationRequest request = new AccountInformationRequest();
			return this.Request<AccountInformationResponse>(request, null);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0004B7DC File Offset: 0x000499DC
		public IEnumerable<ISession> GetSessionsHistory()
		{
			this.EnsureLoggedIn();
			SessionHistoryRequest request = new SessionHistoryRequest();
			return this.Request<SessionHistoryResponse>(request, null);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0004B800 File Offset: 0x00049A00
		public IEnumerable<INode> GetNodes()
		{
			this.EnsureLoggedIn();
			GetNodesRequest request = new GetNodesRequest(null);
			Node[] nodes = this.Request<GetNodesResponse>(request, this.masterKey).Nodes;
			if (this.trashNode == null)
			{
				this.trashNode = nodes.First((Node n) => n.Type == NodeType.Trash);
			}
			return nodes.Distinct<Node>().OfType<INode>();
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0004B86C File Offset: 0x00049A6C
		public IEnumerable<INode> GetNodes(INode parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			return from n in this.GetNodes()
			where n.ParentId == parent.Id
			select n;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0004B8B0 File Offset: 0x00049AB0
		public void Delete(INode node, bool moveToTrash = true)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.Type != NodeType.Directory && node.Type != NodeType.File)
			{
				throw new ArgumentException("Invalid node type");
			}
			this.EnsureLoggedIn();
			if (moveToTrash)
			{
				this.Move(node, this.trashNode);
				return;
			}
			this.Request(new DeleteRequest(node));
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0004B90C File Offset: 0x00049B0C
		public INode CreateFolder(string name, INode parent)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (parent.Type == NodeType.File)
			{
				throw new ArgumentException("Invalid parent node");
			}
			this.EnsureLoggedIn();
			byte[] array = Crypto.CreateAesKey();
			byte[] data = Crypto.EncryptAttributes(new Attributes(name), array);
			byte[] data2 = Crypto.EncryptAes(array, this.masterKey);
			CreateNodeRequest request = CreateNodeRequest.CreateFolderNodeRequest(parent, data.ToBase64(), data2.ToBase64(), array);
			return this.Request<GetNodesResponse>(request, this.masterKey).Nodes[0];
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0004B9A0 File Offset: 0x00049BA0
		public Uri GetDownloadLink(INode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.Type != NodeType.File && node.Type != NodeType.Directory)
			{
				throw new ArgumentException("Invalid node");
			}
			this.EnsureLoggedIn();
			if (node.Type == NodeType.Directory)
			{
				this.Request(new ShareNodeRequest(node, this.masterKey, this.GetNodes()));
				node = this.GetNodes().First((INode x) => x.Equals(node));
			}
			INodeCrypto nodeCrypto = node as INodeCrypto;
			if (nodeCrypto == null)
			{
				throw new ArgumentException("node must implement INodeCrypto");
			}
			GetDownloadLinkRequest request = new GetDownloadLinkRequest(node);
			string arg = this.Request<string>(request, null);
			return new Uri(MegaApiClient.BaseUri, string.Format("/{0}/{1}#{2}", (node.Type == NodeType.Directory) ? "folder" : "file", arg, (node.Type == NodeType.Directory) ? nodeCrypto.SharedKey.ToBase64() : nodeCrypto.FullKey.ToBase64()));
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0004BAC8 File Offset: 0x00049CC8
		public void DownloadFile(INode node, string outputFile, CancellationToken? cancellationToken = null)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (string.IsNullOrEmpty(outputFile))
			{
				throw new ArgumentNullException("outputFile");
			}
			using (Stream stream = this.Download(node, cancellationToken))
			{
				this.SaveStream(stream, outputFile);
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0004BB24 File Offset: 0x00049D24
		public void DownloadFile(Uri uri, string outputFile, CancellationToken? cancellationToken = null)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (string.IsNullOrEmpty(outputFile))
			{
				throw new ArgumentNullException("outputFile");
			}
			using (Stream stream = this.Download(uri, cancellationToken))
			{
				this.SaveStream(stream, outputFile);
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0004BB88 File Offset: 0x00049D88
		public Stream Download(INode node, CancellationToken? cancellationToken = null)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.Type != NodeType.File)
			{
				throw new ArgumentException("Invalid node");
			}
			INodeCrypto nodeCrypto = node as INodeCrypto;
			if (nodeCrypto == null)
			{
				throw new ArgumentException("node must implement INodeCrypto");
			}
			this.EnsureLoggedIn();
			DownloadUrlRequest request = new DownloadUrlRequest(node);
			DownloadUrlResponse downloadUrlResponse = this.Request<DownloadUrlResponse>(request, null);
			Stream stream = new MegaAesCtrStreamDecrypter(new BufferedStream(this.webClient.GetRequestRaw(new Uri(downloadUrlResponse.Url))), downloadUrlResponse.Size, nodeCrypto.Key, nodeCrypto.Iv, nodeCrypto.MetaMac);
			if (cancellationToken != null)
			{
				stream = new CancellableStream(stream, cancellationToken.Value);
			}
			return stream;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0004BC34 File Offset: 0x00049E34
		public Stream Download(Uri uri, CancellationToken? cancellationToken = null)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.EnsureLoggedIn();
			string id;
			byte[] iv;
			byte[] expectedMetaMac;
			byte[] fileKey;
			this.GetPartsFromUri(uri, out id, out iv, out expectedMetaMac, out fileKey);
			DownloadUrlRequestFromId request = new DownloadUrlRequestFromId(id);
			DownloadUrlResponse downloadUrlResponse = this.Request<DownloadUrlResponse>(request, null);
			Stream stream = new MegaAesCtrStreamDecrypter(new BufferedStream(this.webClient.GetRequestRaw(new Uri(downloadUrlResponse.Url))), downloadUrlResponse.Size, fileKey, iv, expectedMetaMac);
			if (cancellationToken != null)
			{
				stream = new CancellableStream(stream, cancellationToken.Value);
			}
			return stream;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0004BCC8 File Offset: 0x00049EC8
		public INodeInfo GetNodeFromLink(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.EnsureLoggedIn();
			string id;
			byte[] array;
			byte[] array2;
			byte[] key;
			this.GetPartsFromUri(uri, out id, out array, out array2, out key);
			DownloadUrlRequestFromId request = new DownloadUrlRequestFromId(id);
			DownloadUrlResponse downloadResponse = this.Request<DownloadUrlResponse>(request, null);
			return new NodeInfo(id, downloadResponse, key);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0004BD1C File Offset: 0x00049F1C
		public IEnumerable<INode> GetNodesFromLink(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.EnsureLoggedIn();
			string shareId;
			byte[] array;
			byte[] array2;
			byte[] key;
			this.GetPartsFromUri(uri, out shareId, out array, out array2, out key);
			GetNodesRequest request = new GetNodesRequest(shareId);
			return (from x in this.Request<GetNodesResponse>(request, key).Nodes
			select new PublicNode(x, shareId)).OfType<INode>();
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0004BD90 File Offset: 0x00049F90
		public INode UploadFile(string filename, INode parent, CancellationToken? cancellationToken = null)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException("filename");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(filename);
			}
			this.EnsureLoggedIn();
			DateTime lastWriteTime = File.GetLastWriteTime(filename);
			INode result;
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				result = this.Upload(fileStream, Path.GetFileName(filename), parent, new DateTime?(lastWriteTime), cancellationToken);
			}
			return result;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0004BE18 File Offset: 0x0004A018
		public INode Upload(Stream stream, string name, INode parent, DateTime? modificationDate = null, CancellationToken? cancellationToken = null)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (parent.Type == NodeType.File)
			{
				throw new ArgumentException("Invalid parent node");
			}
			this.EnsureLoggedIn();
			if (cancellationToken != null)
			{
				stream = new CancellableStream(stream, cancellationToken.Value);
			}
			string text = string.Empty;
			int num = 0;
			TimeSpan retryDelay;
			while (this.options.ComputeApiRequestRetryWaitDelay(++num, out retryDelay))
			{
				UploadUrlRequest request = new UploadUrlRequest(stream.Length);
				UploadUrlResponse uploadUrlResponse = this.Request<UploadUrlResponse>(request, null);
				ApiResultCode apiResultCode = ApiResultCode.Ok;
				using (MegaAesCtrStreamCrypter megaAesCtrStreamCrypter = new MegaAesCtrStreamCrypter(stream))
				{
					long num2 = 0L;
					int[] array = this.ComputeChunksSizesToUpload(megaAesCtrStreamCrypter.ChunksPositions, megaAesCtrStreamCrypter.Length).ToArray<int>();
					Uri url = null;
					for (int i = 0; i < array.Length; i++)
					{
						text = string.Empty;
						int num3 = array[i];
						byte[] buffer = new byte[num3];
						megaAesCtrStreamCrypter.Read(buffer, 0, num3);
						using (MemoryStream memoryStream = new MemoryStream(buffer))
						{
							url = new Uri(uploadUrlResponse.Url + "/" + num2);
							num2 += (long)num3;
							try
							{
								text = this.webClient.PostRequestRaw(url, memoryStream);
								long num4;
								if (string.IsNullOrEmpty(text))
								{
									apiResultCode = ApiResultCode.Ok;
								}
								else if (text.FromBase64().Length != 27 && long.TryParse(text, out num4))
								{
									apiResultCode = (ApiResultCode)num4;
									break;
								}
							}
							catch (Exception exception)
							{
								apiResultCode = ApiResultCode.RequestFailedRetry;
								EventHandler<ApiRequestFailedEventArgs> apiRequestFailed = this.ApiRequestFailed;
								if (apiRequestFailed != null)
								{
									apiRequestFailed(this, new ApiRequestFailedEventArgs(url, num, retryDelay, apiResultCode, exception));
								}
								break;
							}
						}
					}
					if (apiResultCode == ApiResultCode.Ok)
					{
						byte[] data = Crypto.EncryptAttributes(new Attributes(name, stream, modificationDate), megaAesCtrStreamCrypter.FileKey);
						byte[] array2 = new byte[32];
						for (int j = 0; j < 8; j++)
						{
							array2[j] = (megaAesCtrStreamCrypter.FileKey[j] ^ megaAesCtrStreamCrypter.Iv[j]);
							array2[j + 16] = megaAesCtrStreamCrypter.Iv[j];
						}
						for (int k = 8; k < 16; k++)
						{
							array2[k] = (megaAesCtrStreamCrypter.FileKey[k] ^ megaAesCtrStreamCrypter.MetaMac[k - 8]);
							array2[k + 16] = megaAesCtrStreamCrypter.MetaMac[k - 8];
						}
						byte[] data2 = Crypto.EncryptKey(array2, this.masterKey);
						CreateNodeRequest request2 = CreateNodeRequest.CreateFileNodeRequest(parent, data.ToBase64(), data2.ToBase64(), array2, text);
						return this.Request<GetNodesResponse>(request2, this.masterKey).Nodes[0];
					}
					EventHandler<ApiRequestFailedEventArgs> apiRequestFailed2 = this.ApiRequestFailed;
					if (apiRequestFailed2 != null)
					{
						apiRequestFailed2(this, new ApiRequestFailedEventArgs(url, num, retryDelay, apiResultCode, text));
					}
					if (apiResultCode != ApiResultCode.RequestFailedRetry && apiResultCode != ApiResultCode.RequestFailedPermanetly)
					{
						if (apiResultCode != ApiResultCode.TooManyRequests)
						{
							throw new ApiException(apiResultCode);
						}
					}
					this.Wait(retryDelay);
					stream.Seek(0L, SeekOrigin.Begin);
				}
			}
			throw new UploadException(text);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0004C174 File Offset: 0x0004A374
		public INode Move(INode node, INode destinationParentNode)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (destinationParentNode == null)
			{
				throw new ArgumentNullException("destinationParentNode");
			}
			if (node.Type != NodeType.Directory && node.Type != NodeType.File)
			{
				throw new ArgumentException("Invalid node type");
			}
			if (destinationParentNode.Type == NodeType.File)
			{
				throw new ArgumentException("Invalid destination parent node");
			}
			this.EnsureLoggedIn();
			this.Request(new MoveRequest(node, destinationParentNode));
			return this.GetNodes().First((INode n) => n.Equals(node));
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0004C218 File Offset: 0x0004A418
		public INode Rename(INode node, string newName)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.Type != NodeType.Directory && node.Type != NodeType.File)
			{
				throw new ArgumentException("Invalid node type");
			}
			if (string.IsNullOrEmpty(newName))
			{
				throw new ArgumentNullException("newName");
			}
			INodeCrypto nodeCrypto = node as INodeCrypto;
			if (nodeCrypto == null)
			{
				throw new ArgumentException("node must implement INodeCrypto");
			}
			this.EnsureLoggedIn();
			byte[] data = Crypto.EncryptAttributes(new Attributes(newName, ((Node)node).Attributes), nodeCrypto.Key);
			this.Request(new RenameRequest(node, data.ToBase64()));
			return this.GetNodes().First((INode n) => n.Equals(node));
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0004C2F0 File Offset: 0x0004A4F0
		private static string GenerateHash(string email, byte[] passwordAesKey)
		{
			byte[] array = email.ToBytes();
			byte[] array2 = new byte[16];
			for (int i = 0; i < array.Length; i++)
			{
				byte[] array3 = array2;
				int num = i % 16;
				array3[num] ^= array[i];
			}
			using (ICryptoTransform cryptoTransform = Crypto.CreateAesEncryptor(passwordAesKey))
			{
				for (int j = 0; j < 16384; j++)
				{
					array2 = Crypto.EncryptAes(array2, cryptoTransform);
				}
			}
			byte[] array4 = new byte[8];
			Array.Copy(array2, 0, array4, 0, 4);
			Array.Copy(array2, 8, array4, 4, 4);
			return array4.ToBase64();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0004C390 File Offset: 0x0004A590
		private static byte[] PrepareKey(byte[] data)
		{
			byte[] array = new byte[]
			{
				147,
				196,
				103,
				227,
				125,
				176,
				199,
				164,
				209,
				190,
				63,
				129,
				1,
				82,
				203,
				86
			};
			for (int i = 0; i < 65536; i++)
			{
				for (int j = 0; j < data.Length; j += 16)
				{
					byte[] key = data.CopySubArray(16, j);
					array = Crypto.EncryptAes(array, key);
				}
			}
			return array;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0000D62F File Offset: 0x0000B82F
		private string Request(RequestBase request)
		{
			return this.Request<string>(request, null);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
		private TResponse Request<TResponse>(RequestBase request, byte[] key = null) where TResponse : class
		{
			if (this.options.SynchronizeApiRequests)
			{
				object obj = this.apiRequestLocker;
				TResponse result;
				lock (obj)
				{
					result = this.RequestCore<TResponse>(request, key);
				}
				return result;
			}
			return this.RequestCore<TResponse>(request, key);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0004C440 File Offset: 0x0004A640
		private TResponse RequestCore<TResponse>(RequestBase request, byte[] key) where TResponse : class
		{
			string jsonData = JsonConvert.SerializeObject(new object[]
			{
				request
			});
			Uri url = this.GenerateUrl(request.QueryArguments);
			object obj = null;
			int num = 0;
			TimeSpan retryDelay;
			while (this.options.ComputeApiRequestRetryWaitDelay(++num, out retryDelay))
			{
				string text = this.webClient.PostRequestJson(url, jsonData);
				if (!string.IsNullOrEmpty(text) && (obj = JsonConvert.DeserializeObject(text)) != null && !(obj is long) && (!(obj is JArray) || ((JArray)obj)[0].Type != JTokenType.Integer))
				{
					break;
				}
				ApiResultCode apiResultCode = (obj == null) ? ApiResultCode.RequestFailedRetry : ((obj is long) ? ((ApiResultCode)Enum.ToObject(typeof(ApiResultCode), obj)) : ((ApiResultCode)((JArray)obj)[0].Value<int>()));
				if (apiResultCode != ApiResultCode.Ok)
				{
					EventHandler<ApiRequestFailedEventArgs> apiRequestFailed = this.ApiRequestFailed;
					if (apiRequestFailed != null)
					{
						apiRequestFailed(this, new ApiRequestFailedEventArgs(url, num, retryDelay, apiResultCode, text));
					}
				}
				if (apiResultCode == ApiResultCode.RequestFailedRetry)
				{
					this.Wait(retryDelay);
				}
				else
				{
					if (apiResultCode != ApiResultCode.Ok)
					{
						throw new ApiException(apiResultCode);
					}
					break;
				}
			}
			string text2 = ((JArray)obj)[0].ToString();
			if (!(typeof(TResponse) == typeof(string)))
			{
				return JsonConvert.DeserializeObject<TResponse>(text2, new JsonConverter[]
				{
					new GetNodesResponseConverter(key)
				});
			}
			return text2 as TResponse;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0000D639 File Offset: 0x0000B839
		private void Wait(TimeSpan retryDelay)
		{
			Thread.Sleep(retryDelay);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0004C5A8 File Offset: 0x0004A7A8
		private Uri GenerateUrl(Dictionary<string, string> queryArguments)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(queryArguments);
			Dictionary<string, string> dictionary2 = dictionary;
			string key = "id";
			uint num = this.sequenceIndex;
			this.sequenceIndex = num + 1U;
			dictionary2[key] = (num % uint.MaxValue).ToString(CultureInfo.InvariantCulture);
			dictionary["ak"] = this.options.ApplicationKey;
			if (!string.IsNullOrEmpty(this.sessionId))
			{
				dictionary["sid"] = this.sessionId;
			}
			UriBuilder uriBuilder = new UriBuilder(MegaApiClient.BaseApiUri);
			string text = "";
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key,
					"=",
					keyValuePair.Value,
					"&"
				});
			}
			text = text.Substring(0, text.Length - 1);
			uriBuilder.Query = text.ToString();
			return uriBuilder.Uri;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0004C6C0 File Offset: 0x0004A8C0
		private void SaveStream(Stream stream, string outputFile)
		{
			using (FileStream fileStream = new FileStream(outputFile, FileMode.CreateNew, FileAccess.Write))
			{
				stream.CopyTo(fileStream, this.options.BufferSize);
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0000D641 File Offset: 0x0000B841
		private void EnsureLoggedIn()
		{
			if (this.sessionId == null)
			{
				throw new NotSupportedException("Not logged in");
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0000D656 File Offset: 0x0000B856
		private void EnsureLoggedOut()
		{
			if (this.sessionId != null)
			{
				throw new NotSupportedException("Already logged in");
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0004C704 File Offset: 0x0004A904
		private void GetPartsFromUri(Uri uri, out string id, out byte[] iv, out byte[] metaMac, out byte[] key)
		{
			byte[] array;
			bool flag;
			if (!this.TryGetPartsFromUri(uri, out id, out array, out flag) && !this.TryGetPartsFromLegacyUri(uri, out id, out array, out flag))
			{
				throw new ArgumentException(string.Format("Invalid uri. Unable to extract Id and Key from the uri {0}", uri));
			}
			if (flag)
			{
				iv = null;
				metaMac = null;
				key = array;
				return;
			}
			Crypto.GetPartsFromDecryptedKey(array, out iv, out metaMac, out key);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0004C758 File Offset: 0x0004A958
		private bool TryGetPartsFromUri(Uri uri, out string id, out byte[] decryptedKey, out bool isFolder)
		{
			Match match = new Regex("/(?<type>(file|folder))/(?<id>[^#]+)#(?<key>[^$/]+)").Match(uri.PathAndQuery + uri.Fragment);
			if (match.Success)
			{
				id = match.Groups["id"].Value;
				decryptedKey = match.Groups["key"].Value.FromBase64();
				isFolder = (match.Groups["type"].Value == "folder");
				return true;
			}
			id = null;
			decryptedKey = null;
			isFolder = false;
			return false;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0004C7F0 File Offset: 0x0004A9F0
		private bool TryGetPartsFromLegacyUri(Uri uri, out string id, out byte[] decryptedKey, out bool isFolder)
		{
			Match match = new Regex("#(?<type>F?)!(?<id>[^!]+)!(?<key>[^$!\\?]+)").Match(uri.Fragment);
			if (match.Success)
			{
				id = match.Groups["id"].Value;
				decryptedKey = match.Groups["key"].Value.FromBase64();
				isFolder = (match.Groups["type"].Value == "F");
				return true;
			}
			id = null;
			decryptedKey = null;
			isFolder = false;
			return false;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000D66B File Offset: 0x0000B86B
		private IEnumerable<int> ComputeChunksSizesToUpload(long[] chunksPositions, long streamLength)
		{
			MegaApiClient.<ComputeChunksSizesToUpload>d__58 <ComputeChunksSizesToUpload>d__ = new MegaApiClient.<ComputeChunksSizesToUpload>d__58(-2);
			<ComputeChunksSizesToUpload>d__.<>4__this = this;
			<ComputeChunksSizesToUpload>d__.<>3__chunksPositions = chunksPositions;
			<ComputeChunksSizesToUpload>d__.<>3__streamLength = streamLength;
			return <ComputeChunksSizesToUpload>d__;
		}

		// Token: 0x0400069E RID: 1694
		private static readonly Uri BaseApiUri = new Uri("https://g.api.mega.co.nz/cs");

		// Token: 0x0400069F RID: 1695
		private static readonly Uri BaseUri = new Uri("https://mega.nz");

		// Token: 0x040006A0 RID: 1696
		private readonly Options options;

		// Token: 0x040006A1 RID: 1697
		private readonly IWebClient webClient;

		// Token: 0x040006A2 RID: 1698
		private readonly object apiRequestLocker = new object();

		// Token: 0x040006A3 RID: 1699
		private Node trashNode;

		// Token: 0x040006A4 RID: 1700
		private string sessionId;

		// Token: 0x040006A5 RID: 1701
		private byte[] masterKey;

		// Token: 0x040006A6 RID: 1702
		private uint sequenceIndex = (uint)(4294967295.0 * new Random().NextDouble());

		// Token: 0x040006A7 RID: 1703
		private bool authenticatedLogin;

		// Token: 0x02000156 RID: 342
		public class AuthInfos
		{
			// Token: 0x0600095E RID: 2398 RVA: 0x0000D6A9 File Offset: 0x0000B8A9
			public AuthInfos(string email, string hash, byte[] passwordAesKey, string mfaKey = null)
			{
				this.Email = email;
				this.Hash = hash;
				this.PasswordAesKey = passwordAesKey;
				this.MFAKey = mfaKey;
			}

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x0600095F RID: 2399 RVA: 0x0000D6CE File Offset: 0x0000B8CE
			// (set) Token: 0x06000960 RID: 2400 RVA: 0x0000D6D6 File Offset: 0x0000B8D6
			[JsonProperty]
			public string Email { get; private set; }

			// Token: 0x170001B6 RID: 438
			// (get) Token: 0x06000961 RID: 2401 RVA: 0x0000D6DF File Offset: 0x0000B8DF
			// (set) Token: 0x06000962 RID: 2402 RVA: 0x0000D6E7 File Offset: 0x0000B8E7
			[JsonProperty]
			public string Hash { get; private set; }

			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x06000963 RID: 2403 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
			// (set) Token: 0x06000964 RID: 2404 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
			[JsonProperty]
			public byte[] PasswordAesKey { get; private set; }

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x06000965 RID: 2405 RVA: 0x0000D701 File Offset: 0x0000B901
			// (set) Token: 0x06000966 RID: 2406 RVA: 0x0000D709 File Offset: 0x0000B909
			[JsonProperty]
			public string MFAKey { get; private set; }
		}

		// Token: 0x02000157 RID: 343
		public class LogonSessionToken : IEquatable<MegaApiClient.LogonSessionToken>
		{
			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x06000967 RID: 2407 RVA: 0x0000D712 File Offset: 0x0000B912
			[JsonProperty]
			public string SessionId { get; }

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x06000968 RID: 2408 RVA: 0x0000D71A File Offset: 0x0000B91A
			[JsonProperty]
			public byte[] MasterKey { get; }

			// Token: 0x06000969 RID: 2409 RVA: 0x00008754 File Offset: 0x00006954
			private LogonSessionToken()
			{
			}

			// Token: 0x0600096A RID: 2410 RVA: 0x0000D722 File Offset: 0x0000B922
			public LogonSessionToken(string sessionId, byte[] masterKey)
			{
				this.SessionId = sessionId;
				this.MasterKey = masterKey;
			}

			// Token: 0x0600096B RID: 2411 RVA: 0x0004C880 File Offset: 0x0004AA80
			public bool Equals(MegaApiClient.LogonSessionToken other)
			{
				return other != null && (this.SessionId != null && other.SessionId != null && string.Compare(this.SessionId, other.SessionId) == 0) && (this.MasterKey != null && other.MasterKey != null && this.MasterKey.SequenceEqual(other.MasterKey));
			}
		}
	}
}
