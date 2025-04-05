using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort.Service
{
    public static class PathService
    {
        public static string GetCurentFolderPath(string additionPath = "")
        {
            try
            {
                return $"{Environment.CurrentDirectory}\\{additionPath}";
            }
            catch (Exception)
            {
                throw new Exception("Невозможно установить путь к используемой директории");
            }
        }
    }
}
