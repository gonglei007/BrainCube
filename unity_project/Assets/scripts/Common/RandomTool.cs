using System;
using System.Collections.Generic;

public class RandomTool
{
	private static Random 	random;
	private static int		seed;

	private static void CheckRandomInstance()
	{
		if (random == null)
		{
			seed = (int)DateTime.Now.Ticks;
			random = new System.Random(seed);
		}
	}

	public static void SetSeed(int seed)
	{
		RandomTool.seed = seed;
		random = new System.Random(seed);
	}

	public static int GetSeed()
	{
		CheckRandomInstance();
		return seed;
	}

	public static float Float(float min, float max)
	{
		CheckRandomInstance();
		return (float)(min + random.NextDouble() * (max - min));
	}

	public static float Float(float max)
	{
		CheckRandomInstance();
		return (float)(random.NextDouble() * max);
	}

	public static float Float()
	{
		CheckRandomInstance();
		return (float)random.NextDouble();
	}

	public static int Int(int max)
	{
		CheckRandomInstance();
		return random.Next(max);
	}

	public static int Int(int min, int max)
	{
		CheckRandomInstance();
		return random.Next(min, max);
	}

	public static int IntRange(int min, int max)
	{
		CheckRandomInstance();
		return random.Next(min, max + 1);
	}

	public static int NormalIntRange(int min, int max)
	{
		CheckRandomInstance();
		return (int)((random.Next(min, max + 1) + random.Next(min, max + 1)) / 2f);
	}

	public static int Chances(float[] chances)
	{
		CheckRandomInstance();

		int length = chances.Length;

		float sum = chances[0];
		for (int i = 1; i < length; i++)
		{
			sum += chances[i];
		}

		float value = Float(sum);
		sum = chances[0];
		for (int i = 0; i < length; i++)
		{
			if (value < sum)
			{
				return i;
			}
			sum += chances[i + 1];
		}

		return 0;
	}

	public static T Chances<T>(Dictionary<T, float> chances)
	{
		CheckRandomInstance();

		float[] probs = new float[chances.Count];
		float sum = 0;
		int index = 0;
		foreach(KeyValuePair<T, float> kvp in chances)
		{
			probs[index] = kvp.Value;
			sum += kvp.Value;
			index++;
		}

		float value = Float(sum);

		sum = probs[0];
		index = 0;
		foreach(KeyValuePair<T, float> kvp in chances)
		{
			if (value < sum)
			{
				return kvp.Key;
			}
			sum += probs[index + 1];
			index++;
		}

		return default(T);
	}

	public static int Index<T>(ICollection<T> collection)
	{
		CheckRandomInstance();
		return (int)(random.NextDouble() * collection.Count);
	}

	public static T OneOf<T>(params T[] array)
	{
		CheckRandomInstance();
		return array[(int)(random.NextDouble() * array.Length)];
	}

	public static T Element<T>(T[] array)
	{
		CheckRandomInstance();
		return Element(array, array.Length);
	}

	public static T Element<T>(T[] array, int max)
	{
		CheckRandomInstance();
		return array[(int)(random.NextDouble() * max)];
	}

	public static T Element<T>(ICollection<T> collection)
	{
		CheckRandomInstance();
		int size = collection.Count;
		T element = default(T);
		if (size > 0)
		{
			int index = Int(size);
			foreach(T e in collection)
			{
				if (index == 0)
				{
					element = e;
					break;
				}
				index--;
			}
		}
		return element;
	}

	public static void Shuffle<T>(T[] array)
	{
		CheckRandomInstance();
		for (int i = 0; i < array.Length - 1; i++)
		{
			int j = Int(i, array.Length);
			if (j != i)
			{
				T t = array[i];
				array[i] = array[j];
				array[j] = t;
			}
		}
	}

	public static void Shuffle<U, V>(U[] u, V[] v)
	{
		CheckRandomInstance();
		for (int i = 0; i < u.Length - 1; i++)
		{
			int j = Int(i, u.Length);
			if (j != i)
			{
				U ut = u[i];
				u[i] = u[j];
				u[j] = ut;

				V vt = v[i];
				v[i] = v[j];
				v[j] = vt;
			}
		}
	}
}