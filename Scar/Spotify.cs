using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Scar.Structs;

namespace Scar
{

	class Spotify
	{
		private const string loginUrl = "https://accounts.spotify.com/api/token";
		private const string tracksFeatureUrl = "https://api.spotify.com/v1/audio-features?ids=";
		private const string playlistTracksUrl = "https://api.spotify.com/v1/playlists/{0}/tracks?fields=items(track(name,id))";

		private readonly Secrets secrets = JsonSerializer.Deserialize<Secrets>(File.ReadAllText("secrets.json"));

		private string token;

		public void Login()
		{
			try
			{
				UTF8Encoding encoding = new();
				byte[] postData = encoding.GetBytes("grant_type=client_credentials");
				byte[] authData = encoding.GetBytes(secrets.ClientId + ":" + secrets.ClientSecret);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginUrl);
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(authData));
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = postData.Length;
				request.Method = "POST";

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(postData, 0, postData.Length);
				}

				Task<WebResponse> response = request.GetResponseAsync();
				Spinner.Show(response);
				HttpWebResponse loginResponse = (HttpWebResponse)response.Result;

				Dictionary<string, JsonElement> loginResponseParsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
					new StreamReader(loginResponse.GetResponseStream()).ReadToEnd()
				);

				loginResponseParsed.TryGetValue("access_token", out JsonElement jsonToken);
				token = jsonToken.GetString();
			}
			catch (WebException wex)
			{
				Console.WriteLine(new StreamReader(wex.Response.GetResponseStream()).ReadToEnd());
			}
			catch (AggregateException aex)
			{
				foreach (Exception ex in aex.InnerExceptions)
				{
					switch (ex)
					{
						case WebException:
							Console.WriteLine(new StreamReader(((WebException)ex).Response.GetResponseStream()).ReadToEnd());
							break;
						default:
							Console.WriteLine(ex.Message);
							break;
					}
				}
			}
		}

		public AudioAnalysis GetTracksFeatures(HashSet<string> ids) 
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tracksFeatureUrl + string.Join(",", ids));
			request.Headers.Add("Authorization", "Bearer " + token);
			request.Method = "GET";
			request.ContentType = "application/json";

			Task<WebResponse> response = request.GetResponseAsync();
			Spinner.Show(response);
			HttpWebResponse trackFeatureResponse = (HttpWebResponse)response.Result;

			return JsonSerializer.Deserialize<AudioAnalysis>(
				new StreamReader(trackFeatureResponse.GetResponseStream()).ReadToEnd()
			);
		}

		public AudioAnalysis GetTrackFeatures(string id) 
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tracksFeatureUrl + id);
			request.Headers.Add("Authorization", "Bearer " + token);
			request.Method = "GET";
			request.ContentType = "application/json";

			Task<WebResponse> response = request.GetResponseAsync();
			Spinner.Show(response);
			HttpWebResponse trackFeatureResponse = (HttpWebResponse)response.Result;

			return JsonSerializer.Deserialize<AudioAnalysis>(
				new StreamReader(trackFeatureResponse.GetResponseStream()).ReadToEnd()
			);
		}

		public Playlist GetPlaylistInfo(string playlistId) 
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(playlistTracksUrl, playlistId));
			request.Headers.Add("Authorization", "Bearer " + token);
			request.Method = "GET";
			request.ContentType = "application/json";

			Task<WebResponse> response = request.GetResponseAsync();
			Spinner.Show(response);
			HttpWebResponse trackFeatureResponse = (HttpWebResponse)response.Result;

			return JsonSerializer.Deserialize<Playlist>(
				new StreamReader(trackFeatureResponse.GetResponseStream()).ReadToEnd()
			);
		}
	}
}