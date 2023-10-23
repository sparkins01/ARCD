using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HdbscanSharp.Distance;
using HdbscanSharp.Runner;

namespace EventToAction
{
    class Cluster
    {
        String filePath;
        public Cluster(string filePath)
        {
            this.filePath = filePath;

        }

		public int[] PerformClustering()
		{
			//var dataset = LoadCsv("iris.csv", 5);
			var dataset = LoadCsvExcludingHeader(filePath);
			var result = HdbscanRunner.Run(new HdbscanParameters<double[]>
			{
				DataSet = dataset,
				MinPoints = 3,
				MinClusterSize = 3,
				DistanceFunction = new CosineSimilarity()
			});
			return result.Labels;
		}
		public double[][] LoadCsvExcludingHeader(string filePath)
		{
			double[][] toReturn = new double[0][];
			if (File.Exists(filePath))
			{
				string[] readText = File.ReadAllLines(filePath);
				toReturn = new double[readText.Length-1][];
				for (int i = 1; i < readText.Length; i++) //NOTE: using index 1 to skip header line in csv
				{
					String[] line = readText[i].Split(',');
					toReturn[i-1] = new double[line.Length];
					for (int j = 0; j < line.Length; j++)
					{
						toReturn[i-1][j] = int.Parse(line[j]);

					}
				}
			}
			return toReturn;
		}

	}
}
