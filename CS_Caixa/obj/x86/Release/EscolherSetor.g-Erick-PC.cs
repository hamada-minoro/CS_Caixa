﻿#pragma checksum "..\..\..\EscolherSetor.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "831D5EE99876EC2BB29A54C122445ED6B2988A66"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace CS_Caixa {
    
    
    /// <summary>
    /// EscolherSetor
    /// </summary>
    public partial class EscolherSetor : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetor1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetor2;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetor3;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblContagem;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTitulo;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetor4;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\EscolherSetor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblHora;
        
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
            System.Uri resourceLocater = new System.Uri("/CS_Caixa;component/escolhersetor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\EscolherSetor.xaml"
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
            
            #line 4 "..\..\..\EscolherSetor.xaml"
            ((CS_Caixa.EscolherSetor)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\EscolherSetor.xaml"
            ((CS_Caixa.EscolherSetor)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\EscolherSetor.xaml"
            ((CS_Caixa.EscolherSetor)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.Window_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnSetor1 = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\EscolherSetor.xaml"
            this.btnSetor1.Click += new System.Windows.RoutedEventHandler(this.btnSetor1_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnSetor2 = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\EscolherSetor.xaml"
            this.btnSetor2.Click += new System.Windows.RoutedEventHandler(this.btnSetor2_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSetor3 = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\EscolherSetor.xaml"
            this.btnSetor3.Click += new System.Windows.RoutedEventHandler(this.btnSetor3_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lblContagem = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.lblTitulo = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.btnSetor4 = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\EscolherSetor.xaml"
            this.btnSetor4.Click += new System.Windows.RoutedEventHandler(this.btnSetor4_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblHora = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

