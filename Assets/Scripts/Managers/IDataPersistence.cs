using Assets.Scripts.Data;

namespace Assets.Scripts.Managers
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);

        void SaveData(GameData gameData);
    }
}
