
#include <windows.h> //���������� ������������� �� ������� ����������� �������
#include <conio.h> //����������, ������������� �� ������� getch
#include <iostream>
#include <vector>
#include "PersonalInterface.h"
#include "Checks.h"
#include "Menu.h"
#include "ConsoleInput.h"
#include "RandomInput.h"
#include "ConsoleOutput.h"
#include "Algoritm.h"
#include "FileOutput.h"
#define QUIT 27 

int main()
{
	setlocale(LC_ALL, "Rus"); //�����������
	SetConsoleCP(1251); //������� ��� ��������� ����������� � ��������� ���������� ��� �����
	SetConsoleOutputCP(1251);
	int userChoice = 0;
	std::vector<double> nums;
	int n = 0;
	ShowGreetings();
	ShowTask();
	do
	{
		ShowMassiveQuantity();
		int count = GetPositiveIntMoreThan0();
		ShowInputOption();
		userChoice = GetUserChoice(); //���� ����������������� �������
		switch (userChoice) //switch ������ ����������� ����� ��� ���������
		{
		case (ManualInput):
			ConsoleInput(&nums, count); //���� ������ � �������
			break;
		case (RandInput):
			RandomInput(&nums, count);
			std::cout << "��������������� ������:" << std::endl;
			ConsoleOutput(nums); //����� ��������� ������ �� �������
			break;
		}
		MergeSort(nums, 0, nums.size() - 1);
		std::cout << "�������� ������:" << std::endl;
		ConsoleOutput(nums); //����� ��������� ������ �� �������
		ShowOutputType(); //����� ��������� �� ���������� ��������� ������ � ����
		userChoice = GetUserChoice(); // ���� ����������������� �������
		if (userChoice == Yes)
		{
			FileOutput(nums); //���������� ��������� ������� � ����
		}
		std::cout << "������� Esc ����� ��������� ������ ���������." << std::endl;
		std::cout << "������� Enter ����� ����������." << std::endl;
		userChoice = _getch(); //���� ���� ������� ��������� � ����������
	} while (userChoice != QUIT); //�������� ��� ��� ������� ����� ���� ������� Quit
}
