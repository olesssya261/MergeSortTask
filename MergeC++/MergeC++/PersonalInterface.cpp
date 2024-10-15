#include "PersonalInterface.h"

void ShowGreetings()
{
	std::cout << "Лабораторная работа №2, Кутькина Олеся, Ярослава Вьюгина, 434гр. " << std::endl;
}

void ShowTask()
{
	std::cout << "Задан массив. Отсортировать его с помощью сортировки слиянием (Merge Sort)." << std::endl;

}
void ShowOutputDataOption()
{
	std::cout << "Вы хотите сохранить исходные данные в файл?" << std::endl;
	std::cout << "1)Да" << std::endl;
	std::cout << "2)Нет" << std::endl;
	std::cout << "Ввод:";
}
void ShowInputOption()
{
	std::cout << "Какой вид ввода данных вы хотите использовать?" << std::endl;
	std::cout << "1)С клавиаутры" << std::endl;
	std::cout << "2)Случайными числами" << std::endl;
	std::cout << "Ввод:";
}
void ShowOutputType()
{
	std::cout << "Вы хотите записать данные в файл?" << std::endl;
	std::cout << "1)Да" << std::endl;
	std::cout << "2)Нет" << std::endl;
	std::cout << "Ввод:";
}

void ShowMassiveInput()
{
	std::cout << "Введите массив." << std::endl;
}
void ShowOutputChoice()
{
	std::cout << "Вы хотите перезаписать данный файл?" << std::endl;
	std::cout << "1)Да" << std::endl;
	std::cout << "2)Ввести новое имя(путь к файлу)" << std::endl;
	std::cout << "Ввод:";
}

void ShowMassiveQuantity()
{
	std::cout << "Введите количество элементов массива." << std::endl;
	std::cout << "Ввод:";
}
