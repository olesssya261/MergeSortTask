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
			int result = std::stoi(str);//�������������� ������ � ������������� ��� ������
			if (result==1|| result == 2) {
				std::cin.clear();//������� ����� ����� � ������� ��������� 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//������������� ���� �������� � ������
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
			std::cout << "������� ������������ ������. ��������� ������� �����." << std::endl;
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
			double result = std::stod(str);//�������������� ������ � ��� double ������

				std::cin.clear();//������� ����� ����� � ������� ��������� 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//������������� ���� �������� � ������
				return result;
		}
		catch (const std::exception&)
		{
			std::cin.clear();
			std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
			std::cout << "������� ������������ ������. ��������� ������� �����." << std::endl;
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
			int result = std::stoi(str);//�������������� ������ � ������������� ��� ������
			if (result > 0) {
				std::cin.clear();//������� ����� ����� � ������� ��������� 
				std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');//������������� ���� �������� � ������
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
			std::cout << "������� ������������ ������. ��������� ������� �����." << std::endl;
		}

	}
}
