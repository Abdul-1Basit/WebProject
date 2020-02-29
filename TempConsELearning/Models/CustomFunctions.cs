using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TempConsELearning.Models
{
    public class CustomFunctions
    {
        ELearningEntitiesL db = new ELearningEntitiesL();
        static string conStr = @"server=JHONCENA\SQLEXPRESS;Initial Catalog=ELearning; integrated security=true";
        SqlConnection con = new SqlConnection(conStr);


        //---Get all courses--
        public List<Course> getAllCourses()
        {
            List<Course> cl = new List<Course>();
            Course c = new Course();
            con.Open();
            SqlCommand cmd = new SqlCommand("select CourseName,CourseTitle,CourseDescription,Language,Ratings,CourseFieldCategory,CourseCategory,CourseImage from Course", con);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (sdr.HasRows) { 
                c.CourseName = sdr["CourseName"].ToString();
                c.CourseTitle = sdr["CourseTitle"].ToString();
                c.CourseDescription = sdr["CourseDescription"].ToString();
                c.Language = sdr["Language"].ToString();
                c.Ratings = Convert.ToInt32(sdr["Ratings"]);
                c.CourseFieldCategory = sdr["CourseFieldCategory"].ToString();
                c.CourseCategory = sdr["CourseCategory"].ToString();
                c.CourseImage = sdr["CourseImage"].ToString();
                cl.Add(c);
                }

            }
            return cl;
        }

        public bool addInstructor(Instructor i)
        {
            con.Open();
                SqlCommand cmd = new SqlCommand("insert into Instructor(Image,InstructorName,Profession,About,Degree1,Degree2,Degree3,Password) values('" + i.Image + "','" + i.InstructorName + "','" + i.Profession + "','" + i.About + "','" + i.Degree1 + "','" + i.Degree2 + "','" + i.Degree3 +"','"+i.Password+ "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;



           
        }
       

        public List<Course> getCart(string a)
        {
            List<Course> clr = new List<Course>();
            con.Open();
            SqlCommand cmd = new SqlCommand("select CourseId from StudentAssignment where StudentId='"+a+"'",con);
            SqlDataReader sdr = cmd.ExecuteReader();
          string CourseIds = "";
            List<string> sl = new List<string>();
           
            while (sdr.HasRows)
            {
                    sdr.Read();
               
                int alpha = Convert.ToInt32(sdr.ToString());
                    clr.Add(db.Courses.Find(alpha));
            
            }
            con.Close();
            return clr;
        }

        public List<Instructor> getTopThree()
        {
            List<Instructor> s = new List<Instructor>();
            Instructor ins = new Instructor();
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT [Image],InstructorName,NoOfCoursesTaught,Rating,About FROM Instructor WHERE Rating>=2", con);
            SqlDataReader sdr = cmd.ExecuteReader();
           
            while (sdr.HasRows)
            {
                sdr.Read();
                ins.Image = sdr[0].ToString();
                ins.InstructorName= sdr[1].ToString();
                ins.NoOfCoursesTaught= Convert.ToInt16(sdr[2]);
                ins.Rating= Convert.ToInt16(sdr[3].ToString());
                ins.About= sdr[4].ToString();
                s.Add(ins);
            }
            con.Close();

            return s;
        }




        //---------Instructor Controller Functions

        public List<Course> getInstructorAssignedCourses(int id)
        {
            List<Course> sl = new List<Course>();
            List<InstructorAssignment> AssIds = new List<InstructorAssignment>();

            AssIds = db.InstructorAssignments.ToList();

            foreach (InstructorAssignment i in AssIds)
            {
                if (i.InstructorId.Equals(id))
                sl.Add(db.Courses.Find(i.CourseId));
            }
            return sl;
        }


        //----------Lectures-------------
        public List<Lecture> GetLecs(int id)
        {

            return db.Lectures.Where(s => s.CourseId == id).ToList();
        }






    }





}