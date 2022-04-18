using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XLuaTest
{
    public class Sysmbols : EditorWindow
    {
        private static Sysmbols _sysmbolsWindows;
        private const string SYSMBOLS_STRING = "Tools/sysmbols";
        private const string SYSMBOLS_NAME = "Sysmbols";

        [MenuItem(SYSMBOLS_STRING)]
        public static void Init()
        {
            _sysmbolsWindows = EditorWindow.CreateWindow<Sysmbols>();
            _sysmbolsWindows.name = SYSMBOLS_NAME;
        }

        public class SysmbolsData
        {
            public string name = "";
            public string des = "";
            public bool isClick = false;

            public SysmbolsData(string _name, string _des)
            {
                name = _name;
                des = _des;
            }
        }

        private List<SysmbolsData> SysmbolsDatasList = new List<SysmbolsData>()
        {
            new SysmbolsData("HOTFIX_ENABLE", "xlua的hotfix")
        };

        private void InitSysmbols()
        {
            var defineSymbols = PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
            foreach (var it in SysmbolsDatasList)
            {
                it.isClick = defineSymbols.Any(a=>a==it.name);
            }
        }

        private Vector2 verticalVec;
        //gui
        public void OnGUI()
        {//排序
            EditorGUILayout.BeginVertical();
            verticalVec = EditorGUILayout.BeginScrollView(
                scrollPosition: verticalVec,options:
                GUILayout.Height(position.height));
            foreach (var item in SysmbolsDatasList)
            {
                //标准是box包裹
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                item.isClick = EditorGUILayout.Toggle(item.isClick, GUILayout.Width(16));
                EditorGUILayout.LabelField(item.name, GUILayout.ExpandWidth(true), GUILayout.MinWidth(0));
                EditorGUILayout.LabelField(item.des, GUILayout.ExpandWidth(true), GUILayout.MinWidth(0));
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("save"))
            {
                var str = SysmbolsDatasList.Where(a => a.isClick).Select(it => it.name).ToArray();
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildTargetGroup.Android,
                    string.Join(";",str)
                );
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildTargetGroup.Standalone,
                    string.Join(";",str)
                ); 
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildTargetGroup.iOS,
                    string.Join(";",str)
                );
                
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        
        }
    }
}