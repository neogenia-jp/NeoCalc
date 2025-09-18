using System;

namespace CalcLib.Yamamoto3.Extensions;

internal static class StringExt
{
    public static string DeleteLastLetter(this string self)
    {
        if (self.Length > 0)
        {
            return self.Substring(0, self.Length - 1);
        }
        return "";
    }
}
