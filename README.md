# ChatClientServer
Chat Client Server is a lightweight .NET chat client/server combined app, which can work both locally and remotely

<p align="center">
	<img src="https://github.com/ProudlyTM/ChatClientServer/blob/main/Screenshots/client_server_main_windows.png" alt="Client/Server Main Window"></img><br/><br/>
	<img src="https://github.com/ProudlyTM/ChatClientServer/blob/main/Screenshots/client_server_chat_windows.png" alt="Client/Server Chat Window"></img>
</p>

## Requirements to run the app
* .NET Framework 4.7.2

## Run in Client Mode
To connect to an already started server, you just need to enter the IP (local or public) and Port of the server and then press the "CONNECT" button under the "Connect to a Server" panel. After that you can go to the "Chat" tab and chat with the other person.

## Run in Server Mode
To run in Server Mode, you just need to press the "START" button under the "Start a Server" panel. After that, if you have more than one IP in the list, give the client your IP, corresponding to a subnet in which both of you are.

For example - If the server has a local IP: "192.168.0.10" and the client has an IP in the same subnet: "192.168.0.11", then you need to give them that IP (192.168.0.10) and the randomly picked port to the right. After that, head over to the Chat tab and wait for the client to connect to the server. After the client has connected, you can start chatting with each other.

If you want to chat with a client, that's not in your network, you'll have to open the randomly picked port to WAN. For now, each time you start the app, the port will be randomly picked to an unused one in your system to avoid problems, so you'll have to change the forwarded port accordingly, but this will probably change in future versions.

Please note that if you want to chat with a remote client, you need to make sure you forward the randomly picked port to the correct local IP of your system.
