﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAMEL.Baking.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.2.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://MEAPVIP.soundnewenergy.net:50000/sapdevwebservice/MachineAccessTestService" +
            "Service")]
        public string Anchitech_Dispatcher_MachineAccessWebReference_MachineAccessTestServiceService {
            get {
                return ((string)(this["Anchitech_Dispatcher_MachineAccessWebReference_MachineAccessTestServiceService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://MEAPVIP.soundnewenergy.net:50000/sapdevwebservice/ExecutingServiceService")]
        public string Anchitech_Dispatcher_ExecutingWebReference_ExecutingServiceService {
            get {
                return ((string)(this["Anchitech_Dispatcher_ExecutingWebReference_ExecutingServiceService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://192.168.100.11:8094/EquipService.asmx")]
        public string Anchitech_Baking_MesService_EquipService {
            get {
                return ((string)(this["Anchitech_Baking_MesService_EquipService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.21.30.251:9010/DeviceStatusRecordService.asmx")]
        public string CAMEL_Baking_DeviceStatusRecordService_DeviceStatusRecordService {
            get {
                return ((string)(this["CAMEL_Baking_DeviceStatusRecordService_DeviceStatusRecordService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.21.30.251:9010/IdentityVerificationService.asmx")]
        public string CAMEL_Baking_IdentityVerificationService_IdentityVerificationService {
            get {
                return ((string)(this["CAMEL_Baking_IdentityVerificationService_IdentityVerificationService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.21.30.251:9010/ProductionDataUploadService.asmx")]
        public string CAMEL_Baking_ProductionDataUploadService_ProductionDataUploadService {
            get {
                return ((string)(this["CAMEL_Baking_ProductionDataUploadService_ProductionDataUploadService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.21.30.251:9010/TrayBindingService.asmx")]
        public string CAMEL_Baking_TrayBindingService_TrayBindingService {
            get {
                return ((string)(this["CAMEL_Baking_TrayBindingService_TrayBindingService"]));
            }
        }
    }
}
