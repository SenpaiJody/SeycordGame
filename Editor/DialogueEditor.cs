using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEditor.Search;

public class DialogueEditor : EditorWindow
{
    public string currentAssetName;
    public Dialogue currentAsset;
    [SerializeField] public UnityEvent<Dialogue> OnDialogueStart;
    [SerializeField] public UnityEvent<Dialogue> OnDialogueFinish;


    public DialoguePage currentPage;

    int pageNum;

    bool dialogueFoldout = false;
    bool pageFoldout = false;
    bool characterFoldout = false;

    string nextButtonText = "Next";

    [MenuItem("Window/DialogueEditor")]
    public static void ShowWindow()
    {
        GetWindow<DialogueEditor>();
    }

    private void OnGUI()
    {
        
        currentAsset = EditorGUILayout.ObjectField("Dialogue to Load", currentAsset, typeof(Dialogue), false) as Dialogue;
        if (GUILayout.Button("Load"))
        {
            if (currentAsset != null)
            {
                ImportDialogueAsset(currentAsset.name);
                LoadPage(pageNum);
            }
            
        }
        GUILayout.Space(10) ;

        dialogueFoldout = EditorGUILayout.Foldout(dialogueFoldout, "Dialogue Events", true);
        if (dialogueFoldout)
        {
            GUILayout.BeginHorizontal();
            SerializedObject s = new SerializedObject(currentAsset);
            EditorGUILayout.PropertyField((s.FindProperty("OnDialogueStart")));
            EditorGUILayout.PropertyField(s.FindProperty("OnDialogueFinish"));
            s.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(10);

        if (currentAsset.pages.Count == 0)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("This dialogue has no pages.");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add a Page", GUILayout.ExpandWidth(false)))
            {
                AddPage();
                GUI.FocusControl(null);
            };
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Previous", GUILayout.ExpandWidth(false))){
            if (pageNum > 0)
            {
                LoadPage(pageNum - 1);
            }
            GUI.FocusControl(null);
        };
        EditorGUIUtility.labelWidth = 40;
        GUILayout.Label(string.Format("Page {0}/{1}", pageNum+1, currentAsset.pages.Count));
        EditorGUIUtility.labelWidth = 0;
        if (GUILayout.Button(nextButtonText, GUILayout.ExpandWidth(false)))
        {
            if (pageNum < currentAsset.pages.Count-1)
            {
                LoadPage(pageNum + 1);
            }
            else if (pageNum == currentAsset.pages.Count-1)
            {
                AddPage();
            }
            GUI.FocusControl(null);
        };
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
        {
            DeletePage(pageNum);
            LoadPage(pageNum < currentAsset.pages.Count - 1 ? pageNum+1 : pageNum-1);
            GUI.FocusControl(null);
        };
        GUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = 60;
        currentPage.FontSize = EditorGUILayout.IntField("Font Size", currentPage.FontSize,GUILayout.ExpandWidth(false));
        currentPage.content = EditorGUILayout.TextField("Content",currentPage.content,GUILayout.MinHeight(50));
        EditorGUIUtility.labelWidth = 0;

        GUILayout.Space(10);

        characterFoldout = EditorGUILayout.Foldout(characterFoldout, "Characters", true);
        if (characterFoldout)
        {
            //TODO: begin rendering character stuff
        }


        GUILayout.Space(10);
        pageFoldout = EditorGUILayout.Foldout(pageFoldout, "Page Events", true);
        if (pageFoldout)
        {
            GUILayout.BeginHorizontal();
            SerializedObject s = new SerializedObject(currentAsset);
            SerializedProperty p = s.FindProperty("pages").GetArrayElementAtIndex(pageNum);
            EditorGUILayout.PropertyField(p.FindPropertyRelative("OnPageStart"));
            EditorGUILayout.PropertyField(p.FindPropertyRelative("OnPageFinish"));
            s.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }
    }


    void ImportDialogueAsset(string name)
    {
        currentAssetName = currentAsset.name;
        OnDialogueStart = currentAsset.OnDialogueStart;
        OnDialogueFinish = currentAsset.OnDialogueFinish;
        pageNum = 1;
    }

    void LoadPage(int i)
    {
        if (i > currentAsset.pages.Count-1 || currentAsset.pages.Count == 0)
            return;

        currentPage = currentAsset.pages[i];
        pageNum = i;

        nextButtonText = (pageNum == currentAsset.pages.Count - 1) ? "Add" : "Next";

        
    }

    void AddPage()
    {
        currentAsset.pages.Add(new DialoguePage());
        Debug.Log("added");
        LoadPage(currentAsset.pages.Count-1);
    }

    void DeletePage(int i)
    {
        currentAsset.pages.RemoveAt(i);
    }
}
