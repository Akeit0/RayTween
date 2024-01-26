using System.Linq;
using UnityEngine;
using UnityEditor;

namespace RayTween.Editor
{
    /// <summary>
    /// Editor window that displays a list of tweens being tracked.
    /// </summary>
    public class TweenTrackerWindow : EditorWindow
    {
        static TweenTrackerWindow instance;

        [MenuItem("Window/RayTween/Tween Tracker")]
        public static void OpenWindow()
        {
            if (instance != null) instance.Close();
            GetWindow<TweenTrackerWindow>("Tween Tracker").Show();
        }

        static readonly GUILayoutOption[] EmptyLayoutOption = new GUILayoutOption[0];

        TweenTrackerTreeView treeView;
        object splitterState;

        const string EnableTrackingPrefsKey = "RayTween-TweenTracker-EnableTracking";
        const string EnableStackTracePrefsKey = "RayTween-TweenTracker-EnableStackTrace";

        void OnEnable()
        {
            instance = this;
            splitterState = SplitterGUILayout.CreateSplitterState(new float[] { 75f, 25f }, new int[] { 32, 32 }, null);
            treeView = new TweenTrackerTreeView();
            TweenTracker.EnableTracking = EditorPrefs.GetBool(EnableTrackingPrefsKey, false);
            TweenTracker.EnableStackTrace = EditorPrefs.GetBool(EnableStackTracePrefsKey, false);
        }

        void OnGUI()
        {
            RenderHeadPanel();
            SplitterGUILayout.BeginVerticalSplit(this.splitterState, EmptyLayoutOption);
            RenderTable();
            RenderDetailsPanel();
            SplitterGUILayout.EndVerticalSplit();
        }

        static readonly GUIContent ClearHeadContent = EditorGUIUtility.TrTextContent(" Clear ");
        static readonly GUIContent EnableTrackingHeadContent = EditorGUIUtility.TrTextContent("Enable Tracking");
        static readonly GUIContent EnableStackTraceHeadContent = EditorGUIUtility.TrTextContent("Enable Stack Trace");

        void RenderHeadPanel()
        {
            EditorGUILayout.BeginVertical(EmptyLayoutOption);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, EmptyLayoutOption);

            if (GUILayout.Toggle(TweenTracker.EnableTracking, EnableTrackingHeadContent, EditorStyles.toolbarButton, EmptyLayoutOption) != TweenTracker.EnableTracking)
            {
                TweenTracker.EnableTracking = !TweenTracker.EnableTracking;
                EditorPrefs.SetBool(EnableTrackingPrefsKey, TweenTracker.EnableTracking);
            }

            if (GUILayout.Toggle(TweenTracker.EnableStackTrace, EnableStackTraceHeadContent, EditorStyles.toolbarButton, EmptyLayoutOption) != TweenTracker.EnableStackTrace)
            {
                TweenTracker.EnableStackTrace = !TweenTracker.EnableStackTrace;
                EditorPrefs.SetBool(EnableStackTracePrefsKey, TweenTracker.EnableStackTrace);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(ClearHeadContent, EditorStyles.toolbarButton, EmptyLayoutOption))
            {
                TweenTracker.Clear();
                treeView.ReloadAndSort();
                Repaint();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        Vector2 tableScroll;
        GUIStyle tableListStyle;

        void RenderTable()
        {
            if (tableListStyle == null)
            {
                tableListStyle = new GUIStyle("CN Box");
                tableListStyle.margin.top = 0;
                tableListStyle.padding.left = 3;
            }

            EditorGUILayout.BeginVertical(tableListStyle, EmptyLayoutOption);

            tableScroll = EditorGUILayout.BeginScrollView(this.tableScroll, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MaxWidth(2000f)
            });
            var controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true)
            });

            treeView?.OnGUI(controlRect);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        static int interval;
        void Update()
        {
            if (interval++ % 120 == 0)
            {
                treeView.ReloadAndSort();
                Repaint();
            }
        }

        static GUIStyle detailsStyle;
        Vector2 detailsScroll;

        void RenderDetailsPanel()
        {
            if (detailsStyle == null)
            {
                detailsStyle = new GUIStyle("CN Message")
                {
                    wordWrap = false,
                    stretchHeight = true
                };
                detailsStyle.margin.right = 15;
            }

            string message = "";
            var selected = treeView.state.selectedIDs;
            if (selected.Count > 0)
            {
                var first = selected[0];
                if (treeView.CurrentBindingItems.FirstOrDefault(x => x.id == first) is TweenTrackerViewItem item)
                {
                    message = item.Position;
                }
            }

            detailsScroll = EditorGUILayout.BeginScrollView(this.detailsScroll, EmptyLayoutOption);
            var vector = detailsStyle.CalcSize(new GUIContent(message));
            EditorGUILayout.SelectableLabel(message, detailsStyle, new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(vector.x),
                GUILayout.MinHeight(vector.y)
            });
            EditorGUILayout.EndScrollView();
        }
    }
}