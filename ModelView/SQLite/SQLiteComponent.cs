namespace ET.Client
{
    [ComponentOf]
    public class SQLiteComponent : Entity, IAwake
    {
        public DataService DataService { get; set; }
        public SoundTable SoundTable { get; set; }
    }
}