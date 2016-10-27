using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MVP.UI
{
    public partial class View : Form, IView
    {
        #region Events Implementation

        public event Action FormLoad;
        public event Action FormClose;
        public event Action<Model> IncreaseAClick;
        public event Action SaveClick;
        public event Action<string, Model> GridCellValueChanged;
        public event Action<string, BaseContainerValidateEditorEventArgs> GridValidatingEditor;

        #endregion

        #region View Initialization

        public View()
        {
            InitializeComponent();

            if ( !DesignMode )
            {
                InitEvents();
            }
        }

        private void InitEvents()
        {
            this.Load += (s, args) => FormLoad();
            this.FormClosed += (s, args) => FormClose(); 

            this.increaseAButton.Click += (s, args) => {
                try
                {
                    // Suspend UI grid updates for batch operation. See details on https://documentation.devexpress.com/#WindowsForms/CustomDocument773
                    gridView1.BeginUpdate();
                    if (gridView1.IsValidRowHandle(gridView1.FocusedRowHandle))
                    {
                        var model = (Model)gridView1.GetRow(gridView1.FocusedRowHandle);
                        IncreaseAClick(model);
                    };
                }
                finally
                {
                    gridView1.EndUpdate();
                }
            };

            this.saveButton.Click += (s, args) => {
                gridView1.CloseEditor();
                SaveClick();
            };

            this.gridView1.CellValueChanged += (s, args) =>
            {
                try
                {
                    // Suspend UI grid updates for batch operation. See details on https://documentation.devexpress.com/#WindowsForms/CustomDocument773
                    gridView1.BeginUpdate();
                    if (gridView1.IsValidRowHandle(gridView1.FocusedRowHandle))
                    {
                        var model = (Model)gridView1.GetRow(gridView1.FocusedRowHandle);
                        GridCellValueChanged(args.Column.FieldName, model);
                    }
                }
                finally
                {
                    gridView1.EndUpdate();
                }
            };

            gridView1.ValidatingEditor += (s, args) => GridValidatingEditor(gridView1.FocusedColumn.FieldName, args);

        }

        private void gridView1_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.DisplayError;
        }

        #endregion

        #region IView implementation

        public void BindModel(BindingList<Model> model)
        {
            modelBindingSource.DataSource = model;
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion



    }
}
