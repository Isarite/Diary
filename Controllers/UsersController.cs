using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DiaryApp.Models;
using DiaryApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DiaryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: api/Users
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReturn>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var returnList = new List<UserReturn>();
            foreach (var user in users)
            {
                returnList.Add(
                    new UserReturn
                    {
                        Role = (await _userManager.GetRolesAsync(user)).First(),
                        Id = user.Id,
                        Name = user.NormalizedUserName
                    });
            }

            return returnList;
        }

        // GET: api/Users/5
        [Authorize(Roles = "Administrator,User")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var role = (await _userManager.GetRolesAsync(user)).First();
            return Ok(
                new UserReturn
                {
                    Id = user.Id,
                    Name = user.NormalizedUserName,
                    Role = role
                });
        }

        // Put: api/Users/user
        [HttpPut]
        [Route("[action]/{username}")]
        public async Task<ActionResult<string>> RequestToken([FromBody] string password , string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (! await _userManager.CheckPasswordAsync(user,password))
            {
                return NotFound();
            }

            var roles =  await _userManager.GetRolesAsync(user);

            var key = Encoding.ASCII.GetBytes
                     ("lyTfdO0pRUuvjCD7ICfDQ5LBnkcfWL2luGG_HUjvuwubg9_D7qwJCmAJ2Fo3i30hHS4unlSr0Dk8Unm0MEikvqNvJVEsrqQuLvrGRWWHUJts5zyZlp1WAxUOocuf5gWbRlrHfgsi09rZqRcdbtGnNkdQttKrZ26i0vdJuF6npw3lLCWvwi4FRiVkBZYzybyHQ5nLa5xy5xFpTCrubs-GEKN5ErJQr44sUy0JAg2A03OHImx9iKOcRF_02cNNLcMWCgeGu0jSVfi5JonP1fw4bkjYsoNq-FnjJM2A-WgtyVeDlESft1HbfKtpNQKcHi6JUjQ1nnQ7lNAZ_c-blPB7BA");
            //Generate Token for user 
            var JWToken = new JwtSecurityToken(
                issuer: "http://localhost:5000/",
                audience: "http://localhost:5000/",
                claims:  new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, roles[0])//TODO set roles
            },
        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                //Using HS256 Algorithm to encrypt Token
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            await _userManager.SetAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "token", token);
            //TODO token creation
            return new JsonResult (new { token = token });
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId); 
            var roles = await _userManager.GetRolesAsync(user);

            var key = Encoding.ASCII.GetBytes
                     ("lyTfdO0pRUuvjCD7ICfDQ5LBnkcfWL2luGG_HUjvuwubg9_D7qwJCmAJ2Fo3i30hHS4unlSr0Dk8Unm0MEikvqNvJVEsrqQuLvrGRWWHUJts5zyZlp1WAxUOocuf5gWbRlrHfgsi09rZqRcdbtGnNkdQttKrZ26i0vdJuF6npw3lLCWvwi4FRiVkBZYzybyHQ5nLa5xy5xFpTCrubs-GEKN5ErJQr44sUy0JAg2A03OHImx9iKOcRF_02cNNLcMWCgeGu0jSVfi5JonP1fw4bkjYsoNq-FnjJM2A-WgtyVeDlESft1HbfKtpNQKcHi6JUjQ1nnQ7lNAZ_c-blPB7BA");
            //Generate Token for user
            var jwToken = new JwtSecurityToken(
                issuer: "http://localhost:5000/",
                audience: "http://localhost:5000/",
                new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, roles[0])//TODO set roles
            },
        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                //Using HS256 Algorithm to encrypt Token
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwToken);
            await _userManager.SetAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "token", token);
            //TODO token creation
            return new JsonResult(new {token});
        }



        // PUT: api/Markings/5
        [Authorize(Roles = "Administrator,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost("{username}")]
        public async Task<ActionResult<UserReturn>> PostUser(string username, [FromBody] string password)
        {
            if ( (await _userManager.FindByNameAsync(username)) != null)
                return Conflict();
            await _userManager.CreateAsync(new User { UserName = username });
            var user = await _userManager.FindByNameAsync(username);
            var res = await _userManager.AddPasswordAsync(user, password);

            //String hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, password);
            //UserStore<User> store = new UserStore<User>(_context);
            //await store.SetPasswordHashAsync(user, hashedNewPassword);

            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");


            return CreatedAtAction("GetUser", new UserReturn{ Id = user.Id ,Name = user.NormalizedUserName, Role = "User"});
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
