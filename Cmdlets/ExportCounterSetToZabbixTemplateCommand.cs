namespace Cmdlets
{
    using System;
    using System.Diagnostics;
    using System.Management.Automation;
    using ZabbixTemplates;

    [Cmdlet(VerbsData.Export, "CounterSetToZabbixTemplate")]
    public class ExportCounterSetToZabbixTemplateCommand : Cmdlet
    {
        #region Private fields

        private TemplateExport _export;
        private Template _template;

        private string _computerName = ".";
        private string _instanceName = "";
        private string _templateName = "";
        private string _templateGroup = "Templates";
        private int _checkDelay = 60;
        private int _discoveryDelay = 3600;
        private int _discoveryLifetime = 30;
        private int _historyRetention = 7;
        private int _trendsRetention = 365;
        private bool _enableItems = true;
        private bool _activeChecks = true;

        #endregion

        #region Parameters

        /// <summary>
        /// The help
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string[] CounterSet { get; set; }

        [Parameter()]
        public string ComputerName
        {
            get { return _computerName; }
            set { _computerName = value; }
        }

        [Parameter()]
        public string InstanceName
        {
            get { return _instanceName; }
            set { _instanceName = value; }
        }

        [Parameter()]
        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }

        [Parameter()]
        public string TemplateGroup
        {
            get { return _templateGroup; }
            set { _templateGroup = value; }
        }

        [Parameter()]
        public int CheckDelay
        {
            get { return _checkDelay; }
            set { _checkDelay = value; }
        }

        [Parameter()]
        public int DiscoveryDelay
        {
            get { return _discoveryDelay; }
            set { _discoveryDelay = value; }
        }

        [Parameter()]
        public int DiscoveryLifetime
        {
            get { return _discoveryLifetime; }
            set { _discoveryLifetime = value; }
        }

        [Parameter()]
        public int HistoryRetention
        {
            get { return _historyRetention; }
            set { _historyRetention = value; }
        }

        [Parameter()]
        public int TrendsRetention
        {
            get { return _trendsRetention; }
            set { _trendsRetention = value; }
        }

        [Parameter()]
        public bool EnableItems
        {
            get { return _enableItems; }
            set { _enableItems = value; }
        }

        [Parameter()]
        public bool ActiveChecks
        {
            get { return _activeChecks; }
            set { _activeChecks = value; }
        }

        #endregion

        #region Processing

        protected override void BeginProcessing()
        {
            // create export document
            _export = new TemplateExport();
            _export.Groups.Add(_templateGroup);
            
            // create default template
            var hostname = String.IsNullOrEmpty(_computerName) || _computerName == "." ? System.Environment.MachineName : _computerName;
            _template = new Template
            {
                Name = String.IsNullOrEmpty(_templateName) ? String.Format("Template Performance Counters on {0}", hostname) : _templateName,
                Description = "Generated with Export-CounterSetToZabbixTemplate",
            };

            _template.Groups.Add(_templateGroup);

            _export.Templates.Add(_template);
        }

        protected override void ProcessRecord()
        {
            foreach (string counterSet in CounterSet)
            {
                if (!PerformanceCounterCategory.Exists(counterSet))
                    throw new Exception(String.Format("Performance Counter Set not found: {0}.", counterSet));

                var pdhCategory = new PerformanceCounterCategory(counterSet, ComputerName);
                
                // append application
                var appName = String.IsNullOrEmpty(_instanceName) ? pdhCategory.CategoryName : String.Format("{0} ({1})", pdhCategory.CategoryName, _instanceName);
                _template.Applications.Add(appName);
                
                // append a single instance of a counter set
                if (pdhCategory.CategoryType == PerformanceCounterCategoryType.SingleInstance || !String.IsNullOrEmpty(_instanceName))
                {
                    foreach (var pdhCounter in pdhCategory.GetCounters())
                    {
                        if (pdhCounter.IsValidForExport())
                        {
                            var item = new Item(pdhCounter, InstanceName)
                            {
                                ItemType = ActiveChecks ? ItemType.ZabbixAgentActive : ItemType.ZabbixAgent,
                                Status = EnableItems ? ItemStatus.Enabled : ItemStatus.Disabled,
                                Delay = CheckDelay,
                                History = HistoryRetention,
                                Trends = TrendsRetention,
                            };

                            item.Applications.Add(appName);

                            _template.Items.Add(item);
                        }
                    }
                }

                else
                {
                    var discoveryRule = new DiscoveryRule(pdhCategory)
                    {
                        ItemType = ActiveChecks ? ItemType.ZabbixAgentActive : ItemType.ZabbixAgent,
                        Status = EnableItems ? ItemStatus.Enabled : ItemStatus.Disabled,
                        Delay = DiscoveryDelay,
                        Lifetime = DiscoveryLifetime,
                    };

                    // configure item prototypes
                    foreach (var item in discoveryRule.ItemPrototypes)
                    {                        
                        item.ItemType = ActiveChecks ? ItemType.ZabbixAgentActive : ItemType.ZabbixAgent;
                        item.Status = EnableItems ? ItemStatus.Enabled: ItemStatus.Disabled;
                        item.Delay = CheckDelay;
                        item.History = HistoryRetention;
                        item.Trends = TrendsRetention;
                        item.Applications.Add(appName);
                    }

                    _template.DiscoveryRules.Add(discoveryRule);
                }
            }
        }

        protected override void EndProcessing()
        {
            WriteObject(_export.ToXmlString());
        }

        #endregion
    }
}
