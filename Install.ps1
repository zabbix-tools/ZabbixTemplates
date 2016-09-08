# Create module directory
#$modulePath = "${HOME}\Documents\WindowsPowerShell\Modules\ZabbixTemplates"
$modulePath = "${PSHome}\Modules\ZabbixTemplates"
New-Item -Path $modulePath -Type Directory -ErrorAction SilentlyContinue

# Unload module
Remove-Module -Name ZabbixTemplates # -ErrorAction SilentlyContinue

# Copy module
Copy-Item -Force -Path .\bin\Release\ZabbixTemplates.dll -Destination $modulePath
Copy-Item -Force -Path .\ZabbixTemplates.psd1 -Destination $modulePath

# Test import
Import-Module -Name "ZabbixTemplates" -Verbose