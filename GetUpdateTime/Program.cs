using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GetUpdateTime
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Task<string> task = GetUpdateTimeAllAsync(args);
            Clipboard.SetText(task.Result);
        }

        static async Task<string> GetUpdateTimeAllAsync(string[] args)
        {
            string[] updateTimes = await Task.WhenAll(args.OrderBy(x => x).Select(GetUpdateTimeAsync));
            return string.Join("\r\n", updateTimes);
        }

        static async Task<string> GetUpdateTimeAsync(string path)
        {
            Func<string, DateTime> GetLastWriteTime;

            if (File.Exists(path))
            {
                GetLastWriteTime = File.GetLastWriteTime;
            }
            else if (Directory.Exists(path))
            {
                GetLastWriteTime = Directory.GetLastWriteTime;
            }
            else
            {
                return path;
            }

            return await Task.Run<string>(() => $"{path}\t{Path.GetFileName(path)}\t{GetLastWriteTime(path).ToString("yyyy/MM/dd HH:mm:ss")}");
        }

    }
}
