﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonalAnalytics.AccessDataService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AccessDataService.IAccessData")]
    public interface IAccessData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitClientID", ReplyAction="http://tempuri.org/IAccessData/GetFitbitClientIDResponse")]
        string GetFitbitClientID();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitClientID", ReplyAction="http://tempuri.org/IAccessData/GetFitbitClientIDResponse")]
        System.Threading.Tasks.Task<string> GetFitbitClientIDAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitClientSecret", ReplyAction="http://tempuri.org/IAccessData/GetFitbitClientSecretResponse")]
        string GetFitbitClientSecret();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitClientSecret", ReplyAction="http://tempuri.org/IAccessData/GetFitbitClientSecretResponse")]
        System.Threading.Tasks.Task<string> GetFitbitClientSecretAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitFirstAuthorizationCode", ReplyAction="http://tempuri.org/IAccessData/GetFitbitFirstAuthorizationCodeResponse")]
        string GetFitbitFirstAuthorizationCode();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetFitbitFirstAuthorizationCode", ReplyAction="http://tempuri.org/IAccessData/GetFitbitFirstAuthorizationCodeResponse")]
        System.Threading.Tasks.Task<string> GetFitbitFirstAuthorizationCodeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetGarminConsumerKey", ReplyAction="http://tempuri.org/IAccessData/GetGarminConsumerKeyResponse")]
        string GetGarminConsumerKey();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetGarminConsumerKey", ReplyAction="http://tempuri.org/IAccessData/GetGarminConsumerKeyResponse")]
        System.Threading.Tasks.Task<string> GetGarminConsumerKeyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetGarminConsumerSecret", ReplyAction="http://tempuri.org/IAccessData/GetGarminConsumerSecretResponse")]
        string GetGarminConsumerSecret();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccessData/GetGarminConsumerSecret", ReplyAction="http://tempuri.org/IAccessData/GetGarminConsumerSecretResponse")]
        System.Threading.Tasks.Task<string> GetGarminConsumerSecretAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccessDataChannel : PersonalAnalytics.AccessDataService.IAccessData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccessDataClient : System.ServiceModel.ClientBase<PersonalAnalytics.AccessDataService.IAccessData>, PersonalAnalytics.AccessDataService.IAccessData {
        
        public AccessDataClient() {
        }
        
        public AccessDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AccessDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccessDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccessDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetFitbitClientID() {
            return base.Channel.GetFitbitClientID();
        }
        
        public System.Threading.Tasks.Task<string> GetFitbitClientIDAsync() {
            return base.Channel.GetFitbitClientIDAsync();
        }
        
        public string GetFitbitClientSecret() {
            return base.Channel.GetFitbitClientSecret();
        }
        
        public System.Threading.Tasks.Task<string> GetFitbitClientSecretAsync() {
            return base.Channel.GetFitbitClientSecretAsync();
        }
        
        public string GetFitbitFirstAuthorizationCode() {
            return base.Channel.GetFitbitFirstAuthorizationCode();
        }
        
        public System.Threading.Tasks.Task<string> GetFitbitFirstAuthorizationCodeAsync() {
            return base.Channel.GetFitbitFirstAuthorizationCodeAsync();
        }
        
        public string GetGarminConsumerKey() {
            return base.Channel.GetGarminConsumerKey();
        }
        
        public System.Threading.Tasks.Task<string> GetGarminConsumerKeyAsync() {
            return base.Channel.GetGarminConsumerKeyAsync();
        }
        
        public string GetGarminConsumerSecret() {
            return base.Channel.GetGarminConsumerSecret();
        }
        
        public System.Threading.Tasks.Task<string> GetGarminConsumerSecretAsync() {
            return base.Channel.GetGarminConsumerSecretAsync();
        }
    }
}
