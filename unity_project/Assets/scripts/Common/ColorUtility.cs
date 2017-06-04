using UnityEngine;

public class ColorUtility {

	public static Color HSB2RGB(int hue, float saturation, float brightness)
	{
		return HSB2RGB((hue % 360) / 360.0f, saturation, brightness);
	}
	
	public static Color HSB2RGB(float[] hsbValues)
	{
		return HSB2RGB(hsbValues[0], hsbValues[1], hsbValues[2]);
	}
	
	public static Color HSB2RGB(float hue, float saturation, float brightness) 
	{
		float red = 0;
		float green = 0;
		float blue = 0;
		if (saturation == 0)
		{
			red = green = blue = brightness;
		}
		else
		{
			float h = (hue - Mathf.Floor(hue)) * 6.0f;
			float f = h - Mathf.Floor(h);
			float p = brightness * (1.0f - saturation);
			float q = brightness * (1.0f - saturation * f);
			float t = brightness * (1.0f - (saturation * (1.0f - f)));
			switch ((int) h) {
			case 0:
				red = brightness;
				green = t;;
				blue = p;
				break;
			case 1:
				red = q;
				green = brightness;
				blue = p;
				break;
			case 2:
				red = p;
				green = brightness;
				blue = t;
				break;
			case 3:
				red = p;
				green = q;
				blue = brightness;
				break;
			case 4:
				red = t;
				green = p;
				blue = brightness;
				break;
			case 5:
				red = brightness;
				green = p;
				blue = q;
				break;
			}
		}
		
		return new Color(red, green, blue);
	}

	public static float[] RGB2HSB(Color color)
	{
		return RGB2HSB(color.r, color.g, color.b);
	}
	
	public static float[] RGB2HSB(float red, float green, float blue)
	{
		float hue, saturation, brightness;
		float[] hsbValues = new float[3];
	
		float cmax = (red > green) ? red : green;
		if (blue > cmax)
		{
			cmax = blue;
		}

		float cmin = (red < green) ? red : green;
		if (blue < cmin)
		{
			cmin = blue;
		}

		brightness = cmax;

		if (cmax != 0)
		{
			saturation = (cmax - cmin) / cmax;
		}
		else
		{
			saturation = 0;
		}

		if (saturation == 0)
		{
			hue = 0;
		}
		else
		{
			float redc = (cmax - red) / (cmax - cmin);
			float greenc = (cmax - green) / (cmax - cmin);
			float bluec = (cmax - blue) / (cmax - cmin);
			if (red == cmax)
			{
				hue = bluec - greenc;
			}
			else if (green == cmax)
			{
				hue = 2.0f + redc - bluec;
			}
			else
			{
				hue = 4.0f + greenc - redc;
			}
			hue = hue / 6.0f;
			if (hue < 0)
			{
				hue = hue + 1.0f;
			}
		}

		hsbValues[0] = hue;
		hsbValues[1] = saturation;
		hsbValues[2] = brightness;

		return hsbValues;
	}
}
