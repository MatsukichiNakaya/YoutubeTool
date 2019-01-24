using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace YoutubeTool
{
    [DataContract]
    public class ClientInfo
    {
        [DataMember]
        public ClientIDJson installed { get; set; }
    }


    [DataContract]
    public class ClientIDJson
    {
        [DataMember]
        public String client_id { get; set; }
        [DataMember]
        public String project_id { get; set; }
        [DataMember]
        public String auth_uri { get; set; }
        [DataMember]
        public String token_uri { get; set; }
        [DataMember]
        public String auth_provider_x509_cert_url { get; set; }
        [DataMember]
        public String client_secret { get; set; }
        [DataMember]
        public String[] redirect_uris { get; set; }
    }
}
