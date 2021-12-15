using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Streams.WriteJson.Core
{
    public class Enrollment
    {
        public Student Student { get; set; }
        public Course Course { get; set; }
        public Enrollment(Student student, Course course)
        {
            Student = student;
            Course = course;
        }
        public override string ToString()
        {
            return Course.CourseName;
        }
    }
}
