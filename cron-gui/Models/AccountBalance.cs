using System.Collections.Generic;

namespace Cron.Models
{
    public class AccountBalance
    {
        public UInt160 Account { get; set; }
        public List<AccountAssetBalance> Balances { get; set; }
    }
}
