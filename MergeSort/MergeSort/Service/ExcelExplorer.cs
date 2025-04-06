using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MergeSort.Model.ObservableModels;
using System.Globalization;
using System.Windows;

namespace MergeSort.Service
{
    /// <summary>
    /// Сервис для работы с Excel-файлами: экспорт и импорт данных сортировки
    /// </summary>
    public class ExcelExplorer
    {
        /// <summary>
        /// Экспортирует результаты сортировки в Excel-файл
        /// </summary>
        /// <param name="arrayModel">Модель с данными для экспорта</param>
        /// <param name="filePath">Путь к файлу для сохранения</param>
        /// <param name="sheetName">Имя листа (по умолчанию "SortArrayResult")</param>
        /// <remarks>
        /// Формат экспорта:
        /// 1 строка: "Исходный массив:"
        /// 2 строка: значения исходного массива
        /// 3 строка: тип сортировки
        /// 4 строка: количество перестановок
        /// 5 строка: количество сравнений
        /// 6 строка: "Отсортированный массив:"
        /// 7 строка: значения отсортированного массива
        /// </remarks>

        public static void ExportResultsToExcel(SortArrayObservableModel arrayModel, string filePath, string sheetName = "SortArrayResult")
        {
            try
            {
                // Создаем Excel-документ
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Добавляем первую строку с заголовком "Исходный массив:"
                    Row arrayLabelRow = new Row() { RowIndex = 1U };
                    Cell arrayLabelCell = new Cell()
                    {
                        CellReference = "A1",
                        DataType = CellValues.String
                    };
                    arrayLabelCell.Append(new CellValue("Исходный массив:"));
                    arrayLabelRow.Append(arrayLabelCell);
                    sheetData.Append(arrayLabelRow);

                    // Добавляем вторую строку с элементами исходного массива
                    var initialArray = ParserService.ParseStringToDoubleArray(arrayModel.ArrayData);
                    Row arrayRow = new Row() { RowIndex = 2U };
                    for (int i = 0; i < initialArray.Length; i++)
                    {
                        string columnLetter = GetColumnLetter(i + 1);
                        string cellReference = $"{columnLetter}2";

                        Cell cell = new Cell()
                        {
                            CellReference = cellReference,
                            DataType = CellValues.Number
                        };
                        cell.Append(new CellValue(initialArray[i].ToString(System.Globalization.CultureInfo.InvariantCulture)));
                        arrayRow.Append(cell);
                    }
                    sheetData.Append(arrayRow);

                    // Добавляем третью строку с "Тип сортировки"
                    Row sortTypeLabelRow = new Row() { RowIndex = 3U };
                    Cell sortTypeLabelCell = new Cell()
                    {
                        CellReference = "A3",
                        DataType = CellValues.String
                    };
                    sortTypeLabelCell.Append(new CellValue("Тип сортировки:"));
                    Cell sortTypeCell = new Cell()
                    {
                        CellReference = "B3",
                        DataType = CellValues.String
                    };
                    sortTypeCell.Append(new CellValue(arrayModel.SortType ?? "Не указан"));
                    sortTypeLabelRow.Append(sortTypeLabelCell);
                    sortTypeLabelRow.Append(sortTypeCell);
                    sheetData.Append(sortTypeLabelRow);

                    // Добавляем четвёртую строку с "Перестановки"
                    Row swapsLabelRow = new Row() { RowIndex = 4U };
                    Cell swapsLabelCell = new Cell()
                    {
                        CellReference = "A4",
                        DataType = CellValues.String
                    };
                    swapsLabelCell.Append(new CellValue("Перестановки:"));
                    Cell swapsCell = new Cell()
                    {
                        CellReference = "B4",
                        DataType = CellValues.Number
                    };
                    swapsCell.Append(new CellValue(arrayModel.Swaps?.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    swapsLabelRow.Append(swapsLabelCell);
                    swapsLabelRow.Append(swapsCell);
                    sheetData.Append(swapsLabelRow);

                    // Добавляем пятую строку с "Сравнения"
                    Row comparisonsLabelRow = new Row() { RowIndex = 5U };
                    Cell comparisonsLabelCell = new Cell()
                    {
                        CellReference = "A5",
                        DataType = CellValues.String
                    };
                    comparisonsLabelCell.Append(new CellValue("Сравнения:"));
                    Cell comparisonsCell = new Cell()
                    {
                        CellReference = "B5",
                        DataType = CellValues.Number
                    };
                    comparisonsCell.Append(new CellValue(arrayModel.Comparisons?.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    comparisonsLabelRow.Append(comparisonsLabelCell);
                    comparisonsLabelRow.Append(comparisonsCell);
                    sheetData.Append(comparisonsLabelRow);

                    // Добавляем шестую строку с заголовком "Отсортированный массив:"
                    Row sortedArrayLabelRow = new Row() { RowIndex = 6U };
                    Cell sortedArrayLabelCell = new Cell()
                    {
                        CellReference = "A6",
                        DataType = CellValues.String
                    };
                    sortedArrayLabelCell.Append(new CellValue("Отсортированный массив:"));
                    sortedArrayLabelRow.Append(sortedArrayLabelCell);
                    sheetData.Append(sortedArrayLabelRow);

                    // Добавляем седьмую строку с элементами отсортированного массива
                    if (!string.IsNullOrEmpty(arrayModel.SortedArrayData))
                    {
                        var sortArray = ParserService.ParseStringToDoubleArray(arrayModel.SortedArrayData);
                        Row sortArrayRow = new Row() { RowIndex = 7U };
                        for (int i = 0; i < sortArray.Length; i++)
                        {
                            string columnLetter = GetColumnLetter(i + 1);
                            string cellReference = $"{columnLetter}7";

                            Cell cell = new Cell()
                            {
                                CellReference = cellReference,
                                DataType = CellValues.Number
                            };
                            cell.Append(new CellValue(sortArray[i].ToString(System.Globalization.CultureInfo.InvariantCulture)));
                            sortArrayRow.Append(cell);
                        }
                        sheetData.Append(sortArrayRow);
                    }
                    else
                    {
                        throw new Exception("Данные отсортированного массива пусты");
                    }

                    // Сохраняем документ
                    workbookPart.Workbook.Save();
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Результаты успешно экспортированы в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при экспорте результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
        /// <summary>
        /// Экспортирует исходные данные для сортировки в Excel-файл
        /// </summary>
        /// <param name="arrayModel">Модель с данными для экспорта</param>
        /// <param name="filePath">Путь к файлу для сохранения</param>
        /// <param name="sheetName">Имя листа (по умолчанию "SortArrayResult")</param>
        /// <remarks>
        /// Формат экспорта:
        /// 1 строка: "Исходный массив:"
        /// 2 строка: значения исходного массива
        /// </remarks>
        public static void ExportParametersToExcel(SortArrayObservableModel arrayModel, string filePath, string sheetName = "SortArrayResult")
        {
            try
            {
                // Создаем Excel-документ
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Добавляем первую строку с заголовком "Исходный массив:"
                    Row arrayLabelRow = new Row() { RowIndex = 1U };
                    Cell arrayLabelCell = new Cell()
                    {
                        CellReference = "A1",
                        DataType = CellValues.String
                    };
                    arrayLabelCell.Append(new CellValue("Исходный массив:"));
                    arrayLabelRow.Append(arrayLabelCell);
                    sheetData.Append(arrayLabelRow);

                    // Добавляем вторую строку с элементами исходного массива
                    var initialArray = ParserService.ParseStringToDoubleArray(arrayModel.ArrayData);
                    Row arrayRow = new Row() { RowIndex = 2U };
                    for (int i = 0; i < initialArray.Length; i++)
                    {
                        string columnLetter = GetColumnLetter(i + 1);
                        string cellReference = $"{columnLetter}2";

                        Cell cell = new Cell()
                        {
                            CellReference = cellReference,
                            DataType = CellValues.Number
                        };
                        cell.Append(new CellValue(initialArray[i].ToString(System.Globalization.CultureInfo.InvariantCulture)));
                        arrayRow.Append(cell);
                    }
                    sheetData.Append(arrayRow);


                    // Сохраняем документ
                    workbookPart.Workbook.Save();
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Результаты успешно экспортированы в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при экспорте результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
        /// <summary>
        /// Импортирует исходный массив из Excel-файла.
        /// </summary>
        /// <param name="filePath">Путь к Excel-файлу.</param>
        /// <returns>Объект SortArrayObservableModel с заполненным полем ArrayData.</returns>
        /// <exception cref="Exception">Выбрасывается, если файл не удалось открыть или данные некорректны.</exception>
        public static SortArrayObservableModel ImportArrayFromExcel(string filePath)
        {
            try
            {
                // Открываем Excel-документ
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Находим вторую строку (RowIndex = 2)
                    Row arrayRow = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == 2U);
                    if (arrayRow == null)
                    {
                        throw new Exception("Вторая строка с исходным массивом не найдена в Excel-файле.");
                    }

                    // Читаем ячейки из второй строки (A2, B2, C2, ...)
                    List<double> initialArray = new List<double>();
                    foreach (Cell cell in arrayRow.Elements<Cell>())
                    {
                        // Получаем значение ячейки
                        string cellValue = GetCellValue(cell, workbookPart);
                        if (string.IsNullOrEmpty(cellValue))
                        {
                            continue; // Пропускаем пустые ячейки
                        }

                        // Парсим значение в double
                        if (double.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                        {
                            initialArray.Add(number);
                        }
                        else
                        {
                            throw new Exception($"Некорректное значение в ячейке {cell.CellReference}: {cellValue}. Ожидалось число.");
                        }
                    }

                    if (!initialArray.Any())
                    {
                        throw new Exception("Исходный массив в Excel-файле пуст.");
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Исходный массив успешно импортирован из файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                    // Создаём новый объект SortArrayObservableModel
                    SortArrayObservableModel importedModel = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(initialArray.ToArray())
                    };

                    return importedModel;
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при импорте массива из Excel: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
                SortArrayObservableModel importedModel = new SortArrayObservableModel
                {
                    ArrayData = ""
                };
                return importedModel;
            }
        }

        /// <summary>
        /// Получает значение ячейки, учитывая возможные ссылки на SharedStrings.
        /// </summary>
        /// <param name="cell">Ячейка Excel.</param>
        /// <param name="workbookPart">WorkbookPart для доступа к SharedStrings.</param>
        /// <returns>Строковое значение ячейки.</returns>
        private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell == null || cell.CellValue == null)
            {
                return null;
            }

            string value = cell.CellValue.InnerText;

            // Если тип ячейки — SharedString, получаем значение из SharedStringsTable
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                var sharedStringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                if (sharedStringTable != null)
                {
                    value = sharedStringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                }
            }

            return value;
        }
        /// <summary>
        /// Получает литеру ячейки
        /// </summary>
        /// <param name="columnNumber"> номер столбца</param>
        /// <returns>Литера ячейки.</returns>
        private static string GetColumnLetter(int columnNumber)
        {
            string columnLetter = string.Empty;
            while (columnNumber > 0)
            {
                int remainder = (columnNumber - 1) % 26;
                columnLetter = (char)('A' + remainder) + columnLetter;
                columnNumber = (columnNumber - remainder - 1) / 26;
            }
            return columnLetter;
        }

    }
}
