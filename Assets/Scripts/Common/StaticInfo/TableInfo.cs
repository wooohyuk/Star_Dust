// 일단 Json을 사용하는 TableInfo는 사용 X.

namespace Common.StaticInfo
{
    public interface IStaticTableInfo
    {
        void Init(string resourcePath, bool isBinary, bool truncate, bool isReload);
    }
}