using Buisness_DVLD;
using DVLD.People;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{

    public partial class frmAddEditPerson : Form
    {
        enum enMode { AddNew = -1, Update = 0 }
        enum enGendor { Male =  0, Femal = 1 }

        private int _personID = -1;
        enMode Mode = enMode.AddNew;
        clsPerson _Person;

       
        public event EventHandler<DataBackEventArgs> DataBack;

        public virtual void OnDataBack(DataBackEventArgs e)
        {
            DataBack?.Invoke(this, e);
        }

        public frmAddEditPerson(int personID)
        {
            InitializeComponent();
            _personID = personID;

            if (_personID == -1)
            {
                Mode = enMode.AddNew;
            }
            else
            {
                Mode = enMode.Update;
            }


            // Enforce Value For not null components
            txtFirstName.Validating += ValidateEmptyTextBox;
            txtSecondName.Validating += ValidateEmptyTextBox;
            txtLastName.Validating += ValidateEmptyTextBox;
            txtPhone.Validating += ValidateEmptyTextBox;
            txtAddress.Validating += ValidateEmptyTextBox;

            //// 
            //btnColose.Click += frmAddEditPerson_FormClosing;
        }



        #region  Methods
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            if (sender is TextBox txtbox)
            {
                if (string.IsNullOrEmpty(txtbox.Text.Trim()))
                {
                      e.Cancel = true;
                    //  txtbox.Focus();
                    errorProvider1.SetError(txtbox, "Please Enter A Value");
                }
                else
                {
                    //e.Cancel = false;
                    errorProvider1.SetError(txtbox, null);
                }
            }
        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefultValues()
        {

            // handle title depends on the mode 
            if (Mode == enMode.AddNew)
            {
                // change title
                lblMainTitle.Text = "Add New Person";

                // make person object with add new mode 
                _Person = new clsPerson();
            }else if (Mode == enMode.Update)
            {
                lblMainTitle.Text = "Update Person";
            }


            // Default values for your container 
            lblPersonID.Text = "N/A";
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            rbFemale.Checked = false;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";

            // age [18 - 100]
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            _FillCountriesInComoboBox();
            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");

            // lblRemove depends on ...
            llRemove.Visible = (pbPersonImage.ImageLocation != null);

            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }


        }
        private void _LoadData()
        {
            // fill person with person info
            _Person = clsPerson.Find(_personID);

            // check if the person is deleted from other client
            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _personID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close(); // don't forget
                return;
            }

            //the following code will not be executed if the person was found
            lblPersonID.Text = _personID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;

            // cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.countryInfo.CountryName);

            // load person image if found
            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                // pbPersonImage.Load(_Person.ImagePath); // if this ok?
            }

            // lblRemove depends on ...
            llRemove.Visible = (_Person.ImagePath != "");

        }

        private bool _HandlePersonImage()
        {

            //              ** this procedure will handle the person image **

            //1 - it will take care of deleting the old image from the folder in case the image changed
            //2 - and it will rename the new image with GUID and 
            //3 - place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                // 1st delete the old image if found
                if(_Person.ImagePath != "")
                {
                    //first we delete the old image from the folder in case there is any.
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }


                }

                // 2nd copy new image "which inside the pbPersonImage.ImageLocation" into the ProjectFolderImages
                if(pbPersonImage.ImageLocation != null)
                {
                    string srcImgFile = pbPersonImage.ImageLocation.ToString();

                    if(clsUtil.CopyImageToProjectImagesFolder(ref srcImgFile)) {
                        pbPersonImage.ImageLocation= srcImgFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }
                }


            


            return true;
             }

     



        #endregion

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetDefultValues();

            if (Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.ico;*.svg;*.webp";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemove.Visible = true;
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if(rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }

            llRemove.Visible = false;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            // if there is no image at pbPerson Image 
            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            // if there is no image at pbPerson Image 
            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            // email : if there is no email , don't validate 
            if(txtEmail.Text.Trim().Length == 0) { 
                return; 
            }

            else
            {
                if (!clsValidation.ValidateEmail(txtEmail.Text))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
                }else
                {
                    errorProvider1.SetError(txtEmail, null);

                }
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            //required field
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            // unique field
            //if (clsPerson.IsPersonExists(txtNationalNo.Text.Trim()))  // this is ok when you add new person , but if you update

            if(txtNationalNo.Text.Trim() != _Person.NationalNo  && clsPerson.IsPersonExists(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

        }

        private void frmAddEditPerson_FormClosing(object sender, FormClosingEventArgs e)
        {

           DialogResult result =  MessageBox.Show("Close The Form ?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(DialogResult.No == result)
            {
                e.Cancel = true;
            }
        }

        private void btnColose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage()) return;


            int NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            _Person.NationalityCountryID = NationalityCountryID;


            if(rbMale.Checked)
                _Person.Gendor = (byte) enGendor.Male;
            else
                _Person.Gendor = (byte) enGendor.Femal;


            if(pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";


            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();

                /////////// change the mode 
                Mode = enMode.Update;

                lblMainTitle.Text = "Update Person";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // invoke the event💡
                OnDataBack(new DataBackEventArgs() { PersonID = _Person.PersonID});
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
    }
}
