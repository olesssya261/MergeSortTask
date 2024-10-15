#include "ConsoleInput.h"
#include "PersonalInterface.h"
#include "Checks.h"

void ConsoleInput(std::vector<double>& nums, int count)
{
	//Вывод предложения ввести массив
	ShowMassiveInput();
	for (int i = 0; i < count; i++) {
		std::cout << "nums[" << i + 1 << "]= "; //Приглашение ко вводу n-го элемента
		nums.push_back(GetDouble());//Добавление в конец вектора элемента
		std::cout<<std::endl;
	}

}
