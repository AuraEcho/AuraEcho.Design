using System.ComponentModel;
using System.Globalization;
using AuraEcho.Design.Strings;

namespace AuraEcho.Design.Localization
{
    /// <summary>
    /// AuraEcho.Design 的本地化中枢。
    /// 所有 Design 控件模板里需要本地化的文本都应绑定到 <see cref="Current"/>，
    /// 宿主在切换语言时调用 <see cref="ChangeCulture(CultureInfo)"/>（或设置 <see cref="Culture"/>），
    /// 中枢发出一次 <see cref="INotifyPropertyChanged.PropertyChanged"/>，
    /// 所有绑定它的控件同时刷新——不依赖任何特定 UI 时机。
    /// <para>
    /// 文本本身由强类型资源类 <see cref="DesignStrings"/> 提供（对应 Strings/DesignStrings.resx）。
    /// 新增控件文本时，只需往 resx 加键、在本类加一个转发属性即可。
    /// </para>
    /// </summary>
    public sealed class DesignLocalization : INotifyPropertyChanged
    {
        public static DesignLocalization Current { get; } = new DesignLocalization();

        private DesignLocalization()
        {
            DesignStrings.Culture = CultureInfo.CurrentUICulture;
            Strings = new DesignStrings();
        }

        public DesignStrings Strings { get; set; }

        /// <summary>
        /// 当前本地化区域
        /// </summary>
        public CultureInfo Culture
        {
            get => DesignStrings.Culture;
            set
            {
                if (Equals(DesignStrings.Culture, value) || value == null)
                    return;

                DesignStrings.Culture = value;
                Current.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static string GetString(string name)
            => DesignStrings.ResourceManager.GetString(name, DesignStrings.Culture);
    }
}
