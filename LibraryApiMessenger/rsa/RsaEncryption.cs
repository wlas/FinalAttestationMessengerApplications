using System.Security.Cryptography;

namespace LibraryApiMessenger.rsa
{
	public static class RsaEncryption
	{
		public static RSA GetPublicKey()
		{
			var key = File.ReadAllText(@"..\LibraryApiMessenger\rsa\publickey.pem");
			var rsa = RSA.Create();
			rsa.ImportFromPem(key);
			return rsa;
		}

		public static RSA GetPrivateKey()
		{
			var key = File.ReadAllText(@"..\LibraryApiMessenger\rsa\privatekey.pem");
			var rsa = RSA.Create();
			rsa.ImportFromPem(key);
			return rsa;
		}
	}
}
