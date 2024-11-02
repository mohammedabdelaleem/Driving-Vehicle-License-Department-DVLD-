using DataAccess_DVLD;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness_DVLD
{
    public class clsPerson
    {

        // 1-Mode 
        enum enMode { AddNew=0 , Update=1 }
        enMode Mode = enMode.AddNew;


        // 2 - Properties 
      
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName { get {  return FirstName + " " + SecondName + ThirdName!=string.Empty ? " "+ThirdName:" " + LastName; } }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        clsCountry countryInfo;


        // 3 - public default ctor 
        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = string.Empty;
            this.FirstName = string.Empty;
            this.SecondName = string.Empty;
            this.ThirdName = string.Empty;
            this.LastName = string.Empty;
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Address = string.Empty;
            this.Phone = string.Empty;
            this.Email = string.Empty;
            this.NationalityCountryID = -1;
            this.ImagePath = string.Empty;

            Mode = enMode.AddNew;
        }
      
        // 4 - private prameterizd ctor
        private clsPerson(int PersonID,  string NationalNo,  string FirstName,  string SecondName,
             string ThirdName,  string LastName,  DateTime DateOfBirth,  byte Gendor,  string Address,
             string Phone,  string Email,  int NationalityCountryID,  string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            this.countryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }

        // 6 - AddNewPerson
        private bool _AddNewPerson()
        {

            this.PersonID = clsPersonDataAccess.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName,
             this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address,
             this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

            return this.PersonID != -1;
        }

        
        // 7 - _UpdatePerson
        private bool _UpdatePerson()
        {
            return clsPersonDataAccess.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
              this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address,
              this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }

        // 5 - save()
        public  bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                      Mode =  enMode.Update;
                        return true;
                    }
                    else { return false; }

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        // 8 - Find 

        public static clsPerson Find(int personID)
        {
            string NationalNo = "";  string FirstName = "";  string SecondName = "";
             string ThirdName = "";  string LastName = ""; DateTime DateOfBirth=DateTime.Now;  byte Gendor=0;  string Address = "";
             string Phone = "";  string Email = "";  int NationalityCountryID=-1;  string ImagePath = "";
       
        if (clsPersonDataAccess.GetPersonInfoByID(personID, ref  NationalNo, ref  FirstName, ref  SecondName,
            ref  ThirdName, ref  LastName, ref  DateOfBirth, ref  Gendor, ref  Address,
            ref  Phone, ref  Email, ref  NationalityCountryID, ref  ImagePath))
            {
                return new clsPerson(personID, NationalNo,  FirstName,  SecondName,
              ThirdName,  LastName,  DateOfBirth,  Gendor,  Address,
              Phone,  Email,  NationalityCountryID,  ImagePath);
            }
        else 
               { return null; }
        
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            byte Gendor = 0;

            bool IsFound = clsPersonDataAccess.GetPersonInfoByNationalNo
                                (
                                    NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gendor, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }


        // 9- delete
        public static bool DeletePerson(int PersonID)
        {
          return  clsPersonDataAccess.DeletePerson(PersonID);
        }

        // 10- is Person exist 
        public static bool IsPersonExists(int PersonID)
        {
            return clsPersonDataAccess.IsPersonExist(PersonID);
        }
        public static bool IsPersonExists(string NationalNo)
        {
            return clsPersonDataAccess.IsPersonExist(NationalNo);
        }
        public static DataTable ListAllPeople()
        {
            return clsPersonDataAccess.ListAllPeople();
        }

    }
}
