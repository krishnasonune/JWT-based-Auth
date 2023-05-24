using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ITokenGenerate;
using Models;

namespace rep
{
    public class Repository : IGenerateToken
    {
        static UserLogin userLogin = new UserLogin();

        private IConfiguration configuration;

        public Repository(IConfiguration _config)
        {
            configuration = _config;
        }

        public bool Authenticate(UserLogin user){
            userLogin.Email = "krishna@mail.com";
            userLogin.Password = "1234";

            return user.Email == userLogin.Email && user.Password == userLogin.Password ? true: false;
        }

        public string GenerateToken(UserLogin user){
            var skey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(skey, SecurityAlgorithms.HmacSha256);

            var claim = new[]{
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, "Krishna"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"])
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public List<Movies> getMovies(){
            Movies m1 = new Movies();
            Movies m2 = new Movies();
            Movies m3 = new Movies();

            m1.Id = 1000;
            m1.MovieName = "Iron Man";
            m1.MovieDescription = "Iron Man is a fictional superhero who wears a suit of armor. His alter ego is Tony Stark. He was created by Stan Lee, Jack Kirby and Larry Lieber for Marvel Comics in Tales of Suspense #39 in the year 1963 and appears in their comic books. He is also one of the main protagonists in the Marvel Cinematic Universe.";
            m1.Ratings = 5;

            m2.Id = 1001;
            m2.MovieName = "Captain America: The First Avenger";
            m2.MovieDescription = "Captain America is the alter ego of Steve Rogers, a frail young artist enhanced to the peak of human perfection by an experimental super-soldier serum after joining the military to aid the United States government's efforts in World War II.";
            m2.Ratings = 4;

            m3.Id = 1002;
            m3.MovieName = "Thor";
            m3.MovieDescription = "Falling in love with scientist Jane Foster (Natalie Portman) teaches Thor much-needed lessons, and his new-found strength comes into play as a villain from his homeland sends dark forces toward Earth. Two worlds. One hero.";
            m3.Ratings = 5;

            return new List<Movies>{m1, m2, m3};
        }

        public string[] DecodeJwtToken(string token){
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            var name = tokenS.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var email = tokenS.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var Iat = tokenS.Claims.First(x => x.Type == JwtRegisteredClaimNames.Iat).Value;
            var subject = tokenS.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            return new string[]{name, email, Iat, subject};
        }
    }
}