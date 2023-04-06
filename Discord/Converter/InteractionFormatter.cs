using Discord.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Converter
{
    #region Public Area

    public static class InteractionFormatter
    {
        public static string TransformEnumsToId(params Enum[] enums)
        {
            string output = "";
            foreach (var num in enums)
            {
                output += num.ToString("D") + ",";
            }
            output = output.Remove(output.Length - 1);
            return output;
        }

        /// <summary>
        /// Transforms the given values to the given enums. CAUTION: The length of the given strings and enums must be the same.
        /// </summary>
        /// <param name="value">The list of values separated by comma (e.g. "0,5,3")</param>
        /// <param name="enums">The list of enums that should be given out (e.g. typeof(Enum), typeof(Enum)"/></param>
        /// <returns>A list of converted enums that were passed in.</returns>
        /// <exception cref="MismatchedEnumLengthsException">Thrown when the length of enums and values are different.</exception>
        public static IEnumerable<Enum> TransformIdToEnums(string value, params Type[] enums)
        {
            string[] values = value.Split(',');
            if (values.Length != enums.Length)
            {
                throw new MismatchedEnumLengthsException();
            }
            for (int i = 0; i < enums.Length; i++)
            {
                yield return (Enum)Enum.Parse(enums[i], value);
            }
        }

        public static T GetFirstEnumFromIdAndRemove<T>(ref string[] values)
        {
            string firstValue = values[0];
            values = values.Skip(1).ToArray();
            return (T)Enum.Parse(typeof(T), firstValue);
        }

        public static T GetFirstEnumFromId<T>(ref string[] values)
        {
            return (T)Enum.Parse(typeof(T), values[0]);
        }

        public static T GetEnumFromId<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }

    #endregion Public Area
}
