using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Модель для хранения данных о сортировке массива чисел double в базе данных.
/// Основное поле ArrayDataBlob обязательно для заполнения.
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
    /// Исходный массив данных в бинарном формате (обязательное поле).
    /// </summary>
    /// <remarks>
    /// Для работы с массивом double используйте <see cref="ArrayData"/>.
    /// </remarks>
    public byte[] ArrayDataBlob { get; set; }

    /// <summary>
    /// Отсортированный массив в бинарном формате (опционально).
    /// </summary>

    public byte[]? SortedArrayDataBlob { get; set; }

    // Остальные nullable свойства
    public uint? Swaps { get; set; }
    public uint? Comparisons { get; set; }
    public string? SortType { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Доступ к данным как double[].
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

            var result = new double[ArrayDataBlob.Length / sizeof(double)];
            Buffer.BlockCopy(ArrayDataBlob, 0, result, 0, ArrayDataBlob.Length);
            return result;
        }
        set
        {
            ArrayDataBlob = new byte[value.Length * sizeof(double)];
            Buffer.BlockCopy(value, 0, ArrayDataBlob, 0, ArrayDataBlob.Length);
        }
    }
    [NotMapped]
    public double[] SortedArrayData
    {
        get
        {
            if (SortedArrayDataBlob == null)
                return null;
            if (SortedArrayDataBlob.Length % sizeof(double) != 0)
                throw new InvalidOperationException("Некорректный размер данных");

            var result = new double[SortedArrayDataBlob.Length / sizeof(double)];
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
            SortedArrayDataBlob = new byte[value.Length * sizeof(double)];
            Buffer.BlockCopy(value, 0, SortedArrayDataBlob, 0, SortedArrayDataBlob.Length);
        }
    }



    public void UpdateData(SortArrayModel sortArrayModel)
    {
        if (sortArrayModel == null)
            throw new ArgumentNullException(nameof(sortArrayModel));

        ArrayDataBlob = sortArrayModel.ArrayDataBlob != null
            ? (byte[])sortArrayModel.ArrayDataBlob.Clone()
            : null;

        SortedArrayDataBlob = sortArrayModel.SortedArrayDataBlob != null
            ? (byte[])sortArrayModel.SortedArrayDataBlob.Clone()
            : null;

        Swaps = sortArrayModel.Swaps;
        Comparisons = sortArrayModel.Comparisons;
        SortType = sortArrayModel.SortType;
        CreatedAt = sortArrayModel.CreatedAt;

    }

}