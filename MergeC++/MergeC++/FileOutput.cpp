#include "FileOutput.h"
#include <fstream>
#include <iostream>
#include "PersonalInterface.h"
#include "Checks.h"
#include <filesystem>//Подключение библеотеки для проверки состояния фаила
#include "Menu.h"
class FileWriteException
{
public:
	FileWriteException(std::string message) : message{ message } {}//Конструктор класса 
	std::string getMessage() const { return message; }//Возврат сообщение об ошибки

private:
	std::string message;//Поле хранящее сообщение об ошибке
};
void WriteMassive(std::vector <double> nums, std::string fileName) {
	std::ofstream  out;//Создания потока записи в фаил
	out.exceptions(std::ofstream::badbit | std::ofstream::failbit);//Установка побитовых флагов ошибок фаилового взаимодействия
	try {
		out.open(fileName);//открытие файла для записи
		out << " Отсортированный массив:" << std::endl;
		for (auto iter = nums.begin(); iter < nums.end(); ++iter) {
			out << *iter << " ";
		}
		out.close(); //закрытие потока вывода в файл 
		std::cout << "Данные успешно сохранены." << std::endl;
	}
	catch (const std::exception&)//Обработка ошибки
	{
		throw FileWriteException("Невозможно записать данные в файл. Повторите попытку.");//Выброс пользовательской ошибки фаилового вывода
	}

}

void FileOutput(std::vector<double> nums)
{
	std::ifstream out2;//Создания потока чтения из фаила
	std::string fileName;//Переменная имени или пути фаила
	out2.exceptions(std::ifstream::badbit | std::ifstream::failbit);//Установка побитовых флагов ошибок фаилового взаимодействия
	int userChoice = 0;//Переменная пользовательского ввода
	while (true) {
		std::cout << "Введите имя фаила (в разрешении .txt): ";
		std::cin >> fileName;//Ввод пути к фаилу
		try {
			if (fileName.find(".txt") == std::string::npos)//Поиск в имени фаила части .txt если указатель не указывает на элемент строки происходит повторный запрос
			{
				std::cout << "Не верное разрешение у файла.Повторитие попытку. " << std::endl;
				continue;
			}
			try
			{
				if (std::filesystem::is_regular_file(fileName)) {//Проверка на системные фаилы
					std::cout << "Фаил с таким именем уже существует" << std::endl;
				}

			}
			catch (const std::exception&)
			{
				throw FileWriteException("Невозможно записать данные в файл. Повторите попытку.");//Выброс пользовательской ошибки фаилового вывода

			}
			out2.open(fileName);//Попытка открытия фаила
			ShowOutputChoice();//Функция вывода на консоль выбора файла
			userChoice = GetUserChoice();//Ввод пользовательского выбора
			if (userChoice == Yes) {

				out2.close();//Закрытия потока чтения из фаила
				WriteMassive(nums, fileName);//Функция записи в фаил
			}
			else {
				out2.close();//Закрытия потока чтения из фаила
				continue;
			}
			break;
		}
		catch (const std::exception&) {
			try {
				WriteMassive(nums, fileName);//Функция записи в фаил
				break;
			}
			catch (FileWriteException err) {//Обработка ошибки взаимодействия с фаилом
				std::cout << err.getMessage() << std::endl;//Вывод сообщения об ошибки
			}
		}
		catch (FileWriteException err) {//Обработка ошибки взаимодействия с фаилом
			std::cout << err.getMessage() << std::endl;//Вывод сообщения об ошибки
		}

	}
}
