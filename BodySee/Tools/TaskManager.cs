using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BodySee
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
        #endregion

        #region Public Method
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
                        WinApiManager.TakeOverOperation();
                        break;
                    case "close":
                        WinApiManager.CloseWindow();
                        break;
                    case "minimize":
                        WinApiManager.MinimizeWindow();
                        break;
                    case "maximize":
                        WinApiManager.MaximizeWindow();
                        break;
                    case "restore":
                        WinApiManager.RestoreWindow();
                        break;
                }
            }));
            
        }
        #endregion
    }
}
