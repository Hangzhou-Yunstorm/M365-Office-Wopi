param($Name="UnifyEditor", $Port="8080", $root)

$wopipath = "${root}\UnifyEditor"
$root = "${root}\ps1"
Set-Location $root

#dotnet-hosting-3.1.1-win.exe
#Start-Process ".\dotnet-hosting-3.1.1-win.exe" "/install /passive /quiet" -Wait -PassThru

#创建IIS站点及程序池
Import-Module WebAdministration
New-Item iis:\AppPools\$Name -Force
New-Item iis:\Sites\$Name -bindings @{protocol="http";bindingInformation=":${Port}:${Name}"} -physicalPath $wopipath -Force
Set-ItemProperty IIS:\Sites\$Name -name applicationPool -value $Name

#重启iis
iisreset

#文件夹授权
$acl = Get-Acl $wopipath
$ar = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
$acl.SetAccessRule($ar)
Set-Acl $wopipath $acl
