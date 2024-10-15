
#include <windows.h> //Библиотека ответственная за русскую локализацию консоли
#include <conio.h> //Библиотека, ответственная за функцию getch
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
	setlocale(LC_ALL, "Rus"); //Локализация
	SetConsoleCP(1251); //функции для настройки локализации в строковых переменных при вводе
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
		userChoice = GetUserChoice(); //Ввод пользовательского решения
		switch (userChoice) //switch выбора консольного ввода или файлового
		{
		case (ManualInput):
			ConsoleInput(&nums, count); //Ввод текста с консоли
			break;
		case (RandInput):
			RandomInput(&nums, count);
			std::cout << "Сгенерированный массив:" << std::endl;
			ConsoleOutput(nums); //Вывод считанных данных на консоль
			break;
		}
		MergeSort(nums, 0, nums.size() - 1);
		std::cout << "Итоговый массив:" << std::endl;
		ConsoleOutput(nums); //Вывод считанных данных на консоль
		ShowOutputType(); //Вывод сообщения об сохранении выбранных данных в файл
		userChoice = GetUserChoice(); // Ввод пользовательского решения
		if (userChoice == Yes)
		{
			FileOutput(nums); //Сохранение итогового массива в фаил
		}
		std::cout << "Нажмите Esc чтобы завершить работу программы." << std::endl;
		std::cout << "Нажмите Enter чтобы продолжить." << std::endl;
		userChoice = _getch(); //Ввод кода символа введённого с клавиатуры
	} while (userChoice != QUIT); //Проверка что код символа равен коду макроса Quit
}
