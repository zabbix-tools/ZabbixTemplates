namespace ZabbixTemplates
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Item
    {
        public Item()
        {
            Applications = new ApplicationSet();
        }

        public Item (PerformanceCounter pdhCounter, String instanceName = "") : this() {
            Name = pdhCounter.CounterName;
            Description = pdhCounter.CounterHelp;
            Key = String.IsNullOrEmpty(instanceName) ? 
                String.Format(@"perf_counter[""\{0}\{1}""]", pdhCounter.CategoryName, pdhCounter.CounterName) :
                String.Format(@"perf_counter[""\{0} ({1})\{2}""]", pdhCounter.CategoryName, instanceName, pdhCounter.CounterName);
        }
        
        private int _delay = 60;
        private int _history = 7;
        private int _trends = 365;

        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType ItemType { get; set; }
        public string Key { get; set; }
        public ItemStatus Status { get; set; }
        public ApplicationSet Applications { get; protected set; }

        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        public int History
        {
            get { return _history; }
            set { _history = value; }
        }

        public int Trends 
        {
            get { return _trends; }
            set { _trends = value; }
        }
    }

    public class ItemCollection : List<Item>
    {
        public ItemCollection() : base() { }
    }

    public enum ItemType {
        ZabbixAgent = 0,
        ZabbixAgentActive = 7
    }

    public enum ItemStatus
    {
        Enabled = 0,
        Disabled = 1,
    }
}
