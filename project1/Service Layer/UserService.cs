using System.Text;
using System.Security.Cryptography;
namespace project1.Services
{
    public class UserService
    {
        // Şifreyi hash'leme (kaydetmeden önce kullanılır)
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);  // Hash'i Base64 formatında döndürüyoruz
        }

        // Giriş yaparken şifre doğrulama işlemi
        public bool VerifyPassword(string enteredPassword, string? storedHash)
        {
            // Null veya boş hash için kontrol
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                Console.WriteLine("Geçersiz PasswordHash (null veya boş).");
                return false;
            }

            // Base64 formatında olup olmadığını kontrol et
            if (!IsBase64String(storedHash))
            {
                Console.WriteLine("StoredHash geçerli bir Base64 formatında değil.");
                return false;
            }

            using var sha256 = SHA256.Create();
            byte[] enteredHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
            string enteredHash = Convert.ToBase64String(enteredHashBytes);

            Console.WriteLine($"Girilen Şifre Hash: {enteredHash}");
            Console.WriteLine($"Veritabanındaki Hash: {storedHash}");

            bool isMatch = enteredHash == storedHash;
            Console.WriteLine($"Şifre doğrulama sonucu: {isMatch}");
            return isMatch;
        }

        // Base64 formatını doğrulayan bir yardımcı fonksiyon
        private bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}


