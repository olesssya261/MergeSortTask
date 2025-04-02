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
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Доступ к данным как double[].
    /// </summary>
    [NotMapped]
    public double[] ArrayData
    {
        get
        {
            if (ArrayDataBlob == null)
                throw new InvalidOperationException("ArrayDataBlob не может быть null");

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

    /// <summary>
    /// Доступ к отсортированным данным как double[].
    /// </summary>
    [NotMapped]
    public double[]? SortedArrayData
    {
        get => SortedArrayDataBlob == null ? null : GetDoublesFromBytes(SortedArrayDataBlob);
        set => SortedArrayDataBlob = value == null ? null : GetBytesFromDoubles(value);
    }

    private static double[] GetDoublesFromBytes(byte[] bytes)
    {
        var result = new double[bytes.Length / sizeof(double)];
        Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
        return result;
    }

    private static byte[] GetBytesFromDoubles(double[] doubles)
    {
        var bytes = new byte[doubles.Length * sizeof(double)];
        Buffer.BlockCopy(doubles, 0, bytes, 0, bytes.Length);
        return bytes;
    }
}