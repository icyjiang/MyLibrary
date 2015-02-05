﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace JLQ.Common
{
    /// <summary>
    /// 自定义绑定列表类(为DataGridView排序用)
    /// </summary>
    /// <typeparam name="T">列表对象类型</typeparam>
    public class BindingCollection<T> : BindingList<T>
    {
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BindingCollection() : base() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">IList类型的列表对象</param>
        public BindingCollection(IList<T> list) : base(list) { }

        /// <summary>
        /// 自定义排序操作
        /// </summary>
        /// <param name="property"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                PropertyInfo info = items[0].GetType().GetProperty(property.Name);
                ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(direction, info);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// 获取一个值，指示列表是否已排序
        /// </summary>
        protected override bool IsSortedCore { get { return isSorted; } }

        /// <summary>
        /// 获取一个值，指示列表是否支持排序
        /// </summary>
        protected override bool SupportsSortingCore { get { return true; } }

        /// <summary>
        /// 获取一个只，指定类别排序方向
        /// </summary>
        protected override ListSortDirection SortDirectionCore { get { return sortDirection; } }

        /// <summary>
        /// 获取排序属性说明符
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore { get { return sortProperty; } }

        /// <summary>
        /// 移除默认实现的排序
        /// </summary>
        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }

    /// <summary>
    /// 实现ICompare接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPropertyCompare<T> : IComparer<T>
    {
        private ListSortDirection _direction;
        private PropertyInfo _propertyInfo;

        // 构造函数
        public ObjectPropertyCompare(ListSortDirection direction, PropertyInfo info)
        {
            this._direction = direction;
            this._propertyInfo = info;
        }

        // 实现IComparer中方法
        public int Compare(T x, T y)
        {
            object xValue = _propertyInfo.GetValue(x, null);
            object yValue = _propertyInfo.GetValue(y, null);

            int returnValue;
            if (xValue != null)
            {
                if (xValue is IComparable)
                    returnValue = ((IComparable)xValue).CompareTo(yValue);
                else if (xValue.Equals(yValue))
                    returnValue = 0;
                else
                    returnValue = xValue.ToString().CompareTo(yValue.ToString());
                return (_direction == ListSortDirection.Ascending ? (-1) : 1) * returnValue;
            }
            if (yValue != null)
                return _direction == ListSortDirection.Ascending ? 1 : (-1);
            return 0;
        }
    }
}
