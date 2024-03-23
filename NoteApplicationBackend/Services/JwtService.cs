using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoteApplicationBackend.Configurations;
using NoteApplicationBackend.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NoteApplicationBackend.Services
{
    public class JwtService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public JwtService(UserManager<IdentityUser> userManager,
                    IOptionsMonitor<JwtConfiguration> optionsMonitor,
                    RoleManager<IdentityRole> roleManager)
    {
                _roleManager = roleManager;
                _userManager = userManager;
                _jwtConfig = optionsMonitor.CurrentValue;
    }

    

    public async Task<AuthResult> GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            //get user claims
            var claims = await GetAllValidClaims(user);
        

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(4),
                NotBefore = DateTime.UtcNow,    
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256) 
            };
            

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return new AuthResult(){
                Token = jwtToken
            };     
        }

        //get all valid claims for the user

        public bool IsValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Get token validattion params
            var validationParameters = GetValidationParameters();

            try
            {
                // Validate Token
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception )
            {
                // Validate error
                return false;
            }
        }


        private TokenValidationParameters GetValidationParameters()
        {   
            
            // Generate validate params 
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                
            }

            //getting the claims that the we have assigned to the user   
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user); 

            //get the role and add it to the userrole claims
            
            
            foreach(var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);

                if(role != null)        
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach(var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            claims.AddRange(userClaims);
            return claims;

        }
}



}