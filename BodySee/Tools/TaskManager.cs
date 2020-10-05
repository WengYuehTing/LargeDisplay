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
        public MainWindow mainWindow;
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
                        whiteBoard.ChangeColor(Colors.Red);
                        break;
                    case "green":
                        whiteBoard.ChangeColor(Colors.Green);
                        break;
                    case "blue":
                        whiteBoard.ChangeColor(Colors.Blue);
                        break;
                    case "eraser":
                        whiteBoard.EnterEraserMode();
                        break;
                    case "clear":
                        whiteBoard.Clear();
                        break;
                    case "two_fingers":
                        WindowsHandler.AcquirePriortyofScreenTouch();
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
                }
            }));
            
        }
        #endregion
    }
}
