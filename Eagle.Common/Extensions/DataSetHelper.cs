using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Common.Extensions
{
    public class ColumnInfo
    {
        public string RelationName;
        public string ColumnName;	//source table Column name
        public string ColumnAlias;	//destination table Column name
        public string Aggregate;
    }

    public class DataSetHelper
    {
        public static DataSet ds;
        private static System.Collections.ArrayList m_ColumnInfo;
        private static string m_ColumnList; 

        public DataSetHelper(ref DataSet DataSet)
        {
            ds = DataSet;
        }

        public DataSetHelper()
        {
            ds = null;
        }

        public static DataSet CreateDataSetJoinedFrom2Tables(string DataSetName, string joinedColumnName, DataTable dtParent, DataTable dtChild)
        {
            ds = new DataSet(DataSetName);
            ds.Tables.Add(dtParent);
            ds.Tables.Add(dtChild);
            DataRelation dataRelation = new DataRelation(String.Format("{0}_{1}", ds.Tables[0].TableName, ds.Tables[1].TableName), ds.Tables[0].Columns[joinedColumnName], ds.Tables[1].Columns[joinedColumnName], false);
            ds.Relations.Add(dataRelation);
            return ds;
        }


        private void ParseColumnList(string ColumnList, bool AllowRelation)
        {
            /*
             * This code parses ColumnList into ColumnInfo objects  and then 
             * adds them to the m_ColumnInfo private member
             * 
             * ColumnList systax:  [relationname.]Columnname[ alias], ...
            */
            if (m_ColumnList == ColumnList) return;
            m_ColumnInfo = new System.Collections.ArrayList();
            m_ColumnList = ColumnList;
            ColumnInfo Column; string[] ColumnParts;
            string[] Columns = ColumnList.Split(',');
            int i;
            for (i = 0; i <= Columns.Length - 1; i++)
            {
                Column = new ColumnInfo();
                //parse ColumnAlias
                ColumnParts = Columns[i].Trim().Split(' ');
                switch (ColumnParts.Length)
                {
                    case 1:
                        //to be set at the end of the loop
                        break;
                    case 2:
                        Column.ColumnAlias = ColumnParts[1];
                        break;
                    default:
                        throw new Exception("Too many spaces in Column definition: '" + Columns[i] + "'.");
                }
                //parse ColumnName and RelationName
                ColumnParts = ColumnParts[0].Split('.');
                switch (ColumnParts.Length)
                {
                    case 1:
                        Column.ColumnName = ColumnParts[0];
                        break;
                    case 2:
                        if (AllowRelation == false)
                            throw new Exception("Relation specifiers not permitted in Column list: '" + Columns[i] + "'.");
                        Column.RelationName = ColumnParts[0].Trim();
                        Column.ColumnName = ColumnParts[1].Trim();
                        break;
                    default:
                        throw new Exception("Invalid Column definition: " + Columns[i] + "'.");
                }
                if (Column.ColumnAlias == null)
                    Column.ColumnAlias = Column.ColumnName;
                m_ColumnInfo.Add(Column);
            }
        }

        //
        //DataTable dt = dsHelper.CreateJoinTable("TestTable", ds.Tables["Employees"],"FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department");
        //
        public DataTable CreateJoinTable(string TableName, DataTable SourceTable, string ColumnList)
        {
            /*
             * Creates a table based on Columns of another table and related parent tables
             * 
             * ColumnList syntax: [relationname.]Columnname[ alias][,[relationname.]Columnname[ alias]]...
            */
            if (ColumnList == null)
            {
                throw new ArgumentException("You must specify at least one Column in the Column list.");
                //return CreateTable(TableName, SourceTable);
            }
            else
            {
                DataTable dt = new DataTable(TableName);
                ParseColumnList(ColumnList, true);
                foreach (ColumnInfo Column in m_ColumnInfo)
                {
                    if (Column.RelationName == null)
                    {
                        DataColumn dc = SourceTable.Columns[Column.ColumnName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                    else
                    {
                        DataColumn dc = SourceTable.ParentRelations[Column.RelationName].ParentTable.Columns[Column.ColumnName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                }
                if (ds != null)
                    ds.Tables.Add(dt);
                return dt;
            }
        }

        //
        //dsHelper.InsertJoinInto(ds.Tables["TestTable"], ds.Tables["Employees"], "FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department", "EmployeeID<5", "BirthDate");
        //
        public void InsertJoinInto(DataTable DestTable, DataTable SourceTable, string ColumnList, string RowFilter, string Sort)
        {
            /*
            * Copies the selected rows and columns from SourceTable and inserts them into DestTable
            * ColumnList has same format as CreatejoinTable
            */
            if (ColumnList == null)
            {
                throw new ArgumentException("You must specify at least one Column in the Column list.");
                //InsertInto(DestTable, SourceTable, RowFilter, Sort);
            }
            else
            {
                ParseColumnList(ColumnList, true);
                DataRow[] Rows = SourceTable.Select(RowFilter, Sort);
                foreach (DataRow SourceRow in Rows)
                {
                    DataRow DestRow = DestTable.NewRow();
                    foreach (ColumnInfo Column in m_ColumnInfo)
                    {
                        if (Column.RelationName == null)
                        {
                            DestRow[Column.ColumnName] = SourceRow[Column.ColumnName];
                        }
                        else
                        {
                            DataRow ParentRow = SourceRow.GetParentRow(Column.RelationName);
                            DestRow[Column.ColumnName] = ParentRow[Column.ColumnName];
                        }
                    }
                    DestTable.Rows.Add(DestRow);
                }
            }
        }

        //
        //dt = dsHelper.SelectInto("TestTable", ds.Tables["Employees"], "FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department", "EmployeeID<5", "BirthDate");
        //
        public DataTable SelectJoinInto(string TableName, DataTable SourceTable, string ColumnList, string RowFilter, string Sort)
        {
            /*
                * Selects sorted, filtered values from one DataTable to another.
                * Allows you to specify relationname.Columnname in the ColumnList to include Columns from
                *  a parent table. The Sort and Filter only apply to the base table and not to related tables.
            */ 
            DataTable dt = CreateJoinTable(TableName, SourceTable, ColumnList);
            InsertJoinInto(dt, SourceTable, ColumnList, RowFilter, Sort);
            return dt;
        }
        
        //        ds = new DataSet();
        //dsHelper = new DataSetHelper.DataSetHelper(ref ds);
        ////Create source tables
        //DataTable dt = new DataTable("Employees");
        //dt.Columns.Add("EmployeeID",Type.GetType("System.Int32") );
        //dt.Columns.Add("FirstName", Type.GetType("System.String"));
        //dt.Columns.Add("LastName", Type.GetType("System.String"));
        //dt.Columns.Add("BirthDate", Type.GetType("System.DateTime"));
        //dt.Columns.Add("JobTitle", Type.GetType("System.String"));
        //dt.Columns.Add("DepartmentID", Type.GetType("System.Int32"));
        //dt.Rows.Add(new object[] {1, "Tommy", "Hill", new DateTime(1970, 12, 31),  "Manager", 42});
        //dt.Rows.Add(new object[] {2, "Brooke", "Sheals", new DateTime(1977, 12, 31), "Manager", 23});
        //dt.Rows.Add(new object[] {3, "Bill", "Blast", new DateTime(1982, 5, 6), "Sales Clerk", 42});
        //dt.Rows.Add(new object[] {1, "Kevin", "Kline", new DateTime(1978, 5, 13), "Sales Clerk", 42});
        //dt.Rows.Add(new object[] {1, "Martha", "Seward", new DateTime(1976, 7, 4), "Sales Clerk", 23});
        //dt.Rows.Add(new object[] {1, "Dora", "Smith", new DateTime(1985, 10, 22), "Trainee", 42});
        //dt.Rows.Add(new object[] {1, "Elvis", "Pressman", new DateTime(1972, 11, 5), "Manager", 15});
        //dt.Rows.Add(new object[] {1, "Johnny", "Cache", new DateTime(1984, 1, 23), "Sales Clerk", 15});
        //dt.Rows.Add(new object[] {1, "Jean", "Hill", new DateTime(1979, 4, 14), "Sales Clerk", 42});
        //dt.Rows.Add(new object[] {1, "Anna", "Smith", new DateTime(1985, 6, 26), "Trainee", 15});
        //ds.Tables.Add(dt);

        //dt = new DataTable("Departments");
        //dt.Columns.Add("DepartmentID", Type.GetType("System.Int32"));
        //dt.Columns.Add("DepartmentName", Type.GetType("System.String"));
        //dt.Rows.Add(new object[] {15, "Men's Clothing"});
        //dt.Rows.Add(new object[] {23, "Women's Clothing"});
        //dt.Rows.Add(new object[] {42, "Children's Clothing"});
        //ds.Tables.Add(dt);

        //ds.Relations.Add("DepartmentEmployee",     ds.Tables["Departments"].Columns["DepartmentID"], 
        //    ds.Tables["Employees"].Columns["DepartmentID"]);

        //
        //===============================================================================================
        //
       
   
    
    }
}
