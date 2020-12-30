using System;
using System.Linq;

public static class EnumExtentions
{
	public static string Display(this Enum t)
	{
		Type type = t.GetType();
		string name = Enum.GetName(type, t);
		EnumDisplayAttribute enumDisplayAttribute = type.GetField(name).GetCustomAttributes(inherit: false).FirstOrDefault((object p) => p.GetType().Equals(typeof(EnumDisplayAttribute))) as EnumDisplayAttribute;
		if (enumDisplayAttribute != null)
		{
			return enumDisplayAttribute.Display;
		}
		return name;
	}
}
