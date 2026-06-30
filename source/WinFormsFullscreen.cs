using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChaosFramework.Math.Vectors;
using OpenTK.GLControl;

namespace ChaosFramework.Platform.WinForms
{
    public class WinFormsFullscreen : Fullscreen
    {
        public readonly WinFormsMonitor monitor;
        public readonly Form form;
        readonly GLControl control;

        uint PresentationContext.width => (uint)form.Width;
        uint PresentationContext.height => (uint)form.Height;

        Monitor Fullscreen.monitor => monitor;

        public string title { get => form.Name; set => form.Name = value; }

        Vector2i PresentationContext.position => Vector2i.EMPTY;

        public WinFormsFullscreen(Form form, WinFormsMonitor monitor)
        {
            this.monitor = monitor;
            this.form = form;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Bounds = monitor.screen.Bounds;
            control = new GLControl();
            control.Bounds = form.ClientRectangle;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            form.Controls.Add(control);
        }

        void PresentationContext.SetIcon(IEnumerable<Stream> sources)
        {
            foreach (Stream candidate in sources)
                try
                {
                    using (Icon ico = new Icon(candidate))
                        SetIcon(ico);
                    return;
                }
                catch
                {
                    // TODO: figure out what makes sense to actually catch here
                }
        }

        public void SetIcon(Icon icon)
            => form.Icon = icon;

        void PresentationContext.Present()
        {
            control.Context.MakeCurrent();
            control.SwapBuffers();
        }
    }
}
