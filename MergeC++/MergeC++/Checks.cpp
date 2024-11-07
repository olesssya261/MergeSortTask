#include "Checks.h"
#include <string>//Библиотека строк
#include <iostream>//Библиотека ввода-вывода

bool CheckNums(std::string str) {
	for (int i = 0; i < str.size(); i++) {
		if (!isdigit(str[i]) && str[i] != ',') {
			return false;
		}
	}
	return true;
}
int GetUserChoice()
{
	while (true) {

		std::string str;//Объявление времного буфера ввода (временной строки)
		std::cin >> str;//Ввод пользовательских данных
		try
		{
			if (!CheckNums(str)) {
				throw std::exception();
			}
			int result = std::stoi(str);//Преобразование строки в целочисленный тип данных
			if ((result == 1 || result == 2) && str.size() == 1) {
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
			std::cin.clear();//Возврат поток ввода в рабочее состояние 
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

	}
}

double GetDouble()
{
	while (true) {
		std::string str;//Объявление времного буфера ввода (временной строки)
		std::cin >> str;//Ввод пользовательских данных
		try
		{
			if (!CheckNums(str)) {
				throw std::exception();
			}
			double result = std::stod(str);//Преобразование строки в тип double данных
			std::cin.clear();//Возврат поток ввода в рабочее состояние 
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
			return result;
		}
		catch (const std::exception&)
		{
			std::cin.clear();//Возврат поток ввода в рабочее состояние 
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

	}
}

int GetPositiveIntMoreThan0()
{
	while (true) {
		std::string str;//Объявление времного буфера ввода (временной строки)
		std::cin >> str;//Ввод пользовательских данных
		try
		{
			if (!CheckNums(str)) {
				throw std::exception();
			}
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
			std::cin.clear();//Возврат поток ввода в рабочее состояние 
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//Игнорирование всех символов в потоке
			std::cout << "Введены некорректные данные. Повторите попытку ввода." << std::endl;
		}

	}
}
