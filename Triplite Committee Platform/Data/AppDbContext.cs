using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ScholarshipModel> Scholarship { get; set; }
        public DbSet<CollegeModel> College { get; set; }
        public DbSet<DepartmentModel> Department { get; set; }
        public DbSet<RequestTypeModel> RequestType { get; set; }
    }
}