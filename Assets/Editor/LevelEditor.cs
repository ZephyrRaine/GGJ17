using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    int nbScreens;
    GameObject curSequenceObject = null;
    List<GameObject> curObjectList;
    GameObject curObstruction;
    Vector2 newObjectPosition;
    Vector2 futurePosition;
    int screenOffset;
    ObjectType newObjectType;

    [MenuItem("Window/GGJ17 Level Editor")]
    static void OpenWindow()
    {
        LevelEditor editor = EditorWindow.GetWindow<LevelEditor>(true, "Level Editor");
        editor.Show();
        editor.OnSelectionChange();
        editor.curObjectList = new List<GameObject>();
    }

    void OnEnable()
    {

    }

    void OnSelectionChange()
    {

    }

    void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (curSequenceObject == null)
            return;
        else
        {        // grab the center of the parent
            Vector3 center = curSequenceObject.transform.position;

            for (int i = 0; i <= nbScreens; i++)
            {
                Handles.DrawWireCube((center + (18 / 2) * Vector3.right) + Vector3.right * 18 * i, new Vector3(18, 10));
                Handles.Label(center + Vector3.right * i * 18 + Vector3.up * 6.5f, "screen" + i.ToString());
            }
            Handles.DrawWireDisc(center + new Vector3(futurePosition.x, futurePosition.y), Vector3.forward, 0.25f);
        }
    }

    void OnGUI()
    {
        // Mesh Generator
        EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

        if (curSequenceObject == null)
        {
            nbScreens = EditorGUILayout.IntField("Nombre écrans", nbScreens);
            if (GUILayout.Button("New Sequence"))
            {
                curSequenceObject = GameObject.Find("SequenceContainer");
                if (curSequenceObject == null)
                {
                    curSequenceObject = new GameObject("SequenceContainer");
                }
            }
        }
        else
        {


            newObjectPosition = EditorGUILayout.Vector2Field("Object Position", newObjectPosition);

            screenOffset = EditorGUILayout.IntField("OnScreen", screenOffset);
            futurePosition = (Vector2.right * screenOffset * 18) + newObjectPosition;
            bool shouldBeDisabled = (newObjectPosition.x > 18 || newObjectPosition.x < 0 || newObjectPosition.y < -5 || newObjectPosition.y > 5);
            if (shouldBeDisabled)
                EditorGUILayout.HelpBox("Object Position is outside sequence bounds !", MessageType.Info);
            EditorGUI.BeginDisabledGroup(shouldBeDisabled);
            EditorGUI.BeginChangeCheck();
            newObjectType = (ObjectType)EditorGUILayout.EnumPopup("Object Type", newObjectType);

            switch (newObjectType)
            {
                case ObjectType.O_OBSTRUCTION:
                    curObstruction = EditorGUILayout.ObjectField("Prefab", curObstruction, typeof(GameObject), false) as GameObject;
                    break;
            }


            if (GUILayout.Button("Add new object"))
            {
                GameObject spawner = new GameObject(newObjectType.ToString() + "(" + newObjectPosition.ToString() + ")", typeof(SpawnerHelper), typeof(SpriteRenderer));
                spawner.transform.position = futurePosition;
                spawner.transform.parent = curSequenceObject.transform;
                Texture2D t;
                switch (newObjectType)
                {
                    case ObjectType.O_OBSTRUCTION:
                        t = Resources.Load("EditorPlaceholders/01", typeof(Texture2D)) as Texture2D;
                        break;
                    case ObjectType.O_OURSIN:
                        t = Resources.Load("EditorPlaceholders/02", typeof(Texture2D)) as Texture2D;
                        break;
                    case ObjectType.O_ROCHER:
                        t = Resources.Load("EditorPlaceholders/03", typeof(Texture2D)) as Texture2D;
                        break;
                    case ObjectType.O_GEYSER:
                        t = Resources.Load("EditorPlaceholders/04", typeof(Texture2D)) as Texture2D;
                        break;
                    case ObjectType.O_ALGUES:
                        t = Resources.Load("EditorPlaceholders/05", typeof(Texture2D)) as Texture2D;
                        break;
                    case ObjectType.O_NUAGE:
                        t = Resources.Load("EditorPlaceholders/06", typeof(Texture2D)) as Texture2D;
                        break;
                    default:
                        t = Resources.Load("EditorPlaceholders/07", typeof(Texture2D)) as Texture2D;
                        break;
                }
                spawner.GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.one / 2.0f);
                spawner.GetComponent<SpawnerHelper>().init(futurePosition, newObjectType);
                if (newObjectType == ObjectType.O_OBSTRUCTION) spawner.GetComponent<SpawnerHelper>().ifObstructable = curObstruction;
                curObjectList.Add(spawner);
            }
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Sequence"))
            {
                string outPath = EditorUtility.SaveFilePanel("Select output file", Application.dataPath, "sequence.asset", "asset");
                if (outPath == string.Empty || !outPath.StartsWith(Application.dataPath))
                    return;

                EditorUtility.DisplayProgressBar("Progress", "Saving sequence...", 0);
                string destPath = "Assets" + outPath.Substring(Application.dataPath.Length);




                GameSequence gs = CreateInstance<GameSequence>();
                gs.nbScreens = nbScreens;

                for (int i = 0; i < curSequenceObject.transform.childCount; i++)
                {
                    Transform t = curSequenceObject.transform.GetChild(i);
                    EditorUtility.DisplayProgressBar("Progress", "Saving sequence...", i / curSequenceObject.transform.childCount);
                    SpawnerHelper sh = t.GetComponent<SpawnerHelper>();
                    if (sh != null)
                    {

                        ObjectSpawner os = new ObjectSpawner(t.position, sh.type);
                        os.ifObstructable = sh.ifObstructable;
                        gs.Spawners.Add(os);
                    }
                }
                EditorUtility.DisplayProgressBar("Progress", "Saving sequence...", 1);
                EditorUtility.ClearProgressBar();

                AssetDatabase.CreateAsset(gs, destPath);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
            }
                
                if (GUILayout.Button("Delete sequence"))
                {

                    if (EditorUtility.DisplayDialog("Delete sequence?", "Do you really want to delete this sequence? ", "Delete", "Cancel"))
                    {
                        DestroyImmediate(curSequenceObject);
                        curSequenceObject = null;
                    }
                }

                GUILayout.EndHorizontal();
                EditorGUI.EndDisabledGroup();
            }
        }
    }
