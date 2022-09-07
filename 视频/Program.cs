
using System;
using System.IO;

namespace 视频
{
	class Program
	{
		public static void Main(string[] args)
		{
			var dir = @"C:\Users\Administrator\Documents\SharpDevelop Projects\视频\视频";
			var n = "Games.cs";
			if (!File.Exists(Path.Combine(dir, n))) {
				File.Create(Path.Combine(dir, n)).Dispose();
			}
			
			var kbh = new KeyboardShare();
			kbh.ConfigHook();
			int _mode=1;
			kbh.KeyDown += async (s, k) => {
				if (_mode ==1) {
					switch (k.Key) {
					
						case Key.F8:
							
							break;
						case Key.F9:
							Games.FashiStrong();
							break;
					}
				}else if (_mode == 2) {
					switch (k.Key) {
					
						case Key.F8:
							NativeMethods.GetScreenshot();
							break;
						case Key.F9:
						//Utils.CreateDirectory();
							Console.WriteLine(Keys.GetColor(1119, 384));
							Console.WriteLine(0x00FF00);
							break;
					}
				}
			};
			Shared. MSG message;
			while (Shared.GetMessage(out message, IntPtr.Zero, 0, 0) != 0) {
				Shared.TranslateMessage(ref message);
				Shared.DispatchMessage(ref message);
			}
		}
	}
}