using System;
using System.Globalization;
using System.Windows.Forms;

namespace ScientificCalculator
{
    public partial class CalculatorForm : Form
    {
        #region 私有变量
        private double currentValue = 0;
        private double previousValue = 0;
        private string currentOperation = "";
        private bool operationPending = false;
        private bool lastOperationWasEquals = false;
        private double memoryValue = 0;
        private bool hasMemoryValue = false;
        private bool newNumberFlag = true;
        private const int MAX_DIGITS = 15;
        #endregion

        #region 构造函数
        public CalculatorForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            ResetCalculator();
        }
        #endregion

        #region 数字按钮事件处理
        private void btnNumber_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    string digit = btn.Text;
                    InputNumber(digit);
                }
            }
            catch (Exception ex)
            {
                HandleError("输入错误: " + ex.Message);
            }
        }

        private void InputNumber(string digit)
        {
            try
            {
                if (txtDisplay.Text.Length >= MAX_DIGITS)
                {
                    return; // 防止输入过长数字
                }

                if (newNumberFlag || txtDisplay.Text == "0" || lastOperationWasEquals)
                {
                    txtDisplay.Text = digit;
                    newNumberFlag = false;
                    lastOperationWasEquals = false;
                }
                else
                {
                    txtDisplay.Text += digit;
                }

                currentValue = ParseDisplayValue();
            }
            catch (Exception)
            {
                HandleError("数字输入错误");
            }
        }
        #endregion

        #region 小数点处理
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            try
            {
                if (newNumberFlag || lastOperationWasEquals)
                {
                    txtDisplay.Text = "0.";
                    newNumberFlag = false;
                    lastOperationWasEquals = false;
                }
                else if (!txtDisplay.Text.Contains("."))
                {
                    txtDisplay.Text += ".";
                }
                
                currentValue = ParseDisplayValue();
            }
            catch (Exception)
            {
                HandleError("小数点输入错误");
            }
        }
        #endregion

        #region 运算符处理
        private void btnOperation_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    string operation = btn.Text;
                    PerformOperation(operation);
                }
            }
            catch (Exception ex)
            {
                HandleError("运算错误: " + ex.Message);
            }
        }

        private void PerformOperation(string operation)
        {
            try
            {
                if (operationPending && !lastOperationWasEquals)
                {
                    CalculateResult();
                }

                previousValue = currentValue;
                currentOperation = operation;
                operationPending = true;
                newNumberFlag = true;
                lastOperationWasEquals = false;
            }
            catch (Exception)
            {
                HandleError("运算设置错误");
            }
        }
        #endregion

        #region 等号处理
        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                if (operationPending)
                {
                    CalculateResult();
                    operationPending = false;
                    lastOperationWasEquals = true;
                    newNumberFlag = true;
                }
            }
            catch (Exception ex)
            {
                HandleError("计算错误: " + ex.Message);
            }
        }

        private void CalculateResult()
        {
            try
            {
                double result = 0;
                bool validOperation = true;

                switch (currentOperation)
                {
                    case "+":
                        result = previousValue + currentValue;
                        break;
                    case "-":
                        result = previousValue - currentValue;
                        break;
                    case "×":
                        result = previousValue * currentValue;
                        break;
                    case "÷":
                        if (currentValue == 0)
                        {
                            HandleError("除数不能为零");
                            return;
                        }
                        result = previousValue / currentValue;
                        break;
                    case "x^y":
                        if (previousValue == 0 && currentValue < 0)
                        {
                            HandleError("0的负数次幂未定义");
                            return;
                        }
                        result = Math.Pow(previousValue, currentValue);
                        break;
                    default:
                        validOperation = false;
                        break;
                }

                if (validOperation)
                {
                    if (double.IsInfinity(result))
                    {
                        HandleError("结果溢出");
                        return;
                    }
                    if (double.IsNaN(result))
                    {
                        HandleError("未定义的数学运算");
                        return;
                    }

                    currentValue = result;
                    DisplayValue(result);
                }
            }
            catch (OverflowException)
            {
                HandleError("数值溢出");
            }
            catch (Exception)
            {
                HandleError("计算错误");
            }
        }
        #endregion

        #region 科学函数处理
        private void btnFunction_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    string function = btn.Text;
                    ApplyFunction(function);
                }
            }
            catch (Exception ex)
            {
                HandleError("函数计算错误: " + ex.Message);
            }
        }

        private void ApplyFunction(string function)
        {
            try
            {
                double result = 0;
                double input = currentValue;
                bool validFunction = true;

                switch (function)
                {
                    case "√":
                        if (input < 0)
                        {
                            HandleError("负数无法开平方根");
                            return;
                        }
                        result = Math.Sqrt(input);
                        break;
                    case "sin":
                        result = Math.Sin(DegreesToRadians(input));
                        break;
                    case "cos":
                        result = Math.Cos(DegreesToRadians(input));
                        break;
                    case "tan":
                        if (Math.Cos(DegreesToRadians(input)) == 0)
                        {
                            HandleError("tan函数在此点未定义");
                            return;
                        }
                        result = Math.Tan(DegreesToRadians(input));
                        break;
                    case "log":
                        if (input <= 0)
                        {
                            HandleError("对数函数的输入必须大于零");
                            return;
                        }
                        result = Math.Log10(input);
                        break;
                    case "ln":
                        if (input <= 0)
                        {
                            HandleError("自然对数函数的输入必须大于零");
                            return;
                        }
                        result = Math.Log(input);
                        break;
                    case "exp":
                        result = Math.Exp(input);
                        break;
                    case "1/x":
                        if (input == 0)
                        {
                            HandleError("不能计算零的倒数");
                            return;
                        }
                        result = 1.0 / input;
                        break;
                    case "%":
                        result = input / 100.0;
                        break;
                    default:
                        validFunction = false;
                        break;
                }

                if (validFunction)
                {
                    if (double.IsInfinity(result))
                    {
                        HandleError("结果溢出");
                        return;
                    }
                    if (double.IsNaN(result))
                    {
                        HandleError("未定义的数学运算");
                        return;
                    }

                    currentValue = result;
                    DisplayValue(result);
                    newNumberFlag = true;
                    operationPending = false;
                    lastOperationWasEquals = false;
                }
            }
            catch (OverflowException)
            {
                HandleError("数值溢出");
            }
            catch (Exception)
            {
                HandleError("函数计算错误");
            }
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        #endregion

        #region 常数处理
        private void btnConstant_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    string constant = btn.Text;
                    ApplyConstant(constant);
                }
            }
            catch (Exception ex)
            {
                HandleError("常数输入错误: " + ex.Message);
            }
        }

        private void ApplyConstant(string constant)
        {
            try
            {
                double value = 0;

                switch (constant)
                {
                    case "π":
                        value = Math.PI;
                        break;
                    case "e":
                        value = Math.E;
                        break;
                }

                currentValue = value;
                DisplayValue(value);
                newNumberFlag = true;
                lastOperationWasEquals = false;
            }
            catch (Exception)
            {
                HandleError("常数输入错误");
            }
        }
        #endregion

        #region 清除和删除操作
        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetCalculator();
        }

        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            try
            {
                txtDisplay.Text = "0";
                currentValue = 0;
                newNumberFlag = true;
            }
            catch (Exception)
            {
                HandleError("清除当前输入错误");
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDisplay.Text.Length > 1)
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
                }
                else
                {
                    txtDisplay.Text = "0";
                    newNumberFlag = true;
                }

                currentValue = ParseDisplayValue();
            }
            catch (Exception)
            {
                HandleError("删除输入错误");
            }
        }
        #endregion

        #region 正负号处理
        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDisplay.Text != "0")
                {
                    if (txtDisplay.Text.StartsWith("-"))
                    {
                        txtDisplay.Text = txtDisplay.Text.Substring(1);
                    }
                    else
                    {
                        txtDisplay.Text = "-" + txtDisplay.Text;
                    }

                    currentValue = ParseDisplayValue();
                }
            }
            catch (Exception)
            {
                HandleError("正负号切换错误");
            }
        }
        #endregion

        #region 内存操作
        private void btnMemory_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    string memoryOperation = btn.Text;
                    PerformMemoryOperation(memoryOperation);
                }
            }
            catch (Exception ex)
            {
                HandleError("内存操作错误: " + ex.Message);
            }
        }

        private void PerformMemoryOperation(string operation)
        {
            try
            {
                switch (operation)
                {
                    case "MC":
                        memoryValue = 0;
                        hasMemoryValue = false;
                        lblMemory.Visible = false;
                        break;
                    case "MR":
                        if (hasMemoryValue)
                        {
                            currentValue = memoryValue;
                            DisplayValue(memoryValue);
                            newNumberFlag = true;
                        }
                        break;
                    case "M+":
                        memoryValue += currentValue;
                        hasMemoryValue = true;
                        lblMemory.Visible = true;
                        break;
                    case "M-":
                        memoryValue -= currentValue;
                        hasMemoryValue = true;
                        lblMemory.Visible = true;
                        break;
                }
            }
            catch (Exception)
            {
                HandleError("内存操作错误");
            }
        }
        #endregion

        #region 键盘支持
        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessKeyInput(e);
        }

        private void txtDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessKeyInput(e);
        }

        private void ProcessKeyInput(KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.NumPad0:
                        InputNumber("0");
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        InputNumber("1");
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        InputNumber("2");
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        InputNumber("3");
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        InputNumber("4");
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        InputNumber("5");
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        InputNumber("6");
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        InputNumber("7");
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        InputNumber("8");
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        InputNumber("9");
                        break;
                    case Keys.OemPeriod:
                    case Keys.Decimal:
                        btnDecimal_Click(btnDecimal, null);
                        break;
                    case Keys.Add:
                        PerformOperation("+");
                        break;
                    case Keys.Subtract:
                        PerformOperation("-");
                        break;
                    case Keys.Multiply:
                        PerformOperation("×");
                        break;
                    case Keys.Divide:
                        PerformOperation("÷");
                        break;
                    case Keys.Enter:
                        btnEquals_Click(btnEquals, null);
                        break;
                    case Keys.Escape:
                        ResetCalculator();
                        break;
                    case Keys.Back:
                        btnBackspace_Click(btnBackspace, null);
                        break;
                    case Keys.Delete:
                        btnClearEntry_Click(btnClearEntry, null);
                        break;
                }

                e.Handled = true;
            }
            catch (Exception)
            {
                HandleError("键盘输入错误");
            }
        }
        #endregion

        #region 辅助方法
        private void ResetCalculator()
        {
            try
            {
                txtDisplay.Text = "0";
                currentValue = 0;
                previousValue = 0;
                currentOperation = "";
                operationPending = false;
                lastOperationWasEquals = false;
                newNumberFlag = true;
            }
            catch (Exception)
            {
                HandleError("重置计算器错误");
            }
        }

        private double ParseDisplayValue()
        {
            try
            {
                return double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void DisplayValue(double value)
        {
            try
            {
                // 处理科学记数法显示
                if (Math.Abs(value) < 1e-10 && value != 0)
                {
                    txtDisplay.Text = value.ToString("E6", CultureInfo.InvariantCulture);
                }
                else if (Math.Abs(value) >= 1e15)
                {
                    txtDisplay.Text = value.ToString("E6", CultureInfo.InvariantCulture);
                }
                else
                {
                    // 正常数字显示，去除不必要的零
                    string result = value.ToString("G15", CultureInfo.InvariantCulture);
                    
                    // 限制显示长度
                    if (result.Length > MAX_DIGITS)
                    {
                        txtDisplay.Text = value.ToString("E6", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        txtDisplay.Text = result;
                    }
                }
            }
            catch (Exception)
            {
                txtDisplay.Text = "错误";
            }
        }

        private void HandleError(string errorMessage)
        {
            try
            {
                txtDisplay.Text = "错误";
                MessageBox.Show(errorMessage, "计算器错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetCalculator();
            }
            catch (Exception)
            {
                // 如果连错误处理都失败，重置到安全状态
                txtDisplay.Text = "0";
                ResetCalculator();
            }
        }
        #endregion
    }
}