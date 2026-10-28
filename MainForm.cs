using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab6_Payroll_KLeach
{
    public class MainForm : Form
    {
        private readonly List<Employee> employees = new();

        // Layout constants
        private const int LeftCol = 12;
        private const int LabelWidth = 120;
        private const int InputWidth = 140;
        private const int RowHeight = 32;
        private const int ControlHeight = 23;
        private const int RadioSpacing = 110;

        // Controls
        private TextBox txtFirst;
        private TextBox txtLast;

        private RadioButton rbSalaried;
        private RadioButton rbHourly;
        private RadioButton rbCommission;

        private Label lblSalary;
        private TextBox txtSalary;

        private Label lblWage;
        private TextBox txtWage;
        private Label lblHours;
        private TextBox txtHours;

        private Label lblSales;
        private TextBox txtSales;
        private Label lblRate;
        private TextBox txtRate;

        private Button btnAdd;
        private ListBox lstEmployees;

        public MainForm()
        {
            Text = "Employee Payroll System";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var y = 12;

            // First name
            var lblFirst = MakeLabel("First Name:", y);
            txtFirst = MakeTextBox(y);
            y += RowHeight;

            // Last name
            var lblLast = MakeLabel("Last Name:", y);
            txtLast = MakeTextBox(y);
            y += RowHeight + 6;

            // Employee type (radio buttons)
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

            rbSalaried.CheckedChanged += (s, e) => UpdateInputFields();
            rbHourly.CheckedChanged += (s, e) => UpdateInputFields();
            rbCommission.CheckedChanged += (s, e) => UpdateInputFields();
            rbSalaried.Checked = true;
            y += RowHeight + 6;

            // Salaried inputs
            lblSalary = MakeLabel("Weekly Salary:", y);
            txtSalary = MakeTextBox(y);
            y += RowHeight;

            // Hourly inputs
            lblWage = MakeLabel("Hourly Wage:", y);
            txtWage = MakeTextBox(y);
            y += RowHeight;

            lblHours = MakeLabel("Hours Worked:", y);
            txtHours = MakeTextBox(y);
            y += RowHeight;

            // Commission inputs
            lblSales = MakeLabel("Weekly Sales:", y);
            txtSales = MakeTextBox(y);
            y += RowHeight;

            lblRate = MakeLabel("Commission Rate:", y);
            txtRate = MakeTextBox(y);
            y += RowHeight + 10;

            // Add button
            btnAdd = new Button
            {
                Text = "Add Employee",
                Left = LeftCol + LabelWidth,
                Top = y,
                Width = InputWidth,
                Height = ControlHeight + 5
            };
            btnAdd.Click += BtnAdd_Click;

            // Employee list (aligned to top of form, right column)
            lstEmployees = new ListBox
            {
                Left = LeftCol + LabelWidth + InputWidth + 40,
                Top = 12,
                Width = 380,
                Height = 480
            };

            Controls.AddRange(new Control[]
            {
                lblFirst, txtFirst,
                lblLast, txtLast,
                lblType, rbSalaried, rbHourly, rbCommission,
                lblSalary, txtSalary,
                lblWage, txtWage,
                lblHours, txtHours,
                lblSales, txtSales,
                lblRate, txtRate,
                btnAdd, lstEmployees
            });

            UpdateInputFields();
        }

        // Helper: creates a label aligned consistently at the given row
        private Label MakeLabel(string text, int top)
        {
            return new Label
            {
                Text = text,
                Left = LeftCol,
                Top = top + 3, // nudges label text to vertically center with textbox
                Width = LabelWidth,
                Height = ControlHeight
            };
        }

        // Helper: creates a textbox aligned consistently at the given row
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
            // Controls may not all be created yet when this fires
            // (e.g. during initial RadioButton.Checked = true assignment)
            if (lblSalary == null || txtSalary == null || lblWage == null || txtWage == null
                || lblHours == null || txtHours == null || lblSales == null || txtSales == null
                || lblRate == null || txtRate == null)
            {
                return;
            }

            var salaried = rbSalaried.Checked;
            var hourly = rbHourly.Checked;
            var commission = rbCommission.Checked;

            lblSalary.Visible = txtSalary.Visible = salaried;

            lblWage.Visible = txtWage.Visible = hourly;
            lblHours.Visible = txtHours.Visible = hourly;

            lblSales.Visible = txtSales.Visible = commission;
            lblRate.Visible = txtRate.Visible = commission;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var first = txtFirst.Text.Trim();
            var last = txtLast.Text.Trim();

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("Please enter both first and last name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (rbSalaried.Checked)
                {
                    var salary = ParseDecimal(txtSalary.Text, "weekly salary");
                    var emp = new SalaryEmployee(first, last, salary);
                    employees.Add(emp);
                }
                else if (rbHourly.Checked)
                {
                    var wage = ParseDecimal(txtWage.Text, "hourly wage");
                    var hours = ParseDecimal(txtHours.Text, "hours worked");
                    var emp = new HourlyEmployee(first, last, wage, hours);
                    employees.Add(emp);
                }
                else if (rbCommission.Checked)
                {
                    var sales = ParseDecimal(txtSales.Text, "weekly sales");
                    var rate = ParseDecimal(txtRate.Text, "commission rate");
                    var emp = new CommissionEmployee(first, last, sales, rate);
                    employees.Add(emp);
                }

                RefreshEmployeeList();
                ClearInputs();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private decimal ParseDecimal(string input, string fieldName)
        {
            if (decimal.TryParse(input, out var v))
                return v;

            throw new FormatException($"Invalid number for {fieldName}.");
        }

        private void RefreshEmployeeList()
        {
            lstEmployees.Items.Clear();
            for (int i = 0; i < employees.Count; i++)
            {
                var e = employees[i];
                lstEmployees.Items.Add($"{i + 1}. {e} | Weekly Pay: {e.Earnings():C}");
            }
        }

        private void InitializeComponent()
        {

        }

        private void ClearInputs()
        {
            txtFirst.Text = string.Empty;
            txtLast.Text = string.Empty;
            txtSalary.Text = string.Empty;
            txtWage.Text = string.Empty;
            txtHours.Text = string.Empty;
            txtSales.Text = string.Empty;
            txtRate.Text = string.Empty;
        }
    }
}