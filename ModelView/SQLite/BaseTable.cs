using SQLite4Unity3d;

namespace ET.Client
{
    [EnableClass]
    public class BaseTable
    {
        [PrimaryKey]
        public int Id { get; set; }
    }
    [EnableClass]
    public class BaseTableAutoIncrement
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        
    }
}