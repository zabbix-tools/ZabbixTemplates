# ZabbixTemplates

A PowerShell module for working with Zabbix Templates

## Installation

To use this module from within the Zabbix agent, is must be loaded by default
for all users. This means you can't load the module using a profile script or
from a particular user's Documents directory.

* Create the following directory:
  `$PSHome\Modules\ZabbixTemplates`
* Build the `Release` configuration of the module library using
  Visual Studio 2016
* Copy the following files directly into the created Modules directory:

  * `bin/Release/ZabbixTemplates.dll`
  * `ZabbixTemplates.psd1`

* Open a new PowerShell session and test that the module is loaded correctly
  with:

      > Get-Module -Name ZabbixTemplates


## Zabbix agent configuration

Any Zabbix Templates generated with `Export-CounterSetToZabbixTemplate` requires
the `perf_counter.discovery` user parameter key if the template includes
discovery rules.

This key is currently not built in to the Zabbix Windows agent. See Zabbix
feature request [ZBXNEXT-2247](https://support.zabbix.com/browse/ZBXNEXT-2247)
for more information.

To enable the required keys, first install this module on any agent system
you wish to monitor. Then add the following User Parameter to the Zabbix agent
configuration file on the same systems:

	UserParameter=perf_counter.discovery[*],%systemroot%\system32\windowspowershell\v1.0\powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "Get-CounterSetInstances -CounterSet $1 | ConvertTo-ZabbixDiscovery"


Test the configuration using `zabbix_get`:

	$ zabbix_get -s 127.0.0.1 -k perf_counter.discovery[LogicalDisk]

