using System.Windows;

namespace AttachedEventsAndProperties
{
    public class AttachedEventExample
    {
        #region WPF decompiled example:

        public static readonly RoutedEvent DragOverEvent;

        public static void AddDragOverHandler(DependencyObject element, DragEventHandler handler)
        {
        }

        public static void RemoveDragOverHandler(DependencyObject element, DragEventHandler handler)
        {
        }
        #endregion 

        #region docs.microsoft.com example
        public static readonly RoutedEvent NeedsCleaningEvent;
        public static void AddNeedsCleaningHandler(DependencyObject d, RoutedEventHandler handler)
        {
        }

        public static void RemoveNeedsCleaningHandler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion 

        #region Negative example (no RemoveNeedsCleaning2Handler)
        public static readonly RoutedEvent NeedsCleaning2Event;
        public static void AddNeedsCleaning2Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion

        #region Negative example (no AddNeedsCleaning3Handler)
        public static readonly RoutedEvent NeedsCleaning3Event;
        public static void RemoveNeedsCleaning3Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion

        #region Negative example (protected methods)

        public static readonly RoutedEvent NeedsCleaning4Event;

        protected static void AddNeedsCleaning4Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }

        protected static void RemoveNeedsCleaning4Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion

        #region Negative example (non static)
        public readonly RoutedEvent NeedsCleaning5Event;

        public void AddNeedsCleaning5Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }

        public void RemoveNeedsCleaning5Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion


        #region Negative example (field's name doesn't end with "Event")
        public static readonly RoutedEvent NeedsCleaning6Event6;
        public static void AddNeedsCleaning6Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        public static void RemoveNeedsCleaning6Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion 

        #region Negative example (the event type ends with "Event", but the name doesn't)
        public static readonly RoutedEvent E;
        public static void AddNeedsCleaning7Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        public static void RemoveNeedsCleaning7Handler(DependencyObject d, RoutedEventHandler handler)
        {
        }
        #endregion 
    }
}
