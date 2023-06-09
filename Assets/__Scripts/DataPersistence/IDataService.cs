public interface IDataService
{
    void InitializeData<T>(string RelativePath, T Data, bool Encrypted);
    void SaveData<T>(string RelativePath, T Data, bool Encrypted);
    T LoadData<T>(string RelativePath, bool Encrypted);
}
