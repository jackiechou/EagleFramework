using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Common.Utilities
{
    public static class MathUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string ToString(object info)
        {
            if (info == null)
            {
                return string.Empty;
            }
            return info.ToString();
        }

        #region Parse Object To Object
        /// <summary>
        /// Parse Object To Integer Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            try
            {
                return Int32.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Parse Object To Double Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ParseDouble(object obj)
        {
            try
            {
                return Double.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return 0.00;
            }
        }

        /// <summary>
        /// Parse Object To Double Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ParseLong(object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Parse Object To Decimal Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(object obj)
        {
            try
            {
                return Decimal.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Parse Object To Boolean Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ParseBoolean(object obj)
        {
            try
            {
                return Boolean.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Parse Object To String Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ParseString(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static int ParseBooleanToInt(Nullable<bool> obj)
        {
            try
            {
                if (obj == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int? ParseBooleanToIntReturnNull(Nullable<bool> obj)
        {
            try
            {
                if (obj == true)
                {
                    return 1;
                }
                else if (obj == false)
                {
                    return 0;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Parse Object To Integer Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ParseIntReturnNull(object obj)
        {
            try
            {
                return Int32.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static decimal? ParseDecReturnNull(object obj)
        {
            try
            {
                return Decimal.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ParseIntToStringSQL(object obj)
        {
            int? Result = ParseIntReturnNull(obj);
            if (Result != null)
            {
                return Result.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        public static string ParseDecToStringSQL(object obj)
        {
            decimal? Result = ParseDecReturnNull(obj);
            if (Result != null)
            {
                return Result.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        /// <summary>
        /// Parse Object To Double Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? ParseDoubleReturnNull(object obj)
        {
            try
            {
                return Double.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Parse Object To Decimal Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ParseDecimalReturnNull(object obj)
        {
            try
            {
                return Decimal.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Parse Object To Boolean Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool? ParseBooleanReturnNull(object obj)
        {
            try
            {
                return Boolean.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ParseBooleanToStringReturnNull(object obj)
        {
            try
            {
                if (Boolean.Parse(obj.ToString()) == true)
                {
                    return "1";
                }
                else if (Boolean.Parse(obj.ToString()) == false)
                {
                    return "0";
                }
                else
                {
                    return "NULL";
                }
            }
            catch (Exception)
            {
                return "NULL";
            }
        }

        public static string ParseDateTimeToStringReturnNull(object obj)
        {
            try
            {
                string result = string.Empty;
                if (obj.GetType().Name == "String")
                {
                    result = new DateTime(ParseInt(obj.ToString().Split('/')[2]), ParseInt(obj.ToString().Split('/')[1]), ParseInt(obj.ToString().Split('/')[0])).ToString("'yyyy-MM-dd'");
                }
                else if (IsDateTime(obj) == true)
                {
                    result = DateTime.Parse(obj.ToString()).ToString("'yyyy-MM-dd'");
                }
                return result;
            }
            catch (Exception)
            {
                return "NULL";
            }
        }

        public static string ConvertNumberToString(object obj)
        {
            try
            {
                if (IsDouble(obj) == true)
                {
                    return String.Format("{0:#,0.#}", obj);
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public static string ConvertNumberToStringPercent(object obj)
        {
            try
            {
                if (IsDouble(obj) == true)
                {
                    return String.Format("{0:###.###.###.###.###,##}" + "%", obj);
                }
                else
                {
                    return "0%";
                }
            }
            catch (Exception)
            {
                return "0%";
            }
        }

        /// <summary>
        /// Parse DateTime To DimDate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ConvertDateTimeToDimDate(object obj)
        {
            try
            {
                int? result = null;
                if (obj.GetType().Name == "String")
                {
                    result = Convert.ToInt32(new DateTime(ParseInt(obj.ToString().Split('/')[2]), ParseInt(obj.ToString().Split('/')[1]), ParseInt(obj.ToString().Split('/')[0])).ToString("yyyyMMdd"));
                }
                else if (IsDateTime(obj) == true)
                {
                    result = Convert.ToInt32(DateTime.Parse(obj.ToString()).ToString("yyyyMMdd"));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int? ConvertDateTimeToWorkingMonth(object obj)
        {
            try
            {
                int? result = null;
                if (obj.GetType().Name == "String")
                {
                    result = Convert.ToInt32(new DateTime(ParseInt(obj.ToString().Split('/')[2]), ParseInt(obj.ToString().Split('/')[1]), ParseInt(obj.ToString().Split('/')[0])).ToString("yyyyMM"));
                }
                else if (IsDateTime(obj) == true)
                {
                    result = Convert.ToInt32(DateTime.Parse(obj.ToString()).ToString("yyyyMM"));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ConvertDateTimeToDimDateString(DateTime? obj)
        {
            try
            {
                return DateTime.Parse(obj.ToString()).ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ConvertDateTimeToDimDateStringEN(DateTime? obj)
        {
            try
            {
                return DateTime.Parse(obj.ToString()).ToString("MM/dd/yyyy");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime? ConvertDimDateToDateTime(object obj)
        {
            try
            {
                DateTime? result = null;
                if (IsInt(obj) == true)
                {
                    string strDateTime = obj.ToString();
                    result = new DateTime(ParseInt(strDateTime.Substring(0, 4)), ParseInt(strDateTime.Substring(4, 2)), ParseInt(strDateTime.Substring(6, 2)));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Validate Date
        /// <summary>
        /// Validate Integer Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(object obj)
        {
            try
            {
                Int32.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Double Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDouble(object obj)
        {
            try
            {
                Double.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Decimal Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDecimal(object obj)
        {
            try
            {
                Decimal.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate DateTime Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDateTime(object obj)
        {
            try
            {
                DateTime.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
