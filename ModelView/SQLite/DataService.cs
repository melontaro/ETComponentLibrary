using SQLite4Unity3d;
using UnityEngine;
using System.IO;

namespace ET.Client
{
    [EnableClass]
    public class DataService
    {
        private SQLiteConnection _connection;

        public SQLiteConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public DataService(string DatabaseName)
        {
            string dbPath = string.Empty;
            string filepath = string.Empty;
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
            }
            else
            {
                // check if file exists in Application.persistentDataPath
                filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

                if (!File.Exists(filepath))
                {
                    Debug.Log("Database not in Persistent path");
                    // if it doesn't ->
                    // open StreamingAssets directory and load the db ->

                    if (Application.platform == RuntimePlatform.Android)
                    {
                        var loadDb =
                                new WWW("jar:file://" + Application.dataPath + "!/assets/" +
                                    DatabaseName); // this is the path to your StreamingAssets in android
                        while (!loadDb.isDone)
                        {
                        } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

                        // then save to Application.persistentDataPath
                        File.WriteAllBytes(filepath, loadDb.bytes);
                    }
                    else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        var loadDb1 = Application.dataPath + "/Raw/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                        // then save to Application.persistentDataPath
                        File.Copy(loadDb1, filepath);
                    }
                    else if (Application.platform == RuntimePlatform.WP8Player)
                    {
                        var loadDb2 = Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                        // then save to Application.persistentDataPath
                        File.Copy(loadDb2, filepath);
                    }

                    else if (Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        var loadDb3 = Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                        // then save to Application.persistentDataPath
                        File.Copy(loadDb3, filepath);
                    }

                    else if (Application.platform == RuntimePlatform.OSXPlayer)
                    {
                        var loadDb4 = Application.dataPath + "/Resources/Data/StreamingAssets/" +
                                DatabaseName; // this is the path to your StreamingAssets in iOS
                        // then save to Application.persistentDataPath
                        File.Copy(loadDb4, filepath);
                    }
                    else
                    {
                        var loadDb5 = Application.dataPath + "/StreamingAssets/" + DatabaseName; // this is the path to your StreamingAssets in iOS
                        // then save to Application.persistentDataPath
                        File.Copy(loadDb5, filepath);
                    }

                    Debug.Log("Database written");
                }

                dbPath = filepath;
            }

            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Final PATH: " + dbPath);
        }

        /// <summary>
        /// 如果数据库中存在该表则不创建，如果不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int CreateRecord<T>()
        {
            var ii = _connection.CreateTable<T>();
            return ii;
        }

        public T InsertRecord<T>(T t)
        {
            _connection.Insert(t);
            return t;
        }
        public T InsertOrUpdateRecord<T>(T t) where T:BaseTable, new()
        {
            var tb=  this._connection.Table<T>().Where(_ => _.Id == t.Id);
            var count = tb.Count();
            if (count==0)
            {
                _connection.Insert(t);
            }
            else
            {
                _connection.Update(t);
            }
            return t;
        }
        public T InsertOrUpdateAutoIncrementRecord<T>(T t) where T:BaseTableAutoIncrement, new()
        {
            var tb=  this._connection.Table<T>().Where(_ => _.Id == t.Id);
            var count = tb.Count();
            if (count==0)
            {
                _connection.Insert(t);
            }
            else
            {
                _connection.Update(t);
            }
            return t;
        }
        public T UpdateRecord<T>(T t)
        {
            _connection.Update(t);
            return t;
        }
        
        /// <summary>
        /// 检测表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool CheckTableExists(string tableName)
        {
            string sql = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@TableName";
            var cmd = this._connection.CreateCommand(sql);
            cmd.Bind("@TableName", tableName);
            int count = cmd.ExecuteScalar<int>();
            return count > 0;
        }
    }
}