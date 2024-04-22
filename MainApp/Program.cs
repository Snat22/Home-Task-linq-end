using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using MainApp.Data;
using MainApp.Data.Entity;
using MainApp.Data.Entity.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


#region 1. Выбрать всех студентов и их группы, отсортированных по курсам, а затем по фамилии студента:
//
// var query = (from s in DataProvider.Students
//     join sg in DataProvider.StudentGroups on s.Id equals sg.StudentId
//     join g in DataProvider.Groups on sg.GroupId equals g.Id
//     join c in DataProvider.Courses on g.CourseId equals c.Id
//     orderby c.CourseName
//     select new
//     {
//         Student = s,
//         Group = g,
//         CourseName=c.CourseName
//     }).OrderBy(x => x.Student.LastName).ToList();
//
// foreach (var item in query)
// {
//     Console.WriteLine("-------------------------------------------------------------");
//     Console.WriteLine();
//     Console.WriteLine($"{item.CourseName} - Student: ({item.Student.FirstName} - {item.Student.LastName}) || Group: {item.Group.GroupName} - {item.Group.Description}");
//     Console.WriteLine();
// }

#endregion


#region 2. Выбрать всех менторов и количество групп, которые они ведут:

// var mentors = (from m in DataProvider.Mentors
//     let countG = DataProvider.MentorGroups.Count(x => x.MentorId == m.Id)
//     select new
//     {
//         Mentor = m,
//         CounrOfGroups = countG
//     }).ToList();
//           
//           
//
// foreach (var m in mentors)
// {
//     Console.WriteLine($"{string.Concat(m.Mentor.FirstName+" "+m.Mentor.LastName)} - {m.CounrOfGroups}");
// }

#endregion


#region 3.Выбрать все курсы, в которых нет групп:

// var courses = (from c in DataProvider.Courses
//     let countCourse = DataProvider.Groups.Count(x => x.CourseId == c.Id)
//     where countCourse == 0
//     select new
//     {
//         Course = c
//     });
//
// foreach (var c in courses)
// {
//     Console.WriteLine(c.Course.CourseName);
// }

#endregion


#region 4. Выбрать все группы с количеством студентов и менторов в каждой группе:
//
// var groups = (from g in DataProvider.Groups
//     let countStudent = DataProvider.StudentGroups.Count(x => x.GroupId == g.Id)
//     let countMentor = DataProvider.MentorGroups.Count(x => x.GroupId == g.Id)
//     select new
//     {
//         Group = g,
//         CountStudent = countStudent,
//         CountMentor = countMentor
//     }).ToList();
//
// foreach (var g in groups)
// {
//     Console.WriteLine($"Group: {g.Group.GroupName}- CountStudent: {g.CountStudent}- CountMentor: {g.CountMentor} ");
// }

#endregion


#region 5.Выбрать всех студентов, у которых количество групп больше среднего:

// var avg = DataProvider.StudentGroups.Average(x => x.StudentId);
//
// var result = (from s in DataProvider.Students
//     let count = DataProvider.StudentGroups.Count(x => x.StudentId == s.Id)
//     where count > avg
//     select new
//     {
//         Student = s
//     });
//     
//         
//
// foreach (var s in result)
// {
//     Console.WriteLine(s.Student.FirstName+" " + s.Student.LastName+" "+ avg);
// }

#endregion



#region 6.Выбрать всех менторов, у которых количество студентов в группах больше определенного значения:
//
// var max = 5;
//
// var mentorGroup = from g in DataProvider.Groups
//     let students = DataProvider.StudentGroups.Count(x => x.GroupId == g.Id)
//     join mg in DataProvider.MentorGroups on g.Id equals mg.GroupId
//     join m in DataProvider.Mentors on mg.MentorId equals m.Id
//     where students < max
//     select new
//     {
//         Mentor = m.FirstName,
//         Count = students
//
//     };
//    
//
// foreach (var mg in mentorGroup)
// {
//    
//         Console.WriteLine($"{mg.Mentor} {mg.Count}");
// }
//
#endregion

#region 7.Выбрать все группы, в которых есть студенты обоих полов:
// var query = (from g in DataProvider.Groups 
//             where DataProvider.Students.Any(e=>e.Gender==Gender.Male && e.Gender ==Gender.Female)
//             group g by g.GroupName into StdGroup
//             select new 
//             {
//                 StdGroup.Key,
//                 Group = StdGroup
//             }
// );

// foreach (var item in query)
// {
//     System.Console.WriteLine($"{item.Key}");
// }
#endregion 


//
#region 8. Выбрать всех студентов, которые не состоят в группе с определенным ментором:

// var mentorId = 3;
// var result = (from std in DataProvider.Students
//             join stdGr in DataProvider.StudentGroups on std.Id equals stdGr.StudentId 
//             join mentorGr in DataProvider.MentorGroups on stdGr.GroupId equals mentorGr.GroupId
//             where mentorGr.MentorId == mentorId && !DataProvider.StudentGroups.Any(s => s.StudentId == std.Id && s.GroupId == mentorGr.GroupId)
//             select new{ student = std });

