#include "ConsoleInput.h"
#include "PersonalInterface.h"
#include "Checks.h"

void ConsoleInput(std::vector<double>* nums, int count)
{
	ShowMassiveInput();
	for (int i = 0; i < count; i++) {
		std::cout << "nums[" << i + 1 << "]= "; //Приглашение ко вводу
		nums->push_back(GetDouble());
		std::cout<<std::endl;
	}

}
