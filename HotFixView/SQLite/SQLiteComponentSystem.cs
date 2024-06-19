using Newtonsoft.Json;

namespace ET.Client
{
    [EntitySystemOf(typeof(SQLiteComponent))]
    [FriendOf(typeof(SQLiteComponent))]
    public static partial class SQLiteComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.SQLiteComponent self)
        {
        }

        public static async ETTask Init(this SQLiteComponent self)
        {
            self.DataService = new DataService("coo.db");
            // var levelTable = self.DataService.Connection.Table<LevelTable>().First();
        }

        public static async ETTask<LevelDatas> GetLevelDataByMapId(this SQLiteComponent self, int mapId)
        {
            var data = self.DataService.Connection.Find<LevelTable>(o => o.Id == mapId);
            var ld = JsonConvert.DeserializeObject<LevelDatas>(data.ConfigData);
            return ld;
        }
        public static async ETTask<SoundTable> GetSoundConfig(this SQLiteComponent self)
        {

            var soundTableIsExit = self.DataService.CheckTableExists("SoundTable");
            if (soundTableIsExit==false)
            {
               var v= self.DataService.Connection.CreateTable<SoundTable>();
               var soundData = new SoundData() { MusicVolume = -10, EffectVolume = 0 };
               self.SoundTable = new SoundTable();
               self.SoundTable.Id = 0;
               self.SoundTable.SoundData = soundData;
               self.SoundTable.ConfigData=JsonConvert.SerializeObject(soundData);
               self.DataService.InsertOrUpdateRecord<SoundTable>( self.SoundTable);
            }
            else
            {
                self.SoundTable=  self.DataService.Connection.Find<SoundTable>(o => o.Id == 0);
                self.SoundTable.SoundData = JsonConvert.DeserializeObject<SoundData>( self.SoundTable.ConfigData);
            }

         
            return self.SoundTable;
        }
        
        public static async ETTask<SoundTable> SaveSoundConfig(this SQLiteComponent self,float music,float effect)
        {

            var soundTableIsExit = self.DataService.CheckTableExists("SoundTable");
            if (soundTableIsExit==false)
            {
               Log.Error("Sound Table 不存在!!");
            }
            else
            {
                self.SoundTable.SoundData.MusicVolume = music;
                self.SoundTable.SoundData.EffectVolume = effect;
                self.SoundTable.ConfigData=JsonConvert.SerializeObject(self.SoundTable.SoundData);
                self.DataService.InsertOrUpdateRecord<SoundTable>(self.SoundTable);
            }

         
            return self.SoundTable;
        }
    }
}