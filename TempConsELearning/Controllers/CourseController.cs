using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TempConsELearning.Models;
namespace TempConsELearning.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        ELearningEntitiesL DB = new ELearningEntitiesL();

        CustomFunctions custFunc = new CustomFunctions();
        public ActionResult Index()
        {
            return View(DB.Courses.ToList());
            
          
        }
        public ActionResult ViewCourse(string id)
        {
            Response.Write("<script> alert('" + id + "')</script>");
            return View(DB.Courses.Find(id));
        }
        [HttpPost]
        public ActionResult ViewCourse(Course cp)
        {
            
            return RedirectToAction("Index");

        }
        public ActionResult Alpha( int id)
        {
            return View(DB.Courses.Find(id));
        }

        public ActionResult InstructorData(int id)
        {
            return PartialView(DB.Instructors.Find(id));
        }
        public ActionResult Cart()
        {
            FormsAuthentication.SetAuthCookie("1223",false);

            Response.Write("<script> alert('"+FormsAuthentication.FormsCookieName+"')</script>");
            return View(custFunc.getCart("112"));
        }


        public ActionResult _InstructorSlider()
        {
            return PartialView(DB.Instructors.OrderByDescending(x=>x.Rating).Take(3).ToList());
        }

        public PartialViewResult SpecificCourse(string d)
        {
            return PartialView(DB.Courses.SqlQuery("select * from course where CourseFieldCategory ='"+d+"'").ToList());
        }




        public ActionResult addLecture()
        {
            return View();
        }

        public ActionResult _GetLectures(int id)
        {
            
            return PartialView(DB.Lectures.OrderBy(x => x.LectureNo).Where(x=>x.CourseId==id).ToList());
        }
        public ActionResult SortBy(string val)
        {
            return View(DB.Courses.SqlQuery("select * from course where CourseFieldCategory ='" + val + "'"));
        }


    }
}