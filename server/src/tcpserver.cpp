#include "tcpserver.h"
#include <winsock2.h>

namespace http {

	TcpServer::TcpServer() {
		socketID = socket(AF_INET, SOCK_STREAM, 0);
	}

	TcpServer::~TcpServer() {
	
	}

}