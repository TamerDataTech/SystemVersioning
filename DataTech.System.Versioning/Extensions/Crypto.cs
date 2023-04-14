using System.Security.Cryptography;
using System.Text;
using System;

namespace DataTech.System.Versioning.Extensions
{
    public static class Crypto
    { 
            public static string GetSHA1(this string sha1Data)
            {
                SHA1 sHA = new SHA1CryptoServiceProvider();
                byte[] bytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(sha1Data);
                byte[] bytes2 = sHA.ComputeHash(bytes);
                return bytes2.GetHexaDecimal();
            }

            public static string GetHexaDecimal(this byte[] bytes)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int num = bytes.Length;
                for (int i = 0; i <= num - 1; i++)
                {
                    stringBuilder.Append(string.Format("{0,2:x}", bytes[i]).Replace(" ", "0"));
                }

                return stringBuilder.ToString();
            }

            public static string GetMd5Hash(this string input, MD5 md5Hash)
            {
                byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }

            public static bool VerifyMd5Hash(this string hash, MD5 md5Hash, string input)
            {
                string md5Hash2 = input.GetMd5Hash(md5Hash);
                StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
                if (ordinalIgnoreCase.Compare(md5Hash2, hash) == 0)
                {
                    return true;
                }

                return false;
            }

            public static string Encrypt(this string input, string key = "CRYPTO", bool useHashing = true)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] key2;
                if (useHashing)
                {
                    MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                    key2 = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    mD5CryptoServiceProvider.Clear();
                }
                else
                {
                    key2 = Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key2;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
                byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                tripleDESCryptoServiceProvider.Clear();
                return Convert.ToBase64String(array, 0, array.Length);
            }

            public static string Decrypt(this string input, string key = "CRYPTO", bool useHashing = true)
            {
                byte[] array = Convert.FromBase64String(input);
                byte[] key2;
                if (useHashing)
                {
                    MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                    key2 = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    mD5CryptoServiceProvider.Clear();
                }
                else
                {
                    key2 = Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key2;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                tripleDESCryptoServiceProvider.Clear();
                return Encoding.UTF8.GetString(bytes);
            } 
    }
}
