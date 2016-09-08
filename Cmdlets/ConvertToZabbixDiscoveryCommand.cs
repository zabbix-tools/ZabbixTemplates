namespace ZabbixTemplates.Cmdlets
{
    using System.Management.Automation;
    using System.Text;

    [Cmdlet(VerbsData.ConvertTo, "ZabbixDiscovery")]
    public class ConvertToZabbixDiscoveryCommand :Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public object InputObject { get; set; }

        [Parameter()]
        public string DefaultMacroName
        {
            get { return _defaultMacroName; }
            set { _defaultMacroName = value; }
        }

        private StringBuilder _sb;
        private bool _isFirstRecord = true;

        private string _defaultMacroName = "INSTANCE";

        protected override void BeginProcessing()
        {
            _sb = new StringBuilder();
            _sb.Append(@"{""data"":[");
        }

        protected override void ProcessRecord()
        {
            // append a comma
            if (!_isFirstRecord)
                _sb.Append(",");

            _isFirstRecord = false;

            // append macros
            _sb.AppendFormat(@"{{""{{#{0}}}"":""{1}""}}", DefaultMacroName.ToUpperInvariant(), InputObject);
        }

        protected override void EndProcessing()
        {
            _sb.Append("]}");
            WriteObject(_sb.ToString());
        }
    }
}
