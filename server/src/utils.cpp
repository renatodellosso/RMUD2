#include <string>
#include <iostream>
using namespace std;

void log(string msg) {
	cout << msg << endl;
}

void exitWithError(string error) {
	log("FATAL ERROR: " + error);

	exit(1);
}