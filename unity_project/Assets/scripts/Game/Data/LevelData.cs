public class LevelData {

	public static int[] passLevelCountForLength = new int[]{5, 13, 26, 46, 73};

	public static int NormalLevelMaxIndex(GameSystem.Mode mode)
	{
		return LevelData.wavesInfo[(int)mode].Length - 2;
	}
	
	public static int HellLevelIndex(GameSystem.Mode mode)
	{
		return LevelData.wavesInfo[(int)mode].Length - 1;
	}

	public static Wave GetPassLevelWaveData(GameSystem.Mode mode, int waveNumber)
	{
		waveNumber = (waveNumber - 1) / 3 + 1;
		int gridLength = 3;

		if (gridLength == 7)
		{
			int finalCount = 0;
			if (waveNumber > 74)
			{
				finalCount = 27;
			}
			else
			{
				finalCount = (gridLength - 1) + (waveNumber - 1 - passLevelCountForLength[gridLength-4]);
			}
			return new Wave(gridLength, gridLength, finalCount);
		}

		while(waveNumber > passLevelCountForLength[gridLength-3] && gridLength != 7)
		{
			gridLength++;
		}

		int blockCount = 0;
		if (gridLength == 3)
		{
			blockCount = (gridLength - 1) + (waveNumber - 1);
		}
		else
		{
			blockCount = (gridLength - 1) + (waveNumber - 1 - passLevelCountForLength[gridLength-4]);
		}
		return new Wave(gridLength, gridLength, blockCount);
	}

	//每个模式最后一关的数据是给地狱模式使用的
	//21点与单词模式只有挑战，无闯关与地狱, 但为了保证逻辑一致，也添加地狱数据，但不使用.
	public static Wave[][] wavesInfo = new Wave[][]{
		//经典模式
		new Wave[]{
			new Wave(3, 3, 2),
			new Wave(3, 3, 3),
			new Wave(3, 3, 4),
			new Wave(3, 3, 5),
			new Wave(4, 4, 4),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 7),
			new Wave(4, 4, 8),
			new Wave(5, 5, 5),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 8),
			new Wave(5, 5, 9),
			new Wave(5, 5, 10),
			new Wave(5, 5, 11),
			new Wave(5, 5, 12),
			new Wave(5, 5, 13),
			new Wave(6, 6, 6),
			new Wave(6, 6, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(6, 6, 13),
			new Wave(6, 6, 14),
			new Wave(6, 6, 15),
			new Wave(6, 6, 16),
			new Wave(6, 6, 17),
			new Wave(6, 6, 18),
			new Wave(7, 7, 25)
		},
		
		//反转模式
		new Wave[]{
			new Wave(3, 3, 5),
			new Wave(3, 3, 4),
			new Wave(3, 3, 3),
			new Wave(3, 3, 2),
			new Wave(4, 4, 8),
			new Wave(4, 4, 7),
			new Wave(4, 4, 6),
			new Wave(4, 4, 5),
			new Wave(4, 4, 4),
			new Wave(4, 4, 3),
			new Wave(5, 5, 13),
			new Wave(5, 5, 12),
			new Wave(5, 5, 11),
			new Wave(5, 5, 10),
			new Wave(5, 5, 9),
			new Wave(5, 5, 8),
			new Wave(5, 5, 7),
			new Wave(5, 5, 6),
			new Wave(5, 5, 5),
			new Wave(5, 5, 4),
			new Wave(6, 6, 18),
			new Wave(6, 6, 17),
			new Wave(6, 6, 16),
			new Wave(6, 6, 15),
			new Wave(6, 6, 14),
			new Wave(6, 6, 13),
			new Wave(6, 6, 12),
			new Wave(6, 6, 11),
			new Wave(6, 6, 10),
			new Wave(6, 6, 9),
			new Wave(6, 6, 8),
			new Wave(6, 6, 7),
			new Wave(6, 6, 6),
			new Wave(6, 6, 5),
			new Wave(7, 7, 25)
		},
		
		//生存模式
		new Wave[]{
			new Wave(3, 3, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 7),
			new Wave(4, 4, 8),
			new Wave(4, 4, 9),
			new Wave(5, 5, 7),
			new Wave(5, 5, 8),
			new Wave(5, 5, 9),
			new Wave(5, 5, 10),
			new Wave(5, 5, 11),
			new Wave(5, 5, 12),
			new Wave(5, 5, 13),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(6, 6, 13),
			new Wave(6, 6, 14),
			new Wave(6, 6, 15),
			new Wave(6, 6, 16),
			new Wave(6, 6, 17),
			new Wave(6, 6, 18),
			new Wave(7, 7, 25)
		},
		
		//限时模式
		new Wave[]{
			new Wave(3, 3, 4),
			new Wave(3, 3, 5),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 7),
			new Wave(4, 4, 8),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 8),
			new Wave(5, 5, 9),
			new Wave(5, 5, 10),
			new Wave(5, 5, 11),
			new Wave(5, 5, 12),
			new Wave(5, 5, 13),
			new Wave(6, 6, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(6, 6, 13),
			new Wave(6, 6, 14),
			new Wave(6, 6, 15),
			new Wave(6, 6, 16),
			new Wave(6, 6, 17),
			new Wave(6, 6, 18),
			new Wave(7, 7, 25)
		},
		
		//多彩模式
		new Wave[]{
			new Wave(3, 3, 4),
			new Wave(3, 3, 5),
			new Wave(3, 3, 6),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 7),
			new Wave(4, 4, 8),
			new Wave(4, 4, 9),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 8),
			new Wave(5, 5, 9),
			new Wave(5, 5, 10),
			new Wave(5, 5, 11),
			new Wave(5, 5, 12),
			new Wave(5, 5, 13),
			new Wave(5, 5, 14),
			new Wave(6, 6, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(6, 6, 13),
			new Wave(6, 6, 14),
			new Wave(6, 6, 15),
			new Wave(6, 6, 16),
			new Wave(6, 6, 17),
			new Wave(6, 6, 18),
			new Wave(6, 6, 19),
			new Wave(7, 7, 25)
		},
		
		//21点模式
		new Wave[]{
			new Wave(3, 3, 3),
			new Wave(3, 3, 4),
			new Wave(3, 3, 5),
			new Wave(4, 4, 4),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 7),
			new Wave(4, 4, 8),
			new Wave(5, 5, 5),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 8),
			new Wave(5, 5, 9),
			new Wave(5, 5, 10),
			new Wave(5, 5, 11),
			new Wave(5, 5, 12),
			new Wave(5, 5, 13),
			new Wave(6, 6, 6),
			new Wave(6, 6, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(6, 6, 13),
			new Wave(6, 6, 14),
			new Wave(6, 6, 15),
			new Wave(6, 6, 16),
			new Wave(6, 6, 17),
			new Wave(6, 6, 18),
			new Wave(7, 7, 25)
		},
		
		//单词模式
		new Wave[]{
			new Wave(3, 3, 3),
			new Wave(3, 3, 3),
			new Wave(3, 3, 4),
			new Wave(3, 3, 4),
			new Wave(4, 4, 5),
			new Wave(4, 4, 5),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 6),
			new Wave(4, 4, 6),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(7, 7, 12)
		},
		
		//猩猩模式
		new Wave[]{
			new Wave(3, 3, 3),
			new Wave(3, 3, 3),
			new Wave(3, 3, 4),
			new Wave(3, 3, 4),
			new Wave(4, 4, 5),
			new Wave(4, 4, 5),
			new Wave(4, 4, 5),
			new Wave(4, 4, 6),
			new Wave(4, 4, 6),
			new Wave(4, 4, 6),
			new Wave(5, 5, 6),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(5, 5, 7),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 8),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 9),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 10),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 11),
			new Wave(6, 6, 12),
			new Wave(7, 7, 20)
		},
		
		//双人模式
		new Wave[]{
			new Wave(3, 3, 6),
			new Wave(3, 3, 8),
			new Wave(4, 4, 8),
			new Wave(4, 4, 10),
			new Wave(4, 4, 12),
			new Wave(5, 5, 12),
			new Wave(5, 5, 14),
			new Wave(5, 5, 16),
			new Wave(6, 6, 16),
			new Wave(6, 6, 18),
			new Wave(6, 6, 20),
			new Wave(6, 6, 22),
			new Wave(6, 6, 24),
			new Wave(7, 7, 24),
			new Wave(7, 7, 26),
			new Wave(7, 7, 28),
			new Wave(7, 7, 30),
		}
	};
}
