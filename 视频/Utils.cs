
using System;
using System.Collections.Generic;
using System.IO;

namespace 视频
{
 
	public static class Utils
	{
		public static void CreateDirectory()
		{
			var n =	ClipboardShare.GetText().Trim();
			n.GetDesktopFileName().CreateDirectoryIfNotExists();
		}

		public static void SearchInFiles()
		{
			var dir = @"C:\Users\Administrator\Desktop\Links";
			                                    
			var files = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);
			var ls = new List<string>();
			foreach (var element in files) {
				if (File.ReadAllText(element).IndexOf("Capture") != -1) {
					ls.Add(element);
				}
			}
			File.WriteAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "files.txt"), ls);
				
		}
		
		public static void ScreenRecord()
		{
			var i = 1;
			var str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), i.ToString().PadLeft(3, '0') + ".mp4");
			while (File.Exists(str)) {
				i++;
				str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), i.ToString().PadLeft(3, '0') + ".mp4");
			}
			string arg = string.Format(@" -hide_banner -rtbufsize 150M -f gdigrab -framerate 30 -offset_x 0 -offset_y 0 -video_size 1280x720 -draw_mouse 1 -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 28 -pix_fmt yuv420p -movflags +faststart -y ""{0}""", str);
			
			ClipboardShare.SetText(@"ffmpeg.exe" + arg);
		}
		 
	}
}
