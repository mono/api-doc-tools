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

        #region Negative example (the property type ends with "Event", but the name doesn't)

        public static readonly DependencyProperty IsBubbleSource3 = DependencyProperty.RegisterAttached(
  "IsBubbleSource3",
  typeof(Boolean),
  typeof(AquariumObject),
  null
);

        public static void SetIsBubbleSource3(UIElement element, Boolean value)
        {
            element.SetValue(IsBubbleSourceProperty, value);
        }

        public static Boolean GetIsBubbleSource3(UIElement element)
        {
            return (Boolean)element.GetValue(IsBubbleSourceProperty);
        }

        #endregion
    }

}