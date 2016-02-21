using System;
using System.Reflection;
using System.Windows;
using Microsoft.Phone.Controls;

namespace VisibleBoundsWP8Sample
{
    /// <summary>
    /// Extension methods for accessing new VisibleBound property and VisibleBoundsChanged event
    /// on <see cref="PhoneApplicationPage"/> in Windows Phone 8.1 Update 1.
    /// </summary>
    public static class VisibleBoundsExtensions
    {
        /// <summary>
        /// Get the VisibleBounds value from provided <see cref="PhoneApplicationPage"/>
        /// </summary>
        /// <param name="page">Target page.</param>
        /// <returns>Current Visible Bounds value.</returns>
        /// <exception cref="NotSupportedException">Thrown if application is running on older version of
        /// Windows Phone than Windows Phone 8.1 Update 1.</exception>
        public static Thickness GetVisibleBounds(this PhoneApplicationPage page)
        {
            EnsureVersion();

            return (Thickness)page.GetType().GetProperty("VisibleBounds").GetValue(page);
        }

        /// <summary>
        /// Attach provided <paramref name="handler"/> to VisibleBoundsChanged event.
        /// </summary>
        /// <param name="page">Target page.</param>
        /// <param name="handler">Delegate to attach to the event.</param>
        /// <exception cref="NotSupportedException">Thrown if application is running on older version of
        /// Windows Phone than Windows Phone 8.1 Update 1.</exception>
        public static void VisibleBoundsChangedAdd(this PhoneApplicationPage page, EventHandler handler)
        {
            EnsureVersion();

            EventInfo eventInfo = page.GetType().GetEvent("VisibleBoundsChanged");
            Delegate convertedHandler = Delegate.CreateDelegate(
                eventInfo.EventHandlerType, handler.Target, handler.Method);
            eventInfo.AddEventHandler(page, convertedHandler);
        }

        /// <summary>
        /// Detach provided <paramref name="handler"/> from VisibleBoundsChanged event.
        /// </summary>
        /// <param name="page">Target page.</param>
        /// <param name="handler">Delegate to detach from the event.</param>
        /// <exception cref="NotSupportedException">Thrown if application is running on older version of
        /// Windows Phone than Windows Phone 8.1 Update 1.</exception>
        public static void VisibleBoundsChangedRemove(this PhoneApplicationPage page, EventHandler handler)
        {
            EnsureVersion();

            EventInfo eventInfo = page.GetType().GetEvent("VisibleBoundsChanged");
            Delegate convertedHandler = Delegate.CreateDelegate(
                eventInfo.EventHandlerType, handler.Target, handler.Method);
            eventInfo.RemoveEventHandler(page, convertedHandler);
        }

        /// <summary>
        /// Returns true if this extension methods are supported on current version of Windows Phone OS.
        /// </summary>
        public static bool IsSupported 
        {
            get { return Environment.OSVersion.Version >= wp81Update1Version; }
        }
        private static readonly Version wp81Update1Version = new Version(8, 10, 14141);

        private static void EnsureVersion()
        {
            if (!IsSupported)
            {
                throw new NotSupportedException("VisibleBounds is only available in WP8.1 Update 1 and newer.");
            }
        }
    }
}