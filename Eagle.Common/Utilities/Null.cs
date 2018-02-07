//
// DotNetNuke - http://www.dotnetnuke.com
// Copyright (c) 2002-2010
// by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Reflection;

namespace Eagle.Common.Utilities
{
	public class Null
	{
		public static short NullShort => -1;

	    public static int NullInteger => -1;

	    public static byte NullByte => 255;

	    public static float NullSingle => float.MinValue;

	    public static double NullDouble => double.MinValue;

	    public static decimal NullDecimal => decimal.MinValue;

	    public static DateTime NullDate => DateTime.MinValue.ToUniversalTime();

	    public static string NullString => "";

	    public static bool NullBoolean => false;

	    public static Guid NullGuid => Guid.Empty;

	    public static object SetNull(object objValue, object objField)
		{
			object returnValue;
			if (objValue == DBNull.Value)
			{
				if (objField is short)
				{
					returnValue = NullShort;
				}
				else if (objField is byte)
				{
					returnValue = NullByte;
				}
				else if (objField is int)
				{
					returnValue = NullInteger;
				}
				else if (objField is float)
				{
					returnValue = NullSingle;
				}
				else if (objField is double)
				{
					returnValue = NullDouble;
				}
				else if (objField is decimal)
				{
					returnValue = NullDecimal;
				}
				else if (objField is DateTime)
				{
					returnValue = NullDate;
				}
				else if (objField is string)
				{
					returnValue = NullString;
				}
				else if (objField is bool)
				{
					returnValue = NullBoolean;
				}
				else if (objField is Guid)
				{
					returnValue = NullGuid;
				}
				else
				{
					returnValue = null;
				}
			}
			else
			{
				returnValue = objValue;
			}
			return returnValue;
		}
		public static object SetNull(PropertyInfo objPropertyInfo)
		{
			object returnValue;
			switch (objPropertyInfo.PropertyType.ToString())
			{
				case "System.Int16":
					returnValue = NullShort;
					break;
				case "System.Int32":
				case "System.Int64":
					returnValue = NullInteger;
					break;
				case "system.Byte":
					returnValue = NullByte;
					break;
				case "System.Single":
					returnValue = NullSingle;
					break;
				case "System.Double":
					returnValue = NullDouble;
					break;
				case "System.Decimal":
					returnValue = NullDecimal;
					break;
				case "System.DateTime":
					returnValue = NullDate;
					break;
				case "System.String":
				case "System.Char":
					returnValue = NullString;
					break;
				case "System.Boolean":
					returnValue = NullBoolean;
					break;
				case "System.Guid":
					returnValue = NullGuid;
					break;
				default:
					Type pType = objPropertyInfo.PropertyType;
					if (pType.BaseType != null && pType.BaseType == typeof(Enum))
					{
						Array objEnumValues = Enum.GetValues(pType);
						Array.Sort(objEnumValues);
						returnValue = Enum.ToObject(pType, objEnumValues.GetValue(0));
					}
					else
					{
						returnValue = null;
					}
					break;
			}
			return returnValue;
		}
		public static bool SetNullBoolean(object objValue)
		{
			bool retValue = NullBoolean;
			if (objValue != DBNull.Value)
			{
				retValue = Convert.ToBoolean(objValue);
			}
			return retValue;
		}
		public static DateTime SetNullDateTime(object objValue)
		{
			DateTime retValue = NullDate;
			if (objValue != DBNull.Value)
			{
				retValue = Convert.ToDateTime(objValue);
			}
			return retValue;
		}
		public static int SetNullInteger(object objValue)
		{
			int retValue = NullInteger;
			if (objValue != DBNull.Value)
			{
				retValue = Convert.ToInt32(objValue);
			}
			return retValue;
		}
		public static float SetNullSingle(object objValue)
		{
			float retValue = NullSingle;
			if (objValue != DBNull.Value)
			{
				retValue = Convert.ToSingle(objValue);
			}
			return retValue;
		}
		public static string SetNullString(object objValue)
		{
			string retValue = NullString;
			if (objValue != DBNull.Value)
			{
				retValue = Convert.ToString(objValue);
			}
			return retValue;
		}
		public static Guid SetNullGuid(object objValue)
		{
			Guid retValue = Guid.Empty;
			if ((objValue != DBNull.Value) & !string.IsNullOrEmpty(objValue.ToString()))
			{
				retValue = new Guid(objValue.ToString());
			}
			return retValue;
		}
		public static object GetNull(object objField, object objDbNull)
		{
			object returnValue = objField;
			if (objField == null)
			{
				returnValue = objDbNull;
			}
			else if (objField is byte)
			{
				if (Convert.ToByte(objField) == NullByte)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is short)
			{
				if (Convert.ToInt16(objField) == NullShort)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is int)
			{
				if (Convert.ToInt32(objField) == NullInteger)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is float)
			{
				if (Convert.ToSingle(objField) == NullSingle)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is double)
			{
				if (Convert.ToDouble(objField) == NullDouble)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is decimal)
			{
				if (Convert.ToDecimal(objField) == NullDecimal)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is DateTime)
			{
				if (Convert.ToDateTime(objField).Date == NullDate.Date)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is string)
			{
				if (objField == null)
				{
					returnValue = objDbNull;
				}
				else
				{
					if (objField.ToString() == NullString)
					{
						returnValue = objDbNull;
					}
				}
			}
			else if (objField is bool)
			{
				if (Convert.ToBoolean(objField) == NullBoolean)
				{
					returnValue = objDbNull;
				}
			}
			else if (objField is Guid)
			{
				if (((Guid)objField).Equals(NullGuid))
				{
					returnValue = objDbNull;
				}
			}
			return returnValue;
		}
		public static bool IsNull(object objField)
		{
			bool isNull;
			if (objField != null) 
			{
				if (objField is int) {
					isNull = objField.Equals(NullInteger);
				} else if (objField is short) {
					isNull = objField.Equals(NullShort);
				} else if (objField is byte) {
					isNull = objField.Equals(NullByte);
				} else if (objField is float) {
					isNull = objField.Equals(NullSingle);
				} else if (objField is double) {
					isNull = objField.Equals(NullDouble);
				} else if (objField is decimal) {
					isNull = objField.Equals(NullDecimal);
				} else if (objField is DateTime) {
					DateTime objDate = (DateTime)objField;
					isNull = objDate.Date.Equals(NullDate.Date);
				} else if (objField is string) {
					isNull = objField.Equals(NullString);
				} else if (objField is bool) {
					isNull = objField.Equals(NullBoolean);
				} else if (objField is Guid) {
					isNull = objField.Equals(NullGuid);
				} else {
					isNull = false;
				}
			} 
			else
			{
				isNull = true;
			}
			return isNull;
		}
	}
}
