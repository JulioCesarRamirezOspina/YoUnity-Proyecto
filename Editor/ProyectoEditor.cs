using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Yosoft.Proyecto.Editor
{
    public class ProyectoEditorWindow : EditorWindow
    {
        private Vector2 scrollPosition;

        [MenuItem("Yosoft/Proyecto")]
        private static void ShowWindow()
        {
            var window = GetWindow<ProyectoEditorWindow>();
            window.titleContent = new GUIContent("Proyecto");
            window.Show();
        }

        private void OnEnable()
        {
            minSize = new Vector2(300, 100);
        }

        private void OnGUI()
        {
            DrawToolbar();
            DrawSceneList();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Escenas en build");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Configuración Build", EditorStyles.toolbarButton))
                {
                    EditorApplication.ExecuteMenuItem("File/Build Settings...");
                }
            }
        }

        private void DrawSceneList()
        {
            //Drawing the scroll view
            using (var scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                    DrawSceneListItem(i, scene);
                }
            }
        }

        private void DrawSceneListItem(int i, EditorBuildSettingsScene scene)
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(i.ToString(), GUILayout.Width(20));
                GUILayout.Label(new GUIContent(sceneName, scene.path));
                GUILayout.FlexibleSpace();
                // Buttons...
                if (GUILayout.Button("Cargar"))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(scene.path);
                }

                if (GUILayout.Button("Load Additively"))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
                }

                if (GUILayout.Button("..."))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Localizar"), false, () =>
                    {
                        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                        EditorGUIUtility.PingObject(sceneAsset);
                    });
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Item 2"), false, () => { });
                    menu.AddItem(new GUIContent("Item 3"), false, () => { });
                    menu.AddDisabledItem(new GUIContent("Item 4"));
                    menu.AddItem(new GUIContent("Item Selected"), true, () => { });
                    menu.AddItem(new GUIContent("Item 3/More item here"), false, ()
                        => { });
                    menu.AddItem(new GUIContent("Item 3/More item here 2"), false,
                        () => { });
                    menu.ShowAsContext();
                }
            }
        }
    }
}