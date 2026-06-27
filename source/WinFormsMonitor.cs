
using ChaosFramework.Math.Vectors;
using System.Windows.Forms;

namespace ChaosFramework.Platform.WinForms
{
    public class WinFormsMonitor : Monitor
    {
        internal readonly Screen screen;

        internal WinFormsMonitor(Screen screen) {
            this.screen = screen;
        }

        public uint width => (uint)screen.Bounds.Width;

        public uint height => (uint)screen.Bounds.Height;

        public Vector2i position => screen.Bounds.Location;

        public string deviceName => screen.DeviceName;
    }
}