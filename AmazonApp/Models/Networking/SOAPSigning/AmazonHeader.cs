using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace AmazonApp.Models.Networking.SOAPSigning
{

    public class AmazonHeader : MessageHeader
    {
        private string m_name;
        private string value;

        public AmazonHeader(string name, string value)
        {
            this.m_name = name;
            this.value = value;
        }

        public override string Name
        {
            get { return m_name; }
        }
        public override string Namespace
        {
            get { return "http://security.amazonaws.com/doc/2007-01-01/"; }
        }
        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteString(value);
        }
    }
}