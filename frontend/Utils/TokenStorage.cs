using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace frontend.Utils
{
    public static class TokenStorage
    {
        private static IConfiguration _configuration;
        private static string _appName;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            _appName = _configuration["Application:Name"] ?? _appName;
        }

        private static string TokenFilePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                _appName,
                "token.dat"
            );

        private static byte[] GetEncryptionKey()
        {
            string key = _configuration["JwtSettings:Secret"];

            if (string.IsNullOrEmpty(key) || key.Length != 32)
            {
                throw new InvalidOperationException(
                    "Invalid encryption key configuration. Key must be 32 bytes long."
                );
            }

            return Encoding.UTF8.GetBytes(key);
        }

        public static void SaveToken(string token)
        {
            try
            {
                if (_configuration == null)
                    throw new InvalidOperationException("TokenStorage not initialized");

                Directory.CreateDirectory(Path.GetDirectoryName(TokenFilePath));
                byte[] key = GetEncryptionKey();

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = new byte[16];

                    using (var encryptor = aes.CreateEncryptor())
                    using (var fileStream = new FileStream(TokenFilePath, FileMode.Create))
                    using (
                        var cryptoStream = new CryptoStream(
                            fileStream,
                            encryptor,
                            CryptoStreamMode.Write
                        )
                    )
                    {
                        byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                        cryptoStream.Write(tokenBytes, 0, tokenBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save token: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        public static string LoadToken()
        {
            try
            {
                if (_configuration == null)
                    throw new InvalidOperationException("TokenStorage not initialized");

                if (!File.Exists(TokenFilePath))
                    return null;

                byte[] key = GetEncryptionKey();

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = new byte[16];

                    using (var fileStream = new FileStream(TokenFilePath, FileMode.Open))
                    using (var decryptor = aes.CreateDecryptor())
                    using (
                        var cryptoStream = new CryptoStream(
                            fileStream,
                            decryptor,
                            CryptoStreamMode.Read
                        )
                    )
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load token: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return null;
            }
        }

        public static void ClearToken()
        {
            try
            {
                if (File.Exists(TokenFilePath))
                {
                    File.Delete(TokenFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to clear token: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
