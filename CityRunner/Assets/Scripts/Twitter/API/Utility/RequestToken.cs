
namespace Twitter {

	public class RequestToken {
		
		public delegate void RequestTokenCallback(bool success, RequestTokenResponse response);

		public class RequestTokenResponse {
			
			public string Token { get; set; }
			public string TokenSecret { get; set; }
			
		}
	}
}