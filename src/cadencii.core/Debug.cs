using System;
using System.IO;

namespace cadencii
{
	public static class Debug
	{
#if DEBUG
		private static StreamWriter mDebugLog = null;
#endif

		public static void WriteLine(string message)
		{
#if DEBUG
			try {
				if (mDebugLog == null) {
					string log_file = Path.Combine(PortUtil.getApplicationStartupPath(), "log.txt");
					mDebugLog = new StreamWriter(log_file);
				}
				mDebugLog.WriteLine(message);
			} catch (Exception ex) {
				serr.println("Debug#debugWriteLine; ex=" + ex);
				Logger.write(typeof(Debug) + ".debugWriteLine; ex=" + ex + "\n");
			}
			sout.println(message);
#endif
		}
	}
}

