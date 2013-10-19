using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;

namespace Ricordanza.Data
{
    #region DataRowMapper

    /// <summary>
    /// <see cref="System.Data.DataRow"/>用簡易ORマッパー。<br />
    /// 型指定されたオブジェクトのプロパティに<see cref="System.Data.DataRow"/>のオブジェクトを展開します。<br />
    /// </summary>
    /// <typeparam name="T">データオブジェクト型。デフォルト（引数なし）コンストラクタを有している事。</typeparam>
    /// <remarks>
    /// データテーブルおよび、カラム名に一致するフィールドを持つクラスを引き渡した場合に自動的にデータを詰め込んで返却する。<br />
    /// データの型変換を省力化でき、コード補完によってカラムを思い出せる。
    /// </remarks>
    /// <example>
    /// <code>
    /// public class User
    /// {
    ///     public enum EBloodType { UnKnown, A, B, AB, O }
    /// 
    /// #if NullableFields
    ///     #region NullableFields
    ///     [DataRowTypeMapping]
    ///     public int? Id = default(int?);
    ///     [DataRowTypeMapping]
    ///     public string Name = default(string);
    ///     [DataRowTypeMapping]
    ///     public bool? Sex = default(bool?);
    ///     [DataRowTypeMapping]
    ///     public DateTime? BirthDay = default(DateTime?);
    ///     [DataRowTypeMapping]
    ///     public EBloodType? BloodType = default(EBloodType?);
    ///     #endregion
    /// #endif
    /// 
    /// #if NullableProperties
    ///     #region NullableProperties
    ///     [DataRowTypeMapping]
    ///     public int? Id { get; set; }
    ///     [DataRowTypeMapping]
    ///     public string Name { get; set; }
    ///     [DataRowTypeMapping]
    ///     public bool? Sex { get; set; }
    ///     [DataRowTypeMapping]
    ///     public DateTime? BirthDay { get; set; }
    ///     [DataRowTypeMapping]
    ///     public EBloodType? BloodType { get; set; }
    ///     #endregion
    /// #endif
    /// 
    /// #if NotNullableFields
    ///     #region NotNullableFields
    ///     [DataRowTypeMapping]
    ///     public int Id = default(int);
    ///     [DataRowTypeMapping]
    ///     public string Name = default(string);
    ///     [DataRowTypeMapping]
    ///     public bool Sex = default(bool);
    ///     [DataRowTypeMapping]
    ///     public DateTime BirthDay = default(DateTime);
    ///     [DataRowTypeMapping]
    ///     public EBloodType BloodType = default(EBloodType);
    ///     #endregion
    /// #endif
    /// 
    /// #if NotNullableProperties
    ///     #region NotNullableProperties
    ///     [DataRowTypeMapping]
    ///     public int Id { get; set; }
    ///     [DataRowTypeMapping]
    ///     public string Name { get; set; }
    ///     [DataRowTypeMapping]
    ///     public bool Sex { get; set; }
    ///     [DataRowTypeMapping]
    ///     public DateTime BirthDay { get; set; }
    ///     [DataRowTypeMapping]
    ///     public EBloodType BloodType { get; set; }
    ///     #endregion
    /// #endif
    /// }
    /// 
    /// class Program
    /// {
    ///     static void Main()
    ///     {
    ///         var dt = CreateDataTable(10000);
    /// 
    ///         var mdt = new DataRowTypeMapper&lt;User&gt;(dt) { IsVersionTolerant = true , IsDBNullTolerant = true };
    ///         var sw = new Stopwatch();
    ///         var list = new List&lt;string&gt;();
    /// 
    ///         for (var i = 1; i &lt;= 6; i++)
    ///         {
    ///             sw.Reset();
    ///             sw.Start();
    ///             foreach (DataRow user in dt.Rows)
    ///             {
    ///                 Console.WriteLine("{0}:{1}-{2}({3}", DataRowMapperUtility.GetSqlTypeToType&lt;int?&gt;(user["Id"])
    ///                                                    , DataRowMapperUtility.GetSqlTypeToType&lt;string&gt;(user["Name"])
    ///                                                    , DataRowMapperUtility.GetSqlTypeToType&lt;DateTime?&gt;(user["BirthDay"])
    ///                                                    , DataRowMapperUtility.GetSqlTypeToType&lt;User.EBloodType?&gt;(user["BloodType"]));
    ///             }
    ///             sw.Stop();
    ///             list.Add(string.Format("通常のタイム         {0}回目: {1}", i, sw.Elapsed));
    /// 
    ///             Console.WriteLine();
    /// 
    ///             sw.Reset();
    ///             sw.Start();
    ///             foreach (var user in mdt.Rows)
    ///             {
    ///                 Console.WriteLine("{0}:{1}-{2}({3}", user.Id
    ///                                                    , user.Name
    ///                                                    , user.BirthDay
    ///                                                    , user.BloodType);
    ///             }
    ///             sw.Stop();
    ///             list.Add(string.Format("マッピング時のタイム {0}回目: {1}", i, sw.Elapsed));
    ///         }
    /// 
    ///         Console.WriteLine();
    ///         foreach (var result in list) Console.WriteLine(result);
    ///         Console.WriteLine();
    /// 
    ///         // DBNull.Valueなデータ
    ///         var u = mdt[5];
    ///         Console.WriteLine("{0}:{1}-{2}({3}", u.Id, u.Name, u.BirthDay,u.BloodType);
    ///
    ///         // Add
    ///         var u = new User();
    ///         u.Id = 99999;
    ///         u.Name = "testuser";
    ///         u.BirthDay = DateTime.Now;
    ///         u.BloodType = User.EBloodType.O;
    ///         mdt.AddRow(u);
    ///         
    ///         // Marge
    ///         u = new User();
    ///         u.Id = 8888;
    ///         u.Name = "testuser2";
    ///         u.BirthDay = DateTime.Now;
    ///         u.BloodType = User.EBloodType.B;
    ///         mdt.MargeRow(u, "Id=4");
    /// 
    ///         // Remove
    ///         mdt.RemoveRow("Id=1");
    /// 
    ///         Console.ReadLine();
    ///     }
    ///     
    ///     private static DataTable CreateDataTable(int record)
    ///     {
    ///         var dt = new DataTable();
    ///         dt.Columns.AddRange(
    ///             new DataColumn[]{
    ///             new DataColumn("Id",typeof(int)),
    ///             new DataColumn("Name",typeof(string)),
    ///             new DataColumn("BirthDay",typeof(DateTime)),
    ///             new DataColumn("BloodType",typeof(User.EBloodType))
    ///         });
    /// 
    ///         var r = new System.Random();
    ///         for (var i = 0; i &lt; record; i++)
    ///         {
    ///             var row = dt.NewRow();
    /// 
    ///             // DBNull.Valueなデータをこさえてみるよ
    ///             if (i == 5)
    ///             {
    ///                 row["Id"] = DBNull.Value;
    ///                 row["Name"] = DBNull.Value;
    ///                 row["BirthDay"] = DBNull.Value;
    ///                 row["BloodType"] = DBNull.Value;
    ///                 dt.Rows.Add(row);
    ///                 continue;                
    ///             }
    /// 
    ///             row["Id"] = i;
    ///             row["Name"] = "Test" + i.ToString();
    ///             row["BirthDay"] = new DateTime(2009, 1, 17).AddDays(i);
    ///             var blood = r.Next(0, 4);
    ///             row["BloodType"] = (User.EBloodType)Enum.ToObject(typeof(User.EBloodType), blood);
    ///             dt.Rows.Add(row);
    ///         }
    ///         return dt;
    ///     }
    /// }
    /// 
    /// ■実行結果(前半省略)
    /// 通常のタイム         1回目: 00:00:06.4549531
    /// マッピング時のタイム 1回目: 00:00:06.3403792
    /// 通常のタイム         2回目: 00:00:06.3198922
    /// マッピング時のタイム 2回目: 00:00:06.3391677
    /// 通常のタイム         3回目: 00:00:06.3232300
    /// マッピング時のタイム 3回目: 00:00:06.3234989
    /// 通常のタイム         4回目: 00:00:06.3259166
    /// マッピング時のタイム 4回目: 00:00:06.3382356
    /// 通常のタイム         5回目: 00:00:06.3367330
    /// マッピング時のタイム 5回目: 00:00:06.3249356
    /// 通常のタイム         6回目: 00:00:06.3248639
    /// マッピング時のタイム 6回目: 00:00:06.3314302
    /// </code>
    /// </example>
    public class DataRowMapper<T> where T : new()
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="table">マッピングを行うデータを保持した<see cref="System.Data.DataTable"/></param>
        public DataRowMapper(DataTable table)
            : base()
        {
            if (table == null)
                throw new ArgumentNullException("table is null.");

            Table = table;
        }

