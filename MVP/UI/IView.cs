using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;

namespace MVP.UI
{
    public interface IView
    {
        event Action FormLoad;
        event Action FormClose;
        event Action<Model> IncreaseAClick;
        event Action SaveClick;
        event Action<string, Model> GridCellValueChanged;
        event Action<string, BaseContainerValidateEditorEventArgs> GridValidatingEditor;

        void BindModel(BindingList<Model> model);
        void ShowErrorMessage(string message);
    }
}
