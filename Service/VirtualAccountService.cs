using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VirtualAccountSystemBackend.DTO;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace VirtualAccountSystemBackend.Service
{
    public class VirtualAccountService
    {
		public async Task<string> GenerateSHAEncription() {

            RSAService rSAService = new RSAService();
            AESService aess = new AESService();

            var getSHAString = HashUtil.Hash("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAj4592HNylZCOYltlrXjeJweBsauyJCp2lNDZKl/qzJn3T3MuYbrKVqmfgQj8Js24KGhV8Tnaqi3B/x7ccU2xfvdGgI56PShWHkcsjefpcu90/zYc+OB8iRPT23YgJvmKpkXfUJ93Wdu+blHZBx/iJ6NFeND8eKuO+5SSbGbE9OdH0ahN2iyQpApfrCxMYvBMDiAuhLchs3g41vZxP12MzKQot2FIcB3tIJOpTzds7XplqmBs1sU0kYrLqTTPjR9LRfecpHvo3YjdjtpA1Kr82BMoN1kgH9Zs5ghx35yrH80CqLanFuafqqDmdihtynneCc7Nkd0kwGyXVBkJ74zEHwIDAQAB");

            var request = new 
            {

                bvn = "1111111111",
                firstName = "John",
                lastName = "Doe",
                middleName = "Joe",
                accountName = "John Doe Joe",
                email = "john@doe.com",
                phone = "2347011111111",
                productType = "XYZ",
                customerReference = "2527011111112",
                expireAt = "",
                singleDepositLimit = "1000",
                merchant = new {
                code= "A33E0"
                }
            };
           
            string jsonString = JsonConvert.SerializeObject(request);


            var payloadHash = HashUtil.Hash(jsonString);


            var secureToken = rSAService.EncryptWithPublicKey("92b6841b915d47ef15a01808871e6759", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnwNZHLixzfdi6ZBPQ395QRF1r2A2FqkR/1zPu7cN3Nr95I/vc6UBrI5TbhdFs+r+JGTDIgIWqELyfVZkq6K+Al85kTy4XRojI7+DI8bGKWRvDkgFVKHX0oAodvA0dTXt+ln4I7b4IKEVnqeq4sGh0AeOOq5r/qGnh9vF9ypzMONG8f3cG6+8ZJxmDgklnWoH5Gy/pK0M0kiVStf8KFEpl5IcS7ZkUJog6C7heA3EFzLhWbo4MxNW4OPcnDtzmmVYWdE19LY1Nyzkv1yH0HOP00Br27xDlYC+uB6cXwejy0R8WQpb9J/AR58IEmLICz9DietmoyEncW5anstLbnXmxQIDAQAB");
            
            string data = AESService.Encrypt(jsonString, "92b6841b915d47ef15a01808871e6759");

            string signWith = rSAService.SignWithPrivateKey(payloadHash, "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCPjn3Yc3KVkI5iW2WteN4nB4Gxq7IkKnaU0NkqX+rMmfdPcy5huspWqZ+BCPwmzbgoaFXxOdqqLcH/HtxxTbF+90aAjno9KFYeRyyN5+ly73T/Nhz44HyJE9PbdiAm+YqmRd9Qn3dZ275uUdkHH+Ino0V40Px4q477lJJsZsT050fRqE3aLJCkCl+sLExi8EwOIC6EtyGzeDjW9nE/XYzMpCi3YUhwHe0gk6lPN2ztemWqYGzWxTSRisupNM+NH0tF95yke+jdiN2O2kDUqvzYEyg3WSAf1mzmCHHfnKsfzQKotqcW5p+qoOZ2KG3Ked4Jzs2R3STAbJdUGQnvjMQfAgMBAAECggEAU/plP/P5OelAgeU5i8tDGc3YXtkz8tgsWk36XEGUF3CSLhYRMfaVWzELL+7ToteBPZIbhGv1sMvnypiBGH78sGtzAM1YUOKBzmCQfcsG08ekx48eT7Dy6TWCH8XwdRxRsYb6Zi2DIjvcIKkcroBGSi3G/DA1IVoq4J0/Ms7DtQeellZE4vZz0GyMIKFqFFIdxh0wAhya36NY5ydmeBqlQ5RIxUj3cmPnxypl/KjAwkn+iW655WfLR/Wz8K6icE2JOxh7Yt2Ps7ylJCiAgK65FqKP6IZmnUu9A+HSC+55dzK3LdURzIksOuYfBh3lK6SY4YULDpPHIcHC9DF4XpnjoQKBgQDHGJd8VrUSmeB+/WGqALElUcrjX3QRQrWt2s2xKDZbMLZQqZ4cmV38ibqm0nSuff9vT7FzT8psLX6ptCWbSMzsScebKXrWzNdbv2P+x3YJGeN4VvSyDMhcOoZ7lDKthigSbrrAltnvys0yJMuKeOxXTXYIkhpcwBsDBcW9HxtLlwKBgQC4ljQltTtFGhfzo4y+NCvF0VIWaWhstTPJ78b0eL5Ewi42VC878vyAzTbcN/AgsZ9BYsgPSBEYHwXRArsgq0LvlnOr/J7XbbU6K1v/dMd7yySSdjgEcHGA655y1KU8qgeP0FhYbvZP/FZbF5jGrObhY1LSn6Pd2lurvn2GRDp8uQKBgFlzlhQ6TM6XrkLACxI7j1uqHRL9PwPGSiRbi1EONSXRhAhHvQ8yZQTiyKbNJbmESLC1SI/7iIIECsWqd78F/XQoTfNLtrIthJDnM8Ez0reSFx5pSzV4AslVTjjVw9Ms1HI7p9KBtFdFcd4WpBCJ9f3fgqmlIw0LDtfTzx0CjdvvAoGAOCC0SqMJGsIMdnFM4qlKhiRYKahzsFqJHOZkQQJWGOJS2wJOOpRwY1oDTINV6RsZqfA3lS38xLDqO9vwusZY3DGLisiOJEOfz8hPeSYxYlsrC74W2oK2gNUkoRY4CHJLSle4BWv4SeoY+Cn8sqzwp6hkDtesJFG5fke2M5Wbk0ECgYA4XOWiUW0BweEV12zwMuwlfp3iVlIHGamaL7TuUO5mSADrRVFgjYaoWjEDYzUIcEc5ufvkL+M/ht+0kFympSeB5ouO/rCiNhB2aRlRB28dSm3veVjiqg4y3m036VFFigIQrxle9mLXkKe7FX8iYDOb3wUGtbuvxYU47EpOEJybng==");

            string decrypt = AESService.Decrypt("iqp8B9NuYpNh+3Xi1UMn8eb2PDN5/jah+gsx0vKGyWtZRzKkeqDbxRivRgCvTPUl9kkxHXV9wfMDekIlcXEXwEvVL50zXsiO4/Vun1f3UvXqewkA1K9ivLdBMuE/pggpgqOPEKJYeeAu4ALojdIXduAEt/BYs/qgpjrGjwx+Xf8fPub8eM3r4aq5ZyeG2yIgY0eWz6T/lcNiYS2yUnrkaHZNOi6XLcjWHe3DU6/vZkvR3st8Ew/AFXmAPK9TD1ibBnQimtKj0QLN5php+iHzZTC2ZOSOmmQ1+fOmQ8kzdTOcxCGZNwdC9Zx8v3mzBAoMBmaYzpXzN+hktdOtqrDQNSmRfWdt3S7ZDAhzKMz6eTJMgyUH5owRlS1gY3lRICVP74VR51DA59qA0qK3wsMxBfhRxHcZL0YYDaCT+KY7e9Ng2YoYoAmgfXasRVXw6aUW2erxWTiDui8nQe8BBhcZI1cAfu+XGyQ227DahgxBHs3nwVlP9GAdURYaVtWUfSUPMAumPn/TNKVTLsvxYEBJp8xc/VyJD15aIp1/+UubK2Q=", "92b6841b915d47ef15a01808871e6759");

            var check2 = decrypt;
            var check = data;
            return payloadHash;


			
		}

		
	}
}
