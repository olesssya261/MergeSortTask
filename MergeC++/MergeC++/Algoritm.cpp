#include "Algoritm.h"

void merge(std::vector<double>& nums, int left, int mid, int right) {
    //���������� ��������� ����������
    int i, j, k;
    //���������� ���-�� ��������� � ����� �����
    int leftElementsCount = mid - left + 1;
    //���������� ���-�� ��������� � ������ �����
    int rightElementCount = right - mid;
    //�������� ��������� ��������
    std::vector<double> leftVec(leftElementsCount), rightVec(rightElementCount);
    //���������� ��������� � ����� ����� �� ��������� ������
    for (i = 0; i < leftElementsCount; i++) {
        leftVec[i] = nums[left + i];
    }
    //���������� ��������� � ������ ����� �� ��������� ������
    for (j = 0; j < rightElementCount; j++) {
        rightVec[j] = nums[mid + 1 + j];
    }
    //��������� ��������� ����������
    i = 0;
    j = 0;
    k = left;
    //������� ���������� ��������� �� ��������� �������� � ��������
    while (i < leftElementsCount && j < rightElementCount) {
        //���� ������� ������ �������� �� ������ �����
        if (leftVec[i] <= rightVec[j]) {
            //���������� ������ �������� � ������
            nums[k] = leftVec[i];
            i++;
        }
        //����� ���������� ������� ��������
        else {
            nums[k] = rightVec[j];
            j++;
        }
        k++;
    }
    //���� �������� ������������� �������� � ����� �������, ���������� �� � ����� ��������� �������
    while (i < leftElementsCount) {
        nums[k] = leftVec[i];
        i++;
        k++;
    }
    //���� �������� ������������� �������� � ������ �������, ���������� �� � ����� ��������� �������
    while (j < rightElementCount) {
        nums[k] = rightVec[j];
        j++;
        k++;
    }
}
void MergeSort(std::vector<double>& nums, int left, int right)
{
    //������� �������� �������
    if (left < right) {
        //���������� ����������� ��������
        int mid = left + (right - left) / 2;
        //���������� ����� �����
        MergeSort(nums, left, mid);
        //���������� ������ �����
        MergeSort(nums, mid + 1, right);
        //���������� ���� ������ �������
        merge(nums, left, mid, right);
    }
}
