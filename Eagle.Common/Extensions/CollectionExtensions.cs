using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Eagle.Common.Extensions
{
    public class CollectionUitls
    {
        public static DataTable FillDataTableFromIEnumerable(IEnumerable enumerable)
        {
            DataTable dataTable = new DataTable();
            DataRow row;
            PropertyInfo[] propertyInfos;

            foreach (object obj in enumerable)
            {
                propertyInfos = obj.GetType().GetProperties();

                if (dataTable.Columns.Count == 0)
                    foreach (PropertyInfo pi in propertyInfos)
                        dataTable.Columns.Add(pi.Name, pi.PropertyType);

                row = dataTable.NewRow();

                foreach (PropertyInfo pi in propertyInfos)
                {
                    object value = pi.GetValue(obj, null);
                    row[pi.Name] = value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable FillDataTableFromList<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static DataSet ToDataSet<T>(IEnumerable<T> collection, string dataTableName)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (string.IsNullOrEmpty(dataTableName))
            {
                throw new ArgumentNullException("dataTableName");
            }

            DataSet data = new DataSet("NewDataSet");
            data.Tables.Add(FillDataTable(dataTableName, collection));
            return data;
        }

        private static DataTable FillDataTable<T>(string tableName, IEnumerable<T> collection)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            DataTable dt = CreateDataTable<T>(tableName,
            collection, properties);

            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                dt.Rows.Add(FillDataRow<T>(dt.NewRow(),
               enumerator.Current, properties));
            }

            return dt;
        }

        private static DataRow FillDataRow<T>(DataRow dataRow, T item, PropertyInfo[] properties)
        {
            foreach (PropertyInfo property in properties)
            {
                dataRow[property.Name.ToString()] = property.GetValue(item, null);
            }

            return dataRow;
        }

        private static DataTable CreateDataTable<T>(string tableName, IEnumerable<T> collection, PropertyInfo[] properties)
        {
            DataTable dt = new DataTable(tableName);

            foreach (PropertyInfo property in properties)
            {
                dt.Columns.Add(property.Name.ToString());
            }

            return dt;
        }

        public static List<int> ConvertStringToListInt(string strInput, char strSplitCondition)
        {
            List<int> ret = new List<int>();
            if (strInput != null)
            {
                string[] strTmp = strInput.Split(strSplitCondition);
                int tmp;
                foreach (var item in strTmp)
                {
                    try
                    {
                        tmp = Convert.ToInt32(item);
                        ret.Add(tmp);
                    }
                    catch (Exception ex) { ex.ToString(); }
                }
            }

            return ret;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = new List<DataColumn>();
            dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block.
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName == pc.Name);
                        if (d != null)
                            pc.SetValue(cn, item[pc.Name], null);
                    }
                    catch
                    {
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }

        public DataTable ConvertIEnumerableToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            //Column Names
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        //public static DataTable ConvertToDataTable(IList list)
        //{
        //    try
        //    {
        //        DataTable table = CreateTable((Object[])list[0], list);

        //        foreach (Object[] objs in list)
        //        {
        //            int i = 0;
        //            DataRow row = table.NewRow();

        //            foreach (Object obj in objs)
        //            {
        //                row[i++] = obj;
        //            }

        //            table.Rows.Add(row);
        //        }

        //        return table;
        //    }
        //    catch { }
        //    return null;
        //}

        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            foreach (PropertyDescriptor prop in properties)
            {
                // HERE IS WHERE THE ERROR IS THROWN FOR NULLABLE TYPES
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            return table;
        }

        public static DataTable CreateTable(Object[] objs, IList list)
        {
            DataTable table = new DataTable();
            int i = 0;

            foreach (Object obj in objs)
            {
                Object[] objTemplate = ((Object[])list.Cast<Object>().FirstOrDefault(x => ((Object[])x)[i] != null));
                table.Columns.Add(null, obj == null ?
                    objTemplate == null || objTemplate[i] == null ? typeof(string)
                        : objTemplate.GetType() : obj.GetType());
            }

            return table;
        }

        /// <summary>
        /// Convert IEnumerable<T/> to DataTable - using for entity framework mapping
        /// </summary>
        /// <typeparam name="T">Entity of object</typeparam>
        /// <param name="list">The list of data</param>
        /// <param name="tableName"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> list, string tableName)
        {
            var dataTable = new DataTable(tableName);
            try
            {
                if (list != null)
                {
                    var props =
                        typeof(T).GetProperties()
                            .Where(prop => Attribute.IsDefined(prop, typeof(TableColumnAttribute)))
                            .ToList();

                    foreach (var prop in props)
                    {
                        var attr =
                            (TableColumnAttribute)
                            prop.GetCustomAttributes(typeof(TableColumnAttribute), false).FirstOrDefault();

                        if (attr != null)
                        {
                            var propertyType = prop.PropertyType;

                            if (propertyType.IsGenericType &&
                                propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = propertyType.GetGenericArguments()[0];
                            }

                            dataTable.Columns.Add(new DataColumn(attr.Name, propertyType));
                        }
                    }

                    object[] values = new object[props.Count];

                    using (var enumerator = list.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var item = enumerator.Current;
                            for (int i = 0; i < values.Length; i++)
                            {
                                values[i] = props[i].GetValue(item, null);
                            }
                            dataTable.Rows.Add(values);
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Convert IList<T/> to DataTable - using for entity framework mapping
        /// </summary>
        /// <typeparam name="T">Entity of object</typeparam>
        /// <param name="list">The list of data</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            var dataTable = new DataTable();
            try
            {
                dataTable.TableName = GetTableName<T>();

                if (list != null && list.Count > 0)
                {
                    var entity = Activator.CreateInstance(typeof(T));
                    var props =
                        entity.GetType().GetProperties()
                            //.Where(prop => Attribute.IsDefined(prop, typeof(ColumnAttribute)))
                            .ToList();

                    foreach (var prop in props)
                    {
                        //var attr = (ColumnAttribute)prop.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                        var attr = prop;
                        if (attr != null)
                        {
                            var propertyType = prop.PropertyType;

                            if (propertyType.IsGenericType &&
                                propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = propertyType.GetGenericArguments()[0];
                            }

                            dataTable.Columns.Add(new DataColumn(attr.Name, propertyType));
                        }
                    }

                    object[] values = new object[props.Count];

                    foreach (var item in list)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item, null);
                        }
                        dataTable.Rows.Add(values);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetTableName<T>()
        {
            var customAttr = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (customAttr != null)
                return customAttr.Name;
            return string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TableColumnAttribute : Attribute
    {
        public TableColumnAttribute() { }
        public TableColumnAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
