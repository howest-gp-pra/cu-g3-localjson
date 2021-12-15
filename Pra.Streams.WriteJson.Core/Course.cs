using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Streams.WriteJson.Core
{
    public class Course
    {
        public string CourseName { get; set; }
        public Course(string courseName)
        {
            CourseName = courseName;
        }
        public override string ToString()
        {
            return CourseName;
        }
    }
}
