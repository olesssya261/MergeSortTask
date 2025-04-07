using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Модель для хранения данных о сортировке массива чисел double в базе данных.
/// </summary>
public class SortArrayModel
{
    /// <summary>
    /// Уникальный идентификатор записи.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Исходный массив данных в бинарном формате.
    /// </summary>
    public byte[] ArrayDataBlob { get; set; }

    /// <summary>
    /// Отсортированный массив в бинарном формате.
    /// </summary>
    public byte[]? SortedArrayDataBlob { get; set; }

    /// <summary>
    /// Количество перестановок при сортировке.
    /// </summary>
    public uint? Swaps { get; set; }

    /// <summary>
    /// Количество сравнений при сортировке.
    /// </summary>
    public uint? Comparisons { get; set; }

    /// <summary>
    /// Тип сортировки.
    /// </summary>
    public string? SortType { get; set; }

    /// <summary>
    /// Дата и время создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Доступ к данным массива как double[].
    /// </summary>
    [NotMapped]
    public double[] ArrayData
    {
        get
        {
            if (ArrayDataBlob == null)
                return null;

            if (ArrayDataBlob.Length % sizeof(double) != 0)
                throw new InvalidOperationException("Некорректный размер данных");
            //Выделение памяти под result (столько же сколько в ArrayDataBlob)
            var result = new double[ArrayDataBlob.Length / sizeof(double)];
            //Копирование ArrayDataBlob в result и преобразование типа byte[] в double[]
            Buffer.BlockCopy(ArrayDataBlob, 0, result, 0, ArrayDataBlob.Length);
            return result;
        }
        set
        {
            if (value == null)
                throw new InvalidOperationException("Исходный массив не может быть пуст при сохранении в БД");
            ArrayDataBlob = new byte[value.Length * sizeof(double)];
            Buffer.BlockCopy(value, 0, ArrayDataBlob, 0, ArrayDataBlob.Length);
        }
    }

    /// <summary>
    /// Доступ к отсортированным данным массива как double[].
    /// </summary>
    [NotMapped]
    public double[] SortedArrayData
    {
        get
        {
            if (SortedArrayDataBlob == null)
                return null;
            //Выброс ошибки если неудаётся создать из массива byte целое количество элементов типа doublе
            if (SortedArrayDataBlob.Length % sizeof(double) != 0)
                throw new InvalidOperationException("Некорректный размер данных");
            //Выделение памяти под result (столько же сколько в SortedArrayDataBlob)
            var result = new double[SortedArrayDataBlob.Length / sizeof(double)];
            //Копирование SortedArrayDataBlob в result и преобразование типа byte[] в double[]
            Buffer.BlockCopy(SortedArrayDataBlob, 0, result, 0, SortedArrayDataBlob.Length);
            return result;
        }
        set
        {
            if (value is null)
            {
                SortedArrayDataBlob = null;
                return;
            }
            //Выделения памяти пд массив byte[]
            SortedArrayDataBlob = new byte[value.Length * sizeof(double)];
            //Копирование value в SortedArrayDataBlob и преобразование типа double[] в byte[]
            Buffer.BlockCopy(value, 0, SortedArrayDataBlob, 0, SortedArrayDataBlob.Length);
        }
    }


}