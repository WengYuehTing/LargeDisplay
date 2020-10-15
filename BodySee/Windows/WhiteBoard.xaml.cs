using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using BodySee.Tools;

namespace BodySee.Windows
{
    public class CustomBrush
    {
        public Color Color { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public CustomBrush(Color _color, double _width, double _height)
        {
            Color = _color;
            Width = _width;
            Height = _height;
        }
    }

    public enum ESupportedBrush
    {
        Normal,
        Highlight
    }

    /// <summary>
    /// WhiteBoard.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteBoard : Window
    {
        #region Private Fields
        private CommandStack        _cmdStack;
        private int                 _editingOperationCount;
        #endregion

        #region Private Constant
        private Color DEFAULT_NORMAL_COLOR = Colors.Black;
        private double DEFAULT_NORMAL_WIDTH = 2.0f;
        private double DEFAULT_NORMAL_HEIGHT = 2.0f;
        private Color DEFAULT_HIGHTLIGHT_COLOR = Colors.Yellow;
        private double DEFAULT_HIGHLIGHT_WIDTH = 2.0f;
        private double DEFAULT_HIGHLIGHT_HEIGHT = 20.0f;
        private double MAX_NORMAL_SIZE = 10.0f;
        private double MIN_NORMAL_SIZE = 1.0f;
        private double MAX_HIGHLIGHT_HEIGHT = 30.0f;
        private double MIN_HIGHLIGHT_HEIGHT = 10.0f;
        #endregion

        #region Properties
        private CustomBrush _NormalBrush;
        public CustomBrush NormalBrush
        {
            get { return _NormalBrush; }
            set { _NormalBrush = value; }

        }


        private CustomBrush _HighlightBrush;
        public CustomBrush HighlightBrush
        {
            get { return _HighlightBrush; }
            set { _HighlightBrush = value; }
        }


        private ESupportedBrush _BrushMode;
        public ESupportedBrush BrushMode
        {
            get { return _BrushMode; }
            set
            {
                _BrushMode = value;
                ExitEraserMode();
                if (_BrushMode == ESupportedBrush.Normal)
                {
                    canvas.DefaultDrawingAttributes.Color = NormalBrush.Color;
                    canvas.DefaultDrawingAttributes.Width = NormalBrush.Width;
                    canvas.DefaultDrawingAttributes.Height = NormalBrush.Height;
                }
                else
                {
                    canvas.DefaultDrawingAttributes.Color = HighlightBrush.Color;
                    canvas.DefaultDrawingAttributes.Width = HighlightBrush.Width;
                    canvas.DefaultDrawingAttributes.Height = HighlightBrush.Height;
                }
            }
        }

        private EllipseStylusShape _FingerEraserSize;
        public EllipseStylusShape FingerEraserSize
        {
            get { return _FingerEraserSize; }
            set { _FingerEraserSize = value; }
        }

        private EllipseStylusShape _PalmEraserSize;
        public EllipseStylusShape PalmEraserSize
        {
            get { return _PalmEraserSize; }
            set { _PalmEraserSize = value; }
        }
        #endregion

        public WhiteBoard()
        {
            InitializeComponent();
            
            _cmdStack = new CommandStack(canvas.Strokes);
            _editingOperationCount = 0;
            NormalBrush = new CustomBrush(DEFAULT_NORMAL_COLOR, DEFAULT_NORMAL_WIDTH, DEFAULT_NORMAL_HEIGHT);
            HighlightBrush = new CustomBrush(DEFAULT_HIGHTLIGHT_COLOR, DEFAULT_HIGHLIGHT_WIDTH, DEFAULT_HIGHLIGHT_HEIGHT);
            FingerEraserSize = new EllipseStylusShape(50, 50);
            PalmEraserSize = new EllipseStylusShape(500, 500);
            EnterNormalBrushMode();
            

            canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            TaskManager.getInstance().whiteBoard = this;
        }

        #region Public Methods
        /// <summary>
        /// 启用正常笔刷模式
        /// </summary>
        public void EnterNormalBrushMode()
        {
            BrushMode = ESupportedBrush.Normal;
            SizeSlider.Maximum = MAX_NORMAL_SIZE;
            SizeSlider.Minimum = MIN_NORMAL_SIZE;
            SizeSlider.Value = NormalBrush.Width;
        }


        public void ChangeNormalBrushColor(Color color)
        {
            NormalBrush.Color = color;
        }

        public void ChangeNormalBrushWidth(double width)
        {
            NormalBrush.Width = width;
        }

        public void ChangeNormalBrushHeight(double height)
        {
            NormalBrush.Height = height;
        }


        /// <summary>
        /// 启用荧光笔笔刷模式
        /// </summary>
        public void EnterHighlightBrushMode()
        {
            BrushMode = ESupportedBrush.Highlight;
            SizeSlider.Maximum = MAX_HIGHLIGHT_HEIGHT;
            SizeSlider.Minimum = MIN_HIGHLIGHT_HEIGHT;
            SizeSlider.Value = HighlightBrush.Height;
        }

        public void ChangeHighlightBrushColor(Color color)
        {
            HighlightBrush.Color = color;
        }

        public void ChangeHighlightBrushWidth(double width)
        {
            HighlightBrush.Width = width;
        }

        public void ChangeHighlightBrushHeight(double height)
        {
            HighlightBrush.Height = height;
        }


        /// <summary>
        /// 启用手指橡皮擦模式
        /// </summary>
        public void EnterFingerEraserMode()
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            canvas.EraserShape = FingerEraserSize;
        }


