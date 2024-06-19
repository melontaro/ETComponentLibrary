using SQLite4Unity3d;

namespace ET.Client
{
    [EnableClass]
    public class SoundTable : BaseTable
    {
        public string ConfigData { get; set; }

        [Ignore]
        public SoundData SoundData { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: Id={0},   Surname={1}]", Id, ConfigData);
        }
    }
}