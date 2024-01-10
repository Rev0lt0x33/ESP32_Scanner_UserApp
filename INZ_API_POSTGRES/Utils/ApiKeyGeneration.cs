using System.Security.Cryptography;

namespace INZ_API_POSTGRES.Utils
{
    public class ApiKeyGeneration
    {
		public static string GenerateApiKey(int byteSize = 16)
		{
			
			using (RandomNumberGenerator rng =  RandomNumberGenerator.Create() )
			{
				var randomNumber = new byte[byteSize];
				rng.GetBytes(randomNumber);
				return BitConverter.ToString(randomNumber).Replace("-", "").ToLower();
			}
		}
	}
}
