using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcStudents.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcStudents.Data;
public class SchoolDbContext : DbContext {
  public DbSet<Student>? Students { get; set; }

  public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);

      builder.Entity<Student>().HasData(
        new {
            StudentId = 1,
            FirstName = "Jane",
            LastName = "Smith",
            School = "Medicine"
        }, new {
            StudentId = 2,
            FirstName = "John",
            LastName = "Fisher",
            School = "Engineering"
        }, new {
            StudentId = 3,
            FirstName = "Pamela",
            LastName = "Baker",
            School = "Food Science"
        }, new {
            StudentId = 4,
            FirstName = "Peter",
            LastName = "Taylor",
            School = "Mining"
        }
      );
  }
}
