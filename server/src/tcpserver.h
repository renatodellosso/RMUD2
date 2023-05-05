#pragma once
#include <winsock2.h>

namespace http {
	
	class TcpServer {
		public:
			TcpServer();
			~TcpServer();
			SOCKET socketID;
	};

}