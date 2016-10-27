using System.ComponentModel;

namespace MVP.UI
{
    public class Service : IService
    {
        public BindingList<Model> GetData(long Id)
        {
            var data = new BindingList<Model>() { 
                new Model(1, "Name 1", 2, 3, 5, 0), 
                new Model(2, "Name 2", 1, 1, 2, 1),
                new Model(3, "Name 3", 10, 2, 12, 5)
            };

            return data;
        }

        public void SaveData(BindingList<Model> data)
        {

        }
    }
}
