#include "tcpserver.h"
#include "utils.h"
#include "config.h"
#include <ostream>
#include <sstream>
#include <WS2tcpip.h>
#include "network.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif

namespace network {

	TcpServer::TcpServer() {
		//TcpServer(false);
	}

	TcpServer::TcpServer(bool shouldCreate = false) {
		if (shouldCreate && network::tcpServer.socketID == 0) {
			log("Constructing TcpServer...");

			startWSA();

			socketID = socket(AF_INET, SOCK_STREAM, 0);

			if (socketID < 0)
				exitWithError("Socket creation failed");
			else {
				address.sin_family = AF_INET;
				address.sin_addr = in_addr{ 0, 0, 0, 0 };
				address.sin_port = config.port;

				log("Address Family: " + to_string(address.sin_family) + ", Port: " + to_string(address.sin_port));

				if (bind(socketID, (sockaddr*)&address, sizeof(address)) < 0)
					exitWithError("Socket binding failed: "
#ifdef _WIN32
						+ to_string(WSAGetLastError())
#endif // _WIN32
					);
			}

			log("Constructed TcpServer. Socket ID: " + to_string(socketID));
		}
	}

	TcpServer::~TcpServer() {
		log("Deconstructing TcpServer... Socket ID: " + to_string(socketID));

		closesocket(socketID);

#ifdef _WIN32
		WSACleanup();
#endif
	}

	void TcpServer::startListen() {
		log("Starting listening on socket " + to_string(socketID) + "...");

		if (listen(socketID, config.connectionBacklog) < 0)
			return exitWithError("Socket listening failed: "
#ifdef _WIN32
				+ to_string(WSAGetLastError())
#endif // _WIN32
			);

		char addressStr[INET_ADDRSTRLEN]{};
		inet_ntop(address.sin_family, &address.sin_addr, addressStr, INET_ADDRSTRLEN);

		std::stringstream ss;
		ss << "Listening on " << socketID << "..."
			<< addressStr
			<< ", Port: " << ntohs(address.sin_port)
			<< "\n";
		log(ss.str());
	}

}