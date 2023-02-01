using Microsoft.ML;
using System;
using System.Collections.Generic;
using static Microsoft.ML.DataOperationsCatalog;
using static Scar.Structs;

namespace Scar
{
	class MachineLearning
	{
		private static readonly MLContext mlContext = new();
		private static PredictionEngine<AudioFeatureElement, SongBinaryPrediction> predEngine;

		internal static void Train(List<AudioFeatureElement> audioFeatures)
		{
			IDataView dataView = mlContext.Data.LoadFromEnumerable(audioFeatures);
			TrainTestData trainTestSplit = mlContext.Data.TrainTestSplit(dataView);

			var pipeline = mlContext.Transforms.Concatenate
			(
				"Features",
				nameof(AudioFeatureElement.Danceability),
				nameof(AudioFeatureElement.Energy),
				nameof(AudioFeatureElement.Key),
				nameof(AudioFeatureElement.Loudness),
				nameof(AudioFeatureElement.Mode),
				nameof(AudioFeatureElement.Speechiness),
				nameof(AudioFeatureElement.Acousticness),
				nameof(AudioFeatureElement.Instrumentalness),
				nameof(AudioFeatureElement.Liveness),
				nameof(AudioFeatureElement.Valence),
				nameof(AudioFeatureElement.Tempo),
				nameof(AudioFeatureElement.TimeSignature)
			)
			.Append(mlContext.BinaryClassification.Trainers.FastTree());

			ITransformer trainedModel = pipeline.Fit(trainTestSplit.TrainSet);

			IDataView predictions = trainedModel.Transform(trainTestSplit.TestSet);
			var metrics = mlContext.BinaryClassification.Evaluate(data: predictions);
			Console.WriteLine($"Accuracy : {metrics.Accuracy}");
			Console.WriteLine($"F1Score : {metrics.F1Score}");

			predEngine = mlContext.Model.CreatePredictionEngine<AudioFeatureElement, SongBinaryPrediction>(trainedModel);
		}

		internal static void Predict(AudioFeatureElement trackToCheck)
		{
			SongBinaryPrediction resultPrediction = predEngine.Predict(trackToCheck);

			Console.WriteLine($"=============== Single Prediction  ===============");
			Console.WriteLine($"Prediction: {(Convert.ToBoolean(resultPrediction.LikingPrediction) ? "will like" : "won't like")} song");
			Console.WriteLine($"Probability of being liked: {resultPrediction.Probability}");
			Console.WriteLine($"Score of being liked: {resultPrediction.Score}");
			Console.WriteLine($"================End of Process.==================================");
		}
	}
}
