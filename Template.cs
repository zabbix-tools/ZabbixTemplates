namespace ZabbixTemplates
{
    using System.Collections.Generic;

    public class Template
    {
        public Template()
        {
            Items = new ItemCollection();
            DiscoveryRules = new DiscoveryRuleCollection();
            Groups = new GroupSet();
            Applications = new ApplicationSet();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public ItemCollection Items { get; protected set; }
        public DiscoveryRuleCollection DiscoveryRules { get; protected set; }
        public GroupSet Groups { get; protected set; }
        public ApplicationSet Applications { get; protected set; }
    }

    public class TemplateCollection : List<Template>
    {
        public TemplateCollection() : base() { }
    }
}
