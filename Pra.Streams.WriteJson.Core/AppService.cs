using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Pra.Streams.WriteJson.Core
{
    public class AppService
    {
        private List<Student> students;
        private List<Course> courses;
        private List<Enrollment> enrollments;
        public IEnumerable <Student> Students
        {
            get { return students.AsReadOnly(); }
        }
        public IEnumerable<Course> Courses
        {
            get { return courses.AsReadOnly(); }
        }
        public IEnumerable<Enrollment> Enrollments
        {
            get { return enrollments.AsReadOnly(); }
        }
        public void AddEnrollment(Enrollment enrollment)
        {
            foreach(Enrollment enr in enrollments)
            {
                if(enr.Student == enrollment.Student && enr.Course == enrollment.Course)
                {
                    return;
                }
            }
            enrollments.Add(enrollment);
            enrollments = enrollments.OrderBy(e => e.Course.CourseName).ToList();

        }
        public void RemoveEnrollment(Enrollment enrollment)
        {
            enrollments.Remove(enrollment);
            enrollments = enrollments.OrderBy(e => e.Course.CourseName).ToList();

        }
        public AppService()
        {
            GetData();
        }
        private void GetData()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\students.json"))
                ReadStudentJson();
            else
                DoStudentSeeding();
            if (File.Exists(Environment.CurrentDirectory + "\\courses.json"))
                ReadCourseJson();
            else
                DoCourseSeeding();
            if (File.Exists(Environment.CurrentDirectory + "\\enrollments.json"))
                ReadEnrollmentJson();

            if (students is null) students = new List<Student>();
            if (courses is null) courses = new List<Course>();
            if (enrollments is null) enrollments = new List<Enrollment>();
            
        }
        private void ReadStudentJson()
        {
            string content = File.ReadAllText(Environment.CurrentDirectory + "\\students.json");
            students = JsonConvert.DeserializeObject<List<Student>>(content);
            students = students.OrderBy(s => s.Lastname).ThenBy(s => s.Firstname).ToList();
        }
        private void ReadCourseJson()
        {
            string content = File.ReadAllText(Environment.CurrentDirectory + "\\courses.json");
            courses = JsonConvert.DeserializeObject<List<Course>>(content);
            courses = courses.OrderBy(c=>c.CourseName).ToList();
        }
        private void ReadEnrollmentJson()
        {
            string content = File.ReadAllText(Environment.CurrentDirectory + "\\enrollments.json");
            enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
            
            // enrollments corrigeren
            foreach(Enrollment enrollment in enrollments)
            {
                foreach(Student student in students)
                {
                    if(CompareObjects(student, enrollment.Student))
                    {
                        enrollment.Student = student;
                    }
                }
                foreach(Course course in courses)
                {
                    if (CompareObjects(course, enrollment.Course))
                    {
                        enrollment.Course = course;
                    }
                }
            }
            enrollments = enrollments.OrderBy(e => e.Course.CourseName).ToList();
        }
        private bool CompareObjects(object a, object b)
        {
            foreach(var property in a.GetType().GetProperties())
            {
                string propa = property.GetValue(a).ToString();
                string propb = property.GetValue(b).ToString();
                if(propa != propb)
                {
                    return false;
                }
            }
            return true;
        }
        private void DoStudentSeeding()
        {
            students = new List<Student>();
            students.Add(new Student("Janssens", "Jan", "Brugge"));
            students.Add(new Student("Willems", "Wim", "Brugge"));
            students.Add(new Student("Kaerels", "Charel", "Brugge"));
            students.Add(new Student("Taer", "Guy", "Brugge"));
            students.Add(new Student("Banjo", "Lien", "Brugge"));
            students.Add(new Student("Tumas", "Marie", "Brugge"));
        }
        private void DoCourseSeeding()
        {
            courses = new List<Course>();
            courses.Add(new Course("Programming Basics"));
            courses.Add(new Course("Programming Advanced"));
            courses.Add(new Course("Web Frontend Basics"));
            courses.Add(new Course("Web Frontend Advanced"));
            courses.Add(new Course("Databases"));
            courses.Add(new Course("Basic IT Skills"));
            courses.Add(new Course("Workplace exploring"));
        }
        public void WriteJsons()
        {
            string content = JsonConvert.SerializeObject(students);
            File.WriteAllText(Environment.CurrentDirectory + "\\students.json", content);
            content = JsonConvert.SerializeObject(courses);
            File.WriteAllText(Environment.CurrentDirectory + "\\courses.json", content);
            content = JsonConvert.SerializeObject(enrollments);
            File.WriteAllText(Environment.CurrentDirectory + "\\enrollments.json", content);
        }
    }
}
