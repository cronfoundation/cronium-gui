using CommandLine;
using CommandLine.Text;

namespace Cron
{ 
    public class CLSettings
    {
        [Option]
        public string Wallet { set; get; }
        [Option]
        public string Password { set; get; }
    }
}