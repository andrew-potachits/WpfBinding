using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WpfBinding
{
    public class RebarSet : IEnumerable<LineDef>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly List<LineDef> _rebars = new List<LineDef>();
        public int Count { get { return _rebars.Count; } }
        public IEnumerator<LineDef> GetEnumerator()
        {
            return _rebars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _rebars.Clear();
            OnPropertyChanged("Count");
            RaiseCollectionChangedEvent();
        }

        private void RaiseCollectionChangedEvent()
        {
            NotifyCollectionChangedEventHandler @event = CollectionChanged;
            if (@event != null)
                @event.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Generate(Rect bounds, int count)
        {
            int oldCount = _rebars.Count;
            _rebars.Clear();            
            _rebars.AddRange(
                Enumerable
                    .Range(0, count)
                    .Select(i => new LineDef
                                     {
                                         From = new Point(bounds.Left + bounds.Width/count*i, bounds.Top),
                                         To = new Point(bounds.Left + bounds.Width/count*i, bounds.Bottom)
                                     }));
            RaiseCollectionChangedEvent();
            if (oldCount != count)
            {
                OnPropertyChanged("Count");
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}