using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WpfBinding
{
    public class RebarSet : IEnumerable<LineBase>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly List<LineBase> _rebars = new List<LineBase>();
        public int Count { get { return _rebars.Count - 4 - 3; } }
        public IEnumerator<LineBase> GetEnumerator()
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
            int oldCount = _rebars.Count - 4 - 3;
            _rebars.Clear();
            _rebars.AddRange(new[] {new LineBase
                                        {
                                            From = bounds.TopLeft,
                                            To = bounds.TopRight,
                                            Id = "BOX.0",
                                        },new LineBase
                                        {
                                            From = bounds.TopRight,
                                            To = bounds.BottomRight,
                                            Id = "BOX.1",
                                        }, new LineBase
                                        {
                                            From = bounds.BottomRight,
                                            To = bounds.BottomLeft,
                                            Id = "BOX.2",
                                        }, new LineBase
                                        {
                                            From = bounds.BottomLeft,
                                            To = bounds.TopLeft,
                                            Id = "BOX.3",
                                        }});
            _rebars.AddRange(
                Enumerable
                    .Range(0, count)
                    .Select(i => new LineBase
                                     {
                                         From = new Point(bounds.Left + bounds.Width/count*i, bounds.Top),
                                         To = new Point(bounds.Left + bounds.Width/count*i, bounds.Bottom),
                                         Id = string.Format("REBAR.VERTICAL.{0}", i),
                                     }));

            _rebars.AddRange(new []
                                 {
                                     new LineBase
                                         {
                                             From = new Point(bounds.Left, bounds.Top + 10),
                                             To = new Point(bounds.Right, bounds.Top + 10),
                                             Id = "REBAR.HORIZONTAL.0",
                                         },
                                     new LineBase
                                         {
                                             From = new Point(bounds.Left, bounds.Height/2),
                                             To = new Point(bounds.Right, bounds.Height/2),
                                             Id = "REBAR.HORIZONTAL.1",
                                         },
                                     new LineBase
                                         {
                                             From = new Point(bounds.Left, bounds.Bottom - 10),
                                             To = new Point(bounds.Right, bounds.Bottom - 10),
                                             Id = "REBAR.HORIZONTAL.2",
                                         }
                                 });
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