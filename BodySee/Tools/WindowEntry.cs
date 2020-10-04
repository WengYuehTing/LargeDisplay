using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BodySee.Tools
{
    public interface IWindowEntry
    {
        IntPtr HWnd { get; set; }
        uint ProcessId { get; set; }
        string ProcessName { get; set; }
        //TODO: Windows 10 App Icons
        //string ProcessFileName { get; set; }
        string Title { get; set; }
        bool IsVisible { get; set; }
        
        bool IsSameWindow(IWindowEntry other);
    }

    public class WindowEntry
    {
        public IntPtr HWnd { get; set; }
        public uint ProcessId { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
        public string ProcessName { get; set; }

        public bool IsSameWindow(IWindowEntry other)
        {
            if (other == null)
                return false;

            return ProcessId == other.ProcessId && HWnd == other.HWnd;
        }

        public override string ToString()
        {
            return $"{ProcessName} ({ProcessId}): \"{Title}\"";
        }
    }

    public static class WindowEntryFactory
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        public static WindowEntry Create(IntPtr hWnd)
        {
            GetWindowThreadProcessId(hWnd, out uint processId);

            return Create(hWnd, processId);
        }

        public static WindowEntry Create(IntPtr hWnd, uint processId)
        {
            var windowTitle = GetWindowTitle(hWnd);
            var isVisible = !IsIconic(hWnd);

            return new WindowEntry
            {
                HWnd = hWnd,
                Title = windowTitle,
                ProcessId = processId,
                IsVisible = isVisible
            };
        }

        private static string GetWindowTitle(IntPtr hWnd)
        {
            var lLength = GetWindowTextLength(hWnd);
            if (lLength == 0)
                return null;

            var builder = new StringBuilder(lLength);
            GetWindowText(hWnd, builder, lLength + 1);
            return builder.ToString();
        }
    }
}
