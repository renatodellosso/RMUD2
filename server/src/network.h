#pragma once
#include "tcpserver.h"

void networkInit();

class Network {
	public:
		static http::TcpServer tcpServer;
};