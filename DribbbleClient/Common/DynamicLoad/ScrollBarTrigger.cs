using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace DribbbleClient.Common.DynamicLoad
{
    public enum DirectionType { Top, Bottom, Left, Right }
    public class ScrollBarTrigger : TriggerBase<DependencyObject>
    {
        ScrollBar ScrollView;
        public static readonly DependencyProperty DirectionTypeProperty = DependencyProperty.Register("DirectionType", typeof(DirectionType), typeof(ScrollBarTrigger), new PropertyMetadata(DirectionType.Bottom));

        public DirectionType DirectionType
        {
            get
            {
                return (DirectionType)base.GetValue(ScrollBarTrigger.DirectionTypeProperty);
            }
            set
            {
                base.SetValue(ScrollBarTrigger.DirectionTypeProperty, value);
            }

        }

        public event EventHandler ScrollTrigger;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null && this.AssociatedObject is FrameworkElement)
            {
                (this.AssociatedObject as FrameworkElement).SizeChanged += control_SizeChanged;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (ScrollView != null)
                ScrollView.ValueChanged -= ScrollView_ValueChanged;

            if (this.AssociatedObject != null && this.AssociatedObject is FrameworkElement)
            {
                (this.AssociatedObject as FrameworkElement).SizeChanged -= control_SizeChanged;
            }
        }

        void control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.AssociatedObject == null || !(this.AssociatedObject is FrameworkElement))
                return;

            ScrollBar Scroll = this.AssociatedObject.GetFirstDescendantOfType<ScrollBar>();
            if (Scroll != null)
            {
                AttachedScroll(Scroll);
                (this.AssociatedObject as FrameworkElement).SizeChanged -= control_SizeChanged;
            }
        }

        void AttachedScroll(ScrollBar Scroll)
        {
            if (Scroll != null)
            {
                ScrollView = Scroll;
                ScrollView.ValueChanged += ScrollView_ValueChanged;
            }
        }

        void ScrollView_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnOffsetChanged();
        }

        void OnOffsetChanged()
        {
            if (ScrollView == null)
                return;

            ScrollBar Scroll = ScrollView;

            switch (DirectionType)
            {
                case DirectionType.Top:
                case DirectionType.Left:
                    {
                        if (Scroll.Maximum < double.Epsilon || Scroll.Value - Scroll.ViewportSize > 0)
                            return;
                    }
                    break;
                case DirectionType.Bottom:
                case DirectionType.Right:
                    {
                        //部分控件第一次加载数据时Value会和Maximum值相等，会触发命令的执行。增加Scroll.Value == Scroll.Maximum将这种情况去除
                        if (Scroll.Maximum < double.Epsilon || Scroll.Value + Scroll.ViewportSize < Scroll.Maximum || Scroll.Value == Scroll.Maximum)
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
    }
}
