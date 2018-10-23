using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UIConsoleClient.Models.Helpers
{
    public class SDFHelper
    {

        public static string DbConnection = @"Data Source={0};{1}";
        public const string DBNAME = @"c:\temp\test2.sdf";
        private const string PASSWORD = "P@ssw0rd";
        private static string ConnectionString
        {
            get
            {
                return String.Format(DbConnection, DBNAME, String.Format("Password={0}", PASSWORD));
            }
        }


        public static DataContext GetContext()
        {
            SQLCEVersionHelper.SetLatestVersion(ConnectionString, DBNAME);

            var c = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);
            return new DataContext(c);
            
        }

        public static bool CreateLocalSDF()
        {
            try
            {
                using (var en = new System.Data.SqlServerCe.SqlCeEngine(String.Format(DbConnection, DBNAME, String.Format("Password={0}", PASSWORD))))
                {
                    en.CreateDatabase();
                }

               
            }
            catch (Exception e)
            {
               
                return false;
            }

            return true;



        }
        public static void AddTables()
        {
            
            using (var context = GetContext())
            {
                CreateTable(context, typeof(CEWithoutDrivers.Models.Tables.UserTable));
            }
        }


        private static void CreateTable(DataContext context, Type linqTableClass)
        {

            var metaTable = context.Mapping.GetTable(linqTableClass);
            var typeName = "System.Data.Linq.SqlClient.SqlBuilder";
            var type = typeof(DataContext).Assembly.GetType(typeName);
            var bf = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var sql = type.InvokeMember("GetCreateTableCommand", bf, null, null, new[] { metaTable });
            var sqlAsString = sql.ToString();
            context.ExecuteCommand(sqlAsString);

        }
    }
}
