using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Infrastructure.Extension
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string stringHelper)
        {   
            StringBuilder sb = new StringBuilder();

            //find seperator
            var arrayObjects = stringHelper.Split('.');
            if(arrayObjects.Length > 0)
            {
                for(var i=0; i < arrayObjects.Length;i++)
                {   
                    string eachFirstLetter = arrayObjects[i].Substring(0,1);
                    string eachRest = arrayObjects[i].Substring(1, arrayObjects[i].Length - 1);
                    arrayObjects[i] = eachFirstLetter.ToLower() + eachRest;
                }
                sb.Append(ConvertStringArrayToString(arrayObjects));
            }
            else
            {
                string firstLetter = stringHelper.Substring(0, 1);
                string rest = stringHelper.Substring(1, stringHelper.Length - 1);
                sb.Append(firstLetter.ToLower() + rest);
            }
            
            return sb.ToString().Substring(0, sb.ToString().Length);
        }

        private static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append('.');
            }
            var stringArray = builder.ToString();
            return stringArray.Substring(0, stringArray.Length - 1);
        }


    }
}
