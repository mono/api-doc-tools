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


        public static readonly DependencyProperty IsDuplicatedProperty = DependencyProperty.RegisterAttached(
  "IsDuplicated",
  typeof(Boolean),
  typeof(AquariumObject),
  null
);

        public static void SetIsDuplicated(UIElement element, Boolean value)
        {
            element.SetValue(IsDuplicatedProperty, value);
        }

        public static Boolean GetIsDuplicated(UIElement element)
        {
            return (Boolean)element.GetValue(IsDuplicatedProperty);
        }
        public static bool IsDuplicated {get;set;}
        
        #endregion

        #region example (no get method)

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

        public static readonly DependencyProperty P = DependencyProperty.RegisterAttached(
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