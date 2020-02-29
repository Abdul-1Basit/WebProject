using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TempConsELearning.Models;

namespace TempConsELearning.Controllers
{
    public class StudentController : Controller
    {
        ELearningEntitiesL DB = new ELearningEntitiesL();
        // GET: Student
        CustomFunctions cust = new CustomFunctions();
        public static int currentStudentid ;
        public ActionResult Index()
        {

            return View(DB.Courses.ToList());
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string StudentName,string Password)
        {
            Student Stu = DB.Students.Where(s => s.StudentName == StudentName && s.Password == Password).FirstOrDefault();
            if (Stu != null)
            {
                if (StudentName.Equals(Stu.StudentName) && Password.Equals(Stu.Password))
                {
                   FormsAuthentication.SetAuthCookie("Student", false);
                    //CurrentInstructorId = instruc.InstructorId;
                    //       ViewBag.Error = CurrentInstructorId;
                    //    return RedirectToAction("CheckCook");
                    currentStudentid = Stu.StudentId;
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Wrong Email or Password!";
                return View();
            }
            ViewBag.Error = StudentName+"Not Found";
            return View();

        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
        public ActionResult Alpha( int id)
        {
            return View(DB.Courses.Find(id));
        }

        public ActionResult SavedCourses()
        {
            List<StudentAssignment> sl= DB.StudentAssignments.Where(S => S.StudentId == currentStudentid).ToList();
            List<Course> cl=new List<Course>();
            foreach (StudentAssignment a in sl)
            {
                cl.Add(DB.Courses.Find(a.CourseId));
            }
           
            return View(cl);
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Student s, HttpPostedFileBase ImageFile)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string ext = Path.GetExtension(ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
                string img = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                ImageFile.SaveAs(fileName);
                s.Photo = img;
                DB.Students.Add(s);
                DB.SaveChanges();
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                ViewBag.val = "" + ex.InnerException;
                return View();
            }
        }
        public string Enroll(int id)
        {
            string asd = "Enrolled";
            try
            {
                StudentAssignment sp = new StudentAssignment();
                sp.StudentId = currentStudentid;
                sp.CourseId = id;
                DB.StudentAssignments.Add(sp);
                DB.SaveChanges();
            }
            catch (Exception ex) {
                asd = ""+ex.InnerException;
            }
            
            return asd;
            
        }
    }
}