
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace 视频
{
	/// <summary>
	/// Description of Keys.
	/// </summary>
	public class Keys
	{
		// To read color from screen
		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);



		// To carry out mouse movement and simulation
		[DllImport("user32.dll")]
		private static extern void mouse_event(
			UInt32 dwFlags, // motion and click options
			UInt32 dx, // horizontal position or change
			UInt32 dy, // vertical position or change
			UInt32 dwData, // wheel movement
			IntPtr dwExtraInfo // application-defined information
		);

		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);

		private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

		public static void LeftClick(int x, int y)
		{
			SetCursorPos(x, y);
			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
		}
		public static uint GetColor(int x, int y)
		{
          
			IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(hdc, x, y);
			ReleaseDC(IntPtr.Zero, hdc);
//            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
//                (int)(pixel & 0x0000FF00) >> 8,
//                (int)(pixel & 0x00FF0000) >> 16);
//            return color.ToArgb();
			return pixel;
		}
	

		// https://github.com/Spazy7/SQUAD-INTERNAL-HACK-ESP-AIMBOT-SOURCE/blob/0c399d72da93ac9cd5ea3ee45b6bf607df2ced0f/SquadInternal/Squad-AsI/Api.cs
		// https://github.com/Aurora-FN/Launcher/blob/2a25686d9c487a2c25727188c6f44c2d1d350f63/AuroraLauncher/ProcessHelper.cs
		// https://github.com/ARLM-Attic/toapi/blob/7c6bd9e4a7bd9ad3cb4b6aa7ec8d7f1db0ec8e1b/Kernel32/Kernel32_Thread.cs
		
	}
	  public delegate uint THREAD_START_ROUTINE(IntPtr lpThreadParameter);

	// https://github.com/Wiladams/TOAPI/blob/70c0dd060970853efda5e6d02ed0951571dfa9ec/TOAPI.Kernel32/Kernel32_Thread.cs
	// https://github.com/NgonPhi/BTL-OS/blob/b98adafd6e05ec044c1f5896336de52d61075fff/Main/Main/ThreadM.cs
	// https://github.com/VanessaNiculae/Thread-App/blob/30e840b311e24f007f34245c1a2d797a255ca0e7/lab5/WinApiClass.cs
	public static class Kernel32
	{
		// AttachThreadInput
		// CreateRemoteThread
        
		// CreateThread
		[DllImportAttribute("kernel32.dll", EntryPoint = "CreateThread")]
		public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, 
			uint dwStackSize, THREAD_START_ROUTINE lpStartAddress, 
			IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

		// ExitThread
		[DllImportAttribute("kernel32.dll", EntryPoint = "ExitThread")]
		public static extern void ExitThread(uint dwExitCode);

		// GetCurrentThread
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentThread();

		// GetCurrentThreadId
		[DllImport("kernel32.dll")]
		public static extern uint GetCurrentThreadId();

		// GetExitCodeThread
		// GetThreadId
		[DllImportAttribute("kernel32.dll", EntryPoint = "GetThreadId")]
		public static extern uint GetThreadId(IntPtr Thread);

		// GetThreadIOPendingFlag
		// GetThreadPriority
		// GetThreadPriorityBoost
        
		// GetThreadTimes

		// OpenThread
		[DllImport("kernel32.dll", EntryPoint = "OpenThread")]
		public static extern IntPtr OpenThread(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, 
			uint dwThreadId);

		// ResumeThread
		[DllImport("kernel32.dll")]
		public static extern int ResumeThread(IntPtr hThread);

		// SetThreadAffinityMask
		// SetThreadIdealProcessor
		// SetThreadPriority
		// SetThreadPriorityBoost
		// SetThreadStackGuarantee

		// Sleep
		[DllImportAttribute("kernel32.dll", EntryPoint = "Sleep")]
		public static extern void Sleep(uint dwMilliseconds);

		// SleepEx
		[DllImportAttribute("kernel32.dll", EntryPoint = "SleepEx")]
		public static extern uint SleepEx(uint dwMilliseconds, [MarshalAs(UnmanagedType.Bool)] bool bAlertable);

		// SuspendThread
		[DllImport("kernel32.dll")]
		public static extern int SuspendThread(IntPtr hThread);

		// SwithToThread
		// TerminateThread
		// ThreadProc
		// TlsAlloc
		// TlsFree
		// TlsGetValue
		// TlsSetValue
		// WaitForInputIdle

		// Thread Pooling
		// BindIoCompletionCallback
		// QueueUserWorkItem


		[DllImport("kernel32.dll")]
		public static extern uint GetThreadLocale();

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool SetThreadLocale(uint Locale);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern Int32 WaitForSingleObject(SafeWaitHandle hHandle, Int32 dwMilliseconds);

	}
}
