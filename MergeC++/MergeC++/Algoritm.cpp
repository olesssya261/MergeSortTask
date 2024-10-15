#include "Algoritm.h"

void merge(std::vector<double>& nums, int left, int mid, int right) {
    int i, j, k;
    int n1 = mid - left + 1;
    int n2 = right - mid;

    std::vector<double> leftVec(n1), rightVec(n2);
    for (i = 0; i < n1; i++) {
        leftVec[i] = nums[left + i];
    }
    for (j = 0; j < n2; j++) {
        rightVec[j] = nums[mid + 1 + j];
    }
    i = 0;
    j = 0;
    k = left;
    while (i < n1 && j < n2) {
        if (leftVec[i] <= rightVec[j]) {
            nums[k] = leftVec[i];
            i++;
        }
        else {
            nums[k] = rightVec[j];
            j++;
        }
        k++;
    }
    while (i < n1) {
        nums[k] = leftVec[i];
        i++;
        k++;
    }
    while (j < n2) {
        nums[k] = rightVec[j];
        j++;
        k++;
    }
}
void MergeSort(std::vector<double>& nums, int left, int right)
{
    if (left < right) {
        int mid = left + (right - left) / 2;
        MergeSort(nums, left, mid);
        MergeSort(nums, mid + 1, right);
        merge(nums, left, mid, right);
    }
}
