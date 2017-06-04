using UnityEngine;
using System.Collections;

public class RankData {
	public string 	username;
	public int 		rank;
	public string 	score;

	public RankData(string username, int rank, string score)
	{
		this.username = username;
		this.rank = rank;
		this.score = score;
	}
}
