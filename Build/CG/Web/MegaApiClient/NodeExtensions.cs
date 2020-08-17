using System;
using System.Collections.Generic;
using System.Linq;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000163 RID: 355
	public static class NodeExtensions
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x0004CC7C File Offset: 0x0004AE7C
		public static long GetFolderSize(this INodeInfo node, IMegaApiClient client)
		{
			IEnumerable<INode> nodes = client.GetNodes();
			return node.GetFolderSize(nodes);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0004CC98 File Offset: 0x0004AE98
		public static long GetFolderSize(this INodeInfo node, IEnumerable<INode> allNodes)
		{
			if (node.Type == NodeType.File)
			{
				throw new InvalidOperationException("node is not a Directory");
			}
			long num = 0L;
			foreach (INode node2 in from x in allNodes
			where x.ParentId == node.Id
			select x)
			{
				if (node2.Type == NodeType.File)
				{
					num += node2.Size;
				}
				else if (node2.Type == NodeType.Directory)
				{
					long folderSize = node2.GetFolderSize(allNodes);
					num += folderSize;
				}
			}
			return num;
		}
	}
}
