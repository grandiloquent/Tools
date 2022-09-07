using System;
namespace 视频
{
	public static class Games
	{
		public static IntPtr _thread1 = IntPtr.Zero;
		public static bool _threadRun = false;
		public static void FashiStrong()
		{
			Console.WriteLine("法师强化");
			IntPtr thread = IntPtr.Zero;
			if (_thread1 == IntPtr.Zero) {
				_thread1 = Kernel32.CreateThread(IntPtr.Zero, 0, new THREAD_START_ROUTINE((v) => {
					int count = 0;
					while (true) {
						Shared.SendKey(0x32);
						Kernel32.Sleep(1000);
						
						if (count == 9) {
							count = 0;
							
							Shared.SendKey(0x31);
							Kernel32.Sleep(1000);
							Shared.SendKey(0x33);
							Kernel32.Sleep(1000);
						}
						count++;
					}
					return 0;
				}), IntPtr.Zero, 0, thread);
				_threadRun = true;
			} else {
				if (_threadRun) {
					Kernel32.SuspendThread(_thread1);
				} else {
					Kernel32.ResumeThread(_thread1);
				}
				_threadRun = !_threadRun;
			}
		}
	}
}