        /// <summary>
        /// 启用手掌橡皮擦模式
        /// </summary>
        public void EnterPalmEraserMode()
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            canvas.EraserShape = PalmEraserSize;
        }
 
        /// <summary>
        /// 调整橡皮擦粗细
        /// </summary>
        public void ChangeEraserSize(string mode, EllipseStylusShape shape)
        {
            if(mode == "finger")
            {
                FingerEraserSize = shape;
            } else if (mode == "palm")
            {
                PalmEraserSize = shape;
            }
        }


        /// <summary>
        /// 关闭橡皮擦模式
        /// </summary>
        public void ExitEraserMode()
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }
        
        
        /// <summary>
        /// 清空画布
        /// </summary>
        public void Clear()
        {
            canvas.Strokes.Clear();
        }


        /// <summary>
        /// 撤销最近一次的操作
        /// </summary>
        public void Undo()
        {
            _cmdStack.Undo();
        }


        /// <summary>
        /// 返回最近一次的操作
        /// </summary>
        public void Redo()
        {
            _cmdStack.Redo();
        }


        /// <summary>
        /// 保存图片到本地
        /// </summary>
        public void Save()
        {
            SaveFileDialog f = new SaveFileDialog();
            f.FileName = "sketch" + DateTime.Now.ToString("yyyyMMddhhmmss");
            f.DefaultExt = ".jpg";
            f.Filter = "Image (.jpg .png) | *.jpg *.png";
            if(f.ShowDialog() == true)
            {
                string filename = f.FileName;
                int margin = (int)canvas.Margin.Left;
                int width = (int)canvas.ActualWidth - margin;
                int height = (int)canvas.ActualHeight - margin;

                Thickness thickness = canvas.Margin;
                Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
                canvas.Margin = new Thickness(0, 0, 0, 0);
                canvas.Measure(size);
                canvas.Arrange(new Rect(size));
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
                canvas.Margin = thickness;
            }
        }

        public void ShowPenMenu(int x, int y)
        {
            PenMenu.Visibility = Visibility.Visible;
            Thickness margin = PenMenu.Margin;
            margin.Left = x;
            margin.Top = y;
            PenMenu.Margin = margin;
        }

        public void HidePenMenu()
        {
            PenMenu.Visibility = Visibility.Collapsed;
        }


        #endregion

        #region UI Event
        /// <summary>
        /// 监听stroke变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Strokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
        {
            StrokeCollection added = new StrokeCollection(e.Added);
            StrokeCollection removed = new StrokeCollection(e.Removed);

            CommandItem item = new StrokesAddedOrRemovedCI(_cmdStack, canvas.EditingMode, added, removed, _editingOperationCount);
            _cmdStack.Enqueue(item);
        }

        /// <summary>
        /// Increment operation count for undo/redo. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_TouchUp(object sender, TouchEventArgs e)
        {
            _editingOperationCount++;
        }


        #endregion

        #region Debug
        /// <summary>
        /// 监听键盘输入 - Manually control by Wizard of Oz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Q))
            {
                Undo();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.W))
            {
                Redo();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.E))
            {
                EnterFingerEraserMode();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.R))
            {
                EnterHighlightBrushMode();
                
            }
        }
        #endregion

        private void BlackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BrushMode == ESupportedBrush.Normal)
                ChangeNormalBrushColor(Colors.Black);
            else
                ChangeHighlightBrushColor(Colors.Black);

            BrushMode = BrushMode; //refresh
        }

        private void RedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BrushMode == ESupportedBrush.Normal)
                ChangeNormalBrushColor(Colors.Red);
            else
                ChangeHighlightBrushColor(Colors.Red);

            BrushMode = BrushMode; //refresh
        }

        private void BlueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BrushMode == ESupportedBrush.Normal)
                ChangeNormalBrushColor(Colors.Blue);
            else
                ChangeHighlightBrushColor(Colors.Blue);

            BrushMode = BrushMode; //refresh
        }

        private void YellowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BrushMode == ESupportedBrush.Normal)
                ChangeNormalBrushColor(Colors.Yellow);
            else
                ChangeHighlightBrushColor(Colors.Yellow);

            BrushMode = BrushMode; //refresh
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BrushMode == ESupportedBrush.Normal)
            {
                NormalBrush.Width = e.NewValue;
                NormalBrush.Height = e.NewValue;
            }
            else
            {
                HighlightBrush.Height = e.NewValue;
            }

            BrushMode = BrushMode; //refresh
        }
    }
}
