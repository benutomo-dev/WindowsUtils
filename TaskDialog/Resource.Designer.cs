﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsControls {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WindowsControls.Resource", typeof(Resource).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   This TaskDialogPage is already showing. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string AlreadyShowing {
            get {
                return ResourceManager.GetString("AlreadyShowing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   comctl32.dll is old. You must configure &quot;app.manifest&quot;. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ComCtl32VersionError {
            get {
                return ResourceManager.GetString("ComCtl32VersionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Since changing the footer icon after navigating can not be done normally it is prohibited. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string FooterIconModificationAfterNavigate {
            get {
                return ResourceManager.GetString("FooterIconModificationAfterNavigate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   This icon is incompatible. You can not change standard icons and user definded icons in ActiveTaskDialog before navigate. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string IncompatibleIconError {
            get {
                return ResourceManager.GetString("IncompatibleIconError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   This icon is incompatible. You have to use standard icons only after navigated. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string IncompatibleIconForNavigatedError {
            get {
                return ResourceManager.GetString("IncompatibleIconForNavigatedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   To modify expand information is required set ExpandedInformationText at showing. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidExpandedInformationModification {
            get {
                return ResourceManager.GetString("InvalidExpandedInformationModification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   To modify footter information is required set FooterText at showing. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidFooterModification {
            get {
                return ResourceManager.GetString("InvalidFooterModification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   To modify progress information is requered set ProgressBar at showing. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidProgressBarModification {
            get {
                return ResourceManager.GetString("InvalidProgressBarModification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   To use CommanLink is required more than one TaskButtons. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidTaskButtonStyle {
            get {
                return ResourceManager.GetString("InvalidTaskButtonStyle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   To use TaskDialog is required Vista or more than newer Windows. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string OSVersionError {
            get {
                return ResourceManager.GetString("OSVersionError", resourceCulture);
            }
        }
    }
}
