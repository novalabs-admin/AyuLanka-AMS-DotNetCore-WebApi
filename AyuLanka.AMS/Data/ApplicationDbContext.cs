using AyuLanka.AMS.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AyuLanka.AMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TreatmentType> TreatmentTypes { get; set; }
        public DbSet<ShiftMaster> ShiftMasters { get; set; }
        public DbSet<DayOffChangeReason> DayOffChangeReasons { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<TreatmentLocation> TreatmentLocations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<StaffAttendance> StaffAttendances { get; set; }
        public DbSet<StaffRoster> StaffRosters { get; set; }
        public DbSet<AppointmentSchedule> AppointmentSchedules { get; set; }
        public DbSet<StaffLeave> StaffLeaves { get; set; }
        public DbSet<DayOffChangeMaster> DayOffChangeMasters { get; set; }
        public DbSet<DayOffChangeLog> DayOffChangeLogs { get; set; }
        public DbSet<StaffRosterMaster> StaffRosterMasters { get; set; }
        public DbSet<DayOffChangeDetail> DayOffChangeDetails { get; set; }
        public DbSet<ShiftChangeMaster> ShiftChangeMasters { get; set; }
        public DbSet<ShiftChangeDetail> ShiftChangeDetails { get; set; }
        public DbSet<AppoinmentTreatment> AppoinmentTreatments { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DoctorSession> DoctorSessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ShiftMaster)
                .WithMany()
                .HasForeignKey(e => e.ShiftMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StaffRoster>()
                .HasOne(sr => sr.ShiftMaster)
                .WithMany()
                .HasForeignKey(sr => sr.ShiftMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StaffRoster>()
                .HasOne(sr => sr.Employee)
                .WithMany()
                .HasForeignKey(sr => sr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DayOffChangeLog>()
                .HasOne(dcl => dcl.StaffRoster)
                .WithMany()
                .HasForeignKey(dcl => dcl.StaffRosterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DayOffChangeLog>()
                .HasOne(dcl => dcl.DayOffChangeMaster)
                .WithMany()
                .HasForeignKey(dcl => dcl.DayOffChangeMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.LocationType)
                .WithMany(lt => lt.Locations)
                .HasForeignKey(l => l.LocationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company FK relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Company)
                .WithMany()
                .HasForeignKey(l => l.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StaffRosterMaster>()
                .HasOne(srm => srm.Company)
                .WithMany()
                .HasForeignKey(srm => srm.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppointmentSchedule>()
                .HasOne(a => a.Company)
                .WithMany()
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppointmentSchedule>()
                .HasOne(a => a.DoctorSession)
                .WithMany()
                .HasForeignKey(a => a.DoctorSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // DoctorSession FK relationships
            modelBuilder.Entity<DoctorSession>()
                .HasOne(ds => ds.Company)
                .WithMany()
                .HasForeignKey(ds => ds.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSession>()
                .HasOne(ds => ds.Doctor)
                .WithMany()
                .HasForeignKey(ds => ds.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSession>()
                .HasOne(ds => ds.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(ds => ds.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
