#pragma once
#ifdef _WIN32
	#include <WinSock2.h>
#endif


namespace network {
	
	class TcpServer {
		public:
			SOCKET socketID = 0;
			struct sockaddr_in address = sockaddr_in{};

			TcpServer();
			TcpServer(bool shouldCreate);
			~TcpServer();

			void startListen();
	};

}