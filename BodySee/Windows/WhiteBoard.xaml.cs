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
    /// <summary>
    /// WhiteBoard.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteBoard : Window
    {
        #region Private Fields
        private CommandStack        _cmdStack;
        private int                 _editingOperationCount;
        #endregion

        #region Properties
        private Color _InkColor;
        public Color InkColor
        {
            get { return _InkColor; }
            set
            {
                _InkColor = value;
                canvas.DefaultDrawingAttributes.Color = _InkColor;
            }
        }

        private Size _InkSize;
        public Size InkSize
        {
            get { return _InkSize; }
            set
            {
                _InkSize = value;
                canvas.DefaultDrawingAttributes.Width = _InkSize.Width;
                canvas.DefaultDrawingAttributes.Height = _InkSize.Height;
            }
        }

        private EllipseStylusShape _EraserSize;
        public EllipseStylusShape EraserSize
        {
            get { return _EraserSize; }
            set
            {
                _EraserSize = value;
                canvas.EraserShape = _EraserSize;
            }
        }
        #endregion
        
        public WhiteBoard()
        {
            InitializeComponent();
            
            _cmdStack = new CommandStack(canvas.Strokes);
            _editingOperationCount = 0;
            InkColor = Colors.Black;
            InkSize = new Size(canvas.DefaultDrawingAttributes.Width, canvas.DefaultDrawingAttributes.Height);
            EraserSize = new EllipseStylusShape(50, 50);

            canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            TaskManager.getInstance().whiteBoard = this;
        }

        #region Public Methods
        /// <summary>
        /// 修改画笔颜色
        /// </summary>
        /// <param name="color"> 修改成指定的颜色</param>
        public void ChangeColor(Color color)
        {
            ExitEraserMode();
            InkColor = color;
        }


        /// <summary>
        /// 改变画笔粗细
        /// </summary>
        public void ChangeInkSize(Size size)
        {
            InkSize = size;
        }


        /// <summary>
        /// 启用橡皮擦模式
        /// </summary>
        public void EnterEraserMode()
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }


        /// <summary>
        /// 调整橡皮擦粗细
        /// </summary>
        public void ChangeEraserSize(EllipseStylusShape shape)
        {
            EraserSize = shape;
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
                EnterEraserMode();
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.R))
            {
                ChangeColor(Colors.Red);
            }

            if (e.KeyStates == Keyboard.GetKeyStates(Key.S))
            {
                Save();
            }
        }
        

        
        #endregion
    }
}
