﻿#pragma checksum "..\..\..\Windows\Menu.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5B892AC12900B3A0B1FD8DD0D63285A11C0224F3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using BodySee.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BodySee.Windows {
    
    
    /// <summary>
    /// Menu
    /// </summary>
    public partial class Menu : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BodySee.Windows.Menu Change;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid background;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image WhiteBoardIcon;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image UndoIcon;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image RedoIcon;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ScreenshotIcon;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image DesktopIcon;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image AppListIcon;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image VolumeIcon;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Windows\Menu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BrightnessIcon;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BodySee;component/windows/menu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\Menu.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Change = ((BodySee.Windows.Menu)(target));
            
            #line 10 "..\..\..\Windows\Menu.xaml"
            this.Change.LocationChanged += new System.EventHandler(this.Menu_LocationChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.background = ((System.Windows.Controls.Grid)(target));
            
            #line 11 "..\..\..\Windows\Menu.xaml"
            this.background.KeyDown += new System.Windows.Input.KeyEventHandler(this.Background_KeyDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\Windows\Menu.xaml"
            this.background.Loaded += new System.Windows.RoutedEventHandler(this.Background_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.WhiteBoardIcon = ((System.Windows.Controls.Image)(target));
            
            #line 24 "..\..\..\Windows\Menu.xaml"
            this.WhiteBoardIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.WhiteBoardIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.UndoIcon = ((System.Windows.Controls.Image)(target));
            
            #line 25 "..\..\..\Windows\Menu.xaml"
            this.UndoIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.UndoIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RedoIcon = ((System.Windows.Controls.Image)(target));
            
            #line 26 "..\..\..\Windows\Menu.xaml"
            this.RedoIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.RedoIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ScreenshotIcon = ((System.Windows.Controls.Image)(target));
            
            #line 27 "..\..\..\Windows\Menu.xaml"
            this.ScreenshotIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.ScreenshotIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.DesktopIcon = ((System.Windows.Controls.Image)(target));
            
            #line 37 "..\..\..\Windows\Menu.xaml"
            this.DesktopIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.DesktopIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.AppListIcon = ((System.Windows.Controls.Image)(target));
            
            #line 38 "..\..\..\Windows\Menu.xaml"
            this.AppListIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.AppListIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.VolumeIcon = ((System.Windows.Controls.Image)(target));
            
            #line 39 "..\..\..\Windows\Menu.xaml"
            this.VolumeIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.VolumeIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.BrightnessIcon = ((System.Windows.Controls.Image)(target));
            
            #line 40 "..\..\..\Windows\Menu.xaml"
            this.BrightnessIcon.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BrightnessIcon_TouchDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

