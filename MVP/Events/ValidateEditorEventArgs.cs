using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVP.Events
{
    public class ValidateEditorEventArgs : EventArgs
    {
        public bool Valid { get; set; }
        public string ErrorText { get; set; }
        public object Value { get; set; }
    }
}
