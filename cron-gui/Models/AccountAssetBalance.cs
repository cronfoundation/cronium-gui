using Cron.Ledger;

namespace Cron.Models
{
    public class AccountAssetBalance
    {
        public AssetState Asset { get; set; }
        public Fixed8 Balance { get; set; }
    }
}
