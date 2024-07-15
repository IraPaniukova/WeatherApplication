using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeatherApplication
{
    public class FileEncoder
    {
        private static readonly Lazy<FileEncoder> lazyInstance = new Lazy<FileEncoder>(() => new FileEncoder("apikeys.sys"));
        private readonly string filePath;
        private readonly byte[] encryptionKey = Convert.FromBase64String("C/+YjsuTzXJzop3TX46d2WATe1qZ/PiNT/mCRxrSw1o=");
        private readonly byte[] encryptionIv;

        private FileEncoder(string filePath)
        {
            this.filePath = filePath;
            string ivFilePath = filePath + ".iv";

            if (File.Exists(ivFilePath))
            {
                encryptionIv = File.ReadAllBytes(ivFilePath);
            }
            else
            {
                using (Aes aes = Aes.Create())
                {
                    aes.GenerateIV();
                    encryptionIv = aes.IV;
                    File.WriteAllBytes(ivFilePath, encryptionIv);
                }
            }

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath)) { }
            }
        }

        public static FileEncoder GetInstance() => lazyInstance.Value;

        //for testing purpose only
        public static FileEncoder GetTestInstance(string filePath) => new FileEncoder(filePath);

        public void Write(string key, string value)
        {
            // Encrypt the key-value pair
            string encryptedPair = EncryptString($"{key}={value}", encryptionKey, encryptionIv);

            // Write to file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(encryptedPair);
            }
        }
        public string Read(string key)
        {
            // Read lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Find the line corresponding to the key
            foreach (string line in lines)
            {
                string decryptedLine = DecryptString(line, encryptionKey, encryptionIv);
                string[] parts = decryptedLine.Split('=');
                if (parts.Length == 2 && parts[0] == key)
                {
                    // Return the decrypted value
                    return parts[1];
                }
            }

            // Key not found
            return "";
        }
        private string EncryptString(string input, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(input);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        private string DecryptString(string input, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(string.IsNullOrWhiteSpace(input) ? throw new Exception("invalid key") : Convert.FromBase64String(input)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        public void Clear()
        {
            // Clear the contents of the file by truncating it
            File.WriteAllText(filePath, string.Empty);
        }
    }
}