﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TooDoo.SelfTest.ToDoService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ToDo", Namespace="http://schemas.datacontract.org/2004/07/TooDoo.Entities")]
    [System.SerializableAttribute()]
    public partial class ToDo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DeadLineField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int EstimationTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool FinnishedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreatedDate {
            get {
                return this.CreatedDateField;
            }
            set {
                if ((this.CreatedDateField.Equals(value) != true)) {
                    this.CreatedDateField = value;
                    this.RaisePropertyChanged("CreatedDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DeadLine {
            get {
                return this.DeadLineField;
            }
            set {
                if ((this.DeadLineField.Equals(value) != true)) {
                    this.DeadLineField = value;
                    this.RaisePropertyChanged("DeadLine");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int EstimationTime {
            get {
                return this.EstimationTimeField;
            }
            set {
                if ((this.EstimationTimeField.Equals(value) != true)) {
                    this.EstimationTimeField = value;
                    this.RaisePropertyChanged("EstimationTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Finnished {
            get {
                return this.FinnishedField;
            }
            set {
                if ((this.FinnishedField.Equals(value) != true)) {
                    this.FinnishedField = value;
                    this.RaisePropertyChanged("Finnished");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ToDoService.IToDoService")]
    public interface IToDoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IToDoService/GetToDoList", ReplyAction="http://tempuri.org/IToDoService/GetToDoListResponse")]
        System.Collections.Generic.List<TooDoo.SelfTest.ToDoService.ToDo> GetToDoList(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IToDoService/CreateToDoList", ReplyAction="http://tempuri.org/IToDoService/CreateToDoListResponse")]
        bool CreateToDoList(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IToDoServiceChannel : TooDoo.SelfTest.ToDoService.IToDoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ToDoServiceClient : System.ServiceModel.ClientBase<TooDoo.SelfTest.ToDoService.IToDoService>, TooDoo.SelfTest.ToDoService.IToDoService {
        
        public ToDoServiceClient() {
        }
        
        public ToDoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ToDoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ToDoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ToDoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<TooDoo.SelfTest.ToDoService.ToDo> GetToDoList(string name) {
            return base.Channel.GetToDoList(name);
        }
        
        public bool CreateToDoList(string name) {
            return base.Channel.CreateToDoList(name);
        }
    }
}
