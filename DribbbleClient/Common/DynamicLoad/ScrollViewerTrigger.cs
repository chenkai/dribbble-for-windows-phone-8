using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System;

namespace DribbbleClient.Common.DynamicLoad
{
    public class ScrollViewerTrigger : TriggerBase<DependencyObject>
    {
        ScrollViewer ScrollView;
        public static readonly DependencyProperty DirectionTypeProperty = DependencyProperty.Register("DirectionType", typeof(DirectionType), typeof(ScrollViewerTrigger), new PropertyMetadata(DirectionType.Bottom));

        public DirectionType DirectionType
        {
            get
            {
                return (DirectionType)base.GetValue(ScrollViewerTrigger.DirectionTypeProperty);
            }
            set
            {
                base.SetValue(ScrollViewerTrigger.DirectionTypeProperty, value);
            }
        }

        public event EventHandler ScrollTrigger;

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(ScrollViewerTrigger), new PropertyMetadata(0.0, new PropertyChangedCallback(VerticalOffsetPropertyChanged)));

        public double VerticalOffset
        {
            get
            {
                return (double)base.GetValue(ScrollViewerTrigger.VerticalOffsetProperty);
            }
            set
            {
                base.SetValue(ScrollViewerTrigger.VerticalOffsetProperty, value);
            }

        }

        public static void VerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as ScrollViewerTrigger;
            if (behavior != null)
                behavior.OnVerticalOffsetChanged();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null && this.AssociatedObject is FrameworkElement)
            {
                (this.AssociatedObject as FrameworkElement).SizeChanged += control_SizeChanged;
            }
        }

        void control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.AssociatedObject == null || !(this.AssociatedObject is FrameworkElement))
                return;

            ScrollViewer Scroll = this.AssociatedObject.GetFirstDescendantOfType<ScrollViewer>();
            if (Scroll != null)
            {
                AttachedScroll(Scroll);
                (this.AssociatedObject as FrameworkElement).SizeChanged -= control_SizeChanged;
            }
        }

        void AttachedScroll(ScrollViewer Scroll)
        {
            if (Scroll == null)
                return;
            ScrollView = Scroll;

            Binding binding = new Binding();
            binding.Source = Scroll;
            binding.Path = new PropertyPath("VerticalOffset");

            BindingOperations.SetBinding(this, ScrollViewerTrigger.VerticalOffsetProperty, binding);

        }

        void OnVerticalOffsetChanged()
        {
            ScrollViewer Scroll = ScrollView;
            if (Scroll == null)
                return;

            switch (DirectionType)
            {
                case DirectionType.Top:
                    {
                        if (Scroll.ScrollableHeight < double.Epsilon || Scroll.VerticalOffset > double.Epsilon)
                            return;
                    }
                    break;
                case DirectionType.Bottom:
                    {
                        if (Scroll.ScrollableHeight < double.Epsilon || Scroll.VerticalOffset + Scroll.ExtentHeight - Scroll.ScrollableHeight < Scroll.ScrollableHeight)
                            return;
                    }
                    break;
                case DirectionType.Left:
                    {
                        if (Scroll.ScrollableWidth < double.Epsilon || Scroll.HorizontalOffset > double.Epsilon)
                            return;
                    }
                    break;
                case DirectionType.Right:
                    {
                        if (Scroll.ScrollableWidth < double.Epsilon || Scroll.HorizontalOffset < Scroll.ScrollableWidth)
                            return;
                    }
                    break;
                default: break;
            }

            if (ScrollTrigger != null)
            {
                ScrollTrigger(this.AssociatedObject, new EventArgs());
            }
            base.InvokeActions(null);

        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
