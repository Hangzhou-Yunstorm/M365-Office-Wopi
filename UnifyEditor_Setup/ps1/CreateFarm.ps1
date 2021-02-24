param($IntetnalURL, $ExternalUrl="", $root)

$watermark = "${root}\WaterMark"
$root = "${root}\oos"
Set-Location $root

$env:PSModulePath = "{0};{1}" -f [System.Environment]::GetEnvironmentVariable("PSModulePath","Machine"), 
    [System.Environment]::GetEnvironmentVariable("PSModulePath","User")

Import-Module -Name OfficeWebApps

function my-oos-patch () {
    $name = $env:COMPUTERNAME.ToUpper()
    "HKLM:\SYSTEM\ControlSet001\Control\ComputerName\ActiveComputerName",
    "HKLM:\SYSTEM\ControlSet001\Control\ComputerName\ComputerName",
    "HKLM:\SYSTEM\ControlSet002\Control\ComputerName\ComputerName" |% {
        Set-ItemProperty $_ ComputerName $name
    }
    $path = "HKLM:\System\CurrentControlSet\Services\TCPIP\Parameters"
    "Hostname", "NV Hostname" |% {
        Set-ItemProperty $path $_ $name
    }
}

my-oos-patch

if ($ExternalUrl -eq ""){
Write-Output "IntetnalURL=$IntetnalURL"
New-OfficeWebAppsFarm -InternalURL $IntetnalURL -AllowHttp -EditingEnabled -OpenFromUrlEnabled -Force
}
else {
Write-Output "IntetnalURL=$IntetnalURL"
Write-Output "ExternalUrl=$ExternalUrl"
New-OfficeWebAppsFarm -InternalUrl $IntetnalURL -ExternalUrl $ExternalUrl -SSLOffloaded -EditingEnabled -OpenFromUrlEnabled -Force
}

#重启iis
iisreset

#拷贝文件（水印）
Copy-Item "${watermark}\xlviewerinternal.aspx" -Destination "C:\Program Files\Microsoft Office Web Apps\ExcelServicesWfe\_layouts"
Copy-Item "${watermark}\ExcelWaterMark.js" -Destination "C:\Program Files\Microsoft Office Web Apps\ExcelServicesWfe\_layouts\App_Scripts"
Copy-Item "${watermark}\jquery.min.js" -Destination "C:\Program Files\Microsoft Office Web Apps\ExcelServicesWfe\_layouts\App_Scripts"
Copy-Item "${watermark}\powerpointframe.aspx" -Destination "C:\Program Files\Microsoft Office Web Apps\WebPPTViewer"
Copy-Item "${watermark}\jquery.min.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebPPTViewer\App_Scripts"
Copy-Item "${watermark}\PPTWaterMark.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebPPTViewer\App_Scripts"
Copy-Item "${watermark}\wordviewerframe.aspx" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordViewer"
Copy-Item "${watermark}\jquery.min.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordViewer\App_Scripts"
Copy-Item "${watermark}\WordWaterMark.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordViewer\App_Scripts"
Copy-Item "${watermark}\WordEditorFrame.aspx" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordEditor"
Copy-Item "${watermark}\jquery.min.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordEditor\App_Scripts"
Copy-Item "${watermark}\WordEditWaterMark.js" -Destination "C:\Program Files\Microsoft Office Web Apps\WebWordEditor\App_Scripts"
Copy-Item "${watermark}\web.config" -Destination "C:\Program Files\Microsoft Office Web Apps\RootWebsite"

