using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using iiht_AirMonitoringSystemApi.Data;
using iiht_AirMonitoringSystemApi.Models;
//using Jose;

namespace iiht_AirMonitoringSystemApi.Controllers

{
    // Attribute that identifies this class as a controller in the ASP.NET Core MVC framework
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : ControllerBase
    {
        // Private fields that store the database context and the configuration
        private readonly SensorDbContext _dbContext;
        private readonly IConfiguration _configuration;

        // Constructor that initializes the private fields
        public JwtTokenController(SensorDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // Login action method that returns a JSON Web Token (JWT)
        [HttpPost]
        [Route("/[controller]/Login")]
        public IActionResult Login([FromBody] Users obj)
        {
            // Try to retrieve a user with the provided username and password from the database
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.UserName == obj.UserName && u.Password == obj.Password);

            // If the user doesn't exist, return a NotFound result
            if (currentUser == null)
            {
                return NotFound();
            }

            // Generate a symmetric security key using the JWT key from the configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            // Create signing credentials using the security key and the HMAC SHA-256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a claim that contains the user's email address
            var claims = new[]
            {
                new Claim(ClaimTypes.Email , currentUser.Email)
            };

            // Create a JWT using the provided configuration information (issuer, audience, claims, expiration, signing credentials)
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            // Write the JWT to a string
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the JWT as the response to the request
            return Ok(jwt);
        }

        // Register action method that creates a new user in the database
        [HttpPost]
        [Route("/[controller]/Register")]
        public IActionResult Register([FromBody] Users user)
        {
            // Try to retrieve a user with the same username from the database
            var userExists = _dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);
            // Check if user with same email already exists
            if (userExists != null)
            {
                // Return BadRequest with message indicating the user already exists
                return BadRequest("User with same email already exists");
            }

            // Add the new user to the database context
            _dbContext.Users.Add(user);

            // Save the changes to the database
            _dbContext.SaveChanges();

            // Return 201 Created status code to indicate that the user was successfully created
            return StatusCode(StatusCodes.Status201Created);
        }
        // HttpGet method to retrieve user information based on provided username and password
        [HttpGet]
        public async Task<Users> GetUser(string username, string password)
        {
            // Retrieve the user from database based on provided username and password
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