        #endregion

        #region property

        /// <summary>
        /// バージョントレラントの有無を取得または設定します。
        /// </summary>
        public bool IsVersionTolerant { get; set; }

        /// <summary>
        /// <c>Nullable</c>型ではないマッピング対象について、<c>DBNull.Value</c>を許容するか否かを取得または設定します。<br />
        /// 通常、<c>DBNull.Value</c>を受け入れるマッピング対象は<c>Nullable</c>型を指定してください。<br />
        /// <br />
        /// ※ <c>DBNull.Value</c>を許容する場合、<c>DBNull.Value</c>は対象の型の既定値(<c>default</c>)として扱われます。<br />
        /// ※ <c>DBNull.Value</c>を許容しない場合、例外を<c>Throw</c>します。
        /// </summary>
        public bool IsDBNullTolerant { get; set; }

        /// <summary>
        /// マッピングしたRowsを取得します。
        /// </summary>
        public IEnumerable<T> Rows
        {
            get
            {
                foreach (DataRow r in Table.Rows) yield return GetMappingRow(r);
            }
        }

        /// <summary>
        /// インデクサ
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>行データ</returns>
        public T this[int index]
        {
            get { return GetMappingRow(Table.Rows[index]); }
        }

        /// <summary>
        /// マッピングを行うデータ
        /// </summary>
        protected DataTable Table { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="System.Data.DataTable"/>の<see cref="System.Data.DataRow"/>を、T型にマッピングします。
        /// </summary>
        /// <param name="row">マッピングする<see cref="System.Data.DataRow"/></param>
        /// <returns>行データ</returns>
        public T GetMappingRow(DataRow row)
        {
            T mapRow = new T();
            Type t = mapRow.GetType();

            foreach (FieldInfo fi in GetTypeMappingFieldInfo(t))
            {
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(fi.Name))
                        continue;
                }
                fi.SetValue(mapRow, GetSqlTypeToType(fi.FieldType, row[fi.Name]));
            }

