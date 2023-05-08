#pragma once
#include "tcpserver.h"
#include "utils.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif

namespace network {
	extern TcpServer tcpServer; //We use extern for vars in namespaces, then we declare them once in another file (network.cpp here)

	void networkInit();
	void startWSA();
};