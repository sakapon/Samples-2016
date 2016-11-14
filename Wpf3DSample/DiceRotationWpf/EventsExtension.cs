using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Input;

namespace DiceRotationWpf
{
    public class EventsExtension<TElement> where TElement : UIElement
    {
        public TElement Target { get; }

        Point MouseDragLastPoint;
        Subject<Vector> _MouseDragDelta = new Subject<Vector>();
        public IObservable<Vector> MouseDragDelta => _MouseDragDelta;

        public EventsExtension(TElement target)
        {
            Target = target;

            // Replaces events with IObservable objects.
            var mouseDown = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseDown)).Select(e => e.EventArgs);
            var mouseMove = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseMove)).Select(e => e.EventArgs);
            var mouseUp = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseUp)).Select(e => e.EventArgs);
            var mouseLeave = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseLeave)).Select(e => e.EventArgs);
            var mouseDownEnd = mouseUp.Merge(mouseLeave);

            mouseDown
                .Select(e => e.GetPosition(Target))
                .Do(p => MouseDragLastPoint = p)
                .Subscribe(p0 => mouseMove
                    .TakeUntil(mouseDownEnd)
                    .Select(e => new { p1 = MouseDragLastPoint, p2 = e.GetPosition(Target) })
                    .Do(_ => MouseDragLastPoint = _.p2)
                    .Do(_ => _MouseDragDelta.OnNext(_.p2 - _.p1))
                    .Subscribe());
        }
    }
}
