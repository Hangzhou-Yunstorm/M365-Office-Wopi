param($localip = "",$ClusterPrimaryIP="",$Pname="",$Sname="")

Install-WindowsFeature NLB,RSAT-NLB –IncludeManagementTools

$loalip = $localip
$ips = Get-NetIPAddress
$rets = Get-NetAdapter
$InterfaceName = ($rets |? -Property ifIndex -eq ($ips |? -Property ipaddress -EQ $localip).interfaceindex).name

Get-NlbCluster $Pname | Add-NlbClusterNode -NewNodeName $Sname -NewNodeInterface $InterfaceName -Verbose
Get-NlbClusterPortRule | Set-NlbClusterPortRule -NewAffinity None