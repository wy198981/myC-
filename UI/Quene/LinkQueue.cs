using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.Quene
{
    public class LinkQueue
    {
        private Node<ModelNode> front;//队列头
        private Node<ModelNode> rear;//队列尾
        private int num;//队列元素个数


        /// 
        /// 构造器
        /// 
        public LinkQueue()
        {
            //初始时front,rear置为null，num置0
            front = rear = null;
            num = 0;
        }

        public int Count()
        {
            return this.num;
        }


        public void Clear()
        {
            front = rear = null;
            num = 0;
        }

        public bool IsEmpty()
        {
            return (front == rear && num == 0);
        }

        //入队
        public void Enqueue(ModelNode item)
        {
            Node<ModelNode> q = new Node<ModelNode>(item);

            if (rear == null)//第一个元素入列时
            {
                front = rear = q;
            }
            else
            {
                //把新元素挂到链尾
                rear.Next = q;
                //修正rear指向为最后一个元素
                rear = q;
            }
            //元素总数+1
            num++;
        }

        //出队
        public ModelNode Dequeue()
        {
            if (IsEmpty())
            {
               // Console.WriteLine("Queue is empty!");
                return default(ModelNode);
            }

            //取链首元素
            Node<ModelNode> p = front;

            //链头指向后移一位
            front = front.Next;

            //如果此时链表为空，则同步修正rear
            if (front == null)
            {
                rear = null;
            }

            num--;//个数-1

            return p.Data;
        }


        public ModelNode Peek()
        {
            if (IsEmpty())
            {
                //Console.WriteLine("Queue is empty!");
                return default(ModelNode);
            }

            return front.Data;
        }


        public override string ToString()
        {
            if (IsEmpty())
            {
                //Console.WriteLine("Queue is empty!");
            }

            StringBuilder sb = new StringBuilder();

            Node<ModelNode> node = front;

            sb.Append(node.Data.ToString());

            while (node.Next != null)
            {
                sb.Append("," + node.Next.Data.ToString());
                node = node.Next;
            }

            return sb.ToString().Trim(',');
        }
    }
}
