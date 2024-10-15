#include "Checks.h"
#include <string>
#include <iostream>

int GetUserChoice()
{
    while (true) {
        std::string str;
        std::cin >> str;
		try
		{
			int result = std::stoi(str);//Преобразование строки в целочисленный тип данных
			if (result==1|| result == 2) {
				std::cin.clear();//Возврат поток ввода в рабочее состояние 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
				return result;
			}
			else {
				throw std::exception();
			}
		}
		catch (const std::exception&)
		{
			std::cin.clear();
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

    }
    return 0;
}

double GetDouble()
{
	while (true) {
		std::string str;
		std::cin >> str;
		try
		{
			double result = std::stod(str);//Преобразование строки в тип double данных

				std::cin.clear();//Возврат поток ввода в рабочее состояние 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
				return result;
		}
		catch (const std::exception&)
		{
			std::cin.clear();
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

	}
	return 0;
}

int GetPositiveIntMoreThan0()
{
	while (true) {
		std::string str;
		std::cin >> str;
		try
		{
			int result = std::stoi(str);//Преобразование строки в целочисленный тип данных
			if (result > 0) {
				std::cin.clear();//Возврат поток ввода в рабочее состояние 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
				return result;
			}
			else {
				throw std::exception();
			}
		}
		catch (const std::exception&)
		{
			std::cin.clear(); 
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

	}
}
