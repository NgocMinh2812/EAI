using System.Windows;
using System.Windows.Media.Effects;
using GUI.Views.GiangVien;

namespace GUI.Views.Common
{
    public static class ModalHelper
    {
        public static bool? ShowDialogWithDim(Window dialog, Window? owner = null)
        {
            owner ??= Application.Current?.MainWindow;
            if (owner != null && owner.IsLoaded && owner.IsVisible)
            {
                try { dialog.Owner = owner; } catch { }
            }

            GiangVienWindow? gvWindow = owner as GiangVienWindow ?? Application.Current?.MainWindow as GiangVienWindow;

            bool fallbackBlur = false;
            try
            {
                if (gvWindow != null && gvWindow.IsLoaded && gvWindow.IsVisible)
                {
                    gvWindow.ShowModalOverlay();
                }
                else if (owner != null && owner.IsLoaded && owner.IsVisible)
                {
                    owner.Effect = new BlurEffect { Radius = 8, RenderingBias = RenderingBias.Performance };
                    fallbackBlur = true;
                }
            }
            catch { }

            try
            {
                return dialog.ShowDialog();
            }
            finally
            {
                try
                {
                    if (gvWindow != null)
                    {
                        gvWindow.HideModalOverlay();
                    }
                    else if (owner != null && fallbackBlur)
                    {
                        owner.Effect = null;
                    }
                }
                catch { }
            }
        }
    }
}
