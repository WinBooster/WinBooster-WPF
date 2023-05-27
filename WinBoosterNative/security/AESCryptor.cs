using System.Security.Cryptography;
using System.Text;

namespace WinBoosterNative.security
{
    public class AESCryptor
    {
        private static byte[] GenerateAESKey(string password, byte[] salt, int keySize)
        {
            int KeySize = keySize; // AES key size in bits
            int Iterations = 10000; // number of iterations for PBKDF2

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return pbkdf2.GetBytes(KeySize / 8);
            }
        }

        private static byte[] GenerateAESIV(string password, byte[] salt)
        {
            const int IVSize = 128; // AES IV size in bits
            const int Iterations = 10000; // number of iterations for PBKDF2

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return pbkdf2.GetBytes(IVSize / 8);
            }
        }
        private byte[] key = new byte[256];
        private byte[] iv = new byte[256];
        private int keySize = 128;
        public enum KeySize
        {
            Short = 128,
            Default = 256
        }
        public void SetPassword(string password, string salt, KeySize keySize = KeySize.Default)
        {
            this.keySize = (int)keySize;
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            key = GenerateAESKey(password, saltBytes, (int)keySize);
            iv = GenerateAESIV(password, saltBytes);
        }
        
        public byte[] Encrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }
    }
}
