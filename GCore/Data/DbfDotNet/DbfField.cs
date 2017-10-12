using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Data.DbfDotNet
{

    //internal interface IDbfValue<T>
    //    where T : IComparable<T>
    //{
    //    bool OriginalIsNull { get; set; }
    //    T OriginalValue { get; set; }
    //    bool IsNull { get; set; }
    //    T Value { get; set; }
    //}

    //public struct DbfField<T> : IDbfValue<T> 
    //    where T : struct, IComparable<T>
    //{
    //    private bool mIsNull;
    //    internal T mValue;

    //    private bool mOriginalIsNull;
    //    internal T mOriginalValue;

    //    public DbfField(T value)
    //    {
    //        mOriginalIsNull = true;
    //        this.mOriginalValue = default(T);

    //        mIsNull = false;
    //        this.mValue = value;
    //    }

    //    public T Value
    //    {
    //        get
    //        {
    //            if (this.mIsNull)
    //            {
    //                throw new Exception("No value available.");
    //            }
    //            return this.mValue;
    //        }
    //        set
    //        {
    //            mValue = value;
    //            mIsNull = false;
    //        }
    //    }

    //    public override bool Equals(object other)
    //    {
    //        if (this.mIsNull)
    //        {
    //            return (other == null);
    //        }
    //        if (other == null)
    //        {
    //            return false;
    //        }
    //        return this.mValue.Equals(other);
    //    }
                
    //    public static implicit operator DbfField<T>(T value)
    //    {
    //        return new DbfField<T>(value);
    //    }

    //    public static explicit operator T(DbfField<T> value)
    //    {
    //        return value.Value;
    //    }

    //    #region IDbfValue<T> Members


    //    public T OriginalValue
    //    {
    //        get { return mOriginalValue; }
    //        set { mOriginalValue = value; }
    //    }

    //    #endregion
    //}

    //public struct DbfString : IDbfValue<string>
    //{
    //    internal string mValue;
    //    internal string mOriginalValue;

    //    public DbfString(string value)
    //    {
    //        this.mOriginalValue = null;
    //        this.mValue = value;
    //    }

    //    #region IDbfValue<string> Members

    //    bool IDbfValue<string>.OriginalIsNull
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    string IDbfValue<string>.OriginalValue
    //    {
    //        get
    //        {
    //            return mOriginalValue;
    //        }
    //        set
    //        {
    //            mOriginalValue = value;
    //        }
    //    }

    //    public bool IsNull // implements IDbfValue<string>.IsNull
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public string Value // implements IDbfValue<string>.Value
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {

    //            throw new NotImplementedException();
    //        }
    //    }


    //    #endregion
    //}


 

    //public struct DbfInt32 : IDbfValue<Int32>
    //{
    //    #region IDbfValue<int> Members

    //    int IDbfValue<int>.Value
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    int IDbfValue<int>.OriginalValue
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    bool IDbfValue<int>.IsNull
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    bool IDbfValue<int>.OriginalIsNull
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    #endregion
    //}
}
