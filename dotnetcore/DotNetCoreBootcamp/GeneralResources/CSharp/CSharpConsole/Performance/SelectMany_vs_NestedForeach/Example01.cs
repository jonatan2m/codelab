using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharpConsole.Performance.SelectMany_vs_NestedForeach
{
    public class Courses
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Courses> Courses { get; set; }
    }

    public class Example01
    {
        List<Student> students;

        public Example01()
        {
            students = new List<Student>();
            for (int i = 0; i < 1000; i++)
            {
                var student = new Student();
                student.Id = i;
                student.Name = $"Name_{i}";
                student.Courses = new List<Courses>();
                for (int j = 0; j < 1000; j++)
                {
                    student.Courses.Add(new Courses
                    {
                        Code = $"Code_{j}",
                        Id = j
                    });
                }
                students.Add(student);
            }
        }

        public IEnumerable<string> NestedForeach()
        {   
            List<string> coursesAndStudents = new List<string>();
            foreach (var student in students)
            {
                foreach (var course in student.Courses)
                {
                    //coursesAndStudents.Add($"{student.Name}>{course.Code}");
                    yield return $"{student.Name}>{course.Code}";
                }
            }
        }

        public IEnumerable<string> SelectMany()
        {           
            return students.SelectMany(x => x.Courses,
                (student, course) =>
                {
                    return $"{student.Name}>{course.Code}";
                });
           
        }
    }
}
