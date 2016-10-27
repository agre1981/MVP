using MVP.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MVP.UI
{
    public partial class View : Form, IView
    {
        public event Action FormClose;
        public event Action FormLoad;
        public event Action<string, Model> GridCellValueChanged;
        public event Action<string, ValidateEditorEventArgs> GridValidatingEditor;
        public event Action<Model> IncreaseAClick;
        public event Action SaveClick;

        public View()
        {
            InitializeComponent();
            if (!DesignMode)
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
                    var row = dataGridView1.CurrentRow;
                    if (row != null)
                    {
                        var model = (Model)row.DataBoundItem;
                        IncreaseAClick(model);
                    };
                }
                finally
                {
                }
            };

            this.saveButton.Click += (s, args) => {
                dataGridView1.EndEdit();
                SaveClick();
            };

            this.dataGridView1.CellValueChanged += (s, args) =>
            {
                try
                {
                    if (args.RowIndex >= 0)
                    {
                        var gridColumn = dataGridView1.Columns[args.ColumnIndex];
                        var gridRow = dataGridView1.Rows[args.RowIndex];
                        var model = (Model)gridRow.DataBoundItem;
                        GridCellValueChanged(gridColumn.DataPropertyName, model);
                    }
                }
                finally
                {
                }
            };

            dataGridView1.CellValidating += (s, args) =>
            {
                var gridColumn = dataGridView1.Columns[args.ColumnIndex];
                object value = dataGridView1.CurrentCell.Value;
                value = dataGridView1.Rows[args.RowIndex].Cells[args.ColumnIndex];
                var eventArgs = new ValidateEditorEventArgs() { Valid = true, ErrorText = string.Empty, Value = value };
                GridValidatingEditor(gridColumn.DataPropertyName, eventArgs);

                dataGridView1.Rows[args.RowIndex].Cells[args.ColumnIndex].ErrorText = eventArgs.ErrorText;
                args.Cancel = !eventArgs.Valid;
            };

        }

        public void BindModel(BindingList<Model> model)
        {
            modelBindingSource.DataSource = model;
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            e.ThrowException = false;
        }
    }
}
