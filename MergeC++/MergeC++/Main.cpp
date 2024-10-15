#include <windows.h> //���������� ������������� �� ������� ����������� �������
#include <conio.h> //����������, ������������� �� ������� getch
#include <iostream>//���������� �����-������
#include <vector>//���������� �������
#include "PersonalInterface.h"//HeaderFile � ��������� ����������������� ����������
#include "Checks.h"//HeaderFile � ���������� ����������������� �����
#include "Menu.h"//HeaderFile � enum-menu
#include "ConsoleInput.h"//HeaderFile � �������� ����� ������ � �������
#include "RandomInput.h"//HeaderFile � �������� ���������� ������� ���������� �������
#include "ConsoleOutput.h"//HeaderFile � �������� ������ ������ �� �������
#include "Algoritm.h"//HeaderFile � ������������� �����������
#include "FileOutput.h"//HeaderFile � �������� ������ ������ � ����
#define QUIT 27 //���������� ������� QUIT � ��������� 27

int main()
{
	setlocale(LC_ALL, "Rus"); //�����������
	SetConsoleCP(1251); //������� ��� ��������� ����������� � ��������� ���������� ��� �����
	SetConsoleOutputCP(1251);//������� ��� ��������� ����������� � ��������� ���������� ��� ������
	int userChoice = 0;//���������� ����������������� �������
	std::vector<double> nums;//������ ��������� �������
	ShowGreetings();//����� ����������� �� �������
	ShowTask();//����� ������� �� �������
	do
	{
		ShowMassiveQuantity();//����� ����������� ������ ���-�� ��������� �������
		int count = GetPositiveIntMoreThan0();//���� ���������� ��������� �������
		ShowInputOption();//����� ���� ������ ���� �����
		userChoice = GetUserChoice(); //���� ����������������� �������
		switch (userChoice) //switch ������ ����������� ����� ��� ���������
		{
		case (ManualInput):
			ConsoleInput(nums, count); //���� ������ � �������
			break;
		case (RandInput):
			RandomInput(nums, count);//���������� ������� ���������� �������
			std::cout << "��������������� ������:" << std::endl;
			ConsoleOutput(nums); //����� ��������� ������ �� �������
			break;
		}
		MergeSort(nums, 0, static_cast<int>(nums.size() - 1));//���������� �������� ������� �������� ��������
		std::cout << "�������� ������:" << std::endl;
		ConsoleOutput(nums); //����� ��������� ������ �� �������
		ShowOutputType(); //����� ��������� �� ���������� ��������� ������ � ����
		userChoice = GetUserChoice(); // ���� ����������������� �������
		if (userChoice == Yes)
		{
			FileOutput(nums); //���������� ��������� ������� � ����
		}
		nums.clear();//������ �������
		std::cout << "������� Esc ����� ��������� ������ ���������." << std::endl;
		std::cout << "������� Enter ����� ����������." << std::endl;
		userChoice = _getch(); //���� ���� ������� ��������� � ����������
	} while (userChoice != QUIT); //�������� ��� ��� ������� ����� ���� ������� Quit
}
