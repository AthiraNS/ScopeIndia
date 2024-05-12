using Microsoft.Data.SqlClient;
using ScopeIndia.Models;
using System.Data;

namespace ScopeIndia.Data
{
    public class SqlCourse : ICourse
    {
        private readonly string istring;
        public SqlCourse(IConfiguration configuration)
        {
            istring = configuration.GetConnectionString("istring");

        }

        public CourseDetails GetById(int CourseId)
        {
            CourseDetails details = new CourseDetails();

            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string selquery = "SELECT * FROM CourseTable WHERE CourseId=@CourseId";
                using (SqlCommand cmd = new SqlCommand(selquery, conn))
                {
                    cmd.Parameters.AddWithValue("CourseId", CourseId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            details.CourseId = reader.GetInt32(0);
                            details.CourseName = reader.GetString(1);
                            details.CourseDuration = reader.GetString(2);
                            details.CourseFee = reader.GetString(3);
                            //details.CourseFee = reader.GetInt32(3);

                        }

                    }
                }
                conn.Close();
            }
            return details;
        }


        public List<CourseDetails> GetAll()
        {
            List<CourseDetails> course = new List<CourseDetails>();

            using (SqlConnection conn = new SqlConnection(istring))
            {
                conn.Open();
                string selquery = "SELECT * FROM CourseTable";
                using (SqlCommand cmd = new SqlCommand(selquery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int? courseFee = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                            course.Add(new CourseDetails
                            {
                                CourseId = reader.GetInt32(0),
                                CourseName = reader.GetString(1),
                                CourseDuration = reader.GetString(2),
                                CourseFee = reader.GetString(3)
                                //CourseFee = reader.GetInt32(3),


                            });
                        }
                    }
                }
                conn.Close();
            }
            return course;
        }






    }
}
