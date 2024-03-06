using Microsoft.Data.SqlClient;
using ScopeIndia.Models;


namespace ScopeIndia.Data
{
    public class SqlRegistration:IRegistration
    {
        private readonly string? istring;

        public SqlRegistration(IConfiguration configuration)
        {
            istring = configuration.GetConnectionString("istring");
        }
        public void Insert(Registration registration)
        {
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string InsertQuery = "INSERT INTO RegistrationDatabase(FirstName,LastName,Gender,DateOfBirth,Email,MobileNumber,Country,State,City,Hobbies,Courses,ImagePath) VALUES(@FirstName,@LastName,@Gender,@DateOfBirth,@Email,@MobileNumber,@Country,@State,@City,@Hobbies,@Courses,@ImagePath)";
                using (SqlCommand cmd = new SqlCommand(InsertQuery, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@FirstName", registration.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", registration.LastName);
                    cmd.Parameters.AddWithValue("@Gender", registration.Gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", registration.Email);
                    cmd.Parameters.AddWithValue("@MobileNumber", registration.MobileNumber);
                    cmd.Parameters.AddWithValue("@Country", registration.Country);
                    cmd.Parameters.AddWithValue("@State", registration.State);
                    cmd.Parameters.AddWithValue("@City", registration.City);
                    cmd.Parameters.AddWithValue("@Hobbies", registration.Hobbies);
                    cmd.Parameters.AddWithValue("@Courses", registration.Courses);
                    cmd.Parameters.AddWithValue("@ImagePath", registration.ImagePath);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        public void Update(Registration registration)
        {
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string UpdateQuery = "UPDATE RegistrationDatabase SET FirstName=@FirstName,LastName=@LastName,Gender=@Gender,DateOfBirth=@DateOfBirth,Email=@Email,MobileNumber=@MobileNumber,Country=@Country,State=@State,City=@City,Hobbies=@Hobbies,Courses=@Courses WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(UpdateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", registration.Id);
                    cmd.Parameters.AddWithValue("@FirstName",/*((object)*/registration.FirstName)/* ?? DBNull.Value)*/;
                    cmd.Parameters.AddWithValue("@LastName", registration.LastName);
                    cmd.Parameters.AddWithValue("@Gender", registration.Gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", registration.Email);
                    cmd.Parameters.AddWithValue("@MobileNumber", registration.MobileNumber);
                    cmd.Parameters.AddWithValue("@Country", registration.Country);
                    cmd.Parameters.AddWithValue("@State", registration.State);
                    cmd.Parameters.AddWithValue("@City", registration.City);
                    cmd.Parameters.AddWithValue("@Hobbies", registration.Hobbies);
                    cmd.Parameters.AddWithValue("@Courses", registration.Courses);
                    cmd.Parameters.AddWithValue("@ImagePath", registration.ImagePath);
                    
                    cmd.ExecuteNonQuery();

                }
                conn.Close();
            }
        }

        public void Delete(string Email)
        {
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string DeleteQuery = "DELETE FROM RegistrationDatabase WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(DeleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public Registration GetByEmail(string Email)
        {
            Registration registration = new Registration();
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string SelectQuery = "SELECT * FROM RegistrationDatabase WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(SelectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            registration.Id = reader.GetInt32(0);
                            registration.FirstName = reader.GetString(1);
                            registration.LastName = reader.GetString(2);
                            registration.Gender = reader.GetString(3);
                            DateTime dateOfBirth = reader.GetDateTime(4);
                            registration.DateOfBirth = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
                            registration.Email = reader.GetString(5);
                            registration.MobileNumber = reader.GetString(6);
                            registration.Country = reader.GetString(7);
                            registration.State = reader.GetString(8);
                            registration.City = reader.GetString(9);
                            registration.Hobbies = reader.GetString(10);
                            registration.Courses = reader.GetString(11);
                            registration.ImagePath = reader.GetString(12);
                            
                            

                        }
                    }
                    conn.Close();
                }
                return registration;
            }
        }
        
        public Registration GetByName(string FirstName)
        {
            Registration registration = new Registration();
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string SelectQuery = "SELECT * FROM RegistrationDatabase WHERE FirstName=@FirstName";
                using (SqlCommand cmd = new SqlCommand(SelectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            registration.Id = reader.GetInt32(0);
                            registration.FirstName = reader.GetString(1);
                            registration.LastName = reader.GetString(2);
                            registration.Gender = reader.GetString(3);
                            DateTime dateOfBirth = reader.GetDateTime(4);
                            registration.DateOfBirth = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
                            registration.Email = reader.GetString(5);
                            registration.MobileNumber = reader.GetString(6);
                            registration.Country = reader.GetString(7);
                            registration.State = reader.GetString(8);
                            registration.City = reader.GetString(9);
                            registration.Hobbies = reader.GetString(10);
                            registration.Courses = reader.GetString(11);
                            registration.ImagePath = reader.GetString(12);
                            
                         


                        }
                    }
                    conn.Close();
                }
                return registration;
            }
        }
        public List<Registration> GetAll()
        {
            List<Registration> list = new List<Registration>();
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string SelectQuery = "SELECT * FROM RegistrationDatabase";
                using (SqlCommand cmd = new SqlCommand(SelectQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Registration
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Gender = reader.GetString(3),
                                DateOfBirth = new DateOnly(reader.GetDateTime(4).Year, reader.GetDateTime(4).Month, reader.GetDateTime(4).Day),
                                Email = reader.GetString(5),
                                MobileNumber= reader.GetString(6),
                                Country = reader.GetString(7),
                                State = reader.GetString(8),
                                City= reader.GetString(9),
                                Hobbies = reader.GetString(10),
                                Courses = reader.GetString(11),
                                ImagePath= reader.GetString(12)



                            });
                          
                        }
                    }
                }
                conn.Close();
            }
            return list;
        }


    }


    
}
