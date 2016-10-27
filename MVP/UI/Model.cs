using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MVP.UI
{
    public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private long id;
        private string name;
        private long a;
        private long b;
        private long sum;
        private long div;

        public Model(long id, string name, long a, long b, long sum, long div)
        {
            this.id = id;
            this.name = name;
            this.a = a;
            this.b = b;
            this.sum = sum;
            this.div = div;
        }

        public long Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public long A
        {
            get { return a; }
            set
            {
                a = value;
                RaisePropertyChanged(() => A);
            }
        }

        public long B
        {
            get { return b; }
            set
            {
                b = value;
                RaisePropertyChanged(() => B);
            }
        }

        public long Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                RaisePropertyChanged(() => Sum);
            }
        }

        public long Div
        {
            get { return div; }
            set
            {
                div = value;
                RaisePropertyChanged(() => Div);
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = Tools.GetPropertyName(propertyExpression);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
