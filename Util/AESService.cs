using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


	public class AESService
	{
		private const string AesAlgorithm = "AES/CBC/PKCS7Padding"; // PKCS7 is equivalent to PKCS5 in C#
		private const int IvSize = 16;

		public static string Encrypt(string plainText, string hexKey)
		{
			try
			{
				byte[] keyBytes = HexStringToByteArray(hexKey);
				byte[] iv = new byte[IvSize];
				using (var rng = new RNGCryptoServiceProvider())
				{
					rng.GetBytes(iv);
				}

				using (var aes = Aes.Create())
				{
					aes.Key = keyBytes;
					aes.IV = iv;
					aes.Mode = CipherMode.CBC;
					aes.Padding = PaddingMode.PKCS7;

					using (var encryptor = aes.CreateEncryptor())
					{
						byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
						byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

						byte[] ivAndEncrypted = new byte[iv.Length + encryptedBytes.Length];
						Array.Copy(iv, 0, ivAndEncrypted, 0, iv.Length);
						Array.Copy(encryptedBytes, 0, ivAndEncrypted, iv.Length, encryptedBytes.Length);

						return Convert.ToBase64String(ivAndEncrypted);
					}
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error during encryption: {ex.Message}");
				return plainText;
			}
		}

		public static string Decrypt(string encryptedText, string hexKey)
		{
			try
			{
				byte[] combined = Convert.FromBase64String(encryptedText);
				byte[] iv = new byte[IvSize];
				byte[] cipherText = new byte[combined.Length - IvSize];

				Array.Copy(combined, 0, iv, 0, IvSize);
				Array.Copy(combined, IvSize, cipherText, 0, cipherText.Length);

				byte[] keyBytes = HexStringToByteArray(hexKey);

				using (var aes = Aes.Create())
				{
					aes.Key = keyBytes;
					aes.IV = iv;
					aes.Mode = CipherMode.CBC;
					aes.Padding = PaddingMode.PKCS7;

					using (var decryptor = aes.CreateDecryptor())
					{
						byte[] plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
						return Encoding.UTF8.GetString(plainBytes);
					}
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Error during decryption: {ex.Message}");
				return encryptedText;
			}
		}

		private static byte[] HexStringToByteArray(string hex)
		{
			int length = hex.Length;
			byte[] bytes = new byte[length / 2];
			for (int i = 0; i < length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}
	}
