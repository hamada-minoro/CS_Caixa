﻿#pragma checksum "..\..\..\WinQrCode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C61AE9D7B1747D1FCED4CD231296DB82"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.18408
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
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
    /// WinQrCode
    /// </summary>
    public partial class WinQrCode : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label12;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtLetraSelo;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label6;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNumeroSelo;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label12_Copy;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAleatorio;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid1;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSalvar;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\WinQrCode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelar;
        
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
            System.Uri resourceLocater = new System.Uri("/CS_Caixa;component/winqrcode.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\WinQrCode.xaml"
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
            
            #line 4 "..\..\..\WinQrCode.xaml"
            ((CS_Caixa.WinQrCode)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label12 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.txtLetraSelo = ((System.Windows.Controls.TextBox)(target));
            
            #line 10 "..\..\..\WinQrCode.xaml"
            this.txtLetraSelo.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.txtLetraSelo_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\WinQrCode.xaml"
            this.txtLetraSelo.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtLetraSelo_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.label6 = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.txtNumeroSelo = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\..\WinQrCode.xaml"
            this.txtNumeroSelo.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.txtNumeroSelo_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\WinQrCode.xaml"
            this.txtNumeroSelo.LostFocus += new System.Windows.RoutedEventHandler(this.txtNumeroSelo_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.label12_Copy = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.txtAleatorio = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\..\WinQrCode.xaml"
            this.txtAleatorio.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.txtLetraSelo_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.grid1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.btnSalvar = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\WinQrCode.xaml"
            this.btnSalvar.Click += new System.Windows.RoutedEventHandler(this.btnSalvar_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\WinQrCode.xaml"
            this.btnCancelar.Click += new System.Windows.RoutedEventHandler(this.btnCancelar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

