using System.Security.Cryptography;

namespace GitCloneApp.Services
{
    public static class Utils
    {
        public static string ComputeHash(string fileContent)
        {
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fileContent));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}