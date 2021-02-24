param($localip = "",$ClusterPrimaryIP="")

Install-WindowsFeature NLB,RSAT-NLB –IncludeManagementTools

$loalip = $localip
$ips = Get-NetIPAddress
$rets = Get-NetAdapter
$InterfaceName = ($rets |? -Property ifIndex -eq ($ips |? -Property ipaddress -EQ $localip).interfaceindex).name

New-NlbCluster -ClusterName UnifyEditNLB -InterfaceName $InterfaceName -ClusterPrimaryIP $ClusterPrimaryIP -SubnetMask 255.255.255.0 -OperationMode multicast
Get-NlbClusterPortRule | Set-NlbClusterPortRule -NewAffinity None