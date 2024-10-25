namespace SV21T1020228.DataLayers
{
    /// <summary>
    /// Lấy toàn bộ dữ liệu của 1 bảng
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimpleQueryDAL<T> where T : class
    {
        List<T> List();
    }
}
