using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BodySee.Windows;

namespace BodySee.Tools
{
    class TaskManager
    {
        #region Singleton
        private static TaskManager instance;
        public static TaskManager getInstance()
        {
            if(instance == null)
            {
                instance = new TaskManager();
            }
            return instance;
        }
        private TaskManager(){}
        #endregion

        #region Window References
        public WhiteBoard whiteBoard;
        public Menu menu;
        #endregion

        #region Gesture and Command Mapping
        public void Execute(String command)
        {
            command = command.Remove(command.Length - 1);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (command)
                {
                    case "red":
                        whiteBoard.ChangeNormalBrushColor(Colors.Red);
                        whiteBoard.EnterNormalBrushMode();
                        break;
                    case "yellow":
                        whiteBoard.ChangeNormalBrushColor(Colors.Yellow);
                        whiteBoard.EnterNormalBrushMode();
                        break;
                    case "blue":
                        whiteBoard.ChangeNormalBrushColor(Colors.Blue);
                        whiteBoard.EnterNormalBrushMode();
                        break;
                    case "black":
                        whiteBoard.ChangeNormalBrushColor(Colors.Black);
                        whiteBoard.EnterNormalBrushMode();
                        break;
                    case "finger_eraser":
                        whiteBoard.EnterFingerEraserMode();
                        break;
                    case "palm_eraser":
                        whiteBoard.EnterPalmEraserMode();
                        break;
                    case "clear":
                        whiteBoard.Clear();
                        break;
                    case "normal_brush":
                        whiteBoard.EnterNormalBrushMode();
                        break;
                    case "highlight_brush":
                        whiteBoard.EnterHighlightBrushMode();
                        break;
                    case "two_fingers":
                        WindowsHandler.AcquirePriortyofScreenTouch();
                        break;
                    case "back":
                        WindowsHandler.BlockingScreenTouch();
                        break;
                    case "face":
                        WindowsHandler.RecoverScreenTouch();
                        break;
                    case "close":
                        WindowsHandler.CloseWindow(WindowsHandler.FindTopmostWindow());
                        break;
                    case "minimize":
                        WindowsHandler.MinimizeWindow(WindowsHandler.FindTopmostWindow());
                        break;
                    case "maximize":
                        WindowsHandler.MaximizeWindow(WindowsHandler.FindTopmostWindow());
                        break;
                    case "restore":
                        WindowsHandler.RestoreWindow(WindowsHandler.FindTopmostWindow());
                        break;
                    default:
                        //string[] data = command.Split(',');
                        //float x = float.Parse(data[0]);
                        //float y = float.Parse(data[1]);
                        menu.GetComponent().run(command);
                        return;
                       
                }
            }));
            
        }
        #endregion
    }
}
