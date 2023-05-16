using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelperLibrary;
using static HelperLibrary.Cash;

namespace DesktopApp
{
    public partial class Form1 : Form
    {
        private Cash MyCash => new Cash(CurrentCurrency,
            (int)numericUpDown100.Value,
            (int)numericUpDown50.Value,
            (int)numericUpDown20.Value,
            (int)numericUpDown10.Value,
            (int)numericUpDown5.Value,
            (int)numericUpDown1.Value);

        private static Cashes MyCashes { get; set; }

        private decimal Amount => numericUpDown100.Value * decimal.Parse(button100.Text.Trim(',')) +
                                  numericUpDown50.Value * decimal.Parse(button50.Text.Trim(',')) +
                                  numericUpDown20.Value * decimal.Parse(button20.Text.Trim(',')) +
                                  numericUpDown10.Value * decimal.Parse(button10.Text.Trim(',')) +
                                  numericUpDown5.Value * decimal.Parse(button5.Text.Trim(',')) +
                                  numericUpDown1.Value * decimal.Parse(button1.Text.Trim(','));

        private decimal USDRate =>
            decimal.TryParse(USDRateTextBox.Text, out _)
                ? decimal.Parse(USDRateTextBox.Text)
                : DefaultUSDRate;

        private const decimal DefaultUSDRate = (int)CurrencyEnum.USD;
        private string CurrencyString => CurrencyComboBox.Text;
        private readonly Array _currencyEnums = typeof(CurrencyEnum).GetEnumValues();
        private CurrencyEnum CurrentCurrency => (CurrencyEnum)CurrencyComboBox.SelectedItem;

        public Form1(Cashes cashes)
        {
            InitializeComponent();
            MyCashes = cashes;
            SetupAmountLabel();
            SetupCurrencyComboBox();
            SetupButtonsLabel();
            USDRateTextBox.Text = string.Format(LBPStrFormat, DefaultUSDRate);
            BalanceLabelLBP.Text = MyCashes.CashLBPAmountString;
            BalanceLabelUSD.Text = MyCashes.CashUSDAmountString;
            Width = MinimumSize.Width + 10;
        }

        public sealed override Size MinimumSize
        {
            get => base.MinimumSize;
            set => base.MinimumSize = value;
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
            if (CurrencyComboBox.Text != DefaultCurrency.ToString())
            {
                AmountLabel.Text = string.Format(USDStrFormat, Amount) + @" " + CurrencyString;
                AmountLabelEq.Visible = true;
                AmountLabelEq.Text = string.Format(LBPStrFormat, Amount * USDRate) + @" " + DefaultCurrency;
            }
            else
            {
                AmountLabel.Text = string.Format(LBPStrFormat, Amount) + @" " + CurrencyString;
                AmountLabelEq.Visible = false;
            }
        }

        private void UpdateBalance()
        {
            BalanceLabelLBP.Text = MyCashes.CashLBPAmountString;
            if (MyCashes.CashLBP != null)
            {
                sp1p2_100.Text = MyCashes.CashLBP.QtyHundred.ToString();
                sp1p2_50.Text = MyCashes.CashLBP.QtyFifty.ToString();
                sp1p2_20.Text = MyCashes.CashLBP.QtyTwenty.ToString();
                sp1p2_10.Text = MyCashes.CashLBP.QtyTen.ToString();
                sp1p2_5.Text = MyCashes.CashLBP.QtyFive.ToString();
                sp1p2_1.Text = MyCashes.CashLBP.QtyOne.ToString();
            }

            BalanceLabelUSD.Text = MyCashes.CashUSDAmountString;
            if (MyCashes.CashUSD != null)
            {
                sp2p2_100.Text = MyCashes.CashUSD.QtyHundred.ToString();
                sp2p2_50.Text = MyCashes.CashUSD.QtyFifty.ToString();
                sp2p2_20.Text = MyCashes.CashUSD.QtyTwenty.ToString();
                sp2p2_10.Text = MyCashes.CashUSD.QtyTen.ToString();
                sp2p2_5.Text = MyCashes.CashUSD.QtyFive.ToString();
                sp2p2_1.Text = MyCashes.CashUSD.QtyOne.ToString();
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
            ResetNums();
        }

        private void ResetNums()
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
            MyCashes.Add(MyCash);
            ResetNums();
            UpdateBalance();
        }

        private void buttonSub_Click(object sender, EventArgs e)
        {
            try
            {
                MyCashes.Subtract(MyCash);
            }
            catch (Cashes.CashesException ex)
            {
                MessageBox.Show(ex.Message, @"Subtraction Error", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Subtraction Error", MessageBoxButtons.OK);
            }
            finally
            {
                ResetNums();
                UpdateBalance();
            }
        }


        private void buttonShowHide_Click(object sender, EventArgs e)
        {
            switch (splitContainer1.Panel2.Visible)
            {
                case true:
                    buttonShowHide.Text = @">";
                    splitContainer1.Panel2.Hide();
                    splitContainer2.Panel2.Hide();
                    Width = MinimumSize.Width + 10 ;// 196;
                    break;
                case false:
                    buttonShowHide.Text = @"<";
                    splitContainer1.Panel2.Show();
                    splitContainer2.Panel2.Show();
                    Width = MaximumSize.Width; // 600;
                    break;
            }
        }
    }
}