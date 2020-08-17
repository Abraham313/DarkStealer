using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ionic.Zlib;
using Microsoft.CSharp;

namespace Ionic.Zip
{
	// Token: 0x020000EF RID: 239
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
	[ComVisible(true)]
	public class ZipFile : IEnumerable<ZipEntry>, IEnumerable, IDisposable
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x0000B079 File Offset: 0x00009279
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return this.AddItem(fileOrDirectoryName, null);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000B083 File Offset: 0x00009283
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (!Directory.Exists(fileOrDirectoryName))
			{
				throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
			}
			return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000B0B7 File Offset: 0x000092B7
		public ZipEntry AddFile(string fileName)
		{
			return this.AddFile(fileName, null);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00035710 File Offset: 0x00033910
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00035750 File Offset: 0x00033950
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry entry in entriesToRemove)
			{
				this.RemoveEntry(entry);
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000357A8 File Offset: 0x000339A8
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string fileName in entriesToRemove)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000B0C1 File Offset: 0x000092C1
		public void AddFiles(IEnumerable<string> fileNames)
		{
			this.AddFiles(fileNames, null);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000B0CB File Offset: 0x000092CB
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			this.UpdateFiles(fileNames, null);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000B0D5 File Offset: 0x000092D5
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			this.AddFiles(fileNames, false, directoryPathInArchive);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00035800 File Offset: 0x00033A00
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this._addOperationCanceled = false;
			this.OnAddStarted();
			if (preserveDirHierarchy)
			{
				using (IEnumerator<string> enumerator = fileNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (this._addOperationCanceled)
						{
							break;
						}
						if (directoryPathInArchive != null)
						{
							string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(text)));
							this.AddFile(text, fullPath);
						}
						else
						{
							this.AddFile(text, null);
						}
					}
					goto IL_AC;
				}
			}
			foreach (string fileName in fileNames)
			{
				if (this._addOperationCanceled)
				{
					break;
				}
				this.AddFile(fileName, directoryPathInArchive);
			}
			IL_AC:
			if (!this._addOperationCanceled)
			{
				this.OnAddCompleted();
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000358E4 File Offset: 0x00033AE4
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this.OnAddStarted();
			foreach (string fileName in fileNames)
			{
				this.UpdateFile(fileName, directoryPathInArchive);
			}
			this.OnAddCompleted();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000B0E0 File Offset: 0x000092E0
		public ZipEntry UpdateFile(string fileName)
		{
			return this.UpdateFile(fileName, null);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0003594C File Offset: 0x00033B4C
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[fileName2] != null)
			{
				this.RemoveEntry(fileName2);
			}
			return this.AddFile(fileName, directoryPathInArchive);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000B0EA File Offset: 0x000092EA
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return this.UpdateDirectory(directoryName, null);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000B0F4 File Offset: 0x000092F4
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000B0FF File Offset: 0x000092FF
		public void UpdateItem(string itemName)
		{
			this.UpdateItem(itemName, null);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000B109 File Offset: 0x00009309
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				this.UpdateFile(itemName, directoryPathInArchive);
				return;
			}
			if (!Directory.Exists(itemName))
			{
				throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
			}
			this.UpdateDirectory(itemName, directoryPathInArchive);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000B13F File Offset: 0x0000933F
		public ZipEntry AddEntry(string entryName, string content)
		{
			return this.AddEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0003597C File Offset: 0x00033B7C
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000359C0 File Offset: 0x00033BC0
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00035A0C File Offset: 0x00033C0C
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00035A44 File Offset: 0x00033C44
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00035A90 File Offset: 0x00033C90
		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = this.CompressionMethod;
			ze.CompressionLevel = this.CompressionLevel;
			ze.ExtractExistingFile = this.ExtractExistingFile;
			ze.ZipErrorAction = this.ZipErrorAction;
			ze.SetCompression = this.SetCompression;
			ze.AlternateEncoding = this.AlternateEncoding;
			ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
			ze.Password = this._Password;
			ze.Encryption = this.Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			this.InternalAddEntry(ze.FileName, ze);
			this.AfterAddEntry(ze);
			return ze;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000B14E File Offset: 0x0000934E
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return this.UpdateEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000B15D File Offset: 0x0000935D
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, content, encoding);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000B16F File Offset: 0x0000936F
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, writer);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000B180 File Offset: 0x00009380
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, opener, closer);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000B192 File Offset: 0x00009392
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00035B44 File Offset: 0x00033D44
		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string directoryPathInArchive = null;
			if (entryName.IndexOf('\\') != -1)
			{
				directoryPathInArchive = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
			if (this[fileName] != null)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00035B98 File Offset: 0x00033D98
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream stream = new MemoryStream(byteContent);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000B1A3 File Offset: 0x000093A3
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, byteContent);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000B1B4 File Offset: 0x000093B4
		public ZipEntry AddDirectory(string directoryName)
		{
			return this.AddDirectory(directoryName, null);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000B1BE File Offset: 0x000093BE
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00035BC8 File Offset: 0x00033DC8
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = this.AlternateEncoding;
			zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			this.InternalAddEntry(zipEntry.FileName, zipEntry);
			this.AfterAddEntry(zipEntry);
			return zipEntry;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000B1C9 File Offset: 0x000093C9
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = "";
			}
			return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000B1E0 File Offset: 0x000093E0
		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			this._entries.Add(name, entry);
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00035C50 File Offset: 0x00033E50
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
			}
			if (level == 0)
			{
				this._addOperationCanceled = false;
				this.OnAddStarted();
			}
			if (this._addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int i = level; i > 0; i--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != "")
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = this.AlternateEncoding;
				zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
				if (!this._entries.ContainsKey(zipEntry.FileName))
				{
					this.InternalAddEntry(zipEntry.FileName, zipEntry);
					this.AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!this._addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					foreach (string fileName in files)
					{
						if (this._addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							this.AddFile(fileName, text);
						}
						else
						{
							this.UpdateFile(fileName, text);
						}
					}
					if (!this._addOperationCanceled)
					{
						string[] directories = Directory.GetDirectories(directoryName);
						foreach (string text2 in directories)
						{
							FileAttributes attributes = File.GetAttributes(text2);
							if (this.AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								this.AddOrUpdateDirectoryImpl(text2, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				this.OnAddCompleted();
			}
			return zipEntry;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000B1FD File Offset: 0x000093FD
		public static bool CheckZip(string zipFileName)
		{
			return ZipFile.CheckZip(zipFileName, false, null);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00035E38 File Offset: 0x00034038
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = ZipFile.Read(zipFileName);
				foreach (ZipEntry zipEntry in zipFile)
				{
					foreach (ZipEntry zipEntry2 in zipFile2)
					{
						if (zipEntry.FileName == zipEntry2.FileName)
						{
							if (zipEntry._RelativeOffsetOfLocalHeader != zipEntry2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._RelativeOffsetOfLocalHeader, zipEntry2._RelativeOffsetOfLocalHeader);
								}
							}
							if (zipEntry._CompressedSize != zipEntry2._CompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._CompressedSize, zipEntry2._CompressedSize);
								}
							}
							if (zipEntry._UncompressedSize != zipEntry2._UncompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._UncompressedSize, zipEntry2._UncompressedSize);
								}
							}
							if (zipEntry.CompressionMethod != zipEntry2.CompressionMethod)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.CompressionMethod, zipEntry2.CompressionMethod);
								}
							}
							if (zipEntry.Crc != zipEntry2.Crc)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.Crc, zipEntry2.Crc);
								}
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string text = Path.GetFileNameWithoutExtension(zipFileName);
					text = string.Format("{0}_fixed.zip", text);
					zipFile.Save(text);
				}
			}
			finally
			{
				if (zipFile != null)
				{
					zipFile.Dispose();
				}
				if (zipFile2 != null)
				{
					zipFile2.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000360A0 File Offset: 0x000342A0
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000360E4 File Offset: 0x000342E4
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool result = false;
			try
			{
				using (ZipFile zipFile = ZipFile.Read(zipFileName))
				{
					foreach (ZipEntry zipEntry in zipFile)
					{
						if (!zipEntry.IsDirectory && zipEntry.UsesEncryption)
						{
							zipEntry.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				result = true;
			}
			catch (BadPasswordException)
			{
			}
			return result;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00036178 File Offset: 0x00034378
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipFile: {0}\n", this.Name));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("          Comment: {0}\n", this._Comment));
				}
				if (this._versionMadeBy != 0)
				{
					stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", this._versionMadeBy));
				}
				if (this._versionNeededToExtract != 0)
				{
					stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", this._versionNeededToExtract));
				}
				stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", this.InputUsesZip64));
				stringBuilder.Append(string.Format("     disk with CD: {0}\n", this._diskNumberWithCd));
				if (this._OffsetOfCentralDirectory == 4294967295U)
				{
					stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", this._OffsetOfCentralDirectory64));
				}
				else
				{
					stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", this._OffsetOfCentralDirectory));
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					stringBuilder.Append(zipEntry.Info);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0000B207 File Offset: 0x00009407
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x0000B20F File Offset: 0x0000940F
		public bool FullScan { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0000B218 File Offset: 0x00009418
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x0000B220 File Offset: 0x00009420
		public bool SortEntriesBeforeSaving { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0000B229 File Offset: 0x00009429
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x0000B231 File Offset: 0x00009431
		public bool AddDirectoryWillTraverseReparsePoints { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0000B23A File Offset: 0x0000943A
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x0000B242 File Offset: 0x00009442
		public int BufferSize
		{
			get
			{
				return this._BufferSize;
			}
			set
			{
				this._BufferSize = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0000B24B File Offset: 0x0000944B
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x0000B253 File Offset: 0x00009453
		public int CodecBufferSize { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0000B25C File Offset: 0x0000945C
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x0000B264 File Offset: 0x00009464
		public bool FlattenFoldersOnExtract { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000B26D File Offset: 0x0000946D
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x0000B275 File Offset: 0x00009475
		public CompressionStrategy Strategy
		{
			get
			{
				return this._Strategy;
			}
			set
			{
				this._Strategy = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0000B27E File Offset: 0x0000947E
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x0000B286 File Offset: 0x00009486
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0000B28F File Offset: 0x0000948F
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0000B297 File Offset: 0x00009497
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0000B2A0 File Offset: 0x000094A0
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0000B2B1 File Offset: 0x000094B1
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x0000B2B9 File Offset: 0x000094B9
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._contentsChanged = true;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0000B2C9 File Offset: 0x000094C9
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x0000B2D1 File Offset: 0x000094D1
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0000B2DA File Offset: 0x000094DA
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0000B2E2 File Offset: 0x000094E2
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0000B2EB File Offset: 0x000094EB
		internal bool Verbose
		{
			get
			{
				return this._StatusMessageTextWriter != null;
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000B2F9 File Offset: 0x000094F9
		public bool ContainsEntry(string name)
		{
			return this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0000B30C File Offset: 0x0000950C
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x0000B314 File Offset: 0x00009514
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return this._CaseSensitiveRetrieval;
			}
			set
			{
				if (value != this._CaseSensitiveRetrieval)
				{
					this._CaseSensitiveRetrieval = value;
					this._initEntriesDictionary();
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0000B32C File Offset: 0x0000952C
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x0000B34B File Offset: 0x0000954B
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.GetEncoding("UTF-8");
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipFile.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0000B37A File Offset: 0x0000957A
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x0000B382 File Offset: 0x00009582
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return this._zip64;
			}
			set
			{
				this._zip64 = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000362F0 File Offset: 0x000344F0
		public bool? RequiresZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				if (this._hasBeenSaved && !this._contentsChanged)
				{
					foreach (ZipEntry zipEntry in this._entries.Values)
					{
						if (zipEntry.RequiresZip64.Value)
						{
							return new bool?(true);
						}
					}
					return new bool?(false);
				}
				return null;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0000B38B File Offset: 0x0000958B
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00036398 File Offset: 0x00034598
		public bool? InputUsesZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				foreach (ZipEntry zipEntry in this)
				{
					if (zipEntry.Source != ZipEntrySource.ZipFile)
					{
						return null;
					}
					if (zipEntry._InputUsesZip64)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0000B393 File Offset: 0x00009593
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0000B3A6 File Offset: 0x000095A6
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0000B3B6 File Offset: 0x000095B6
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0000B3BE File Offset: 0x000095BE
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0000B3C7 File Offset: 0x000095C7
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x0000B3CF File Offset: 0x000095CF
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0000B3D8 File Offset: 0x000095D8
		public static Encoding DefaultEncoding
		{
			get
			{
				return ZipFile._defaultEncoding;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0000B3DF File Offset: 0x000095DF
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x0000B3E7 File Offset: 0x000095E7
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return this._StatusMessageTextWriter;
			}
			set
			{
				this._StatusMessageTextWriter = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0000B3F0 File Offset: 0x000095F0
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x0000B3F8 File Offset: 0x000095F8
		public string TempFileFolder
		{
			get
			{
				return this._TempFileFolder;
			}
			set
			{
				this._TempFileFolder = value;
				if (value == null)
				{
					return;
				}
				if (!Directory.Exists(value))
				{
					throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0000B446 File Offset: 0x00009646
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0000B41E File Offset: 0x0000961E
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this.Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this.Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000B44E File Offset: 0x0000964E
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0000B456 File Offset: 0x00009656
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000B45F File Offset: 0x0000965F
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0000B476 File Offset: 0x00009676
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return this._zipErrorAction;
			}
			set
			{
				this._zipErrorAction = value;
				if (this._zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0000B497 File Offset: 0x00009697
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0000B49F File Offset: 0x0000969F
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0000B4B7 File Offset: 0x000096B7
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x0000B4BF File Offset: 0x000096BF
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000B4C8 File Offset: 0x000096C8
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0000B4D0 File Offset: 0x000096D0
		public int MaxOutputSegmentSize
		{
			get
			{
				return this._maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				this._maxOutputSegmentSize = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000B4EF File Offset: 0x000096EF
		public int NumberOfSegmentsForMostRecentSave
		{
			get
			{
				return (int)(this._numberOfSegmentsForMostRecentSave + 1U);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000B531 File Offset: 0x00009731
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0000B4F9 File Offset: 0x000096F9
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0000B539 File Offset: 0x00009739
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0000B541 File Offset: 0x00009741
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000B55E File Offset: 0x0000975E
		public override string ToString()
		{
			return string.Format("ZipFile::{0}", this.Name);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0000B570 File Offset: 0x00009770
		public static Version LibraryVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000B581 File Offset: 0x00009781
		internal void NotifyEntryChanged()
		{
			this._contentsChanged = true;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0000B58A File Offset: 0x0000978A
		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1U != this._diskNumberWithCd && (diskNumber != 0U || this._diskNumberWithCd != 0U))
			{
				return ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);
			}
			return this.ReadStream;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00036424 File Offset: 0x00034624
		internal void Reset(bool whileSaving)
		{
			if (this._JustSaved)
			{
				using (ZipFile zipFile = new ZipFile())
				{
					zipFile._readName = (zipFile._name = (whileSaving ? (this._readName ?? this._name) : this._name));
					zipFile.AlternateEncoding = this.AlternateEncoding;
					zipFile.AlternateEncodingUsage = this.AlternateEncodingUsage;
					ZipFile.ReadIntoInstance(zipFile);
					foreach (ZipEntry zipEntry in zipFile)
					{
						foreach (ZipEntry zipEntry2 in this)
						{
							if (zipEntry.FileName == zipEntry2.FileName)
							{
								zipEntry2.CopyMetaData(zipEntry);
								break;
							}
						}
					}
				}
				this._JustSaved = false;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00036534 File Offset: 0x00034734
		public ZipFile(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("Could not read {0} as a zip file", fileName), innerException);
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x000365D8 File Offset: 0x000347D8
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00036688 File Offset: 0x00034888
		public ZipFile()
		{
			this._InitInstance(null, null);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00036704 File Offset: 0x00034904
		public ZipFile(Encoding encoding)
		{
			this.AlternateEncoding = encoding;
			this.AlternateEncodingUsage = ZipOption.Always;
			this._InitInstance(null, null);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00036790 File Offset: 0x00034990
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			try
			{
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00036834 File Offset: 0x00034A34
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000368E4 File Offset: 0x00034AE4
		public void Initialize(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00036920 File Offset: 0x00034B20
		private void _initEntriesDictionary()
		{
			StringComparer comparer = this.CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
			this._entries = ((this._entries == null) ? new Dictionary<string, ZipEntry>(comparer) : new Dictionary<string, ZipEntry>(this._entries, comparer));
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00036964 File Offset: 0x00034B64
		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			this._name = zipFileName;
			this._StatusMessageTextWriter = statusMessageWriter;
			this._contentsChanged = true;
			this.AddDirectoryWillTraverseReparsePoints = true;
			this.CompressionLevel = CompressionLevel.Default;
			this.ParallelDeflateThreshold = 524288L;
			this._initEntriesDictionary();
			if (File.Exists(this._name))
			{
				if (this.FullScan)
				{
					ZipFile.ReadIntoInstance_Orig(this);
				}
				else
				{
					ZipFile.ReadIntoInstance(this);
				}
				this._fileAlreadyExists = true;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000B5C5 File Offset: 0x000097C5
		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (this._zipEntriesAsList == null)
				{
					this._zipEntriesAsList = new List<ZipEntry>(this._entries.Values);
				}
				return this._zipEntriesAsList;
			}
		}

		// Token: 0x170000D6 RID: 214
		public ZipEntry this[int ix]
		{
			get
			{
				return this.ZipEntriesAsList[ix];
			}
		}

		// Token: 0x170000D7 RID: 215
		public ZipEntry this[string fileName]
		{
			get
			{
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				text = text.Replace("/", "\\");
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				return null;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000B5F9 File Offset: 0x000097F9
		public ICollection<string> EntryFileNames
		{
			get
			{
				return this._entries.Keys;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000B606 File Offset: 0x00009806
		public ICollection<ZipEntry> Entries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00036A30 File Offset: 0x00034C30
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry item in this.Entries)
				{
					list.Add(item);
				}
				StringComparison sc = this.CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
				list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, sc));
				return list.AsReadOnly();
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0000B613 File Offset: 0x00009813
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000B620 File Offset: 0x00009820
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00036AB4 File Offset: 0x00034CB4
		public void RemoveEntry(string fileName)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[fileName2];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			this.RemoveEntry(zipEntry);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000B655 File Offset: 0x00009855
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00036AE8 File Offset: 0x00034CE8
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!this._disposed)
			{
				if (disposeManagedResources)
				{
					if (this._ReadStreamIsOurs && this._readstream != null)
					{
						this._readstream.Dispose();
						this._readstream = null;
					}
					if (this._temporaryFileName != null && this._name != null && this._writestream != null)
					{
						this._writestream.Dispose();
						this._writestream = null;
					}
					if (this.ParallelDeflater != null)
					{
						this.ParallelDeflater.Dispose();
						this.ParallelDeflater = null;
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00036B70 File Offset: 0x00034D70
		internal Stream ReadStream
		{
			get
			{
				if (this._readstream == null && (this._readName != null || this._name != null))
				{
					this._readstream = File.Open(this._readName ?? this._name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					this._ReadStreamIsOurs = true;
				}
				return this._readstream;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00036BC0 File Offset: 0x00034DC0
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x0000B664 File Offset: 0x00009864
		private Stream WriteStream
		{
			get
			{
				if (this._writestream != null)
				{
					return this._writestream;
				}
				if (this._name == null)
				{
					return this._writestream;
				}
				if (this._maxOutputSegmentSize != 0)
				{
					this._writestream = ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
					return this._writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
				return this._writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				this._writestream = null;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0000B67B File Offset: 0x0000987B
		private string ArchiveNameForEvent
		{
			get
			{
				if (this._name == null)
				{
					return "(stream)";
				}
				return this._name;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600058F RID: 1423 RVA: 0x00036C40 File Offset: 0x00034E40
		// (remove) Token: 0x06000590 RID: 1424 RVA: 0x00036C78 File Offset: 0x00034E78
		public event EventHandler<SaveProgressEventArgs> SaveProgress;

		// Token: 0x06000591 RID: 1425 RVA: 0x00036CB0 File Offset: 0x00034EB0
		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00036CF4 File Offset: 0x00034EF4
		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00036D3C File Offset: 0x00034F3C
		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00036D78 File Offset: 0x00034F78
		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00036DB4 File Offset: 0x00034FB4
		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
				saveProgress(this, e);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000596 RID: 1430 RVA: 0x00036DE0 File Offset: 0x00034FE0
		// (remove) Token: 0x06000597 RID: 1431 RVA: 0x00036E18 File Offset: 0x00035018
		public event EventHandler<ReadProgressEventArgs> ReadProgress;

		// Token: 0x06000598 RID: 1432 RVA: 0x00036E50 File Offset: 0x00035050
		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00036E7C File Offset: 0x0003507C
		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00036EA8 File Offset: 0x000350A8
		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
				readProgress(this, e);
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00036EE8 File Offset: 0x000350E8
		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count);
				readProgress(this, e);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0000B691 File Offset: 0x00009891
		private long LengthOfReadStream
		{
			get
			{
				if (this._lengthOfReadStream == -99L)
				{
					this._lengthOfReadStream = (this._ReadStreamIsOurs ? SharedUtilities.GetFileLength(this._name) : -1L);
				}
				return this._lengthOfReadStream;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600059D RID: 1437 RVA: 0x00036F3C File Offset: 0x0003513C
		// (remove) Token: 0x0600059E RID: 1438 RVA: 0x00036F74 File Offset: 0x00035174
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

		// Token: 0x0600059F RID: 1439 RVA: 0x00036FAC File Offset: 0x000351AC
		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00036FF8 File Offset: 0x000351F8
		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0003703C File Offset: 0x0003523C
		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00037090 File Offset: 0x00035290
		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000370D4 File Offset: 0x000352D4
		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00037100 File Offset: 0x00035300
		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060005A5 RID: 1445 RVA: 0x0003712C File Offset: 0x0003532C
		// (remove) Token: 0x060005A6 RID: 1446 RVA: 0x00037164 File Offset: 0x00035364
		public event EventHandler<AddProgressEventArgs> AddProgress;

		// Token: 0x060005A7 RID: 1447 RVA: 0x0003719C File Offset: 0x0003539C
		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000371D8 File Offset: 0x000353D8
		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
				addProgress(this, e);
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00037204 File Offset: 0x00035404
		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060005AA RID: 1450 RVA: 0x0003724C File Offset: 0x0003544C
		// (remove) Token: 0x060005AB RID: 1451 RVA: 0x00037284 File Offset: 0x00035484
		public event EventHandler<ZipErrorEventArgs> ZipError;

		// Token: 0x060005AC RID: 1452 RVA: 0x000372BC File Offset: 0x000354BC
		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				lock (this.LOCK)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(this.Name, entry, exc);
					this.ZipError(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						this._saveOperationCanceled = true;
					}
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000B6CE File Offset: 0x000098CE
		public void ExtractAll(string path)
		{
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000B6D8 File Offset: 0x000098D8
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00037328 File Offset: 0x00035528
		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = this.Verbose;
			this._inExtractAll = true;
			try
			{
				this.OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (flag)
					{
						this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[]
						{
							"Name",
							"Modified",
							"Size",
							"Ratio",
							"Packed"
						});
						this.StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[]
						{
							zipEntry.FileName,
							zipEntry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
							zipEntry.UncompressedSize,
							zipEntry.CompressionRatio,
							zipEntry.CompressedSize
						});
						if (!string.IsNullOrEmpty(zipEntry.Comment))
						{
							this.StatusMessageTextWriter.WriteLine("  Comment: {0}", zipEntry.Comment);
						}
					}
					zipEntry.Password = this._Password;
					this.OnExtractEntry(num, true, zipEntry, path);
					if (overrideExtractExistingProperty)
					{
						zipEntry.ExtractExistingFile = this.ExtractExistingFile;
					}
					zipEntry.Extract(path);
					num++;
					this.OnExtractEntry(num, false, zipEntry, path);
					if (this._extractOperationCanceled)
					{
						break;
					}
				}
				if (!this._extractOperationCanceled)
				{
					foreach (ZipEntry zipEntry2 in this._entries.Values)
					{
						if (zipEntry2.IsDirectory || zipEntry2.FileName.EndsWith("/"))
						{
							string fileOrDirectory = zipEntry2.FileName.StartsWith("/") ? Path.Combine(path, zipEntry2.FileName.Substring(1)) : Path.Combine(path, zipEntry2.FileName);
							zipEntry2._SetTimes(fileOrDirectory, false);
						}
					}
					this.OnExtractAllCompleted(path);
				}
			}
			finally
			{
				this._inExtractAll = false;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000B6E9 File Offset: 0x000098E9
		public static ZipFile Read(string fileName)
		{
			return ZipFile.Read(fileName, null, null, null);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000B6F4 File Offset: 0x000098F4
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000375BC File Offset: 0x000357BC
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ZipFile.ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000B71C File Offset: 0x0000991C
		public static ZipFile Read(Stream zipStream)
		{
			return ZipFile.Read(zipStream, null, null, null);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000B727 File Offset: 0x00009927
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00037628 File Offset: 0x00035828
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position == 0L) ? zipStream : new OffsetStream(zipStream));
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ZipFile.ReadIntoInstance(zipFile);
			return zipFile;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000376B8 File Offset: 0x000358B8
		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ZipFile.ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				uint num = ZipFile.ReadFirstFourBytes(readStream);
				if (num == 101010256U)
				{
					return;
				}
				int num2 = 0;
				bool flag = false;
				long num3 = readStream.Length - 64L;
				long num4 = Math.Max(readStream.Length - 16384L, 10L);
				do
				{
					if (num3 < 0L)
					{
						num3 = 0L;
					}
					readStream.Seek(num3, SeekOrigin.Begin);
					long num5 = SharedUtilities.FindSignature(readStream, 101010256);
					if (num5 != -1L)
					{
						flag = true;
					}
					else
					{
						if (num3 == 0L)
						{
							break;
						}
						num2++;
						num3 -= (long)(32 * (num2 + 1) * num2);
					}
					if (flag)
					{
						break;
					}
				}
				while (num3 > num4);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4L;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535U)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd += 1U;
					uint num6 = BitConverter.ToUInt32(array, 12);
					if (num6 == 4294967295U)
					{
						ZipFile.Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num6;
						readStream.Seek((long)((ulong)num6), SeekOrigin.Begin);
					}
					ZipFile.ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, SeekOrigin.Begin);
					ZipFile.ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception innerException)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					zf._readstream.Dispose();
					zf._readstream = null;
				}
				throw new ZipException("Cannot read that as a ZipFile", innerException);
			}
			zf._contentsChanged = false;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000378B4 File Offset: 0x00035AB4
		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, SeekOrigin.Current);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = uint.MaxValue;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, SeekOrigin.Begin);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792U)
			{
				throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, readStream.Position));
			}
			readStream.Read(array, 0, 8);
			long num3 = BitConverter.ToInt64(array, 0);
			array = new byte[num3];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, SeekOrigin.Begin);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00037978 File Offset: 0x00035B78
		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00037990 File Offset: 0x00035B90
		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0L)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ZipFile.ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00037A94 File Offset: 0x00035C94
		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries = new Dictionary<string, ZipEntry>();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool first = true;
			ZipContainer zc = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				first = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0L)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
				}
				ZipFile.ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00037C0C File Offset: 0x00035E0C
		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			int num2 = 0;
			byte[] array;
			if ((long)num == 101075792L)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44L)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44L];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008L)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256L)
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0U)
			{
				zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
			}
			ZipFile.ReadZipFileComment(zf);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00037D70 File Offset: 0x00035F70
		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)((int)array[0] + (int)array[1] * 256);
			if (num > 0)
			{
				array = new byte[(int)num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0000B74F File Offset: 0x0000994F
		public static bool IsZipFile(string fileName)
		{
			return ZipFile.IsZipFile(fileName, false);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00037DD8 File Offset: 0x00035FD8
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool result = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					result = ZipFile.IsZipFile(fileStream, testExtract);
				}
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00037E44 File Offset: 0x00036044
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool result = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = ZipFile.Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry zipEntry in zipFile)
						{
							if (!zipEntry.IsDirectory)
							{
								zipEntry.Extract(@null);
							}
						}
					}
				}
				result = true;
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00037F0C File Offset: 0x0003610C
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			int num2 = 0;
			while (num2 < num && !flag)
			{
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + num2 * 200);
				}
				num2++;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00037F68 File Offset: 0x00036168
		public void Save()
		{
			try
			{
				bool flag = false;
				this._saveOperationCanceled = false;
				this._numberOfSegmentsForMostRecentSave = 0U;
				this.OnSaveStarted();
				if (this.WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!this._contentsChanged)
				{
					this.OnSaveCompleted();
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
				}
				else
				{
					this.Reset(true);
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("saving....");
					}
					if (this._entries.Count >= 65535 && this._zip64 == Zip64Option.Default)
					{
						throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
					}
					int num = 0;
					ICollection<ZipEntry> collection = this.SortEntriesBeforeSaving ? this.EntriesSorted : this.Entries;
					foreach (ZipEntry zipEntry in collection)
					{
						this.OnSaveEntry(num, zipEntry, true);
						zipEntry.Write(this.WriteStream);
						if (this._saveOperationCanceled)
						{
							break;
						}
						num++;
						this.OnSaveEntry(num, zipEntry, false);
						if (this._saveOperationCanceled)
						{
							break;
						}
						if (zipEntry.IncludedInMostRecentSave)
						{
							flag |= zipEntry.OutputUsedZip64.Value;
						}
					}
					if (!this._saveOperationCanceled)
					{
						ZipSegmentedStream zipSegmentedStream = this.WriteStream as ZipSegmentedStream;
						this._numberOfSegmentsForMostRecentSave = ((zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 1U);
						bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, collection, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer(this));
						this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
						this._hasBeenSaved = true;
						this._contentsChanged = false;
						flag = (flag || flag2);
						this._OutputUsesZip64 = new bool?(flag);
						if (this._name != null && (this._temporaryFileName != null || zipSegmentedStream != null))
						{
							this.WriteStream.Dispose();
							if (this._saveOperationCanceled)
							{
								return;
							}
							if (this._fileAlreadyExists && this._readstream != null)
							{
								this._readstream.Close();
								this._readstream = null;
								foreach (ZipEntry zipEntry2 in collection)
								{
									ZipSegmentedStream zipSegmentedStream2 = zipEntry2._archiveStream as ZipSegmentedStream;
									if (zipSegmentedStream2 != null)
									{
										zipSegmentedStream2.Dispose();
									}
									zipEntry2._archiveStream = null;
								}
							}
							string text = null;
							if (File.Exists(this._name))
							{
								text = this._name + "." + Path.GetRandomFileName();
								if (File.Exists(text))
								{
									this.DeleteFileWithRetry(text);
								}
								File.Move(this._name, text);
							}
							this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
							File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentTempName : this._temporaryFileName, this._name);
							this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
							if (text != null)
							{
								try
								{
									if (File.Exists(text))
									{
										File.Delete(text);
									}
								}
								catch
								{
								}
							}
							this._fileAlreadyExists = true;
						}
						ZipFile.NotifyEntriesSaveComplete(collection);
						this.OnSaveCompleted();
						this._JustSaved = true;
					}
				}
			}
			finally
			{
				this.CleanupAfterSaveOperation();
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000382F8 File Offset: 0x000364F8
		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry zipEntry in c)
			{
				zipEntry.NotifySaveComplete();
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00038340 File Offset: 0x00036540
		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(this._temporaryFileName))
				{
					File.Delete(this._temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00038398 File Offset: 0x00036598
		private void CleanupAfterSaveOperation()
		{
			if (this._name != null)
			{
				if (this._writestream != null)
				{
					try
					{
						this._writestream.Dispose();
					}
					catch (IOException)
					{
					}
				}
				this._writestream = null;
				if (this._temporaryFileName != null)
				{
					this.RemoveTempFile();
					this._temporaryFileName = null;
				}
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000383F4 File Offset: 0x000365F4
		public void Save(string fileName)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			else
			{
				this._readName = this._name;
			}
			this._name = fileName;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this.Save();
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0003846C File Offset: 0x0003666C
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			this._name = null;
			this._writestream = new CountingStream(outputStream);
			this._contentsChanged = true;
			this._fileAlreadyExists = false;
			this.Save();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000384C8 File Offset: 0x000366C8
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
		{
			this.SaveSelfExtractor(exeToGenerate, new SelfExtractorSaveOptions
			{
				Flavor = flavor
			});
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000384EC File Offset: 0x000366EC
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			this._SavingSfx = true;
			this._name = exeToGenerate;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "exeToGenerate"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this._SaveSfxStub(exeToGenerate, options);
			this.Save();
			this._SavingSfx = false;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0003856C File Offset: 0x0003676C
		private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
		{
			byte[] array = new byte[1024];
			using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					throw new ZipException(string.Format("missing resource '{0}'", resourceName));
				}
				using (FileStream fileStream = File.OpenWrite(filename))
				{
					int num;
					do
					{
						num = manifestResourceStream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000385F8 File Offset: 0x000367F8
		private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			string text = null;
			string text2 = null;
			string dir = null;
			try
			{
				if (File.Exists(exeToGenerate) && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
				}
				if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
				}
				dir = (this.TempFileFolder ?? Path.GetDirectoryName(exeToGenerate));
				text = ZipFile.GenerateTempPathname(dir, "exe");
				Assembly assembly = typeof(ZipFile).Assembly;
				using (CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider())
				{
					ZipFile.ExtractorSettings extractorSettings = null;
					ZipFile.ExtractorSettings[] settingsList = ZipFile.SettingsList;
					int i = 0;
					while (i < settingsList.Length)
					{
						ZipFile.ExtractorSettings extractorSettings2 = settingsList[i];
						if (extractorSettings2.Flavor == options.Flavor)
						{
							extractorSettings = extractorSettings2;
							IL_C2:
							if (extractorSettings == null)
							{
								throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", options.Flavor));
							}
							CompilerParameters compilerParameters = new CompilerParameters();
							compilerParameters.ReferencedAssemblies.Add(assembly.Location);
							if (extractorSettings.ReferencedAssemblies != null)
							{
								foreach (string value in extractorSettings.ReferencedAssemblies)
								{
									compilerParameters.ReferencedAssemblies.Add(value);
								}
							}
							compilerParameters.GenerateInMemory = false;
							compilerParameters.GenerateExecutable = true;
							compilerParameters.IncludeDebugInformation = false;
							compilerParameters.CompilerOptions = "";
							Assembly executingAssembly = Assembly.GetExecutingAssembly();
							StringBuilder stringBuilder = new StringBuilder();
							string text3 = ZipFile.GenerateTempPathname(dir, "cs");
							using (ZipFile zipFile = ZipFile.Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
							{
								text2 = ZipFile.GenerateTempPathname(dir, "tmp");
								if (string.IsNullOrEmpty(options.IconFile))
								{
									Directory.CreateDirectory(text2);
									ZipEntry zipEntry = zipFile["zippedFile.ico"];
									if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
									{
										zipEntry.Attributes ^= FileAttributes.ReadOnly;
									}
									zipEntry.Extract(text2);
									string arg = Path.Combine(text2, "zippedFile.ico");
									CompilerParameters compilerParameters2 = compilerParameters;
									compilerParameters2.CompilerOptions += string.Format("/win32icon:\"{0}\"", arg);
								}
								else
								{
									CompilerParameters compilerParameters3 = compilerParameters;
									compilerParameters3.CompilerOptions += string.Format("/win32icon:\"{0}\"", options.IconFile);
								}
								compilerParameters.OutputAssembly = text;
								if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
								{
									CompilerParameters compilerParameters4 = compilerParameters;
									compilerParameters4.CompilerOptions += " /target:winexe";
								}
								if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
								{
									CompilerParameters compilerParameters5 = compilerParameters;
									compilerParameters5.CompilerOptions = compilerParameters5.CompilerOptions + " " + options.AdditionalCompilerSwitches;
								}
								if (string.IsNullOrEmpty(compilerParameters.CompilerOptions))
								{
									compilerParameters.CompilerOptions = null;
								}
								if (extractorSettings.CopyThroughResources != null && extractorSettings.CopyThroughResources.Count != 0)
								{
									if (!Directory.Exists(text2))
									{
										Directory.CreateDirectory(text2);
									}
									foreach (string text4 in extractorSettings.CopyThroughResources)
									{
										string text5 = Path.Combine(text2, text4);
										ZipFile.ExtractResourceToFile(executingAssembly, text4, text5);
										compilerParameters.EmbeddedResources.Add(text5);
									}
								}
								compilerParameters.EmbeddedResources.Add(assembly.Location);
								stringBuilder.Append("// " + Path.GetFileName(text3) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ").Append(ZipFile.LibraryVersion.ToString()).Append("\n//         at ").Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss")).Append("\n//\n// --------------------------------------------\n\n\n");
								if (!string.IsNullOrEmpty(options.Description))
								{
									stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
								}
								else
								{
									stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
								}
								if (!string.IsNullOrEmpty(options.ProductVersion))
								{
									stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
								}
								string str = string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright © Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", "");
								if (!string.IsNullOrEmpty(options.ProductName))
								{
									stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
								}
								else
								{
									stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
								}
								stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + str + "\")]\n").Append(string.Format("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]\n", ZipFile.LibraryVersion.ToString()));
								if (options.FileVersion != null)
								{
									stringBuilder.Append(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}\")]\n", options.FileVersion.ToString()));
								}
								stringBuilder.Append("\n\n\n");
								string text6 = options.DefaultExtractDirectory;
								if (text6 != null)
								{
									text6 = text6.Replace("\"", "").Replace("\\", "\\\\");
								}
								string text7 = options.PostExtractCommandLine;
								if (text7 != null)
								{
									text7 = text7.Replace("\\", "\\\\");
									text7 = text7.Replace("\"", "\\\"");
								}
								foreach (string text8 in extractorSettings.ResourcesToCompile)
								{
									using (Stream stream = zipFile[text8].OpenReader())
									{
										if (stream == null)
										{
											throw new ZipException(string.Format("missing resource '{0}'", text8));
										}
										using (StreamReader streamReader = new StreamReader(stream))
										{
											while (streamReader.Peek() >= 0)
											{
												string text9 = streamReader.ReadLine();
												if (text6 != null)
												{
													text9 = text9.Replace("@@EXTRACTLOCATION", text6);
												}
												text9 = text9.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString());
												text9 = text9.Replace("@@QUIET", options.Quiet.ToString());
												if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
												{
													text9 = text9.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
												}
												text9 = text9.Replace("@@EXTRACT_EXISTING_FILE", ((int)options.ExtractExistingFile).ToString());
												if (text7 != null)
												{
													text9 = text9.Replace("@@POST_UNPACK_CMD_LINE", text7);
												}
												stringBuilder.Append(text9).Append("\n");
											}
										}
										stringBuilder.Append("\n\n");
									}
								}
							}
							string text10 = stringBuilder.ToString();
							CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[]
							{
								text10
							});
							if (compilerResults == null)
							{
								throw new SfxGenerationException("Cannot compile the extraction logic!");
							}
							if (this.Verbose)
							{
								foreach (string value2 in compilerResults.Output)
								{
									this.StatusMessageTextWriter.WriteLine(value2);
								}
							}
							if (compilerResults.Errors.Count != 0)
							{
								using (TextWriter textWriter = new StreamWriter(text3))
								{
									textWriter.Write(text10);
									textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
									textWriter.Write("// Errors during compilation: \n//\n");
									string fileName = Path.GetFileName(text3);
									foreach (object obj in compilerResults.Errors)
									{
										CompilerError compilerError = (CompilerError)obj;
										textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", new object[]
										{
											fileName,
											compilerError.Line,
											compilerError.Column,
											compilerError.IsWarning ? "Warning" : "error",
											compilerError.ErrorNumber,
											compilerError.ErrorText
										}));
									}
								}
								throw new SfxGenerationException(string.Format("Errors compiling the extraction logic!  {0}", text3));
							}
							this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
							using (Stream stream2 = File.OpenRead(text))
							{
								byte[] array = new byte[4000];
								int num = 1;
								while (num != 0)
								{
									num = stream2.Read(array, 0, array.Length);
									if (num != 0)
									{
										this.WriteStream.Write(array, 0, num);
									}
								}
							}
							goto IL_8D0;
						}
						else
						{
							i++;
						}
					}
					goto IL_C2;
				}
				IL_8D0:
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			}
			finally
			{
				try
				{
					if (Directory.Exists(text2))
					{
						try
						{
							Directory.Delete(text2, true);
						}
						catch (IOException arg2)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg2);
						}
					}
					if (File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch (IOException arg3)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg3);
						}
					}
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000390A0 File Offset: 0x000372A0
		internal static string GenerateTempPathname(string dir, string extension)
		{
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string text2;
			do
			{
				string text = Guid.NewGuid().ToString();
				string path = string.Format("{0}-{1}-{2}.{3}", new object[]
				{
					name,
					DateTime.Now.ToString("yyyyMMMdd-HHmmss"),
					text,
					extension
				});
				text2 = Path.Combine(dir, path);
			}
			while (File.Exists(text2) || Directory.Exists(text2));
			return text2;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000B758 File Offset: 0x00009958
		public void AddSelectedFiles(string selectionCriteria)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000B768 File Offset: 0x00009968
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000B778 File Offset: 0x00009978
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000B784 File Offset: 0x00009984
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000B790 File Offset: 0x00009990
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000B79C File Offset: 0x0000999C
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000B7AA File Offset: 0x000099AA
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000B7B8 File Offset: 0x000099B8
		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00039130 File Offset: 0x00037330
		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			ReadOnlyCollection<string> readOnlyCollection = fileSelector.SelectFiles(directoryOnDisk, recurseDirectories);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			this.OnAddStarted();
			AddOrUpdateAction action = wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly;
			foreach (string text in readOnlyCollection)
			{
				string text2 = (directoryPathInArchive == null) ? null : ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(text), directoryOnDisk, directoryPathInArchive);
				if (File.Exists(text))
				{
					if (wantUpdate)
					{
						this.UpdateFile(text, text2);
					}
					else
					{
						this.AddFile(text, text2);
					}
				}
				else
				{
					this.AddOrUpdateDirectoryImpl(text, text2, action, false, 0);
				}
			}
			this.OnAddCompleted();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00039270 File Offset: 0x00037470
		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			int num = text.IndexOf(text2);
			if (num != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000392AC File Offset: 0x000374AC
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000392D0 File Offset: 0x000374D0
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this, directoryPathInArchive);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000392F4 File Offset: 0x000374F4
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00039318 File Offset: 0x00037518
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria, directoryPathInArchive);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0003933C File Offset: 0x0003753C
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00039398 File Offset: 0x00037598
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractExistingFile);
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000393F4 File Offset: 0x000375F4
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00039450 File Offset: 0x00037650
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000394AC File Offset: 0x000376AC
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory, extractExistingFile);
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0003950C File Offset: 0x0003770C
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			ZipFile.<GetEnumerator>d__3 <GetEnumerator>d__ = new ZipFile.<GetEnumerator>d__3(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000B7D4 File Offset: 0x000099D4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000B7D4 File Offset: 0x000099D4
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400035E RID: 862
		private TextWriter _StatusMessageTextWriter;

		// Token: 0x0400035F RID: 863
		private bool _CaseSensitiveRetrieval;

		// Token: 0x04000360 RID: 864
		private Stream _readstream;

		// Token: 0x04000361 RID: 865
		private Stream _writestream;

		// Token: 0x04000362 RID: 866
		private ushort _versionMadeBy;

		// Token: 0x04000363 RID: 867
		private ushort _versionNeededToExtract;

		// Token: 0x04000364 RID: 868
		private uint _diskNumberWithCd;

		// Token: 0x04000365 RID: 869
		private int _maxOutputSegmentSize;

		// Token: 0x04000366 RID: 870
		private uint _numberOfSegmentsForMostRecentSave;

		// Token: 0x04000367 RID: 871
		private ZipErrorAction _zipErrorAction;

		// Token: 0x04000368 RID: 872
		private bool _disposed;

		// Token: 0x04000369 RID: 873
		private Dictionary<string, ZipEntry> _entries;

		// Token: 0x0400036A RID: 874
		private List<ZipEntry> _zipEntriesAsList;

		// Token: 0x0400036B RID: 875
		private string _name;

		// Token: 0x0400036C RID: 876
		private string _readName;

		// Token: 0x0400036D RID: 877
		private string _Comment;

		// Token: 0x0400036E RID: 878
		internal string _Password;

		// Token: 0x0400036F RID: 879
		private bool _emitNtfsTimes = true;

		// Token: 0x04000370 RID: 880
		private bool _emitUnixTimes;

		// Token: 0x04000371 RID: 881
		private CompressionStrategy _Strategy;

		// Token: 0x04000372 RID: 882
		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		// Token: 0x04000373 RID: 883
		private bool _fileAlreadyExists;

		// Token: 0x04000374 RID: 884
		private string _temporaryFileName;

		// Token: 0x04000375 RID: 885
		private bool _contentsChanged;

		// Token: 0x04000376 RID: 886
		private bool _hasBeenSaved;

		// Token: 0x04000377 RID: 887
		private string _TempFileFolder;

		// Token: 0x04000378 RID: 888
		private bool _ReadStreamIsOurs = true;

		// Token: 0x04000379 RID: 889
		private object LOCK = new object();

		// Token: 0x0400037A RID: 890
		private bool _saveOperationCanceled;

		// Token: 0x0400037B RID: 891
		private bool _extractOperationCanceled;

		// Token: 0x0400037C RID: 892
		private bool _addOperationCanceled;

		// Token: 0x0400037D RID: 893
		private EncryptionAlgorithm _Encryption;

		// Token: 0x0400037E RID: 894
		private bool _JustSaved;

		// Token: 0x0400037F RID: 895
		private long _locEndOfCDS = -1L;

		// Token: 0x04000380 RID: 896
		private uint _OffsetOfCentralDirectory;

		// Token: 0x04000381 RID: 897
		private long _OffsetOfCentralDirectory64;

		// Token: 0x04000382 RID: 898
		private bool? _OutputUsesZip64;

		// Token: 0x04000383 RID: 899
		internal bool _inExtractAll;

		// Token: 0x04000384 RID: 900
		private Encoding _alternateEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x04000385 RID: 901
		private ZipOption _alternateEncodingUsage;

		// Token: 0x04000386 RID: 902
		private static Encoding _defaultEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x04000387 RID: 903
		private int _BufferSize = ZipFile.BufferSizeDefault;

		// Token: 0x04000388 RID: 904
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x04000389 RID: 905
		private long _ParallelDeflateThreshold;

		// Token: 0x0400038A RID: 906
		private int _maxBufferPairs = 16;

		// Token: 0x0400038B RID: 907
		internal Zip64Option _zip64;

		// Token: 0x0400038C RID: 908
		private bool _SavingSfx;

		// Token: 0x0400038D RID: 909
		public static readonly int BufferSizeDefault = 32768;

		// Token: 0x04000390 RID: 912
		private long _lengthOfReadStream = -99L;

		// Token: 0x04000394 RID: 916
		private static ZipFile.ExtractorSettings[] SettingsList = new ZipFile.ExtractorSettings[]
		{
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.WinFormsApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll",
					"System.Windows.Forms.dll",
					"System.Drawing.dll"
				},
				CopyThroughResources = new List<string>
				{
					"Ionic.Zip.WinFormsSelfExtractorStub.resources",
					"Ionic.Zip.Forms.PasswordDialog.resources",
					"Ionic.Zip.Forms.ZipContentsDialog.resources"
				},
				ResourcesToCompile = new List<string>
				{
					"WinFormsSelfExtractorStub.cs",
					"WinFormsSelfExtractorStub.Designer.cs",
					"PasswordDialog.cs",
					"PasswordDialog.Designer.cs",
					"ZipContentsDialog.cs",
					"ZipContentsDialog.Designer.cs",
					"FolderBrowserDialogEx.cs"
				}
			},
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.ConsoleApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll"
				},
				CopyThroughResources = null,
				ResourcesToCompile = new List<string>
				{
					"CommandLineSelfExtractorStub.cs"
				}
			}
		};

		// Token: 0x020000F0 RID: 240
		private class ExtractorSettings
		{
			// Token: 0x0400039D RID: 925
			public SelfExtractorFlavor Flavor;

			// Token: 0x0400039E RID: 926
			public List<string> ReferencedAssemblies;

			// Token: 0x0400039F RID: 927
			public List<string> CopyThroughResources;

			// Token: 0x040003A0 RID: 928
			public List<string> ResourcesToCompile;
		}
	}
}
