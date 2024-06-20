using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    public static class Encryption
    {

        private static readonly string _MD5Key = "";


        public static string MD5_Encryption(string Clear_Text)
        {
            string Encrypted_Text = "";

            try
            {
                using (var md5 = MD5.Create())
                {
                    var data = md5.ComputeHash(Encoding.UTF8.GetBytes(Clear_Text));
                    var sb = new StringBuilder();
                    foreach (var c in data)
                    {
                        sb.Append(c.ToString("x2"));
                    }
                    Encrypted_Text = sb.ToString();
                }

                return Encrypted_Text;
            }
            catch
            {
                return null;
            }
        }


        public static string AIS_Encryption(string Clear_Text, string Key = "")
        {
            string Encrypted_Text = "";
            string MD5Key = _MD5Key;
            if (Key == "") { Key = MD5_Encryption(MD5Key); }

            byte[] iv = new byte[16];
            byte[] array;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(Clear_Text);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                Encrypted_Text = Convert.ToBase64String(array);

                return Encrypted_Text;
            }
            catch
            {
                return null;
            }

        }


        public static string AIS_Decryption(string Encrypted_Text, string Key = "")
        {
            string Clear_Text = "";
            string MD5Key = _MD5Key;
            if (Key == "") { Key = MD5_Encryption(MD5Key); }

            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(Encrypted_Text);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                try { Clear_Text = streamReader.ReadToEnd(); } catch { return null; }
                            }
                        }
                    }
                }

                return Clear_Text;
            }
            catch
            {
                return null;
            }
        }

        public static string Encrypt(string value, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(string encryptedValue, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedValue);
                string decryptedValue;
                using (var msDecrypt = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            decryptedValue = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return decryptedValue;
            }
        }

    }
}

