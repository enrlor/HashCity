using System.Collections.Generic;

namespace Twitter {

	public class AccessToken {
		
		public delegate void AccessTokenCallback(bool success, AccessTokenResponse response);

		public class AccessTokenResponse {
			
			public string Token { get; set; }
			public string TokenSecret { get; set; }
			public string UserId { get; set; }
			public string ScreenName { get; set; }
			
		}

		public static string GetHeaderWithAccessToken(string httpRequestType, string apiURL, 
		                                              AccessTokenResponse response, Dictionary<string, string> parameters) {

			OauthConnection.AddDefaultOAuthParams(parameters);
			
			parameters.Add("oauth_token", response.Token);
			parameters.Add("oauth_token_secret", response.TokenSecret);
			
			return OauthConnection.GetFinalOAuthHeader(httpRequestType, apiURL, parameters);
		
		}

	}

}