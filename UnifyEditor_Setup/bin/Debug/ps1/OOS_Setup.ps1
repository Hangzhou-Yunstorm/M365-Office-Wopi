param($root)

$root = "${root}\oos"
$extdir = "${root}\ext"
Set-Location $root

#VC++ 2013
Start-Process ".\vcredist_x64.exe" "/install /passive /quiet" -Wait -PassThru

#VC++ 2015
Start-Process ".\vc_redist.x64.exe" "/install /passive /quiet" -Wait -PassThru

#Microsoft Identity Extension
Start-Process "msiexec" "/i MicrosoftIdentityExtensions-64.msi /qn /passive /quiet" -Wait -PassThru

$iso = "${root}\oos.iso"
$ret = Mount-DiskImage -ImagePath $iso -StorageType ISO -Access ReadOnly -PassThru
$vol = ($ret | Get-Volume).DriveLetter

Start-Process "${vol}:\setup.exe" -ArgumentList "/config `"${root}\oos.xml`"" -Wait -PassThru

Start-Process "${root}\wacserverlanguagepack.exe" "/extract:`"${extdir}`" /quiet" -Wait -PassThru

Start-Process "${extdir}\setup.exe" "/config `"${root}\oos.xml`"" -Wait -PassThru

