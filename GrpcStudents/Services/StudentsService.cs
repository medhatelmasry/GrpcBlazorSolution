using System;
using Grpc.Core;
using GrpcStudents.Data;
using GrpcStudents.Models;

namespace GrpcStudents.Services;

public class StudentsService : StudentRemote.StudentRemoteBase
{
    private readonly ILogger<StudentsService> _logger;
    private readonly SchoolDbContext _context;

    public StudentsService(ILogger<StudentsService> logger, SchoolDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public override Task<StudentModel> GetStudentInfo(StudentLookupModel request, ServerCallContext context)
    {
        StudentModel output = new StudentModel();

        var c = _context.Students!.Find(request.StudentId);

        _logger.LogInformation("Sending Student response");

        if (c != null)
        {
            output.StudentId = c.StudentId;
            output.FirstName = c.FirstName;
            output.LastName = c.LastName;
            output.School = c.School;
        }

        return Task.FromResult(output);
    }

    public override Task<Reply> InsertStudent(StudentModel request, ServerCallContext context)
    {
        var c = _context.Students!.Find(request.StudentId);

        if (c != null)
        {
            return Task.FromResult(
              new Reply()
              {
                  Result = $"Student {request.FirstName} {request.LastName} already exists.",
                  IsOk = false
              }
            );
        }

        Student student = new Student()
        {
            StudentId = request.StudentId,
            LastName = request.LastName,
            FirstName = request.FirstName,
            School = request.School,
        };

        _logger.LogInformation("Sending student response");

        try
        {
            _context.Students!.Add(student);
            var returnVal = _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.ToString());
        }

        return Task.FromResult(
           new Reply()
           {
               Result = $"Student {request.FirstName} {request.LastName} was successfully inserted.",
               IsOk = true
           }
        );
    }

    public override Task<Reply> UpdateStudent(StudentModel request, ServerCallContext context)
    {
        var c = _context.Students!.Find(request.StudentId);

        if (c == null)
        {
            return Task.FromResult(
              new Reply()
              {
                  Result = $"Student {request.FirstName} {request.LastName} cannot be found.",
                  IsOk = false
              }
            );
        }

        c.FirstName = request.FirstName;
        c.LastName = request.LastName;
        c.School = request.School;

        _logger.LogInformation("Sending Student response");

        try
        {
            var returnVal = _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.ToString());
        }

        return Task.FromResult(
           new Reply()
           {
               Result = $"Student {request.FirstName} {request.LastName} was successfully updated.",
               IsOk = true
           }
        );
    }

    public override Task<Reply> DeleteStudent(StudentLookupModel request, ServerCallContext context)
    {
        var c = _context.Students!.Find(request.StudentId);

        if (c == null)
        {
            return Task.FromResult(
              new Reply()
              {
                  Result = $"Student ID {request.StudentId} cannot be found.",
                  IsOk = false
              }
            );
        }

        _logger.LogInformation("Sending Student response");

        try
        {
            _context.Students!.Remove(c);
            var returnVal = _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.ToString());
        }

        return Task.FromResult(
           new Reply()
           {
               Result = $"Student with {request.StudentId} was successfully deleted.",
               IsOk = true
           }
        );
    }
    public override Task<StudentList> RetrieveAllStudents(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("Sending Student response");

        StudentList list = new StudentList();

        try
        {
            List<StudentModel> studentList = new List<StudentModel>();

            var students = _context.Students!.ToList();

            foreach (Student c in students)
            {
                studentList.Add(new StudentModel()
                {
                    StudentId = c.StudentId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    School = c.School,
                });
            }

            list.Items.AddRange(studentList);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.ToString());
        }

        return Task.FromResult(list);
    }

}

