using System;
using System.Collections.Generic;
using System.Linq;
using static Scar.Structs;

namespace Scar
{
	class Program
	{

		static void Main(string[] args)
		{
			List<AudioFeatureElement> tracksAudioFeatures = new();
			string positivePlaylistId = "3KyhJgWHFeHOKgsRq6Q3d6";
			string negativePlaylistId = "7MYmBd7PoT0gIvDzJtlbOx";

			Spotify spotifyHelper = new();
			spotifyHelper.Login();

			Playlist positivePlaylist = spotifyHelper.GetPlaylistInfo(positivePlaylistId);
			foreach (AudioFeatureElement item in spotifyHelper.GetTracksFeatures(positivePlaylist.Items.Select(x => x.Track.Id).ToHashSet()).AudioFeatures)
			{
				item.IsLiked = true;
				tracksAudioFeatures.Add(item);
			}

			Playlist negativePlaylist = spotifyHelper.GetPlaylistInfo(negativePlaylistId);
			foreach (AudioFeatureElement item in spotifyHelper.GetTracksFeatures(negativePlaylist.Items.Select(x => x.Track.Id).ToHashSet()).AudioFeatures)
			{
				item.IsLiked = false;
				tracksAudioFeatures.Add(item);
			}

			MachineLearning.Train(tracksAudioFeatures);

			string trackId = string.Empty;
			do
			{
				Console.WriteLine("[+] Gimme Track ID");
				trackId = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(trackId))
				{
					MachineLearning.Predict(spotifyHelper.GetTrackFeatures(trackId).AudioFeatures[0]);
				}
				else
				{
					Console.WriteLine("[+] Bye");
				}
			} while (!string.IsNullOrWhiteSpace(trackId));
		}
	}
}