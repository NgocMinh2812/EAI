using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GUI
{
    public enum LucideIconKind
    {
        None = 0,
        Home,
        House,
        Target,
        BookOpen,
        Book,
        Books,
        Library,
        Pencil,
        Edit,
        Edit2,
        Edit3,
        PenTool,
        Clock,
        Timer,
        TrendingUp,
        LineChart,
        BarChart,
        BarChart2,
        BarChart3,
        PieChart,
        Bot,
        User,
        Users,
        UserCheck,
        UserPlus,
        UserX,
        GraduationCap,
        School,
        LogOut,
        LogIn,
        Check,
        CheckCircle,
        CheckCircle2,
        X,
        XCircle,
        Minus,
        MinusCircle,
        CircleMinus,
        Plus,
        PlusCircle,
        Star,
        Sparkles,
        Zap,
        Folder,
        FolderOpen,
        FolderPlus,
        FolderSync,
        FolderArchive,
        File,
        FileText,
        FileSpreadsheet,
        FileCode,
        FileCheck,
        FilePlus,
        FileEdit,
        Save,
        Download,
        Upload,
        Trash,
        Trash2,
        Search,
        RefreshCw,
        RotateCcw,
        Printer,
        Lock,
        Unlock,
        Key,
        Eye,
        EyeOff,
        Bell,
        Settings,
        Sliders,
        HelpCircle,
        Info,
        AlertTriangle,
        AlertCircle,
        Play,
        Pause,
        Volume2,
        VolumeX,
        Mic,
        MicOff,
        Send,
        Share2,
        Award,
        Trophy,
        Medal,
        Flame,
        Calendar,
        Mail,
        Phone,
        MapPin,
        Globe,
        ExternalLink,
        ChevronLeft,
        ChevronRight,
        ChevronUp,
        ChevronDown,
        ChevronsLeft,
        ChevronsRight,
        ArrowLeft,
        ArrowRight,
        ArrowUp,
        ArrowDown,
        Maximize,
        Minimize,
        Square,
        MoreHorizontal,
        MoreVertical,
        Filter,
        Copy,
        Cpu,
        Server,
        Database,
        Activity,
        Shield,
        ShieldCheck,
        ThumbsUp,
        ThumbsDown,
        MessageSquare,
        MessageCircle,
        Lightbulb,
        Headphones,
        Bookmark,
        Compass,
        Flag,
        Heart,
        Layers,
        Laptop,
        Monitor,
        Smartphone,
        Tablet,
        Music,
        Radio,
        Shuffle,
        Repeat,
        Tag,
        SlidersHorizontal,
        Terminal,
        Hash,
        Link,
        Paperclip,
        Clipboard,
        ClipboardCheck,
        ClipboardList,
        History,
        Glasses,
        Wand2,
        Brain,
        Workflow,
        Sprout,
        Leaf,
        Rocket,
        Crown,
        Camera,
        LayoutDashboard,
        LayoutGrid
    }

    /// <summary>
    /// Vector Lucide Icon control for WPF, rendering crisp 24x24 icons at any scale.
    /// Supports vector scaling, foreground inheritance, custom stroke thickness, and spin animation.
    /// </summary>
    public class LucideIcon : FrameworkElement
    {
        private static readonly ConcurrentDictionary<string, Geometry?> GeometryCache =
            new ConcurrentDictionary<string, Geometry?>(StringComparer.OrdinalIgnoreCase);

        #region Dependency Properties

        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register(
                nameof(Kind),
                typeof(LucideIconKind),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(LucideIconKind.None, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnKindChanged));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(string),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconNameChanged));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                nameof(Size),
                typeof(double),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(18.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(Brush),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(
                nameof(Fill),
                typeof(Brush),
                typeof(LucideIcon),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register(
                nameof(Spin),
                typeof(bool),
                typeof(LucideIcon),
                new PropertyMetadata(false, OnSpinChanged));

        #endregion

        #region CLR Properties

        public LucideIconKind Kind
        {
            get => (LucideIconKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public Brush? Stroke
        {
            get => (Brush?)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public Brush? Fill
        {
            get => (Brush?)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public bool Spin
        {
            get => (bool)GetValue(SpinProperty);
            set => SetValue(SpinProperty, value);
        }

        #endregion

        private Storyboard? _spinStoryboard;

        static LucideIcon()
        {
            // Default styling
        }

        public LucideIcon()
        {
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
        }

        private static void OnKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Trigger redraw
        }

        private static void OnIconNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Trigger redraw
        }

        private static void OnSpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LucideIcon icon)
            {
                icon.UpdateSpinAnimation((bool)e.NewValue);
            }
        }

        private void UpdateSpinAnimation(bool shouldSpin)
        {
            if (shouldSpin)
            {
                RenderTransformOrigin = new Point(0.5, 0.5);
                var rotate = new RotateTransform();
                RenderTransform = rotate;

                var anim = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1)))
                {
                    RepeatBehavior = RepeatBehavior.Forever
                };

                _spinStoryboard = new Storyboard();
                _spinStoryboard.Children.Add(anim);
                Storyboard.SetTarget(anim, this);
                Storyboard.SetTargetProperty(anim, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                _spinStoryboard.Begin();
            }
            else
            {
                _spinStoryboard?.Stop();
                _spinStoryboard = null;
                RenderTransform = null;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double w = Width;
            double h = Height;
            double s = Size;

            if (double.IsNaN(w) || w <= 0)
            {
                w = s > 0 ? s : 18.0;
            }
            if (double.IsNaN(h) || h <= 0)
            {
                h = s > 0 ? s : 18.0;
            }

            return new Size(w, h);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var geometry = ResolveGeometry();
            if (geometry == null) return;

            double actualW = RenderSize.Width;
            double actualH = RenderSize.Height;
            if (actualW <= 0 || actualH <= 0) return;

            Brush strokeBrush = Stroke ?? Foreground ?? Brushes.Black;
            double baseThickness = StrokeThickness > 0 ? StrokeThickness : 2.0;

            // Lucide icons are natively 24x24
            double scaleX = actualW / 24.0;
            double scaleY = actualH / 24.0;
            double scale = Math.Min(scaleX, scaleY);

            double offsetX = (actualW - (24.0 * scale)) / 2.0;
            double offsetY = (actualH - (24.0 * scale)) / 2.0;

            var pen = new Pen(strokeBrush, baseThickness)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round
            };
            pen.Freeze();

            dc.PushTransform(new TranslateTransform(offsetX, offsetY));
            dc.PushTransform(new ScaleTransform(scale, scale));
            dc.DrawGeometry(Fill, pen, geometry);
            dc.Pop();
            dc.Pop();
        }

        private Geometry? ResolveGeometry()
        {
            // 1. Try explicit Icon string property if provided
            if (!string.IsNullOrWhiteSpace(Icon))
            {
                return LucideIcons.GetGeometry(Icon);
            }

            // 2. Try Kind enum property
            if (Kind != LucideIconKind.None)
            {
                return LucideIcons.GetGeometry(Kind);
            }

            return null;
        }
    }

    public static class LucideIcons
    {
        private static readonly ConcurrentDictionary<string, Geometry?> Cache =
            new ConcurrentDictionary<string, Geometry?>(StringComparer.OrdinalIgnoreCase);

        public static Geometry? GetGeometry(LucideIconKind kind)
        {
            string name = KindToKebabName(kind);
            return GetGeometry(name);
        }

        public static Geometry? GetGeometry(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            return Cache.GetOrAdd(name, key =>
            {
                string? pathData = LucideIconData.GetPathData(key);
                if (string.IsNullOrWhiteSpace(pathData))
                {
                    // Fallback: check kebab case
                    string kebab = ToKebabCase(key);
                    pathData = LucideIconData.GetPathData(kebab);
                }

                if (string.IsNullOrWhiteSpace(pathData)) return null;

                try
                {
                    var geom = Geometry.Parse(pathData);
                    geom.Freeze();
                    return geom;
                }
                catch
                {
                    return null;
                }
            });
        }

        public static LucideIcon Create(LucideIconKind kind, double size = 18.0, Brush? foreground = null)
        {
            var icon = new LucideIcon
            {
                Kind = kind,
                Size = size
            };
            if (foreground != null)
            {
                icon.Foreground = foreground;
            }
            return icon;
        }

        public static string KindToKebabName(LucideIconKind kind)
        {
            return kind switch
            {
                LucideIconKind.None => string.Empty,
                LucideIconKind.Home => "house",
                LucideIconKind.House => "house",
                LucideIconKind.BookOpen => "book-open",
                LucideIconKind.Pencil => "pencil",
                LucideIconKind.Edit => "pencil",
                LucideIconKind.Edit2 => "pen-line",
                LucideIconKind.Edit3 => "pencil-line",
                LucideIconKind.PenTool => "pencil-line",
                LucideIconKind.TrendingUp => "chart-line",
                LucideIconKind.LineChart => "chart-line",
                LucideIconKind.BarChart => "chart-column-increasing",
                LucideIconKind.BarChart2 => "chart-bar-big",
                LucideIconKind.BarChart3 => "chart-bar",
                LucideIconKind.PieChart => "chart-pie",
                LucideIconKind.Square => "square",
                LucideIconKind.Maximize => "maximize",
                LucideIconKind.Minimize => "minimize",
                LucideIconKind.UserCheck => "user-check",
                LucideIconKind.UserPlus => "user-plus",
                LucideIconKind.UserX => "user-x",
                LucideIconKind.GraduationCap => "graduation-cap",
                LucideIconKind.LogOut => "log-out",
                LucideIconKind.LogIn => "log-in",
                LucideIconKind.CheckCircle => "circle-check",
                LucideIconKind.CheckCircle2 => "circle-check-big",
                LucideIconKind.XCircle => "circle-x",
                LucideIconKind.PlusCircle => "circle-plus",
                LucideIconKind.MinusCircle => "circle-minus",
                LucideIconKind.AlertCircle => "circle-alert",
                LucideIconKind.AlertTriangle => "triangle-alert",
                LucideIconKind.HelpCircle => "circle-help",
                LucideIconKind.FolderOpen => "folder-open",
                LucideIconKind.FolderPlus => "folder-plus",
                LucideIconKind.FolderSync => "folder-sync",
                LucideIconKind.FolderArchive => "folder-archive",
                LucideIconKind.FileText => "file-text",
                LucideIconKind.FileSpreadsheet => "file-spreadsheet",
                LucideIconKind.FileCode => "file-code",
                LucideIconKind.FileCheck => "file-check",
                LucideIconKind.FilePlus => "file-plus",
                LucideIconKind.FileEdit => "file-pen-line",
                LucideIconKind.Trash => "trash-2",
                LucideIconKind.Trash2 => "trash-2",
                LucideIconKind.RefreshCw => "refresh-cw",
                LucideIconKind.RotateCcw => "rotate-ccw",
                LucideIconKind.EyeOff => "eye-off",
                LucideIconKind.VolumeX => "volume-x",
                LucideIconKind.MicOff => "mic-off",
                LucideIconKind.Share2 => "share-2",
                LucideIconKind.ExternalLink => "external-link",
                LucideIconKind.ChevronLeft => "chevron-left",
                LucideIconKind.ChevronRight => "chevron-right",
                LucideIconKind.ChevronUp => "chevron-up",
                LucideIconKind.ChevronDown => "chevron-down",
                LucideIconKind.ChevronsLeft => "chevrons-left",
                LucideIconKind.ChevronsRight => "chevrons-right",
                LucideIconKind.ArrowLeft => "arrow-left",
                LucideIconKind.ArrowRight => "arrow-right",
                LucideIconKind.ArrowUp => "arrow-up",
                LucideIconKind.ArrowDown => "arrow-down",
                LucideIconKind.MoreHorizontal => "more-horizontal",
                LucideIconKind.MoreVertical => "more-vertical",
                LucideIconKind.ShieldCheck => "shield-check",
                LucideIconKind.ThumbsUp => "thumbs-up",
                LucideIconKind.ThumbsDown => "thumbs-down",
                LucideIconKind.MessageSquare => "message-square",
                LucideIconKind.MessageCircle => "message-circle",
                LucideIconKind.ClipboardCheck => "clipboard-check",
                LucideIconKind.ClipboardList => "clipboard-list",
                LucideIconKind.SlidersHorizontal => "sliders-horizontal",
                LucideIconKind.Wand2 => "wand-2",
                _ => ToKebabCase(kind.ToString())
            };
        }

        private static string ToKebabCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return Regex.Replace(str, "([a-z0-9])([A-Z])", "$1-$2").ToLowerInvariant();
        }
    }
}
