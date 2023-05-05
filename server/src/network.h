#pragma once
#include "tcpserver.h"
#include "utils.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif

void networkInit();
void startWSA();

class Network {
	public:
		static http::TcpServer tcpServer;
};