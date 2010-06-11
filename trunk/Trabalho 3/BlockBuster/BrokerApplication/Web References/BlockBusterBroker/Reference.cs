﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.4927.
// 
#pragma warning disable 1591

namespace BrokerApplication.BlockBusterBroker {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSBrokerSoap", Namespace="http://sd.deetc.isel.pt/")]
    public partial class WSBroker : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetCinemasOperationCompleted;
        
        private System.Threading.SendOrPostCallback RegisterCinemaOperationCompleted;
        
        private System.Threading.SendOrPostCallback UnregisterCinemaOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSBroker() {
            this.Url = global::BrokerApplication.Properties.Settings.Default.BrokerApplication_BlockBusterBroker_WSBroker;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetCinemasCompletedEventHandler GetCinemasCompleted;
        
        /// <remarks/>
        public event RegisterCinemaCompletedEventHandler RegisterCinemaCompleted;
        
        /// <remarks/>
        public event UnregisterCinemaCompletedEventHandler UnregisterCinemaCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sd.deetc.isel.pt/GetCinemas", RequestNamespace="http://sd.deetc.isel.pt/", ResponseNamespace="http://sd.deetc.isel.pt/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public CinemaRegistry[] GetCinemas() {
            object[] results = this.Invoke("GetCinemas", new object[0]);
            return ((CinemaRegistry[])(results[0]));
        }
        
        /// <remarks/>
        public void GetCinemasAsync() {
            this.GetCinemasAsync(null);
        }
        
        /// <remarks/>
        public void GetCinemasAsync(object userState) {
            if ((this.GetCinemasOperationCompleted == null)) {
                this.GetCinemasOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCinemasOperationCompleted);
            }
            this.InvokeAsync("GetCinemas", new object[0], this.GetCinemasOperationCompleted, userState);
        }
        
        private void OnGetCinemasOperationCompleted(object arg) {
            if ((this.GetCinemasCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCinemasCompleted(this, new GetCinemasCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sd.deetc.isel.pt/RegisterCinema", RequestNamespace="http://sd.deetc.isel.pt/", ResponseNamespace="http://sd.deetc.isel.pt/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void RegisterCinema(string name, string url) {
            this.Invoke("RegisterCinema", new object[] {
                        name,
                        url});
        }
        
        /// <remarks/>
        public void RegisterCinemaAsync(string name, string url) {
            this.RegisterCinemaAsync(name, url, null);
        }
        
        /// <remarks/>
        public void RegisterCinemaAsync(string name, string url, object userState) {
            if ((this.RegisterCinemaOperationCompleted == null)) {
                this.RegisterCinemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegisterCinemaOperationCompleted);
            }
            this.InvokeAsync("RegisterCinema", new object[] {
                        name,
                        url}, this.RegisterCinemaOperationCompleted, userState);
        }
        
        private void OnRegisterCinemaOperationCompleted(object arg) {
            if ((this.RegisterCinemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RegisterCinemaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sd.deetc.isel.pt/UnregisterCinema", RequestNamespace="http://sd.deetc.isel.pt/", ResponseNamespace="http://sd.deetc.isel.pt/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void UnregisterCinema(string name) {
            this.Invoke("UnregisterCinema", new object[] {
                        name});
        }
        
        /// <remarks/>
        public void UnregisterCinemaAsync(string name) {
            this.UnregisterCinemaAsync(name, null);
        }
        
        /// <remarks/>
        public void UnregisterCinemaAsync(string name, object userState) {
            if ((this.UnregisterCinemaOperationCompleted == null)) {
                this.UnregisterCinemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUnregisterCinemaOperationCompleted);
            }
            this.InvokeAsync("UnregisterCinema", new object[] {
                        name}, this.UnregisterCinemaOperationCompleted, userState);
        }
        
        private void OnUnregisterCinemaOperationCompleted(object arg) {
            if ((this.UnregisterCinemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UnregisterCinemaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://sd.deetc.isel.pt/")]
    public partial class CinemaRegistry {
        
        private string nameField;
        
        private string urlField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string Url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetCinemasCompletedEventHandler(object sender, GetCinemasCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCinemasCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCinemasCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CinemaRegistry[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CinemaRegistry[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void RegisterCinemaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void UnregisterCinemaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591