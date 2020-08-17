using System;
using System.Collections.Generic;
using System.Text;

namespace Echelon.Stealer.Browsers.Gecko
{
	// Token: 0x02000044 RID: 68
	public class Gecko4
	{
		// Token: 0x060001BC RID: 444 RVA: 0x00009885 File Offset: 0x00007A85
		public Gecko4()
		{
			this.Objects = new List<Gecko4>();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009898 File Offset: 0x00007A98
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000098A0 File Offset: 0x00007AA0
		public Gecko2 ObjectType { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000098A9 File Offset: 0x00007AA9
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000098B1 File Offset: 0x00007AB1
		public byte[] ObjectData { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000098BA File Offset: 0x00007ABA
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000098C2 File Offset: 0x00007AC2
		public int ObjectLength { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000098CB File Offset: 0x00007ACB
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000098D3 File Offset: 0x00007AD3
		public List<Gecko4> Objects { get; set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x000207D8 File Offset: 0x0001E9D8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			Gecko2 objectType = this.ObjectType;
			switch (objectType)
			{
			case Gecko2.Integer:
				foreach (byte b in this.ObjectData)
				{
					stringBuilder2.AppendFormat("{0:X2}", b);
				}
				stringBuilder.Append("\tINTEGER ").Append(stringBuilder2).AppendLine();
				break;
			case Gecko2.BitString:
			case Gecko2.Null:
				break;
			case Gecko2.OctetString:
				foreach (byte b2 in this.ObjectData)
				{
					stringBuilder2.AppendFormat("{0:X2}", b2);
				}
				stringBuilder.Append("\tOCTETSTRING ").AppendLine(stringBuilder2.ToString());
				break;
			case Gecko2.ObjectIdentifier:
				foreach (byte b3 in this.ObjectData)
				{
					stringBuilder2.AppendFormat("{0:X2}", b3);
				}
				stringBuilder.Append("\tOBJECTIDENTIFIER ").AppendLine(stringBuilder2.ToString());
				break;
			default:
				if (objectType == Gecko2.Sequence)
				{
					stringBuilder.AppendLine("SEQUENCE {");
				}
				break;
			}
			foreach (Gecko4 value in this.Objects)
			{
				stringBuilder.Append(value);
			}
			if (this.ObjectType == Gecko2.Sequence)
			{
				stringBuilder.AppendLine("}");
			}
			stringBuilder2.Remove(0, stringBuilder2.Length - 1);
			return stringBuilder.ToString();
		}
	}
}
