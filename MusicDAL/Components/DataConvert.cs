using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDAL.Components
{
    public class DataConvert
    {
        /// <summary>
        /// The convert to.
        /// </summary>
        /// <param name="value">The value. </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T ConvertTo<T>(object value, object defaultValue) where T : struct
        {
            if (value == DBNull.Value)
            {
                return (T)defaultValue;
            }
            else
            {
                return (T)value;
            }
        }

        /// <summary>
        /// Change html date to dot Net.
        /// </summary>
        /// <param name="dateValue">The date value.</param>
        /// <returns>
        /// The <see cref="DateTime"/>formatted date time.</returns>
        public static DateTime HtmlDateToDotNet(string dateValue)
        {
            var value = DateTime.Now;
            string temp = string.Empty;

            if (!DateTime.TryParse(dateValue, out value))
            {
                // Assume it is a javascript date
                temp = Encoding.Default.GetString(Encoding.ASCII.GetBytes(dateValue)).Replace("?", string.Empty);

                if (!DateTime.TryParse(temp, out value))
                {
                    value = DateTime.MinValue;
                }
            }

            return value;
        }
    }
}
