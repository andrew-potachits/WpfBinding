using System.ComponentModel;
using System.Windows;

namespace WpfBinding
{
    public class LineBase : INotifyPropertyChanged
    {
        private Point _from;
        private Point _to;
        private bool _selected;
        private DragTypes _dragType = DragTypes.None;

        public string GroupName { get; set; }

        public DragTypes DragType
        {
            get { return _dragType; }
            set { _dragType = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                bool oldVal = _selected;
                _selected = value;
                if (oldVal != _selected)
                    OnPropertyChanged("Selected");
            }
        }

        public Point From
        {
            get { return _from; }
            set
            {
                Point oldFrom = _from;
                _from = value;
                if (oldFrom != _from)
                    OnPropertyChanged("From");
            }
        }

        public Point To
        {
            get { return _to; }
            set
            {
                Point oldTo = _to;
                _to = value;
                if (oldTo != _to)
                    OnPropertyChanged("To");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}