            foreach (PropertyInfo prp in GetTypeMappingSetPropertyInfo(t))
            {
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(prp.Name))
                        continue;
                }
                prp.SetValue(mapRow, GetSqlTypeToType(prp.PropertyType, row[prp.Name]), null);
            }

            return mapRow;
        }

        /// <summary>
        /// 行データを追加します。
        /// </summary>
        /// <param name="row">行データ</param>
        /// <returns>追加した<see cref="System.Data.DataRow"/></returns>
        public DataRow AddRow(T row)
        {
            DataRow entity = Table.NewRow();
            Table.Rows.Add(entity);

            Type t = typeof(T);
            foreach (FieldInfo fi in GetTypeMappingFieldInfo(t))
            {
                object o = fi.GetValue(row);
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(fi.Name))
                        continue;
                }
                entity[fi.Name] = GetTypeToSqlType(fi.FieldType, o);
            }

            foreach (PropertyInfo prp in GetTypeMappingGetPropertyInfo(t))
            {
                object o = prp.GetValue(row, null);
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(prp.Name))
                        continue;
                }
                entity[prp.Name] = GetTypeToSqlType(prp.PropertyType, o);
            }

            return entity;
        }

        /// <summary>
        /// 行データを更新します。
        /// </summary>
        /// <param name="row">行データ</param>
        /// <param name="expression">更新条件</param>
        /// <returns>更新した<see cref="System.Data.DataRow"/></returns>
        /// <remarks>
        /// 更新条件に一致するデータが存在しない場合は<see cref="System.Data.DataRow"/>を追加します。<br />
        /// 更新条件に一致するデータが複数存在する場合は最初の<see cref="System.Data.DataRow"/>を更新します。
        /// </remarks>
        public DataRow MargeRow(T row, string expression)
        {
            DataRow entity = Table.NewRow();
            var rows = Table.Select(expression);
            if (rows == null || rows.Length == 0)
                Table.Rows.Add(entity);
            else
                entity = rows[0];

            Type t = typeof(T);
            foreach (FieldInfo fi in GetTypeMappingFieldInfo(t))
            {
                object o = fi.GetValue(row);
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(fi.Name))
                        continue;
                }
                entity[fi.Name] = GetTypeToSqlType(fi.FieldType, o);
            }

            foreach (PropertyInfo prp in GetTypeMappingGetPropertyInfo(t))
            {
                object o = prp.GetValue(row, null);
                if (IsVersionTolerant)
                {
                    if (!Table.Columns.Contains(prp.Name))
                        continue;
                }
                entity[prp.Name] = GetTypeToSqlType(prp.PropertyType, o);
            }

            return entity;
        }

        /// <summary>
        /// 行データを削除します。
        /// </summary>
        /// <param name="expression">削除条件</param>
        /// <returns>削除した<see cref="System.Data.DataRow"/></returns>
        /// <remarks>
        /// 削除条件に一致するデータが存在しない場合は<see cref="System.Data.DataRow"/>を削除しません。<br />
        /// 削除条件に一致するデータが複数存在する場合は最初の<see cref="System.Data.DataRow"/>を削除します。
        /// </remarks>
        public DataRow RemoveRow(string expression)
        {
            var rows = Table.Select(expression);
            if (rows == null || rows.Length == 0)
                return null;

            Table.Rows.Remove(rows[0]);

            return rows[0];
        }

        /// <summary>
        /// 前回<see cref="System.Data.DataTable#AcceptChanges"/>呼び出した以降にこのテーブルに対して行われたすべての変更をコミットします。  
        /// </summary>
        public void AcceptChanges()
        {
            Table.AcceptChanges();
        }

        /// <summary>
        /// 前回<see cref="System.Data.DataTable#AcceptChanges"/>呼び出した以降にこのテーブルに対して行われたすべての変更をロールバックします。 
        /// </summary>
        public void RejectChanges()
        {
            Table.RejectChanges();
        }

        /// <summary>
        /// 前回<see cref="System.Data.DataTable"/>を読み取るか、<br />
        /// <see cref="AcceptChanges"/>を呼び出した以降にこのデータセットに対して行われた<br />
        /// すべての変更が格納されているこのデータセットのコピーを取得します。 
        /// </summary>
        /// <returns>変更のコピー。変更がない場合は<c>null</c></returns>
        public DataTable GetChanges()
        {
            return Table.GetChanges();
        }

        /// <summary>
        /// データをクリアします。
        /// </summary>
        public void Clear()
        {
            Table.Rows.Clear();
        }

        /// <summary>
        /// <see cref="System.Data.DataTable"/>を元の状態にリセットします。
        /// </summary>
        public void Reset()
        {
            Table.Reset();
        }

        #endregion

        #region protected method

        /// <summary>
        /// マッピング対象Fieldメタデータを取得します。
        /// </summary>
        /// <param name="t">データ型</param>
        /// <returns><see cref="System.Reflection.FieldInfo"/>反復子</returns>
        protected IEnumerable<FieldInfo> GetTypeMappingFieldInfo(Type t)
        {
            return from f in t.GetFields(BindingFlags.Public
                                       | BindingFlags.Instance)
                   where f.FieldType.IsPrimitive
                       | f.FieldType.IsValueType
                       | f.FieldType == typeof(string)
                   where !f.IsLiteral
                   where f.GetCustomAttributes(typeof(DataRowTypeMappingAttribute), false).Count() != 0
                   select f;
        }

        /// <summary>
        /// マッピング対象<c>SetProperty</c>メタデータを取得します。
        /// </summary>
        /// <param name="t">データ型</param>
        /// <returns><see cref="System.Reflection.PropertyInfo"/>反復子</returns>
        protected IEnumerable<PropertyInfo> GetTypeMappingGetPropertyInfo(Type t)
        {
            return from p in t.GetProperties(BindingFlags.Public
                                           | BindingFlags.Instance
                                           | BindingFlags.GetProperty)
                   where p.PropertyType.IsPrimitive
                       | p.PropertyType.IsValueType
                       | p.PropertyType == typeof(string)
                   where p.GetCustomAttributes(typeof(DataRowTypeMappingAttribute), false).Count() != 0
                   select p;
        }

        /// <summary>
        /// マッピング対象<c>GetProperty</c>メタデータを取得します。
        /// </summary>
        /// <param name="t">データ型</param>
        /// <returns><see cref="System.Reflection.PropertyInfo"/>反復子</returns>
        protected IEnumerable<PropertyInfo> GetTypeMappingSetPropertyInfo(Type t)
        {
            return from p in t.GetProperties(BindingFlags.Public
                                           | BindingFlags.Instance
                                           | BindingFlags.SetProperty)
                   where p.PropertyType.IsPrimitive
                       | p.PropertyType.IsValueType
                       | p.PropertyType == typeof(string)
                   where p.GetCustomAttributes(typeof(DataRowTypeMappingAttribute), false).Count() != 0
                   select p;
        }

        /// <summary>
        /// <c>SqlType</c>に対応する<c>Type</c>を取得します。(<c>DBNull.Value</c>と<c>Nullable</c>型の扱い)
        /// </summary>
        /// <param name="t">データ型</param>
        /// <param name="value">値</param>
        /// <returns>値</returns>
        protected object GetSqlTypeToType(Type t, object value)
        {
            if (DataRowMapperUtility.IsNullable(t))
            {
                if (value == DBNull.Value) 
                    return null;
                
                var ga = t.GetGenericArguments();
                if (ga[0].IsEnum) 
                    return Enum.ToObject(ga[0], value);
                
                return value;
            }

            if (t.IsClass)
            {
                if (value == DBNull.Value)
                    return null;

                return value;
            }

            if (!IsDBNullTolerant && value == DBNull.Value)
                throw new InvalidCastException("It is invalid Cast. The DBNull.Value value cannot be set to the mapping object that is not the Nullable type.");

            if (value == DBNull.Value)
                return Activator.CreateInstance(t);
            
            if (t.IsEnum)
                return Enum.ToObject(t, value);
 
            return value;
        }

        /// <summary>
        /// <c>Type</c>に対応する<c>SqlType</c>を取得します。(<c>Nullable</c>と<c>DBNull.Value</c>型の扱い)
        /// </summary>
        /// <param name="t">データ型</param>
        /// <param name="value">値</param>
        /// <returns>値</returns>
        protected object GetTypeToSqlType(Type t, object value)
        {
            if (DataRowMapperUtility.IsNullable(t))
            {
                if (value == null)
                    return DBNull.Value;

                var ga = t.GetGenericArguments();
                if (ga[0].IsEnum)
                    return Enum.ToObject(ga[0], value);

                return value;
            }

            if (t.IsClass)
            {
                if (value == null)
                    return DBNull.Value;

                return value;
            }

            if (!IsDBNullTolerant && value == null)
                throw new InvalidCastException("It is invalid Cast. The DBNull.Value value cannot be set to the mapping object that is not the Nullable type.");

            if (value == null)
                return Activator.CreateInstance(t);

            if (t.IsEnum)
                return Enum.ToObject(t, value);

            return value;
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region DataRowTypeMappingAttribute

    /// <summary>
    /// <see cref="Ricordanza.Data.DataRowMapper"/>のマッピング対象とする<c>Field</c>または<c>Property</c>に設定します。<br />
    /// <c>Field</c>および<c>Property</c>名は、<c>DataColumn</c>名と同じにしてください。<br />
    /// <br />
    /// <br />
    /// ※ <c>DataRowTypeMapper</c>の<c>IsVersionTolerant</c>プロパティが<c>true</c>の場合、<br />
    ///    マッピング対象と同じ名前の<c>DataColumn</c>が存在しないことを許容します。<br />
    /// <br />
    /// ※ <c>DataRowTypeMapper</c>の<c>IsDBNullTolerant</c>プロパティが<c>true</c>の場合、<br />
    ///    <c>DBNull.Value</c>は、対象の型の既定値(<c>default</c>)として扱われます。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DataRowTypeMappingAttribute : Attribute
    {
        public DataRowTypeMappingAttribute() { }
    }

    #endregion

    #region DataRowMapperUtility

    /// <summary>
    /// ユーティリティ
    /// </summary>
    public static class DataRowMapperUtility
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <c>Nullable</c>かどうかを取得します。
        /// </summary>
        /// <param name="type">データ型</param>
        /// <returns><c>Nullable</c>の場合は<c>true</c>。それ以外の場合は<c>false</c></returns>
        public static bool IsNullable(Type type)
        {
            if (type.IsClass)
                return false;
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            
            return false;
        }

        /// <summary>
        /// <c>SqlType</c>に対応する<c>Type</c>を取得します。(<c>DBNull.Value</c>と<c>Nullable</c>型の扱い)
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="value">値</param>
        /// <returns>値</returns>
        public static T GetSqlTypeToType<T>(object value)
        {
            if (value == DBNull.Value)
                return default(T);
            
            if (DataRowMapperUtility.IsNullable(typeof(T)))
            {
                var ga = typeof(T).GetGenericArguments();
                if (ga[0].IsEnum)
                    return (T)Enum.ToObject(ga[0], value);
            }

            if (typeof(T).IsEnum) 
                return (T)Enum.ToObject(typeof(T), value);
            
            return (T)value;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion
}