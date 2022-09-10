
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace 视频
{
 
	public static class Utils
	{
		public static void SetKeyFrames()
		{
			var lines = ClipboardShare.GetText().Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
			var ls = new List<string>();
			for (int i = 0; i < lines.Length; i += 2) {
				var pieces = lines[i].Split(':');
				var total = float.Parse(pieces[0]) * 3600 + float.Parse(pieces[1]) * 60 + float.Parse(pieces[2]) + float.Parse(pieces[3]) * .04;
				ls.Add(string.Format(@"app.project.activeItem.selectedProperties[1].setValueAtTime({0}, {1});", total-0.04, 0));
				ls.Add(string.Format(@"app.project.activeItem.selectedProperties[1].setValueAtTime({0}, {1});", total, 60));
				pieces = lines[i + 1].Split(':');
				total = float.Parse(pieces[0]) * 3600 + float.Parse(pieces[1]) * 60 + float.Parse(pieces[2]) + float.Parse(pieces[3]) * .04;
				ls.Add(string.Format(@"app.project.activeItem.selectedProperties[1].setValueAtTime({0}, {1});", total, 60));
								ls.Add(string.Format(@"app.project.activeItem.selectedProperties[1].setValueAtTime({0}, {1});", total+0.04, 0));
			}
			ClipboardShare.SetText(string.Join(Environment.NewLine,ls));
			
		}

		public static void DownloadFile()
		{
			var dir = "Downloads".GetDesktopPath();
			dir.CreateDirectoryIfNotExists();
			
			Process.Start(new ProcessStartInfo {
				FileName = "aria2c",
				Arguments = "\"" + ClipboardShare.GetText() + "\""
				//+" --https-proxy=\"https://127.0.0.1:10809\"" 
				, WorkingDirectory = dir
			});
		}

		public static void ConvertSourceCodes(string dir)
		{
			var xxx = "Sources".GetDesktopPath();
			xxx.CreateDirectoryIfNotExists();
			if (!Directory.Exists(dir)) {
				var lines = dir.Split('\n').Select(x => x.TrimEnd());
				var ls = new List<string>();
				var fn = string.Empty;
				var xv = false;
				foreach (var element in lines) {
					if (element.StartsWith("## ")) {
						fn = element.Substring(3).Trim();
						continue;
					}
					if (element.StartsWith("```")) {
						if (xv) {
							File.WriteAllLines(Path.Combine(xxx, fn), ls);
							ls.Clear();
						}
					 
						
						xv = !xv;
						continue;
					}
					if (xv)
						ls.Add(element);
				}
			} else {
				var files = Directory.GetFiles(dir, "*").Where(x => Regex.IsMatch(x, "\\.(?:c|js|h)$"));
				var ls = new List<string>();
				foreach (var element in files) {
					ls.Add(string.Format("## {0}\n\n```\n\n{1}\n\n```\n\n", Path.GetFileName(element), File.ReadAllText(element)));
				}
				ClipboardShare.SetText(string.Join(Environment.NewLine, ls));
			}
			
		}

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
				if (File.ReadAllText(element).IndexOf("CompileCpp") != -1) {
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
	
	public static class Androids
	{
		public static void CreateFile(string fileName)
		{
			var dir = @"C:\Users\Administrator\Desktop\Links\Key\SecretGarden\app\src\main\java\euphoria\psycho\porn";
			fileName = Path.Combine(dir, fileName);
			if (!File.Exists(fileName)) {
				File.Create(fileName).Dispose();
			}
		}
	}
}
