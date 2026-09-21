using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.BusinessSevices;
using AyuLanka.AMS.Data;
using AyuLanka.AMS.Repositories.Contracts;
using AyuLanka.AMS.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Initialize and configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new AyuLanka.AMS.AMSWeb.Converters.TimeSpanJsonConverter());
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your application services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployementTypeRepository, EmployementTypeRepository>();
builder.Services.AddScoped<IEmployementTypeService, EmployementTypeService>();
builder.Services.AddScoped<IShiftMasterRepository, ShiftMasterRepository>();
builder.Services.AddScoped<IShiftMasterService, ShiftMasterService>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<IStaffLeaveRepository, StaffLeaveRepository>();
builder.Services.AddScoped<IStaffLeaveService, StaffLeaveService>();
builder.Services.AddScoped<ITreatmentTypeRepository, TreatmentTypeRepository>();
builder.Services.AddScoped<ITreatmentTypeService, TreatmentTypeService>();
builder.Services.AddScoped<ITreatmentLocationRepository, TreatmentLocationRepository>();
builder.Services.AddScoped<ITreatmentLocationService, TreatmentLocationService>();
builder.Services.AddScoped<IAppointmentScheduleRepository, AppointmentScheduleRepository>();
builder.Services.AddScoped<IAppointmentScheduleService, AppointmentScheduleService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IStaffRosterService, StaffRosterService>();
builder.Services.AddScoped<IStaffRosterMasterRepository, StaffRosterMasterRepository>();
builder.Services.AddScoped<IStaffRosterRepository, StaffRosterRepository>();
builder.Services.AddScoped<IDayOffChangeMasterService, DayOffChangeMasterService>();
builder.Services.AddScoped<IDayOffChangeMasterRepository, DayOffChangeMasterRepository>();
builder.Services.AddScoped<IDayOffChangeDetailRepository, DayOffChangeDetailRepository>();
builder.Services.AddScoped<IShiftChangeMasterService, ShiftChangeMasterService>();
builder.Services.AddScoped<IShiftChangeMasterRepository, ShiftChangeMasterRepository>();
builder.Services.AddScoped<IShiftChangeDetailRepository, ShiftChangeDetailRepository>();
builder.Services.AddScoped<IAppoinmentTreatmentService, AppoinmentTreatmentService>();
builder.Services.AddScoped<IAppoinmentTreatmentRepository, AppoinmentTreatmentRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDoctorSessionRepository, DoctorSessionRepository>();
builder.Services.AddScoped<IDoctorSessionService, DoctorSessionService>();

builder.Services.AddHttpClient("CustomerApi", client =>
{
    client.BaseAddress = new Uri("http://api.ayulankamedical.com/");
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:3000", "http://173.212.241.66:8025", "http://oms.ayulankamedical.com"
                , "http://oms-uat.ayulankamedical.com", "http://oms-sigiriya.ayulankamedical.com") // React app's URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush(); // Ensures any buffered events are logged
}
