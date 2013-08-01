using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DribbbleClient.Common.DynamicLoad
{
    public static class ElementExtension
    {
        public static PhoneApplicationPage GetParentPhonePage(this DependencyObject start)
        {
            if (start == null)
                return null;

            DependencyObject parent = start;
            DependencyObject current;

            do
            {
                current = parent;
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is PhoneApplicationPage)
                    return parent as PhoneApplicationPage;

            } while (parent != null);

            if (current is PhoneApplicationPage)
                return current as PhoneApplicationPage;

            return null;
        }


        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendantsOfType<T>().FirstOrDefault();
        }

        public static DependencyObject GetDescendantOfName(this DependencyObject start, string name)
        {
            if (start == null)
                return null;
            var Descendants = start.GetDescendants();

            if (Descendants == null || Descendants.Count() == 0)
                return null;

            var Descendant = Descendants.FirstOrDefault(x => x.GetValue(FrameworkElement.NameProperty).Equals(name));

            return Descendant;
        }

        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendants().OfType<T>();
        }

        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
        {
            if (start == null)
                yield break;

            var queue = new Queue<DependencyObject>();
            queue.Enqueue(start);
            yield return start;

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }
    }
}
