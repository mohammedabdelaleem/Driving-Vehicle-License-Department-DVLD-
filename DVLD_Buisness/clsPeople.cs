using DataAccess_DVLD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness_DVLD
{
    public class clsPeople
    {

        public static DataTable ListAllPeople()
        {
            return clsPeopleDataAccess.ListAllPeople();
        }

    }
}
