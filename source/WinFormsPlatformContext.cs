using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ChaosFramework.Platform.WinForms
{
    public class WinFormsPlatformContext
        : PlatformContext
        , GlContext
    {
        Window PlatformContext.CreateWindow(string title)
            => throw new NotImplementedException();

        Fullscreen PlatformContext.CreateFullscreen(string title, Monitor monitor)
            => CreateFullscreen(title, monitor as WinFormsMonitor ?? throw new ArgumentException($"Monitor must be a {nameof(WinFormsMonitor)}."));

        public WinFormsFullscreen CreateFullscreen(string title, WinFormsMonitor monitor)
        {
            Form form = new Form();
            form.Name = title;
            WinFormsFullscreen fullscreen = new WinFormsFullscreen(form, monitor);
            form.Show();
            void RaiseTerminate(object _, FormClosingEventArgs __)
            {
                form.FormClosing -= RaiseTerminate;
                Terminate?.Invoke();
            };
            form.FormClosing += RaiseTerminate;
            return fullscreen;
        }

        Overhead PlatformContext.messageQueue => Overhead;

        GlContext PlatformContext.glContext => this;

        public WinFormsMonitor PrimaryMonitor => new WinFormsMonitor(Screen.PrimaryScreen);
        Monitor PlatformContext.PrimaryMonitor => PrimaryMonitor;

        public event Action Terminate;

        void GlContext.Init() { }

        void Overhead()
        {
            Application.DoEvents();
            Cursor.Hide();
            Cursor.Position = Screen.PrimaryScreen.Bounds.Location;
        }

        public IEnumerable<WinFormsMonitor> EnumerateMonitors()
            => Screen.AllScreens.Select(s => new WinFormsMonitor(s));

        IEnumerable<Monitor> PlatformContext.EnumerateMonitors()
            => EnumerateMonitors();
    }
}
