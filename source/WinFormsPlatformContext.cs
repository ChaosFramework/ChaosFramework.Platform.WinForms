using ChaosFramework.Math.Vectors;
using OpenTK.GLControl;
using System;
using System.Windows.Forms;

namespace ChaosFramework.Platform.WinForms
{
    public class WinFormsPlatformContext
        : PlatformContext
        , GlContext
    {
        class FullscreenForm : Fullscreen
        {
            public readonly Form form;
            readonly GLControl control;

            uint PresentationContext.width => (uint)form.Width;
            uint PresentationContext.height => (uint)form.Height;

            Monitor Fullscreen.monitor => throw new NotImplementedException();

            public string title { get => form.Name; set => form.Name = value; }

            Vector2i PresentationContext.position => Vector2i.EMPTY;

            public FullscreenForm(Form form)
            {
                this.form = form;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Bounds = Screen.PrimaryScreen.Bounds;
                control = new GLControl();
                control.Bounds = form.ClientRectangle;
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                form.Controls.Add(control);
            }

            void PresentationContext.Present()
            {
                control.Context.MakeCurrent();
                control.SwapBuffers();
            }
        }

        public Form GetForm(PresentationContext window) => (window as FullscreenForm)?.form;

        Window PlatformContext.CreateWindow(string title)
            => throw new NotSupportedException();

        Fullscreen PlatformContext.CreateFullscreen(string title)
        {
            var form = new Form();
            form.Name = title;
            var window = new FullscreenForm(form);
            form.Show();
            form.FormClosing += RaiseTerminate;
            return window;
        }

        Overhead PlatformContext.messageQueue => Overhead;

        GlContext PlatformContext.glContext => this;

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
    }
}
