using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment_7
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             *  Note that Courses.csv is a folder named App_Data, this folder is located at
             *      bin\Debug\netcoreapp3.1
             *  in the program's folder, this is because that is where it defaults to read from.
             *  
             *  Instructors.csv is also in this same App_Data folder if you want to read it.
             */



            List<Course> courses = new List<Course>();
            var filepath = @"App_Data\\Courses.csv";
            var txtread = new StreamReader(filepath);

            //Begin reading the Courses.csv file

            String line = txtread.ReadLine();   //the first line will hold no relevant info
            if(line != null)
            {
                line = txtread.ReadLine();  //get the second line
            }

            //1.1
            while (line != null)
            {
                String[] columns = line.Split(','); //length of 11

                //create a new Course object using the data from the Courses.csv file
                Course newCourse = new Course(columns[2], columns[0], columns[1], columns[3], columns[7], columns[10]);

                //add it to the course list
                courses.Add(newCourse);

                line = txtread.ReadLine();
            }

            //1.1 turn the list into an in-memory array
            Course[] courseArray = courses.ToArray();

            //1.2 a
            IEnumerable<Course> myQuerryA =
                from c in courseArray
                where Int32.Parse(c.getCourseCode()) >= 300
                where c.getSubject() == "IEE"
                orderby c.getInstructor()
                select c;

            Console.WriteLine("----------1.2 A----------");

            foreach(Course course in myQuerryA)
            {
                if(course.getTitle().Length > 23) Console.WriteLine(course.getTitle() + "\t\t" + course.getInstructor());
                else if(course.getTitle().Length > 15) Console.WriteLine(course.getTitle() + "\t\t\t" + course.getInstructor());
                else Console.WriteLine(course.getTitle() + "\t\t\t\t" + course.getInstructor());
            }

            Console.WriteLine();

            //1.2 b
            var subjectGroup =
                from c in courseArray
                group c by c.getSubject() into group1
                from group2 in
                    (from c in group1
                     group c by c.getCourseCode())
                group group2 by group1.Key;


            Console.WriteLine("----------1.2 B----------");

            foreach (var outer in subjectGroup)
            {
                if(outer.Count() >= 2)
                {
                    Console.WriteLine($"Class Subject:\t{outer.Key}");
                    foreach (var inner in outer)
                    {
                        Console.WriteLine($"\tClass Number:\t{inner.Key}");
                    }
                    Console.WriteLine();
                }
            }

            txtread.Close();

            //1.3 the Instructors.csv is in the same App_Data folder as Courses.csv

            //1.4
            List<Instructor> instructors = new List<Instructor>();
            filepath = @"App_Data\\Instructors.csv";
            txtread = new StreamReader(filepath);

            line = txtread.ReadLine();   //the first line will hold no relevant info
            if (line != null)
            {
                line = txtread.ReadLine();  //get the second line
            }

            while (line != null)
            {
                String[] columns = line.Split(','); //length of 3

                //create a new Instructor object using the data from the Instructors.csv file
                Instructor newIns = new Instructor(columns[0], columns[1], columns[2]);

                //add it to the course list
                instructors.Add(newIns);

                line = txtread.ReadLine();
            }

            Console.WriteLine("----------1.5----------");

            /*
             * For the following LINQ querry, since it says to only print out the 200 level courses, I went ahead and only
             *      returned the 200 level courses. If you want to test the querry for other course levels, you can change
             *      or remove the "where" statements in the querry and add an "if" condition in the foreach print loop to
             *      only print the course level you want.
             */

            //1.5
            var courseEmails =
                from c in courseArray
                where Int32.Parse(c.getCourseCode()) >= 200
                where Int32.Parse(c.getCourseCode()) < 300
                orderby c.getCourseCode()
                let cSubject = c.getSubject()
                let cCode = c.getCourseCode()
                let ins = c.getInstructor()
                from i in instructors
                where i.getName() == ins
                let email = i.getEmail()
                select new { Subject = cSubject, CourseCode = cCode, insEmail = email };

            foreach(var item in courseEmails)
            {
                Console.WriteLine(item.Subject + "\t" + item.CourseCode + "\t" + item.insEmail);
            }
        }
    }

    //1.1
    class Course
    {
        String CourseId, Subject, CourseCode, Title, Location, Instructor = "";

        public Course(String _id, String _subject, String _code, String _title, String _location, String _instructor)
        {
            this.CourseId = _id;
            this.Subject = _subject;
            this.CourseCode = _code;
            this.Title = _title;
            this.Location = _location;
            this.Instructor = _instructor;
        }

        public String getId()
        {
            return this.CourseId;
        }

        public String getSubject()
        {
            return this.Subject;
        }

        public String getCourseCode()
        {
            return this.CourseCode;
        }

        public String getTitle()
        {
            return this.Title;
        }

        public String getLocation()
        {
            return this.Location;
        }

        public String getInstructor()
        {
            return this.Instructor;
        }
    }

    //1.4
    class Instructor
    {
        String InstructorName, OfficeNumber, EmailAddress = "";

        public Instructor(String name, String officeNum, String email)
        {
            this.InstructorName = name;
            this.OfficeNumber = officeNum;
            this.EmailAddress = email;
        }

        public String getName()
        {
            return this.InstructorName;
        }

        public String getOfficeNum()
        {
            return this.OfficeNumber;
        }

        public String getEmail()
        {
            return this.EmailAddress;
        }
    }
}
