# ChatClientServer
Chat Client/Server is a lightweight .NET chat client/server combined app, which can work both locally and remotely

<p align="center">
	<img src="https://github.com/ProudlyTM/ChatClientServer/blob/main/Screenshots/client_server_main_windows.png" alt="Client/Server Main Window"></img><br/><br/>
	<img src="https://github.com/ProudlyTM/ChatClientServer/blob/main/Screenshots/client_server_chat_windows.png" alt="Client/Server Chat Window"></img>
</p>

## Requirements to run the app
* .NET Framework 4.7.2

## Run in Client mode
To connect to a server, enter the IP (local or public) and Port of the server and then press the "CONNECT" button under the "Connect to a Server" panel. After that, you can chat with the other client(s) and the server in the "Chat" tab.

## Run in Server mode
To run in Server Mode, press the "START" button under the "Start a Server" panel. If you don't enter a port manually before that, the app will attempt to pick a random unused one for you. After a server has been started, you can chat with the client(s) in the "Chat" tab.

Please note that in order to enable communication with remote clients, the server port must be forwarded to WAN.
