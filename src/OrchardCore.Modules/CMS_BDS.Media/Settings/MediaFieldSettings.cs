using System.ComponentModel;

namespace CMS_BDS.Media.Settings
{
    public class MediaFieldSettings
    {
        public string Hint { get; set; }
        public bool Required { get; set; }

        [DefaultValue(true)]
        public bool Multiple { get; set; } = true;
    }
}