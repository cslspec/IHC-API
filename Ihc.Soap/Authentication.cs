﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ihc.Soap.Authentication
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="utcs", ConfigurationName="Ihc.Soap.Authentication.AuthenticationService")]
    public interface AuthenticationService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="ping", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName1> pingAsync(Ihc.Soap.Authentication.inputMessageName1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="authenticate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName2> authenticateAsync(Ihc.Soap.Authentication.inputMessageName2 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="disconnect", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName3> disconnectAsync(Ihc.Soap.Authentication.inputMessageName3 request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class inputMessageName1
    {
        
        public inputMessageName1()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class outputMessageName1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="utcs", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> ping1;
        
        public outputMessageName1()
        {
        }
        
        public outputMessageName1(System.Nullable<bool> ping1)
        {
            this.ping1 = ping1;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="utcs")]
    public partial class WSAuthenticationData
    {
        
        private string passwordField;
        
        private string usernameField;
        
        private string applicationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="utcs")]
    public partial class WSUserGroup
    {
        
        private string typeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="utcs")]
    public partial class WSDate
    {
        
        private int secondsField;
        
        private int yearField;
        
        private int monthWithJanuaryAsOneField;
        
        private int dayField;
        
        private int hoursField;
        
        private int minutesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int seconds
        {
            get
            {
                return this.secondsField;
            }
            set
            {
                this.secondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int monthWithJanuaryAsOne
        {
            get
            {
                return this.monthWithJanuaryAsOneField;
            }
            set
            {
                this.monthWithJanuaryAsOneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int hours
        {
            get
            {
                return this.hoursField;
            }
            set
            {
                this.hoursField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int minutes
        {
            get
            {
                return this.minutesField;
            }
            set
            {
                this.minutesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="utcs")]
    public partial class WSUser
    {
        
        private WSDate createdDateField;
        
        private WSDate loginDateField;
        
        private string usernameField;
        
        private string passwordField;
        
        private string emailField;
        
        private string firstnameField;
        
        private string lastnameField;
        
        private string phoneField;
        
        private WSUserGroup groupField;
        
        private string projectField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public WSDate createdDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public WSDate loginDate
        {
            get
            {
                return this.loginDateField;
            }
            set
            {
                this.loginDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string firstname
        {
            get
            {
                return this.firstnameField;
            }
            set
            {
                this.firstnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string lastname
        {
            get
            {
                return this.lastnameField;
            }
            set
            {
                this.lastnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=8)]
        public WSUserGroup group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string project
        {
            get
            {
                return this.projectField;
            }
            set
            {
                this.projectField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="utcs")]
    public partial class WSLoginResult
    {
        
        private WSUser loggedInUserField;
        
        private bool loginWasSuccessfulField;
        
        private bool loginFailedDueToConnectionRestrictionsField;
        
        private bool loginFailedDueToInsufficientUserRightsField;
        
        private bool loginFailedDueToAccountInvalidField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public WSUser loggedInUser
        {
            get
            {
                return this.loggedInUserField;
            }
            set
            {
                this.loggedInUserField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public bool loginWasSuccessful
        {
            get
            {
                return this.loginWasSuccessfulField;
            }
            set
            {
                this.loginWasSuccessfulField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public bool loginFailedDueToConnectionRestrictions
        {
            get
            {
                return this.loginFailedDueToConnectionRestrictionsField;
            }
            set
            {
                this.loginFailedDueToConnectionRestrictionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool loginFailedDueToInsufficientUserRights
        {
            get
            {
                return this.loginFailedDueToInsufficientUserRightsField;
            }
            set
            {
                this.loginFailedDueToInsufficientUserRightsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public bool loginFailedDueToAccountInvalid
        {
            get
            {
                return this.loginFailedDueToAccountInvalidField;
            }
            set
            {
                this.loginFailedDueToAccountInvalidField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class inputMessageName2
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="utcs", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Ihc.Soap.Authentication.WSAuthenticationData authenticate1;
        
        public inputMessageName2()
        {
        }
        
        public inputMessageName2(Ihc.Soap.Authentication.WSAuthenticationData authenticate1)
        {
            this.authenticate1 = authenticate1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class outputMessageName2
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="utcs", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Ihc.Soap.Authentication.WSLoginResult authenticate2;
        
        public outputMessageName2()
        {
        }
        
        public outputMessageName2(Ihc.Soap.Authentication.WSLoginResult authenticate2)
        {
            this.authenticate2 = authenticate2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class inputMessageName3
    {
        
        public inputMessageName3()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class outputMessageName3
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="utcs", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> disconnect1;
        
        public outputMessageName3()
        {
        }
        
        public outputMessageName3(System.Nullable<bool> disconnect1)
        {
            this.disconnect1 = disconnect1;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface AuthenticationServiceChannel : Ihc.Soap.Authentication.AuthenticationService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class AuthenticationServiceClient : System.ServiceModel.ClientBase<Ihc.Soap.Authentication.AuthenticationService>, Ihc.Soap.Authentication.AuthenticationService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public AuthenticationServiceClient() : 
                base(AuthenticationServiceClient.GetDefaultBinding(), AuthenticationServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.AuthenticationServiceBindingPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthenticationServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(AuthenticationServiceClient.GetBindingForEndpoint(endpointConfiguration), AuthenticationServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthenticationServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(AuthenticationServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthenticationServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(AuthenticationServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthenticationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName1> Ihc.Soap.Authentication.AuthenticationService.pingAsync(Ihc.Soap.Authentication.inputMessageName1 request)
        {
            return base.Channel.pingAsync(request);
        }
        
        public System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName1> pingAsync()
        {
            Ihc.Soap.Authentication.inputMessageName1 inValue = new Ihc.Soap.Authentication.inputMessageName1();
            return ((Ihc.Soap.Authentication.AuthenticationService)(this)).pingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName2> Ihc.Soap.Authentication.AuthenticationService.authenticateAsync(Ihc.Soap.Authentication.inputMessageName2 request)
        {
            return base.Channel.authenticateAsync(request);
        }
        
        public System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName2> authenticateAsync(Ihc.Soap.Authentication.WSAuthenticationData authenticate1)
        {
            Ihc.Soap.Authentication.inputMessageName2 inValue = new Ihc.Soap.Authentication.inputMessageName2();
            inValue.authenticate1 = authenticate1;
            return ((Ihc.Soap.Authentication.AuthenticationService)(this)).authenticateAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName3> Ihc.Soap.Authentication.AuthenticationService.disconnectAsync(Ihc.Soap.Authentication.inputMessageName3 request)
        {
            return base.Channel.disconnectAsync(request);
        }
        
        public System.Threading.Tasks.Task<Ihc.Soap.Authentication.outputMessageName3> disconnectAsync()
        {
            Ihc.Soap.Authentication.inputMessageName3 inValue = new Ihc.Soap.Authentication.inputMessageName3();
            return ((Ihc.Soap.Authentication.AuthenticationService)(this)).disconnectAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.AuthenticationServiceBindingPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.AuthenticationServiceBindingPort))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost/AuthenticationService");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return AuthenticationServiceClient.GetBindingForEndpoint(EndpointConfiguration.AuthenticationServiceBindingPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return AuthenticationServiceClient.GetEndpointAddress(EndpointConfiguration.AuthenticationServiceBindingPort);
        }
        
        public enum EndpointConfiguration
        {
            
            AuthenticationServiceBindingPort,
        }
    }
}
