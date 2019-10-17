using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace GCodeSender.Util
{
    static class UpdateCheck
    {
        static WebClient client;
        static Regex versionRegex = new Regex("\"name\":\\s*\"v([0-9\\.]+)\",");
        //static Regex versionRegex = new Regex("\"tag_name\":\\s*\"v([0-9\\.]+)\",");
        static Regex releaseRegex = new Regex("\"html_url\":\\s*\"([^\"]*)\",");

        public static void CheckForUpdate()
        {
            MainWindow.Logger.Info("++++++ CHECKING FOR UPDATE ++++++");
            client = new WebClient();
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15";
            client.Proxy = null;
            client.DownloadStringCompleted += Client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri("https://api.github.com/repos/3RD-Dimension/3RDD-GCode-Sender-Issues/releases/latest"));
        }

        private static void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MainWindow.Logger.Warn("Error while checking for new version:");
                    MainWindow.Logger.Warn(e.Error.Message);
                      return;
                }

                Match m = versionRegex.Match(e.Result);

                if (!m.Success)
                {
                    MainWindow.Logger.Warn("No matching tag_id found");
                    return;
                }

                Version latest;

                if (!Version.TryParse(m.Groups[1].Value, out latest))
                {
                    MainWindow.Logger.Warn($"Error while parsing version string <{m.Groups[1].Value}>");
                    return;
                }

                MainWindow.Logger.Info($"Latest version on GitHub: {latest}");

                if (System.Reflection.Assembly.GetEntryAssembly().GetName().Version < latest)
                {
                    Match urlMatch = releaseRegex.Match(e.Result);

                    string url = "https://github.com/3RD-Dimension/3RDD-GCode-Sender-Issues/releases";

                    if (urlMatch.Success)
                    {
                        url = urlMatch.Groups[1].Value;
                    }

                    if (MessageBox.Show($"There is an update available! Version {latest}\nOpen in browser?", "Update", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        System.Diagnostics.Process.Start(url);                    
                }
            }
            catch { }   //update check is non-critical and should never interrupt normal application operation
        }
    }
}
