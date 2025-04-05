using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace frontend.Utils
{
    public class TokenStorage
    {
        private readonly IConfiguration _configuration;
        private readonly string _appName;

        public TokenStorage(IConfiguration configuration)
        {
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
            _appName = _configuration["Application:Name"] ?? "DefaultAppName";
        }

        private string TokenFilePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                _appName,
                "token.dat"
            );

        private byte[] GetEncryptionKey()
        {
            string key = _configuration["JwtSettings:Secret"]!;

            if (string.IsNullOrEmpty(key) || key.Length != 32)
            {
                throw new InvalidOperationException(
                    $"Invalid key length: {key.Length}. Key must be 32 bytes long."
                );
            }

            return Encoding.UTF8.GetBytes(key);
        }

        public void SaveToken(string token)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(TokenFilePath)!);
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

        public string LoadToken()
        {
            try
            {
                if (!File.Exists(TokenFilePath))
                    return string.Empty;

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
                return string.Empty;
            }
        }

        public void ClearToken()
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
