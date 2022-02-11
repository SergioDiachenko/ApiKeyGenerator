using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ApiKeyGenerator apiKey = new ApiKeyGenerator();
            Console.WriteLine("Api key not URL Safe {0}", apiKey.GenerateApiKeyNotUrlSafe());
            Console.WriteLine("Api key URL Safe {0}", apiKey.GenerateApiKeyUrlSafe());
            Console.WriteLine("Api key URL Safe Before NET 6 {0}", apiKey.GenerateApiKeyBeforeNet6());
            Console.WriteLine("Api key URL Safe NET 6 {0}", apiKey.GenerateApiKey());
            Console.WriteLine("Api key Sha 256 {0}", apiKey.GenerateSha256("secret"));
        }
    }

    public class ApiKeyGenerator
    {
        //  not being URL-safe
        public string GenerateApiKeyNotUrlSafe()
        {
            using var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[32];
            provider.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public string GenerateApiKeyUrlSafe()
        {
            using var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[32];
            provider.GetBytes(bytes);

            return Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "");
        }

        public string GenerateApiKeyBeforeNet6()
        {
            using var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[32];
            provider.GetBytes(bytes);

            return "CT-" + Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .Substring(0, 33);
        }

        public string GenerateApiKey()
        {
            var bytes = new byte[32];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            return "CT-" + Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .Substring(0, 33);
        }

        public string GenerateSha256(string secret)
        {
            SHA256 sha256Hash = SHA256.Create();

            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(secret));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
