#include <iostream>
#include "utils.h"
#include "network.h"
using namespace std;

int main() {
	log("Server initializing...");

	networkInit();

	log("Initialization complete");

	return 0;
}