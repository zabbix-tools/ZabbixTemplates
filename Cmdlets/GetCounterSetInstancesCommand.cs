namespace ZabbixTemplates.Cmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Management.Automation;
    using System.Diagnostics;

    [Cmdlet("Get", "CounterSetInstances")]
    public class GetCounterSetInstancesCommand : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public String CounterSet { get; set; }

        protected override void EndProcessing()
        {
            if (!PerformanceCounterCategory.Exists(CounterSet))
                throw new Exception(String.Format("Cannot find any performance counter sets on the localhost computer that match the following: {0}.", CounterSet));

            var pdhCounterSet = new PerformanceCounterCategory(CounterSet);
            if (pdhCounterSet.CategoryType != PerformanceCounterCategoryType.MultiInstance)
                throw new Exception(String.Format("Performance Counter Set {0} does not have multiple instances", CounterSet));

            var instances = pdhCounterSet.GetInstanceNames();
            foreach (var instance in instances)
            {
                WriteObject(instance);
            }
        }
    }
}
