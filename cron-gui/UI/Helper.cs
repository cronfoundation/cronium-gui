using Akka.Actor;
using Cron.Ledger;
using Cron.Network.P2P;
using Cron.Network.P2P.Payloads;
using Cron.Properties;
using Cron.SmartContract;
using Cron.Wallets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Text;

namespace Cron.UI
{
    internal static class Helper
    {
        public static AssetDescriptor CustomAssetDescriptor(UInt256 asset_id)
        {
            AssetDescriptor desc = new AssetDescriptor(asset_id);
            if (desc != null)
            {
                if (desc.AssetName == "NEO")
                    desc.AssetName = "CRONIUM";
                if (desc.AssetName == "NeoGas")
                    desc.AssetName = "CRON";

            }
            return desc;
        }

        public static string CustomGetAssetName(AssetState state,CultureInfo culture = null)
        {
            string name = state.GetName();

            if (name != null)
            {
                if (name == "NEO")
                    name = "CRONIUM";
                if (name == "NeoGas")
                    name = "CRON";

            }
            return name;
        }
        private static Dictionary<Type, Form> tool_forms = new Dictionary<Type, Form>();

        private static void Helper_FormClosing(object sender, FormClosingEventArgs e)
        {
            tool_forms.Remove(sender.GetType());
        }

        public static void Show<T>() where T : Form, new()
        {
            Type t = typeof(T);
            if (!tool_forms.ContainsKey(t))
            {
                tool_forms.Add(t, new T());
                tool_forms[t].FormClosing += Helper_FormClosing;
            }
            tool_forms[t].Show();
            tool_forms[t].Activate();
        }

        public static void SignAndShowInformation(Transaction tx)
        {
            if (tx == null)
            {
                MessageBox.Show(Strings.InsufficientFunds);
                return;
            }
            ContractParametersContext context;
            try
            {
                context = new ContractParametersContext(tx);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(Strings.UnsynchronizedBlock);
                return;
            }
            Program.CurrentWallet.Sign(context);
            if (context.Completed)
            {
                tx.Witnesses = context.GetWitnesses();
                Program.CurrentWallet.ApplyTransaction(tx);
                Program.CronSystem.LocalNode.Tell(new LocalNode.Relay { Inventory = tx });
                InformationBox.Show(tx.Hash.ToString(), Strings.SendTxSucceedMessage, Strings.SendTxSucceedTitle);
            }
            else
            {
                InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
            }
        }

        public static bool CostRemind(Fixed8 SystemFee, Fixed8 NetFee)
        {
            NetFeeDialog frm = new NetFeeDialog(SystemFee, NetFee);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
