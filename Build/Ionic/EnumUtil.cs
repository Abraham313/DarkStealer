using System;
using System.ComponentModel;
using System.Reflection;

namespace Ionic
{
	// Token: 0x020000DC RID: 220
	internal sealed class EnumUtil
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00008754 File Offset: 0x00006954
		private EnumUtil()
		{
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0002E808 File Offset: 0x0002CA08
		internal static string GetDescription(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length > 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A604 File Offset: 0x00008804
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return EnumUtil.Parse(enumType, stringRepresentation, false);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0002E858 File Offset: 0x0002CA58
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (object obj in Enum.GetValues(enumType))
			{
				Enum @enum = (Enum)obj;
				string text = EnumUtil.GetDescription(@enum);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return @enum;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
