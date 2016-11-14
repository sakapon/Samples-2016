using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace DiceRotationWpf
{
    public class EventsExtension<TElement> where TElement : UIElement
    {
        public TElement Target { get; }

        Point MouseDragLastPoint;
        public IObservable<DeltaInfo> MouseDragDelta { get; }

        public EventsExtension(TElement target)
        {
            Target = target;

            // Replaces events with IObservable objects.
            var mouseDown = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseDown)).Select(e => e.EventArgs);
            var mouseMove = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseMove)).Select(e => e.EventArgs);
            var mouseUp = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseUp)).Select(e => e.EventArgs);
            var mouseLeave = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseLeave)).Select(e => e.EventArgs);
            var mouseDownEnd = mouseUp.Merge(mouseLeave);

            MouseDragDelta = mouseDown
                .Select(GetSlidedPosition)
                .Do(p => MouseDragLastPoint = p)
                .SelectMany(p0 => mouseMove
                    .TakeUntil(mouseDownEnd)
                    .Select(e => new DeltaInfo { Start = MouseDragLastPoint, End = GetSlidedPosition(e) })
                    .Do(_ => MouseDragLastPoint = _.End));
        }

        Point GetSlidedPosition(MouseEventArgs e) => e.GetPosition(Target) - (Vector)Target.RenderSize / 2;
    }

    public class EventsExtensionForTrackball<TElement> where TElement : UIElement
    {
        public TElement Target { get; }

        Point MouseDragLastPoint;
        public IObservable<DeltaInfo> MouseDragDelta { get; }

        public EventsExtensionForTrackball(TElement target)
        {
            Target = target;

            // Replaces events with IObservable objects.
            var mouseEnter = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseEnter)).Select(e => e.EventArgs);
            var mouseMove = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseMove)).Select(e => e.EventArgs);
            var mouseLeave = Observable.FromEventPattern<MouseEventArgs>(Target, nameof(UIElement.MouseLeave)).Select(e => e.EventArgs);

            MouseDragDelta = mouseEnter
                .Select(e => e.GetPosition(Target))
                .Do(p => MouseDragLastPoint = p)
                .SelectMany(p0 => mouseMove
                    .TakeUntil(mouseLeave)
                    .Select(e => new DeltaInfo { Start = MouseDragLastPoint, End = e.GetPosition(Target) })
                    .Do(_ => MouseDragLastPoint = _.End));
        }
    }

    public struct DeltaInfo
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }
}
