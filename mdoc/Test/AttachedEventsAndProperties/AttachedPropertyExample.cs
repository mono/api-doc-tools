using System;
using System.Windows;

namespace AttachedEventsAndProperties
{
    public static class AttachedPropertyExample
    {
        #region modified docs.microsoft.com example

        public static readonly DependencyProperty IsBubbleSourceProperty = DependencyProperty.RegisterAttached(
  "IsBubbleSource",
  typeof(Boolean),
  typeof(AquariumObject),
  null
);

        public static void SetIsBubbleSource(UIElement element, Boolean value)
        {
            element.SetValue(IsBubbleSourceProperty, value);
        }

        public static Boolean GetIsBubbleSource(UIElement element)
        {
            return (Boolean)element.GetValue(IsBubbleSourceProperty);
        }
        #endregion

        #region negative example (no get method)

        public static readonly DependencyProperty IsBubbleSource2Property = DependencyProperty.RegisterAttached(
  "IsBubbleSource2",
  typeof(Boolean),
  typeof(AquariumObject),
  null
);

        public static void SetIsBubbleSource2(UIElement element, Boolean value)
        {
            element.SetValue(IsBubbleSourceProperty, value);
        }
        #endregion
    }

}