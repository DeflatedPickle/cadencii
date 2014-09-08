using System;
using System.IO;

namespace cadencii.core2
{
	
    public static class debug
    {
        private static StreamWriter s_debug_out = null;
        private static string s_path = "";

        public static void force_logfile_path(string path)
        {
            try {
                if (s_debug_out != null) {
                    s_debug_out.Close();
                    s_debug_out = new StreamWriter(path);
                }
            } catch {
            }
            s_path = path;
        }

        public static void push_log(string s)
        {
            try {
                if (s_debug_out == null) {
                    if (s_path == "") {
                        s_debug_out = new StreamWriter(Path.Combine(System.Windows.Forms.Application.StartupPath, "run.log"));
                    } else {
                        s_debug_out = new StreamWriter(s_path);
                    }
                    s_debug_out.AutoFlush = true;
                    s_debug_out.WriteLine("************************************************************************");
                    s_debug_out.WriteLine("  Date: " + DateTime.Now.ToString());
                    s_debug_out.WriteLine("------------------------------------------------------------------------");
                }
                s_debug_out.WriteLine(s);
            } catch (Exception ex) {
                Console.WriteLine("org.kbinani.debug.push_log; log file I/O Exception");
            }
            Console.WriteLine(s);
        }

        public static void close()
        {
            if (s_debug_out != null) {
                s_debug_out.Close();
                s_debug_out = null;
                s_path = "";
            }
        }
    }

}

