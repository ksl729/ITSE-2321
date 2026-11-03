using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Employee_Payroll_Form_KLeach
{
    public class MainForm : Form
    {
        private readonly List<Employee> employees = new();

        // The delimiter is excluded from names and numeric fields use invariant formatting.
        private const char Delimiter = '|';
        private string activePayrollFile = string.Empty;

        private const int LeftCol = 12;
        private const int LabelWidth = 120;
        private const int InputWidth = 140;
        private const int RowHeight = 32;
        private const int ControlHeight = 23;
        private const int RadioSpacing = 110;

        private TextBox txtFirst = null!;
        private TextBox txtLast = null!;
        private RadioButton rbSalaried = null!;
        private RadioButton rbHourly = null!;
        private RadioButton rbCommission = null!;
        private Label lblSalary = null!;
        private TextBox txtSalary = null!;
        private Label lblWage = null!;
        private TextBox txtWage = null!;
        private Label lblHours = null!;
        private TextBox txtHours = null!;
        private Label lblSales = null!;
        private TextBox txtSales = null!;
        private Label lblRate = null!;
        private TextBox txtRate = null!;
        private Button btnAdd = null!;
        private ListBox lstEmployees = null!;

        private Label lblActiveFile = null!;
        private TextBox txtActiveFile = null!;
        private Button btnSelectFile = null!;
        private Button btnLoadFile = null!;
        private Button btnExit = null!;
        private ListBox lstPayrollResults = null!;
        private Label lblTotalPay = null!;

        public MainForm()
        {
            Text = "Employee Payroll System";
            ClientSize = new Size(984, 610);
            MinimumSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            InitializeControls();
        }

        private void InitializeControls()
        {
            var y = 12;

            var lblFirst = MakeLabel("First Name:", y);
            txtFirst = MakeTextBox(y);
            y += RowHeight;

            var lblLast = MakeLabel("Last Name:", y);
            txtLast = MakeTextBox(y);
            y += RowHeight + 6;

            var lblType = MakeLabel("Employee Type:", y);
            rbSalaried = new RadioButton
            {
                Text = "Salaried",
                Left = LeftCol + LabelWidth,
                Top = y,
                Width = RadioSpacing - 10,
                Height = ControlHeight
            };
            rbHourly = new RadioButton
            {
                Text = "Hourly",
                Left = LeftCol + LabelWidth + RadioSpacing,
                Top = y,
                Width = RadioSpacing - 10,
                Height = ControlHeight
            };
            rbCommission = new RadioButton
            {
                Text = "Commission",
                Left = LeftCol + LabelWidth + RadioSpacing * 2,
                Top = y,
                Width = RadioSpacing,
                Height = ControlHeight
            };

            rbSalaried.CheckedChanged += (_, _) => UpdateInputFields();
            rbHourly.CheckedChanged += (_, _) => UpdateInputFields();
            rbCommission.CheckedChanged += (_, _) => UpdateInputFields();
            rbSalaried.Checked = true;
            y += RowHeight + 6;

            lblSalary = MakeLabel("Weekly Salary:", y);
            txtSalary = MakeTextBox(y);
            y += RowHeight;

            lblWage = MakeLabel("Hourly Wage:", y);
            txtWage = MakeTextBox(y);
            y += RowHeight;

            lblHours = MakeLabel("Hours Worked:", y);
            txtHours = MakeTextBox(y);
            y += RowHeight;

            lblSales = MakeLabel("Weekly Sales:", y);
            txtSales = MakeTextBox(y);
            y += RowHeight;

            lblRate = MakeLabel("Commission Rate:", y);
            txtRate = MakeTextBox(y);
            y += RowHeight + 10;

            btnAdd = new Button
            {
                Text = "Add Employee",
                Left = LeftCol + LabelWidth,
                Top = y,
                Width = InputWidth,
                Height = ControlHeight + 5
            };
            btnAdd.Click += BtnAdd_Click;

            // File controls remain in the left column beneath employee entry.
            lblActiveFile = new Label
            {
                Text = "Active payroll file:",
                Left = LeftCol,
                Top = y + 45,
                Width = LabelWidth,
                Height = ControlHeight
            };
            txtActiveFile = new TextBox
            {
                Left = LeftCol + LabelWidth,
                Top = y + 40,
                Width = 330,
                Height = ControlHeight,
                ReadOnly = true
            };
            btnSelectFile = new Button
            {
                Text = "Select File",
                Left = LeftCol + LabelWidth,
                Top = y + 75,
                Width = 100,
                Height = ControlHeight + 5
            };
            btnSelectFile.Click += BtnSelectFile_Click;
            btnLoadFile = new Button
            {
                Text = "Load Payroll",
                Left = LeftCol + LabelWidth + 108,
                Top = y + 75,
                Width = 110,
                Height = ControlHeight + 5
            };
            btnLoadFile.Click += BtnLoadPayroll_Click;
            btnExit = new Button
            {
                Text = "Exit",
                Left = LeftCol + LabelWidth + 226,
                Top = y + 75,
                Width = 90,
                Height = ControlHeight + 5
            };
            btnExit.Click += BtnExit_Click;

            // Both list boxes occupy a dedicated right-hand column, anchored at top-right.
            const int listWidth = 450;
            int listLeft = ClientSize.Width - listWidth - 12;
            lstEmployees = new ListBox
            {
                Left = listLeft,
                Top = 12,
                Width = listWidth,
                Height = 245,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                HorizontalScrollbar = true
            };
            lstPayrollResults = new ListBox
            {
                Left = listLeft,
                Top = 285,
                Width = listWidth,
                Height = 245,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                HorizontalScrollbar = true
            };
            lblTotalPay = new Label
            {
                Text = "Total Pay: $0.00",
                Left = listLeft,
                Top = 540,
                Width = listWidth,
                Height = ControlHeight,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Controls.AddRange(new Control[]
            {
                lblFirst, txtFirst, lblLast, txtLast,
                lblType, rbSalaried, rbHourly, rbCommission,
                lblSalary, txtSalary, lblWage, txtWage,
                lblHours, txtHours, lblSales, txtSales, lblRate, txtRate,
                btnAdd, lblActiveFile, txtActiveFile, btnSelectFile, btnLoadFile, btnExit,
                lstEmployees, lstPayrollResults, lblTotalPay
            });

            UpdateInputFields();
        }

        private Label MakeLabel(string text, int top)
        {
            return new Label
            {
                Text = text,
                Left = LeftCol,
                Top = top + 3,
                Width = LabelWidth,
                Height = ControlHeight
            };
        }

        private TextBox MakeTextBox(int top)
        {
            return new TextBox
            {
                Left = LeftCol + LabelWidth,
                Top = top,
                Width = InputWidth,
                Height = ControlHeight
            };
        }

        private void UpdateInputFields()
        {
            if (lblSalary == null || txtSalary == null || lblWage == null || txtWage == null
                || lblHours == null || txtHours == null || lblSales == null || txtSales == null
                || lblRate == null || txtRate == null)
            {
                return;
            }

            bool salaried = rbSalaried.Checked;
            bool hourly = rbHourly.Checked;
            bool commission = rbCommission.Checked;

            lblSalary.Visible = txtSalary.Visible = salaried;
            lblWage.Visible = txtWage.Visible = hourly;
            lblHours.Visible = txtHours.Visible = hourly;
            lblSales.Visible = txtSales.Visible = commission;
            lblRate.Visible = txtRate.Visible = commission;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            string first = txtFirst.Text.Trim();
            string last = txtLast.Text.Trim();

            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            {
                ShowInputError("Please enter both first and last name.");
                return;
            }

            if (first.Contains(Delimiter) || last.Contains(Delimiter)
                || first.Contains('\r') || first.Contains('\n')
                || last.Contains('\r') || last.Contains('\n'))
            {
                ShowInputError($"Names cannot contain the delimiter '{Delimiter}' or line breaks.");
                return;
            }

            try
            {
                Employee employee;
                if (rbSalaried.Checked)
                {
                    employee = new SalaryEmployee(first, last, ParseDecimal(txtSalary.Text, "weekly salary"));
                }
                else if (rbHourly.Checked)
                {
                    decimal wage = ParseDecimal(txtWage.Text, "hourly wage");
                    decimal hours = ParseDecimal(txtHours.Text, "hours worked");
                    employee = new HourlyEmployee(first, last, wage, hours);
                }
                else if (rbCommission.Checked)
                {
                    decimal sales = ParseDecimal(txtSales.Text, "weekly sales");
                    decimal rate = ParseDecimal(txtRate.Text, "commission rate");
                    employee = new CommissionEmployee(first, last, sales, rate);
                }
                else
                {
                    ShowInputError("Select an employee type.");
                    return;
                }

                // Do not add a record to the in-memory list unless the disk write succeeds.
                if (!WriteEmployeeRecordToFile(employee))
                    return;

                employees.Add(employee);
                RefreshEmployeeList();
                ClearInputs();
            }
            catch (FormatException ex)
            {
                ShowInputError(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ShowInputError(ex.Message);
            }
            catch (OverflowException)
            {
                ShowInputError("The entered values are too large to calculate. Enter smaller values.");
            }
        }

        private decimal ParseDecimal(string input, string fieldName)
        {
            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value))
                throw new FormatException($"Enter a valid number for {fieldName}.");
            return value;
        }

        private void RefreshEmployeeList()
        {
            lstEmployees.Items.Clear();
            for (int i = 0; i < employees.Count; i++)
            {
                Employee employee = employees[i];
                lstEmployees.Items.Add($"{i + 1}. {employee} | Weekly Pay: {employee.Earnings():C}");
            }
        }

        private void ClearInputs()
        {
            txtFirst.Clear();
            txtLast.Clear();
            txtSalary.Clear();
            txtWage.Clear();
            txtHours.Clear();
            txtSales.Clear();
            txtRate.Clear();
        }

        private void BtnSelectFile_Click(object? sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Title = "Select or create the active weekly payroll file",
                Filter = "Payroll files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                AddExtension = true,
                OverwritePrompt = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            activePayrollFile = dialog.FileName;
            txtActiveFile.Text = activePayrollFile;
            MessageBox.Show(this, "New employee records will be appended to:\n" + activePayrollFile,
                "Active Payroll File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool EnsureActiveFileSelected()
        {
            if (!string.IsNullOrWhiteSpace(activePayrollFile))
                return true;

            using var dialog = new SaveFileDialog
            {
                Title = "Choose a file for the weekly payroll",
                Filter = "Payroll files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                AddExtension = true,
                OverwritePrompt = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return false;

            activePayrollFile = dialog.FileName;
            txtActiveFile.Text = activePayrollFile;
            return true;
        }

        private bool WriteEmployeeRecordToFile(Employee employee)
        {
            if (!EnsureActiveFileSelected())
                return false;

            try
            {
                string record;
                if (employee is SalaryEmployee salary)
                {
                    record = string.Join(Delimiter,
                        salary.FirstName, salary.LastName, "Salaried",
                        FormatStoredDecimal(salary.WeeklySalary), FormatStoredDecimal(salary.Earnings()));
                }
                else if (employee is HourlyEmployee hourly)
                {
                    record = string.Join(Delimiter,
                        hourly.FirstName, hourly.LastName, "Hourly",
                        FormatStoredDecimal(hourly.Wage), FormatStoredDecimal(hourly.Hours),
                        FormatStoredDecimal(hourly.Earnings()));
                }
                else if (employee is CommissionEmployee commission)
                {
                    record = string.Join(Delimiter,
                        commission.FirstName, commission.LastName, "Commission",
                        FormatStoredDecimal(commission.GrossSales), FormatStoredDecimal(commission.CommissionRate),
                        FormatStoredDecimal(commission.Earnings()));
                }
                else
                {
                    ShowInputError("This employee type cannot be saved.");
                    return false;
                }

                using (var writer = new StreamWriter(activePayrollFile, append: true))
                    writer.WriteLine(record);

                MessageBox.Show(this, "Employee record was successfully written to the active payroll file.",
                    "Record Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                ShowFileError("Access was denied. Select a different file or check its permissions.");
            }
            catch (IOException ex)
            {
                ShowFileError("The record could not be written. Check that the file is available and that you have permission to write to it.\n\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                ShowFileError("The selected file path is invalid. Select a valid file path.\n\n" + ex.Message);
            }
            catch (NotSupportedException ex)
            {
                ShowFileError("The selected file path is not supported. Choose another location.\n\n" + ex.Message);
            }
            catch (OverflowException)
            {
                ShowInputError("The entered values are too large to calculate. Enter smaller values.");
            }
            return false;
        }

        private static string FormatStoredDecimal(decimal value) => value.ToString(CultureInfo.InvariantCulture);

        private void BtnLoadPayroll_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Load a weekly payroll file",
                Filter = "Payroll files (*.txt)|*.txt|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            LoadPayrollFile(dialog.FileName);
        }

        private void LoadPayrollFile(string fileName)
        {
            lstPayrollResults.Items.Clear();
            lblTotalPay.Text = "Total Pay: $0.00";
            decimal totalPay = 0m;
            int loadedCount = 0;
            int invalidCount = 0;
            string firstProblem = string.Empty;

            try
            {
                using var reader = new StreamReader(fileName);
                string? line;
                int lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        Employee employee = ParseEmployeeRecord(line);
                        decimal pay = employee.Earnings();
                        totalPay = checked(totalPay + pay);
                        string payType = GetPayType(employee);
                        lstPayrollResults.Items.Add($"{employee.FirstName} {employee.LastName} | {payType} | {pay:C}");
                        loadedCount++;
                    }
                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException || ex is OverflowException)
                    {
                        invalidCount++;
                        if (firstProblem.Length == 0)
                            firstProblem = $"Line {lineNumber}: {ex.Message}";
                    }
                }

                lblTotalPay.Text = "Total Pay: " + totalPay.ToString("C", CultureInfo.CurrentCulture);
                string summary = $"Loaded {loadedCount} valid payroll record(s).";
                if (invalidCount > 0)
                    summary += $"\nSkipped {invalidCount} invalid record(s). First issue: {firstProblem}\nCheck the file's delimiter and record values.";

                MessageBox.Show(this, summary, invalidCount == 0 ? "Payroll Loaded" : "Payroll Loaded with Warnings",
                    MessageBoxButtons.OK, invalidCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException)
            {
                ShowFileError("Access was denied while reading this file. Select another file or check its permissions.");
            }
            catch (IOException ex)
            {
                ShowFileError("The payroll file could not be read. Check that it exists and is not in use by another application.\n\n" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                ShowFileError("The selected file path is invalid. Choose a valid payroll file.\n\n" + ex.Message);
            }
            catch (OverflowException)
            {
                ShowFileError("The payroll total is too large to calculate. Check the records in the file.");
            }
        }

        private static Employee ParseEmployeeRecord(string line)
        {
            string[] fields = line.Split(Delimiter);
            if (fields.Length < 3 || string.IsNullOrWhiteSpace(fields[0]) || string.IsNullOrWhiteSpace(fields[1]))
                throw new FormatException("Record is missing a name or employee type.");

            string first = fields[0].Trim();
            string last = fields[1].Trim();
            string type = fields[2].Trim();

            if (type.Equals("Salaried", StringComparison.OrdinalIgnoreCase) && fields.Length == 5)
            {
                var employee = new SalaryEmployee(first, last, ParseStoredDecimal(fields[3]));
                VerifyStoredPay(employee, fields[4]);
                return employee;
            }

            if (type.Equals("Hourly", StringComparison.OrdinalIgnoreCase) && fields.Length == 6)
            {
                var employee = new HourlyEmployee(first, last,
                    ParseStoredDecimal(fields[3]), ParseStoredDecimal(fields[4]));
                VerifyStoredPay(employee, fields[5]);
                return employee;
            }

            if (type.Equals("Commission", StringComparison.OrdinalIgnoreCase) && fields.Length == 6)
            {
                var employee = new CommissionEmployee(first, last,
                    ParseStoredDecimal(fields[3]), ParseStoredDecimal(fields[4]));
                VerifyStoredPay(employee, fields[5]);
                return employee;
            }

            throw new FormatException("Unknown employee type or incorrect number of delimited fields.");
        }

        private static decimal ParseStoredDecimal(string value)
        {
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
                throw new FormatException("A numeric field is invalid.");
            return result;
        }

        private static void VerifyStoredPay(Employee employee, string storedPay)
        {
            decimal pay = ParseStoredDecimal(storedPay);
            if (pay != employee.Earnings())
                throw new FormatException("Stored pay does not match the employee's pay details.");
        }

        private static string GetPayType(Employee employee)
        {
            if (employee is SalaryEmployee) return "Salaried";
            if (employee is HourlyEmployee) return "Hourly";
            if (employee is CommissionEmployee) return "Commission";
            return "Unknown";
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ShowInputError(string message)
        {
            MessageBox.Show(this, message, "Input or Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowFileError(string message)
        {
            MessageBox.Show(this, message, "Payroll File Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}