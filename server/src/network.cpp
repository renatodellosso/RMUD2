#include "network.h"
#include "tcpserver.h"
#include "utils.h"
#include <WinSock2.h>

void networkInit() {
	log("Initting network...");

	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0)
		return exitWithError("WSAStartup failed: " + to_string(WSAGetLastError()));

	Network::tcpServer = http::TcpServer();
	
	if(Network::tcpServer.socketID < 0)
		exitWithError("Socket creation failed");

	log("Network initted");
}