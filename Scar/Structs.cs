using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Scar
{
	class Structs
	{
		public partial class Playlist
		{
			[JsonPropertyName("items")]
			public List<Item> Items { get; set; }
		}

		public partial class Item
		{
			[JsonPropertyName("track")]
			public Track Track { get; set; }
		}

		public partial class Track
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("name")]
			public string Name { get; set; }
		}

		public partial class Secrets
		{
			[JsonPropertyName("clientId")]
			public string ClientId { get; set; }

			[JsonPropertyName("clientSecret")]
			public string ClientSecret { get; set; }
		}

		public partial class AudioAnalysis
		{
			[JsonPropertyName("audio_features")]
			public List<AudioFeatureElement> AudioFeatures { get; set; }
		}

		public partial class AudioFeatureElement
		{
			[JsonPropertyName("danceability")]
			public float Danceability { get; set; }

			[JsonPropertyName("energy")]
			public float Energy { get; set; }

			[JsonPropertyName("key")]
			public float Key { get; set; }

			[JsonPropertyName("loudness")]
			public float Loudness { get; set; }

			[JsonPropertyName("mode")]
			public float Mode { get; set; }

			[JsonPropertyName("speechiness")]
			public float Speechiness { get; set; }

			[JsonPropertyName("acousticness")]
			public float Acousticness { get; set; }

			[JsonPropertyName("instrumentalness")]
			public float Instrumentalness { get; set; }

			[JsonPropertyName("liveness")]
			public float Liveness { get; set; }

			[JsonPropertyName("valence")]
			public float Valence { get; set; }

			[JsonPropertyName("tempo")]
			public float Tempo { get; set; }

			/*
			[JsonPropertyName("duration_ms")]
			public float DurationMs { get; set; }
			*/

			[JsonPropertyName("time_signature")]
			public float TimeSignature { get; set; }

			[ColumnName("Label")]
			public bool IsLiked { get; set; }
		}

		public class SongBinaryPrediction
		{
			[ColumnName("PredictedLabel")]
			public bool LikingPrediction { get; set; }
			public float Probability { get; set; }
			public float Score { get; set; }
		}
	}
}