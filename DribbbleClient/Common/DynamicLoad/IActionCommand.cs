using System;
using System.Windows.Input;
namespace DribbbleClient.Common.DynamicLoad
{

    public interface IActionCommand<T> : ICommand
    {
        /// <summary>
        /// 设置执行函数
        /// </summary>
        /// <param name="action"></param>
        void SetAction(Action<T> action);

        /// <summary>
        /// 设置确定此命令是否可以在其当前状态下执行的方法
        /// </summary>
        /// <param name="canExecute"></param>
        void SetCanExecute(Func<T, bool> canExecute);

        /// <summary>
        /// 执行函数改变时发生
        /// </summary>
        event EventHandler ActionChanged;
    }
}
