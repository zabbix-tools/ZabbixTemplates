#$modulePath = "${HOME}\Documents\WindowsPowerShell\Modules\ZabbixTemplates"
$modulePath = "C:\Windows\System32\WindowsPowerShell\Modules\ZabbixTemplates"
New-Item -Path $modulePath -Type Directory -ErrorAction SilentlyContinue
Copy-Item -Force -Source .\bin\Release\ZabbixTemplates.dll -Destination $modulePath
Copy-Item -Force -Source .\ZabbixTemplates.psd1 -Destination $modulePath
Import-Module -Name "ZabbixTemplates" -Verbose