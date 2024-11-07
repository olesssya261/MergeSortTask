#include "FileOutput.h"
#include <fstream>//Библиотека ввода-вывода в файл
#include <iostream>//Библиотека ввода-вывода на консоль
#include "PersonalInterface.h"
#include "Checks.h"
#include <filesystem>//Подключение библеотеки для проверки состояния фаила
#include "Menu.h"
/// <summary>
/// Конструктор класса
/// Возврат сообщение об ошибки
/// Поле хранящее сообщение об ошибке
/// </summary>
class FileWriteException
{
public:
	FileWriteException(std::string message) : message{ message } {} 
	std::string getMessage() const { return message; }

private:
	std::string message;
};
/// <summary>
/// Создания потока записи в файл
/// Установка побитовых флагов ошибок файлового взаимодействия
/// открытие файла для записи
/// закрытие потока вывода в файл
/// Обработка ошибки
/// Выброс пользовательской ошибки фаилового вывода
/// <param name="nums"></param>
/// <param name="fileName"></param>
///  </summary>
void WriteMassive(std::vector <double> nums, std::string fileName) {
	std::ofstream  writeThreat;
	writeThreat.exceptions(std::ofstream::badbit | std::ofstream::failbit);
	try {
		writeThreat.open(fileName);
		writeThreat << " Отсортированный массив:" << std::endl;
		for (auto iter = nums.begin(); iter < nums.end(); ++iter) {
			writeThreat << *iter << " ";
		}
		writeThreat.close(); 
		std::cout << "Данные успешно сохранены." << std::endl;
	}
	catch (const std::exception&)
	{
		throw FileWriteException("Невозможно записать данные в файл. Повторите попытку.");
	}

}

void FileOutput(std::vector<double> nums)
{
	std::ifstream readThreat; /// Создания потока чтения из файла
	std::string fileName; /// Переменная имени или пути файла
	readThreat.exceptions(std::ifstream::badbit | std::ifstream::failbit); /// Установка побитовых флагов ошибок фаилового взаимодействия
	int userChoice = 0; /// Переменная пользовательского ввода
	while (true) {
		std::cout << "Введите имя фаила (в разрешении .txt): ";
		std::cin >> fileName; /// Ввод пути к файлу
		try {
			if (fileName.find(".txt") == std::string::npos) /// Поиск в имени фаила части .txt если указатель не указывает на элемент строки происходит повторный запрос
			{
				std::cout << "Не верное разрешение у файла. Повторитие попытку. " << std::endl;
				continue;
			}
			try
			{ 
				if (std::filesystem::is_regular_file(fileName)) { /// Проверка на системные файлы
					std::cout << "Файл с таким именем уже существует" << std::endl;
				}

			}
			catch (const std::exception&)
			{
				throw FileWriteException("Невозможно записать данные в файл. Повторите попытку."); /// Выброс пользовательской ошибки файлового вывода

			}
			readThreat.open(fileName);
			ShowOutputChoice();
			userChoice = GetUserChoice();//Ввод пользовательского выбора
			if (userChoice == Yes) {

				readThreat.close();//Закрытия потока чтения из файла
				WriteMassive(nums, fileName);//Функция записи в файл
			}
			else {
				readThreat.close();//Закрытия потока чтения из файла
				continue;
			}
			break;
		}
		catch (const std::exception&) {
			try {
				WriteMassive(nums, fileName);//Функция записи в файл
				break;
			}
			catch (FileWriteException err) {//Обработка ошибки взаимодействия с файлом
				std::cout << err.getMessage() << std::endl;//Вывод сообщения об ошибки
			}
		}
		catch (FileWriteException err) {//Обработка ошибки взаимодействия с файилом
			std::cout << err.getMessage() << std::endl;//Вывод сообщения об ошибки
		}

	}
}
