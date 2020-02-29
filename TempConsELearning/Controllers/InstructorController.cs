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
    public class InstructorController : Controller
    {
        // GET: Instructor
       static int CurrentInstructorId { get; set; }
        public HttpPostedFile ImageFile { get; set; }
        string img { get; set; }
        ELearningEntitiesL DB = new ELearningEntitiesL();
        CustomFunctions cust = new CustomFunctions();

        public ActionResult SignIn()
        {

            return View();
        }


        [HttpPost]
        public ActionResult SignIn(string last_name, string password)
        {
            Instructor instruc = DB.Instructors.Where(s => s.InstructorName == last_name && s.Password == password).FirstOrDefault();
            if (instruc != null)
            { 
                if (last_name.Equals(instruc.InstructorName) && password.Equals(instruc.Password))
                {
                     FormsAuthentication.SetAuthCookie("Instructor", false);
                       CurrentInstructorId = instruc.InstructorId;
                 //       ViewBag.Error = CurrentInstructorId;
                     //    return RedirectToAction("CheckCook");
                  return RedirectToAction("Index");
                }
                ViewBag.Error = "Wrong Email or Password!";
                return View();
            }
            ViewBag.Error = "No User Found";
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }



        [Authorize]
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public PartialViewResult _ImageSaver (HttpPostedFile ps )
        {
            string fileName = Path.GetFileNameWithoutExtension(ps.FileName);
            string ext = Path.GetExtension(ps.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            img = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            ps.SaveAs(fileName);
            ViewBag.val = "Image has been saved";
            return PartialView("_ImageSaved");
         }



    [Authorize]
        public PartialViewResult _Create()
        {
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult _Create(Course ins,HttpPostedFileBase ImageFile)
        {


            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string ext = Path.GetExtension(ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            img = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            ImageFile.SaveAs(fileName);
               ins.CourseImage = img;
               ins.InstructorId = CurrentInstructorId;
            
            //cp.CourseName = ins.CourseName;
               //cp.CourseTitle = ins.CourseTitle;
               //cp.CourseDescription = ins.CourseDescription;
               //cp.CourseCategory = ins.CourseCategory;
               //cp.CourseFieldCategory = ins.CourseFieldCategory;
               try
               {
                   DB.Courses.Add(ins);
                   DB.SaveChanges();
                   DB.InstructorAssignments.SqlQuery("insert into InstructorAssignments(InstructorId,CourseId) values('"+ins.InstructorId+"','"+ins.CourseId+"')");
                   DB.SaveChanges();
                   return RedirectToAction("Index");
               }
               catch(Exception ex)
               {
                   Response.Write(ex.Message+"  inner exception "+ex.InnerException);
                   return View();
               }
         
             
        }
        [Authorize]
        public ActionResult _AddLecture(int id)
        {
            Lecture lsp = new Lecture();
            lsp.CourseId = id;
            ViewBag.id = id;
            return View(lsp);
        }

        [HttpPost]
        public ActionResult _AddLecture(Lecture lc,HttpPostedFileBase ImageFile)
        {
            

             string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
             string ext = Path.GetExtension(ImageFile.FileName);
             fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
             lc.VideoLink = "~/Videos/" + fileName;
             fileName = Path.Combine(Server.MapPath("~/Videos/"), fileName);
             ImageFile.SaveAs(fileName);

             try
             {
                 DB.Lectures.Add(lc);
                 DB.SaveChanges();

                 return RedirectToAction("Index");
             }
             catch (Exception xe)
             {
                 Response.Write("EROOR"+xe.InnerException);
                 return View();
             }
        }



        [Authorize]
        public PartialViewResult _InstructorDetails()
        {
            return PartialView(DB.Instructors.Find(CurrentInstructorId));
        }
        [Authorize]
        public PartialViewResult _MyCourses()
        {

            
            return PartialView(DB.Courses.Where(s=>s.InstructorId==CurrentInstructorId).ToList());
        }
        [Authorize]
        public PartialViewResult _NewCourse()
        {
            return PartialView();
        }

        public ActionResult _ViewDetails(int id)
        {
            return View(DB.Courses.Find(id));
        }



        public ActionResult _SignUp()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult _SignUp(Instructor i, HttpPostedFileBase ImageFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string ext = Path.GetExtension(ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            img = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            ImageFile.SaveAs(fileName);
            i.Image= img;
           
            try
            {
                if(cust.addInstructor(i))
                return RedirectToAction("SignIn");

            }
            catch (Exception ex)
            {
                ViewBag.val = "Error is " + ex.Message;
                
            }
            return View();
        }
    }
}