using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.Quene
{
    public interface IQuene<T>
    {
        /// <summary>
        /// 取得队列实际元素的个数
        /// </summary>
        /// <returns></returns>
         int Count();

        /// <summary>
        /// 判断队列是否为空
        /// </summary>
        /// <returns></returns>
         bool IsEmpty();

        /// <summary>
        /// 清空队列
        /// </summary>
         void Clear();

        /// <summary>
        /// 入队（即向队列尾部添加一个元素）
        /// </summary>
        /// <param name="item"></param>
         void Enquene(T item);

        /// <summary>
        /// 出队(即从队列头部删除一个元素)
        /// </summary>
        /// <returns></returns>
         T Dequene();

        /// <summary>
        /// 取得队列头部第一元素
        /// </summary>
        /// <returns></returns>
         T Peek();
    }
}
