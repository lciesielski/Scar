using System;
using System.Linq;
using static Scar.Structs;

namespace Scar
{
	class Program
	{
		static void Main(string[] args)
		{
			string teenSpirit = "4CeeEOM32jQcH3eN9Q2dGj";
			string getLucky = "69kOkLUCkxIZYexIgSG8rq";
			string playlistId = "3KyhJgWHFeHOKgsRq6Q3d6";

			Spotify spotifyHelper = new();
			spotifyHelper.Login();
			Playlist playlist = spotifyHelper.GetPlaylistInfo(playlistId);
			AudioAnalysis tracksAudio = spotifyHelper.GetTrackFeatures(playlist.Items.Select(x => x.Track.Id).ToHashSet());

			Console.ReadKey();
		}
	}
}