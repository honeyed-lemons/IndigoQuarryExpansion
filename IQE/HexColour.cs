using System;
using System.Collections.Generic;
using UnityEngine;

namespace iqe
{
	public static class HexColour
	{
		public static Color FromHex(string hex)
		{
			Color result;
			ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out result);
			return result;
		}

		public static Color[] FromHexArray(params string[] hexas)
		{
			List<Color> list = new List<Color>();
			foreach (string text in hexas)
			{
				Color item;
				ColorUtility.TryParseHtmlString("#" + text.ToUpper(), out item);
				list.Add(item);
			}
			return list.ToArray();
		}

		public static string ToHexRGB(Color color)
		{
			return ColorUtility.ToHtmlStringRGB(color);
		}

		public static string ToHexRGBA(Color color)
		{
			return ColorUtility.ToHtmlStringRGB(color);
		}
	}
}
