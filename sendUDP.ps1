﻿<#  
.SYNOPSIS 
    Sends a UDP datagram to a port 
.DESCRIPTION 
    This script used system.net.socckets to send a UDP 
    datagram to a particular port. Being UDP, there's 
if the UDP datagram actually 
    was received.  
for this sample, a port was chosen (20000). 
.NOTES 
    File Name  : Send-UDPDatagram 
@psp.co.uk 
    Requires   : PowerShell V2 CTP3 
.LINK 
    http://www.pshscripts.blogspot.com 
.EXAMPLE 
#>  
 
###  
#  Start of Script  
##  
 
# Define port and target IP address  
# Random here! 

Param( 
[int]$Port = 2048,
[string]$IP = "127.0.0.1",
[string]$Message = "{`"type`":`"Fire`",`"x`":0.1,`"y`":0.1,`"z`":0.5}"
)


$Address = [system.net.IPAddress]::Parse( $IP ) 
 
# Create IP Endpoint  
$End = New-Object System.Net.IPEndPoint $address , $port  
 
# Create Socket  
$Saddrf    = [System.Net.Sockets.AddressFamily]::InterNetwork 
$Stype    = [System.Net.Sockets.SocketType]::Dgram 
$Ptype     = [System.Net.Sockets.ProtocolType]::UDP 
$Sock      = New-Object System.Net.Sockets.Socket $saddrf , $stype , $ptype  
$Sock.TTL = 26 
 
# Connect to socket  
$sock.Connect( $end ) 
 
# Create encoded buffer  
$Enc      = [System.Text.Encoding]::ASCII 
$Buffer   = $Enc.GetBytes( $Message ) 
 
# Send the buffer  
$Sent   = $Sock.Send( $Buffer ) 
"{0} characters sent to: {1} " -f $Sent , $IP  
"Message is:"  
$Message  
# End of Script 