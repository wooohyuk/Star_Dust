using System;
using System.Collections.Generic;
namespace Common.StaticData
{
    public class IntegerKeyType : Tuple<int, int>
    {
        public IntegerKeyType(int id, int level) : base(id, level) { }
    }
    public class StringKeyType : Tuple<string, int>
    {
        public StringKeyType(string id, int level) : base(id, level) { }
    }

    public class TestKey : List<int>
    {
    }
    public interface ITableData<T>
    {
        T Key
        {
            get;
        }
    }
    public abstract class IntegerKeyTable : ITableData<IntegerKeyType>
    {
        private IntegerKeyType _key;
        public int Id;
        public int Level;
        public IntegerKeyType Key
        {
            get
            {
                if (_key == null)
                {
                    _key = new IntegerKeyType(Id, Level);
                }
                return _key;
            }
        }
    }

    public abstract class StringKeyTable : ITableData<StringKeyType>
    {
        private StringKeyType _key;
        public string Id;
        public int Level;

        public StringKeyType Key
        {
            get
            {
                if (_key == null)
                {
                    _key = new StringKeyType(Id, Level);
                }
                return _key;
            }
        }
    }
}