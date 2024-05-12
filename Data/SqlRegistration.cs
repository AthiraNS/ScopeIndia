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
                string InsertQuery = "INSERT INTO RegistrationDatabase(FirstName,LastName,Gender,DateOfBirth,Email,MobileNumber,Country,State,City,Hobbies,Courses,ImagePath,Otp) VALUES(@FirstName,@LastName,@Gender,@DateOfBirth,@Email,@MobileNumber,@Country,@State,@City,@Hobbies,@Courses,@ImagePath,@Otp)";
                using (SqlCommand cmd = new SqlCommand(InsertQuery, conn))
                {
                   
                    cmd.Parameters.AddWithValue("@FirstName", registration.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", registration.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", registration.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", registration.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobileNumber", registration.MobileNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Country", registration.Country ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@State", registration.State ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", registration.City ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Hobbies", registration.Hobbies ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Courses", registration.Courses ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImagePath", registration.ImagePath ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Otp", registration.Otp ?? (object)DBNull.Value);
                    //cmd.Parameters.AddWithValue("@Password", registration.Password);
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
                string UpdateQuery = "UPDATE RegistrationDatabase SET FirstName=@FirstName,LastName=@LastName,Gender=@Gender,DateOfBirth=@DateOfBirth,Email=@Email,MobileNumber=@MobileNumber,Country=@Country,State=@State,City=@City,Hobbies=@Hobbies,Courses=@Courses, Password=@Password WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(UpdateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id",registration.Id);
                    cmd.Parameters.AddWithValue("@FirstName", ((object)registration.FirstName) ??DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", ((object)registration.LastName) ??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", ((object)registration.Gender) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", ((object)registration.DateOfBirth) ??DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", ((object)registration.Email) ??DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobileNumber", ((object)registration.MobileNumber) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Country", ((object)registration.Country) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@State", ((object)registration.State) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", ((object)registration.City) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Hobbies", ((object)registration.Hobbies) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Courses", ((object)registration.Courses) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImagePath", ((object)registration.ImagePath) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", ((object)registration.Password) ?? DBNull.Value);
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
                            registration.FirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            registration.LastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                            registration.Gender = reader.IsDBNull(3) ? null : reader.GetString(3);
                            DateTime dateOfBirth = reader.GetDateTime(4);
                            registration.DateOfBirth = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
                            registration.Email = reader.IsDBNull(5) ? null : reader.GetString(5);
                            registration.MobileNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                            registration.Country = reader.IsDBNull(7) ? null : reader.GetString(7);
                            registration.State = reader.IsDBNull(8) ? null : reader.GetString(8);
                            registration.City = reader.IsDBNull(9) ? null : reader.GetString(9);
                            registration.Hobbies = reader.IsDBNull(10) ? null : reader.GetString(10);
                            registration.Courses = reader.IsDBNull(11) ? null : reader.GetString(11);
                            registration.ImagePath = reader.IsDBNull(12) ? null : reader.GetString(12);
                            registration.Password = reader.IsDBNull(14) ? null : reader.GetString(14);
                        }
                    }
                    conn.Close();
                }
                return registration;
            }
        }

        public Registration GetById(int Id)
        {
            Registration registration = new Registration();
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string SelectQuery = "SELECT * FROM RegistrationDatabase WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(SelectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            registration.Id = reader.GetInt32(0);
                            registration.FirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            registration.LastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                            registration.Gender = reader.IsDBNull(3) ? null : reader.GetString(3);
                            DateTime dateOfBirth = reader.GetDateTime(4);
                            registration.DateOfBirth = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
                            registration.Email = reader.IsDBNull(5) ? null : reader.GetString(5);
                            registration.MobileNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                            registration.Country = reader.IsDBNull(7) ? null : reader.GetString(7);
                            registration.State = reader.IsDBNull(8) ? null : reader.GetString(8);
                            registration.City = reader.IsDBNull(9) ? null : reader.GetString(9);
                            registration.Hobbies = reader.IsDBNull(10) ? null : reader.GetString(10);
                            registration.Courses = reader.IsDBNull(11) ? null : reader.GetString(11);
                            registration.ImagePath = reader.IsDBNull(12) ? null : reader.GetString(12);
                            registration.Password = reader.IsDBNull(13) ? null : reader.GetString(13);
                        }
                    }
                    conn.Close();
                }
                return registration;
            }
        }

        public void UpdateCourseId(int Id,int CourseId)
        {
            
            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string UpdateSelectQuery = "UPDATE RegistrationDatabase SET Courses=@Courses WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(UpdateSelectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Courses", CourseId);
                    cmd.ExecuteNonQuery();

                }
                conn.Close();
            }
        }


        //public Registration GetByName(string FirstName)
        //{
        //    Registration registration = new Registration();
        //    using (SqlConnection conn = new SqlConnection(istring))
        //    {
        //        conn.Open();
        //        string SelectQuery = "SELECT * FROM RegistrationDatabase WHERE FirstName=@FirstName";
        //        using (SqlCommand cmd = new SqlCommand(SelectQuery, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@FirstName", FirstName);
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    registration.Id = reader.GetInt32(0);
        //                    registration.FirstName = reader.GetString(1);
        //                    registration.LastName = reader.GetString(2);
        //                    registration.Gender = reader.GetString(3);
        //                    DateTime dateOfBirth = reader.GetDateTime(4);
        //                    registration.DateOfBirth = new DateOnly(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
        //                    registration.Email = reader.GetString(5);
        //                    registration.MobileNumber = reader.GetString(6);
        //                    registration.Country = reader.GetString(7);
        //                    registration.State = reader.GetString(8);
        //                    registration.City = reader.GetString(9);
        //                    registration.Hobbies = reader.GetString(10);
        //                    registration.Courses = reader.GetString(11);
        //                    registration.ImagePath = reader.GetString(12);
        //                }
        //            }
        //            conn.Close();
        //        }
        //        return registration;
        //    }
        //}
        public List<Registration> GetAll()
        {
            List<Registration> registrations = new List<Registration>();
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
                            registrations.Add(new Registration
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Gender = reader.IsDBNull(3) ? null : reader.GetString(3),
                                DateOfBirth = new DateOnly(reader.GetDateTime(4).Year, reader.GetDateTime(4).Month, reader.GetDateTime(4).Day),
                                Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                                MobileNumber= reader.IsDBNull(6) ? null : reader.GetString(6),
                                Country = reader.IsDBNull(7) ? null : reader.GetString(7),
                                State = reader.IsDBNull(8) ? null : reader.GetString(8),
                                City= reader.IsDBNull(9) ? null : reader.GetString(9),
                                Hobbies = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Courses = reader.IsDBNull(11) ? null : reader.GetString(11),
                                ImagePath= reader.IsDBNull(12) ? null : reader.GetString(12),
                                Password= reader.IsDBNull(13) ? null : reader.GetString(13) 
                            });
                          
                        }
                    }
                }
                conn.Close();
            }
            return registrations;
        }




        //Login Table Database starts here..

        //public void InsertPassword(PasswordGeneration passwordGeneration, Registration registration)
        //{
        //    using (SqlConnection conn = new SqlConnection(istring))
        //    {
        //        conn.Open();
        //        string InsertLoginQuery = "UPDATE RegistrationDatabase SET Password=@ConfirmPassword WHERE Email=@Email";
                
        //        using (SqlCommand cmd = new SqlCommand(InsertLoginQuery, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Email", passwordGeneration.Email);
        //            cmd.Parameters.AddWithValue("@ConfirmPassword", passwordGeneration.ConfirmPassword);
        //            cmd.ExecuteNonQuery();
        //        }
        //        conn.Close();
        //    }
        //}






    }


    
}
