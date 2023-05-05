#include <iostream>
#include "utils.h"
#include "network.h"

using namespace std;

int main() {
	log("\nServer initializing...\n");

	networkInit();

	log("\nInitialization complete\n");

	return 0;
}