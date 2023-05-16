using System;
using Models;
namespace ITokenGenerate
{
    public interface IGenerateToken
    {
        public string GenerateToken(UserLogin user);
    }
}