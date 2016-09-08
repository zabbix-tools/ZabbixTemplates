namespace ZabbixTemplates
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DiscoveryRule
    {
        public DiscoveryRule()
        {
            ItemPrototypes = new ItemCollection();
        }

        public DiscoveryRule(PerformanceCounterCategory pdhCategory) : this()
        {
            Name = String.Format("{0} discovery", pdhCategory.CategoryName);
            Description = String.Format(@"{0}

* Note*
This discovery rule requires the 'perf_counter.discovery[]' key to be configured on the remote agent to execute the 'Get-CounterSetInstances.ps1' PowerShell script.
", pdhCategory.CategoryHelp);

            Key = String.Format(@"perf_counter.discovery[""{0}""]", pdhCategory.CategoryName);

            /*
             * If instances exist on the source machine, search counters on each instance. 
             * Otherwise just call GetCounters() on an empty instance.
             */
            var counters = new Dictionary<String, PerformanceCounter>();
            var instances = pdhCategory.GetInstanceNames();
            if (instances.Length > 0)
            {
                foreach (var instance in instances)
                {
                    foreach (var pdhCounter in pdhCategory.GetCounters(instance))
                    {
                        if(pdhCounter.IsValidForExport())
                            counters[pdhCounter.CounterName] = pdhCounter;
                    }
                }
            }
            else
            {
                foreach (var pdhCounter in pdhCategory.GetCounters())
                {
                    if(pdhCounter.IsValidForExport())
                        counters[pdhCounter.CounterName] = pdhCounter;
                }

            }

            foreach(var pdhCounter in counters.Values)
            {
                ItemPrototypes.Add(new Item{
                    Name = String.Format("{0} on {{#INSTANCE}}", pdhCounter.CounterName),
                    Key = String.Format(@"perf_counter[""\{0}({{#INSTANCE}})\{1}""]", pdhCategory.CategoryName, pdhCounter.CounterName),
                    Description = pdhCounter.CounterHelp,
                });
            }
        }

        private int _delay = 3600;
        private int _lifetime = 30;

        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType ItemType { get; set; }
        public string Key { get; set; }
        public ItemStatus Status { get; set; }
        public ItemCollection ItemPrototypes {get; protected set;}

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public int Lifetime
        {
            get { return _lifetime; }
            set { _lifetime = value; }
        }
    }

    public class DiscoveryRuleCollection : List<DiscoveryRule>
    {
        public DiscoveryRuleCollection() : base() { }
    }
}
