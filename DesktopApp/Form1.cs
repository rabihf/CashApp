using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static HelperLibrary.Cash;
using static HelperLibrary.Cash.CurrencyEnum;

namespace DesktopApp
{
    public partial class Form1 : Form
    {
        private readonly IEnumerable<int> _billsLBP = CreateEnumsList(typeof(CashLBPEnum), true);
        private readonly IEnumerable<int> _billsUSD = CreateEnumsList(typeof(CashUSDEnum), true);

        private decimal Amount => numericUpDown100.Value * decimal.Parse(button100.Text.Trim(',')) +
                                  numericUpDown50.Value * decimal.Parse(button50.Text.Trim(',')) +
                                  numericUpDown20.Value * decimal.Parse(button20.Text.Trim(',')) +
                                  numericUpDown10.Value * decimal.Parse(button10.Text.Trim(',')) +
                                  numericUpDown5.Value * decimal.Parse(button5.Text.Trim(',')) +
                                  numericUpDown1.Value * decimal.Parse(button1.Text.Trim(','));

        private decimal USDRate =>
            decimal.TryParse(USDRateTextBox.Text, out _) ? decimal.Parse(USDRateTextBox.Text) : 0; // 110_000;


        private string CurrencyString => CurrencyComboBox.Text;
        private const CurrencyEnum DefaultCurrency = LBP;
        private const string USDStrFormat = @"{0:N2} ";
        private const string LBPStrFormat = @"{0:N0} ";
        private readonly Array _currencyEnums = typeof(CurrencyEnum).GetEnumValues();

        public Form1()
        {
            InitializeComponent();
            SetupAmountLabel();
            SetupCurrencyComboBox();
            SetupButtonsLabel();
            USDRateTextBox.Text = $@"{DefaultUSDRate:N0}";
        }

        private const decimal DefaultUSDRate = 110_000;

        private void SetupButtonsLabel()
        {
            switch (CurrencyComboBox.Text)
            {
                case "LBP":
                    button100.Text = $@"{_billsLBP.ElementAt(0):N0}"; // $@"{USDRate:N0}"
                    button50.Text  = $@"{_billsLBP.ElementAt(1):N0}";
                    button20.Text  = $@"{_billsLBP.ElementAt(2):N0}";
                    button10.Text  = $@"{_billsLBP.ElementAt(3):N0}";
                    button5.Text   = $@"{_billsLBP.ElementAt(4):N0}";
                    button1.Text   = $@"{_billsLBP.ElementAt(5):N0}";
                    break;
                case "USD":
                    button100.Text =$@"{_billsUSD.ElementAt(0):N0}";
                    button50.Text  =$@"{_billsUSD.ElementAt(1):N0}";
                    button20.Text  =$@"{_billsUSD.ElementAt(2):N0}";
                    button10.Text  =$@"{_billsUSD.ElementAt(3):N0}";
                    button5.Text   =$@"{_billsUSD.ElementAt(4):N0}";
                    button1.Text   =$@"{_billsUSD.ElementAt(5):N0}";
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
            SetupButtonsLabel();
            if (CurrencyComboBox.Text != DefaultCurrency.ToString())
            {
                AmountLabel.Text = string.Format(USDStrFormat, Amount) + CurrencyString;
                AmountLabelEq.Visible = true;
                AmountLabelEq.Text = string.Format(LBPStrFormat, Amount * USDRate) + DefaultCurrency;
            }
            else
            {
                AmountLabel.Text = string.Format(LBPStrFormat, Amount) + CurrencyString;
                AmountLabelEq.Visible = false;
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
            USDRateTextBox.Text = $@"{USDRate:N0}";
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
    }
}