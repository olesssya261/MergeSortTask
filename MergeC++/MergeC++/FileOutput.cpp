#include "FileOutput.h"
#include <fstream>
#include <iostream>
#include "PersonalInterface.h"
#include "Checks.h"
#include <filesystem>//����������� ���������� ��� �������� ��������� �����
#include "Menu.h"
class FileWriteException
{
public:
	FileWriteException(std::string message) : message{ message } {}//����������� ������ 
	std::string getMessage() const { return message; }//������� ��������� �� ������

private:
	std::string message;//���� �������� ��������� �� ������
};
void WriteMassive(std::vector <double> nums, std::string fileName) {
	std::ofstream  out;//�������� ������ ������ � ����
	out.exceptions(std::ofstream::badbit | std::ofstream::failbit);//��������� ��������� ������ ������ ��������� ��������������
	try {
		out.open(fileName);//�������� ����� ��� ������
		out << " ��������������� ������:" << std::endl;
		for (auto iter = nums.begin(); iter < nums.end(); ++iter) {
			out << *iter << " ";
		}
		out.close(); //�������� ������ ������ � ���� 
		std::cout << "������ ������� ���������." << std::endl;
	}
	catch (const std::exception&)//��������� ������
	{
		throw FileWriteException("���������� �������� ������ � ����. ��������� �������.");//������ ���������������� ������ ��������� ������
	}

}

void FileOutput(std::vector<double> nums)
{
	std::ifstream out2;//�������� ������ ������ �� �����
	std::string fileName;//���������� ����� ��� ���� �����
	out2.exceptions(std::ifstream::badbit | std::ifstream::failbit);//��������� ��������� ������ ������ ��������� ��������������
	int userChoice = 0;//���������� ����������������� �����
	while (true) {
		std::cout << "������� ��� ����� (� ���������� .txt): ";
		std::cin >> fileName;//���� ���� � �����
		try {
			if (fileName.find(".txt") == std::string::npos)//����� � ����� ����� ����� .txt ���� ��������� �� ��������� �� ������� ������ ���������� ��������� ������
			{
				std::cout << "�� ������ ���������� � �����.���������� �������. " << std::endl;
				continue;
			}
			try
			{
				if (std::filesystem::is_regular_file(fileName)) {//�������� �� ��������� �����
					std::cout << "���� � ����� ������ ��� ����������" << std::endl;
				}

			}
			catch (const std::exception&)
			{
				throw FileWriteException("���������� �������� ������ � ����. ��������� �������.");//������ ���������������� ������ ��������� ������

			}
			out2.open(fileName);//������� �������� �����
			ShowOutputChoice();//������� ������ �� ������� ������ �����
			userChoice = GetUserChoice();//���� ����������������� ������
			if (userChoice == Yes) {

				out2.close();//�������� ������ ������ �� �����
				WriteMassive(nums, fileName);//������� ������ � ����
			}
			else {
				out2.close();//�������� ������ ������ �� �����
				continue;
			}
			break;
		}
		catch (const std::exception&) {
			try {
				WriteMassive(nums, fileName);//������� ������ � ����
				break;
			}
			catch (FileWriteException err) {//��������� ������ �������������� � ������
				std::cout << err.getMessage() << std::endl;//����� ��������� �� ������
			}
		}
		catch (FileWriteException err) {//��������� ������ �������������� � ������
			std::cout << err.getMessage() << std::endl;//����� ��������� �� ������
		}

	}
}
