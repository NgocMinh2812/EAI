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

            bool blurredOwner = false;
            try
            {
                if (gvWindow != null && gvWindow.IsLoaded && gvWindow.IsVisible)
                {
                    gvWindow.ShowModalOverlay();
                }

                // Nếu owner là một dialog khác (không phải cửa sổ chính GiangVienWindow), làm mờ chính dialog owner
                if (owner != null && owner != gvWindow && owner.IsLoaded && owner.IsVisible)
                {
                    owner.Effect = new BlurEffect { Radius = 6, RenderingBias = RenderingBias.Performance };
                    blurredOwner = true;
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
                    if (blurredOwner && owner != null)
                    {
                        owner.Effect = null;
                    }

                    if (gvWindow != null)
                    {
                        gvWindow.HideModalOverlay();
                    }
                }
                catch { }
            }
        }
    }
}
