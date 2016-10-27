using MVP.Events;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MVP.UI
{
    public class Presenter
    {
        private IView view;
        private IService service;
        private BindingList<Model> model;

        #region Initialization

        public Presenter(IView view, IService service)
        {
            if (view == null) throw new ArgumentException("Argument view can not be null");
            if (service == null) throw new ArgumentException("Argument service can not be null");

            this.view = view;
            this.service = service;

            SubscribeEvents(view);
        }

        private void SubscribeEvents(IView view)
        {
            view.FormLoad += view_FormLoad;
            view.FormClose += view_FormClose;
            view.IncreaseAClick += view_IncreaseAClick;
            view.GridCellValueChanged += view_GridCellValueChanged;
            view.GridValidatingEditor += view_GridValidatingEditor;
            view.SaveClick += view_SaveClick;
        }

        private void UnSubscribeEvents(IView view)
        {
            view.FormLoad -= view_FormLoad;
            view.FormClose -= view_FormClose;
            view.IncreaseAClick -= view_IncreaseAClick;
            view.GridCellValueChanged -= view_GridCellValueChanged;
            view.GridValidatingEditor -= view_GridValidatingEditor;
            view.SaveClick -= view_SaveClick;
        }

        #endregion 

        #region View Events Handlers

        private void view_FormLoad()
        {
            model = service.GetData(1L);
            view.BindModel(model);
        }

        private void view_FormClose()
        {
            UnSubscribeEvents(view);

            // Clear resources
            view = null;
            service = null;
            model = null;
        }

        private void view_IncreaseAClick(Model entity)
        {
            entity.A++;
            // start BL by event in view (not by changing property in a model!!!)
            CalculateSum(entity);
            CalculateDiv(entity);
        }

        private void CalculateSum(Model entity)
        {
            entity.Sum = entity.A + entity.B;
        }

        private void CalculateDiv(Model entity)
        {
            entity.Div = entity.A / entity.B;
        }

        private void view_GridCellValueChanged(string fieldName, Model entity)
        {
            if (fieldName == "A" || fieldName == "B")
            {
                // start BL by event in view (not by changing property in a model!!!)
                CalculateSum(entity);
                CalculateDiv(entity);
            }
        }

        private void view_GridValidatingEditor(string fieldName, ValidateEditorEventArgs e)
        {
            if(fieldName == "B")
            {
                long value;
                if (e.Value == null || e.Value.ToString().Trim().Length == 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Field B should not be empty !";
                }
                else if (long.TryParse(e.Value.ToString(), out value))
                {
                    if (value == 0)
                    {
                        e.Valid = false;
                        e.ErrorText = "Field B should not be equal 0 !";
                    }
                }
                else
                {
                    e.Valid = false;
                    e.ErrorText = "Field B should be a number !";
                }
            }
        }

        private void view_SaveClick()
        {
            try
            {
                service.SaveData(model);
            }
            catch (ServiceException ex)
            {
                view.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Internal Members

        internal static long Sum(long a, long b)
        {
            return a + b;
        }

        #endregion
    }
}
