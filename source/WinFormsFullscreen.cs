
using ChaosFramework.Math.Vectors;
using OpenTK.GLControl;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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

        void PresentationContext.SetIcon(ApplicationIcon icon)
        {
            switch (icon.format)
            {
                case ApplicationIcon.IconFormat.ico:
                    using (Stream str = icon.getStream())
                        SetIcon(new Icon(str));
                    return;
                default:
                    throw new ArgumentException($"Unsupported format {icon.format}", nameof(icon));
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
