#pragma once
#include "tcpserver.h"
#ifdef _WIN32
	#include <WinSock2.h>
#endif


namespace http {
	
	class TcpServer {
		public:
			SOCKET socketID;
			struct sockaddr_in address;

			TcpServer();
			~TcpServer();

			void startListen();
	};

}