using System;
using System.Security.Cryptography;
using System.Text;


	public class RSAService
	{
		private const string KeyAlgorithm = "RSA";
		private const string SecurePadding = "RSA/ECB/PKCS1Padding";

		public string EncryptWithPublicKey(string plainText, string publicKeyContent)
		{
			try
			{
				var publicKeyBytes = Convert.FromBase64String(publicKeyContent);
				using var rsa = RSA.Create();
				rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

				var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
				return Convert.ToBase64String(encryptedBytes);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during encryption: {ex.Message}");
				return plainText;
			}
		}

		public string DecryptWithPrivateKey(string encodedText, string privateKeyContent)
		{
			try
			{
				var privateKeyBytes = Convert.FromBase64String(privateKeyContent);
				using var rsa = RSA.Create();
				rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

				var decryptedBytes = rsa.Decrypt(Convert.FromBase64String(encodedText), RSAEncryptionPadding.Pkcs1);
				return Encoding.UTF8.GetString(decryptedBytes);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during decryption: {ex.Message}");
				return encodedText;
			}
		}

		public string SignWithPrivateKey(string plainText, string privateKeyContent)
		{
			try
			{
				var privateKeyBytes = Convert.FromBase64String(privateKeyContent);
				using var rsa = RSA.Create();
				rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

				var dataBytes = Encoding.UTF8.GetBytes(plainText);
				var signedBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
				return Convert.ToBase64String(signedBytes);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during signing: {ex.Message}");
				return null;
			}
		}

		public bool VerifySignatureWithPublicKey(string plainText, string encodedText, string publicKeyContent)
		{
			try
			{
				var publicKeyBytes = Convert.FromBase64String(publicKeyContent);
				using var rsa = RSA.Create();
				rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

				var dataBytes = Encoding.UTF8.GetBytes(plainText);
				var signatureBytes = Convert.FromBase64String(encodedText);
				return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Signature Verification Failed: {ex.Message}");
				return false;
			}
		}
	}
