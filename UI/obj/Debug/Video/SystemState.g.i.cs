﻿#pragma checksum "..\..\..\Video\SystemState.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2CFAF40FA7FC350FBA206E77B5A715A7"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using RootLibrary.WPF.Localization;
using SFMControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace UI.Video {
    
    
    /// <summary>
    /// SystemState
    /// </summary>
    public partial class SystemState : SFMControls.WindowBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Video\SystemState.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFMControls.ButtonSfm btnPartSet;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Video\SystemState.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFMControls.ButtonSfm btnAllSet;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Video\SystemState.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFileName;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Video\SystemState.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFMControls.ButtonSfm btnRead;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Video\SystemState.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFMControls.ButtonSfm btnUpdate;
        
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
            System.Uri resourceLocater = new System.Uri("/分布式停车场管理系统;component/video/systemstate.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Video\SystemState.xaml"
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
            
            #line 5 "..\..\..\Video\SystemState.xaml"
            ((UI.Video.SystemState)(target)).Loaded += new System.Windows.RoutedEventHandler(this.WindowBase_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnPartSet = ((SFMControls.ButtonSfm)(target));
            
            #line 11 "..\..\..\Video\SystemState.xaml"
            this.btnPartSet.Click += new System.Windows.RoutedEventHandler(this.btnPartSet_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnAllSet = ((SFMControls.ButtonSfm)(target));
            
            #line 12 "..\..\..\Video\SystemState.xaml"
            this.btnAllSet.Click += new System.Windows.RoutedEventHandler(this.btnAllSet_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtFileName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btnRead = ((SFMControls.ButtonSfm)(target));
            
            #line 21 "..\..\..\Video\SystemState.xaml"
            this.btnRead.Click += new System.Windows.RoutedEventHandler(this.btnRead_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnUpdate = ((SFMControls.ButtonSfm)(target));
            
            #line 22 "..\..\..\Video\SystemState.xaml"
            this.btnUpdate.Click += new System.Windows.RoutedEventHandler(this.btnUpdate_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

