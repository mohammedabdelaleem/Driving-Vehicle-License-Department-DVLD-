using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsCountry
    {
        public int ID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            ID = 0;
            CountryName = "";
        }

        private clsCountry(int ID, string CountryName)
        {
            this.ID = ID;
            this.CountryName = CountryName;
        }

        public static clsCountry Find(int countryID)
        {
            string countryName = "";

            if (clsCountryDataAccess.GetCountryInfoByID(countryID, ref countryName))
            {
                return new clsCountry(countryID, countryName);
            }
            else { return null; }

        }

        public static clsCountry Find(string countryName)
        {
            int countryID = 0;

            if (clsCountryDataAccess.GetCountryInfoByName(countryName, ref countryID)) 
            { 
                return new clsCountry(countryID, countryName); 
            } 
            
            else { return null; }
        }


        public static DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }




    }
}
