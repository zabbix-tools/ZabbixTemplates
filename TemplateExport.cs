namespace ZabbixTemplates
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class TemplateExport
    {
        public TemplateExport()
        {
            Templates = new TemplateCollection();
            Groups = new GroupSet();
        }

        private string _version = "3.0";

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string Date { 
            get { return DateTime.UtcNow.ToString(@"yyyy-MM-dd\THH:mm:ss\Z"); }
        }

        public TemplateCollection Templates { get; protected set; }

        public GroupSet Groups { get; protected set; }

        #region Serialization

        public XmlDocument ToXmlDocument()
        {
            // create document
            var doc = new XmlDocument();
            var dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.InsertBefore(dec, doc.DocumentElement);

            // create root
            var root = doc.AppendChild(doc.CreateElement("zabbix_export"));
            root.AppendChild(doc.CreateElement("version")).InnerText = Version;
            root.AppendChild(doc.CreateElement("date")).InnerText = Date;

            // append groups
            var groupsNode = root.AppendChild(doc.CreateElement("groups"));
            foreach (var group in Groups)
            {
                groupsNode.AppendChild(doc.CreateElement("group")).AppendChild(doc.CreateElement("name")).InnerText = group;
            }

            // append templates
            var templatesNode = root.AppendChild(doc.CreateElement("templates"));
            foreach (var template in Templates)
            {
                var templateNode = templatesNode.AppendChild(doc.CreateElement("template"));
                templateNode.AppendChild(doc.CreateElement("template")).InnerText = template.Name;
                templateNode.AppendChild(doc.CreateElement("name")).InnerText = template.Name;
                templateNode.AppendChild(doc.CreateElement("description")).InnerText = template.Description;

                // append template groups
                var templateGroupsNode = templateNode.AppendChild(doc.CreateElement("groups"));
                foreach (var group in template.Groups)
                {
                    templateGroupsNode.AppendChild(doc.CreateElement("group")).AppendChild(doc.CreateElement("name")).InnerText = group;
                }

                // append template applications
                var applicationsNode = templateNode.AppendChild(doc.CreateElement("applications"));
                foreach (var app in template.Applications)
                {
                    applicationsNode.AppendChild(doc.CreateElement("application")).AppendChild(doc.CreateElement("name")).InnerText = app;
                }

                // append template items
                var itemsNode = templateNode.AppendChild(doc.CreateElement("items"));
                foreach (var item in template.Items)
                {
                    var itemNode = itemsNode.AppendChild(doc.CreateElement("item"));

                    itemNode.AppendChild(doc.CreateElement("name")).InnerText = item.Name;
                    itemNode.AppendChild(doc.CreateElement("description")).InnerText = item.Description;
                    itemNode.AppendChild(doc.CreateElement("type")).InnerText = ((int)item.ItemType).ToString();
                    itemNode.AppendChild(doc.CreateElement("key")).InnerText = item.Key;
                    itemNode.AppendChild(doc.CreateElement("delay")).InnerText = item.Delay.ToString();
                    itemNode.AppendChild(doc.CreateElement("history")).InnerText = item.History.ToString();
                    itemNode.AppendChild(doc.CreateElement("trends")).InnerText = item.Trends.ToString();
                    itemNode.AppendChild(doc.CreateElement("status")).InnerText = ((int)item.Status).ToString();

                    // append item applications
                    var itemApplicationsNode = itemNode.AppendChild(doc.CreateElement("applications"));
                    foreach (var app in item.Applications)
                    {
                        itemApplicationsNode.AppendChild(doc.CreateElement("application")).AppendChild(doc.CreateElement("name")).InnerText = app;
                    }

                    // dummy values
                    itemNode.AppendChild(doc.CreateElement("allowed_hosts"));
                    itemNode.AppendChild(doc.CreateElement("authtype")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("data_type")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("delay_flex"));
                    itemNode.AppendChild(doc.CreateElement("delta")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("formula")).InnerText = "1";
                    itemNode.AppendChild(doc.CreateElement("inventory_link")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("ipmi_sensor"));
                    itemNode.AppendChild(doc.CreateElement("logtimefmt"));
                    itemNode.AppendChild(doc.CreateElement("multiplier")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("params"));
                    itemNode.AppendChild(doc.CreateElement("password"));
                    itemNode.AppendChild(doc.CreateElement("port"));
                    itemNode.AppendChild(doc.CreateElement("privatekey"));
                    itemNode.AppendChild(doc.CreateElement("publickey"));
                    itemNode.AppendChild(doc.CreateElement("snmp_community"));
                    itemNode.AppendChild(doc.CreateElement("snmp_oid"));
                    itemNode.AppendChild(doc.CreateElement("snmpv3_authpassphrase"));
                    itemNode.AppendChild(doc.CreateElement("snmpv3_authprotocol")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("snmpv3_contextname"));
                    itemNode.AppendChild(doc.CreateElement("snmpv3_privpassphrase"));
                    itemNode.AppendChild(doc.CreateElement("snmpv3_privprotocol")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("snmpv3_securitylevel")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("snmpv3_securityname"));
                    itemNode.AppendChild(doc.CreateElement("units"));
                    itemNode.AppendChild(doc.CreateElement("username"));
                    itemNode.AppendChild(doc.CreateElement("value_type")).InnerText = "0";
                    itemNode.AppendChild(doc.CreateElement("valuemap"));
                }

                // append Discovery rules
                var discoveryRulesNode = templateNode.AppendChild(doc.CreateElement("discovery_rules"));
                foreach (var discoveryRule in template.DiscoveryRules)
                {
                    var discoveryRuleNode = discoveryRulesNode.AppendChild(doc.CreateElement("discovery_rule"));

                    discoveryRuleNode.AppendChild(doc.CreateElement("name")).InnerText = discoveryRule.Name;
                    discoveryRuleNode.AppendChild(doc.CreateElement("description")).InnerText = discoveryRule.Description;
                    discoveryRuleNode.AppendChild(doc.CreateElement("type")).InnerText = ((int)discoveryRule.ItemType).ToString();
                    discoveryRuleNode.AppendChild(doc.CreateElement("key")).InnerText = discoveryRule.Key;
                    discoveryRuleNode.AppendChild(doc.CreateElement("delay")).InnerText = discoveryRule.Delay.ToString();
                    discoveryRuleNode.AppendChild(doc.CreateElement("status")).InnerText = ((int)discoveryRule.Status).ToString();
                    discoveryRuleNode.AppendChild(doc.CreateElement("lifetime")).InnerText = ((int)discoveryRule.Lifetime).ToString();

                    discoveryRuleNode.AppendChild(doc.CreateElement("trigger_prototypes"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("graph_prototypes"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("host_prototypes"));

                    // dummy values
                    discoveryRuleNode.AppendChild(doc.CreateElement("allowed_hosts"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("authtype")).InnerText = "0";
                    discoveryRuleNode.AppendChild(doc.CreateElement("delay_flex"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("ipmi_sensor"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("params"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("password"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("port"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("privatekey"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("publickey"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmp_community"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmp_oid"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_authpassphrase"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_authprotocol")).InnerText = "0";
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_contextname"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_privpassphrase"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_privprotocol")).InnerText = "0";
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_securitylevel")).InnerText = "0";
                    discoveryRuleNode.AppendChild(doc.CreateElement("snmpv3_securityname"));
                    discoveryRuleNode.AppendChild(doc.CreateElement("username"));

                    var filterNode = discoveryRuleNode.AppendChild(doc.CreateElement("filter"));
                    filterNode.AppendChild(doc.CreateElement("evaltype")).InnerText = "0";
                    filterNode.AppendChild(doc.CreateElement("formula"));
                    filterNode.AppendChild(doc.CreateElement("conditions"));


                    var itemPrototypesNode = discoveryRuleNode.AppendChild(doc.CreateElement("item_prototypes"));
                    foreach (var itemPrototype in discoveryRule.ItemPrototypes)
                    {
                        var itemPrototypeNode = itemPrototypesNode.AppendChild(doc.CreateElement("item_prototype"));

                        itemPrototypeNode.AppendChild(doc.CreateElement("name")).InnerText = itemPrototype.Name;
                        itemPrototypeNode.AppendChild(doc.CreateElement("description")).InnerText = itemPrototype.Description;
                        itemPrototypeNode.AppendChild(doc.CreateElement("type")).InnerText = ((int)itemPrototype.ItemType).ToString();
                        itemPrototypeNode.AppendChild(doc.CreateElement("key")).InnerText = itemPrototype.Key;
                        itemPrototypeNode.AppendChild(doc.CreateElement("delay")).InnerText = itemPrototype.Delay.ToString();
                        itemPrototypeNode.AppendChild(doc.CreateElement("history")).InnerText = itemPrototype.History.ToString();
                        itemPrototypeNode.AppendChild(doc.CreateElement("trends")).InnerText = itemPrototype.Trends.ToString();
                        itemPrototypeNode.AppendChild(doc.CreateElement("status")).InnerText = ((int)itemPrototype.Status).ToString();

                        // append item applications
                        var itemApplicationsNode = itemPrototypeNode.AppendChild(doc.CreateElement("applications"));
                        foreach (var app in itemPrototype.Applications)
                        {
                            itemApplicationsNode.AppendChild(doc.CreateElement("application")).AppendChild(doc.CreateElement("name")).InnerText = app;
                        }

                        // dummy values
                        itemPrototypeNode.AppendChild(doc.CreateElement("allowed_hosts"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("application_prototypes"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("authtype")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("data_type")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("delay_flex"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("delta")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("formula")).InnerText = "1";
                        itemPrototypeNode.AppendChild(doc.CreateElement("inventory_link")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("ipmi_sensor"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("logtimefmt"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("multiplier")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("params"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("password"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("port"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("privatekey"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("publickey"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmp_community"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmp_oid"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_authpassphrase"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_authprotocol")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_contextname"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_privpassphrase"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_privprotocol")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_securitylevel")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("snmpv3_securityname"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("units"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("username"));
                        itemPrototypeNode.AppendChild(doc.CreateElement("value_type")).InnerText = "0";
                        itemPrototypeNode.AppendChild(doc.CreateElement("valuemap"));
                    }
                }
            }

            return doc;
        }

        public void ToXmlStream(StreamWriter w)
        {
            // write doc header
            w.WriteLine(String.Format(
@"<?xml version=""1.0"" encoding=""{0}""?>
<zabbix_export>
    <version>{1}</version>
    <date>{2}</date>
    <groups />
    <templates>", w.Encoding.WebName, Version, Date));

            // write each template
            foreach(var template in Templates)
            {
                w.WriteLine(String.Format(
@"        <template>
            <template>{0}</template>
            <name>{0}</name>
            <description>{1}</description>
        </template>", template.Name, template.Description));
            }
            
            // close templates section
            w.WriteLine("   </templates>");

            // close document
            w.WriteLine("</zabbix_export>");
        }

        public string ToXmlString()
        {
            string result;
            /*

            // write XML data to memory
            using (var m = new MemoryStream()) {
                using (var w = new StreamWriter(m, Encoding.UTF8))
                {
                    ToXmlStream(w);
                    w.Flush();
                    
                    m.Position = 0;
                    using (var r = new StreamReader(m, Encoding.UTF8))
                    {
                        result = r.ReadToEnd();
                    }
                }
            }

            return result;
             * */
            using (var w = new StringWriter()) {
                var doc = ToXmlDocument();
                doc.Save(w);
                result = w.ToString();
            }

            return result;
        }

        #endregion
    }
}
