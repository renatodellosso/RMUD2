#include "network.h"
#include "tcpserver.h"
#include "utils.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif

http::TcpServer Network::tcpServer;

void networkInit() {
	log("Initting network...");

	Network::tcpServer.~TcpServer();

	Network::tcpServer = http::TcpServer();

	log("Created socket: " + to_string(Network::tcpServer.socketID));

	Network::tcpServer.startListen();

	log("Network initted");
}

void startWSA() {
#ifdef _WIN32
	log("Starting WSA...");

	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0)
		exitWithError("WSAStartup failed: " + to_string(WSAGetLastError()));

	log("Started WSA. Version: " + to_string(wsaData.wVersion));
#endif
}