// foreach (var item in result)
// {
//     System.Console.WriteLine($"{item.student.FirstName} {item.student.FirstName}");
// }

    
#endregion

//

//9
//Выбрать всех менторов, которые ведут группу на курсе с наименьшим количеством студентов:
var query = (from mentor in DataProvider.Mentors

    join mentorGr in DataProvider.MentorGroups on mentor.Id equals mentorGr.MentorId

    join gr in DataProvider.Groups on mentorGr.GroupId equals gr.Id
    
    join c in DataProvider.Courses on gr.CourseId equals c.Id
    
    join studentGroup in DataProvider.StudentGroups on gr.Id equals studentGroup.GroupId into sg
    
    let stdCount = sg.Count()

    where c.Id == DataProvider.Courses.Min(c => c.Id)
    select new {
        Mentor = mentor,
        StudentCount = stdCount
        });

var StudentKamtarin = query.Min(m => m.StudentCount);

var result = query.Where(m => m.StudentCount == StudentKamtarin).Select(m => m.Mentor);

System.Console.WriteLine($"Count  Student: {StudentKamtarin}");
    foreach (var item in result)
    {
        System.Console.WriteLine($"Mentor Id: {item.Id} First Name: {item.FirstName} Last Name: {item.LastName}");
    }

//10
//Выбрать всех студентов, которые состоят в группе на курсе с наибольшим количеством менторов:
// var query = from st in DataProvider.Students
//         join sdtgr in DataProvider.StudentGroups on st.Id equals sdtgr.StudentId
//         join gr in DataProvider.Groups on sdtgr.GroupId equals gr.Id
//         join mgr in DataProvider.MentorGroups on


#region 11.Выбрать всех студентов, принадлежащих курсу, который имеет больше всего групп:
// var query = (from stdgr in DataProvider.StudentGroups
//             join gr in DataProvider.Groups on stdgr.GroupId equals gr.Id
//             join c in DataProvider.Courses on gr.CourseId equals c.Id
//             select new
//             {
//                 std = stdgr.Student
//             }
//         ).GroupBy(x=>x.st)




    // var query = (from c in DataProvider.Courses
    //             join gr in DataProvider.Groups on c.Id equals gr.CourseId
    //             join stdgr in DataProvider.StudentGroups on gr.Id equals stdgr.GroupId
    //             join std in DataProvider.Students on stdgr.StudentId equals std.Id
    //             select new
    //             {
    //                 Course = c,
    //                 Student = std
    //             }
    //         ).GroupBy(x=>x.Course.Id).OrderByDescending(g=>g.Count()).First();
            
    //         foreach (var item in query)
    //         {
    //             System.Console.WriteLine($"{item.Course.CourseName} {item.Student.FirstName}");
    //         }
#endregion


#region 12. Выбрать все группы, в которых количество студентов превышает количество менторов более чем в два раза:

    // var query = (from g in DataProvider.Groups 
    //             let CountStd = DataProvider.Students.Count()
    //             let CountMen = DataProvider.Mentors.Count()
    //             where CountStd > CountMen * 2
    //             select new 
    //             {
    //                 Groups = g 
    //             }
    //                 );

    //                 foreach (var item in query)
    //                 {
    //                     System.Console.WriteLine($"{item.Groups.GroupName}");
    //                 }
    
#endregion


#region 13.Выбрать все группы, которые имеют одинаковое количество студентов и менторов:

    // var query = (from g in DataProvider.Groups 
    //             let CountStd = DataProvider.Students.Count()
    //             let CountMentor = DataProvider.Mentors.Count()
    //             where CountStd == CountMentor
    //             select new 
    //             {
    //                 Groups = g 
    //             }
    //                 );

    //                 foreach (var item in query)
    //                 {
    //                     System.Console.WriteLine($"{item.Groups.GroupName}");
    //                 }
#endregion

#region 14 .Выбрать всех студентов, которые состоят во всех группах на всех курсах:

    
#endregion


#region 15.Выбрать все курсы, в которых количество групп превышает количество студентов:

    // var query = (from c in DataProvider.Courses
    //             let CountStd = DataProvider.Students.Count()
    //             let CountGr = DataProvider.Groups.Count()
    //             where CountGr > CountStd
    //             select new {
    //                 Course = c
    //             }
    //             );
    //             foreach (var item in query)
    //             {
    //                 System.Console.WriteLine($"{item.Course.CourseName}");
    //             }
#endregion

//16
//Выбрать всех студентов, которые состоят во всех группах на курсе с наименьшим количеством менторов:

// var query = (from std in DataProvider.Students
//             join stdGr in DataProvider.StudentGroups on std.Id equals stdGr.StudentId
//             join gr in DataProvider.Groups on stdGr.GroupId equals gr.Id
//             join c in DataProvider.Courses on gr.CourseId equals c.Id
            

//             select new 
//             {
//                 Student = std
//             }
// );


// foreach (var item in query)
// {
//     System.Console.WriteLine($"{item.Student.FirstName}");
// }

// foreach (var item in result)
// {
//     Console.WriteLine($"Mentor: {item.Mentor.Name}, Students Count: {item.StudentCount}");
// }




