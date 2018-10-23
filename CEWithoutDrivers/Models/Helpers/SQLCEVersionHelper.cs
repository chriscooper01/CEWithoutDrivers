using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIConsoleClient.Models.Helpers
{
    public static class SQLCEVersionHelper
    {
        public static void SetLatestVersion(string connectionString, string path)
        {
            try
            {
                path = getFileLocation(connectionString);
                if (!File.Exists(path))
                    return;//Do nothing if it does not exists
             
                var engine = new System.Data.SqlServerCe.SqlCeEngine(connectionString);
                engine.EnsureVersion40(path);

            }
            catch (Exception e)
            {

              
            }
       
        }



        private static string getFileLocation(string connectionString)
        {
            List<string> elements = connectionString.Split(';').ToList();
            if (elements.Count > 0)
            {
                string path = elements[0];
                path = path.Replace("Data Source=", String.Empty);
                return path;
            }

            return String.Empty;
        }



        public static void EnsureVersion40(this System.Data.SqlServerCe.SqlCeEngine engine, string filename)
        {

            SQLCEVersion fileversion = DetermineVersion(filename);


            if (fileversion == SQLCEVersion.SQLCE20)
                throw new ApplicationException("Unable to upgrade from 2.0 to 4.0");



            if (SQLCEVersion.SQLCE40 > fileversion)
            {
                
                engine.Upgrade();

                
            }
        }
        private enum SQLCEVersion
        {
            SQLCE20 = 0,
            SQLCE30 = 1,
            SQLCE35 = 2,
            SQLCE40 = 3
        }
        private static SQLCEVersion DetermineVersion(string filename)
        {
            var versionDictionary = new Dictionary<int, SQLCEVersion>
        {
            { 0x73616261, SQLCEVersion.SQLCE20 },
            { 0x002dd714, SQLCEVersion.SQLCE30},
            { 0x00357b9d, SQLCEVersion.SQLCE35},
            { 0x003d0900, SQLCEVersion.SQLCE40}
        };
            int versionLONGWORD = 0;
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    fs.Seek(16, SeekOrigin.Begin);
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        versionLONGWORD = reader.ReadInt32();
                    }
                }
            }
            catch
            {
                throw;
            }
            if (versionDictionary.ContainsKey(versionLONGWORD))
            {
                return versionDictionary[versionLONGWORD];
            }
            else
            {
                throw new ApplicationException("Unable to determine database file version");
            }
        }

    }
}
