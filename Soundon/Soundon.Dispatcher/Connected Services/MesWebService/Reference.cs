﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Soundon.Dispatcher.MesWebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/", ConfigurationName="MesWebService.MachineAccessTestService")]
    public interface MachineAccessTestService {
        
        // CODEGEN: 操作 getResourceDescription 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1 getResourceDescription(Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1> getResourceDescriptionAsync(Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/")]
    public partial class getResourceDescription : object, System.ComponentModel.INotifyPropertyChanged {
        
        private machineAccessTestRequest pRequestField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public machineAccessTestRequest pRequest {
            get {
                return this.pRequestField;
            }
            set {
                this.pRequestField = value;
                this.RaisePropertyChanged("pRequest");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/")]
    public partial class machineAccessTestRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string siteField;
        
        private string resourceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string site {
            get {
                return this.siteField;
            }
            set {
                this.siteField = value;
                this.RaisePropertyChanged("site");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string resource {
            get {
                return this.resourceField;
            }
            set {
                this.resourceField = value;
                this.RaisePropertyChanged("resource");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/")]
    public partial class machineAccessTestResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string statusField;
        
        private string messageField;
        
        private string descriptionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/")]
    public partial class getResourceDescriptionResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private machineAccessTestResponse returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public machineAccessTestResponse @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
                this.RaisePropertyChanged("return");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getResourceDescriptionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/", Order=0)]
        public Soundon.Dispatcher.MesWebService.getResourceDescription getResourceDescription;
        
        public getResourceDescriptionRequest() {
        }
        
        public getResourceDescriptionRequest(Soundon.Dispatcher.MesWebService.getResourceDescription getResourceDescription) {
            this.getResourceDescription = getResourceDescription;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getResourceDescriptionResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://accesstest.uppermachine.ws.sapdev.com/", Order=0)]
        public Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse getResourceDescriptionResponse;
        
        public getResourceDescriptionResponse1() {
        }
        
        public getResourceDescriptionResponse1(Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse getResourceDescriptionResponse) {
            this.getResourceDescriptionResponse = getResourceDescriptionResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MachineAccessTestServiceChannel : Soundon.Dispatcher.MesWebService.MachineAccessTestService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MachineAccessTestServiceClient : System.ServiceModel.ClientBase<Soundon.Dispatcher.MesWebService.MachineAccessTestService>, Soundon.Dispatcher.MesWebService.MachineAccessTestService {
        
        public MachineAccessTestServiceClient() {
        }
        
        public MachineAccessTestServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MachineAccessTestServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MachineAccessTestServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MachineAccessTestServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1 Soundon.Dispatcher.MesWebService.MachineAccessTestService.getResourceDescription(Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest request) {
            return base.Channel.getResourceDescription(request);
        }
        
        public Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse getResourceDescription(Soundon.Dispatcher.MesWebService.getResourceDescription getResourceDescription1) {
            Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest inValue = new Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest();
            inValue.getResourceDescription = getResourceDescription1;
            Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1 retVal = ((Soundon.Dispatcher.MesWebService.MachineAccessTestService)(this)).getResourceDescription(inValue);
            return retVal.getResourceDescriptionResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1> Soundon.Dispatcher.MesWebService.MachineAccessTestService.getResourceDescriptionAsync(Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest request) {
            return base.Channel.getResourceDescriptionAsync(request);
        }
        
        public System.Threading.Tasks.Task<Soundon.Dispatcher.MesWebService.getResourceDescriptionResponse1> getResourceDescriptionAsync(Soundon.Dispatcher.MesWebService.getResourceDescription getResourceDescription) {
            Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest inValue = new Soundon.Dispatcher.MesWebService.getResourceDescriptionRequest();
            inValue.getResourceDescription = getResourceDescription;
            return ((Soundon.Dispatcher.MesWebService.MachineAccessTestService)(this)).getResourceDescriptionAsync(inValue);
        }
    }
}
