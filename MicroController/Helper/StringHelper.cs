using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microController.Helper
{
    public static class StringHelper
    {
        public static string GetDataBetweenChars(string data, char firstChar, char lastChar)
        {
            string result = "";
            for (int i = 0; i < data.IndexOf(lastChar) - data.IndexOf(firstChar) - 2; i++)
            {
                result += data[data.IndexOf(firstChar) + 2 + i];
            }
            return result;
        }
        public static string GetDataBetweenChars(string data, char Char)
        {
            string result = "";
            for (int i = 0; i < data.Length - data.IndexOf(Char) - 2; i++)
            {
                result += data[data.IndexOf(Char) + 2 + i];
            }
            return result;
        }
        public static string GetDataAfterChar(string data, char Char)
        {
            string result = "";
            for (int i = 0; i < data.Length - data.IndexOf(Char) - 1; i++)
            {
                result += data[data.IndexOf(Char) + 1 + i];
            }
            return result;
        }
    }
}
