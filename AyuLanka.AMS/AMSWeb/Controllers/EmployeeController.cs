// EmployeeController.cs
using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AyuLanka.AMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees([FromQuery] int? companyId = null)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(companyId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeRequestModel employeeRequestModel)
        {
            var createdEmployee = await _employeeService.AddEmployeeAsync(employeeRequestModel);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, EmployeeRequestModel employeeRequestModel)
        {
            if (id != employeeRequestModel.Id)
            {
                return BadRequest();
            }

            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, employeeRequestModel);
            if (updatedEmployee == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/reset-password")]
        public async Task<ActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.NewPassword))
                return BadRequest("New password is required.");
            try
            {
                await _employeeService.ResetPasswordAsync(id, request.NewPassword);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequestModel loginRequest)
        {
            // Authenticate the user using the employee service
            var user = await _employeeService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(); // or return BadRequest("Invalid credentials"); based on your preference
            }

            // Assuming GenerateJwtToken is a method that generates a JWT token for the authenticated user
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, User = user });
        }

        private string GenerateJwtToken(Employee user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Decoding the key correctly from Base64 to ensure it meets the required length
            var keyBytes = Convert.FromBase64String("QXq1F1oOPo3rMgIqu17aDLkC2ieT3ZcBHx5Ux2SQFgA=");
            var key = new SymmetricSecurityKey(keyBytes);

            // Define the claims to include in the token
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("fullName", user.FullName),
                new Claim("callingName", user.CallingName),
                new Claim("employeeNumber", user.EmployeeNumber),
                new Claim("designationCode", user.Designation.DesignationCode.ToString()),
                new Claim("companyId", user.CompanyId.ToString()),
                new Claim("companyCode", user.Company?.CompanyCode ?? "")
            };

            // Create the JWT token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2), // Token expiration time
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the serialized token
            return tokenHandler.WriteToken(token);
        }


    }
}
