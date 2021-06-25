using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZulfieP.Models;
using ZulfieP.Models.Entities;

namespace ZulfieP.Services
{
    public class Auth
    {
		readonly HttpContext _httpContext;
		readonly Cryptography _cryptography;
		readonly AuthOptions _authConfiguration;
		readonly SalariesContext _context;

		public Auth(
			IHttpContextAccessor contextAccessor,
			Cryptography cryptography,
			IOptions<AuthOptions> authConfiguration,
			SalariesContext context)
		{
			_httpContext = contextAccessor.HttpContext;
			_cryptography = cryptography;
			_authConfiguration = authConfiguration.Value;
			_context = context;
		}

		private AuthInfo _scopeAuthInfo = null;
		public AuthInfo ScopeAuthInfo
		{
			get
			{
				if (_scopeAuthInfo == null)
				{
					AuthInfo tokenAuthInfo = null;
					var cookieValue = _httpContext.Request.Cookies["Auth"];
					if (!string.IsNullOrEmpty(cookieValue))
					{
						try
						{
							tokenAuthInfo = AuthInfoFromToken(cookieValue);
						}
						catch
						{
						}
					}
					_scopeAuthInfo = tokenAuthInfo != null ? tokenAuthInfo : new AuthInfo();

				}
				return _scopeAuthInfo;
			}
		}

		public bool SignIn(string username, string password)
		{
			var user = _context.Users.Where(s => s.Username == username).First();
			var passwords = _context.Passwords.Where(s => s.Id == user.PasswordId).First(); 
			
			if (user == null) return false;

			
			var claimedPasswordHashed = _cryptography.HashSHA256(password + passwords.PasswordSalt);

			if (claimedPasswordHashed != passwords.HashedPassword) return false;

			AuthInfo authInfo = new AuthInfo()
			{
				UserId = user.Id,
				UserName = user.Username
			};

			_httpContext.Response.Cookies.Append("Auth", AuthInfoToToken(authInfo));
			return true;
		}

		public void SignOut()
		{
			_httpContext.Response.Cookies.Delete("Auth");
		}

		private string AuthInfoToToken(AuthInfo authInfo)
		{
			var serializedAuthInfo = JsonConvert.SerializeObject(authInfo);

			// Encrypt serialized authInfo
			var key = Encoding.UTF8.GetBytes(_authConfiguration.AuthKey);
			var iv = Aes.Create().IV;
			var ivBase64 = Convert.ToBase64String(iv);
			var encBytes = _cryptography.EncryptStringToBytes_Aes(serializedAuthInfo, key, iv);
			var result = $"{ivBase64.Length.ToString().PadLeft(3, '0')}{ivBase64}{Convert.ToBase64String(encBytes)}";
			return result;
		}

		private AuthInfo AuthInfoFromToken(string token)
		{
			// Decrypt token
			string decryptedToken;
			var ivLength = Convert.ToInt32(token.Substring(0, 3));
			var ivBase64 = token.Substring(3, ivLength);
			var iv = Convert.FromBase64String(ivBase64);
			var encBase64 = token.Substring(ivLength + 3);
			var encBytes = Convert.FromBase64String(encBase64);
			var key = Encoding.UTF8.GetBytes(_authConfiguration.AuthKey);
			decryptedToken = _cryptography.DecryptStringFromBytes_Aes(encBytes, key, iv);

			// Deserialize decrypted token
			var result = JsonConvert.DeserializeObject<AuthInfo>(decryptedToken);
			return result;
		}
	}
}
