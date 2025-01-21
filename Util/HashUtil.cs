using System;
using System.Security.Cryptography;
using System.Text;

public class HashUtil
{
	private HashUtil()
	{
		throw new InvalidOperationException("Utility class");
	}

	public static string Hash(string input)
	{
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] encodedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
			return BytesToHex(encodedHash);
		}
	}

	private static string BytesToHex(byte[] hash)
	{
		StringBuilder hexString = new StringBuilder(2 * hash.Length);
		foreach (byte b in hash)
		{
			string hex = b.ToString("x2");
			hexString.Append(hex);
		}
		return hexString.ToString();
	}
}