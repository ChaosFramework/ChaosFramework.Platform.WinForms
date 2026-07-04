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
        Form MakeAGoodForm(string title)
        {
            Form form = new Form();
            form.Name = title;
            form.Show();
            form.FormClosing += RaiseTerminate;
            return form;
        }

        Window PlatformContext.CreateWindow(string title)
            => new WinFormsWindow(MakeAGoodForm(title));

        Fullscreen PlatformContext.CreateFullscreen(string title, Monitor monitor)
            => CreateFullscreen(title, monitor as WinFormsMonitor ?? throw new ArgumentException($"Monitor must be a {nameof(WinFormsMonitor)}."));

        public WinFormsFullscreen CreateFullscreen(string title, WinFormsMonitor monitor)
            => new WinFormsFullscreen(MakeAGoodForm(title), monitor);

        Overhead PlatformContext.messageQueue => Overhead;

        GlContext PlatformContext.glContext => this;

        public WinFormsMonitor PrimaryMontior => new WinFormsMonitor(Screen.PrimaryScreen);
        Monitor PlatformContext.PrimaryMonitor => PrimaryMontior;

        public event Action Terminate;

        void GlContext.Init() { }

        void Overhead()
        {
            Application.DoEvents();
            Cursor.Hide();
            Cursor.Position = Screen.PrimaryScreen.Bounds.Location;
        }

        void RaiseTerminate(object _, EventArgs __)
            => Terminate?.Invoke();

        public IEnumerable<WinFormsMonitor> EnumerateMonitors()
            => Screen.AllScreens.Select(s => new WinFormsMonitor(s));

        IEnumerable<Monitor> PlatformContext.EnumerateMonitors()
            => EnumerateMonitors();
    }
}
