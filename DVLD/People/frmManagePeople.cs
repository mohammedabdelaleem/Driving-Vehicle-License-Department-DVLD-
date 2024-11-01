using System;
using System.Data;
using System.Windows.Forms;
using Buisness_DVLD;


namespace DVLD
{
    public partial class frmManagePeople : Form
    {

        private static DataTable _dtAllPeople = clsPeople.ListAllPeople();

        //only select the columns that you want to show in the grid
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                         "FirstName", "SecondName", "ThirdName", "LastName",
                                                         "GendorCaption", "DateOfBirth", "CountryName",
                                                         "Phone", "Email");

        public void RefreshPeopleList()
        {
            _dtAllPeople = clsPeople.ListAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                         "FirstName", "SecondName", "ThirdName", "LastName",
                                                         "GendorCaption", "DateOfBirth", "CountryName",
                                                         "Phone", "Email");

            dgvListPeople.DataSource = _dtPeople;
            lblPeopleRecords.Text = dgvListPeople.Rows.Count.ToString();
        }
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {

            dgvListPeople.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblPeopleRecords.Text = dgvListPeople.Rows.Count.ToString();

            // i need to handle each dgv column header text and its width
            if (dgvListPeople.Rows.Count > 0)
            {
                dgvListPeople.Columns[0].HeaderText = "Person ID";
                dgvListPeople.Columns[0].Width = 100;

                dgvListPeople.Columns[1].HeaderText = "National No";
                dgvListPeople.Columns[1].Width = 100; 
                

                dgvListPeople.Columns[2].HeaderText = "First Name";
                dgvListPeople.Columns[2].Width = 100;

                dgvListPeople.Columns[3].HeaderText = "Second Name";
                dgvListPeople.Columns[3].Width = 100;

                dgvListPeople.Columns[4].HeaderText = "Third Name";
                dgvListPeople.Columns[4].Width = 100;

                dgvListPeople.Columns[5].HeaderText = "Last Name";
                dgvListPeople.Columns[5].Width = 100;

                dgvListPeople.Columns[6].HeaderText = "Gender";
                dgvListPeople.Columns[6].Width = 100;

                dgvListPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvListPeople.Columns[7].Width = 130;

                dgvListPeople.Columns[8].HeaderText = "Nationality";
                dgvListPeople.Columns[8].Width = 100;

                dgvListPeople.Columns[9].HeaderText = "Phone";
                dgvListPeople.Columns[9].Width = 100;

                dgvListPeople.Columns[10].HeaderText = "Email";
                dgvListPeople.Columns[10].Width = 140;

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // txtFilterValue status
        private void cbFilterValue_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Focus();
            }
    
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            // 1- Extract filter column  
            // 2- Don't forget mapping
            // 


            string FilterColumn = "";

            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gender":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }


            if(FilterColumn==string.Empty || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = string.Empty;  // no string condition filter -> show all results
                lblPeopleRecords.Text = dgvListPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")  // Dealing With Numbers
            {
                // check for prevent the exception
                if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
                {
                    _dtPeople.DefaultView.RowFilter = ""; // clear condition
                }
                else
                {
                    _dtPeople.DefaultView.RowFilter = $"[{FilterColumn}] = {txtFilterValue.Text.Trim()}";
                }
            }
            else
                _dtPeople.DefaultView.RowFilter = $"[{FilterColumn}] like '{txtFilterValue.Text.Trim()}%'";


            lblPeopleRecords.Text = dgvListPeople.Rows.Count.ToString();

 

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
         frmAddEditPerson frm = new frmAddEditPerson();
           frm.ShowDialog();
        }
    }
}
