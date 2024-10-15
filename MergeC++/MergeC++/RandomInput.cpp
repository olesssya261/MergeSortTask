#include "RandomInput.h"
#include <time.h>

void RandomInput(std::vector<double>* nums, int count)
{
	const int lowerBound = -100;
	const int upperBound = 100;
	srand(static_cast<unsigned int>(time(NULL)));
	for (int i = 0; i < count; i++) {
		nums->push_back(rand() % (upperBound - lowerBound + 1) + lowerBound);
		
	}
}

