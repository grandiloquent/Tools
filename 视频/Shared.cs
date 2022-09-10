namespace 视频
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System;
	using System.Globalization;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Text;
	
	public static class Shared
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct MOUSEINPUT
		{
			public int dx;
			public int dy;
			public int mouseData;
			public int dwFlags;
			public int time;
			public int dwExtraInfo;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct KEYBDINPUT
		{
			public short wVk;
			public short wScan;
			public int dwFlags;
			public int time;
			public int dwExtraInfo;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct HARDWAREINPUT
		{
			public int uMsg;
			public short wParamL;
			public short wParamH;
		};

		[StructLayout(LayoutKind.Explicit)]
		public struct INPUT
		{
			[FieldOffset(0)]
			public int type;
			[FieldOffset(4)]
			public MOUSEINPUT no;
			[FieldOffset(4)]
			public KEYBDINPUT ki;
			[FieldOffset(4)]
			public HARDWAREINPUT hi;
		};
		const int INPUT_KEYBOARD = 1;
		const int KEYEVENTF_KEYDOWN = 0x0;
		const int KEYEVENTF_KEYUP = 0x2;
		const int KEYEVENTF_EXTENDEDKEY = 0x1;
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("user32.dll")]
		public	static extern void SendInput(int nInputs, ref INPUT pInputs, int cbsize);
		public static void SendKey(int key, bool isExtend = false)
		{
			
			INPUT input = GetKeyDownInput(key, isExtend);
			SendInput(1, ref input, Marshal.SizeOf(input));
			Thread.Sleep(100); // wait
			input = GetKeyUpInput(input, isExtend);
			SendInput(1, ref input, Marshal.SizeOf(input));
		}
		static INPUT GetKeyDownInput(int key, bool isExtend)
		{
			return new INPUT {
				type = INPUT_KEYBOARD,
				ki = new KEYBDINPUT() {
					wVk = (short)key,
					wScan = 0,
					dwFlags = ((isExtend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYDOWN,
					time = 0,
					dwExtraInfo = 0
				},
			};
		}
		static INPUT GetKeyUpInput(INPUT input, bool isExtend)
		{
			input.ki.dwFlags = ((isExtend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYUP;
			return input;
		}
		public static void SendKeyWithShift(int key, bool isExtend = false)
		{
			INPUT input0 = GetKeyDownInput(0x10, isExtend); // VK_SHIFT
			INPUT input1 = GetKeyDownInput(key, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			Thread.Sleep(100); // wait
			input1 = GetKeyUpInput(input1, isExtend);
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			input0 = GetKeyUpInput(input0, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
		}
		public static void SendKeyWithCtrl(int key, bool isExtend = false)
		{
			INPUT input0 = GetKeyDownInput(0x11, isExtend); // VK_CONTROL
			INPUT input1 = GetKeyDownInput(key, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			Thread.Sleep(100); // wait
			input1 = GetKeyUpInput(input1, isExtend);
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			input0 = GetKeyUpInput(input0, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
		}

		public static void SendKeyWithAlt(int key, bool isExtend = false)
		{
			
			INPUT input0 = GetKeyDownInput(0x12, isExtend); // VK_MENU : Alt key
			INPUT input1 = GetKeyDownInput(key, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			Thread.Sleep(100); // wait
			input1 = GetKeyUpInput(input1, isExtend);
			SendInput(1, ref input1, Marshal.SizeOf(input1));
			input0 = GetKeyUpInput(input0, isExtend);
			SendInput(1, ref input0, Marshal.SizeOf(input0));
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct MSG
		{
			public IntPtr hwnd;
			public int message;
			public IntPtr wParam;
			public IntPtr lParam;
			public int time;
			public int pt_x;
			public int pt_y;
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool TranslateMessage([In, Out] ref MSG msg);
		[DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		public static extern int DispatchMessage([In] ref MSG msg);
		[DllImport("user32.dll")]
		public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
			uint wMsgFilterMax);
		
		 public static string GetDesktopFileName(this string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        }
        public static void CreateDirectoryIfNotExists(this string path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
        public static void CompileCpp(string f)
		{
			//string exeDirectory=@"C:\msys64\mingw64\bin";
			var extension = Path.GetExtension(f).ToLower();
			var cmd = "";
			var dir = Path.Combine(Path.GetDirectoryName(f), "bin");
			if (extension == ".fsx") {
				cmd = string.Format("/K dotnet \"C:\\Program Files\\dotnet\\sdk\\3.0.100-preview-009812\\FSharp\\fsi.exe\" \"{0}\"", f);
				Process.Start(new ProcessStartInfo() {
					FileName = "cmd",
					Arguments = cmd,
					WorkingDirectory = dir
				});
				return;
			}
			//			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output");
			//			if (!Directory.Exists(dir)) {
			//				Directory.CreateDirectory(dir);
			//			}
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			var arg = "";
			var arg2 = "";
			var argLines = File.ReadLines(f, new UTF8Encoding(false)).ToList();
//			foreach (var element in argLines) {
//				if (string.IsNullOrWhiteSpace(element))
//					continue;
//				if (element.StartsWith("// ")) {
//					arg += element.Substring(3) + " ";
//				} else
//					break;s
//			}
			if (argLines[0].StartsWith("// ")) {
				arg = argLines[0].Substring(3) + " ";
			}
			if (argLines[1].StartsWith("// ")) {
				arg2 = argLines[1].Substring(3) + " ";
			}
			if (argLines[2].StartsWith("// ")) {
				arg2 += argLines[2].Substring(3) + " ";
			}
			arg = Regex.Replace(arg, "\\./[0-9a-zA-Z_\\.]+", new MatchEvaluator(m => m.Value.GetFullPath(Path.GetDirectoryName(f))));
			var exe = (Path.GetFileNameWithoutExtension(f) + ".exe").GetDesktopPath();
			try {
				var ps = Process.GetProcesses().Where(i => i.ProcessName == Path.GetFileNameWithoutExtension(f) || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch (Exception e) {
				Console.WriteLine("ERROR:" + e.Message);
			}
			string workingDirectory = @"C:\msys64\mingw64\bin\";
			// -finput-charset=UTF-8 -fexec-charset=GBK -lstdc++fs  -std=c++17
			//var cmd = string.Format("/K gcc -Wall -g -finput-charset=GBK -fexec-charset=GBK \"{0}\" -o \"{1}\\{3}\" {2} && \"{1}\\{3}\" ", f, dir, arg, exe);
			// -Wall -g
			// -std=c99 -Wall -O2
			// -Wl,-E
			if (extension == ".c") {
				cmd = string.Format("/K {4}gcc \"{0}\" -o \"{3}\" {2} && \"{3}\" {5}", f, dir, arg, exe, workingDirectory, arg2);
			} else if (extension == ".cpp") {
				// dir {1}\\{1}\\
				cmd = string.Format("/K {4}g++ -Wall -g \"{0}\" -o \"{3}\" {2} && \"{3}\" {5}", f, "", arg, exe, workingDirectory, arg2);
			}
			Console.WriteLine(cmd);
			Process.Start(new ProcessStartInfo() {
				FileName = "cmd",
				Arguments = cmd,
				WorkingDirectory = Path.GetDirectoryName(f)
			});
		}
public static string GetFullPath(this string relativePath, string replacePath = null)
		{
			if (replacePath == null) {
				replacePath = AppDomain.CurrentDomain.BaseDirectory;
			}
			int parentStart = relativePath.IndexOf("..\\");
			int absoluteStart = relativePath.IndexOf("./");
			checked {
				if (parentStart >= 0) {
					int parentLength;
					for (parentLength = 0;
					     relativePath.Substring(parentStart + parentLength).Contains("..\\");
					     parentLength += "..\\".Length) {
						replacePath = new DirectoryInfo(replacePath).Parent.FullName;
					}
					relativePath = relativePath.Replace(relativePath.Substring(parentStart, parentLength),
						replacePath + "\\");
				} else if (absoluteStart >= 0) {
					relativePath = replacePath + relativePath.Substring(1);
				}
				return relativePath;
			}
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		
	}
	public static class ClipboardShare
	{
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GlobalLock(IntPtr hMem);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int GlobalSize(IntPtr hMem);
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GlobalUnlock(IntPtr hMem);
		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsClipboardFormatAvailable(uint format);
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);
		[DllImport("user32.dll")]
		static extern bool EmptyClipboard();
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CloseClipboard();
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern int DragQueryFile(IntPtr hDrop, int iFile, StringBuilder lpszFile, int cch);
		[DllImport("User32.dll", SetLastError = true)]
		static extern IntPtr GetClipboardData(uint uFormat);
		const uint cfUnicodeText = 13;
		public static void OpenClipboard()
		{
			var num = 10;
			while (true) {
				if (OpenClipboard(IntPtr.Zero)) {
					break;
				}
				if (--num == 0) {
					ThrowWin32();
				}
				System.Threading.Thread.Sleep(100);
			}
		}
		public static void SetText(string text)
		{
			OpenClipboard();
			EmptyClipboard();
			IntPtr hGlobal = IntPtr.Zero;
			try {
				var bytes = (text.Length + 1) * 2;
				hGlobal = Marshal.AllocHGlobal(bytes);
				if (hGlobal == IntPtr.Zero) {
					ThrowWin32();
				}
				var target = GlobalLock(hGlobal);
				if (target == IntPtr.Zero) {
					ThrowWin32();
				}
				try {
					Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
				} finally {
					GlobalUnlock(target);
				}
				// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclipboarddata
				if (SetClipboardData(cfUnicodeText, hGlobal) == IntPtr.Zero) {
					ThrowWin32();
				}
				hGlobal = IntPtr.Zero;
			} finally {
				if (hGlobal != IntPtr.Zero) {
					Marshal.FreeHGlobal(hGlobal);
				}
				CloseClipboard();
			}
		}
		// https://github.com/nanoant/ChromeSVG2Clipboard/blob/e135818eb25be5f5f1076a3746b675e9228657d1/ChromeClipboardHost/Program.cs
		static void ThrowWin32()
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		public static string GetText()
		{
			if (!IsClipboardFormatAvailable(cfUnicodeText)) {
				return null;
			}
			IntPtr handle = IntPtr.Zero;
			IntPtr pointer = IntPtr.Zero;
			try {
				OpenClipboard();
				handle = GetClipboardData(cfUnicodeText);
				if (handle == IntPtr.Zero) {
					return null;
				}
				pointer = GlobalLock(handle);
				if (pointer == IntPtr.Zero) {
					return null;
				}
				var size = GlobalSize(handle);
				var buff = new byte[size];
				Marshal.Copy(pointer, buff, 0, size);
				return Encoding.Unicode.GetString(buff).TrimEnd('\0');
			} finally {
				if (pointer != IntPtr.Zero) {
					GlobalUnlock(handle);
				}
				CloseClipboard();
			}
		}
		public static IEnumerable<string> GetFileNames()
		{
			if (!IsClipboardFormatAvailable(15)) {
				var n = GetText();
				if (Directory.Exists(n) || File.Exists(n)) {
					return new string[] { n };
				}
				return null;
			}
			IntPtr handle = IntPtr.Zero;
			try {
				OpenClipboard();
				handle = GetClipboardData(15);
				if (handle == IntPtr.Zero) {
					return null;
				}
				var count = DragQueryFile(handle, unchecked((int)0xFFFFFFFF), null, 0);
				if (count == 0) {
					return Enumerable.Empty<string>();
				}
				var sb = new StringBuilder(260);
				var files = new string[count];
				for (var i = 0; i < count; i++) {
					var charlen = DragQueryFile(handle, i, sb, sb.Capacity);
					var s = sb.ToString();
					if (s.Length > charlen) {
						s = s.Substring(0, charlen);
					}
					files[i] = s;
				}
				return files;
			} finally {
				CloseClipboard();
			}
		}
	}
	public class KeyboardShare
	{
		[DllImport("User32.dll")]
		private static extern IntPtr SetWindowsHookExA(HookID hookID, KeyboardHookProc lpfn, IntPtr hmod,
			int dwThreadId);
		[DllImport("User32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		public delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);
		private event KeyboardHookProc keyhookevent;
		private IntPtr hookPtr;
		public KeyboardShare()
		{
			this.keyhookevent += KeyboardHook_keyhookevent;
		}
		private IntPtr KeyboardHook_keyhookevent(int code, IntPtr wParam, IntPtr lParam)
		{
			KeyStaus ks = (KeyStaus)wParam.ToInt32();
			KeyboardHookStruct khs = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
			KeyEvent ke = ks == KeyStaus.KeyDown || ks == KeyStaus.SysKeyDown ? KeyDown : KeyUp;
			if (ke != null) {
				ke.Invoke(this, new KeyEventArg() {
					Key = khs.Key,
					KeyStaus = ks
				});
			}
			return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
		}
		public void ConfigHook()
		{
			hookPtr = SetWindowsHookExA(HookID.Keyboard_LL, keyhookevent, IntPtr.Zero, 0);
			if (hookPtr == null)
				throw new Exception();
		}
		public delegate void KeyEvent(object sender, KeyEventArg e);
		public event KeyEvent KeyDown;
		public event KeyEvent KeyUp;
	}
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct KeyboardHookStruct
	{
		[FieldOffset(0)] public Key Key;
		[FieldOffset(4)] public int ScanCode;
		[FieldOffset(8)] public int Flags;
		[FieldOffset(12)] public int Time;
		[FieldOffset(16)] public IntPtr dwExtraInfo;
	}
	public enum KeyStaus
	{
		KeyDown = 0x0100,
		KeyUp = 0x0101,
		SysKeyDown = 0x0104,
		SysKeyUp = 0x0105
	}
	public class KeyEventArg
	{
		public Key Key;
		public KeyStaus KeyStaus;
	}
	public enum HookID
	{
		Callwndproc = 4,
		Callwndprocert = 12,
		Cbt = 5,
		Debug = 9,
		Foregroundidle = 11,
		GetMessage = 3,
		JournalPlayback = 1,
		JournalRecord = 0,
		Keyboard = 2,
		Keyboard_LL = 13,
		Mouse = 7,
		MouseLL = 14,
		MsgFilter = -1,
		Shell = 10,
		SysmsgFilter = 6
	}
	public enum Key
	{
		LeftButton = 0x01,
		RightButton = 0x02,
		Cancel = 0x03,
		MiddleButton = 0x04,
		XButton1 = 0x05,
		XButton2 = 0x06,
		BackSpace = 0x08,
		Tab = 0x09,
		Clear = 0x0C,
		Return = 0x0D,
		Enter = Return,
		Shift = 0x10,
		Control = 0x11,
		Menu = 0x12,
		Pause = 0x13,
		CapsLock = 0x14,
		IMEKana = 0x15,
		IMEHanguel = IMEKana,
		IMEHangul = IMEKana,
		IMEJunja = 0x17,
		IMEFinal = 0x18,
		IMEHanja = 0x19,
		IMEKanji = IMEHanja,
		Escape = 0x1B,
		IMEConvert = 0x1C,
		IMENonConvvert = 0x1D,
		IMEAccept = 0x1E,
		IMEModeChange = 0x1F,
		SpaceBar = 0x20,
		PageUp = 0x21,
		PageDown = 0x22,
		End = 0x23,
		Home = 0x24,
		Left = 0x25,
		Up = 0x26,
		Right = 0x27,
		Down = 0x28,
		Select = 0x29,
		Print = 0x2A,
		Execute = 0x2B,
		Snapshot = 0x2C,
		Insert = 0x2D,
		Delete = 0x2E,
		Help = 0x2F,
		Key0 = 0x30,
		Key1 = 0x31,
		Key2 = 0x322,
		Key3 = 0x33,
		Key4 = 0x34,
		Key5 = 0x35,
		Key6 = 0x36,
		Key7 = 0x37,
		Key8 = 0x38,
		Key9 = 0x39,
		KeyA = 0x41,
		KeyB = 0x42,
		KeyC = 0x43,
		KeyD = 0x44,
		KeyE = 0x45,
		KeyF = 0x46,
		KeyG = 0x47,
		KeyH = 0x48,
		KeyI = 0x49,
		KeyJ = 0x4A,
		KeyK = 0x4B,
		KeyL = 0x4C,
		KeyM = 0x4D,
		KeyN = 0x4E,
		KeyO = 0x4F,
		KeyP = 0x50,
		KeyQ = 0x51,
		KeyR = 0x52,
		KeyS = 0x53,
		KeyT = 0x54,
		KeyU = 0x55,
		KeyV = 0x56,
		KeyW = 0x57,
		KeyX = 0x58,
		KeyY = 0x59,
		KeyZ = 0x5A,
		LeftWinKey = 0x5B,
		RightWinKey = 0x5C,
		AppsKey = 0x5D,
		Sleep = 0x5F,
		NumPad0 = 0x60,
		NumPad1 = 0x61,
		NumPad2 = 0x62,
		NumPad3 = 0x63,
		NumPad4 = 0x64,
		NumPad5 = 0x65,
		NumPad6 = 0x66,
		NumPad7 = 0x67,
		NumPad8 = 0x68,
		NumPad9 = 0x69,
		Multiply = 0x6A,
		Add = 0x6B,
		Separator = 0x6C,
		Subtract = 0x6D,
		Decimal = 0x6E,
		Divide = 0x6F,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		F13 = 0x7C,
		F14 = 0x7D,
		F15 = 0x7E,
		F16 = 0x7F,
		F17 = 0x80,
		F18 = 0x81,
		F19 = 0x82,
		F20 = 0x83,
		F21 = 0x84,
		F22 = 0x85,
		F23 = 0x86,
		F24 = 0x87,
		NumLock = 0x90,
		ScrollLock = 0x91,
		OEM92 = 0x92,
		OEM93 = 0x93,
		OEM94 = 0x94,
		OEM95 = 0x95,
		OEM96 = 0x96,
		LeftShfit = 0xA0,
		RightShfit = 0xA1,
		LeftCtrl = 0xA2,
		RightCtrl = 0xA3,
		LeftMenu = 0xA4,
		RightMenu = 0xA5,
		BrowserBack = 0xA6,
		BrowserForward = 0xA7,
		BrowserRefresh = 0xA8,
		BrowserStop = 0xA9,
		BrowserSearch = 0xAA,
		BrowserFavorites = 0xAB,
		BrowserHome = 0xAC,
		BrowserVolumeMute = 0xAD,
		BrowserVolumeDown = 0xAE,
		BrowserVolumeUp = 0xAF,
		MediaNextTrack = 0xB0,
		MediaPreviousTrack = 0xB1,
		MediaStop = 0xB2,
		MediaPlayPause = 0xB3,
		LaunchMail = 0xB4,
		LaunchMediaSelect = 0xB5,
		LaunchApp1 = 0xB6,
		LaunchApp2 = 0xB7,
		OEM1 = 0xBA,
		OEMPlus = 0xBB,
		OEMComma = 0xBC,
		OEMMinus = 0xBD,
		OEMPeriod = 0xBE,
		OEM2 = 0xBF,
		OEM3 = 0xC0,
		OEM4 = 0xDB,
		OEM5 = 0xDC,
		OEM6 = 0xDD,
		OEM7 = 0xDE,
		OEM8 = 0xDF,
		OEM102 = 0xE2,
		IMEProcess = 0xE5,
		Packet = 0xE7,
		Attn = 0xF6,
		CrSel = 0xF7,
		ExSel = 0xF8,
		EraseEOF = 0xF9,
		Play = 0xFA,
		Zoom = 0xFB,
		PA1 = 0xFD,
		OEMClear = 0xFE
	}
	
	public class NativeMethods
	{
		
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);
		
		
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		private static Bitmap CaptureRectangleNative(IntPtr handle, Rectangle rect, bool captureCursor = false)
		{
			if (rect.Width == 0 || rect.Height == 0) {
				return null;
			}

			IntPtr hdcSrc = NativeMethods.GetWindowDC(handle);
			IntPtr hdcDest = NativeMethods.CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(hdcSrc, rect.Width, rect.Height);
			IntPtr hOld = NativeMethods.SelectObject(hdcDest, hBitmap);
			NativeMethods.BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, hdcSrc, rect.X, rect.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);

			if (captureCursor) {
//				try {
//					CursorData cursorData = new CursorData();
//					cursorData.DrawCursor(hdcDest, rect.Location);
//				} catch (Exception e) {
//					DebugHelper.WriteException(e, "Cursor capture failed.");
//				}
			}

			NativeMethods.SelectObject(hdcDest, hOld);
			NativeMethods.DeleteDC(hdcDest);
			NativeMethods.ReleaseDC(handle, hdcSrc);
			Bitmap bmp = Image.FromHbitmap(hBitmap);
			NativeMethods.DeleteObject(hBitmap);

			return bmp;
		}
		
		public static Bitmap CaptureRectangle(Rectangle rect)
		{
			//            if (RemoveOutsideScreenArea)
			//            {
			//                Rectangle bounds = CaptureHelpers.GetScreenBounds();
			//                rect = Rectangle.Intersect(bounds, rect);
			//            }

			return CaptureRectangleNative(rect);
		}
		public static Rectangle GetClientRect(IntPtr handle)
		{
			RECT rect;
			GetClientRect(handle, out  rect);
			Point position = rect.Location;
			ClientToScreen(handle, ref position);
			return new Rectangle(position, rect.Size);
		}
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public int X {
				get {
					return Left;
				}
				set {
					Right -= Left - value;
					Left = value;
				}
			}

			public int Y {
				get {
					return Top;
				}
				set {
					Bottom -= Top - value;
					Top = value;
				}
			}

			public int Width {
				get {
					return Right - Left;
				}
				set {
					Right = value + Left;
				}
			}

			public int Height {
				get {
					return Bottom - Top;
				}
				set {
					Bottom = value + Top;
				}
			}

			public Point Location {
				get {
					return new Point(Left, Top);
				}
				set {
					X = value.X;
					Y = value.Y;
				}
			}

			public Size Size {
				get {
					return new Size(Width, Height);
				}
				set {
					Width = value.Width;
					Height = value.Height;
				}
			}

			public RECT(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public RECT(Rectangle r)
				: this(r.Left, r.Top, r.Right, r.Bottom)
			{
			}

			public static implicit operator Rectangle(RECT r)
			{
				return new Rectangle(r.Left, r.Top, r.Width, r.Height);
			}

			public static implicit operator RECT(Rectangle r)
			{
				return new RECT(r);
			}

			public static bool operator ==(RECT r1, RECT r2)
			{
				return r1.Equals(r2);
			}

			public static bool operator !=(RECT r1, RECT r2)
			{
				return !r1.Equals(r2);
			}

			public bool Equals(RECT r)
			{
				return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
			}

			public override bool Equals(object obj)
			{
				//            if (obj is RECT rect)
				//            {
				//                return Equals(rect);
				//            }
//
				//            if (obj is Rectangle rectangle)
				//            {
				//                return Equals(new RECT(rectangle));
				//            }

				return false;
			}

			public override int GetHashCode()
			{
				return ((Rectangle)this).GetHashCode();
			}

			public override string ToString()
			{
				return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
			}
		}
		
		public static Bitmap CaptureWindow(IntPtr handle)
		{
			if (handle.ToInt32() > 0) {
				Rectangle rect;

				//if (CaptureClientArea) {
				rect = NativeMethods.GetClientRect(handle);
//				} else {
//					rect = CaptureHelpers.GetWindowRectangle(handle);
//				}

				//bool isTaskbarHide = false;

				//                try
				//                {
				//                    if (AutoHideTaskbar)
				//                    {
				//                        isTaskbarHide = NativeMethods.SetTaskbarVisibilityIfIntersect(false, rect);
				//                    }
//
				return CaptureRectangle(rect);
				//                }
				//                finally
				//                {
				//                    if (isTaskbarHide)
				//                    {
				//                        NativeMethods.SetTaskbarVisibility(true);
				//                    }
				//                }
			}

			return null;
		}
		public static Rectangle GetWindowRect(IntPtr handle)
		{
			RECT rect;
			GetWindowRect(handle, out  rect);
			return rect;
		}
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		private static Bitmap CaptureRectangleNative(Rectangle rect, bool captureCursor = false)
		{
			IntPtr handle = NativeMethods.GetDesktopWindow();
			return CaptureRectangleNative(handle, rect, captureCursor);
		}
		public static void WriteBitmapToFile(string filename, Bitmap bitmap)
		{
			bitmap.Save(filename, ImageFormat.Png);
		}
		public static void GetScreenshot()
		{
			POINT p;
			GetCursorPos(out p);
			
			IntPtr hwnd = WindowFromPoint(p);//handle here
			var bitmap = CaptureWindow(hwnd);
			var src = GetDesktopPath(string.Format("{0}-{1}.png", DateTime.Now.ToString("yyyy-MM-dd"), GenerateRandomString(6)));
			WriteBitmapToFile(src, bitmap);
			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			bitmap.Dispose();
		}
		public static string GetDesktopPath(string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		public static string GenerateRandomString(int length)
		{
			var coupon = new StringBuilder();
			var rng = new RNGCryptoServiceProvider();
			var rnd = new byte[1];
			var n = 0;
			while (n < length) {
				rng.GetBytes(rnd);
				var c = (char)rnd[0];
				if (c <= 122 && c >= 97) {
					++n;
					coupon.Append(c);
				}
			}

			return coupon.ToString();
		}
		public	struct POINT
		{
			public int X;
			public int Y;
		}
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT lpPoint);
		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(POINT p);
	}
}
