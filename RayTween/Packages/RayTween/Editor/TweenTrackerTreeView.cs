using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace RayTween.Editor
{
    internal sealed class TweenTrackerViewItem : TreeViewItem
    {
        public TweenTrackerViewItem(int id) : base(id) { }

        static readonly Regex removeHref = new("<a href.+>(.+)</a>", RegexOptions.Compiled);

        public string TweenType { get; set; }
        public string SchedulerType { get; set; }
        public string Elapsed { get; set; }

        string position;
        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                PositionFirstLine = value == null ? string.Empty : GetFirstLine(position);
            }
        }

        public string PositionFirstLine { get; private set; }

        static string GetFirstLine(string str)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\r' || str[i] == '\n')
                {
                    break;
                }
                sb.Append(str[i]);
            }

            return removeHref.Replace(sb.ToString(), "$1");
        }
    }

    internal sealed class TweenTrackerTreeView : TreeView
    {
        const string sortedColumnIndexStateKey = "TweenTrackerTreeView_sortedColumnIndex";

        public IReadOnlyList<TreeViewItem> CurrentBindingItems;

        public TweenTrackerTreeView()
            : this(new TreeViewState(), new MultiColumnHeader(new MultiColumnHeaderState(new[]
            {
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Tween Type"), width = 55},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Scheduler"), width = 25},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Elapsed"), width = 15},
                new MultiColumnHeaderState.Column() { headerContent = new GUIContent("Position")},
            })))
        {

        }

        TweenTrackerTreeView(TreeViewState state, MultiColumnHeader header)
            : base(state, header)
        {
            rowHeight = 20;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            header.sortingChanged += HeaderSortingChanged;

            header.ResizeToFit();
            Reload();

            header.sortedColumnIndex = SessionState.GetInt(sortedColumnIndexStateKey, 1);
        }

        public void ReloadAndSort()
        {
            var currentSelected = state.selectedIDs;
            Reload();
            HeaderSortingChanged(multiColumnHeader);
            state.selectedIDs = currentSelected;
        }

        void HeaderSortingChanged(MultiColumnHeader multiColumnHeader)
        {
            SessionState.SetInt(sortedColumnIndexStateKey, multiColumnHeader.sortedColumnIndex);
            var index = multiColumnHeader.sortedColumnIndex;
            var ascending = multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex);

            var items = rootItem.children.Cast<TweenTrackerViewItem>();
            IOrderedEnumerable<TweenTrackerViewItem> orderedEnumerable = index switch
            {
                0 => ascending ? items.OrderBy(item => item.TweenType) : items.OrderByDescending(item => item.TweenType),
                1 => ascending ? items.OrderBy(item => item.SchedulerType) : items.OrderByDescending(item => item.SchedulerType),
                2 => ascending ? items.OrderBy(item => double.Parse(item.Elapsed)) : items.OrderByDescending(item => double.Parse(item.Elapsed)),
                3 => ascending ? items.OrderBy(item => item.Position) : items.OrderByDescending(item => item.PositionFirstLine),
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null),
            };
            CurrentBindingItems = rootItem.children = orderedEnumerable.Cast<TreeViewItem>().ToList();
            BuildRows(rootItem);
        }

        static string GetSchedulerName(ITweenScheduler scheduler)
        {
            static string GetTimeKindName(TweenTimeKind tweenTimeKind)
            {
                return tweenTimeKind switch
                {
                    TweenTimeKind.Time => "",
                    TweenTimeKind.UnscaledTime => "IgnoreTimeScale",
                    TweenTimeKind.Realtime => "Realtime",
                    _ => null
                };
            }
#if UNITY_EDITOR
            if(!EditorApplication.isPlaying)return "EditorUpdate";
#else
                scheduler = TweenScheduler.Update;
#endif
           
            return scheduler switch
            {
                PlayerLoopTweenScheduler loopTweenScheduler => loopTweenScheduler.UpdateTiming.ToString() + GetTimeKindName(loopTweenScheduler.timeKind),
                ManualTweenScheduler => "Manual",
                _ => scheduler.GetType()?.Name,
            };
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { depth = -1 };
            var children = new List<TreeViewItem>();

            var id = 0;
            foreach (var tracking in TweenTracker.Items)
            {
                children.Add(new TweenTrackerViewItem(id)
                { 
                    TweenType = $"[{tracking.ValueType.Name}, {tracking.PluginType.Name}]",
                    SchedulerType = GetSchedulerName(tracking.Scheduler),
                    Elapsed = (DateTime.UtcNow - tracking.CreationTime).TotalSeconds.ToString("00.00"),
                    Position = tracking.StackTrace?.AddHyperLink()
                });
                id++;
            }

            CurrentBindingItems = children;
            root.children = CurrentBindingItems as List<TreeViewItem>;
            return root;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as TweenTrackerViewItem;

            for (var visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); visibleColumnIndex++)
            {
                var rect = args.GetCellRect(visibleColumnIndex);
                var columnIndex = args.GetColumn(visibleColumnIndex);

                var labelStyle = args.selected ? EditorStyles.whiteLabel : EditorStyles.label;
                labelStyle.alignment = TextAnchor.MiddleLeft;
                switch (columnIndex)
                {
                    case 0:
                        EditorGUI.LabelField(rect, item.TweenType, labelStyle);
                        break;
                    case 1:
                        EditorGUI.LabelField(rect, item.SchedulerType, labelStyle);
                        break;
                    case 2:
                        EditorGUI.LabelField(rect, item.Elapsed, labelStyle);
                        break;
                    case 3:
                        EditorGUI.LabelField(rect, item.PositionFirstLine, labelStyle);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, null);
                }
            }
        }
    }

}