#include "network.h"
#include "tcpserver.h"
#include "utils.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif

namespace network {

	TcpServer tcpServer;

	void networkInit() {
		log("Initting network...");

		//(& network::tcpServer).~TcpServer(); //Destroy the old one if it exists

		log("Creating socket...");
		network::tcpServer = new TcpServer(true); //Make sure to use new

		log("Created TcpServer: " + to_string(network::tcpServer.socketID));

		network::tcpServer.startListen();

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

}