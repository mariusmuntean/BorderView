using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace BorderView
{
    public class ViewHelper
    {
        public static T GetTemplateChild<T>(Element parent, string name) where T : Element
        {
            if (parent == null)
                return null;
            var templateChild = parent.FindByName<T>(name);
            if (templateChild != null)
                return templateChild;
            foreach (var child in FindVisualChildren<Element>(parent, false))
            {
                templateChild = GetTemplateChild<T>(child, name);
                if (templateChild != null)
                    return templateChild;
            }

            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(Element element, bool recursive = true) where T : Element
        {
            if (element != null && element is Layout)
            {
                var childrenProperty = element.GetType().GetProperty("InternalChildren", BindingFlags.Instance | BindingFlags.NonPublic);
                if (childrenProperty != null)
                {
                    var children = (IEnumerable<Element>) childrenProperty.GetValue(element);
                    foreach (var child in children)
                    {
                        if (child != null && child is T)
                        {
                            yield return (T) child;
                        }

                        if (recursive)
                        {
                            foreach (T childOfChild in FindVisualChildren<T>(child))
                            {
                                yield return childOfChild;
                            }
                        }
                    }
                }
            }
        }
    }
}