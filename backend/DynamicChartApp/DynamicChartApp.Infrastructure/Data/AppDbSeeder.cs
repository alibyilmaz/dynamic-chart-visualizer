using System.Security.Cryptography;
using System.Text;
using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Data;

namespace DynamicChartApp.Infrastructure;

public static class AppDbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var user = new User
            {
                Username = "testuser",
                PasswordHash = ComputeSha256Hash("test123")
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    private static string ComputeSha256Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var b in bytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }


}
