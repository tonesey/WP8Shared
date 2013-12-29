using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Wp8Shared.Helpers
{
    public class MyVisualTreeHelper
    {
        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                var childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child);

                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        public static IEnumerable<T> FindChilds<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                var parentType = parent as T;
                if (parentType != null)
                {
                    yield return (T)parent;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    foreach (T child in FindChilds<T>(VisualTreeHelper.GetChild(parent, i)))
                    {
                        if (child != null)
                        {
                            yield return child;
                        }
                    }
                }
            }
            yield break;
        }

    }
}
