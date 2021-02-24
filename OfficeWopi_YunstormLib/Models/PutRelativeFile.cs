using System.Runtime.Serialization;

namespace OfficeWopi_YunstormLib.Models
{
    [DataContract(Name = "PutRelativeFile")]
    public class PutRelativeFile
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Url")]
        public string Url { get; set; }

        [DataMember(Name = "HostViewUrl")]
        public string HostViewUrl { get; set; }

        [DataMember(Name = "HostEditUrl")]
        public string HostEditUrl { get; set; }
    }

}
