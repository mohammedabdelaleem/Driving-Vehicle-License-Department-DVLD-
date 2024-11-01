using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddEditPerson : Form
    {
        public frmAddEditPerson()
        {
            InitializeComponent();

            // Enforce Value For not null components
            txtFirst.Validating += ForceTextboxValue;
            txtSecond.Validating += ForceTextboxValue;
            txtLast.Validating += ForceTextboxValue;
            txtNationalNo.Validating += ForceTextboxValue;
            txtPhone.Validating += ForceTextboxValue;
            txtAddress.Validating += ForceTextboxValue;
        }



        #region  Methods
        private void ForceTextboxValue(object sender, CancelEventArgs e)
        {
            if(sender is TextBox txtbox)
            {
                if (string.IsNullOrWhiteSpace(txtbox.Text))
                {
                    e.Cancel = true;
                    txtbox.Focus();
                    errorProvider1.SetError(txtbox, "Please Enter A Value");
                }
                else
                {
                    e.Cancel = false;
                    //  errorProvider1.SetError(txtbox, "Please Enter A Value");
                }
            }
        }

        private void _FillCountriesInCombobox()
        {
            DataTable countries = clsCountry.GetAllCountries();

            foreach (DataRow row in countries.Rows)
            {
                cbCountry.Items.Add(row.ToString());
            }
        }


        private void _ResetDefultValues()
        {
            _FillCountriesInCombobox();

            // age [18 - 100]
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);







        }



        #endregion

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetDefultValues();

        }
    }
}
