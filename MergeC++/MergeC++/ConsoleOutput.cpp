#include "ConsoleOutput.h"
#include <iostream>
//����� ������� �� �������
void ConsoleOutput(std::vector<double> nums)
{
	for (auto iter = nums.begin(); iter < nums.end(); ++iter) {
		std::cout << *iter<<" ";
	}
	std::cout << std::endl;
}
