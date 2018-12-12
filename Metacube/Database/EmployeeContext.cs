namespace sample.model
{
    using Metacube.Models;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

    }
}