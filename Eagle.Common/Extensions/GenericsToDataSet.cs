using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace Eagle.Common.Extensions
{
    /// <summary>
    /// Extension for IEnumerable to convert a generic List to a DataSet
    /// Offers for an option DataTable name to be assigned to the table.
    /// Can be called as a normal method for the IEnumerable generic List
    /// SAMPLE USAGE:
    /// DataSet ds = YourList.ConvertGenericList();
    /// DataSet ds = YourList.ConvertGenericList("SomeTableName");
    /// </summary>
    public static class GenericsToDataSet
    {
        public static DataSet CreateDataSetFromMasterDetailList<T, P>(IEnumerable<T> listOfMaster, IEnumerable<P> listOfDetail, string DataSetName, string parentTableName, string childTableName, string keyUsedToJoin)
        {
            if (listOfMaster == null || listOfDetail == null)
                throw new ArgumentNullException("list");

            DataSet converted = new DataSet(DataSetName);
            converted.Tables.Add(GenericsToDataSet.newTable(parentTableName, listOfMaster));
            converted.Tables.Add(newTable(childTableName, listOfDetail));
            DataRelation relation = new DataRelation(String.Format("{0}_{1}", parentTableName, childTableName), converted.Tables[0].Columns[keyUsedToJoin], converted.Tables[1].Columns[keyUsedToJoin]);
            converted.Relations.Add(relation);
            return converted;
        }

        public static DataSet ConvertGenericList<T,P>(string name, IEnumerable<T> listOfMaster, IEnumerable<P> listOfDetail, string parent, string child)
        {
            if (listOfMaster == null || listOfDetail == null)
                throw new ArgumentNullException("list");           

            DataSet converted = new DataSet(name);
            converted.Tables.Add(newTable(parent, listOfMaster));
            converted.Tables.Add(newTable(child, listOfDetail));
            DataRelation relation = new DataRelation($"{parent}_{child}", converted.Tables[parent].Columns["LSCompanyID"], converted.Tables[child].Columns["LSCompanyID"]);
            converted.Relations.Add(relation);
            return converted;
        }
        /// <summary>
        /// (Overload) Extension method to convert a IEnumerable List<T> To a DataSet
        /// </summary>
        /// <typeparam name="T">List type that is being converted<typeparam>
        /// <param name="list">The list itself that is being converted</param>
        /// <param name="name">Name of the DataTable to be added to the DataSet</param>
        /// <returns>A populated DataSet</returns>
        public static DataSet ConvertGenericList<T>(this IEnumerable<T> list, string name)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            DataSet converted = new DataSet(name);
            converted.Tables.Add(newTable(name, list));
            return converted;
        }

        /// <summary>
        /// Extension method to convert a IEnumerable List<T> To a DataSet
        /// </summary>
        /// <typeparam name="T">List type that is being converted</typeparam>
        /// <param name="list">The list itself that is being converted</param>
        /// <returns>A populated DataSet</returns>
        public static DataSet ConvertGenericList<T>(this IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            DataSet converted = new DataSet();
            converted.Tables.Add(newTable(list));
            return converted;
        }

        /// <summary>
        /// (Overload) Method for getting and populating the DataTable that
        /// will be in the converted DataSet
        /// </summary>
        /// <typeparam name="T">List type that is being converted</typeparam>
        /// <param name="name">Name of the DataTable we want</param>
        /// <param name="list">The list being converted</param>
        /// <returns>A populated DataTable</returns>
        public static DataTable newTable<T>(string name, IEnumerable<T> list)
        {
            PropertyInfo[] pi = typeof(T).GetProperties();

            DataTable table = Table<T>(name, list, pi);

            IEnumerator<T> e = list.GetEnumerator();

            while (e.MoveNext())
                table.Rows.Add(newRow<T>(table.NewRow(), e.Current, pi));

            return table;
        }

        /// <summary>
        /// Method for getting and populating the DataTable that
        /// will be in the converted DataSet
        /// </summary>
        /// <typeparam name="T">List type that is being converted</typeparam>
        /// <param name="list">The list being converted</param>
        /// <returns>A populated DataTable</returns>
        private static DataTable newTable<T>(IEnumerable<T> list)
        {
            PropertyInfo[] pi = typeof(T).GetProperties();

            DataTable table = Table<T>(list, pi);

            IEnumerator<T> e = list.GetEnumerator();

            while (e.MoveNext())
                table.Rows.Add(newRow<T>(table.NewRow(), e.Current, pi));

            return table;
        }

        /// <summary>
        /// Method for getting the data from the list then create a new
        /// DataRow with the property values in the PropertyInfo being
        /// provided, then return the row to be added to the Dataable
        /// </summary>
        /// <typeparam name="T">Type of the Generic list being converted</typeparam>
        /// <param name="row">DatRow to populate and add</param>
        /// <param name="listItem">The current item in the list</param>
        /// <param name="pi">Properties for the current item in the list</param>
        /// <returns>A populated DataRow</returns>
        private static DataRow newRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
        {
            foreach (PropertyInfo p in pi)
                row[p.Name.ToString()] = p.GetValue(listItem, null) ?? DBNull.Value;

            return row;
        }

        /// <summary>
        /// (Overoad) Method resposible for the generation of the DataTable
        /// </summary>
        /// <typeparam name="T">Type of the List being converted</typeparam>
        /// <param name="name">Name for the DataTable</param>
        /// <param name="list">List being converted</param>
        /// <param name="pi">Properties for the list</param>
        /// <returns></returns>
        private static DataTable Table<T>(string name, IEnumerable<T> list, PropertyInfo[] pi)
        {
            DataTable table = new DataTable(name);

            foreach (PropertyInfo p in pi)
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

            return table;
        }

        /// <summary>
        /// Method resposible for the generation of the DataTable
        /// </summary>
        /// <typeparam name="T">Type of the List being converted</typeparam>
        /// <param name="list">List being converted</param>
        /// <param name="pi">Properties for the list</param>
        /// <returns></returns>
        private static DataTable Table<T>(IEnumerable<T> list, PropertyInfo[] pi)
        {
            DataTable table = new DataTable();

            foreach (PropertyInfo p in pi)
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);
            
            return table;
        }  


    }


}