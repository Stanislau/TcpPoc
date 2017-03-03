#TCP POC

##Description

This POC describes how to arrange a streaming (potentially life streaming) using TCP or UDP
from server to client.

Technologie stack:
* Xamarin Android and iOS
* Microsoft Azure Worker Role
* TCP/UDP socket connection

##Solution structure and how to make it works

Solution contains AzureServer and TcpServerRole projects. They are our server and AzureServer
should be deployed into Azure Cloud. Make sure you are using 4088 port in the AzureServer WorkerRole
settings endpoint.

TcpPoc.Server is a console application and could be ran as usual console application.
Quite good to demonstrate the POC work for android and to debug.
For stable work copy 1.wav file from Materials into TcpPoc.Server\bin\Debug folder.

ConsoleClient is a just utility project for smoke tests and for connection test.

TcpPoc.Apple and TcpPoc.Droid are the client implementations. Make sure you are using the right ip.
You could know what ip to use from Azure Panel. The ip is the same as a public ip of TcpServerRole.

##Future plans

This POC should be promoted to life streaming UDP client, but I haven't got enough time to do this.
Features to be implemented:

* Connection rooms
* Voise recording
* Fault tolerans and memory management