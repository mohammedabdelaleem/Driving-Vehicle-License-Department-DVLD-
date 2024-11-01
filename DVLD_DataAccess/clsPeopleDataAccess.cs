using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace DataAccess_DVLD
{
    public class clsPeopleDataAccess
    {

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
               DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email,int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"

                 INSERT INTO People (NationalNo,FirstName,SecondName,ThirdName,LastName,
                 DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath)
        
                 VALUES
    	         (@NationalNo,@FirstName,@SecondName,@ThirdName, @LastName,@DateOfBirth,
                 @Gendor,@Address,@Phone,@Email,@NationalityCountryID ,@ImagePath     )
		 
                select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            //  check allow nulls containers
              ThirdName = (ThirdName != string.Empty) ? ThirdName : DBNull.Value.ToString();
              ImagePath = (ImagePath != string.Empty) ? ImagePath : DBNull.Value.ToString();
              Email = (Email != string.Empty) ? Email : DBNull.Value.ToString();


            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    PersonID = insertedID;

            }
            catch (Exception ex)
            {
                // Log Exception
                // we are inside class library
            }
            finally
            {
                // may be the CLR throw exception during initialization
                connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID,string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
             DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int rowsEffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"update People 
                            set NationalNo=@NationalNo ,
                                FirstName=@FirstName,
                                SecondName=@SecondName,
                                ThirdName=@ThirdName,
                                LastName=@LastName,
                                DateOfBirth=@DateOfBirth,
                                Gendor=@Gendor,
                                Address=@Address,
                                Phone=@Phone,
                                Email=@Email,
                                NationalityCountryID=@NationalityCountryID,
                                ImagePath=@ImagePath

                            where PersonID=@PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            //  check allow nulls containers
            ThirdName = (ThirdName != string.Empty) ? ThirdName : DBNull.Value.ToString();
            ImagePath = (ImagePath != string.Empty) ? ImagePath : DBNull.Value.ToString();
            Email = (Email != string.Empty) ? Email : DBNull.Value.ToString();


            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                rowsEffected = command.ExecuteNonQuery();

             

            }
            catch (Exception ex)
            {
                // Log Exception
                // we are inside class library
            }
            finally
            {
                // may be the CLR throw exception during initialization
                connection.Close();
            }

           return rowsEffected > 0; ;
        }


        public static bool DeletePerson(int PersonID)
        {
            int RowsEffected = 0;
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"
                             delete People
                             where PersonID = @PersonID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();
                    RowsEffected = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // Log Ex.
                }
                finally
                {
                    connection.Close();
                }

                return RowsEffected > 0;
            }

        public static bool GetPersonInfoByID(int PersonID,ref string NationalNo, ref string FirstName, ref string SecondName,
            ref string ThirdName, ref string LastName,ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
            ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath) { 
        
            bool isFound= false ;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"
                             select * from People
                             where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;
                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = reader["ThirdName"] !=DBNull.Value ? (string)reader["ThirdName"] : string.Empty;
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : string.Empty;

                }

                reader.Close();
            }
            catch (Exception e)
            {
                // Log Ex.
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
       ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
       ref short Gendor, ref string Address, ref string Phone, ref string Email,
       ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                    // The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : string.Empty;
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : string.Empty;
                   
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static DataTable ListAllPeople()
        {
            DataTable dtpeople = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query =
                @"
        select People.PersonID , People.NationalNo, People.FirstName,People.SecondName,
		People.ThirdName, People.LastName , People.DateOfBirth , 
		case 
			when People.Gendor=0 then 'Male'
			else 'Female'
		end as GendorCaption ,
		People.Address, People.Phone , People.Email , People.NationalityCountryID , 
		Countries.CountryName, People.ImagePath 

		from People inner join Countries on People.NationalityCountryID = Countries.CountryID

		order by People.NationalNo
";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dtpeople.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //log Exception
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dtpeople;
        }


    }
}
