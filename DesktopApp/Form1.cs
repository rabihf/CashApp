using System;
using System.Linq;
using System.Windows.Forms;
using HelperLibrary;
using static HelperLibrary.Cash;

namespace DesktopApp
{
    public partial class Form1 : Form
    {
        private Cash MyCash { get; set; } // = new Cash();

        private static Cashes MyCashes { get; set; } = new Cashes();

        private decimal Amount => numericUpDown100.Value * decimal.Parse(button100.Text.Trim(',')) +
                                  numericUpDown50.Value * decimal.Parse(button50.Text.Trim(',')) +
                                  numericUpDown20.Value * decimal.Parse(button20.Text.Trim(',')) +
                                  numericUpDown10.Value * decimal.Parse(button10.Text.Trim(',')) +
                                  numericUpDown5.Value * decimal.Parse(button5.Text.Trim(',')) +
                                  numericUpDown1.Value * decimal.Parse(button1.Text.Trim(','));

        private static decimal BalanceLBP => MyCashes.CashLBPAmount;
        private static decimal BalanceUSD => MyCashes.CashUSDAmount;

        private decimal USDRate =>
            decimal.TryParse(USDRateTextBox.Text, out _)
                ? decimal.Parse(USDRateTextBox.Text)
                : DefaultUSDRate; // 110_000;

        private const decimal DefaultUSDRate = 110_000;
        private string CurrencyString => CurrencyComboBox.Text;
        private readonly Array _currencyEnums = typeof(CurrencyEnum).GetEnumValues();
        private CurrencyEnum CurrentCurrency => (CurrencyEnum)CurrencyComboBox.SelectedItem;

        public Form1(Cash cash)
        {
            InitializeComponent();
            MyCash = cash;
            SetupAmountLabel();
            SetupCurrencyComboBox();
            SetupButtonsLabel();
            USDRateTextBox.Text = string.Format(LBPStrFormat, DefaultUSDRate);
        }

        private void SetupButtonsLabel()
        {
            switch (CurrencyComboBox.Text)
            {
                case "LBP":
                    button100.Text = $@"{MyCash.BillsLBP.ElementAt(0):N0}"; // $@"{USDRate:N0}"
                    button50.Text = $@"{MyCash.BillsLBP.ElementAt(1):N0}";
                    button20.Text = $@"{MyCash.BillsLBP.ElementAt(2):N0}";
                    button10.Text = $@"{MyCash.BillsLBP.ElementAt(3):N0}";
                    button5.Text = $@"{MyCash.BillsLBP.ElementAt(4):N0}";
                    button1.Text = $@"{MyCash.BillsLBP.ElementAt(5):N0}";
                    break;
                case "USD":
                    button100.Text = $@"{MyCash.BillsUSD.ElementAt(0):N0}";
                    button50.Text = $@"{MyCash.BillsUSD.ElementAt(1):N0}";
                    button20.Text = $@"{MyCash.BillsUSD.ElementAt(2):N0}";
                    button10.Text = $@"{MyCash.BillsUSD.ElementAt(3):N0}";
                    button5.Text = $@"{MyCash.BillsUSD.ElementAt(4):N0}";
                    button1.Text = $@"{MyCash.BillsUSD.ElementAt(5):N0}";
                    break;
            }
        }

        private void SetupCurrencyComboBox()
        {
            CurrencyComboBox.DataSource = _currencyEnums;
        }

        private void SetupAmountLabel()
        {
            UpdateAmount();
        }

        private void button100_Click(object sender, EventArgs e)
        {
            numericUpDown100.Value += 1;
            UpdateAmount();
        }

        private void button50_Click(object sender, EventArgs e)
        {
            numericUpDown50.Value += 1;
            UpdateAmount();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            numericUpDown20.Value += 1;
            UpdateAmount();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            numericUpDown10.Value += 1;
            UpdateAmount();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            numericUpDown5.Value += 1;
            UpdateAmount();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 1;
            UpdateAmount();
        }

        private void CurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtonsLabel();
            UpdateAmount();
        }

        private void numericUpDown100_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void UpdateAmount()
        {
            //var thisAmount = MyCashes.CashDict[CurrencyEnum.LBP].Amount;
            if (CurrencyComboBox.Text != DefaultCurrency.ToString())
            {
                AmountLabel.Text = string.Format(USDStrFormat, Amount) + @" " + CurrencyString;
                AmountLabelEq.Visible = true;
                AmountLabelEq.Text = string.Format(LBPStrFormat, Amount * USDRate) + @" " + DefaultCurrency;
                BalanceLabel.Text = string.Format(USDStrFormat, BalanceLBP) + @" " + CurrencyString;
                BalanceLabelEq.Visible = true;
                BalanceLabelEq.Text = string.Format(LBPStrFormat, BalanceUSD) + @" " + DefaultCurrency;
            }
            else
            {
                AmountLabel.Text = string.Format(LBPStrFormat, Amount) + @" " + CurrencyString;
                AmountLabelEq.Visible = false;
                BalanceLabel.Text = string.Format(LBPStrFormat, BalanceLBP) + @" " + CurrencyString;
                BalanceLabelEq.Visible = true;
            }
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmount();
        }

        private void USDRateTextBox_Leave(object sender, EventArgs e)
        {
            USDRateTextBox.Text = string.Format(LBPStrFormat, USDRate); // $@"{USDRate:N0}";
            UpdateAmount();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            numericUpDown100.Text = @"0";
            numericUpDown50.Text = @"0";
            numericUpDown20.Text = @"0";
            numericUpDown10.Text = @"0";
            numericUpDown5.Text = @"0";
            numericUpDown1.Text = @"0";
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // Form add = new Form1();
            // add.Text = @"New Form";
            // add.StartPosition = FormStartPosition.Manual;// (StartPosition == FormStartPosition.Manual) + new Point(Width, 0);
            // add.Location = Location + new Size(Width, 0);
            //
            // add.Show();
            // // add.ShowDialog(this);
            // using (Form form = new Form1())
            // {
            //     form.ShowDialog(this);
            // } 
            Form1 form1 = new Form1(new Cash());
            // MyCash = new Cash(CurrentCurrency, Amount);
            // MyCashes.Add(MyCash);
            form1.ShowDialog();
            UpdateAmount();
        }

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            // Form1 form1 = new Form1();
            // DialogResult dialogResult; // = ShowDialog();
            // form1.ShowDialog() == DialogResult.OK
            if (DialogResult == DialogResult.OK)
            {
                // MyCashes = new Cashes();
                MyCashes = MyCash + new Cash(CurrentCurrency, Amount);
                // MyCashes.Add(MyCash);
                MessageBox.Show(MyCashes.ToString(), @"ttt", MessageBoxButtons.OK);
                UpdateAmount();
            }
        }
    }
}