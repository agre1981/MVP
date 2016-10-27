using System.ComponentModel;

namespace MVP.UI
{
    public interface IService
    {
        BindingList<Model> GetData(long Id);
        void SaveData(BindingList<Model> data);
    }
}
