﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace CAMEL.Baking.ProductionDataUploadWebService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ProductionDataUploadServiceSoap", Namespace="http://172.21.30.251/CamelPostSection/")]
    public partial class ProductionDataUploadService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback UploadInjectionDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetInjectionWeightDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadDegas1DataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadDegas2DataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadSecondaryHighTempDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadIntoATankDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadSubCapacityCabinetDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSubCapacityCabinetResultOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadOCV1_2DataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadOCV3_4DataOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ProductionDataUploadService() {
            this.Url = global::CAMEL.Baking.Properties.Settings.Default.CAMEL_Baking_ProductionDataUploadService_ProductionDataUploadService;
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
        public event UploadInjectionDataCompletedEventHandler UploadInjectionDataCompleted;
        
        /// <remarks/>
        public event GetInjectionWeightDataCompletedEventHandler GetInjectionWeightDataCompleted;
        
        /// <remarks/>
        public event UploadDegas1DataCompletedEventHandler UploadDegas1DataCompleted;
        
        /// <remarks/>
        public event UploadDegas2DataCompletedEventHandler UploadDegas2DataCompleted;
        
        /// <remarks/>
        public event UploadSecondaryHighTempDataCompletedEventHandler UploadSecondaryHighTempDataCompleted;
        
        /// <remarks/>
        public event UploadIntoATankDataCompletedEventHandler UploadIntoATankDataCompleted;
        
        /// <remarks/>
        public event UploadSubCapacityCabinetDataCompletedEventHandler UploadSubCapacityCabinetDataCompleted;
        
        /// <remarks/>
        public event GetSubCapacityCabinetResultCompletedEventHandler GetSubCapacityCabinetResultCompleted;
        
        /// <remarks/>
        public event UploadOCV1_2DataCompletedEventHandler UploadOCV1_2DataCompleted;
        
        /// <remarks/>
        public event UploadOCV3_4DataCompletedEventHandler UploadOCV3_4DataCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadInjectionData", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadInjectionData(string xmlParams) {
            object[] results = this.Invoke("UploadInjectionData", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadInjectionDataAsync(string xmlParams) {
            this.UploadInjectionDataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadInjectionDataAsync(string xmlParams, object userState) {
            if ((this.UploadInjectionDataOperationCompleted == null)) {
                this.UploadInjectionDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadInjectionDataOperationCompleted);
            }
            this.InvokeAsync("UploadInjectionData", new object[] {
                        xmlParams}, this.UploadInjectionDataOperationCompleted, userState);
        }
        
        private void OnUploadInjectionDataOperationCompleted(object arg) {
            if ((this.UploadInjectionDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadInjectionDataCompleted(this, new UploadInjectionDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/GetInjectionWeightData", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetInjectionWeightData(string xmlParams) {
            object[] results = this.Invoke("GetInjectionWeightData", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetInjectionWeightDataAsync(string xmlParams) {
            this.GetInjectionWeightDataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void GetInjectionWeightDataAsync(string xmlParams, object userState) {
            if ((this.GetInjectionWeightDataOperationCompleted == null)) {
                this.GetInjectionWeightDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInjectionWeightDataOperationCompleted);
            }
            this.InvokeAsync("GetInjectionWeightData", new object[] {
                        xmlParams}, this.GetInjectionWeightDataOperationCompleted, userState);
        }
        
        private void OnGetInjectionWeightDataOperationCompleted(object arg) {
            if ((this.GetInjectionWeightDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInjectionWeightDataCompleted(this, new GetInjectionWeightDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadDegas1Data", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadDegas1Data(string xmlParams) {
            object[] results = this.Invoke("UploadDegas1Data", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadDegas1DataAsync(string xmlParams) {
            this.UploadDegas1DataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadDegas1DataAsync(string xmlParams, object userState) {
            if ((this.UploadDegas1DataOperationCompleted == null)) {
                this.UploadDegas1DataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadDegas1DataOperationCompleted);
            }
            this.InvokeAsync("UploadDegas1Data", new object[] {
                        xmlParams}, this.UploadDegas1DataOperationCompleted, userState);
        }
        
        private void OnUploadDegas1DataOperationCompleted(object arg) {
            if ((this.UploadDegas1DataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadDegas1DataCompleted(this, new UploadDegas1DataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadDegas2Data", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadDegas2Data(string xmlParams) {
            object[] results = this.Invoke("UploadDegas2Data", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadDegas2DataAsync(string xmlParams) {
            this.UploadDegas2DataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadDegas2DataAsync(string xmlParams, object userState) {
            if ((this.UploadDegas2DataOperationCompleted == null)) {
                this.UploadDegas2DataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadDegas2DataOperationCompleted);
            }
            this.InvokeAsync("UploadDegas2Data", new object[] {
                        xmlParams}, this.UploadDegas2DataOperationCompleted, userState);
        }
        
        private void OnUploadDegas2DataOperationCompleted(object arg) {
            if ((this.UploadDegas2DataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadDegas2DataCompleted(this, new UploadDegas2DataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadSecondaryHighTempData", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadSecondaryHighTempData(string xmlParams) {
            object[] results = this.Invoke("UploadSecondaryHighTempData", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadSecondaryHighTempDataAsync(string xmlParams) {
            this.UploadSecondaryHighTempDataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadSecondaryHighTempDataAsync(string xmlParams, object userState) {
            if ((this.UploadSecondaryHighTempDataOperationCompleted == null)) {
                this.UploadSecondaryHighTempDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadSecondaryHighTempDataOperationCompleted);
            }
            this.InvokeAsync("UploadSecondaryHighTempData", new object[] {
                        xmlParams}, this.UploadSecondaryHighTempDataOperationCompleted, userState);
        }
        
        private void OnUploadSecondaryHighTempDataOperationCompleted(object arg) {
            if ((this.UploadSecondaryHighTempDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadSecondaryHighTempDataCompleted(this, new UploadSecondaryHighTempDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadIntoATankData", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadIntoATankData(string xmlParams) {
            object[] results = this.Invoke("UploadIntoATankData", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadIntoATankDataAsync(string xmlParams) {
            this.UploadIntoATankDataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadIntoATankDataAsync(string xmlParams, object userState) {
            if ((this.UploadIntoATankDataOperationCompleted == null)) {
                this.UploadIntoATankDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadIntoATankDataOperationCompleted);
            }
            this.InvokeAsync("UploadIntoATankData", new object[] {
                        xmlParams}, this.UploadIntoATankDataOperationCompleted, userState);
        }
        
        private void OnUploadIntoATankDataOperationCompleted(object arg) {
            if ((this.UploadIntoATankDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadIntoATankDataCompleted(this, new UploadIntoATankDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadSubCapacityCabinetData", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadSubCapacityCabinetData(string xmlParams) {
            object[] results = this.Invoke("UploadSubCapacityCabinetData", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadSubCapacityCabinetDataAsync(string xmlParams) {
            this.UploadSubCapacityCabinetDataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadSubCapacityCabinetDataAsync(string xmlParams, object userState) {
            if ((this.UploadSubCapacityCabinetDataOperationCompleted == null)) {
                this.UploadSubCapacityCabinetDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadSubCapacityCabinetDataOperationCompleted);
            }
            this.InvokeAsync("UploadSubCapacityCabinetData", new object[] {
                        xmlParams}, this.UploadSubCapacityCabinetDataOperationCompleted, userState);
        }
        
        private void OnUploadSubCapacityCabinetDataOperationCompleted(object arg) {
            if ((this.UploadSubCapacityCabinetDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadSubCapacityCabinetDataCompleted(this, new UploadSubCapacityCabinetDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/GetSubCapacityCabinetResult", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSubCapacityCabinetResult(string xmlParams) {
            object[] results = this.Invoke("GetSubCapacityCabinetResult", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSubCapacityCabinetResultAsync(string xmlParams) {
            this.GetSubCapacityCabinetResultAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void GetSubCapacityCabinetResultAsync(string xmlParams, object userState) {
            if ((this.GetSubCapacityCabinetResultOperationCompleted == null)) {
                this.GetSubCapacityCabinetResultOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSubCapacityCabinetResultOperationCompleted);
            }
            this.InvokeAsync("GetSubCapacityCabinetResult", new object[] {
                        xmlParams}, this.GetSubCapacityCabinetResultOperationCompleted, userState);
        }
        
        private void OnGetSubCapacityCabinetResultOperationCompleted(object arg) {
            if ((this.GetSubCapacityCabinetResultCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSubCapacityCabinetResultCompleted(this, new GetSubCapacityCabinetResultCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadOCV1_2Data", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadOCV1_2Data(string xmlParams) {
            object[] results = this.Invoke("UploadOCV1_2Data", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadOCV1_2DataAsync(string xmlParams) {
            this.UploadOCV1_2DataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadOCV1_2DataAsync(string xmlParams, object userState) {
            if ((this.UploadOCV1_2DataOperationCompleted == null)) {
                this.UploadOCV1_2DataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadOCV1_2DataOperationCompleted);
            }
            this.InvokeAsync("UploadOCV1_2Data", new object[] {
                        xmlParams}, this.UploadOCV1_2DataOperationCompleted, userState);
        }
        
        private void OnUploadOCV1_2DataOperationCompleted(object arg) {
            if ((this.UploadOCV1_2DataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadOCV1_2DataCompleted(this, new UploadOCV1_2DataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://172.21.30.251/CamelPostSection/UploadOCV3_4Data", RequestNamespace="http://172.21.30.251/CamelPostSection/", ResponseNamespace="http://172.21.30.251/CamelPostSection/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadOCV3_4Data(string xmlParams) {
            object[] results = this.Invoke("UploadOCV3_4Data", new object[] {
                        xmlParams});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadOCV3_4DataAsync(string xmlParams) {
            this.UploadOCV3_4DataAsync(xmlParams, null);
        }
        
        /// <remarks/>
        public void UploadOCV3_4DataAsync(string xmlParams, object userState) {
            if ((this.UploadOCV3_4DataOperationCompleted == null)) {
                this.UploadOCV3_4DataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadOCV3_4DataOperationCompleted);
            }
            this.InvokeAsync("UploadOCV3_4Data", new object[] {
                        xmlParams}, this.UploadOCV3_4DataOperationCompleted, userState);
        }
        
        private void OnUploadOCV3_4DataOperationCompleted(object arg) {
            if ((this.UploadOCV3_4DataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadOCV3_4DataCompleted(this, new UploadOCV3_4DataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadInjectionDataCompletedEventHandler(object sender, UploadInjectionDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadInjectionDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadInjectionDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetInjectionWeightDataCompletedEventHandler(object sender, GetInjectionWeightDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInjectionWeightDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetInjectionWeightDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadDegas1DataCompletedEventHandler(object sender, UploadDegas1DataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadDegas1DataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadDegas1DataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadDegas2DataCompletedEventHandler(object sender, UploadDegas2DataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadDegas2DataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadDegas2DataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadSecondaryHighTempDataCompletedEventHandler(object sender, UploadSecondaryHighTempDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadSecondaryHighTempDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadSecondaryHighTempDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadIntoATankDataCompletedEventHandler(object sender, UploadIntoATankDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadIntoATankDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadIntoATankDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadSubCapacityCabinetDataCompletedEventHandler(object sender, UploadSubCapacityCabinetDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadSubCapacityCabinetDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadSubCapacityCabinetDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void GetSubCapacityCabinetResultCompletedEventHandler(object sender, GetSubCapacityCabinetResultCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSubCapacityCabinetResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSubCapacityCabinetResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadOCV1_2DataCompletedEventHandler(object sender, UploadOCV1_2DataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadOCV1_2DataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadOCV1_2DataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void UploadOCV3_4DataCompletedEventHandler(object sender, UploadOCV3_4DataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadOCV3_4DataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadOCV3_4DataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591