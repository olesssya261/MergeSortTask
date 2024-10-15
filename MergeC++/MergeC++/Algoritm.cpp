#include "Algoritm.h"

void merge(std::vector<double>& nums, int left, int mid, int right) {
    //объявление временных переменных
    int i, j, k;
    //Нахождение кол-ва элементов в левой части
    int leftElementsCount = mid - left + 1;
    //Нахождение кол-ва элементов в правой части
    int rightElementCount = right - mid;
    //Создание временных векторов
    std::vector<double> leftVec(leftElementsCount), rightVec(rightElementCount);
    //Фильтрация элементов в левой части во временный вектор
    for (i = 0; i < leftElementsCount; i++) {
        leftVec[i] = nums[left + i];
    }
    //Фильтрация элементов в правой части во временный вектор
    for (j = 0; j < rightElementCount; j++) {
        rightVec[j] = nums[mid + 1 + j];
    }
    //Обнуление временных переменных
    i = 0;
    j = 0;
    k = left;
    //Условие фильтрации элементов из временных векторов в итоговый
    while (i < leftElementsCount && j < rightElementCount) {
        //Если элемент меньше элемента из правой части
        if (leftVec[i] <= rightVec[j]) {
            //Добавление левого элемента в вектор
            nums[k] = leftVec[i];
            i++;
        }
        //Иначе добавление правого элемента
        else {
            nums[k] = rightVec[j];
            j++;
        }
        k++;
    }
    //Если остались неразрешённые элементы в левом векторе, добавление их в конец итогового вектора
    while (i < leftElementsCount) {
        nums[k] = leftVec[i];
        i++;
        k++;
    }
    //Если остались неразрешённые элементы в правом векторе, добавление их в конец итогового вектора
    while (j < rightElementCount) {
        nums[k] = rightVec[j];
        j++;
        k++;
    }
}
void MergeSort(std::vector<double>& nums, int left, int right)
{
    //Условие возврата функции
    if (left < right) {
        //Нахождение серединного элемента
        int mid = left + (right - left) / 2;
        //Сортировка левой части
        MergeSort(nums, left, mid);
        //Сортировка правой части
        MergeSort(nums, mid + 1, right);
        //Соединение двух частей вектора
        merge(nums, left, mid, right);
    }
}
