#include "RandomInput.h"
#include <time.h>

void RandomInput(std::vector<double>& nums, int count)
{
	const int lowerBound = -100;//Ќижн€€ граница генерируемых значений
	const int upperBound = 100;//¬ерхн€€ граница генерируемых значений
	srand(static_cast<unsigned int>(time(NULL)));//ѕрив€зка генератора случайных чисел ко времени
	for (int i = 0; i < count; i++) {
		nums.push_back(rand() % (upperBound - lowerBound + 1) + lowerBound + (double)rand() / RAND_MAX);
		
	}
}

