using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// MachineListUC.xaml 的交互逻辑
    /// </summary>
    public partial class MachineSelectUC : UserControl
    {
        public MachineSelectUC()
        {
            InitializeComponent();

            var machineTrees = new List<MachineTree>
            {
                new MachineTree() { Id = 1, Name = Current.Tester.Name, ParentId = 0 ,IsChecked = Current.Tester.IsEnable},
                new MachineTree() { Id = 2, Name = Current.Collector.Name, ParentId = 0 ,IsChecked = Current.Collector.IsEnable},
                new MachineTree() { Id = 3, Name = Current.Cooler.Name, ParentId = 0 ,IsChecked = Current.Cooler.IsEnable},
                new MachineTree() { Id = 4, Name = Current.Scaner.Name, ParentId = 0 ,IsChecked = Current.Scaner.IsEnable},
                //new MachineTree() { Id = 5, Name = Current.Cooler.PLC.Name, ParentId = 4 ,IsChecked = Current.Cooler.PLC.IsEnable},
                new MachineTree() { Id = 6, Name = Current.Mes.Name, ParentId = 0 ,IsChecked = Current.Mes.IsEnable},
            };

            this.SetItemsSourceData(machineTrees, m => m.Name, m => m.Id, m => m.ParentId, m => m.IsChecked);
        }
        public IList<TreeViewItemModel> ItemsSourceData
        {
            get { return (IList<TreeViewItemModel>)innerTree.ItemsSource; }
        }

        /// <summary>
        /// 设置数据源, 以及各个字段
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TId">主键类型</typeparam>
        /// <param name="itemsArray">数据源列表</param>
        /// <param name="captionSelector">指定显示为Caption的属性</param>
        /// <param name="idSelector">指定主键属性</param>
        /// <param name="parentIdSelector">指定父项目主键属性</param>
        public void SetItemsSourceData<TSource, TId>(IEnumerable<TSource> itemsArray, Func<TSource, string> captionSelector, Func<TSource, TId> idSelector, Func<TSource, TId> parentIdSelector, Func<TSource, bool> isCheckedSelector)
                where TId : IEquatable<TId>
        {
            var list = new List<TreeViewItemModel>();

            foreach (var item in itemsArray.Where(a => object.Equals(default(TId), parentIdSelector(a))))
            {
                var tvi = new TreeViewItemModel();
                tvi.Caption = captionSelector(item).ToString();
                tvi.Id = idSelector(item);
                tvi.Tag = item;
                tvi.TagType = item.GetType();
                tvi.IsChecked = isCheckedSelector(item);
                list.Add(tvi);
                RecursiveAddChildren(tvi, itemsArray, captionSelector, idSelector, parentIdSelector, isCheckedSelector);
            }

            innerTree.ItemsSource = list;
            return;
        }

        /// <summary>
        /// 递归加载children
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="parent"></param>
        /// <param name="itemsArray"></param>
        /// <param name="captionSelector"></param>
        /// <param name="idSelector"></param>
        /// <param name="parentIdSelector"></param>
        /// <returns></returns>
        private TreeViewItemModel RecursiveAddChildren<TSource, TId>(TreeViewItemModel parent, IEnumerable<TSource> itemsArray, Func<TSource, string> captionSelector, Func<TSource, TId> idSelector, Func<TSource, TId> parentIdSelector, Func<TSource, bool> isCheckedSelector)
        {

            foreach (var item in itemsArray.Where(a => object.Equals(parent.Id, parentIdSelector(a))))
            {
                var tvi = new TreeViewItemModel();
                tvi.Caption = captionSelector(item);
                tvi.Id = idSelector(item);
                tvi.Tag = item;
                tvi.TagType = item.GetType();
                tvi.Parent = parent;
                tvi.IsChecked = isCheckedSelector(item);
                parent.Children.Add(tvi);
                RecursiveAddChildren(tvi, itemsArray, captionSelector, idSelector, parentIdSelector, isCheckedSelector);
            }
            return parent;
        }
    }

    public class TreeViewItemModel : INotifyPropertyChanged
    {

        #region 属性

        public event PropertyChangedEventHandler PropertyChanged;

        protected object id;
        /// <summary>
        /// 唯一性Id
        /// </summary>
        public object Id
        {
            get { return id; }
            set { id = value; }
        }

        protected string caption;
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { caption = value; if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Caption")); }
        }

        protected bool isChecked;
        /// <summary>
        /// 是否被勾选
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
                SetIsCheckedByParent(value);
                if (Parent != null) Parent.SetIsCheckedByChild(value);

                if (AppCurrent.IsTerminalInitFinished)
                {

                    //**************改变设备启用状态 Start***********************
                    if (Caption == Current.Tester.Name)
                    {
                        Current.Tester.IsEnable = value;
                        Context.InsulationContext.SaveChangesAsync();
                    }
                    else if (Caption == Current.Collector.Name)
                    {
                        Current.Collector.IsEnable = value;
                        Context.CollectorContext.SaveChangesAsync();
                    }
                    else if (Caption == Current.Cooler.Name)
                    {
                        Current.Cooler.IsEnable = value;
                        Context.CoolerContext.SaveChangesAsync();
                    }
                    else if (Caption == Current.Scaner.Name)
                    {
                        Current.Scaner.IsEnable = value;
                        Context.ScanerContext.SaveChangesAsync();
                    }
                    else if (Caption == Current.Mes.Name)
                    {
                        Current.Mes.IsEnable = value;
                        Context.MesContext.SaveChangesAsync();
                    }
                    //**************改变设备启用状态 Finished********************

                }
            }
        }

        protected bool isExpanded = true;
        /// <summary>
        /// 是否被展开
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsExpanded")); }
        }


        protected TreeViewItemModel parent;
        /// <summary>
        /// 父项目
        /// </summary>
        public TreeViewItemModel Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected List<TreeViewItemModel> children = new List<TreeViewItemModel>();
        /// <summary>
        /// 含有的子项目
        /// </summary>
        public List<TreeViewItemModel> Children
        {
            get { return children; }
            set { children = value; }
        }


        /// <summary>
        /// 包含的对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 包含对象的类型
        /// </summary>
        public Type TagType { get; set; }
        #endregion


        #region 业务逻辑, 如果你需要改成其他逻辑, 要修改的也就是这两行

        /// <summary>
        /// 子项目的isChecked改变了, 通知 是否要跟着改变 isChecked
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetIsCheckedByChild(bool value)
        {
            if (this.isChecked == value)
            {
                return;
            }

            bool isAllChildrenChecked = this.Children.All(c => c.IsChecked == true);
            this.isChecked = isAllChildrenChecked;
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
            if (Parent != null) Parent.SetIsCheckedByChild(value);
        }

        /// <summary>
        /// 自己的isChecked改变了, 所有子项目都要跟着改变
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetIsCheckedByParent(bool value)
        {
            this.isChecked = value;
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
            foreach (var child in Children)
            {
                child.SetIsCheckedByParent(value);
            }
        }
        #endregion

    }

    public class MachineTree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public bool IsChecked { get; set; }
    }
}
