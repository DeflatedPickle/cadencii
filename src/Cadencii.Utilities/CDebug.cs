using System;
using System.IO;
using Cadencii.Utilities;

namespace cadencii
{
	public static class CDebug
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
				Logger.StdErr("Debug#WriteLine; ex=" + ex);
				Logger.write(typeof(CDebug) + ".WriteLine; ex=" + ex + "\n");
			}
			Logger.StdOut(message);
#endif
		}
	}
}

