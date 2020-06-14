﻿using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.CourseFeedbackModels;
using IptApis.Models.CouseFeedbackModels;
using System.Web.Http.Cors;

namespace IptApis.Controllers
{
    //CourseEnrollment ->FacultySections->CourseFaculty->CourseOffered->Course
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    [RoutePrefix("")]
    public class CourseFeedbackController : ApiController
    {
        [HttpGet]
        public string test()
        {
            return "Hello this is cafeteria";
        }

        [HttpGet]
        public Student getCurrentStudent(string studentRollNo)
        {
            //Will return the current student from the session(Logged in)
            //Currently no session is setup i.e why taking student id in parameter
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> responseStudent;
            responseStudent = db.Query("Student").Where("RollNumber", studentRollNo).Get().Cast<IDictionary<string, object>>();
            Student x = new Student();
            foreach (var res in responseStudent)
            {
                x.Name = res["SName"].ToString();
                x.Email = res["Email"].ToString();
                x.MobileNo = res["MobileNumber"].ToString();
                x.RollNo = res["RollNumber"].ToString();
                x.StudentID = res["StudentID"].ToString();
            }
            return x;
        }

        [HttpGet]
        public List<Course> getAllCourses(string studentID)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> responseStudent;
            responseStudent = db.Query("AllStudentCourses").Where("StudentID", studentID).Get().Cast<IDictionary<string, object>>();
            List<Course> courses = new List<Course>();
            foreach (var res in responseStudent)
            {
                Course course = new Course();
                course.CourseName = res["CourseName"].ToString();
                course.CourseCode = res["CourseCode"].ToString();
                course.CourseID = res["CourseID"].ToString();

                courses.Add(course);
            }
            //Will return the list of courses current student is enrolled in
            return courses;
        }

        [HttpGet]
        public List<Questions> getQuestions(string courseName, string courseType)
        {
            //Will return the list of questions/options based on course/course type

            List<Questions> questions = new List<Questions>();
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            if (courseType == "1")
            {
                //CourseType is Theory
                
                IEnumerable<IDictionary<string, object>> responseQuestions;
                IEnumerable<IDictionary<string, object>> responseOptions;

                responseQuestions = db.Query("Question").Where("CourseType", "1").Get().Cast<IDictionary<string, object>>();
                responseOptions = db.Query("options").Where("QuestionID", "3").Get().Cast<IDictionary<string, object>>();
                
                foreach (var res in responseQuestions)
                {
                    Questions x=new Questions();
                    Debug.WriteLine(res);
                    x.QuestionText = res["Question"].ToString();
                    x.QuestionType = res["QuestionType"].ToString();
                    x.CourseType = res["CourseType"].ToString();
                    x.QuestionID = res["QuestionID"].ToString();

                    if (x.QuestionType=="2")
                    {
                       foreach(var z in responseOptions)
                        {
                            x.options.Add(z["Opt"].ToString());
                        }
                    }
                    questions.Add(x);
                }

                //Debug.(response);
                Debug.WriteLine("AAA");
                return questions;
            }
            else
            {
                //CourseType is Lab
                IEnumerable<IDictionary<string, object>> responseQuestions;
                IEnumerable<IDictionary<string, object>> responseOptions;

                responseQuestions = db.Query("Question").Where("CourseType", "2").Get().Cast<IDictionary<string, object>>();
                responseOptions = db.Query("options").Where("QuestionID", "3").Get().Cast<IDictionary<string, object>>();

                foreach (var res in responseQuestions)
                {
                    Questions x = new Questions();
                    Debug.WriteLine(res);
                    x.QuestionText = res["Question"].ToString();
                    x.QuestionType = res["QuestionType"].ToString();
                    x.CourseType = res["CourseType"].ToString();
                    x.QuestionID = res["QuestionID"].ToString();
                    if (x.QuestionType == "2")
                    {
                        foreach (var z in responseOptions)
                        {
                            x.options.Add(z["Opt"].ToString());
                        }
                    }
                    questions.Add(x);
                }

                //Debug.(response);
                Debug.WriteLine("AAA");
                return questions;

            }
        }


        [HttpPost]
        public object postAnswers([FromBody]Object test)
        {
            //Will accept the answers as post request and add them to DB
            //var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Product));
            return test;
        }

        [HttpGet]
        public HttpResponseMessage getQuestionAnswerPairs()
        {
            //LATER TASK
            //For admin portal to view all question and answer
            try
            {
                //return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                return Request.CreateResponse(HttpStatusCode.OK, "test");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);

            }

        }
    }
}
