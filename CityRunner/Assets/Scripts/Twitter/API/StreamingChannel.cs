using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.IO;
using System.Net;
using System.Threading;


namespace Twitter {
	public class StreamingChannel: UnityEngine.Object {

		private AccessToken.AccessTokenResponse response;
		private Dictionary<string, string> userSpecifiedParameters;

		private List<string> backgroundLinesList;
		private object listLock = new object();
		private Thread streamReaderThread;



		public StreamingChannel(AccessToken.AccessTokenResponse response, 
		                        Dictionary<string, string> p) {

			this.response = response;
			this.userSpecifiedParameters = p;

			streamReaderThread = new Thread(this.ReadWebStream);
			streamReaderThread.Start();

		}


		public List<string> Read() {

			if (!streamReaderThread.IsAlive) {
				streamReaderThread = new Thread(this.ReadWebStream);
				streamReaderThread.Start();
			}
			
			List<string> lines = null;

			lock (listLock) { 
				if (backgroundLinesList != null) {
					lines = backgroundLinesList;
					backgroundLinesList = null;
				}
			}
			
			return lines;

		}


		private void ReadWebStream() {

			try {

				System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback (this.ReturnTrue);

				HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(TwitterUrls.FILTER_STREAM);
				webRequest.Timeout = -1;

				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add ("track", "a");
				parameters.Add ("stall_warnings", "true");

				if (userSpecifiedParameters != null) {
					
					foreach (KeyValuePair<string, string> elem in userSpecifiedParameters){
						if(parameters.ContainsKey(elem.Key))
							parameters[elem.Key] =  elem.Value;
						else
							parameters.Add(elem.Key, elem.Value);
					}
				}

				string postParameter = Converter.BuildStringParameters (parameters);

				webRequest.Headers.Add("Authorization", AccessToken.GetHeaderWithAccessToken("POST", 
				                                                                             TwitterUrls.FILTER_STREAM,  
				                                                                             response, 
				                                                                             parameters));

				Encoding encode = Encoding.GetEncoding("utf-8");

				if (parameters.Count > 0) {

					webRequest.Method = "POST";
					webRequest.ContentType = "application/x-www-form-urlencoded";
					
					byte[] _twitterTrack = encode.GetBytes(postParameter);
					
					webRequest.ContentLength = _twitterTrack.Length;
					Stream _twitterPost = webRequest.GetRequestStream();
					_twitterPost.Write(_twitterTrack, 0, _twitterTrack.Length);
					_twitterPost.Close();

				}

				HttpWebResponse webResponse = (HttpWebResponse) webRequest.GetResponse();
				StreamReader responseStream = new StreamReader (webResponse.GetResponseStream (), encode);

				while (!responseStream.EndOfStream) {

					var line = responseStream.ReadLine();

					lock (listLock) {
						if (backgroundLinesList == null) {
							backgroundLinesList = new List<string>();
						}
						backgroundLinesList.Add(line);
					}
				}

				Debug.Log("Stream closed");

			}

			catch (Exception e) {
				Debug.Log("WebStream thread failure: " + e + " Stack: " + e.StackTrace);
			}

		}


		
		private bool ReturnTrue(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){
			return true;
		}

	}

}