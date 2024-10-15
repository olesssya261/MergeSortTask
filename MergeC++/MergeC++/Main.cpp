#include <windows.h> //Библиотека ответственная за русскую локализацию консоли
#include <conio.h> //Библиотека, ответственная за функцию getch
#include <iostream>//Библиотека ввода-вывода
#include <vector>//Библиотека вектора
#include "PersonalInterface.h"//HeaderFile с функциями пользовательского интерфейса
#include "Checks.h"//HeaderFile с проверками пользовательского ввода
#include "Menu.h"//HeaderFile с enum-menu
#include "ConsoleInput.h"//HeaderFile с функцией ввода данных с консоли
#include "RandomInput.h"//HeaderFile с функцией заполнения массива случайными числами
#include "ConsoleOutput.h"//HeaderFile с функцией вывода данных на консоль
#include "Algoritm.h"//HeaderFile с используемыми алгоритмами
#include "FileOutput.h"//HeaderFile с функцией вывода данных в файл
#define QUIT 27 //Объявление макроса QUIT с значением 27

int main()
{
	setlocale(LC_ALL, "Rus"); //Локализация
	SetConsoleCP(1251); //функции для настройки локализации в строковых переменных при вводе
	SetConsoleOutputCP(1251);//функции для настройки локализации в строковых переменных при выводе
	int userChoice = 0;//Переменная пользовательского решения
	std::vector<double> nums;//Вектор элементов массива
	ShowGreetings();//Вывод приветствия на консоль
	ShowTask();//Вывод задания на консоль
	do
	{
		ShowMassiveQuantity();//Вывод предложения ввести кол-во элементов массива
		int count = GetPositiveIntMoreThan0();//Ввод количества элементов массива
		ShowInputOption();//Вывод меню выбора типа ввода
		userChoice = GetUserChoice(); //Ввод пользовательского решения
		switch (userChoice) //switch выбора консольного ввода или файлового
		{
		case (ManualInput):
			ConsoleInput(nums, count); //Ввод текста с консоли
			break;
		case (RandInput):
			RandomInput(nums, count);//Заполнение вектора случайными числами
			std::cout << "Сгенерированный массив:" << std::endl;
			ConsoleOutput(nums); //Вывод считанных данных на консоль
			break;
		}
		MergeSort(nums, 0, static_cast<int>(nums.size() - 1));//Сортировка слиянием вектора введённых значений
		std::cout << "Итоговый массив:" << std::endl;
		ConsoleOutput(nums); //Вывод считанных данных на консоль
		ShowOutputType(); //Вывод сообщения об сохранении выбранных данных в файл
		userChoice = GetUserChoice(); // Ввод пользовательского решения
		if (userChoice == Yes)
		{
			FileOutput(nums); //Сохранение итогового вектора в фаил
		}
		nums.clear();//Чистка вектора
		std::cout << "Нажмите Esc чтобы завершить работу программы." << std::endl;
		std::cout << "Нажмите Enter чтобы продолжить." << std::endl;
		userChoice = _getch(); //Ввод кода символа введённого с клавиатуры
	} while (userChoice != QUIT); //Проверка что код символа равен коду макроса Quit
}
