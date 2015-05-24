using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Linq;

public class ReadCSV : MonoBehaviour 
{
	public string[] getSingleDimCSVData
	(
		string path
	)
	{
		string[] dataRaw = File.ReadAllLines (path);

		string[] data = new string[dataRaw.Length-1];

		for (int row = 1; row < dataRaw.Length; row++) 
		{
			data[row-1] = dataRaw[row];
		}

		return data;
	}

	public string[,] getMultiDimCSVData
	(
		string path
	)
	{
		string[] dataRaw = File.ReadAllLines (path);

		string[] dataSingleDim = new string[dataRaw.Length-1];

		for (int row = 1; row < dataRaw.Length; row++) 
		{
			dataSingleDim[row-1] = dataRaw[row];
		}
		
		int noOfCols = dataSingleDim[0].Count(f => f == ',') + 1;

		string[,] data = new string[dataSingleDim.Length,noOfCols];

		for (int row = 0; row < dataSingleDim.Length; row++) 
		{
			for( int col = 0; col < noOfCols; col++)
			{
				data[row,col] = dataSingleDim[row].Split (',')[col];
			}
		}

		return data;
	}
}
